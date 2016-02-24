using Scada.Comm.Channels;
using Scada.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;

namespace Scada.Comm.Devices
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
        // Условие остановки чтения данных
        private static readonly Connection.BinStopCondition StopCond = new Connection.BinStopCondition(0x0A);

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
            InitKPTags(new List<KPTag>()
            {
                new KPTag(1, "Connection"),
                new KPTag(2, "Record"),
                new KPTag(3, "Received messages"),
                new KPTag(4, "Failed messages"),
                new KPTag(5, "Messages per second"),
                new KPTag(6, "Roll Gyro Rate"),
                new KPTag(7, "Pitch Gyro Rate"),
                new KPTag(8, "Yaw Gyro Rate"),
                new KPTag(9, "Accel X Axis"),
                new KPTag(10, "Accel Y Axis"),
                new KPTag(11, "Accel Z Axis"),
                new KPTag(12, "Mag Raw Value X Axis"),
                new KPTag(13, "Mag Raw Value Y Axis"),
                new KPTag(14, "Mag Raw Value Z Axis")
            });

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
                                SetCurData(paramIndex, val, 1);
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
                            decodeOK = paramIndex == KPTags.Length;

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
                SetCurData(CONNECT_SIGNAL - 1, decodeOK ? 1.0 : 0.0, 1);

                // установка тега количества сообщений в секунду
                if (recordOn)
                {
                    int curSec = DateTime.Now.Second;
                    if (startSec != curSec)
                    {
                        SetCurData(MSG_PER_SEC_SIGNAL - 1, msgPerSec, 1);
                        startSec = curSec;
                        msgPerSec = 0;
                    }
                }

                // установка недостоверности для непринятых тегов
                for (int i = paramIndex; i < KPTags.Length; i++)
                    SetCurData(i, 0.0, 0);
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
                    string dir = AppDirs.LogDir + "AeroQuad" + Path.DirectorySeparatorChar;
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
            SetCurData(RECORD_SIGNAL - 1, 1.0, 1);
            SetCurData(RECEIVED_MSG_SIGNAL - 1, 0.0, 1);
            SetCurData(FAILED_MSG_SIGNAL - 1, 0.0, 1);
            SetCurData(MSG_PER_SEC_SIGNAL - 1, 0.0, 1);

            recordOn = true;
            startSec = DateTime.Now.Second;
            msgPerSec = 0;
        }

        /// <summary>
        /// Отключить запись телеметрии
        /// </summary>
        private void SetRecordOff()
        {
            SetCurData(RECORD_SIGNAL - 1, 0.0, 1);
            SetCurData(RECEIVED_MSG_SIGNAL - 1, 0.0, 0);
            SetCurData(FAILED_MSG_SIGNAL - 1, 0.0, 0);
            SetCurData(MSG_PER_SEC_SIGNAL - 1, 0.0, 0);

            recordOn = false;
            CloseTelemetryStream();
        }

        /// <summary>
        /// Увеличить значение тега счётчика
        /// </summary>
        private void IncCounter(int signal)
        {
            int paramIndex = signal - 1;
            SetCurData(paramIndex, curData[paramIndex].Val + 1, 1);
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


        protected override string ConvertTagDataToStr(int signal, SrezTableLight.CnlData tagData)
        {
            if (tagData.Stat > 0)
            {
                if (signal == CONNECT_SIGNAL)
                    return tagData.Val > 0 ? "Yes" : "No";
                else if (signal == RECORD_SIGNAL)
                    return tagData.Val > 0 ? "On" : "Off";
                else if (signal <= FAILED_MSG_SIGNAL && signal < TELEMETRY_START_SIGNAL)
                    return tagData.Val.ToString("N0");
            }

            return base.ConvertTagDataToStr(signal, tagData);
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
            bool stopReceived;
            string logText;
            inBufLen = Connection.Read(inBuf, 0, InBufSize, ReqParams.Timeout, StopCond, 
                out stopReceived, CommUtils.ProtocolLogFormats.String, out logText);
            WriteToLog(logText);

            // обработка полученных данных
            if (Connection.Connected && DecodeResponse())
                lastCommSucc = true;

            // завершение запроса и расчёт статистики
            FinishRequest();
            CalcSessStats();
        }

        public override void SendCmd(Command cmd)
        {
            base.SendCmd(cmd);
            lastCommSucc = false;

            if (cmd.CmdNum == 1 && cmd.CmdTypeID == BaseValues.CmdTypes.Binary)
            {
                // команда отправки данных контроллеру
                if (cmd.CmdData != null && cmd.CmdData.Length > 0)
                {
                    // отправка данных команды
                    string logText;
                    Connection.Write(cmd.CmdData, 0, cmd.CmdData.Length, 
                        CommUtils.ProtocolLogFormats.String, out logText);
                    WriteToLog(logText);
                    lastCommSucc = true;
                    Thread.Sleep(ReqParams.Delay);
                }
                else
                {
                    WriteToLog(CommPhrases.NoCmdData);
                }
            }
            else if (cmd.CmdNum == 2 && cmd.CmdTypeID == BaseValues.CmdTypes.Standard)
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
                WriteToLog(CommPhrases.IllegalCommand);
            }

            // расчёт статистики
            CalcCmdStats();
        }
    }
}
