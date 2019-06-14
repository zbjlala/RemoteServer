using System;
using System.Collections.Generic;
using System.Text;

namespace Server_Helper.HttpHelper
{
    public static class InterfaceUrl
    {
        /// <summary>
        /// 两个客户端建立连接接口
        /// </summary>
        public static readonly string REMOTE_ESCONNECTION = "https://inte-service.chanjet.com/robot/api/remote/remote-create";
        //客户端登录日志
        public static readonly string LOGIN_LOG = "https://inte-service.chanjet.com/robot/api/remote/remote-login";
        //查询用户详细信息
        public static readonly string USER_INFO = "http://moni-service.chanjet.com/robot/api/tplus-login/get-user-info";
        //根据IP获取地区信息
        public static readonly string IP_Address = "http://moni-service.chanjet.com/robot/api/remote/remote-ip";

    }
}
