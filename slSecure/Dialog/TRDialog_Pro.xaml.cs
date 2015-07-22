using slSecure.Web;
using System.ServiceModel.DomainServices.Client;
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
using Visifire.Charts;
using System.Windows.Browser;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Printing;

namespace slSecure.Dialog
{
    public partial class TRDialog_Pro : Page
    {
        #region Definition of Date

        public DateTime Project_StartDate;

        private DateTime Start_Date { get; set; }
        private string format_Start_Date { get; set; }

        private DateTime End_Date { get; set; }
        private string format_End_Date { get; set; }

        #endregion

        #region Definition of DB object

        slSecure.Web.SecureDBContext DBContext = new SecureDBContext();

        #endregion

        #region Definition of Chart object

        List<List<tblAIItem1HourLog>> RowData_Container =
            new List<List<tblAIItem1HourLog>>();

        #endregion

        #region Definition of Print Obj

        PrintDocument printDocument = new PrintDocument();

        #endregion

        // Constructor
        public TRDialog_Pro()
        {
            this.InitializeComponent();

            // Default Date Selection
            this.Project_StartDate = new DateTime(2015, 1, 1);
            this.Start_Date = DateTime.Now.AddDays(-1);
            this.calendar1.SelectedDate = this.Start_Date;
            this.End_Date = DateTime.Now;
            this.calendar2.SelectedDate = this.End_Date;

            // Print
            // wire up all the events needed for printing
            printDocument.BeginPrint += new EventHandler<BeginPrintEventArgs>(printDocument_BeginPrint);
            printDocument.PrintPage += new EventHandler<PrintPageEventArgs>(printDocument_PrintPage);
            printDocument.EndPrint += new EventHandler<EndPrintEventArgs>(printDocument_EndPrint);
            
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // ERPlan
            var plan =
                from n in this.DBContext.GetTblERPlaneQuery()
                select n;

            LoadOperation<tblERPlane> lo_plan =
                this.DBContext.Load<tblERPlane>(plan);

            lo_plan.Completed += (plan_o, plan_e) =>
                {
                    if (lo_plan.Entities.Count() != 0)
                    {
                        foreach (var ent in lo_plan.Entities)    
                        {
                            this.ERPlan_cb.Items.Add(new ComboBoxItem()
                            {
                                Content = "(" + ent.PlaneID + ") " + ent.PlaneName,
                                FontSize = 15,
                                Tag = ent.PlaneID,
                                VerticalContentAlignment = VerticalAlignment.Center,
                                HorizontalContentAlignment = HorizontalAlignment.Center
                            });
                        }
                        
                    }
                    
                };
            // plan
            this.DBContext.tblERPlanes.Clear();
        }

        // 使用者巡覽至這個頁面時執行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        // 啟始日期
        private void calendar1_Loaded(object sender, RoutedEventArgs e)
        {
            // Limitation Past Date
            // Project StartDate = 2014/12/31
            this.calendar1.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue,
                                                                   this.Project_StartDate.AddDays(-1),
                                                                   "專案底下的機器未運作"));

