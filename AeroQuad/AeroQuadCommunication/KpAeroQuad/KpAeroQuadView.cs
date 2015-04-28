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
            DefaultReqParams = new KPLogic.ReqParams(false) { Timeout = 5000, Delay = 200 };
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
