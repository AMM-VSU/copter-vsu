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
            InitArrays(2, 0);
            KPParams[0] = new Param(1, "Связь");
            KPParams[1] = new Param(2, "Принятых сообщений");
        }

        public override void Session()
        {
            base.Session();
        }
    }
}
