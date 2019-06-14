/*****************************************************************************************************
* 项目名称：FBIM_Remote_Server
* 命名空间：Remote_Model.Enum
* 类名称：VideoSizeModel
* 创建时间：2018/10/30 
* 创建人：zhangbaoj
* 创建说明：视频模式 
*****************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace Server_Model.Enum
{
    public enum VideoSizeModel
    {
        /// <summary>
        /// 捕获视频大小宽为160，高120像素
        /// </summary>
        W160_H120,
        /// <summary>
        /// 捕获视频大小宽为176，高144像素
        /// </summary>
        W176_H144,
        /// <summary>
        /// 捕获视频大小宽为320，高240像素
        /// </summary>
        W320_H240,
        /// <summary>
        /// 捕获视频大小宽为352，高288像素
        /// </summary>
        W352_H288,
        /// <summary>
        /// 捕获视频大小宽为640，高480像素
        /// </summary>
        W640_H480,
        /// <summary>
        /// 捕获视频大小宽为800，高600像素
        /// </summary>
        W800_H600,
    }
}
