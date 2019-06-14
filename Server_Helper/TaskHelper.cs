using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server_Helper
{
    /// <summary>
    /// 任务加强辅助类
    /// </summary>
    public static class TaskHelper
    {
        /// <summary>
        /// 启动异步任务
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static Task Run(Action action)
        {
            return Task.Factory.StartNew(action);
        }

        /// <summary>
        /// 任务超时时长为30秒
        /// </summary>
        /// <param name="action"></param>
        /// <param name="expreid"></param>
        /// <param name="failed"></param>
        public static void StartNew(Action action, int expreid = 30 * 1000, Action failed = null)
        {
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken token = cancelTokenSource.Token;
            var task = Task.Factory.StartNew(action, token);
            Task.Factory.StartNew(() =>
            {
                if (task != null && task.Status != TaskStatus.RanToCompletion && task.Status != TaskStatus.Faulted && task.Status != TaskStatus.Canceled)
                {
                    Thread.Sleep(expreid);
                    cancelTokenSource.Cancel();
                    failed?.Invoke();
                }
            });
        }
        /// <summary>
        /// 任务超时时长为30秒
        /// </summary>
        /// <param name="action"></param>
        /// <param name="expreid"></param>
        /// <param name="failed"></param>
        /// <param name="finall"></param>
        public static void StartNew(Action action, int expreid = 30 * 1000, Action failed = null, Action finall = null)
        {
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken token = cancelTokenSource.Token;
            var task = Task.Factory.StartNew(action, token);
            Task.Factory.StartNew(() =>
            {
                if (task != null && task.Status != TaskStatus.RanToCompletion && task.Status != TaskStatus.Faulted && task.Status != TaskStatus.Canceled)
                {
                    Thread.Sleep(expreid);
                    cancelTokenSource.Cancel();
                    failed?.Invoke();
                }
                finall?.Invoke();
            });
        }
        /// <summary>
        /// 任务超时时长为30秒
        /// </summary>
        /// <param name="action"></param>
        /// <param name="state"></param>
        /// <param name="expreid"></param>
        /// <param name="failed"></param>
        public static void StartNew(Action<object> action, object state, int expreid = 30 * 1000, Action<object> failed = null)
        {
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken token = cancelTokenSource.Token;
            var task = Task.Factory.StartNew(action, state, token);
            Task.Factory.StartNew(() =>
            {
                if (task != null && task.Status != TaskStatus.RanToCompletion && task.Status != TaskStatus.Faulted && task.Status != TaskStatus.Canceled)
                {
                    Thread.Sleep(expreid);
                    cancelTokenSource.Cancel();
                    failed?.Invoke(state);
                }
            });
        }
        /// <summary>
        /// 任务超时时长为30秒
        /// </summary>
        /// <param name="action"></param>
        /// <param name="state"></param>
        /// <param name="expreid"></param>
        /// <param name="failed"></param>
        /// <param name="finall"></param>
        public static void StartNew(Action<object> action, object state, int expreid = 30 * 1000, Action<object> failed = null, Action finall = null)
        {
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken token = cancelTokenSource.Token;
            var task = Task.Factory.StartNew(action, state, token);
            Task.Factory.StartNew(() =>
            {
                if (task != null && task.Status != TaskStatus.RanToCompletion && task.Status != TaskStatus.Faulted && task.Status != TaskStatus.Canceled)
                {
                    Thread.Sleep(expreid);
                    cancelTokenSource.Cancel();
                    failed?.Invoke(state);
                }
                finall?.Invoke();
            });
        }
    }
}
