/*****************************************************************************************************
* 项目名称：FBIM_Remote_Server
* 命名空间：Server_Helper
* 类名称：FunHelper
* 创建时间：2018/10/30 
* 创建人：zhangbaoj
* 创建说明：  任务处理帮助类
*****************************************************************************************************/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server_Helper
{
    /// <summary>
    ///     任务处理帮助类
    /// </summary>
    public static class FunHelper
    {
        static FunHelper()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Action a;
                    ActionQueue.TryDequeue(out a);
                    a?.Invoke();
                    if (a == null)
                        Thread.Sleep(1);
                }
            });
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Func<string> f;
                    FunQueue.TryDequeue(out f);
                    f?.Invoke();
                    if (f == null)
                        Thread.Sleep(1);
                }
            });
        }

        public static void OneceAsync(Action funcation)
        {
            Task.Factory.StartNew(() => { funcation(); });
        }

        private static void SingleWhileAsync(Action funcation, int mil)
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    funcation();
                    Thread.Sleep(mil);
                }
            });
        }

        public static void WhileAsync(Action funcation, int mil, bool muti = false)
        {
            if (!muti)
                SingleWhileAsync(funcation, mil);
            else
                Task.Factory.StartNew(() =>
                {
                    while (true)
                    {
                        Task.Factory.StartNew(() => { funcation(); });
                        Thread.Sleep(mil);
                    }
                });
        }

        public static void ForAsync(Action funcation, int num)
        {
            Task.Factory.StartNew(() => { Parallel.For(0, num, i => { funcation(); }); });
        }

        #region Queues

        /// <summary>
        ///     无返回值的任务队列
        /// </summary>
        public static ConcurrentQueue<Action> ActionQueue { get; } = new ConcurrentQueue<Action>();

        /// <summary>
        ///     含返回值的任务队列
        /// </summary>
        public static ConcurrentQueue<Func<string>> FunQueue { get; } = new ConcurrentQueue<Func<string>>();

        #endregion
    }
}
