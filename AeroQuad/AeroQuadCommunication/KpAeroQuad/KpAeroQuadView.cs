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
                return "Взаимодействие с полётным контроллером AeroQuad.\n\n" +
                    "Команды ТУ:\n" +
                    "1 (бинарная) - передача данных контроллеру,\n" +
                    "2 (стандартная) - включение или отключение записи телеметрии.";
            }
        }

        public override void ShowProps()
        {
            FrmControl.ShowDialog(Number, CmdDir);
        }
    }
}
