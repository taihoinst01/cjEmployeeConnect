using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using com.miracom.transceiverx;
using com.miracom.transceiverx.session;
using com.miracom.transceiverx.message;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using System.IO;

namespace SampleDemo.SAP
{
    public class TestEaiCall
    {
        //static void Main(string[] args)
        public String call(string userId, string sabun, string reissue)
        {
            try
            {
                H101EaiCall eai = new H101EaiCall();

                // 채널시스템 구분
                String systemID = "CJ_HelpDesk";
                // 인터페이스ID ==> 연계 업무마다 정의됨
                String interfaceID = "MxZSAP_PASSCHANGE_EKP";
                // EAI호출프로그램명 ==> 구분가능한 프로그램명
                String sessionName = "CJ_CHATBOT";

                //String data = "<?xml version='1.0' encoding='UTF-8'?>";
                //data = data + "<EAI_REQUEST>";
                //data = data + "<INTERFACE_INFO>";
                //data = data + "<REQUEST_SYSTEM>CJ_HelpDesk</REQUEST_SYSTEM>";
                //data = data + "<UUID>CJ_CHATBOT_" + System.DateTime.Now.ToString("yyyyMMddhhmmssff") + "</UUID>";
                //data = data + "<OPTIONAL_1>CJ_SAP</OPTIONAL_1>";
                //data = data + "<OPTIONAL_2>CHATBOT</OPTIONAL_2>";
                //data = data + "<OPTIONAL_3></OPTIONAL_3>";
                //data = data + "<OPTIONAL_4></OPTIONAL_4>";
                //data = data + "</INTERFACE_INFO>";
                //data = data + "<INPUT_DATA>";
                //data = data + "<P_ID>chatbot01</P_ID>";
                //data = data + "<P_NUMxP_PERNR>123456</P_NUMxP_PERNR>";
                //data = data + "<P_TEXT>ABCDEFGHI</P_TEXT>";
                //data = data + "</INPUT_DATA>";
                //data = data + "</EAI_REQUEST>";

                //전사 ERP(PRD) -> CJ_SAP(개발서버 cjerpdev / 52.2.199.15)
                //전사 BI(BIP) -> CJ_BI(개발서버 cjbidev / 52.2.199.6)
                //해외 BI(BW1) -> CJG_BI(개발서버 globwdev / 52.2.199.23)

                String data = " { ";
                //data = data + "   'EAI_REQUEST': { ";
                data = data + "     \"INTERFACE_INFO\":  ";
                //data = data + "       \"REQUEST_SYSTEM\": \"CJ_CHATBOT\", ";
                data = data + "       { ";
                data = data + "       \"REQUEST_SYSTEM\": \"CJ_HelpDesk\", ";
                data = data + "       \"UUID\": \"CJ_CHATBOT_" + System.DateTime.Now.ToString("yyyyMMddhhmmssff") + "\", ";
                //data = data + "       \"OPTIONAL_1\": \"CJ_SAP / CJ_BI / CJG_BI (응답시스템에 따라 선택)\", ";
                data = data + "       \"OPTIONAL_1\": \"CJ_SAP\", ";
                data = data + "       \"OPTIONAL_2\": \"CHATBOT\", ";
                data = data + "       \"OPTIONAL_3\": \"\", ";
                data = data + "       \"OPTIONAL_4\": \"\" ";
                data = data + "     } ";
                data = data + "     , ";
                data = data + "     \"INPUT_DATA\":  ";
                data = data + "     { ";
                data = data + "       \"P_ID\": \"chatbot01\",  ";
                data = data + "       \"P_NUMxP_PERNR\": \"123456\",";
                data = data + "       \"P_TEXT\": \"ABCDEFGHI\" ";
                data = data + "     } ";
                data = data + "    ";
                //data = data + "   } ";
                data = data + " } ";
                 
                Debug.WriteLine("------START Send Request Message...\n" + data + "------END Send Request Message...\n");
                HistoryLog("Data = " + data);
                String repData = eai.sendMessage(sessionName, systemID, interfaceID, data, 60000);
                HistoryLog("repData = " + repData);
                //JObject sapJson = new JObject();
                //String repData = "bbb";

                //Console.WriteLine("------Reply Message...\n" + repData);
                return repData;
            }
            catch (TrxException ex)
            {
                Debug.WriteLine("error = " + ex.ToString());
                HistoryLog("error = " + ex.ToString());
                return "";
            }
        }

        public static void HistoryLog(String strMsg)
        {
            try
            {
                //Debug.WriteLine("AppDomain.CurrentDomain.BaseDirectory : " + AppDomain.CurrentDomain.BaseDirectory);
                string m_strLogPrefix = AppDomain.CurrentDomain.BaseDirectory + @"LOG\";
                string m_strLogExt = @".LOG";
                DateTime dtNow = DateTime.Now;
                string strDate = dtNow.ToString("yyyy-MM-dd");
                string strPath = String.Format("{0}{1}{2}", m_strLogPrefix, strDate, m_strLogExt);
                string strDir = Path.GetDirectoryName(strPath);
                DirectoryInfo diDir = new DirectoryInfo(strDir);

                if (!diDir.Exists)
                {
                    diDir.Create();
                    diDir = new DirectoryInfo(strDir);
                }

                if (diDir.Exists)
                {
                    System.IO.StreamWriter swStream = File.AppendText(strPath);
                    string strLog = String.Format("{0}: {1}", dtNow.ToString("MM/dd/yyyy hh:mm:ss.fff"), strMsg);
                    swStream.WriteLine(strLog);
                    swStream.Close(); ;
                }
            }
            catch (System.Exception e)
            {
                HistoryLog(e.Message);
            }
        }
    }
}
