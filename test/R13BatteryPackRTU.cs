using ModbusTCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SecureServer.RTU
{
    //(?<omh>[0-9]+.[0-9]+m&Omega;)|(?<temp>[0-9]+.[0-9]+&deg;C)|(?<v>[0-9]+.[0-9]+\s*V)
    public class R13BatteryPackRTU : IRTU
    {
        public string IP { get; set; }
        public int Port { get; set; }
        public ushort RegisterLength { get; set; }
        public string ControlID { get; set; }
        public int DevID { get; set; }
        public ushort StartAddress;
        Master RTUDevice;
        System.Threading.Timer tmr;
        byte[] data;

        object lockobj = new object();
        public R13BatteryPackRTU(string ControlID, int DevID, string IP, int Port, int StartAddress, int RegisterLength, int comm_state)
        {
            this.StartAddress = (ushort)StartAddress;
            data = new byte[RegisterLength * 2];
            Console.WriteLine(ControlID + ",DataLength:" + data.Length);
            this.ControlID = ControlID;
            this.IP = IP;
            this.Port = Port;
            this.RegisterLength = (ushort)RegisterLength;
            this.DevID = DevID;
            _Comm_state = comm_state;
            //new Thread(ConnectTask).Start();
            //tmr = new System.Threading.Timer(new System.Threading.TimerCallback(timerBack));
            //tmr.Change(0, 3000);
            data = new byte[RegisterLength * 2];
            new Thread(ReadingTask).Start();

        }
        public bool IsConnected
        {
            get
            {
                if (this.Comm_state == 1)
                    return true;
                else
                    return false;
            }
        }

        int _Comm_state;

        int Comm_state
        {
            get
            {
                return _Comm_state;
            }
            set
            {
                if (value != _Comm_state)
                {
                    _Comm_state = value;
                    if (OnCommStateChanged != null)
                        OnCommStateChanged(this, value);
                }
            }
        }
        void ReadingTask()
        {
            while (true)
            {
                try
                {
                    System.Net.WebClient client = new System.Net.WebClient();
                    string s = "";
                    try
                    {
                        s = client.DownloadString("http://" + IP + ":" + Port + "/stringindex?0_0");
                        Comm_state = 1;
                    }
                    catch
                    {
                        Comm_state = 0;
                        continue;
                    }

                  //Regex regex = new Regex(@">(?<v>[0-9]+.[0-9]+)\s*V|>(?<temp>[0-9]+.[0-9]+)&deg;C");
                    Regex regx = new Regex(@"String Voltage :\s*(?<v>[0-9]+.[0-9]+)\s*V");
                    
                    double volt = double.Parse(regx.Match(s).Groups[1].Value);

                    data[0] = (byte)((volt * 100) / 256);
                    data[1] = (byte)((volt * 100) % 256);

                   
                    regx = new Regex(@"String Current :\s*(?<v>-*[0-9]+.[0-9]+)\s*A");

                    double a = double.Parse(regx.Match(s).Groups[1].Value);
                    data[2] = (byte)((a * 100) / 256);
                    data[3] = (byte)((a * 100) % 256);
                    Regex regex = new Regex(@">(?<v>[0-9]+.[0-9]+)\s*V|>(?<temp>[0-9]+.[0-9]+)&deg;C");
                    MatchCollection collection = regex.Matches(s);
                    byte[] temp = new byte[2];
                    for (int i = 0; i < collection.Count; i++)
                    {
                        double val=0;
                        try
                        {
                            val = double.Parse(collection[i].Groups[(i % 2) + 1].Value);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(collection[i].Groups[(i % 2) + 1].Value + ",Parse error!");
                        }
                        temp[0] = (byte)(((short)(val * 100)) / 256);
                        temp[1] = (byte)(((short)(val * 100)) % 256);
                        Array.Copy(temp, 0, data, 4+i * 2, 2);
                        //  Console.WriteLine(collection[i].Groups[(i % 2) + 1].Value);

                    }
                    Console.WriteLine(collection.Count);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + "," + ex.StackTrace);
                }
                finally
                {
                    System.Threading.Thread.Sleep(3000);
                }

            }

        }

        public void WriteRegister(ushort address, ushort data)
        {
            throw new NotImplementedException();
        }

        public event OnCommStateChangedHandler OnCommStateChanged;
        public int? GetRegisterReading(ushort RTUAddress)
        {
            int address = RTUAddress;
            return data[(address - StartAddress) * 2] * 256 + data[(address - StartAddress) * 2 + 1];
        }
    }
}
