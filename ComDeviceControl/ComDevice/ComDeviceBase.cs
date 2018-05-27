using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace ComDeviceControl.ComDevice
{
    public abstract class ComDeviceBase
    {
        private StringBuilder m_stringBuilder = new StringBuilder();
        private readonly object m_dataLocker = new object();

        /// <summary>
        /// 获取已经接收到的字符串
        /// </summary>
        protected string ReceivedString
        {
            get
            {
                lock (m_dataLocker)
                {
                    return m_stringBuilder.ToString();
                }
            }
        }

        /// <summary>
        /// 获取或设置串口
        /// </summary>
        public SerialPort SerialDevice { get; set; }

        /// <summary>
        /// 获取或设置自定义串口通讯协议
        /// </summary>
        public ComDeviceProtocol Protocol { get; set; }

        /// <summary>
        /// 打开串口资源
        /// </summary>
        /// <returns></returns>
        public bool Open()
        {
            try
            {
                if (!SerialDevice.IsOpen)
                {
                    SerialDevice.Open();
                }
                return SerialDevice.IsOpen;
            }
            catch (Exception ex)
            {
                ExceptionProcess(ex);
            }
            return false;
        }

        /// <summary>
        /// 关闭串口资源
        /// </summary>
        public void Close()
        {
            try
            {
                SerialDevice.Close();
                SerialDevice.Dispose();
            }
            catch (Exception ex)
            {
                ExceptionProcess(ex);
            }
        }

        /// <summary>
        /// 向串口发送数据
        /// </summary>
        /// <param name="data"></param>
        public void SendData(byte[] data)
        {
            try
            {
                SerialDevice.Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                ExceptionProcess(ex);
            }
        }

        /// <summary>
        /// 使用自定义通讯协议，发送数据至串口，并接收返回数据
        /// </summary>
        /// <returns></returns>
        public abstract Tuple<bool, string> GetData();


        /// <summary>
        /// 注册数据接收事件
        /// </summary>
        protected void RegisterReceivedEvent()
        {
            RemoveReceivedEvent();
            SerialDevice.DataReceived += SerialPort_DataReceived;
        }
        /// <summary>
        /// 解除数据接收事件
        /// </summary>
        protected void RemoveReceivedEvent()
        {
            SerialDevice.DataReceived -= SerialPort_DataReceived;
        }
        /// <summary>
        /// 清除已接收数据
        /// </summary>
        protected void ClearReceivedData()
        {
            lock (m_dataLocker)
            {
                m_stringBuilder.Clear();
            }
        }
        /// <summary>
        /// 串口接收事件响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            System.Threading.Thread.Sleep(20);
            try
            {
                lock (m_dataLocker)
                {
                    m_stringBuilder.Append(SerialDevice.ReadExisting());
                }
            }
            catch (Exception ex)
            {
                ExceptionProcess(ex);
            }
        }

        protected abstract void ExceptionProcess(Exception ex);
    }
}
