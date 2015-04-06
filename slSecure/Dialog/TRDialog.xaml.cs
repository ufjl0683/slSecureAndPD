using slSecure.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.DomainServices.Client;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Visifire.Charts;

namespace slSecure.Dialog
{
    public partial class TRDialog : ChildWindow
    {
        #region Definition
        public Int32 Get_Item_ID { get; set; }

        public DateTime Start_Date { get; set; }

        public DateTime End_Date { get; set; }

        public double Data_Sum { get; set; }

        public double[] Range = new double[2];

        public string ItemName { get; set; }

        public double? WarningUpper { get; set; }

        public double? AlarmUpper { get; set; }

        public string Unit { get; set; }

        public slSecure.Web.SecureDBContext DBContext =
            new SecureDBContext();
        #endregion

        // Constructor
        public TRDialog(Int32 ItemID)
        {
            this.InitializeComponent();
            this.Get_Item_ID = ItemID;
        }

        // Loaded Event
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Query();
        }

        // 查詢
        private void Query()
        {
            this.DBContext.tblItemConfigs.Clear();

            // ItemID
            //* 1 = 溫度
            //  2 = 感應式燈狀態
            //  3 = 感應式照明開關
            //* 4 = 濕度
            //  5 = 感應式燈狀態2
            //  6 = switch1
            //  7 = switch2
            //  8 = 感應式照明開關2

            var Config_Query =
                from n in this.DBContext.GetTblItemConfigQuery()
                where n.ItemID == Get_Item_ID // ***
                select n;

            // Load Query
            LoadOperation<tblItemConfig> Load_ConfigQuery =
                this.DBContext.Load<tblItemConfig>(Config_Query);

            // Event
            Load_ConfigQuery.Completed += (o, e) =>
            {

                this.DBContext.tblAIItem1HourLogs.Clear();

                this.Data_Sum = Load_ConfigQuery.Entities.Count();

                // Exception
                if (Load_ConfigQuery.Error != null)
                {
                    MessageBox.Show(Load_ConfigQuery.Error.Message);
                    return;
                }

                // Data quantity equal 0
                if (this.Data_Sum == 0)
                {
                    MessageBox.Show("查無資料!");
                    return;
                }

                // Get Config Value
                var _Query = Load_ConfigQuery.Entities.FirstOrDefault();
                this.ItemName = _Query.ItemName;
                this.WarningUpper = _Query.WarningUpper;
                this.AlarmUpper = _Query.AlarmUpper;
                this.Unit = _Query.Unit;

                var Log_Query =
                    from n in this.DBContext.GetTblAIItem1HourLogQuery()
                    where n.ItemID == Get_Item_ID &&
                          n.Timestamp > this.Start_Date &&
                          n.Timestamp <= this.End_Date.AddDays(1)
                    orderby n.Timestamp ascending
                    select n;

                LoadOperation<tblAIItem1HourLog> Log_LoadQuery =
                    this.DBContext.Load<tblAIItem1HourLog>(Log_Query);

                Log_LoadQuery.Completed += (LogO, LogE) =>
                {
                    this.Data_Sum = Log_LoadQuery.Entities.Count();

                    // Normal
                    if (this.Normal_rb.IsChecked == true)
                    {
                        // 清空sp
                        this.sp_left.Children.Clear();
                        this.sp_left.UpdateLayout();

                        // 製圖並加入sp
                        this.sp_left.Children.Add(Create_Chart_Common());
                    }

                    // Range
                    else if (this.Range_rb.IsChecked == true)
                    {
                        // 清空sp
                        this.sp_left.Children.Clear();
                        this.sp_left.UpdateLayout();

                        // 製圖並加入sp
                        this.sp_left.Children.Add(Create_Chart_Range());
                    }

                    Find_Chart_Range();
                };

            };
        }

