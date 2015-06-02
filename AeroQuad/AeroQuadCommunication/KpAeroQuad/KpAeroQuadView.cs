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
                return "Interacting with AeroQuad filght controllers.\n\n" +
                    "Commands:\n" +
                    "1 (Binary) - send data to controller,\n" +
                    "2 (Standard) - turn telemetry recording on or off.";
            }
        }

        public override void ShowProps()
        {
            FrmControl.ShowDialog(Number, CmdDir);
        }
    }
}
