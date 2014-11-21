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
        MyClient client;
        int PlaneID;
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
        protected async  override void OnNavigatedTo(NavigationEventArgs e)
        {
         
            db = new SecureDBContext();
            this.PlaneID = int.Parse(this.NavigationContext.QueryString["PlaneID"]);

            client = new MyClient("CustomBinding_ISecureService", false);

            client.OnRegistEvent += async (s) =>
            {


                await HookDoorEvent(PlaneID);

            };
            client.OnDoorEvent += client_OnDoorEvent;
            await client.RegistAndGetKey();


            this.image.Source = new BitmapImage(new Uri("/Diagrams/" + PlaneID + ".jpg", UriKind.Relative));
            await GetALLDoorBindingData(PlaneID);
            await GetALLCCTVBindingData(PlaneID);
            PlaceDoor();
            PlaceCCTV();
          


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

        void item_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
           
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
