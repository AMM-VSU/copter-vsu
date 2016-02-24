using System;

namespace Scada.Web.Plugins.AeroQuad
{
    /// <summary>
    /// Веб-форма для отображения телеметрии AeroQuad
    /// </summary>
    public partial class WFrmTelemetry : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AppData.InitAppData();
        }
    }
}