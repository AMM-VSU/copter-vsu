namespace Scada.Comm.KP
{
    public class KpAeroQuadView : KPView
    {
        public KpAeroQuadView()
            : this(0)
        {
        }

        public KpAeroQuadView(int number)
            : base(number)
        {
            DefaultReqParams = new KPLogic.ReqParams(false) { Timeout = 1000, Delay = 0 };
        }

        public override string KPDescr
        {
            get
            {
                return "Взаимодействие с полётным контроллером AeroQuad";
            }
        }
    }
}
