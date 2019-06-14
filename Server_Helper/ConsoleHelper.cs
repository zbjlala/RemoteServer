/*****************************************************************************************************
* 项目名称：FBIM_Remote_Server
* 命名空间：Server_Helper
* 类名称：ConsoleHelper
* 创建时间：2018/10/30 
* 创建人：zhangbaoj
* 创建说明： console输出帮助类
*****************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace Server_Helper
{
    /// <summary>
    ///     console输出帮助类
    /// </summary>
    public static class ConsoleHelper
    {
        public static void WriteInfo(ConsoleColor color = ConsoleColor.Green, params object[] args)
        {
            FunHelper.FunQueue.Enqueue(() =>
            {
                var sb = new StringBuilder();
                if ((args != null) && (args.Length > 0))
                {
                    sb.Append("datetime:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "  ");
                    if (args.Length == 1)
                    {
                        if (args[0] != null)
                            sb.Append(args[0] + "   ");
                    }
                    else
                        foreach (var item in args)
                            if (item != null)
                                sb.Append(item.GetType().Name + ":" + item + "   ");
                }
                var c = Console.ForegroundColor;
                Console.ForegroundColor = color;
                Console.WriteLine(sb.ToString());
                Console.ForegroundColor = c;
                return sb.ToString();
            });
        }

        public static void WriteInfo(params object[] args)
        {
            FunHelper.ActionQueue.Enqueue(() => { WriteInfo(ConsoleColor.Green, args); });
        }

        public static void WriteErr(Exception ex, ConsoleColor color = ConsoleColor.Red, params object[] p)
        {
            FunHelper.FunQueue.Enqueue(() =>
            {
                var sb = new StringBuilder();
                if ((p != null) && (p.Length > 0))
                {
                    sb.Append("datetime:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "  ");
                    foreach (var item in p)
                        if (item != null)
                            sb.Append(item.GetType().Name + ":" + item + "   ");
                    if (ex != null)
                        sb.Append("Error:" + ex.Message + "  " + Environment.NewLine + ex.StackTrace);
                }
                var c = Console.ForegroundColor;
                Console.ForegroundColor = color;
                Console.WriteLine(sb.ToString());
                Console.ForegroundColor = c;
                return sb.ToString();
            });
        }

        public static void WriteErr(Exception ex, params object[] args)
        {
            FunHelper.ActionQueue.Enqueue(() => { WriteErr(ex, ConsoleColor.Red, args); });
        }

        public static void WriteLine(string msg, ConsoleColor color = ConsoleColor.Gray, bool simple = true,
            params object[] args)
        {
            FunHelper.ActionQueue.Enqueue(() =>
            {
                var sb = new StringBuilder();
                sb.Append("datetime:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "  ");
                if (simple)
                {
                    if (args.Length > 0)
                        sb.Append(string.Format(msg, args));
                    else
                        sb.Append(msg);
                    var c = Console.ForegroundColor;
                    Console.ForegroundColor = color;
                    Console.WriteLine(sb.ToString());
                    Console.ForegroundColor = c;
                }
                else
                {
                    if (args.Length > 0)
                        sb.Append(string.Format(msg, args));
                    else
                        sb.Append(msg);
                    var c = Console.ForegroundColor;
                    Console.ForegroundColor = color;
                    Console.WriteLine(sb.ToString());
                    Console.ForegroundColor = c;
                }
            });
        }
    }
}
