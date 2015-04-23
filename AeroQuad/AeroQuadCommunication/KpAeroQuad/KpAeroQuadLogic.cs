using System;
using System.Collections.Generic;
using System.Text;

namespace Scada.Comm.KP
{
    public class KpAeroQuadLogic : KPLogic
    {
        public KpAeroQuadLogic(int number)
            : base(number)
        {
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
        }
    }
}
