using Scada.Client;
using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Web.Script.Serialization;

namespace Scada.Web.Plugins.AeroQuad
{
    /// <summary>
    /// WCF-сервис для получения текущих данных фиксированного списка входных каналов
    /// </summary>
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class AeroQuadSvc
    {
        /// <summary>
        /// Номера входных каналов, данные которых передаются на веб-форму
        /// </summary>
        private static readonly int[] CnlNums = 
            { 1001, 1002, 1003, 1004, 1005, 1006, 1007, 1008, 1009, 1010, 1011, 1012, 1013, 1014 };
        /// <summary>
        /// Количество входных каналов
        /// </summary>
        private static int CnlCnt = CnlNums.Length;
        /// <summary>
        /// Номер канала управления для отправки данных коптеру
        /// </summary>
        private const int SendDataOutCnlNum = 1001;
        /// <summary>
        /// Номер канала управления для включения и отключения записи телеметрии на диск
        /// </summary>
        private const int RecordOutCnlNum = 1002;
        /// <summary>
        /// Ид. пользователя, от имени которого отправляются команды, admin
        /// </summary>
        private const int UserID = 11;


        // To use HTTP GET, add [WebGet] attribute. (Default ResponseFormat is WebMessageFormat.Json)
        // To create an operation that returns XML,
        //     add [WebGet(ResponseFormat=WebMessageFormat.Xml)],
        //     and include the following line in the operation body:
        //         WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml";
        [OperationContract]
        [WebGet]
        public string GetCurData()
        {
            try
            {
                // получение текущих данных
                string[] curDataArr = new string[CnlCnt];
                AppData.MainData.RefreshData();

                for (int i = 0; i < CnlCnt; i++)
                {
                    string color;
                    curDataArr[i] = AppData.MainData.GetCnlVal(CnlNums[i], false, out color);
                }

                // форматирование результата в JSON
                JavaScriptSerializer js = new JavaScriptSerializer();
                return js.Serialize(curDataArr);
            }
            catch (Exception ex)
            {
                AppData.Log.WriteException(ex, "Error in AeroQuadSvc");
                return "";
            }
        }

        [OperationContract]
        [WebGet]
        public void StartReading()
        {
            bool result;
            AppData.MainData.ServerComm.SendBinaryCommand(UserID, RecordOutCnlNum, new byte[] { 0x69 }, out result);
        }

        [OperationContract]
        [WebGet]
        public void RecordOn()
        {
            bool result;
            AppData.MainData.ServerComm.SendStandardCommand(UserID, RecordOutCnlNum, 1.0, out result);
        }

        [OperationContract]
        [WebGet]
        public void RecordOff()
        {
            bool result;
            AppData.MainData.ServerComm.SendStandardCommand(UserID, RecordOutCnlNum, 0.0, out result);
        }
    }
}
