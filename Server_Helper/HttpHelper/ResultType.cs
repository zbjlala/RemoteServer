using System;
using System.Collections.Generic;
using System.Text;

namespace Server_Helper.HttpHelper
{
    public enum ResultType
    {
        USER_INFO = 1, //登录返回用户信息
        RCONNECT_INFO = 2, //重连失败返回信息
        OTHER_LOGOUT = 3 //对方已登出
    }
}
