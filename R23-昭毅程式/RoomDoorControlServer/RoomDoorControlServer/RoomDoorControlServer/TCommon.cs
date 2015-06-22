using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace RoomDoorControlServer
{  
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    struct SNCUInfo
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] MACID;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] IP;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] SubMask;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] GateWay;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] Version;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] Date;
        public byte ItemCount;
    }

    [StructLayoutAttribute(LayoutKind.Sequential, Pack = 1)]
    struct SDeviceInfo
    {
        public Int16 DeviceID;
        public byte DeviceType;
    }

    [StructLayoutAttribute(LayoutKind.Sequential, Pack = 1)]
    struct SCardFormat
    {
        public int iType;
        public int iCompressed;
        public int iCardLen;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public char[] cCardNo;
        public int iPassLen;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        public char[] cPassword;
        public int iDisplayLen;
        public int iDisplayType;
        public int iDisplayID;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public char[] cDisplayText;
        public int iLimitDate;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public char[] cStartDate;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public char[] cEndDate;
        public int iHoliday;
        public int iGroupType;
        public int iGroup;
        public int iDoors;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public byte[] cTimeZone;
    }

    [StructLayoutAttribute(LayoutKind.Sequential, Pack = 1)]
    struct STest
    {
        public int id;
        public byte type;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public char[] name;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public byte[] des;
    }

    [StructLayoutAttribute(LayoutKind.Sequential, Pack = 1)]
    struct PollList
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public byte[] EventCode;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public byte[] DateTime;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public byte[] Card;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] DeviceID;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] ReaderID;
    }

    class TCommon
    {
        

        /// <summary>
        /// convert byte array to struct
        /// </summary>
        public static object BytesToStuct(byte[] bytes, Type type)
        {

            int size = Marshal.SizeOf(type);
            if (size > bytes.Length)
            {
                return null;
            }
            //分配記憶體空間
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            Marshal.Copy(bytes, 0, structPtr, size);
            object obj = Marshal.PtrToStructure(structPtr, type);
            //釋放記憶體空間
            Marshal.FreeHGlobal(structPtr);
            return obj;
        }

        public static object BytesToStuct(byte[] bytes, Type type, int startPosition)
        {
            int size = Marshal.SizeOf(type);
            byte[] buffer = new byte[size];
            for (int i = 0; i < size; i++)
            {
                buffer[i] = bytes[startPosition + i];
            }
            object obj = BytesToStuct(buffer, type);
            return obj;
        }

        /// <summary>
        /// convert byte array to struct array
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="type"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static object[] BufferToStuct(byte[] bytes, Type type, int startPos, int amount)
        {
            int size = Marshal.SizeOf(type);
            byte[] buffer = new byte[size];

            //account total of type
            int objTotal;
            if (amount != 0)
            {
                if (amount < ((bytes.Length - startPos) / size))
                {
                    objTotal = amount;
                }
                else
                {
                    objTotal = (bytes.Length - startPos) / size;
                }
            }
            else
            {
                objTotal = (bytes.Length - startPos) / size;
            }
            object[] objList = new object[objTotal];

            //full data
            for (int i = 0; i < objTotal; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    buffer[j] = bytes[startPos + i * size + j];
                }
                object obj = BytesToStuct(buffer, type);
                objList[i] = obj;
            }
            return objList;
        }

        /// <summary>
        /// convert struct to byte array
        /// </summary>
        public static byte[] StructToBytes(object structObj)
        {
            int size = Marshal.SizeOf(structObj);
            byte[] bytes = new byte[size];
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(structObj, structPtr, false);
            Marshal.Copy(structPtr, bytes, 0, size);
            Marshal.FreeHGlobal(structPtr);
            return bytes;
        }

        public static string ByteArrayToString(byte[] buffer)
        {
            StringBuilder sb = new StringBuilder(buffer.Length);
            for (int i = 0; i < buffer.Length; i++)
            {
                char c = Convert.ToChar(buffer[i]);
                if (c != '\0')
                    sb.Append(c);
            }
            return sb.ToString();
        }

        public static string ByteArrayToString(byte[] buffer,int StartPoint,int Length)
        {
            StringBuilder sb = new StringBuilder(buffer.Length);
            int End = StartPoint + Length;
            for (int i = StartPoint; i < End && i < buffer.Length; i++)
            {
                char c = Convert.ToChar(buffer[i]);
                if (c != '\0')
                    sb.Append(c);

            }
            return sb.ToString();
        }

        public static string ByteArrayToString(byte[] buffer, int length)
        {
            string sBuffer = "";
            for (int i = 0; i < length; i++)
            {
                sBuffer += Convert.ToChar(buffer[i]);
            }
            return sBuffer;
        }

        public static int BCDByteToInt(byte bcd)
        {
            return (bcd >> 4) * 10 + ((byte)(bcd << 4) >> 4);
        }

        public static string GetWEG(string ABA)
        {
            int aba = Convert.ToInt32(ABA);
            int weg1 = aba / 65536;
            int weg2 = aba % 65536;
            return weg1.ToString("00000") + weg2.ToString("00000");
        }

        public static string GetABA(string WEG)
        {
            if (WEG.Length > 10)
                throw new Exception("WEG Code legth > 10");
            while (WEG.Length < 10)
            {
                WEG = "0" + WEG;
            }
            int weg1 = Convert.ToInt32(WEG.Substring(0, 5));
            int weg2 = Convert.ToInt32(WEG.Substring(5));
            int aba = weg1 * 65536 + weg2;
            return aba.ToString("0000000000");
        }

        public static bool ByteArrayEquel(byte[] Array1, byte[] Array2)
        {
            if (Array1.Length != Array2.Length)
                return false;
            for (int i = 0; i < Array1.Length; i++)
            {
                if (Array1[i] != Array2[i])
                    return false;
            }
            return true;
        }

        public static void SaveLog(string Message)
        {
            try
            {
                string text = DateTime.Now.ToString() + " " + Message + "\r\n";
                lock (System.Text.ASCIIEncoding.UTF8)
                {
                    System.IO.File.AppendAllText(@".\log\Error_" + DateTime.Now.ToString("yyyyMMdd") + ".log", text, System.Text.ASCIIEncoding.UTF8);
                }
            }
            catch
            {
            }
        }

        public static List<string> GetAdderList(List<string> oldlist, List<string> newlist)
        {
            List<string> addList = new List<string>();
            List<string> compList = new List<string>(oldlist);
            foreach (string str in newlist)
            {
                if (!compList.Contains(str))
                {
                    addList.Add(str);
                }
                else
                    compList.Remove(str);
            }
            return addList;
        }

    }   
}
