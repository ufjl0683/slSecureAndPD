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
using slSecure.Web;
using System.ServiceModel.DomainServices.Client;
using System.Collections.ObjectModel;
using Visifire.Charts;

namespace slSecure.Dialog
{
    public partial class PDCompare : Page
    {
        #region Definition of Date

        public DateTime Project_StartDate;

        private DateTime Start_Date { get; set; }
        private string format_Start_Date { get; set; }

        private DateTime End_Date { get; set; }
        private string format_End_Date { get; set; }

        #endregion

        #region Definition of DB object

      //  slSecure.Web.SecureDBContext DBContext = new SecureDBContext();

        #endregion

        #region Definition of SensorData

        ObservableCollection<Sensor> Item_Collection
            = new ObservableCollection<Sensor>();

        ObservableCollection<Sensor> Compare_Collection
            = new ObservableCollection<Sensor>();

        private int Compared_Sensor_Amount = 6;

        #endregion

        #region Definition of Chart object

        List<List<tblAIItem1HourLog>> RowData_Container =
            new List<List<tblAIItem1HourLog>>();

        #endregion

        // Constructor
        public PDCompare()
        {
            this.InitializeComponent();

            #region Default Date Selection

            this.Project_StartDate = new DateTime(2015, 1, 1);
            this.Start_Date = DateTime.Now.AddDays(-1);
            this.Start_DataPicker.SelectedDate = this.Start_Date;
            this.End_Date = DateTime.Now;
            this.End_DataPicker.SelectedDate = this.End_Date;

            #endregion

            #region ObservableCollection Changed Event

            this.Compare_Collection.CollectionChanged += Compare_Collection_CollectionChanged;

            this.Item_Collection.CollectionChanged += Item_Collection_CollectionChanged;

            #endregion
        }

        /// <summary>
        /// 預先讀取當前資料庫各地的機房資料。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SecureDBContext db = new SecureDBContext();
            var Engine_Room_Data =
                from n in db.GetTblEngineRoomConfigQuery()
                select n;

            LoadOperation<tblEngineRoomConfig> lo_ERD =
                db.Load<tblEngineRoomConfig>(Engine_Room_Data);

            lo_ERD.Completed += (o_ERD, e_ERD) =>
                {
                    if (lo_ERD.Entities.Count() != 0)
                    {
                        foreach (tblEngineRoomConfig _entity in lo_ERD.Entities)
                        {
                            this.Engine_Room_combobox.Items.Add(new ComboBoxItem()
                            {
                                VerticalContentAlignment = System.Windows.VerticalAlignment.Center,
                                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                                HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center,
                                Content = _entity.ERName,
                                Tag = _entity.ERID
                            });
                        }

                    }
                    else { MessageBox.Show("查詢不到各地區的機房資料!", "錯誤", MessageBoxButton.OK); }
                };
        //    this.DBContext.tblEngineRoomConfigs.Clear();

        }

        // 使用者巡覽至這個頁面時執行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        #region Start_DataPicker Function

        private void Start_DataPicker_Loaded(object sender, RoutedEventArgs e)
        {
            // Limitation Past Date
            // Project StartDate = 2015/1/1
            // Black Date = 0/0/0 ~ 2014/12/31
            this.Start_DataPicker
                .BlackoutDates
                .Add(new CalendarDateRange(DateTime.MinValue,
                                           this.Project_StartDate.AddDays(-1),
                                           "專案底下的機器未運作"));

            // Limitation Future Date
            // Next day ~ future
            this.Start_DataPicker
                .BlackoutDates
                .Add(new CalendarDateRange(this.End_Date,
                                           DateTime.MaxValue,
                                           "無法查詢未發生的事件值"));
        }

        private void Start_DataPicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            // Change current date value
            this.Start_Date = Convert.ToDateTime(this.Start_DataPicker.SelectedDate);

            // Change Cal2's BlackOut Date
            this.End_DataPicker.BlackoutDates.Clear();

