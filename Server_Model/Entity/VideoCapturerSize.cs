/*****************************************************************************************************
* 项目名称：FBIM_Remote_Server
* 命名空间：Server_Model.Entity
* 类名称：VideoCapturerSize
* 创建时间：2018/10/30 
* 创建人：zhangbaoj
* 创建说明：视频显示尺寸大小 
*****************************************************************************************************/
using Server_Model.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server_Model.Entity
{
    /// <summary>
    /// 视频显示尺寸大小 
    /// </summary>
    public class VideoCapturerSize
    {
        /// <summary>
        ///  视频显示高度
        /// </summary>
        public int Width = 160;

        /// <summary>
        ///  视频显示宽度
        /// </summary>
        public int Height = 120;

        /// <summary>
        /// 设置大小模式
        /// </summary>
        /// <param name="Model">视频显示尺寸大小</param>
        public VideoCapturerSize(VideoSizeModel Model)
        {
            SetModel(Model);
        }


        /// <summary>
        /// 设置大小模式
        /// </summary>
        /// <param name="Model"></param>
        public void SetModel(VideoSizeModel Model)
        {
            switch (Model)
            {
                case VideoSizeModel.W160_H120:
                    Width = 160;
                    Height = 120;
                    break;
                case VideoSizeModel.W176_H144:
                    Width = 176;
                    Height = 144;
                    break;
                case VideoSizeModel.W320_H240:
                    Width = 320;
                    Height = 240;
                    break;
                case VideoSizeModel.W352_H288:
                    Width = 352;
                    Height = 288;
                    break;
                case VideoSizeModel.W640_H480:
                    Width = 640;
                    Height = 480;
                    break;
                case VideoSizeModel.W800_H600:
                    Width = 800;
                    Height = 600;
                    break;
            }
        }
    }
}
