using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureServer.PD
{
    public  class PDStatus
    {
        byte[] data;
        public PDStatus(byte[] data)
        {
            this.data = data;
        }

        public int R0
        {
            get
            {
                return (System.BitConverter.ToUInt16(data, 0)) & 0x01;
            }
        }

        public int S0
        {
            get
            {
                return (System.BitConverter.ToUInt16(data, 0) >> 1) & 0x01;
            }
        }
        public int T0
        {
            get
            {
                return (System.BitConverter.ToUInt16(data, 0) >> 2) & 0x01;
            }
        }
        public int R1
        {
            get
            {
                return (System.BitConverter.ToUInt16(data, 0) >> 3) & 0x01;
            }
        }

        public int S1
        {
            get
            {
                return (System.BitConverter.ToUInt16(data, 0) >> 4) & 0x01;
            }
        }
        public int T1
        {
            get
            {
                return (System.BitConverter.ToUInt16(data, 0) >> 5) & 0x01;
            }
        }

        public int L0
        {
            get
            {
                return (System.BitConverter.ToUInt16(data, 0) >> 6) & 0x01;
            }
        }
        public int L1
        {
            get
            {
                return (System.BitConverter.ToUInt16(data, 0) >> 7) & 0x01;
            }
        }

        public int L2
        {
            get
            {
                return (System.BitConverter.ToUInt16(data, 0) >> 8) & 0x01;
            }
        }
        public int L3
        {
            get
            {
                return (System.BitConverter.ToUInt16(data, 0) >> 9) & 0x01;
            }
        }
        public int L4
        {
            get
            {
                return (System.BitConverter.ToUInt16(data, 0) >> 10) & 0x01;
            }
        }
        public int Cabinet
        {
            get
            {
                return (System.BitConverter.ToUInt16(data, 0) >> 11) & 0x01;
            }
        }

    }
}
