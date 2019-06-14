/*****************************************************************************************************
* 项目名称：FBIM_Remote_Server
* 命名空间：Server_Core.Tcp.Model
* 类名称：Mapping
* 创建时间：2018/10/30
* 创建人：zhangbaoj
* 创建说明：Server接口
*****************************************************************************************************/
using Server_Model.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server_Core.Tcp.Model
{
    public class Mapping
    {
        public string ChannelID
        {
            get; set;
        }

        public string UID
        {
            get; set;
        }

        public IUserToken UserToken
        {
            get; set;
        }
    }
}