            // Limitation Future Date
            // Today ~ 2100/1/1
            this.calendar1.BlackoutDates.Add(new CalendarDateRange(this.End_Date,
                                                                   DateTime.MaxValue,
                                                                   "無法查詢未發生的事件值"));
        }
        private void calendar1_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            // Change current date value
            this.Start_Date = Convert.ToDateTime(this.calendar1.SelectedDate);

            // Change Cal2's BlackOut Date
            this.calendar2.BlackoutDates.Clear();

            // Cal2 past black time
            this.calendar2.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue,
                                                                   this.Start_Date));

            // Cal2 End Blackout
            this.calendar2.BlackoutDates.Add(new CalendarDateRange(this.End_Date.AddDays(1),
                                                                   DateTime.MaxValue));

            // Sync Data
            Sync_LogData_Refresh();
        }

        // 結束日期
        private void calendar2_Loaded(object sender, RoutedEventArgs e)
        {
            // Limitation Past Date
            this.calendar2.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue,
                                                                   this.Start_Date,
                                                                   "機器未運作"));

            // Limitation Future Date
            this.calendar2.BlackoutDates.Add(new CalendarDateRange(this.End_Date.AddDays(1),
                                                                   DateTime.MaxValue,
                                                                   "無法查詢未發生的事件值"));
        }
        private void calendar2_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            // Change current date value
            this.End_Date = Convert.ToDateTime(this.calendar2.SelectedDate);

            // Change Cal1's BlackOut Date
            this.calendar1.BlackoutDates.Clear();

            // Cal1 past blackout
            this.calendar1.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue,
                                                                   this.Project_StartDate.AddDays(-1)));

            // Cal1 End Blackout
            this.calendar1.BlackoutDates.Add(new CalendarDateRange(this.End_Date,
                                                                   DateTime.MaxValue));

            // Sync Data
            Sync_LogData_Refresh();
        }

        private void ERPlan_cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox er_cb = sender as ComboBox;
            int plan_id = Convert.ToInt32((er_cb.SelectedItem as ComboBoxItem).Tag);

            if (plan_id != 0)
            {
                this.Group_cb.Items.Clear();
                this.Sensor_List.Items.Clear();

                // Group
                var group =
                    from n in this.DBContext.GetTblItemGroupQuery()
                    where plan_id == n.PlaneID
                    select n;

                LoadOperation<tblItemGroup> lo_group =
                    this.DBContext.Load<tblItemGroup>(group);

                lo_group.Completed += (o_group, e_group) =>
                    {
                        if (lo_group.Entities.Count() != 0)
                        {
                            foreach (var ent in lo_group.Entities)
                            {
                                this.Group_cb.Items.Add(new ComboBoxItem()
                                {
                                    Content = "(" + ent.GroupID + ") " + ent.GroupName,
                                    Tag = ent.GroupID,
                                    FontSize = 15,
                                    VerticalContentAlignment = VerticalAlignment.Center,
                                    HorizontalContentAlignment = HorizontalAlignment.Center
                                });
                            }
                        }
                    };
                this.DBContext.tblItemGroups.Clear();
                // group
            }
            else
                MessageBox.Show("Plan ID Error", 
                                "ERROR", 
                                MessageBoxButton.OK);
        }

        private void Group_cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If user change plan, group_cb will
            // clear all items, that cause NullException
            if (this.Group_cb.Items.Count == 0)
                return;

            string plan_content =
                (this.ERPlan_cb.SelectedItem as ComboBoxItem).Content as string;
            plan_content = plan_content.Split(')').ToList()[1].Trim();

            int group_id =
                Convert.ToInt32((this.Group_cb.SelectedItem as ComboBoxItem).Tag);

            string group_content =
                (this.Group_cb.SelectedItem as ComboBoxItem).Content as string;
            group_content = group_content.Split(')').ToList()[1].Trim();

            if (group_id != 0)
            {
                this.Sensor_List.Items.Clear();

                /// Sensor Item
                var item =
                    from n in this.DBContext.GetTblItemConfigQuery()
                    where n.GroupID == group_id &&
                          n.Type == "AI"
                    select n;

                LoadOperation<tblItemConfig> lo_item =
                    this.DBContext.Load<tblItemConfig>(item);

                lo_item.Completed += (o_item, e_item) =>
                    {
                        if (lo_item.Entities.Count() != 0)
                        {
                            foreach (var ent in lo_item.Entities)
                            {
                                Button Item_btn = new Button()
                                {
                                    Content = "(" + ent.ItemID + ") " + ent.ItemName,
                                    FontSize = 16,
                                    Tag = ent.ItemID,
                                    VerticalContentAlignment = VerticalAlignment.Center,
                                    HorizontalContentAlignment = HorizontalAlignment.Center,
                                    VerticalAlignment = VerticalAlignment.Stretch,
                                    HorizontalAlignment = HorizontalAlignment.Stretch,
                                    Margin = new Thickness(1),
                                };

                                // Prevent user adding two same sensors
                                // into WaitingLobby Listbox
                                foreach (ListBoxItem wl_item in this.Waiting_lobby.Items)
                                {
                                    int itemID = Convert.ToInt32(wl_item.Tag);
                                    if (ent.ItemID == itemID)
                                        Item_btn.IsEnabled = false;
                                }

                                Item_btn.Click += (itembtn_o, itembtn_e) =>
                                    {
                                        // Itembtn Self
                                        // Once clicked it will be disable
                                        Button ItemBtn = itembtn_o as Button;
                                        ItemBtn.IsEnabled = false;

                                        // Lobby Sensors
                                        this.Waiting_lobby.Items.Add(new ListBoxItem()
                                        {
                                            Content = "[ " + ent.ItemName + " ]" + '(' + ent.Unit + ')' + '\n' +
                                                      plan_content + '\n' +
                                                      group_content + '\n' +
                                                      "------------",
                                            FontSize = 13,
                                            Tag = ent.ItemID,
                                            VerticalContentAlignment = VerticalAlignment.Center,
                                            HorizontalContentAlignment = HorizontalAlignment.Center,
                                            VerticalAlignment = VerticalAlignment.Stretch,
                                            HorizontalAlignment = HorizontalAlignment.Stretch
                                        });

                                        ///
                                        /// Saving Data with Sync Conception
                                        /// !! WARNING: CODE IS MESSY !!
                                        ///
                                        Sync_LogData_Save(ent.ItemID);
                                    };
                                this.Sensor_List.Items.Add(Item_btn);
                            }
                        }
                    };
                this.DBContext.tblItemConfigs.Clear();
                // group
            }
            else
                MessageBox.Show("Plan ID or Group ID Error",
                                "ERROR",
                                MessageBoxButton.OK);

        }

        private void Waiting_lobby_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox lb = sender as ListBox;

            if (lb.SelectedIndex != -1)
            {
                int _index = lb.SelectedIndex;

                int itemID =
                    Convert.ToInt32((lb.SelectedItem as ListBoxItem).Tag);

                string item_content =
                    (lb.SelectedItem as ListBoxItem).Content as string;

                MessageBoxResult result =
                    MessageBox.Show("你確定要刪除它嗎?" + '\n' + item_content,
                                    "提示",
                                    MessageBoxButton.OKCancel);

                if (result == MessageBoxResult.OK)
                {
                    // Due to SensorList will disabled Sensor selection once
                    // user clicked it from Lobby, so we need to recover the
                    // button usable status in Sensor_List
                    foreach (Button item in this.Sensor_List.Items)
                    {
                        if (Convert.ToInt32(item.Tag) == itemID)
                        {
                            item.IsEnabled = true;
                        }
                    }

                    ///
                    /// Deleteing Data with Sync Conception
                    /// !! WARNING: CODE IS MESSY !!
                    ///
                    Sync_LogData_Delete(_index);


                    lb.Items.RemoveAt(_index);
                }
                else
                {
                    this.Waiting_lobby.SelectedIndex = -1;
                }
                // if result
            }
        }

        private void Sync_LogData_Refresh()
        {
            // Sync Data
            foreach (ListBoxItem item in this.Waiting_lobby.Items)
            {
                this.RowData_Container.Clear();
                int itemID = Convert.ToInt32(item.Tag);
                Sync_LogData_Save(itemID);
            }
        }

        private void Sync_LogData_Save(int itemID)
        {
            /// From Sensor_List children button click event
            var row_data =
                from n in this.DBContext.GetTblAIItem1HourLogQuery()
                where n.ItemID == itemID &&
                      n.Timestamp >= this.Start_Date &&
                      n.Timestamp <= this.End_Date
                orderby n.Timestamp ascending
                select n;

            LoadOperation<tblAIItem1HourLog> lo_rw =
                this.DBContext.Load<tblAIItem1HourLog>(row_data);

            lo_rw.Completed += (rw_o, rw_e) =>
                {
                    if (lo_rw.Entities.Count() != 0)
                    {
                        this.RowData_Container.Add(lo_rw.Entities.ToList());
                    }
                };
        }

        private void Sync_LogData_Delete(int index)
        {
            /// From Waiting_Lobby Delete Event
            this.RowData_Container.RemoveAt(index);
        }

        private void Draw_btn_Click(object sender, RoutedEventArgs e)
        {
            // Null Exception
            if (this.RowData_Container.Count == 0)
            {
                MessageBox.Show("查詢不到點資料! 製圖中斷...");
                MessageBox.Show("會不會是設備在時間範圍內並沒有上線?");
                return;
            }

            // Default Initialize Value
            // Definition
            Double Data_Sum = 0;
            Double[] Range = new Double[2];
            Double Min = 0;
            Double Max = 0;

            // Chart
            Visifire.Charts.Chart chart = new Visifire.Charts.Chart();

            // Title
            Title title = new Visifire.Charts.Title() 
            { 
                FontSize = 22,
                VerticalAlignment = System.Windows.VerticalAlignment.Bottom 
            };
            

            foreach (var sensorData in this.RowData_Container)
            {
                // Get Current Sensor Detail Data
                int _index = this.RowData_Container.IndexOf(sensorData);
                List<string> _getSplit =
                    ((this.Waiting_lobby.Items[_index] as ListBoxItem).Content as string).Split('\n').ToList();
                
                string item_name = _getSplit[0].Split('(').ToList()[0].Trim();
                string item_unit = _getSplit[0].Split('(').ToList()[1].Trim().Split(')').ToList()[0];
                string plan_name = _getSplit[1].Trim(); 
                string group_name = _getSplit[2].Trim();

                // Sum
                Data_Sum += sensorData.Count;

                // Min & Max
                var min = sensorData.Min(s => s.Value);
                var max = sensorData.Max(s => s.Value);

                if (Range[0] > min)
                    Range[0] = min;
                else if (Range[0] == 0.0)
                    Range[0] = min;

                if (Range[1] < max)
                    Range[1] = max;

                if (Range[0] == Range[1])
                {
                    Range[0]--;
                    Range[1]++;
                }

                // DataSeries
                DataSeries dataSeries = new DataSeries();

                if (Data_Sum > 200)
                    dataSeries.RenderAs = RenderAs.QuickLine;
                else
                    dataSeries.RenderAs = RenderAs.Line;

               
                
                chart.Legends.Add(new Legend()
                {
                    Title = "圖例",
                    IsEnabled = false,
                    TitleBackground = null,
                    TitleFontSize = 22,
                    TitleFontColor = new SolidColorBrush(Colors.Black),
                    //Background = new SolidColorBrush(Colors.Black),
                    FontSize = 13,
                    MarkerSize = 20,
                    VerticalAlignment = VerticalAlignment.Top,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    HorizontalContentAlignment = HorizontalAlignment.Right,
                    Background = new SolidColorBrush(Colors.White)
                });
                 
                
                
                //dataSeries.Color = new SolidColorBrush(Colors.White);
                //dataSeries.MarkerType = Visifire.Commons.MarkerTypes.Line;
                //dataSeries.LegendMarkerType = Visifire.Commons.MarkerTypes.Square;
                dataSeries.LineThickness = 2;
                dataSeries.LightingEnabled = true;
                dataSeries.LineStyle = LineStyles.Solid;
                dataSeries.SelectionEnabled = true;
                dataSeries.MovingMarkerEnabled = true;
                dataSeries.ShowInLegend = true; // Show Legend 
                dataSeries.IncludeDataPointsInLegend = false;
                //dataSeries.LegendText = item_name + '\n' + 
                //                        plan_name + '\n' + 
                //                        group_name;
                dataSeries.LegendText = item_name + '\n' +
                                        plan_name;

                // Due to data equal zero exception
                if (Data_Sum != 0)
                {
                    foreach (tblAIItem1HourLog data in sensorData)
                    {
                        // DataPoint
                        DataPoint dataPoint = new DataPoint();

                        if (this.Sensor_List.Items.Count == 1)
                        {
                            #region 設定Point大小，隨資料多寡，縮放。

                            if (Data_Sum < 500)
                                dataPoint.MarkerSize = 5;
                            else if (Data_Sum < 1000)
                                dataPoint.MarkerSize = 4.5;
                            else if (Data_Sum < 1500)
                                dataPoint.MarkerSize = 4;
                            else if (Data_Sum < 2000)
                                dataPoint.MarkerSize = 3.5;
                            else if (Data_Sum < 2500)
                                dataPoint.MarkerSize = 3;
                            else if (Data_Sum < 3000)
                                dataPoint.MarkerSize = 2.5;
                            else if (Data_Sum < 3500)
                                dataPoint.MarkerSize = 2;
                            else if (Data_Sum < 4000)
                                dataPoint.MarkerSize = 1.5;
                            else if (Data_Sum < 4500)
                                dataPoint.MarkerSize = 1;
                            else
                                dataPoint.MarkerSize = 0.5;

                            #endregion 設定Point大小，隨資料多寡，縮放。
                        }

                        // Value
                        dataPoint.YValue = data.Value;

                        // Label
                        dataPoint.AxisXLabel = string.Format("{0:MM/dd HH:mm}", data.Timestamp);

                        // Adding to dataseries
                        dataSeries.DataPoints.Add(dataPoint);
                    }

                    // DataSeries Tooltips
                    dataSeries.ToolTipText =
                        "感知器類型: " + item_name + '\n' +
                        "地區: " + plan_name + '\n' +
                        "群組: " + group_name  + '\n' +
                        "時間: #AxisXLabel" + "\n" +
                        "資料: #YValue " + item_unit;

                    // Adding
                    chart.Series.Add(dataSeries);
                }
            }

            #region Detail Information

            this.sum_tb.Text = "資料量: " + string.Format("{0:0.00}",Data_Sum);
            this.min_tb.Text = "最小值: " + string.Format("{0:0.00}", Range[0]);
            this.max_tb.Text = "最大值: " + string.Format("{0:0.00}", Range[1]);

            #endregion

            #region Final Chart Set

            // Chart Title
            //title.Text = "感知器關聯分析圖";
            title.Text = String.Format("{0:yyyy/MM/dd}", this.Start_Date) + "～" + String.Format("{0:yyyy/MM/dd}", this.End_Date)
                              + " 感知器關聯分析圖";

            //!++ X軸
            Axis axisX = new Axis();
            axisX.AxisLabels = new AxisLabels();
            //axisX.AxisLabels.Enabled = false;
            axisX.AxisLabels.Enabled = true;
            axisX.AxisLabels.Angle = -90;
            axisX.Title = "時間";

            //!++ Y軸
            Axis yaxis = new Axis();
            //yaxis.Opacity = 0;

            // Max & Min
            yaxis.AxisMaximum = Range[1];
            yaxis.AxisMinimum = Range[0];

            double _min = Convert.ToDouble(yaxis.AxisMinimum);
            double _max = Convert.ToDouble(yaxis.AxisMaximum);
            if (_max <= _min)
            {
                //MessageBox.Show("產生 最大值 < 最小值 的問題");
                yaxis.AxisMaximum = _max = 1;
                yaxis.AxisMinimum = _min = 0;
            }

            // 加入
            chart.AxesX.Add(axisX);
            chart.AxesY.Add(yaxis);
            chart.Titles.Add(title);

            //?+ Chart 設定
            chart.ToolBarEnabled = false;
            chart.ScrollingEnabled = true;
            chart.LightingEnabled = true;
            chart.IndicatorEnabled = true;
            chart.AnimationEnabled = true;
            chart.AnimatedUpdate = true;
            chart.ZoomingEnabled = true;
            chart.ShadowEnabled = true;
            chart.Theme = "Theme3";
            //chart.Height = this.Chart_sp.ActualHeight;
            Thickness tk = new Thickness() { Bottom = 1, Top = 1 };
            chart.Margin = tk;

            #endregion

            this.Chart_Grid.Children.Clear();
            this.Chart_Grid.Children.Add(chart);

            Button exportBtn = new Button() { Content = "圖片" };
        }

        private void Download_btn_Click(object sender, RoutedEventArgs e)
        {
            int itemID = 0;
            foreach (ListBoxItem item in this.Waiting_lobby.Items)
            {
                if (item != null)
                {
                    itemID = Convert.ToInt32(item.Tag);

                    DateTime_Format();

                    Download_Http(itemID);
                }
            }

            /*
            if (this.Waiting_lobby.Items.Count != 0)
            {
                int[] itemID = new int[this.Waiting_lobby.Items.Count];
                foreach (ListBoxItem item in this.Waiting_lobby.Items)
                {
                    if (item != null)
                    {

                    }

                }
            }
             * */
        }

        private void DateTime_Format()
        {
            this.format_Start_Date = string.Format("{0:yyyy-MM-dd HH:00:00}", this.Start_Date);
            this.format_End_Date = string.Format("{0:yyyy-MM-dd HH:00:00}", this.End_Date.AddDays(1));
        }

        private void Download_Http(int itemID)
        {
            // For Local Testing URL
            //string URL = @"http://" + SaveWebConfig() + @"/secure/";

            // For Online Testing URL
            //string URL = @"http://" + SaveWebConfig() + @"/secure/";

            if (Application.Current.IsRunningOutOfBrowser)
            {
                MyHyperlinkButton button = new MyHyperlinkButton();

                button.NavigateUri = new Uri(SaveWebConfig() +
                                             @"TRDialog_DownloadForm.aspx?" +
                                             @"id=" + itemID + "&" +
                                             @"start-date=" + this.format_Start_Date + "&" +
                                             @"end-date=" + this.format_End_Date, UriKind.Absolute);

                button.TargetName = "_blank";
                button.ClickMe();
            }
            else
            {
                /*
                HtmlWindow html = HtmlPage.Window;
                html.Navigate(new Uri(@"TRDialog_DownloadForm.aspx?" +
                                      @"id=" + itemID + "&" +
                                      @"start-date=" + this.format_Start_Date + "&" +
                                      @"end-date=" + this.format_End_Date,
                                      UriKind.Relative));
                 * */

                MyHyperlinkButton button = new MyHyperlinkButton();

                button.NavigateUri = new Uri(SaveWebConfig() +
                                             @"TRDialog_DownloadForm.aspx?" +
                                             @"id=" + itemID + "&" +
                                             @"start-date=" + this.format_Start_Date + "&" +
                                             @"end-date=" + this.format_End_Date, UriKind.Absolute);

                button.TargetName = "_blank";
                button.ClickMe();
            }
        }

        // Get Current HTTP URL
        private string SaveWebConfig()
        {
            return new Uri(App.Current.Host.Source + "/../..", UriKind.Absolute).ToString();

            //string[] tempSplit =
            //    Application.Current.Host.Source.AbsoluteUri.Split('/');

            //return tempSplit[2];
        }

        // For OutOfBrowser purpose 
        // -> Using HyperLinkButton
        public class MyHyperlinkButton : HyperlinkButton
        {
            public void ClickMe()
            {
                base.OnClick();
            }
        }

        /// <summary>
        /// Saving Chart as Image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExportToImageButton_Click(object sender, RoutedEventArgs e)
        {
            Chart chart = this.Chart_Grid.Children.OfType<Chart>().FirstOrDefault();

            if (chart == null)
                return;

            // Select a location for saving the file
            SaveFileDialog saveDialog = new SaveFileDialog();

            // Set the current file name filter string that appear in the "Save as file  
            // type".
            saveDialog.Filter = "BMP (*.bmp)|*.bmp";

            // Set the default file name extension.
            saveDialog.DefaultExt = ".bmp";

            if (saveDialog.ShowDialog() == true)
            {
                Chart _chart = this.Chart_Grid.Children.OfType<Chart>().FirstOrDefault();

                if (_chart != null)
                {
                    WriteableBitmap wb = new WriteableBitmap(_chart, null);
                    using (Stream stream = saveDialog.OpenFile())
                    {
                        byte[] buffer = GetBuffer(wb);
                        stream.Write(buffer, 0, buffer.Length);
                    }
                }
            }
        }

        private static byte[] GetBuffer(WriteableBitmap bitmap)
        {
            int width = bitmap.PixelWidth;
            int height = bitmap.PixelHeight;

            MemoryStream ms = new MemoryStream();

            #region BMP File Header(14 bytes)

            //the magic number(2 bytes):BM

            ms.WriteByte(0x42);
            ms.WriteByte(0x4D);

            //the size of the BMP file in bytes(4 bytes)

            long len = bitmap.Pixels.Length * 4 + 0x36;

            ms.WriteByte((byte)len);
            ms.WriteByte((byte)(len >> 8));
            ms.WriteByte((byte)(len >> 16));
            ms.WriteByte((byte)(len >> 24));

            //reserved(2 bytes)

            ms.WriteByte(0x00);
            ms.WriteByte(0x00);

            //reserved(2 bytes)

            ms.WriteByte(0x00);
            ms.WriteByte(0x00);

            //the offset(4 bytes)

            ms.WriteByte(0x36);
            ms.WriteByte(0x00);
            ms.WriteByte(0x00);
            ms.WriteByte(0x00);

            #endregion

            #region Bitmap Information(40 bytes:Windows V3)

            //the size of this header(4 bytes)

            ms.WriteByte(0x28);
            ms.WriteByte(0x00);
            ms.WriteByte(0x00);
            ms.WriteByte(0x00);

            //the bitmap width in pixels(4 bytes)

            ms.WriteByte((byte)width);
            ms.WriteByte((byte)(width >> 8));
            ms.WriteByte((byte)(width >> 16));
            ms.WriteByte((byte)(width >> 24));

            //the bitmap height in pixels(4 bytes)

            ms.WriteByte((byte)height);
            ms.WriteByte((byte)(height >> 8));
            ms.WriteByte((byte)(height >> 16));
            ms.WriteByte((byte)(height >> 24));

            //the number of color planes(2 bytes)

            ms.WriteByte(0x01);
            ms.WriteByte(0x00);

            //the number of bits per pixel(2 bytes)

            ms.WriteByte(0x20);
            ms.WriteByte(0x00);

            //the compression method(4 bytes)

            ms.WriteByte(0x00);
            ms.WriteByte(0x00);
            ms.WriteByte(0x00);
            ms.WriteByte(0x00);

            //the image size(4 bytes)

            ms.WriteByte(0x00);
            ms.WriteByte(0x00);
            ms.WriteByte(0x00);
            ms.WriteByte(0x00);

            //the horizontal resolution of the image(4 bytes)

            ms.WriteByte(0x00);
            ms.WriteByte(0x00);
            ms.WriteByte(0x00);
            ms.WriteByte(0x00);

            //the vertical resolution of the image(4 bytes)

            ms.WriteByte(0x00);
            ms.WriteByte(0x00);
            ms.WriteByte(0x00);
            ms.WriteByte(0x00);

            //the number of colors in the color palette(4 bytes)

            ms.WriteByte(0x00);
            ms.WriteByte(0x00);
            ms.WriteByte(0x00);
            ms.WriteByte(0x00);

            //the number of important colors(4 bytes)

            ms.WriteByte(0x00);
            ms.WriteByte(0x00);
            ms.WriteByte(0x00);
            ms.WriteByte(0x00);

            #endregion

            #region Bitmap data

            for (int y = height - 1; y >= 0; y--)
            {

                for (int x = 0; x < width; x++)
                {

                    int pixel = bitmap.Pixels[width * y + x];

                    ms.WriteByte((byte)(pixel & 0xff)); //B
                    ms.WriteByte((byte)((pixel >> 8) & 0xff)); //G
                    ms.WriteByte((byte)((pixel >> 0x10) & 0xff)); //R
                    ms.WriteByte(0x00); //reserved
                }

            }

            #endregion

            return ms.GetBuffer();
        }

        private void PrintImage_btn_Click(object sender, RoutedEventArgs e)
        {
            Chart _chart = this.Chart_Grid.Children.OfType<Chart>().FirstOrDefault();
            //double _GridOriginalWidth = this.Chart_Grid.Width;
            
            if (_chart == null)
                return;
            else
            {
                #region Cancel Method 1

                //pd = new System.Windows.Printing.PrintDocument();
                //pd.PrintPage += (s, args) =>
                //{
                //    args.PageVisual = _chart;
                //};

                //pd.Print("TRDialog Chart");

                #endregion

                #region Current Method

                this.printDocument.Print("TRDialog Chart");

                _chart.IsEnabled = false;

                #endregion

                #region Cancel Method 2

                // Clear all the mouse move-in event
                /*
                _chart.Theme = "Theme2";
                _chart.IsEnabled = false;
                this.Chart_Grid.Width = 770;
                
                _chart.Print();

                _chart.Theme = "Theme3";
                this.Chart_Grid.Width = _GridOriginalWidth;
                _chart.IsEnabled = true;
                */

                #endregion
            }
        }

        #region Print Function

        //double _chartOriginalWidth;
        //double _chartOriginalHeight;
        double _gridOriginalWidth;
        private void printDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            Chart _chart = this.Chart_Grid.Children.OfType<Chart>().FirstOrDefault();

            if (_chart != null)
            {
                
                // A4 Size
                //_chart.Width = 770;
                //_chart.Height = e.PrintableArea.Height;

                e.PageVisual = this.Chart_Grid;
                e.HasMorePages = false;
            }
            else
                MessageBox.Show("請先查詢製圖再列印!");
        }

        private void printDocument_BeginPrint(object sender, BeginPrintEventArgs e)
        {
            // Change the current grid size to 770 fit the A4 papper size.
            _gridOriginalWidth = this.Chart_Grid.Width;
            this.Chart_Grid.Width = 770;

        }

        private void printDocument_EndPrint(object sender, EndPrintEventArgs e)
        {
            // if an error occurred, alert the user to the error
            if (e.Error != null)
                MessageBox.Show(e.Error.Message);
            else
            {
                Chart _chart = this.Chart_Grid.Children.OfType<Chart>().FirstOrDefault();
                if (_chart != null)
                {
                    //_chart.Width = _chartOriginalWidth;
                    //_chart.Height = _chartOriginalHeight;
                    //_chart.Margin = new Thickness(0);

                    // Change back the grid's original state
                    this.Chart_Grid.Width = _gridOriginalWidth;
                    _chart.IsEnabled = true;
                }

            }
        }
        

        #endregion
    }
}
