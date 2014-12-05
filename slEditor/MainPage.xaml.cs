using Common;
using slSecure;
using slSecure.Controls;
using slSecure.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.DomainServices.Client;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace slEditor
{
    public partial class MainPage : UserControl
    {

        slSecure.Web.SecureDBContext db;
        Control selectedDevice;
        double deltaX, deltaY;  //計算滑鼠落點與元件座標差值

        public MainPage()
        {
            InitializeComponent();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
               db =  slSecure.DB.GetDB();
            var q = from n in db.GetTblERPlaneQuery() select n;


            cbDiagramSelect.ItemsSource = await slSecure.DB.LoadAsync<tblERPlane>(db, q);

            
         //   AI ai = new AI();
         //   tblItem info = CreateInputItemInfo("AI", "AI", "Unit", 99.9, 0);

         //   ai.DataContext = info;
         //   ai.SetBinding(AI.DegreeeProperty, new Binding("Degreee"));
            
         //   this.lstTool.Items.Add(ai);
         //   DI di = new DI();
         //   info = CreateInputItemInfo("DI", "DI", "單位", 100, 0);
         //   di.SetBinding(DI.DegreeeProperty, new Binding("Degreee"));
         //   di.DataContext = info;
         //   this.lstTool.Items.Add(di);

         //   CCTV cctv = new CCTV();
         //   lstTool.Items.Add(cctv);

         //   DOOR door = new DOOR();
         //   lstTool.Items.Add(door);

            

         //  // foreach (UIElement ee in LayoutRoot.Children )
         //  // {
         //  //     ((I_IO)(ee)).IsSelect = true;
         //  // }

         //  //InputItemInfo  info= CreateInputItemInfo("AI", "Voltage", "volts", 100,0);
         
         //  //this.AI.DataContext = info;
         //  //this.AI.SetBinding(AI.DegreeeProperty, new Binding("Degreee"));
         
         ////  info.Degreee = 1;

         //   db = new slSecure.Web.SecureDBContext();
         //   EntityQuery<slSecure.Web.tblSite> q = db.GetTblSiteQuery();
         //   var lo = db.Load(q);
            
         //   lo.Completed += (s, a) =>
         //       {
         //           cbDiagramSelect.ItemsSource = lo.Entities;
         //       };

        }


        //public tblItem CreateInputItemInfo(string type,string label,string unit,object value,int Degreee)
        //{
           
        //    tblItem item= new tblItem() { Label = label, Type = type, Degree = Degreee, Unit = unit ,Value=System.Convert.ToDouble(value),Rotation=0,ScaleX=1,ScaleY=1};

        //    return item;
                 

        //}

        private void scrollViewer1_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (rdNone.IsChecked == true)
            {
                vbImage.Stretch = Stretch.None;
                vbImage.Width = imgPic.Width;
                vbImage.Height = imgPic.Height;
                vbImage.Margin = new Thickness(0);
                vbImage.UpdateLayout();
            }
            else
            {
                vbImage.Width = scrollViewer1.ViewportWidth;
                vbImage.Height = scrollViewer1.ViewportHeight;
                vbImage.Margin = new Thickness(0);
                vbImage.Stretch = Stretch.Uniform;
            }
        }

        private void rdNone_Loaded(object sender, RoutedEventArgs e)
        {
            //slSecure.Web.SecureDBContext db = new slSecure.Web.SecureDBContext();
            //var q = db.GetDiagramList();
           
           
            //q.Completed+=(s,a)=>
            //    {
            //        cbDiagramSelect.ItemsSource = q.Value;
                  
            //    };

           
            
        }

        private void rdUniform_Checked(object sender, RoutedEventArgs e)
        {
            if (vbImage == null) return;

            // vbImage.Width=scrollViewer1.ViewportWidth;
            // vbImage.Height = scrollViewer1.ViewportHeight ;
            // vbImage.Margin = new Thickness(0);
            vbImage.Stretch = Stretch.Uniform;
            scrollViewer1_SizeChanged(null, null);
            //   imgPic.Height = vbImage.ActualHeight;
            //vbImage.UpdateLayout();
        }

        private void rdNone_Checked(object sender, RoutedEventArgs e)
        {
            vbImage.Stretch = Stretch.None;
            scrollViewer1_SizeChanged(null, null);
            //vbImage.Width = imgPic.Width;
            //vbImage.Height = imgPic.Height;
            //vbImage.Margin = new Thickness(0);
            //vbImage.UpdateLayout();
        }

        private void imgPic_MouseMove(object sender, MouseEventArgs e)
        {
            //Point pimage = e.GetPosition(this.imgPic);
            //textBlock2.Text = string.Format("x:{0:0.0},y:{1:0.00}", pimage.X, pimage.Y);
             


            //if (selectedDevice == null)
            //    return;
            //Point p = e.GetPosition(grdDeviceLayer);
            //selectedDevice.SetValue(Grid.MarginProperty,
            //    new Thickness(p.X - deltaX, p.Y - deltaY, 0, 0));
            //(selectedDevice.DataContext as tblDevice).X = (int)(p.X - deltaX);
            //(selectedDevice.DataContext as tblDevice).Y = (int)(p.Y - deltaY);
            //selectedDevice.SetValue(Grid.WidthProperty, 20.0);
            //selectedDevice.SetValue(Grid.HeightProperty, 30.0);



        }

        private async void cbDiagramSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Collections.Generic.List<UIElement> Removelist = new System.Collections.Generic.List<UIElement>();

            int planeid= (cbDiagramSelect.SelectedItem as tblERPlane).PlaneID;
             this.imgPic.Source = new BitmapImage(new Uri("/Diagrams/" + (cbDiagramSelect.SelectedItem as tblERPlane).PlaneID+".png", UriKind.Relative));
             foreach (UIElement c in grdDeviceLayer.Children)
             {
                 if (c is Image) continue;
                 Removelist.Add(c);
                 //grdDeviceLayer.Children.Remove(c);
             }

            foreach(UIElement ui in Removelist)
                grdDeviceLayer.Children.Remove(ui);
            Removelist.Clear();
             #region  Door
             var q = from n in db.GetTblControllerConfigQuery() where (n.ControlType == 1 || n.ControlType == 2  ) && n.PlaneID== (cbDiagramSelect.SelectedValue as tblERPlane).PlaneID   select n;
             var res= await   DB.LoadAsync<tblControllerConfig>(db, q);

             foreach (tblControllerConfig tbl in res)
             {
                DOOR item = new DOOR();
                item.Name = "Door"+tbl.ControlID;
             //  tblItem info = CreateInputItemInfo("DOOR", "DOOR", "", 1, 0);
                
                item.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                item.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                item.SetValue(Grid.MarginProperty, new Thickness(tbl.X??0, tbl.Y??0, 0, 0));
              
              //  item.Margin = new Thickness(0);

              // grdSetting.DataContext = 
                item.DataContext = tbl;
                 
                item.SetDefaultColor();
                

                this.grdDeviceLayer.Children.Add(item);

                item.MouseLeftButtonDown += selectedDevice_MouseLeftButtonDown;
                item.MouseLeftButtonUp += selectedDevice_MouseLeftButtonUp;
                item.MouseMove += selectedDevice_MouseMove;
               // ctl = item;

             }
             #endregion
            #region          CCTV
             var q2 = from n in db.GetTblCCTVConfigQuery() where n.PlaneID == planeid select n;
             var res2 = await DB.LoadAsync<tblCCTVConfig>(db, q2);
             foreach (tblCCTVConfig tbl in res2)
             {
                 CCTV item = new CCTV();
                 item.Name = "CCTV" + tbl.CCTVID;
                 item.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                 item.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                 item.SetValue(Grid.MarginProperty, new Thickness(tbl.X, tbl.Y, 0, 0));
                 //  item.Margin = new Thickness(0);

                 //  grdSetting.DataContext = 
                 item.DataContext = tbl;



                 this.grdDeviceLayer.Children.Add(item);

                 item.MouseLeftButtonDown += selectedDevice_MouseLeftButtonDown;
                 item.MouseLeftButtonUp += selectedDevice_MouseLeftButtonUp;
                 item.MouseMove += selectedDevice_MouseMove;
             }

            #endregion

        }

        private void AddItem_Click(object sender, RoutedEventArgs e)
        {

            //Control ctl=null;
            //if (this.lstTool.SelectedItem == null)
            //    return;

            //if (this.lstTool.SelectedItem is AI)
            //{
            //    AI item = new AI();
            //    item.Name = "AI" + Guid.NewGuid();
            //    tblItem info = CreateInputItemInfo("AI", "AI", "Unit", 99.9, 0);
               
            //    item.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            //    item.VerticalAlignment = System.Windows.VerticalAlignment.Top;
               
            //    item.Margin = new Thickness(0);
               
            //    grdSetting.DataContext=  item.DataContext = info;
             

               
            //    this.grdDeviceLayer.Children.Add(item);
             
            //   ctl=item;
                
                
            //  //  (grdSetting.DataContext as InputItemInfo).ScaleX = 3;
            //}
            //else if (this.lstTool.SelectedItem is DI)
            //{

            //    DI item = new DI();
            //    item.Name = "DI" + Guid.NewGuid();
            //    tblItem info = CreateInputItemInfo("DI", "DI", "", 1, 0);

            //    item.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            //    item.VerticalAlignment = System.Windows.VerticalAlignment.Top;

            //    item.Margin = new Thickness(0);

            //    grdSetting.DataContext = item.DataContext = info;



            //    this.grdDeviceLayer.Children.Add(item);

            //    ctl = item;
            //}

            //else if (this.lstTool.SelectedItem is CCTV)
            //{
            //    CCTV item = new CCTV();
            //    item.Name = "CCTV" + Guid.NewGuid();
            //   // item.Url = "http://192.192.85.33:201/axis-cgi/mjpg/video.cgi";
            //    item.Url = "http://192.192.85.33:201/mjpg/video.mjpg";
            //    tblItem info = CreateInputItemInfo("CCTV", "CCTV", "", 1, 0);

            //    item.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            //    item.VerticalAlignment = System.Windows.VerticalAlignment.Top;

            //    item.Margin = new Thickness(0);

            //    grdSetting.DataContext = item.DataContext = info;


               
            //    this.grdDeviceLayer.Children.Add(item);

            //    ctl = item;
            //}

            //else if (this.lstTool.SelectedItem is DOOR)
            //{
            //    DOOR item = new DOOR();
            //    item.Name = "DOOR" + Guid.NewGuid();
            //    tblItem info = CreateInputItemInfo("DOOR", "DOOR", "", 1, 0);

            //    item.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            //    item.VerticalAlignment = System.Windows.VerticalAlignment.Top;

            //    item.Margin = new Thickness(0);

            //    grdSetting.DataContext = item.DataContext = info;



            //    this.grdDeviceLayer.Children.Add(item);

            //    ctl = item;
            //}



            //    ctl.MouseLeftButtonDown += selectedDevice_MouseLeftButtonDown;
            //    ctl.MouseLeftButtonUp += selectedDevice_MouseLeftButtonUp;
            //    ctl.MouseMove += selectedDevice_MouseMove;
        }

        void selectedDevice_MouseMove(object sender, MouseEventArgs e)
        {



            if (selectedDevice == null)
                return;
            Point p = e.GetPosition(grdDeviceLayer);
           // txtBlock2.Text = string.Format("x:{0:0.0},y:{1:0.00}", p.X, p.Y);
            selectedDevice.SetValue(Grid.MarginProperty,
                new Thickness(p.X - deltaX, p.Y - deltaY, 0, 0));
            if (selectedDevice is DOOR)
            {
                (selectedDevice.DataContext as tblControllerConfig).X = (int)(p.X - deltaX);
                (selectedDevice.DataContext as tblControllerConfig).Y = (int)(p.Y - deltaY);
            }
            else if (selectedDevice is CCTV)
            {
                (selectedDevice.DataContext as tblCCTVConfig).X = (int)(p.X - deltaX);
                (selectedDevice.DataContext as tblCCTVConfig).Y = (int)(p.Y - deltaY);
            }
            txtBlock2.Text = string.Format("x:{0:0.0},y:{1:0.00}", (int)(p.X - deltaX), (int)(p.Y - deltaY));
             
        }

        void selectedDevice_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            selectedDevice.ReleaseMouseCapture();
            selectedDevice = null;
            deltaX = 0;
            deltaY = 0;


           
            //throw new NotImplementedException();
        }

        void selectedDevice_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            selectedDevice = (Control)sender;
            selectedDevice.CaptureMouse();
            deltaX = e.GetPosition(grdDeviceLayer).X - ((Thickness)selectedDevice.GetValue(Grid.MarginProperty)).Left;
            deltaY = e.GetPosition(grdDeviceLayer).Y - ((Thickness)selectedDevice.GetValue(Grid.MarginProperty)).Top;

            UnSelectAllDevice();

            (sender as I_IO).IsSelect = true;
            this.grdSetting.DataContext = selectedDevice.DataContext;
           // throw new NotImplementedException();
        }


        void UnSelectAllDevice()
        {
            foreach (UIElement element in grdDeviceLayer.Children)
            {
                if (element is I_IO)
                {

                    (element as I_IO).IsSelect= false;
                }

            }


        }


        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private async void cmdSave_Click(object sender, RoutedEventArgs e)
        {
          bool res=await  this.db.SubmitChangesAsync();
          if (res)
          {
              MessageBox.Show("ok");
          }
        }
    }
}
