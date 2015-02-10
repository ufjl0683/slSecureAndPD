using System;
namespace ModbusTCP
{
   public interface IRTU
    {
        string ControlID { get; set; }
        int DevID { get; set; }
        int? GetRegisterReading(ushort RTUAddress);
        string IP { get; set; }
        bool IsConnected { get; }
        int Port { get; set; }
        ushort RegisterLength { get; set; }
        string ToString();
        void WriteRegister(ushort address, ushort data);
    }
}
