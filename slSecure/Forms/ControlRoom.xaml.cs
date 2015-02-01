using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;
using Common;
using System.Windows.Media.Imaging;
using System.ServiceModel.DomainServices.Client;
using slSecure.Web;
using slSecure.Controls;
using slWCFModule;
using slWCFModule.RemoteService;
using System.Threading.Tasks;

namespace slSecure.Forms
{
    public partial class ControlRoom : Page
    {

        slSecure.Web.SecureDBContext db;
        DoorBindingData[] DoorBindingDatas;
        CCTVBindingData[] CCTVBindingDatas;
        ItemBindingData[] ItemBindingDatas;
        ItemGroupBindingData[] ItemGroupBindingDatas;
        MyClient client;
        int PlaneID;
        tblERPlane tblPlane;
        public ControlRoom()
        {
            InitializeComponent();
        }

        // 使用者巡覽至這個頁面時執行。

            Task<bool> GetALLDoorBindingData(int planeid)
        {
            TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();
              
              client.SecureService.GetALLDoorBindingDataCompleted += (s, a) =>
                {
                    if (a.Error != null)
                    {
                        taskCompletionSource.TrySetResult(false);
                        return;
                    }
                    DoorBindingDatas = a.Result.ToArray();
                    taskCompletionSource.TrySetResult(true);
                };
              client.SecureService.GetALLDoorBindingDataAsync(planeid);
              return taskCompletionSource.Task;
        }
            Task<bool> GetALLCCTVBindingData(int planeid)
            {
                TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();

                client.SecureService.GetAllCCTVBindingDataCompleted += (s, a) =>
                {
                    if (a.Error != null)
                    {
                        taskCompletionSource.TrySetResult(false);
                        return;
                    }
                    CCTVBindingDatas = a.Result.ToArray();
                    taskCompletionSource.TrySetResult(true);
                };
                client.SecureService.GetAllCCTVBindingDataAsync(planeid);
                return taskCompletionSource.Task;
            }

            Task<bool> GetAllItemBindingData(int planeid)
            {
                TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();

                client.SecureService.GetAllItemBindingDataCompleted += (s, a) =>
                {
                    if (a.Error != null)
                    {
                        taskCompletionSource.TrySetResult(false);
                        return;
                    }
                    ItemBindingDatas = a.Result.ToArray();
                    taskCompletionSource.TrySetResult(true);
                };
                client.SecureService.GetAllItemBindingDataAsync(planeid);
                return taskCompletionSource.Task;
            }


            Task<bool> GetAllItemGroupBindingData(int planeid)
            {
                TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();

                client.SecureService.GetAllItemGroupBindingDataCompleted += (s, a) =>
                {
                    if (a.Error != null)
                    {
                        taskCompletionSource.TrySetResult(false);
                        return;
                    }
                    ItemGroupBindingDatas = a.Result.ToArray();
                    taskCompletionSource.TrySetResult(true);
                };
                client.SecureService.GetAllItemGroupBindingDataAsync(planeid);
                return taskCompletionSource.Task;
            }


            Task HookDoorEvent(int planeid)
          {
              TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();
              client.SecureService.HookCardReaderEventCompleted += (s, a) =>
              {
                  if (a.Error != null)
                  {
                      taskCompletionSource.TrySetResult(false);
                     
                  }
                  taskCompletionSource.TrySetResult(true);
                 
               
              };
              client.SecureService.HookCardReaderEventAsync(client.Key,planeid);

              return taskCompletionSource.Task;
          }

        Task HookItemValueChangeEvent(int planeid)
        {
            TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();
            client.SecureService.HookItemValueChangedEventCompleted += (s, a) =>
            {
                if (a.Error != null)
                {
                    taskCompletionSource.TrySetResult(false);

                }
                taskCompletionSource.TrySetResult(true);


            };
            client.SecureService.HookItemValueChangedEventAsync(client.Key, planeid);

            return taskCompletionSource.Task;
        }
        protected async  override void OnNavigatedTo(NavigationEventArgs e)
        {
         
            db = new SecureDBContext();
            this.PlaneID = int.Parse(this.NavigationContext.QueryString["PlaneID"]);

            client = new MyClient("CustomBinding_ISecureService", false);

            client.OnRegistEvent += async (s) =>
            {


                await HookDoorEvent(PlaneID);
                await HookItemValueChangeEvent(PlaneID);

            };
            client.OnDoorEvent += client_OnDoorEvent;
            client.OnItemValueChangedEvent += client_OnItemValueChangedEvent;
            await client.RegistAndGetKey();


            this.image.Source = new BitmapImage(new Uri("/Diagrams/" + PlaneID + ".png", UriKind.Relative));
            await GetALLDoorBindingData(PlaneID);
            await GetALLCCTVBindingData(PlaneID);
            await GetAllItemBindingData(PlaneID);
            await GetAllItemGroupBindingData(PlaneID);
            PlaceDoor();
            PlaceCCTV();
            PlaceItem();
            PlaceItemGroup();
            var erplanes= await  db.LoadAsync<tblERPlane>(db.GetTblERPlaneQuery().Where(n=>n.PlaneID==this.PlaneID));
           this.tblPlane= erplanes.FirstOrDefault();
           this.DataContext = tblPlane;
           // tblPlane.PlaneName
        }

