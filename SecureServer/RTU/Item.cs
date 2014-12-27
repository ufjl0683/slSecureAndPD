using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SecureServer
{
   public  class Item
    {
       ModbusTCP.RTU rtu;
       tblItemConfig ItemConfig;
       public int ItemID { get; set; }
       public string ItemType { get; set; }
       public double Value { get; set; }
       public int? Degree { get; set; }
       public bool IsConnecte
       {

           get
           {
               return rtu.IsConnected;
           }
       }

       public Item(int ItemID,ModbusTCP.RTU rtu,string ItemType,tblItemConfig ItemConfig)
       {
           this.rtu = rtu;
           this.ItemConfig = ItemConfig;
           this.ItemType = ItemType;
           this.ItemID = ItemID;
           Task task = Task.Run(new Action(ReadDingAction));
       }

       void ReadDingAction()
       {
           while (true)
           {
               System.Threading.Thread.Sleep(1000);
               try
               {
                   if (rtu == null)
                       continue;


                   int? reading = rtu.GetRegisterReading((ushort)ItemConfig.Address);

                   if (reading != null)
                   {
                       switch (this.ItemType)
                       {
                           case "AI":
                               Value = (int)reading * ItemConfig.ValueScale * ItemConfig.Coefficient + ItemConfig.Offset;
                               if (Value <= ItemConfig.WarningUpper && Value >= ItemConfig.WarningLower)
                                   Degree = 0;
                               else if (Value <= ItemConfig.AlarmUpper && Value >= ItemConfig.AlarmLower)
                                   Degree = 1;
                               else
                                   Degree = 2;
                               break;
                         
                       }

                       Console.WriteLine("ItemID"+this.ItemID + ",Value:" + Value+",Degree:"+Degree);
                   }



               }
               catch (Exception ex)
               {
                   Console.WriteLine(ex.Message + "," + ex.StackTrace);



               }
           }
       }


    }
}
