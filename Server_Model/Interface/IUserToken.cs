/*****************************************************************************************************
* 项目名称：FBIM_Remote_Server
* 命名空间：Server_Model.Interface
* 类名称：IUserToken
* 创建时间：2018/10/30
* 创建人：zhangbaoj
* 创建说明：UserToken 接口
*****************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Server_Model.Interface
{
    public interface IUserToken
    {
        string UID
        {
            get; set;
        }
        string LastID
        {
            get;set;
        }

        SocketAsyncEventArgs ReceiveArgs
        {
            get; set;
        }

        Socket ConnectSocket
        {
            get;
            set;
        }

        int MaxBufferSize
        {
            get;
        }
        DateTime? ConnectDateTime
        {
            get; set;
        }
        DateTime ActiveDateTime
        {
            get; set;
        }

        void UnPackage(byte[] receiveData, Action<TcpPackage> action);
    }
}
