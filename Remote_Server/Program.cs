using Server_Core.Tcp;
using Server_Helper;
using Server_Helper.Log4net;
using Server_Model.Entity;
using Server_Model.Enum;
using Server_Model.Interface;
using SocketConnectedTest;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Remote_Server
{
    //public delegate bool ControlCtrlDelegate(int CtrlType);
    class Program
    {
        private static ServerSocket server;
        private static Log4netHelper log;


        //[DllImport("kernel32.dll")]
        //private static extern bool SetConsoleCtrlHandler(ControlCtrlDelegate HandlerRoutine, bool Add);

        //static ControlCtrlDelegate newDelegate = new ControlCtrlDelegate(HandlerRoutine);

        //public static bool HandlerRoutine(int CtrlType)
        //{
        //    switch (CtrlType)
        //    {
        //        case 0:
        //            Console.WriteLine("0工具被强制关闭"); //Ctrl+C关闭
        //            RedisHelper.KeyDelete("RemoteServerList");
        //            break;
        //        case 2:
        //            Console.WriteLine("2工具被强制关闭");//按控制台关闭按钮关闭
        //            RedisHelper.KeyDelete("RemoteServerList");
        //            break;
        //    }
        //    return false;
        //}


        private static void Main(string[] args)
        {
           // bool bRet = SetConsoleCtrlHandler(newDelegate, true);

            
                try
                {
                var config = Environment.GetEnvironmentVariable("ConfigType"); ;
                    ConsoleHelper.WriteLine("正在初始化服务器...", ConsoleColor.Green);
                    log = new Log4netHelper();//log先加载后面加载项会用到log
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
                    server._wsStocket.Stop();
                }
                catch (Exception ex)
                {
                    ConsoleHelper.WriteErr(ex);
                    Log4netHelper.WriteError(ex, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.ToString());
                }
                finally
                {
                    RedisHelper.KeyDelete("moni-RemoteServerList");
                }
            }

        }

}