            // Cal2 past black time
            this.End_DataPicker
                .BlackoutDates
                .Add(new CalendarDateRange(DateTime.MinValue,
                                           this.Start_Date));

            // Cal2 End Blackout
            this.End_DataPicker
                .BlackoutDates
                .Add(new CalendarDateRange(this.End_Date.AddDays(1),
                                           DateTime.MaxValue));

            // Sync Data
            //Sync_LogData_Refresh();
        }

        #endregion

        #region End_Picker Function

        private void End_DataPicker_Loaded(object sender, RoutedEventArgs e)
        {
            // Limitation Past Date
            this.End_DataPicker
                .BlackoutDates
                .Add(new CalendarDateRange(DateTime.MinValue,
                                           this.Start_Date,
                                           "機器未運作"));

            // Limitation Future Date
            this.End_DataPicker
                .BlackoutDates
                .Add(new CalendarDateRange(this.End_Date.AddDays(1),
                                           DateTime.MaxValue,
                                           "無法查詢未發生的事件值"));
        }

        private void End_DataPicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            // Change current date value
            this.End_Date = Convert.ToDateTime(this.End_DataPicker.SelectedDate);

            // Change Cal1's BlackOut Date
            this.Start_DataPicker.BlackoutDates.Clear();

            // Cal1 past blackout
            this.Start_DataPicker
                .BlackoutDates
                .Add(new CalendarDateRange(DateTime.MinValue,
                                           this.Project_StartDate.AddDays(-1)));

            // Cal1 End Blackout
            this.Start_DataPicker
                .BlackoutDates
                .Add(new CalendarDateRange(this.End_Date,
                                           DateTime.MaxValue));

            // Sync Data
            //Sync_LogData_Refresh();
        }

        #endregion

        /// <summary>
        /// 在時間、站臺的條件選擇下，查詢製圖的點資料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Query_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (this.CompareObj_List.Items.Count != 0)
            {
                PDCompare_ChildWindow child_w =
                    new PDCompare_ChildWindow(
                        this.Compare_Collection, 
                        this.Start_Date, 
                        this.End_Date, 
                        this.Compare_Collection.Count);

                child_w.Show();
            }
            else { MessageBox.Show("請先加入欲比較的感知器!"); }

        }

        private void Engine_Room_combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox me = sender as ComboBox;
            Int32 _erid;
            string _ername = (me.SelectedItem as ComboBoxItem).Content as string;
            //this.Selection_List.Items.Clear();
            this.Item_Collection.Clear();

            if (me.SelectedIndex != -1)
            {
                _erid = Convert.ToInt32((me.SelectedItem as ComboBoxItem).Tag);
                SecureDBContext db1=new SecureDBContext();
                var RTU =
                    from n in db1.GetTblControllerConfigQuery()
                    where n.ERID == _erid && (
                          n.ControlType == 3 ||
                          n.ControlType==6 ||
                          n.ControlType==7 ||
                          n.ControlType==8 ||
                          n.ControlType==9 || n.ControlType==10)
                    select n;

               
                LoadOperation<tblControllerConfig> lo_RTU =
                    db1.Load<tblControllerConfig>(RTU);

                lo_RTU.Completed += (o_RTU, e_RTU) =>
                    {
                        if (lo_RTU.Entities.Count() != 0)
                        {
                            // It is posible if this company has more than 1 RTU.
                            foreach (tblControllerConfig _controller in lo_RTU.Entities)
                            {
                                SecureDBContext db2 = new SecureDBContext();
                                var ItemConfig =
                                    from n in db2.GetTblItemConfigQuery()
                                    where n.ControlID == _controller.ControlID &&
                                          n.Type == "AI"
                                    select n;
                              
                                LoadOperation<tblItemConfig> lo_item =
                                    db2.Load<tblItemConfig>(ItemConfig);

                                lo_item.Completed += (o_item, e_item) =>
                                    {
                                        if (lo_item.Entities.Count() != 0)
                                        {
                                            foreach (tblItemConfig _item in lo_item.AllEntities)
                                            {
                                                if (this.Compare_Collection.Count != 0)
                                                {
                                                    Sensor snr =
                                                       ( from n in this.Compare_Collection
                                                        where n.ItemID == _item.ItemID
                                                         select n).FirstOrDefault() ;

                                                    if (snr==null)
                                                        this.Item_Collection.Add(
                                                                new Sensor(_ername,
                                                                           _controller.ControlID,
                                                                           _item.ItemID,
                                                                           _item.ItemName,
                                                                           _item.Unit));
                                                }
                                                else
                                                {
                                                    this.Item_Collection.Add(
                                                                   new Sensor(_ername,
                                                                              _controller.ControlID,
                                                                              _item.ItemID,
                                                                              _item.ItemName,
                                                                              _item.Unit));
                                                }
                                            }

                                            // Binding
                                            this.Selection_List.ItemsSource = this.Item_Collection;
                                           
                                        }
                                        //else { MessageBox.Show(_ername + "底下的" + _controller.ControlID + "並沒有可用的感知器!"); }
                                    };
                              //  this.DBContext.tblItemConfigs.Clear();

                            }
                        }
                        else { MessageBox.Show("找不到ER ID: " + _erid + " 的 RTU資料!"); }
                        
                    };
            //    this.DBContext.tblControllerConfigs.Clear();

            }
        }

        private void Selection_List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox me = sender as ListBox;
            Sensor clicked_sensor = me.SelectedItem as Sensor;

            if (me.SelectedIndex != -1)
            {
                // Adding selected sensor into compare Collection
                if (this.Compare_Collection.Count < this.Compared_Sensor_Amount)
                {
                    this.Compare_Collection.Add(clicked_sensor);
                    this.Item_Collection.Remove(clicked_sensor);
                }
            }
        }

        private void CompareObj_List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox me = sender as ListBox;
            Sensor clicked_sensor = me.SelectedItem as Sensor;

            if (me.SelectedIndex != -1)
            {
                this.Item_Collection.Add(clicked_sensor);
                this.Compare_Collection.Remove(clicked_sensor);
            }
        }

        #region ObservableCollection Changed Event

        private void Item_Collection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ObservableCollection<Sensor> me = sender as ObservableCollection<Sensor>;

            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {

            }

            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {

            }
        }

        private void Compare_Collection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ObservableCollection<Sensor> me = sender as ObservableCollection<Sensor>;

            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                this.CompareObj_List.ItemsSource = me;
            }

            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {

            }
        }

        #endregion
    }


    // Collectable Sensor Item Definition Class
    public class Sensor
    {
        private int _ItemID;

        private string _ItemName;

        private string _ERName;

        private string _RTUName;

        private string _Unit;

        public int ItemID
        {
            get { return _ItemID; }
            set { _ItemID = value; }
        }

        public string ItemName
        {
            get { return _ItemName; }
            set { _ItemName = value; }
        }

        public string ERName
        {
            get { return _ERName; }
            set { _ERName = value; }
        }

        public string RTUName
        {
            get { return _RTUName; }
            set { _RTUName = value; }
        }

        public string Unit
        {
            get { return _Unit; }
            set { _Unit = value; }
        }

        public Sensor(string ername, string rtuname, int itemid, string itemname, string unit)
        {
            this._ERName = ername;
            this._RTUName = rtuname;
            this._ItemID = itemid;
            this._ItemName = itemname;
            this._Unit = unit;
        }

        public override string ToString()
        {
            // (old format)中交控中心-RTU32A (12) 濕度計
            // (now formet)中交控中心-濕度計
            return this._ERName 
                 //+ "-" + this._RTUName
                 //+ " (" + this._ItemID + ") " 
                 + "-" + this._ItemName;
            //return base.ToString();
        }
    }
}