        void client_OnItemValueChangedEvent(ItemBindingData itemdata)
        {
            if (this.ItemBindingDatas == null)
                return;

            ItemBindingData data = this.ItemBindingDatas.Where(n => n.ItemID == itemdata.ItemID).FirstOrDefault();

         if (data == null)
             return;

         data.Content = itemdata.Content;
         data.ColorString = itemdata.ColorString;
         data.IsAlarm = itemdata.IsAlarm;
         data.Value = itemdata.Value;
         data.Degree = itemdata.Degree;
        
            if(itemdata.GroupID!=null)
                   UpdateItemGroup((int)itemdata.GroupID);
        }
        void UpdateItemGroup(int GroupID)
        {
            if (ItemGroupBindingDatas == null)
                return;
            client.SecureService.GetAllItemGroupBindingDataCompleted += (s, a) =>
                {
                    if (a.Error != null)
                        return;
                    ItemGroupBindingData[] datas = a.Result.ToArray();

                    ItemGroupBindingData source = datas.Where(n => n.GroupID == GroupID).FirstOrDefault();
                   
                   ItemGroupBindingData  dest = ItemGroupBindingDatas.Where(n=>n.GroupID==GroupID).FirstOrDefault();
                    if(dest!=null && source!=null)
                        dest.ColorString=source.ColorString;

                };
            client.SecureService.GetAllItemGroupBindingDataAsync(this.PlaneID);
        }
        void client_OnDoorEvent(DoorEventType evttype, DoorBindingData bindingdata)
        {

            DOOR door = Canvas.FindName("Door" + bindingdata.ControlID) as DOOR;
            door.DataContext = bindingdata;

            //throw new NotImplementedException();
        }

        async void PlaceCCTV()
        {
            var q = from n in db.GetTblCCTVConfigQuery() where  n.PlaneID == this.PlaneID select n;
            var res = await db.LoadAsync<tblCCTVConfig>(q);

            foreach (tblCCTVConfig tbl in res)
            {
                CCTV item = new CCTV();
                item.Name = "CCTV" + tbl.CCTVID;


                item.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                item.VerticalAlignment = System.Windows.VerticalAlignment.Top;




                // this.Canvas.DataContext = item.DataContext = tbl;




                item.SetValue(Grid.MarginProperty, new Thickness(tbl.X, tbl.Y, 0, 0));
                CompositeTransform transform = new CompositeTransform() { Rotation = tbl.Rotation, ScaleX = tbl.ScaleX, ScaleY = tbl.ScaleY };
                item.RenderTransform = transform;
               
                CCTVBindingData bindingdata=CCTVBindingDatas.FirstOrDefault(n => n.CCTVID==tbl.CCTVID );
                item.UserName = bindingdata.UserName;
                item.Password = bindingdata.Password;
                item.Url = bindingdata.MjpegCgiString;
           
                item.DataContext =   bindingdata;
              
                this.Canvas.Children.Add(item);
                
                //item.MouseMove += selectedDevice_MouseMove;


            }
        }

    
        async void PlaceDoor()
        {
            var q = from n in db.GetTblControllerConfigQuery() where (n.ControlType == 1 || n.ControlType == 3) && n.PlaneID == this.PlaneID select n;
            var res = await db.LoadAsync<tblControllerConfig>(q);

            foreach (tblControllerConfig tbl in res)
            {
                DOOR item = new DOOR();
                item.Name = "Door" + tbl.ControlID;


                item.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                item.VerticalAlignment = System.Windows.VerticalAlignment.Top;

             

             
                // this.Canvas.DataContext = item.DataContext = tbl;



               
                item.SetValue(Grid.MarginProperty, new Thickness(tbl.X ?? 0, tbl.Y ?? 0, 0, 0));
                CompositeTransform transform = new CompositeTransform() { Rotation = tbl.Rotation ?? 0, ScaleX = tbl.ScaleX ?? 0, ScaleY = tbl.ScaleY ?? 0 };
                item.RenderTransform = transform;
                item.DataContext =  DoorBindingDatas.FirstOrDefault(n => n.ControlID == tbl.ControlID);
                this.Canvas.Children.Add(item);
                //item.MouseLeftButtonDown += selectedDevice_MouseLeftButtonDown;
                //item.MouseLeftButtonUp += selectedDevice_MouseLeftButtonUp;
                //item.MouseMove += selectedDevice_MouseMove;


            }
        }


