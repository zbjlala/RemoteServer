/*****************************************************************************************************
* 项目名称：FBIM_Remote_Server
* 命名空间：Server_Model.Enum
* 类名称：MessageProtocalEnum
* 创建时间：2018/10/30
* 创建人：zhangbaoj
* 创建说明：传输消息类型枚举
*****************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace Server_Model.Enum
{
    public enum MessageProtocalEnum
    {
        Heart = 0,
        Login = 1,
        Logout = 2,
        Subscribe = 3,
        Unsubscribe = 4,
        PrivateMsg = 5,
        Message = 6,
        File = 7,
        RemoteConnect = 8,
        RemoteStart = 9,
        FileSlice = 10,
        CheckStatus = 11,
        ReConnection = 12,
        StopSend = 13,
        RLogin = 101,
        RSubscribe = 103
    }
}
