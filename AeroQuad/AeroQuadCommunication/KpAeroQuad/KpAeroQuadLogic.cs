using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Scada.Comm.KP
{
    public class KpAeroQuadLogic : KPLogic
    {
        private const int InBufSize = 100; // размер буфера входных данных
        private byte[] inBuf;              // буфер входных данных


        public KpAeroQuadLogic(int number)
            : base(number)
        {
            inBuf = new byte[InBufSize]; 

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
        }

        public override void Session()
        {
            base.Session();

            // проверка доступности последовательного порта
            if (SerialPort == null)
                lastCommSucc = false;

            // чтение данных пока не будет получен конец строки, заполнен буфер или превышен таймаут
            string logText;
            KPUtils.ReadFromSerialPort(SerialPort, inBuf, 0, InBufSize, 0x0D, KPReqParams.Timeout, 
                false, KPUtils.SerialLogFormat.String, out logText);
            WriteToLog(logText);

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
    }
}
