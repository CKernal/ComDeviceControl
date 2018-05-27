using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ComDeviceControl.ComDevice
{
    /// <summary>
    /// 串口控制类，需设置串口和自定义串口通讯协议。
    /// </summary>
    public class ComDevice : ComDeviceBase, IComDevice
    {

        public override Tuple<bool, string> GetData()
        {
            if (!SerialDevice.IsOpen
                && !Open())
            {
                return new Tuple<bool, string>(false, "ComDevice open failed.");
            }

            if (Protocol == null)
            {
                return new Tuple<bool, string>(false, "ComDeviceProtocol is null.");
            }

            bool bRet = false;
            string strData = string.Empty;

            ClearReceivedData();
            RegisterReceivedEvent();
            SendCommand(Protocol.StartCommand);
            DateTime startDateTime = DateTime.Now;

            while (true)
            {
                Thread.Sleep(1);
                
                //超时机制
                if (Protocol.TimeOut >= 0
                    && (DateTime.Now - startDateTime).TotalMilliseconds > Protocol.TimeOut)
                {
                    bRet = false;
                    strData = string.Format("TimeOut: {0} ms", Protocol.TimeOut);
                    break;
                }

                //检查错误数据机制
                if (CheckDataError())
                {
                    bRet = false;
                    strData = Protocol.ErrorString;
                    break;
                }

                //校验数据机制
                if (ReceivedString.Length >= Protocol.ReceiveDataSize
                    && CheckDataExist(Protocol.StartString)
                    && CheckDataExist(Protocol.EndString))
                {
                    try
                    {
                        bRet = true;
                        strData = ReceivedString;
                        DataProcess(ref strData);
                    }
                    catch (Exception ex)
                    {
                        bRet = false;
                        strData = ex.Message;
                        ExceptionProcess(ex);
                    }
                    break;
                }
            }

            RemoveReceivedEvent();
            SendCommand(Protocol.StopCommand);
            return new Tuple<bool, string>(bRet, strData);
        }

        private void DataProcess(ref string strData)
        {
            ProcessStartString(ref strData);
            ProcessEndString(ref strData);
            ProcessFinally(ref strData);
        }

        private void ProcessStartString(ref string strData)
        {
            if (!string.IsNullOrEmpty(Protocol.StartString))
            {
                int startIndex = strData.IndexOf(Protocol.StartString) + Protocol.StartString.Length;
                strData = strData.Substring(startIndex);
            }
        }

        private void ProcessEndString(ref string strData)
        {
            if (!string.IsNullOrEmpty(Protocol.EndString))
            {
                int lentgh = strData.IndexOf(Protocol.EndString);
                strData = strData.Substring(0, lentgh);
            }
        }

        private void ProcessFinally(ref string strData)
        {
            if (Protocol.DataStartIndex > 0)
            {
                strData = strData.Substring(Protocol.DataStartIndex);
            }

            if (Protocol.DataLength > 0)
            {
                strData = strData.Substring(0, Protocol.DataLength);
            }
        }

        private void SendCommand(byte[] cmd)
        {
            if (cmd != null
                && cmd.Length > 0)
            {
                SendData(cmd);
            }
        }

        private bool CheckDataExist(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return true;
            }
            if (ReceivedString.Contains(data))
            {
                return true;
            }
            return false;
        }

        private bool CheckDataError()
        {
            if (!string.IsNullOrEmpty(Protocol.ErrorString)
                && ReceivedString.IndexOf(Protocol.ErrorString) >= 0)
            {
                return true;
            }
            return false;
        }



        protected override void ExceptionProcess(Exception ex)
        {
            System.Diagnostics.Trace.WriteLine(string.Format("ExceptionProcess: ", ex.Message));
        }
    }
}
