using System;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace Scada.Comm.KP
{
    public class KpAeroQuadLogic : KPLogic
    {
        // Номера сигналов тегов
        private const int CONNECT_SIGNAL = 1;
        private const int RECORD_SIGNAL = 2;
        private const int RECEIVED_MSG_SIGNAL = 3;
        private const int FAILED_MSG_SIGNAL = 4;
        private const int MSG_PER_SEC_SIGNAL = 5;
        private const int TELEMETRY_START_SIGNAL = 6;

        // Разделитель данных в ответе контроллера
        private static readonly char[] Separator = { ',' }; 
        // Формат получаемых чисел
        private static readonly NumberFormatInfo NumFormat = CultureInfo.GetCultureInfo("en-GB").NumberFormat;

        private const int InBufSize = 100;  // размер буфера входных данных
        private byte[] inBuf;               // буфер входных данных
        private int inBufLen;               // используемая длина буфера входных данных
        private int startSec;               // стартовая секунда для расчёта количества сообщений
        private int msgPerSec;              // счётчик количества сообщений в секунду
        private FileStream telemetryStream; // поток для записи телеметрии
        private bool recordOn;              // запись данных телеметрии включена


        public KpAeroQuadLogic(int number)
            : base(number)
        {
            // инициализация полей
            inBuf = new byte[InBufSize];
            inBufLen = 0;
            startSec = DateTime.Now.Second;
            msgPerSec = 0;
            telemetryStream = null;
            recordOn = false;

            // инициализация тегов КП
            InitArrays(14, 0);
            KPParams[0] = new Param(1, "Connection");
            KPParams[1] = new Param(2, "Record");
            KPParams[2] = new Param(3, "Received messages");
            KPParams[3] = new Param(4, "Failed messages");
            KPParams[4] = new Param(5, "Messages per second");
            KPParams[5] = new Param(6, "Roll Gyro Rate");
            KPParams[6] = new Param(7, "Pitch Gyro Rate");
            KPParams[7] = new Param(8, "Yaw Gyro Rate");
            KPParams[8] = new Param(9, "Accel X Axis");
            KPParams[9] = new Param(10, "Accel Y Axis");
            KPParams[10] = new Param(11, "Accel Z Axis");
            KPParams[11] = new Param(12, "Mag Raw Value X Axis");
            KPParams[12] = new Param(13, "Mag Raw Value Y Axis");
            KPParams[13] = new Param(14, "Mag Raw Value Z Axis");

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
                int paramIndex = TELEMETRY_START_SIGNAL - 1; // индекс устанавливаемого тега

                if (inBufLen > 0)
                {
                    // распознавание ответа
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
                                    "Unable to convert string \"" + parts[i] + "\" to number.", ex);
                            }
                        }

                        if (recordOn)
                        {
                            // определение успешности обработки данных
                            decodeOK = paramIndex == KPParams.Length;

                            if (decodeOK)
                            {
                                // увеличение счётчика принятых сообщений
                                IncCounter(RECEIVED_MSG_SIGNAL);
                                // увеличение счётчика сообщений в секунду
                                msgPerSec++;
                                // запись телеметрии в файл
                                SaveRecord(rec); 
                            }
                            else
                            {
                                // увеличение счётчика повреждённых сообщений
                                IncCounter(FAILED_MSG_SIGNAL);
                            }
                        }
                    }
                }

                // установка тега наличия связи
                SetParamData(CONNECT_SIGNAL - 1, decodeOK ? 1.0 : 0.0, 1);

                // установка тега количества сообщений в секунду
                if (recordOn)
                {
                    int curSec = DateTime.Now.Second;
                    if (startSec != curSec)
                    {
                        SetParamData(MSG_PER_SEC_SIGNAL - 1, msgPerSec, 1);
                        startSec = curSec;
                        msgPerSec = 0;
                    }
                }

                // установка недостоверности для непринятых тегов
                for (int i = paramIndex; i < KPParams.Length; i++)
                    SetParamData(i, 0.0, 0);
            }
            catch (Exception ex)
            {
                WriteToLog("Error decoding response: " + ex.Message);
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
                if (telemetryStream == null)
                {
                    string dir = LogDir + "AeroQuad" + Path.DirectorySeparatorChar;
                    Directory.CreateDirectory(dir);
                    string path = dir + "telemetry_" + DateTime.Now.ToString("yyyy'-'MM'-'dd'_'HH'-'mm'-'ss") + ".f01";
                    telemetryStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read);
                }

                byte[] bytes = rec.GetBytes();
                telemetryStream.Write(bytes, 0, bytes.Length);
                telemetryStream.Flush(true);
            }
            catch (Exception ex)
            {
                WriteToLog("Error saving record to file: " + ex.Message);
            }
        }

        /// <summary>
        /// Включить запись телеметрии
        /// </summary>
        private void SetRecordOn()
        {
            SetParamData(RECORD_SIGNAL - 1, 1.0, 1);
            SetParamData(RECEIVED_MSG_SIGNAL - 1, 0.0, 1);
            SetParamData(FAILED_MSG_SIGNAL - 1, 0.0, 1);
            SetParamData(MSG_PER_SEC_SIGNAL - 1, 0.0, 1);

            recordOn = true;
            startSec = DateTime.Now.Second;
            msgPerSec = 0;
        }

        /// <summary>
        /// Отключить запись телеметрии
        /// </summary>
        private void SetRecordOff()
        {
            SetParamData(RECORD_SIGNAL - 1, 0.0, 1);
            SetParamData(RECEIVED_MSG_SIGNAL - 1, 0.0, 0);
            SetParamData(FAILED_MSG_SIGNAL - 1, 0.0, 0);
            SetParamData(MSG_PER_SEC_SIGNAL - 1, 0.0, 0);

            recordOn = false;
            CloseTelemetryStream();
        }

        /// <summary>
        /// Увеличить значение тега счётчика
        /// </summary>
        private void IncCounter(int signal)
        {
            int paramIndex = signal - 1;
            SetParamData(paramIndex, CurData[paramIndex].Val + 1, 1);
        }

        /// <summary>
        /// Закрыть поток для записи телеметрии
        /// </summary>
        private void CloseTelemetryStream()
        {
            if (telemetryStream != null)
            {
                telemetryStream.Close();
                telemetryStream = null;
            }
        }

        /// <summary>
        /// Считать данные из последовательного порта
        /// </summary>
        private static int ReadFromSerialPort(SerialPort serialPort, byte[] buffer, int index, int maxCount,
            byte stopCode, int timeout, bool wait, KPUtils.SerialLogFormat logFormat, out string logText)
        {
            int readCnt = 0;

            if (serialPort == null)
            {
                logText = KPUtils.ReadDataImpossible;
            }
            else
            {
                DateTime nowDT = DateTime.Now;
                DateTime startDT = nowDT;
                DateTime stopDT = startDT.AddMilliseconds(timeout);

                bool stop = false;
                int curInd = index;
                serialPort.ReadTimeout = 100;

                while (readCnt <= maxCount && !stop && startDT <= nowDT && nowDT <= stopDT)
                {
                    int read;
                    try { read = serialPort.Read(buffer, curInd, Math.Min(maxCount - readCnt, serialPort.BytesToRead)); }
                    catch { read = 0; }

                    if (read > 0)
                    {
                        for (int i = curInd, j = 0; j < read && !stop; i++, j++)
                            stop = buffer[i] == stopCode;
                        curInd += read;
                        readCnt += read;
                    }
                    else
                    {
                        Thread.Sleep(10); // накопление входных данных в буфере порта
                    }

                    nowDT = DateTime.Now;
                }

                logText = KPUtils.ReceiveNotation + " (" + readCnt + "): " + (logFormat == KPUtils.SerialLogFormat.Hex ?
                        KPUtils.BytesToHex(buffer, index, readCnt) : KPUtils.BytesToString(buffer, index, readCnt));

                if (wait && startDT <= nowDT)
                {
                    int delay = (int)(stopDT - nowDT).TotalMilliseconds;
                    if (delay > 0)
                        Thread.Sleep(delay);
                }
            }

            return readCnt;
        }

        /// <summary>
        /// Записать данные в последовательный порт
        /// </summary>
        private static void WriteToSerialPort(SerialPort serialPort, byte[] buffer, int index, int count,
            KPUtils.SerialLogFormat logFormat, out string logText)
        {
            try
            {
                if (serialPort == null)
                {
                    logText = KPUtils.WriteDataImpossible;
                }
                else
                {
                    serialPort.Write(buffer, index, count);
                    logText = KPUtils.SendNotation + " (" + count + "): " + (logFormat == KPUtils.SerialLogFormat.Hex ?
                        KPUtils.BytesToHex(buffer, index, count) : KPUtils.BytesToString(buffer, index, count));
                }
            }
            catch (Exception ex)
            {
                logText = "Error sending data: " + ex.Message;
            }
        }


        public override void OnCommLineStart()
        {
            SetRecordOff();
        }

        public override void OnCommLineTerminate()
        {
            CloseTelemetryStream();
        }

        public override void Session()
        {
            base.Session();
            lastCommSucc = false;

            // чтение данных пока не будет получен конец строки, заполнен буфер или превышен таймаут
            string logText;
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
                // команда отправки данных контроллеру
                if (cmd.CmdData != null && cmd.CmdData.Length > 0)
                {
                    // отправка данных команды
                    string logText;
                    WriteToSerialPort(SerialPort, cmd.CmdData, 0, cmd.CmdData.Length, 
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
            else if (cmd.CmdNum == 2 && cmd.CmdType == CmdType.Standard)
            {
                // команда включения или отключения записи
                if (cmd.CmdVal > 0)
                {
                    WriteToLog("Telemetry recording is turned ON");
                    SetRecordOn();
                }
                else
                {
                    WriteToLog("Telemetry recording is turned OFF");
                    SetRecordOff();
                }
                lastCommSucc = true;
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
                if (signal == CONNECT_SIGNAL)
                    return paramData.Val > 0 ? "Yes" : "No";
                else if (signal == RECORD_SIGNAL)
                    return paramData.Val > 0 ? "On" : "Off";
                else if (signal <= FAILED_MSG_SIGNAL && signal < TELEMETRY_START_SIGNAL)
                    return paramData.Val.ToString("N0");
            }

            return base.ParamDataToStr(signal, paramData);
        }
    }
}
