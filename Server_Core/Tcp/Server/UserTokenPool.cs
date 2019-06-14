/*****************************************************************************************************
* 项目名称：FBIM_Remote_Server
* 命名空间：Server_Core.Tcp.Server
* 类名称：UserTokenPool
* 创建时间：2018/10/30
* 创建人：zhangbaoj
* 创建说明： 用户列表复用池
*****************************************************************************************************/
using Server_Model.Interface;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Server_Core.Tcp.Server
{
    public class UserTokenPool
    {
        private static ConcurrentQueue<IUserToken> pool = new ConcurrentQueue<IUserToken>();

        /// <summary>
        ///     用户列表复用池
        /// </summary>
        /// <param name="max"></param>
        public UserTokenPool(Type type, int max, int initBufferSize, EventHandler<SocketAsyncEventArgs> IOEvent,BufferManager buffer)
        {
            for (var i = 0; i < max; i++)
            {
                var userToken = (IUserToken)Activator.CreateInstance(type);
                userToken.ReceiveArgs.Completed += IOEvent;
                userToken.ReceiveArgs.UserToken = userToken;
                buffer.SetBuffer(userToken.ReceiveArgs);
                pool.Enqueue(userToken);
            }
        }

        public void Push(IUserToken userToken)
        {
            pool.Enqueue(userToken);
        }

        public IUserToken Pop()
        {
            IUserToken userToken = null;
            pool.TryDequeue(out userToken);
            return userToken;
        }

        public int Count()
        {
            return pool.Count;
        }
    }
}