        // [製圖] 一般顯示
        private Chart Create_Chart_Common()
        {
            // Chart
            Visifire.Charts.Chart chart = new Visifire.Charts.Chart();

            // Title
            Title title = new Visifire.Charts.Title();

            // DataSeries
            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.QuickLine;
            dataSeries.Color = new SolidColorBrush(Colors.White);
            dataSeries.LineThickness = 1;
            dataSeries.LightingEnabled = true;
            dataSeries.SelectionEnabled = false;
            dataSeries.MovingMarkerEnabled = true;

            // Information about query data & date
            this.Description_tb.Text = "開始:" + this.Start_Date.ToString("yyyy/MM/dd") + '\n' +
                                       "結束:" + this.End_Date.ToString("yyyy/MM/dd") + '\n' +
                                       "共 " + this.Data_Sum + " 筆資料！";

            // Due to data equal zero exception
            if (this.Data_Sum != 0)
            {
                // Row data
                foreach (tblAIItem1HourLog data in this.DBContext.tblAIItem1HourLogs)
                {
                    // DataPoint
                    DataPoint dataPoint = new DataPoint();

                    #region 設定Point大小，隨資料多寡，縮放。

                    if (this.Data_Sum < 500)
                        dataPoint.MarkerSize = 5;
                    else if (this.Data_Sum < 1000)
                        dataPoint.MarkerSize = 4.5;
                    else if (this.Data_Sum < 1500)
                        dataPoint.MarkerSize = 4;
                    else if (this.Data_Sum < 2000)
                        dataPoint.MarkerSize = 3.5;
                    else if (this.Data_Sum < 2500)
                        dataPoint.MarkerSize = 3;
                    else if (this.Data_Sum < 3000)
                        dataPoint.MarkerSize = 2.5;
                    else if (this.Data_Sum < 3500)
                        dataPoint.MarkerSize = 2;
                    else if (this.Data_Sum < 4000)
                        dataPoint.MarkerSize = 1.5;
                    else if (this.Data_Sum < 4500)
                        dataPoint.MarkerSize = 1;
                    else
                        dataPoint.MarkerSize = 0.5;

                    #endregion 設定Point大小，隨資料多寡，縮放。

                    #region 設定Point顏色

                    if (data.Value >= this.WarningUpper)
                        dataPoint.MarkerColor = new SolidColorBrush(Colors.Yellow); // 警告: 黃色
                    if (data.Value >= this.AlarmUpper)
                        dataPoint.MarkerColor = new SolidColorBrush(Colors.Red);    // 警報: 紅色

                    #endregion

                    // Value
                    dataPoint.YValue = data.Value;

                    // Label
                    dataPoint.AxisXLabel = string.Format("{0:yyyy年MM月dd日 HH:mm}", data.Timestamp);

                    // Adding to dataseries
                    dataSeries.DataPoints.Add(dataPoint);
                }

                #region Trend Lines

                // Trend Lines
                // Warning Upper Line
                chart.TrendLines.Add(new TrendLine()
                {
                    Value = this.WarningUpper,
                    LineColor = new SolidColorBrush(Colors.Yellow),
                });
                // Alarm Upper Line
                chart.TrendLines.Add(new TrendLine()
                {
                    Value = this.AlarmUpper,
                    LineColor = new SolidColorBrush(Colors.Red),
                });

                #endregion

                #region Final Chart Set

                // Chart Title
                title.Text = this.ItemName + "計";

                // DataSeries Tooltips
                dataSeries.ToolTipText = "時間: #AxisXLabel\n資料: #YValue " + this.Unit;

                //!++ X軸
                Axis axisX = new Axis();
                axisX.AxisLabels = new AxisLabels();
                axisX.AxisLabels.Enabled = false;
                axisX.Title = "時間";

                //!++ Y軸
                Axis yaxis = new Axis();
                yaxis.Title = this.ItemName + "(" + this.Unit + ")";
                yaxis.Opacity = 100;

                // Max & Min
                // ItemID
                //* 1 = 溫度
                //  2 = 感應式燈狀態
                //  3 = 感應式照明開關
                //* 4 = 濕度
                //  5 = 感應式燈狀態2
                //  6 = switch1
                //  7 = switch2
                //  8 = 感應式照明開關2
                if (Get_Item_ID == 1)
                {
                    yaxis.AxisMaximum = 40;
                    yaxis.AxisMinimum = 0;
                    yaxis.Interval = 5;
                    yaxis.ValueFormatString = "#,0.0";
                }
                else if (Get_Item_ID == 4)
                {
                    yaxis.AxisMaximum = 100;
                    yaxis.AxisMinimum = 0;
                    yaxis.Interval = 10;
                    yaxis.ValueFormatString = "#,0.0";
                }

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
                chart.Series.Add(dataSeries);
                chart.Theme = "Theme3";
                chart.Height = this.LayoutRoot.ActualHeight;
                Thickness tk = new Thickness() { Bottom = 1, Top = 1 };
                chart.Margin = tk;

                #endregion
            }

            return chart;
        }

