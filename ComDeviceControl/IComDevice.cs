using ComDeviceControl.ComDevice;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace ComDeviceControl
{
    public interface IComDevice
    {
        /// <summary>
        /// 获取或设置串口
        /// </summary>
        SerialPort SerialDevice { get; set; }

        /// <summary>
        /// 获取或设置自定义串口通讯协议
        /// </summary>
        ComDeviceProtocol Protocol { get; set; }

        /// <summary>
        /// 打开串口资源
        /// </summary>
        /// <returns></returns>
        bool Open();

        /// <summary>
        /// 关闭串口资源
        /// </summary>
        void Close();

        /// <summary>
        /// 向串口发送数据
        /// </summary>
        /// <param name="data"></param>
        void SendData(byte[] data);

        /// <summary>
        /// 使用自定义通讯协议，发送数据至串口，并接收返回数据
        /// </summary>
        /// <returns></returns>
        Tuple<bool, string> GetData();
    }
}
