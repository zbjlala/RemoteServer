/*****************************************************************************************************
* 项目名称：FBIM_Remote_Server
* 命名空间：Server_Helper
* 类名称：MutiThreadHelper
* 创建时间：2018/10/30
* 创建人：zhangbaoj
* 创建说明： 多线程处理类
*****************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Server_Helper
{
    public class MutiThreadHelper : IDisposable
    {
        private List<Thread> list;

        public MutiThreadHelper()
        {
            list = new List<Thread>();
        }

        public int Count { get; private set; }

        public bool IsStarted { get; private set; }

        public void Dispose()
        {
            Stop();
        }

        /// <summary>
        ///     多线程任务启动
        /// </summary>
        /// <param name="num"></param>
        /// <param name="action"></param>
        public void Start(int num, Action action)
        {
            if (num > 0)
                if (!IsStarted)
                {
                    Count = num;
                    IsStarted = true;
                    for (var i = 0; i < num; i++)
                    {
                        var th = new Thread(new ThreadStart(action));
                        th.IsBackground = true;
                        th.Start();
                        list.Add(th);
                    }
                }
        }

        /// <summary>
        ///     多线程任务停止
        /// </summary>
        public void Stop()
        {
            if (IsStarted)
            {
                foreach (var td in list)
                    if (td != null)
                        td.Abort();
                IsStarted = false;
                Count = 0;
                list.Clear();
            }
        }
    }
}
