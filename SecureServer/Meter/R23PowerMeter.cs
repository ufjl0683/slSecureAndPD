using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureServer.Meter
{

    public enum Address
    {
        VA = 2351 - 1,
        VB = 2352 - 1,
        VC = 2353 - 1,
        AVGV = 2354 - 1,
        IA = 2359 - 1,
        IB = 2360 - 1,
        IC = 2361 - 1,
        AVGI = 2362 - 1,
        KW = 2367 - 1,
        PF = 2379 - 1,
        CumulateValue = 2301 - 1,
        InstantaneousValue = 2303 - 1

    }
    public class R23PowerMeter
    {
        string ip;
        int port;
        byte[] data = new byte[29 * 2];
        System.Threading.Timer tmr;
        public R23PowerMeter(int erid, string ip, int port)
        {
            this.ip = ip;
            this.port = port;
            this.ERID = erid;
            tmr = new System.Threading.Timer(TmrCallBack);
            tmr.Change(0, 10 * 60 * 1000);
            TmrCallBack(null);
        }


        public int ERID { get; set; }
        bool intmr = false;
        void TmrCallBack(object a)
        {
            if (intmr)
                return;
            try
            {
                intmr = true;
                GetAllData();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "," + ex.StackTrace);
            }
            finally
            {
                intmr = false;
            }

        }
        void GetAllData()
        {
            ModbusTCP.Master master = new ModbusTCP.Master();
            //   data = new byte[29 * 2];
            byte[] tdata = null;
            try
            {
                master.connect(ip, (ushort)port);
                master.ReadHoldingRegister(1, 0, (ushort)(Address.VA), 29, ref tdata);
                if (tdata != null)
                    data = tdata;
                byte[] temp = new byte[4];
                byte[] dest = new byte[4];
                master.ReadHoldingRegister(1, 0, (ushort)(Address.CumulateValue), 2, ref temp);
                if (temp != null)
                {
                    dest[0] = temp[1];
                    dest[1] = temp[0];
                    dest[2] = temp[3];
                    dest[3] = temp[2];
                    CumulateValue = System.BitConverter.ToSingle(dest, 0);
                }

                //this.data = data;
                master.ReadHoldingRegister(1, 0, (ushort)(Address.InstantaneousValue), 2, ref temp);
                if (temp != null)
                {
                    dest[0] = temp[1];
                    dest[1] = temp[0];
                    dest[2] = temp[3];
                    dest[3] = temp[2];
                    InstantaneousValue = System.BitConverter.ToSingle(dest, 0);
                }

            }
            catch
            {
                //  data = null;
                Console.WriteLine(master.connected);

            }
            finally
            {
                master.Dispose();
            };
        }

        public bool IsValid
        {
            get
            {
                return !(data == null);
            }
        }

        public double CumulateValue
        {
            get;
            set;
        }

        public double InstantaneousValue
        {
            get;
            set;
        }


        public double VA
        {
            get
            {
                if (!IsValid)
                    return -1;
                else
                    return value((int)Address.VA) * 0.01;
            }
        }

        public double VB
        {
            get
            {
                if (!IsValid)
                    return -1;
                else
                    return value((int)Address.VB) * 0.01;
            }
        }

        public double VC
        {
            get
            {
                if (!IsValid)
                    return -1;
                else
                    return value((int)Address.VC) * 0.01;
            }
        }

        public double AVGV
        {
            get
            {
                if (!IsValid)
                    return -1;
                else
                    return value((int)Address.AVGV) * 0.01;
            }
        }


        public double IA
        {
            get
            {
                if (!IsValid)
                    return -1;
                else
                    return value((int)Address.IA) * 0.01;
            }
        }

        public double IB
        {
            get
            {
                if (!IsValid)
                    return -1;
                else
                    return value((int)Address.IB) * 0.01;
            }
        }

        public double IC
        {
            get
            {
                if (!IsValid)
                    return -1;
                else
                    return value((int)Address.IC) * 0.01;
            }
        }


        public double AVGI
        {
            get
            {
                if (!IsValid)
                    return -1;
                else
                    return value((int)Address.AVGI) * 0.01;
            }
        }

        public double KW
        {
            get
            {
                if (!IsValid)
                    return -1;
                else
                    return value((int)Address.KW);
            }
        }
        public double PF
        {
            get
            {
                if (!IsValid)
                    return -1;
                else
                    return value((int)Address.PF) * 0.001;
            }
        }


        int value(int address)
        {
            return data[(address - (int)Address.VA) * 2] * 256 + data[(address - (int)Address.VA) * 2 + 1];
        }

    }
}