        // [製圖] 全距顯示
        private Chart Create_Chart_Range()
        {
            // Chart
            Visifire.Charts.Chart chart = new Visifire.Charts.Chart();

            // Title
            Title title = new Visifire.Charts.Title();

            // DataSeries
            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.QuickLine;
            dataSeries.Color = new SolidColorBrush(Colors.White);
            dataSeries.LineThickness = 1;
            dataSeries.LightingEnabled = true;
            dataSeries.SelectionEnabled = false;
            dataSeries.MovingMarkerEnabled = true;

            // Information about query data & date
            this.Description_tb.Text = "開始:" + this.Start_Date.ToString("yyyy/MM/dd") + '\n' +
                                       "結束:" + this.End_Date.ToString("yyyy/MM/dd") + '\n' +
                                       "共 " + this.Data_Sum + " 筆資料！";

            // Due to data equal zero exception
            if (this.Data_Sum != 0)
            {
                // Row data
                foreach (tblAIItem1HourLog data in this.DBContext.tblAIItem1HourLogs)
                {
                    // DataPoint
                    DataPoint dataPoint = new DataPoint();

                    #region 設定Point大小，隨資料多寡，縮放。

                    if (this.Data_Sum < 500)
                        dataPoint.MarkerSize = 5;
                    else if (this.Data_Sum < 1000)
                        dataPoint.MarkerSize = 4.5;
                    else if (this.Data_Sum < 1500)
                        dataPoint.MarkerSize = 4;
                    else if (this.Data_Sum < 2000)
                        dataPoint.MarkerSize = 3.5;
                    else if (this.Data_Sum < 2500)
                        dataPoint.MarkerSize = 3;
                    else if (this.Data_Sum < 3000)
                        dataPoint.MarkerSize = 2.5;
                    else if (this.Data_Sum < 3500)
                        dataPoint.MarkerSize = 2;
                    else if (this.Data_Sum < 4000)
                        dataPoint.MarkerSize = 1.5;
                    else if (this.Data_Sum < 4500)
                        dataPoint.MarkerSize = 1;
                    else
                        dataPoint.MarkerSize = 0.5;

                    #endregion 設定Point大小，隨資料多寡，縮放。

                    #region 設定Point顏色

                    if (data.Value >= this.WarningUpper)
                        dataPoint.MarkerColor = new SolidColorBrush(Colors.Yellow); // 警告: 黃色
                    if (data.Value >= this.AlarmUpper)
                        dataPoint.MarkerColor = new SolidColorBrush(Colors.Red);    // 警報: 紅色

                    #endregion

                    // Value
                    dataPoint.YValue = data.Value;

                    // Label
                    dataPoint.AxisXLabel = string.Format("{0:yyyy年MM月dd日 HH:mm}", data.Timestamp);

                    // Adding to dataseries
                    dataSeries.DataPoints.Add(dataPoint);
                }

                #region Trend Lines

                // Trend Lines
                // Warning Upper Line
                chart.TrendLines.Add(new TrendLine()
                {
                    Value = this.WarningUpper,
                    LineColor = new SolidColorBrush(Colors.Yellow),
                });
                // Alarm Upper Line
                chart.TrendLines.Add(new TrendLine()
                {
                    Value = this.AlarmUpper,
                    LineColor = new SolidColorBrush(Colors.Red),
                });

                #endregion

                #region Final Chart Set

                // Chart Title
                title.Text = this.ItemName + "計";

                // DataSeries Tooltips
                dataSeries.ToolTipText = "時間: #AxisXLabel\n資料: #YValue " + this.Unit;

                //!++ X軸
                Axis axisX = new Axis();
                axisX.AxisLabels = new AxisLabels();
                axisX.AxisLabels.Enabled = false;
                axisX.Title = "時間";

                //!++ Y軸
                Axis yaxis = new Axis();
                yaxis.Title = this.ItemName + "(" + this.Unit + ")";
                yaxis.Opacity = 100;

                // Max & Min
                // 
                // ItemID
                //* 1 = 溫度
                //  2 = 感應式燈狀態
                //  3 = 感應式照明開關
                //* 4 = 濕度
                //  5 = 感應式燈狀態2
                //  6 = switch1
                //  7 = switch2
                //  8 = 感應式照明開關2
                //
                // Find_Chart_Range()
                // 0 = Min
                // 1 = Max
                yaxis.AxisMaximum = this.Range[1];
                yaxis.AxisMinimum = this.Range[0];

                if (Get_Item_ID == 1)
                {
                    yaxis.Interval = 0.5;
                    yaxis.ValueFormatString = "#,0.0";
                }
                else if (Get_Item_ID == 4)
                {
                    yaxis.Interval = 0.5;
                    yaxis.ValueFormatString = "#,0.0";
                }

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
                chart.Series.Add(dataSeries);
                chart.Theme = "Theme3";
                chart.Height = this.LayoutRoot.ActualHeight;
                Thickness tk = new Thickness() { Bottom = 1, Top = 1 };
                chart.Margin = tk;

                #endregion
            }

            return chart;
        }