        async void PlaceItemGroup()
        {
            var q = from n in db.GetTblItemGroupQuery() where n.PlaneID == this.PlaneID && n.IsShow select n ;
            var res = await db.LoadAsync<tblItemGroup>(q);
            foreach (tblItemGroup tbl in res)
            {
                ItemGroup item = new ItemGroup() { Name = "Grp" + tbl.GroupID };

                (item as Control).HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                (item as Control).VerticalAlignment = System.Windows.VerticalAlignment.Top;
                (item as Control).SetValue(Grid.MarginProperty, new Thickness(tbl.X , tbl.Y , 0, 0));
                CompositeTransform transform = new CompositeTransform() { Rotation = tbl.Rotation , ScaleX = tbl.ScaleX  , ScaleY = tbl.ScaleY   };
                (item as Control).RenderTransform = transform;
                (item as Control).DataContext = ItemGroupBindingDatas.FirstOrDefault(n => n.PlaneID == this.PlaneID && n.GroupID == tbl.GroupID);
                item.MouseLeftButtonDown += itemGroup_MouseLeftButtonDown;
                this.Canvas.Children.Add(item);
            }
        }
        void itemGroup_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            int gid = ((sender as ItemGroup).DataContext as ItemGroupBindingData).GroupID;

            this.lstIOMenu.ItemsSource = ItemBindingDatas.Where(n => n.GroupID == gid && (n.Type == "AI" || n.Type == "DI"));
        }
        async void  PlaceItem()
        {
             var q = from n in db.GetTblItemConfigQuery()  where   n.tblItemGroup.PlaneID == this.PlaneID  && n.IsShow select n;
             var res = await db.LoadAsync<tblItemConfig>(q);

             foreach (tblItemConfig tbl in res)
             {

                 I_IO item;
                 if (tbl.Type == "AI")
                 {
                     item = new AI() { Name = "AI" + tbl.ItemID };

                 }
                 else if (tbl.Type == "DI")
                     item = new DI() { Name = "DI" + tbl.ItemID };
                 else if (tbl.Type == "DO")
                     item = new DO() { Name = "DO" + tbl.ItemID };
                 else
                     continue;
                // item.Name = "Door" + tbl.ControlID;


                 (item as Control).HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                 (item as Control).VerticalAlignment = System.Windows.VerticalAlignment.Top;
                 (item as Control).SetValue(Grid.MarginProperty, new Thickness(tbl.X ?? 0, tbl.Y ?? 0, 0, 0));
                 CompositeTransform transform = new CompositeTransform() { Rotation = tbl.Rotation ?? 0, ScaleX = tbl.ScaleX ?? 0, ScaleY = tbl.ScaleY ?? 0 };
                 (item as Control).RenderTransform = transform;
                (item as Control).DataContext = ItemBindingDatas.FirstOrDefault(n=>n.PlaneID==this.PlaneID && n.ItemID==tbl.ItemID  );

                if (tbl.Type == "DO") 
                (item as Control).MouseLeftButtonDown += DO_MouseLeftButtonDown;
                 this.Canvas.Children.Add(item as Control);
             }
        }

        void DO_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DO Do=sender as DO;
            if((int)(Do.DataContext as ItemBindingData).Value==0)
                client.SecureService.SetItemDOValueAsync((Do.DataContext as ItemBindingData).ItemID,true);
            else
                client.SecureService.SetItemDOValueAsync((Do.DataContext as ItemBindingData).ItemID, false);

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void DOOR_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void CCTV_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            new Dialog.CCTVDialog().Show();
        }

        private void ScrollViewer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            viewBox.Width = scrollViewer.ViewportWidth;
            viewBox.Height = scrollViewer.ViewportHeight;
           
            //Canvas.Width = image.ActualWidth;
            //Canvas.Height = image.ActualHeight;
        }

        private void Viewbox_SizeChanged(object sender, SizeChangedEventArgs e)
        {
          
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void LayoutRoot_MouseMove(object sender, MouseEventArgs e)
        {
            //Point p = e.GetPosition(Canvas);
            //txtCoor.Text = string.Format("{0},{1}", p.X, p.Y);
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            if(client!=null)
            client.Dispose();
        }

      

    }
}
