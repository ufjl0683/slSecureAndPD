using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace test
{
    public class PowerControl
    {
        string ip;
        int port;

        public byte status;

        ModbusTCP.Master client;
        public PowerControl(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
            new System.Threading.Thread(ReadingTask).Start();
            //  client = new ModbusTCP.Master();
        }


       
        public bool IsConnected
        {
            get
            {
                if (client == null)
                    return false;
                else
                    return client.connected;
            }
        }

        void MakeConnect()
        {

            client = new ModbusTCP.Master();
          //  client.OnException += client_OnException;
            client.connect(ip, (ushort)port);
            Console.WriteLine("try COnnect");

        }

        void CloseConnection()
        {
            try
            {
                
                client.disconnect();
            }
            catch { ;}
            try
            {
                client.Dispose();
            }
            catch { ;}
            //throw new NotImplementedException();
        }

        public void SwitchPower(bool onoff)
        {
            byte[] data=null;
            if (client.connected)
            {
                lock(this)
                client.WriteSingleCoils(1, 1, 16, onoff, ref data);

            }
        }

        void ReadingTask()
        {
            while (true)
            {
                try
                {
                    if (client == null || !client.connected)
                    {

                        MakeConnect();
                    }

                    if (client.connected)
                    {
                        //do job here
                        Console.WriteLine("do job");

                        byte[] data = null;
                        lock (this)
                            client.ReadCoils(1, 1, 0, 1, ref data);
                        if (data != null)
                        {
                            status = data[0];
                            Console.WriteLine(data[0]);
                        }
                        else
                            CloseConnection();
                    }

                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message + "," + ex.StackTrace);
                    CloseConnection();

                }
                System.Threading.Thread.Sleep(5000);
            }

        }

    }
}

