using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SSODecodeCJW;
using SampleDemo.SAP;
using Microsoft.VisualBasic.CompilerServices;
using System.Web;
using SmartCJ.SSO;
using System.Text;
using System.Security.Cryptography;
using System.Diagnostics;
using System.IO;

namespace SampleDemo
{
    public class Startup
    {

        public static TestEaiCall tec = new TestEaiCall();

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                string id =context.Request.QueryString.Value;
                
                string PlainText = "";

                if (id.Substring(0,3) == "?P=")
                {
                    //CJ 월드
                    string KeyStr = "CEJKSP";

                    string encryptedText = id.Replace("?P=", "");

                    // CJWKEY 로  8evVae2ekt7WtC2umaHAqYVyhf2W9eNA 을 decrypt 결과는 cjwsampleuser 입니다. 
                    // KeyStr ="CJWKEY";
                    //string encryptedText = "g1USFCLOzazH1TKqMb+jTvvuG1gyo4iW";

                    CryptoDotNet cdn = new CryptoDotNet();
                    PlainText = cdn.Decrypt(encryptedText, KeyStr);
                }
                else if(id.Substring(0, 3) == "?M=")
                {
                    //get방식 진행이 된다고함
                    //SMART CJ
                    string encKey = "WC00000075531151";     // 복호화 Key 별도 전달
                    HistoryLog("encrypt_uId1" + id.Replace("?M=", ""));
                    string encrypt_uId = HttpUtility.UrlDecode(id.Replace("?M=", ""));
                    HistoryLog("encrypt_uId1" + encrypt_uId);
                    string decrypt_uId = SmartCJ.SSO.Utils.Decrypt(encrypt_uId, encKey);		// 복호화된 CJ월드 로그인ID

                    PlainText = decrypt_uId;
                    //tec.call();
                }
                else
                {
                    //sap 비밀번호 초기화
                    string str = id.Replace("?T=", "");
                    HistoryLog("str=" + str);
                    //string urlParameter = "&sabun=" + sabun + "sabun=" + uData[0].reissue;
                    string[] urlParameter = str.Split("&");
                    //아이디, 사번, 재발급 사유 

                    string userId = urlParameter[1].Replace("userid=", "");
                    string sabun = urlParameter[2].Replace("sabun=", "");
                    string reissue = urlParameter[3].Replace("reissue=", "");
                    string optional_1 = urlParameter[4].Replace("optional_1=", "");
                    reissue = System.Web.HttpUtility.UrlDecode(reissue);
                    string sapInitResult  = tec.call(userId, sabun, reissue, optional_1);

                    string utf = "<!DOCTYPE html><html xmlns = \"http://www.w3.org/1999/xhtml\" ><head><meta charset=\"utf-8\" ><title>CJ Employee</title></ head><body>"+ sapInitResult + "</body></html>";
                    PlainText = utf;
                }
                await context.Response.WriteAsync(PlainText);
            });
        }

        public static string Encrypt(string toEncrypt, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            // Get the key from config file

            string key = "IDANNM";
            //System.Windows.Forms.MessageBox.Show(key);
            //If hashing use get hashcode regards to your key
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                //Always release the resources and flush data
                //of the Cryptographic service provide. Best Practice

                hashmd5.Clear();
            }
            else
            {
                keyArray = UTF8Encoding.UTF8.GetBytes(key);
            }               

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes. We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            byte[] resultArray = cTransform.TransformFinalBlock
                    (toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
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
