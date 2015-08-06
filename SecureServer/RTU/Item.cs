using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace SecureServer
{
    public delegate void ItemValueChangeEventHandler(Item sender,double NewValue);
    public delegate void ItemDegreeChangeEventHandler(Item sender, int? NewValue);
   public  class Item
    {
       ModbusTCP.IRTU rtu;
      internal  tblItemConfig ItemConfig;
       public int ItemID { get; set; }
       public string ItemType { get; set; }
       public event ItemValueChangeEventHandler ItemValueChanged;
       public event ItemDegreeChangeEventHandler ItemDegreeChanged;
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


       int? _Degree;
       public int? Degree
       {
           get
           {
               return _Degree;
           }
           set
           {
               if (value != _Degree)
               {

                   _Degree = value;
                  
               }
           }
       }


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

       public Item(int ItemID,ModbusTCP.IRTU rtu,string ItemType,tblItemConfig ItemConfig)
       {
           this.rtu = rtu;
           this.ItemConfig = ItemConfig;
           this.ItemType = ItemType;
           this.ItemID = ItemID;
           this._Value = ItemConfig.Value??0;
           this._Degree = ItemConfig.Degree;
           Task task = Task.Run(new Action(ReadindAction));
       }

       public void LoadItemConfig()
       {
           SecureDBEntities1 client = new SecureDBEntities1();
          tblItemConfig config= client.tblItemConfig.Where(n => n.ItemID == this.ItemID).FirstOrDefault();
           if(config!=null)
                 this.ItemConfig = config;
           if (this.ItemValueChanged != null)
               this.ItemValueChanged(this, this.Value);
          
       }
       string ColorString(string code)
       {

           string ret = "Black";
           switch (code)
           {
               case "R":
                   ret = "Red";
                   break;
               case "G":
                   ret = "Green";
                   break;
               case "Y":
                   ret = "Yellow";
                   break;
           }

           return ret;
       }
       public string Lv0Color { get {return ColorString(this.ItemConfig.Lv0Color) ;} }
       public string Lv1Color { get { return ColorString(this.ItemConfig.Lv1Color); } }
       public string Lv2Color { get { return ColorString(this.ItemConfig.Lv2Color); } }

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
               bool IsDegreeChange = false;
               System.Threading.Thread.Sleep(1000);
               IsDegreeChange = false;
               if (Program.MyServiceObject == null)
                   continue;
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
                               {
                                   if (this.Degree != 0)
                                       IsDegreeChange = true;
                                   Degree = 0;
                               }
                               else if (value <= ItemConfig.AlarmUpper && value >= ItemConfig.AlarmLower)
                               {
                                   if (this.Degree != 1)
                                       IsDegreeChange = true;
                                   Degree = 1;
                               }
                               else
                               {
                                   if (this.Degree != 2)
                                       IsDegreeChange = true;
                                   Degree = 2;

                               }

                               Value = value;

                               if (this.ItemDegreeChanged != null  && IsDegreeChange)
                                   ItemDegreeChanged(this, Degree);
                           
                               break;
                          
                           case "DI":
                             
                               value =(double) reading;
                               //if (ItemConfig.Address == 2001)
                               //    Console.WriteLine("test");
                               if ((System.Convert.ToInt32(reading) & (1 << ItemConfig.BitNo)) > 0)
                               
                                   value = 1;
                               else
                                   value = 0;

                             

                               if (ItemConfig.DIInvokeWarningValue == System.Convert.ToInt32(value))
                               {
                                   if (this.Degree != 2)
                                       IsDegreeChange = true;
                                   this.Degree = 2;
                               }
                               else
                               {
                                   if (this.Degree != 0)
                                       IsDegreeChange = true;
                                   this.Degree = 0;

                               }

                               Value = value;
                               if (this.ItemDegreeChanged != null && IsDegreeChange )
                                   ItemDegreeChanged(this, Degree);
                               break;

                           case "DO":
                               if (System.Convert.ToInt32(reading)>0 /*& (1 << ItemConfig.BitNo)) > 0*/)
                               {
                                   value = 1;
                                   Degree = 2;
                                  
                               
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

                   data.ColorString =Lv0Color;
                   break;
               case 1:
                    data.ColorString = Lv1Color;
                   break;
               case 2:
                      data.ColorString = Lv2Color;
                   break;
               default:
                   data.ColorString = "Gray";
                   break;

           }

           if (this.Degree > 0 && ItemConfig.AlarmMode == "Y")
           {
               data.IsAlarm = true;
           }
           else
               data.IsAlarm = false; 

           if (!this.rtu.IsConnected)
           {
               data.ColorString = "Gray";
               data.IsAlarm = false; 
               //data.Content = "斷線";
           }
           //else
           //{
               try
               {
                   if (this.ItemType == "AI")
                       data.Content = this.ItemConfig.Lable + string.Format("{0:0.0}", this.Value) + ItemConfig.Unit;
                   else
                   {
                       if (this.Degree > 0 && ItemConfig.AlarmMode == "Y")
                           data.Content = this.ItemConfig.AlarmContent;
                       else
                           data.Content = this.ItemConfig.Lable;
                   }
               }
               catch (Exception ex)
               {
                   data.Content = this.ItemID + ",Generate content error!";
               }
           //}

           data.PlaneID = this.ItemConfig.tblItemGroup.PlaneID??0;
           data.Value = this.Value;

           if (ItemConfig.Suppress ?? false)
               data.IsAlarm =false;

           return data;

       }


    }
}
