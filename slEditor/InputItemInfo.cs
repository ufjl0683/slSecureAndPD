﻿using System;
using System.Net;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;


namespace   slSecure.Web
{

    //public partial class tblControllerConfig
    //{
    //    //public string DoorColorString
    //    //{

    //    //    get
    //    //    {
    //    //        return "Green";
    //    //    }
    //    //    set
    //    //    {
               

    //    //        // return;
    //    //    }
    //   // }
    //}
    public class InputItemInfo 
    {
       
            public int ItemID { get; set; }
            public string Type { get; set; }
            public object Value { get; set; }
            public string Label { get; set; }
            public string Unit { get; set; }
            public int Degree { get; set; }

            public double Rotation { get; set; }
            public double ScaleX { get; set; }
            public double ScaleY { get; set; }
            public string Content { get; set; }
             
        
    }

  
}
