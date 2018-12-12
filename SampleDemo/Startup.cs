using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SSODecodeCJW;

namespace SampleDemo
{
    public class Startup
    {
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
                string KeyStr = "CJWKEY";
                string encryptedText = "8evVae2ekt7WtC2umaHAqYVyhf2W9eNA";

                CryptoDotNet cdn = new CryptoDotNet();
                string PlainText = cdn.Decrypt(encryptedText, KeyStr);

                await context.Response.WriteAsync(PlainText);
            });
        }
    }
}
