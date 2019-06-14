/*****************************************************************************************************
* 项目名称：FBIM_Remote_Server
* 命名空间：Server_Model.BusinessModel
* 类名称：OnRemotingModel
* 创建时间：2018/11/23 
* 创建人：zhangbaoj
* 创建说明：正在远程用户双方模型
*****************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace Server_Model.BusinessModel
{
    public class OnRemotingModel
    {
        /// <summary>
        /// 远程发起人ID
        /// </summary>
        private string accepter;
        /// <summary>
        /// 被远程人ID
        /// </summary>
        private string sender;
        /// <summary>
        /// 开始远程时间
        /// </summary>
        private DateTime startConnectTime;

        public string Accepter { get => accepter; set => accepter = value; }
        public string Sender { get => sender; set => sender = value; }
        public DateTime StartConnectTime { get => startConnectTime; set => startConnectTime = value; }
    }
}
