/*****************************************************************************************************
* 项目名称：FBIM_Remote_Server
* 命名空间：Server_Model.Handler
* 类名称：Handler
* 创建时间：2018/10/30
* 创建人：zhangbaoj
* 创建说明：委托类
*****************************************************************************************************/
using Server_Model.Entity;
using Server_Model.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server_Model.Handler
{
    #region client

    public delegate void OnConnectedHandler(object sender);

    public delegate void OnCleintMessageReceivedHandler(object sender, Message msg);

    public delegate void OnCleintDataReceivedHandler(object sender, byte[] data);

    public delegate void OnClientErrorHandler(Exception ex, string msg);

    /// <summary>
    /// 用户退出
    /// </summary>
    public delegate void OnOneSideLogout();


    #endregion


    #region server

    public delegate void OnAcceptedHandler(int num, IUserToken userToken);

    public delegate void OnUnAcceptedHandler(int num, IUserToken userToken);

    public delegate void OnSocketReceivedMessageHandler(IUserToken userToken, byte[] args);

    public delegate void OnServerReceivedMessageHandler(IUserToken userToken, Message msg);

    public delegate void OnServerErrorHandler(Exception ex, params object[] args);

    public delegate void OnWSConnectedHandler(int num, IUserToken userToken);

    public delegate void OnTestHandler(string UID, string msg);


    #endregion


}
