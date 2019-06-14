/*****************************************************************************************************
* 项目名称：FBIM_Remote_Server
* 命名空间：Server_Helper.Log4net
* 类名称：LogLevel
* 创建时间：2018/11/21
* 创建人：zhangbaoj
* 创建说明：  日志级别
*****************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace Server_Helper.Log4net
{
    public enum LogLevel
    {
        /// <summary>
        /// 消息
        /// </summary>
        info,

        /// <summary>
        /// 警告
        /// </summary>
        InterfaceError,

        /// <summary>
        /// 错误
        /// </summary>
        Error
    }
}
