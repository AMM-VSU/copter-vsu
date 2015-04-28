using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;

namespace Scada.Comm.KP
{
    public class KpAeroQuadLogic : KPLogic
    {
        // Разделитель данных в ответе контроллера
        private static readonly char[] Separator = { ',' }; 
        // Формат получаемых чисел
        private static readonly NumberFormatInfo NumFormat = CultureInfo.GetCultureInfo("en-GB").NumberFormat;

        private const int InBufSize = 100; // размер буфера входных данных
        private byte[] inBuf;              // буфер входных данных
        private int inBufLen;              // используемая длина буфера входных данных
        private FileStream stream;         // файловый поток


        public KpAeroQuadLogic(int number)
            : base(number)
        {
            // инициализация полей
            inBuf = new byte[InBufSize];
            inBufLen = 0;
            stream = null;

            // инициализация тегов КП
            InitArrays(11, 0);
            KPParams[0] = new Param(1, "Связь");
            KPParams[1] = new Param(2, "Принятых сообщений");
            KPParams[2] = new Param(3, "Roll Gyro Rate");
            KPParams[3] = new Param(4, "Pitch Gyro Rate");
            KPParams[4] = new Param(5, "Yaw Gyro Rate");
            KPParams[5] = new Param(6, "Accel X Axis");
            KPParams[6] = new Param(7, "Accel Y Axis");
            KPParams[7] = new Param(8, "Accel Z Axis");
            KPParams[8] = new Param(9, "Mag Raw Value X Axis");
            KPParams[9] = new Param(10, "Mag Raw Value Y Axis");
            KPParams[10] = new Param(11, "Mag Raw Value Z Axis");

            // установка признака возможности отправки команд
            CanSendCmd = true;
        }


        /// <summary>
        /// Расшифровать и обработать полученные данные
        /// </summary>
        private bool DecodeResponse()
        {
            bool decodeOK = true;

            try
            {
                int paramIndex = 2; // индекс устанавливаемого тега

                if (inBufLen > 0)
                {
                    string line = Encoding.Default.GetString(inBuf, 0, inBufLen).TrimEnd();
                    string[] parts = line.Split(Separator, StringSplitOptions.RemoveEmptyEntries);

                    if (parts.Length == 9)
                    {
                        FileFormats.F01Record rec = new FileFormats.F01Record();
                        rec.Time = DateTime.Now.ToBinary();

                        for (int i = 0; i < 9; i++)
                        {
                            try
                            {
                                double val = double.Parse(parts[i], NumFormat);
                                SetParamData(paramIndex, val, 1);
                                rec.SetFieldByIndex(i, val);
                                paramIndex++;
                            } 
                            catch (FormatException ex)
                            {
                                throw new FormatException(
                                    "Невозможно преобразовать строку \"" + parts[i] + "\" в число.", ex);
                            }
                        }

                        // определение успешности обработки данных
                        decodeOK = paramIndex == KPParams.Length;

                        // запись в файл
                        if (decodeOK)
                            SaveRecord(rec);
                    }

                    // установка тегов
                    if (decodeOK)
                    {
                        SetParamData(0, 1.0, 1);                // связь есть
                        SetParamData(1, CurData[1].Val + 1, 1); // увеличение счётчика сообщений
                    }
                    else
                    {
                        SetParamData(0, 0.0, 1); // связи нет
                    }
                }
                else
                {
                    decodeOK = true;
                    SetParamData(0, 0.0, 1); // связи нет
                }

                // установка недостоверности для непринятых тегов
                for (int i = paramIndex; i < KPParams.Length; i++)
                    SetParamData(i, 0.0, 0);
            }
            catch (Exception ex)
            {
                WriteToLog("Ошибка при обработке данных: " + ex.Message);
            }

            return decodeOK;
        }

        /// <summary>
        /// Сохранить запись в файле
        /// </summary>
        private void SaveRecord(FileFormats.F01Record rec)
        {
            try
            {
                if (stream == null)
                {
                    string dir = LogDir + "AeroQuad\\";
                    Directory.CreateDirectory(dir);
                    string path = dir + "telemetry_" + DateTime.Now.ToString("yyyy'-'MM'-'dd'_'HH'-'mm'-'ss") + ".f01";
                    stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read);
                }

                byte[] bytes = rec.GetBytes();
                stream.Write(bytes, 0, bytes.Length);
                stream.Flush(true);
            }
            catch (Exception ex)
            {
                WriteToLog("Ошибка при записи в файл: " + ex.Message);
            }
        }


        public override void OnCommLineTerminate()
        {
            base.OnCommLineTerminate();

            if (stream != null)
                stream.Close();
        }

        public override void Session()
        {
            base.Session();
            lastCommSucc = false;

            // отправка данных команды
            string logText;
            KPUtils.WriteToSerialPort(SerialPort, new byte[] { 0x69, 0x0D, 0x0A }, 0, 3,
                KPUtils.SerialLogFormat.String, out logText);
            WriteToLog(logText);

            // чтение данных пока не будет получен конец строки, заполнен буфер или превышен таймаут
            //string logText;
            inBufLen = KPUtils.ReadFromSerialPort(SerialPort, inBuf, 0, InBufSize, 0x0A, KPReqParams.Timeout, 
                false, KPUtils.SerialLogFormat.String, out logText);
            WriteToLog(logText);

            // обработка полученных данных
            if (SerialPort != null && DecodeResponse())
                lastCommSucc = true;

            // завершение запроса и расчёт статистики
            FinishRequest();
            CalcSessStats();
        }

        public override void SendCmd(KPLogic.Command cmd)
        {
            base.SendCmd(cmd);
            lastCommSucc = false;

            if (cmd.CmdNum == 1 && cmd.CmdType == CmdType.Binary)
            {
                if (cmd.CmdData != null && cmd.CmdData.Length > 0)
                {
                    // отправка данных команды
                    string logText;
                    KPUtils.WriteToSerialPort(SerialPort, cmd.CmdData, 0, cmd.CmdData.Length, 
                        KPUtils.SerialLogFormat.String, out logText);
                    WriteToLog(logText);
                    lastCommSucc = true;
                    Thread.Sleep(KPReqParams.Delay);
                }
                else
                {
                    WriteToLog(KPUtils.NoCommandData);
                }
            }
            else
            {
                WriteToLog(KPUtils.IllegalCommand);
            }

            // расчёт статистики
            CalcCmdStats();
        }

        public override string ParamDataToStr(int signal, ParamData paramData)
        {
            if (paramData.Stat > 0)
            {
                if (signal == 1)
                    return paramData.Val > 0 ? "Есть" : "Нет";
                else if (signal <= 2)
                    return paramData.Val.ToString("N0");
            }

            return base.ParamDataToStr(signal, paramData);
        }
    }
}
