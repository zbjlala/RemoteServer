/*****************************************************************************************************
* 项目名称：FBIM_Remote_Server
* 命名空间：Remote_Model.Interface
* 类名称：Handler
* 创建时间：2018/10/30 
* 创建人：zhangbaoj
* 创建说明：传输信息接口
*****************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace Server_Model.Interface
{
    public interface IMessage
    {
        string Accepter { get; set; }

        byte Protocal { get; set; }

        byte[] Data { get; set; }

        string Sender { get; set; }

        long SendTick { get; set; }
    }
}
