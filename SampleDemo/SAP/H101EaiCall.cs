using System;
using System.Collections.Generic;
using System.Text;
using com.miracom.transceiverx;
using com.miracom.transceiverx.session;
using com.miracom.transceiverx.message;
using System.IO;

namespace SampleDemo.SAP
{
    class H101EaiCall : SessionEventListener
    {
        //private const String connectString = "52.2.198.170:10101";  // EAI DEV Server IP Address
        //private const String connectString = "scmbodev:10101";  // x
        private const String connectString = "scmbo:10101";  // x
        //private const String connectString = "scmbodev.cjad.net:10101";  // x
        //private const String connectString = "http://scmbodev.cjad.net:10101";  // x
        //private const String connectString = "scmbo_dev.cjad.net:10101";  // x
        //private const String connectString = "scmbo_dev:10101";  // x

        private Session ioiSession = null;
        private int sessionMode = Session_Fields.SESSION_INTER_STATION_MODE |
            Session_Fields.SESSION_PULL_DELIVERY_MODE;
        private short msgDeliveryType = DeliveryType.REQUEST;

        private void connect(String sessionName)
        {
            try
            {
                ioiSession = Transceiver.createSession(sessionName, sessionMode);
                HistoryLog("1111");
                ioiSession.addSessionEventListener(this);
                HistoryLog("2222");
                ioiSession.setAutoRecovery(true);
                HistoryLog("3333");
                ioiSession.setDefaultTTL(60000);
                HistoryLog("4444");
                ioiSession.connect(connectString);
                HistoryLog("5555");
            }
            catch (SessionException ex)
            {
                throw new TrxException(ex.ToString(), ex);
            }
        }

        public String sendMessage(String sessionName, String systemID, String interfaceID, String contents, long ttl)
        {
            String strRep = null;

            try
            {
                connect(sessionName);
                Message msg = ioiSession.createMessage();

                String msgChannel = "/" + systemID + "/" + interfaceID;
                msg.setChannel(msgChannel);
                msg.setDeliveryMode(this.msgDeliveryType);
                msg.setTTL(ttl);
                Encoding en = Encoding.UTF8;
                HistoryLog("contents=" + contents);
                msg.setData(en.GetBytes(contents));
                HistoryLog("msg=" + msg);
                Message reply = ioiSession.sendRequest(msg);
                HistoryLog("reply=" + reply);
                if (reply == null)
                    throw new TrxException(TrxException.INVALID_MESSAGE);
                strRep = en.GetString(reply.getData());
                
            }
            catch (TrxException ex)
            {
                throw new TrxException(ex.ToString(), ex);
            }
            finally
            {
                try
                {
                    stop();
                }
                catch (Exception e)
                {
                    throw new TrxException(e.ToString(), e);
                }
            }
            return strRep;
        }

        private void stop()
        {
            ioiSession.disconnect();
            ioiSession.destroy();
        }

        #region SessionEventListener 멤버

        void SessionEventListener.onConnect(Session ss)
        {
            Console.WriteLine("Session Connected...");
        }

        void SessionEventListener.onDisconnect(Session ss)
        {
            Console.WriteLine("Session Disconnected...");
        }

        #endregion

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
