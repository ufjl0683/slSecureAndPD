using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SecureServer
{
    public delegate void ItemValueChangeEventHandler(Item sender,double NewValue);
   public  class Item
    {
       ModbusTCP.RTU rtu;
       tblItemConfig ItemConfig;
       public int ItemID { get; set; }
       public string ItemType { get; set; }
       public event ItemValueChangeEventHandler ItemValueChanged;
       private double _Value;
       public double Value {
           get
           {
               return _Value;
           }

           set
           {
               if (value != _Value)
               {
                   _Value = value;
                   if (ItemValueChanged != null)
                       ItemValueChanged(this, Value);


               }
           }
           
           
            }
       public int? Degree { get; set; }


       public int PlaneID
       {
           get
           {
               
               return this.ItemConfig.tblItemGroup.PlaneID??0;
           }
       }
       public bool IsConnected
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
           Task task = Task.Run(new Action(ReadindAction));
       }

       public  void  SetDOValue(bool onoff)
       {
           if(this.ItemType!="DO")
               return;
             ushort output=0;
           if (onoff)
           {
                 output = (ushort)((ushort)Value | (1 << ItemConfig.BitNo));
           }
           else
           {
                 output = (ushort)((ushort)Value & (~(1 << ItemConfig.BitNo)));
           }
           rtu.WriteRegister((ushort)(this.ItemConfig.Address), output);
       }

       void ReadindAction()
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
                          
                           case "DI":
                               if ((System.Convert.ToInt32(reading) & (1 << ItemConfig.BitNo)) > 0)
                                   Value = 1;
                               else
                                   Value = 0;

                               if (ItemConfig.DIInvokeWarningValue == System.Convert.ToInt32(Value))
                                   this.Degree = 2;
                               else
                                   this.Degree = 0;
                               break;
                           case "DO":
                               if ((System.Convert.ToInt32(reading) & (1 << ItemConfig.BitNo)) > 0)
                               {
                                   Value = 1;
                                   Degree = 2;
                               }
                               else
                               {
                                   Degree = 0;
                                   Value = 0;
                               }
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

       public BindingData.ItemBindingData ToBindingData()
       {
           BindingData.ItemBindingData data = new BindingData.ItemBindingData() { ItemID = this.ItemID, Type = this.ItemType };
           switch (this.Degree)
           {
               case 0:
                   data.ColorString = "Green";
                   break;
               case 1:
                    data.ColorString = "Yellow";
                   break;
               case 2:
                      data.ColorString = "Red";
                   break;
               default:
                   data.ColorString = "Grey";
                   break;

           }

           try
           {
               if (this.ItemType == "AI")
                   data.Content = this.ItemConfig.Lable + string.Format("{0:0.0}", this.Value) + ItemConfig.Unit;
               else
                   data.Content = this.ItemConfig.Lable;
           }
           catch (Exception ex)
           {
               data.Content = this.ItemID + ",Generate content error!";
           }

           data.PlaneID = this.ItemConfig.tblItemGroup.PlaneID??0;
           data.Value = this.Value;
           return data;

       }


    }
}
