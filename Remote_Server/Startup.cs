using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Server_Core.Tcp;
using Server_Helper;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Remote_Server
{
    public class Startup
    {
        private static ServerSocket server;
        public Startup(IConfiguration configuration)
        {
            RemoteServerStart();
            Configuration = configuration;
        }
        public void RemoteServerStart()
        {
            Task.Factory.StartNew(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                try
                {
                    var config = Environment.GetEnvironmentVariable("ConfigType"); ;
                    ConsoleHelper.WriteLine("正在初始化服务器...", ConsoleColor.Green);
                    //log = new Log4netHelper();//log先加载后面加载项会用到log
                    server = new ServerSocket();
                    ConsoleHelper.WriteLine("服务器初始化完毕...", ConsoleColor.Green);
                    ConsoleHelper.WriteLine("正在启动服务器...", ConsoleColor.Green);
                    try
                    {
                        server.StartAsync();
                    }
                    catch (Exception ex)
                    {
                        ConsoleHelper.WriteLine(ex.ToString());
                    }
                    ConsoleHelper.WriteLine("服务器启动完毕...", ConsoleColor.Green);
                    ConsoleHelper.WriteLine("点击回车，结束服务");
                    Console.ReadLine();
                    //server._wsStocket.Stop();
                }
                catch (Exception ex)
                {
                    ConsoleHelper.WriteErr(ex);
                    //Log4netHelper.WriteError(ex, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.ToString());
                }
                finally
                {
                   // RedisHelper.KeyDelete("moni-RemoteServerList");
                }
            });

        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "api/{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    //spa.UseAngularCliServer(npmScript: "start");
                    spa.UseProxyToSpaDevelopmentServer("http://localohost:4200/");
                }
            });
        }
    }
}
