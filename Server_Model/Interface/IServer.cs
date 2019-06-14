/*****************************************************************************************************
* 项目名称：FBIM_Remote_Server
* 命名空间：Server_Model.Interface
* 类名称：IServer
* 创建时间：2018/10/30 
* 创建人：zhangbaoj
* 创建说明：Server接口
*****************************************************************************************************/
using Server_Model.Handler;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server_Model.Interface
{
    public interface IServer
    {
        event OnServerErrorHandler OnErrored;

        /// <summary>
        /// 启动服务
        /// </summary>
        void Start();

        /// <summary>
        /// 停止服务器
        /// </summary>
        void Stop();

        /// <summary>
        /// 是否启动
        /// </summary>
        bool IsStarted
        {
            get;
        }

        /// <summary>
        /// 在线客户端数
        /// </summary>
        int OnlineNums
        {
            get;
        }
    }
}
