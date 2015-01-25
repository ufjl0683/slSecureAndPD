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
                 output = 1;// (ushort)((ushort)Value | (1 << ItemConfig.BitNo));
           }
           else
           {
               output = 0;// (ushort)((ushort)Value & (~(1 << ItemConfig.BitNo)));
           }
           rtu.WriteRegister((ushort)(this.ItemConfig.Address), output);
       }

       public string AlarmMode
       {

           get
           {
               return this.ItemConfig.AlarmMode;
           }
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
                   double value=0;
                   if (reading != null)
                   {
                       switch (this.ItemType)
                       {
                           case "AI":
                               value = (int)reading * ItemConfig.ValueScale * ItemConfig.Coefficient + ItemConfig.Offset;
                               if (value <= ItemConfig.WarningUpper && value >= ItemConfig.WarningLower)
                                   Degree = 0;
                               else if (value <= ItemConfig.AlarmUpper && value >= ItemConfig.AlarmLower)
                                   Degree = 1;
                               else
                                   Degree = 2;


                           
                               break;
                          
                           case "DI":
                               value =(double) reading;
                               //if (ItemConfig.Address == 2001)
                               //    Console.WriteLine("test");
                               if ((System.Convert.ToInt32(reading) & (1 << ItemConfig.BitNo)) > 0)
                               
                                   value = 1;
                               else
                                   value = 0;

                               if (ItemConfig.DIInvokeWarningValue == System.Convert.ToInt32(value) )
                                   this.Degree = 2;
                               else
                                   this.Degree = 0;
                               break;
                           case "DO":
                               if (System.Convert.ToInt32(reading)>0 /*& (1 << ItemConfig.BitNo)) > 0*/)
                               {
                                   Degree = 2;
                                   value = 1;
                               
                               }
                               else
                               {
                                   value = 0;
                                   Degree = 0;
                                 
                               }
                               break;
 

                         
                       }

                       Value = value;

                //      Console.WriteLine("ItemID:"+this.ItemConfig.ItemName + ",Value:" + Value+",Degree:"+Degree);
                   }



               }
               catch (Exception ex)
               {
                   Console.WriteLine("RTU:{0},address:{1}",rtu.ControlID,ItemConfig.Address);
                   Console.WriteLine(ex.Message + "," + ex.StackTrace);



               }
           }
       }

       public BindingData.ItemBindingData ToBindingData()
       {
           BindingData.ItemBindingData data = new BindingData.ItemBindingData() { ItemID = this.ItemID, Type = this.ItemType, GroupID=this.ItemConfig.GroupID, Degree=this.Degree??0 };
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
                   data.ColorString = "Gray";
                   break;

           }

           if (this.Degree > 0 && ItemConfig.AlarmMode == "Y")
               data.IsAlarm = true;
           else
               data.IsAlarm = false; 

           if (!this.rtu.IsConnected)
           {
               data.ColorString = "Gray";
               data.Content = "斷線";
           }
           else
           {
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
           }

           data.PlaneID = this.ItemConfig.tblItemGroup.PlaneID??0;
           data.Value = this.Value;
           return data;

       }


    }
}
