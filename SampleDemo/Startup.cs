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
                //string aaa = context.Request.Query.Keys.Count;
                //?M=DkvHa3xSeO7DWcEhcFHHug==
                string id =context.Request.QueryString.Value;
                
                string PlainText = "";

                if (id.Substring(0,3) != "?M=")
                {
                    //CJ 월드
                    string KeyStr = "CJWKEY";
                    string encryptedText = id.Replace("?P=", "");

                    CryptoDotNet cdn = new CryptoDotNet();
                    PlainText  = cdn.Decrypt(encryptedText, KeyStr);
                }
                else
                {
                    //get방식 진행이 된다고함
                    //SMART CJ
                    string encKey = "WC00000075531151";     // 복호화 Key 별도 전달
                    string encrypt_uId = HttpUtility.UrlDecode(id.Replace("?M=", ""));
                    string decrypt_uId = SmartCJ.SSO.Utils.Decrypt(encrypt_uId, encKey);		// 복호화된 CJ월드 로그인ID

                    PlainText = decrypt_uId;
                    //tec.call();
                }
                
                
                await context.Response.WriteAsync(PlainText);
            });
        }
    }
}