        // Range Mode ->  Find Max & Min
        private void Find_Chart_Range()
        {
            double _min;
            double _max;

            // Find Chart
            DataSeries dataseries =
                (this.sp_left.Children.FirstOrDefault() as Chart).Series.FirstOrDefault();

            // Get First DataPoint Value
            _min = _max = dataseries.DataPoints.FirstOrDefault().YValue;

            // Check
            foreach (DataPoint datapoint in dataseries.DataPoints)
            {
                if (datapoint.YValue != 0)
                {
                    if (_max < datapoint.YValue)
                        _max = datapoint.YValue;

                    if (_min > datapoint.YValue)
                        _min = datapoint.YValue;
                }
            }

            // 0 = Min
            // 1 = Max
            this.Range[0] = _min;
            this.Range[1] = _max;
        }

        // Date Change Event
        private void Calender_Picker_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            Calendar calender = sender as Calendar;

            // Change current date value
            this.Start_Date = calender.SelectedDates.FirstOrDefault();
            this.End_Date = calender.SelectedDates.LastOrDefault();
        }

        // Query Button click event
        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            Query();
        }

        // 日曆讀取Event
        private void Calender_Picker_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Calendar calender =
                sender as System.Windows.Controls.Calendar;

            // Default date
            calender.SelectedDate = DateTime.Now.Date.AddDays(0);

            // Limitation Past Date
            calender.BlackoutDates.Add(new CalendarDateRange(new DateTime(2014, 1, 1),
                                                             new DateTime(2014, 12, 31),
                                                             "機器未運作"));

            // Limitation Future Date
            calender.BlackoutDates.Add(new CalendarDateRange(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + 1),
                                                             new DateTime(2100, 1, 1),
                                                             "無法查詢未發生的事件值"));
        }

        // 下載Btn event
        private void DownLoad_Click(object sender, RoutedEventArgs e)
        {
            HtmlWindow html = HtmlPage.Window;
            html.Navigate(new Uri(@"TRDialog_DownloadForm.aspx?" +
                                        "id=" + this.Get_Item_ID + "&" +
                                        "start-date=" + this.Start_Date + "&" +
                                        "end-date=" + this.End_Date,
                                                                    UriKind.Relative));
        }

        // 視窗大小切換Event
        private void ChildWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            int cnt = this.sp_left.Children.Count;
            foreach (Chart u in this.sp_left.Children)
                u.Height = LayoutRoot.ActualHeight / cnt;
        }

    }
}
