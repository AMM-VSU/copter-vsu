using Scada.Comm.KP.AeroQuad;
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
            CanShowProps = true;
            DefaultReqParams = new KPLogic.ReqParams(false) { Timeout = 1000, Delay = 0 };
        }

        public override string KPDescr
        {
            get
            {
                return "Взаимодействие с полётным контроллером AeroQuad";
            }
        }

        public override void ShowProps()
        {
            (new FrmControl()).ShowDialog();
        }
    }
}
