using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComDeviceControl.ComDevice
{
    #region 自定义串口通讯协议详解
    /* 
        * 1、向指定串口发送StartCommand；
        * 2、接收数据，直到接收到的数据长度足够（ReceiveDataSize）；
        * 3、校验接收到的数据中有是否有开始字符和结束字符（StartString & EndString）；
        * 4、校验成功则开始从接收到的数据中截取字符串（DataStartIndex , DataLength）；
        * 5、若接收到错误字符串，则返回失败（ErrorString）；
        * 6、超时机制，指定时间未接收到足够的数据长度，返回失败（TimeOut）；
        * 7、最后发送关闭指令StopCommand；
    */
    #endregion

    /// <summary>
    /// 自定义串口通讯协议
    /// </summary>
    public class ComDeviceProtocol
    {
        /// <summary>
        /// 开始需要发送的指令
        /// </summary>
        public byte[] StartCommand { get; set; }

        /// <summary>
        /// 结束需要发送的指令
        /// 可选，某些设备可能需要在使用完之后关闭
        /// </summary>
        public byte[] StopCommand { get; set; }

        /// <summary>
        /// 接收到的错误字符串
        /// </summary>
        public string ErrorString { get; set; }

        /// <summary>
        /// 需要接收的数据长度 
        /// </summary>
        public int ReceiveDataSize { get; set; }

        /// <summary>
        /// 接收到的开始字符串
        /// </summary>
        public string StartString { get; set; }

        /// <summary>
        /// 接收到的结束字符串
        /// </summary>
        public string EndString { get; set; }

        /// <summary>
        /// 从接收到的字符串获取数据：起始字符位置索引（从零开始）
        /// </summary>
        public int DataStartIndex { get; set; }

        /// <summary>
        /// 从接收到的字符串获取数据：需要截取的字符串的字符数。
        /// </summary>
        public int DataLength { get; set; }

        /// <summary>
        /// 超时时间，单位：ms
        /// （小于0 则无限等待）
        /// </summary>
        public int TimeOut { get; set; }
    }
}
