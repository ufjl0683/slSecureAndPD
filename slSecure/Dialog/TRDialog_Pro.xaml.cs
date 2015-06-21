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
                return;

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
                FontSize = 38,
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

                // DataSeries
                DataSeries dataSeries = new DataSeries();

                if (Data_Sum > 200)
                    dataSeries.RenderAs = RenderAs.QuickLine;
                else
                    dataSeries.RenderAs = RenderAs.Line;

                chart.Legends.Add(new Legend()
                {
                    Title = "圖例",
                    TitleBackground = null,
                    TitleFontSize = 28,
                    TitleFontColor = new SolidColorBrush(Colors.Yellow),
                    //Background = new SolidColorBrush(Colors.Black),
                    FontSize = 14,
                    MarkerSize = 14,
                    VerticalAlignment = VerticalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    HorizontalContentAlignment = HorizontalAlignment.Right
                });

                //dataSeries.Color = new SolidColorBrush(Colors.White);
                //dataSeries.MarkerType = Visifire.Commons.MarkerTypes.Line;
                dataSeries.LegendMarkerType = Visifire.Commons.MarkerTypes.Square;
                dataSeries.LineThickness = 2;
                dataSeries.LightingEnabled = true;
                dataSeries.LineStyle = LineStyles.Solid;
                dataSeries.SelectionEnabled = false;
                dataSeries.MovingMarkerEnabled = true;
                dataSeries.ShowInLegend = true;
                dataSeries.IncludeDataPointsInLegend = false;
                dataSeries.LegendText = item_name + '\n' + 
                                        plan_name + '\n' + 
                                        group_name;

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
                        dataPoint.AxisXLabel = string.Format("{0:yyyy年MM月dd日 HH:mm}", data.Timestamp);

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
            title.Text = "感知器關聯分析圖";

            //!++ X軸
            Axis axisX = new Axis();
            axisX.AxisLabels = new AxisLabels();
            axisX.AxisLabels.Enabled = false;
            axisX.Title = "時間";

            //!++ Y軸
            Axis yaxis = new Axis();
            //yaxis.Opacity = 0;

            // Max & Min
            yaxis.AxisMaximum = Range[1];
            yaxis.AxisMinimum = Range[0];

            // 加入
            chart.AxesX.Add(axisX);
            chart.AxesY.Add(yaxis);
            chart.Titles.Add(title);

            //?+ Chart 設定
            chart.ScrollingEnabled = false;
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
            string URL = @"http://" + SaveWebConfig() + @"/secure/";

            if (Application.Current.IsRunningOutOfBrowser)
            {
                MyHyperlinkButton button = new MyHyperlinkButton();

                button.NavigateUri = new Uri(URL +
                                             @"TRDialog_DownloadForm.aspx?" +
                                             @"id=" + itemID + "&" +
                                             @"start-date=" + this.format_Start_Date + "&" +
                                             @"end-date=" + this.format_End_Date, UriKind.Absolute);

                button.TargetName = "_blank";
                button.ClickMe();
            }
            else
            {
                HtmlWindow html = HtmlPage.Window;
                html.Navigate(new Uri(@"TRDialog_DownloadForm.aspx?" +
                                      @"id=" + itemID + "&" +
                                      @"start-date=" + this.format_Start_Date + "&" +
                                      @"end-date=" + this.format_End_Date,
                                      UriKind.Relative));
            }
        }

        // Get Current HTTP URL
        private string SaveWebConfig()
        {
            string[] tempSplit =
                Application.Current.Host.Source.AbsoluteUri.Split('/');

            return tempSplit[2];
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

    }
}
