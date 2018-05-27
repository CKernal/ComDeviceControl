using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComDeviceControl;
using ComDeviceControl.ComDevice;
using System.IO.Ports;

namespace ComDeviceTest
{
    class Program
    {
        static void Main(string[] args)
        {
            IComDevice comDevice = new ComDevice();

            comDevice.SerialDevice = new SerialPort
            {
                PortName = "COM1",
                BaudRate = 9600,
                Parity = Parity.None,
                DataBits = 8,
                StopBits = StopBits.One,
            };

            comDevice.Protocol = new ComDeviceProtocol
            {
                StartCommand = new byte[] { 0x02, 0x55, 0x0d, 0x0a },
                StopCommand = new byte[] { 0x02, 0x54, 0x0d, 0x0a },
                ErrorString = "error",
                ReceiveDataSize = 20,
                StartString = "S",
                EndString = "E",
                DataStartIndex = 1,
                DataLength = 10,
                TimeOut = 1000 * 10,
            };


            if (comDevice.Open())
            {
                Console.WriteLine("Open sucessful.");
                while (true)
                {
                    Console.WriteLine(DateTime.Now.ToString() + " => Start...");
                    var ret = comDevice.GetData();

                    if (ret.Item1)
                    {
                        Console.WriteLine("Recived OK :{0}", ret.Item2);
                    }
                    else
                    {
                        Console.WriteLine("Recived NG :{0}", ret.Item2);
                    }

                    System.Threading.Thread.Sleep(1);
                }
            }
            else
            {
                Console.WriteLine("Open failed.");
            }


            Console.ReadKey();
        }
    }
}
