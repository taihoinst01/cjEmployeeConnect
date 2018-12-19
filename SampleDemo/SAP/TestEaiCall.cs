using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using com.miracom.transceiverx;
using com.miracom.transceiverx.session;
using com.miracom.transceiverx.message;
using System.Diagnostics;
using Newtonsoft.Json.Linq;

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

                // ä�νý��� ����
                String systemID = "CJ_Feed_CJONE";
                // �������̽�ID ==> ���� �������� ���ǵ�
                String interfaceID = "MxZ_SD_FEEDPRM_CREDIT";
                // EAIȣ�����α׷��� ==> ���а����� ���α׷���
                String sessionName = "APP_PRG_NAME";

                //String data = "<?xml version='1.0' encoding='UTF-8'?>";
                //data = data + "<EAI_REQUEST>";
                //data = data + "<INTERFACE_INFO>";
                //data = data + "<REQUEST_SYSTEM>CJ_Feed_CJONE</REQUEST_SYSTEM>";
                //data = data + "<UUID>CJ_Feed_CJONE_20160226172910785</UUID>";
                //data = data + "<OPTIONAL_1/>";
                //data = data + "<OPTIONAL_2/>";
                //data = data + "<OPTIONAL_3/>";
                //data = data + "<OPTIONAL_4/>";
                //data = data + "</INTERFACE_INFO>";
                //data = data + "<INPUT_DATA>";
                //data = data + "<I_KUNNR>0000200873</I_KUNNR>";
                //data = data + "</INPUT_DATA>";
                //data = data + "</EAI_REQUEST>";

                String data = " { ";
                data = data + "   'EAI_REQUEST': { ";
                data = data + "     'INTERFACE_INFO': { ";
                data = data + "       'REQUEST_SYSTEM': 'CJ_CHATBOT', ";
                data = data + "       'UUID': 'CJ_CHATBOT_"+ System.DateTime.Now.ToString("yyyyMMddhhmmssff") + "', ";
                //data = data + "       'OPTIONAL_1': 'CJ_SAP / CJ_BI / CJG_BI (����ý��ۿ� ���� ����)', ";
                data = data + "       'OPTIONAL_1': 'CJ_SAP', ";
                data = data + "       'OPTIONAL_2': 'CHATBOT' ";
                data = data + "       'OPTIONAL_3': '' ";
                data = data + "       'OPTIONAL_4': '' ";
                data = data + "     }, ";
                data = data + "     'INPUT_DATA': { ";
                data = data + "       'P_ID': 'CJ����ID',  ";
                data = data + "       'P_NUMxP_PERNR': '���',";
                data = data + "       'P_TEXT': '��߱޻���(5�����̻�)' ";
                data = data + "     } ";
                data = data + "   }";
                data = data + " } ";

                Debug.WriteLine("------START Send Request Message...\n" + data + "------END Send Request Message...\n");

                String repData = eai.sendMessage(sessionName, systemID, interfaceID, data, 5000);
                //JObject sapJson = new JObject();

                //String repData = "bbb";

                //Console.WriteLine("------Reply Message...\n" + repData);
                return repData;
            }
            catch (TrxException ex)
            {
                Debug.WriteLine(ex.ToString());
                return "";
            }
        }
    }
}
