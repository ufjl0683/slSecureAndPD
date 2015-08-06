using slSecure.Web;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Media.Imaging;
using System.Windows.Printing;
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

        public string Location { get; set; }

        public string ItemName { get; set; }

        public double? WarningUpper { get; set; }

        public double? WarningLower { get; set; }

        public double? AlarmUpper { get; set; }

        public double? AlarmLower { get; set; }

        public string Unit { get; set; }

        public slSecure.Web.SecureDBContext DBContext =
            new SecureDBContext();

        private System.Windows.Printing.PrintDocument pd;

        #endregion

        #region Definition of Print Obj

        PrintDocument printDocument = new PrintDocument();

        #endregion

        // Constructor
        public TRDialog(Int32 ItemID)
        {
            this.InitializeComponent();
            this.Get_Item_ID = ItemID;

            // Query();

            // Print
            // wire up all the events needed for printing
            printDocument.BeginPrint += new EventHandler<BeginPrintEventArgs>(printDocument_BeginPrint);
            printDocument.PrintPage += new EventHandler<PrintPageEventArgs>(printDocument_PrintPage);
            printDocument.EndPrint += new EventHandler<EndPrintEventArgs>(printDocument_EndPrint);
            
        }

        // Loaded Event
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //Query();
        }

        // 查詢
        private void Query()
        {
            #region Points
            this.DBContext.tblItemConfigs.Clear();
            this.DBContext.tblAIItem1HourLogs.Clear();

            // ******* 各個業主所安排的ID都不一樣!!
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

                // RTU底下的監測儀器名稱
                this.ItemName = _Query.ItemName;

                // 警告上限
                this.WarningUpper = _Query.WarningUpper;

                // 警告下限
                this.WarningLower = _Query.WarningLower;

                // 警報上限
                this.AlarmUpper = _Query.AlarmUpper;

                // 警報下限
                this.AlarmLower = _Query.AlarmLower;

                // 數值單位
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

                    if (this.Data_Sum != 0)
                    {
                        // Min & Max
                        this.Range[0] = this.DBContext.tblAIItem1HourLogs.Min(s => s.Value);
                        this.Range[1] = this.DBContext.tblAIItem1HourLogs.Max(s => s.Value);

                        #region Get EngineRoom location
                        var getControllID =
                            from n in this.DBContext.GetTblItemConfigQuery()
                            where n.ItemID == this.Get_Item_ID
                            select n;

                        LoadOperation<tblItemConfig> lo_getConID =
                            this.DBContext.Load<tblItemConfig>(getControllID);

                        lo_getConID.Completed += (o_getConID, e_getConID) =>
                        {
                            string _conid = lo_getConID.Entities.FirstOrDefault().ControlID;

                            System.Diagnostics.Debug.WriteLine("Controller ID: " + _conid);

                            var getERID =
                                from n in this.DBContext.GetTblControllerConfigQuery()
                                where n.ControlID == _conid
                                select n;

                            LoadOperation<tblControllerConfig> lo_getERID =
                                this.DBContext.Load<tblControllerConfig>(getERID);

                            lo_getERID.Completed += (o_erid, e_erid) =>
                            {
                                int _erid = lo_getERID.Entities.FirstOrDefault().ERID;

                                System.Diagnostics.Debug.WriteLine("Engine Room ID: " + _erid);

                                var get_room =
                                    from n in this.DBContext.GetTblEngineRoomConfigQuery()
                                    where n.ERID == _erid
                                    select n;

                                LoadOperation<tblEngineRoomConfig> lo_getRoom =
                                    this.DBContext.Load<tblEngineRoomConfig>(get_room);

                                lo_getRoom.Completed += (o_room, e_room) =>
                                {
                                    this.Location = lo_getRoom.Entities.FirstOrDefault().ERName;

                                    System.Diagnostics.Debug.WriteLine("Location Name: " + this.Location);
                                };

                                this.DBContext.tblEngineRoomConfigs.Clear();
                            };

                            this.DBContext.tblControllerConfigs.Clear();
                        };

                        //this.DBContext.tblItemConfigs.Clear();
                        #endregion

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
                    }
                    else
                    {
                        if (this.Start_Date == this.End_Date)
                        {
                            MessageBox.Show("查詢不到 " +
                                            string.Format("{0:yyyy/MM/dd hh:mm}", this.Start_Date) +
                                            " 的監測資料資料!!", "提示", MessageBoxButton.OK);
                        }
                        else
                        {
                            MessageBox.Show("查詢不到" + '\n' +
                                            string.Format("{0:yyyy/MM/dd hh:mm}", this.Start_Date) + '\n'
                                            + "          到" + '\n' +
                                            string.Format("{0:yyyy/MM/dd hh:mm}", this.End_Date) + '\n' +
                                            "的監測資料資料!!", "提示", MessageBoxButton.OK);
                        }
                    }
                };

                this.DBContext.tblAIItem1HourLogs.Clear();
            };

            this.DBContext.tblItemConfigs.Clear();
            #endregion

        }

        // [製圖] 一般顯示
        private Chart Create_Chart_Common()
        {
            // Chart
            Visifire.Charts.Chart chart = new Visifire.Charts.Chart();

            // Title
            Title title = new Visifire.Charts.Title()
            {
                FontSize = 22,
                VerticalAlignment = System.Windows.VerticalAlignment.Bottom
            };

            // DataSeries
            DataSeries dataSeries = new DataSeries();

            if (this.Data_Sum > 200)
                dataSeries.RenderAs = RenderAs.QuickLine;
            else
                dataSeries.RenderAs = RenderAs.Line;

            dataSeries.Color = new SolidColorBrush(Colors.White);
            dataSeries.LineThickness = 1;
            dataSeries.LightingEnabled = true;
            dataSeries.SelectionEnabled = true;
            dataSeries.MovingMarkerEnabled = true;

            // Information about query data & date
            this.Description_tb.Text = "開始:" + this.Start_Date.ToString("yyyy/MM/dd") + '\n' +
                                       "結束:" + this.End_Date.ToString("yyyy/MM/dd") + '\n' +
                                       "共 " + this.Data_Sum + " 筆資料！" + '\n' +
                                       "最低" + this.ItemName + "= " + this.Range[0].ToString() + this.Unit + '\n' +
                                       "最高" + this.ItemName + "= " + this.Range[1].ToString() + this.Unit + '\n';

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
                    dataPoint.AxisXLabel = string.Format("{0:MM/dd HH:mm}", data.Timestamp);

                    // Adding to dataseries
                    dataSeries.DataPoints.Add(dataPoint);
                }

                #region Trend Lines

                //Create_Trend_Lines(chart);

                #endregion

                #region Final Chart Set

                // Chart Title
                //title.Text = this.ItemName + "計";
                title.Text = String.Format("{0:yyyy/MM/dd}", this.Start_Date) + "～" + String.Format("{0:yyyy/MM/dd}", this.End_Date)
                              + " " + this.Location + " " + this.ItemName + "計趨勢圖";

                // DataSeries Tooltips
                dataSeries.ToolTipText = "時間: #AxisXLabel\n資料: #YValue " + this.Unit;

                //!++ X軸
                Axis axisX = new Axis();
                axisX.AxisLabels = new AxisLabels();
                //axisX.AxisLabels.Enabled = false;
                axisX.AxisLabels.Enabled = true;
                axisX.AxisLabels.Angle = -90;
                axisX.Title = "時間";

                //!++ Y軸
                Axis yaxis = new Axis();
                yaxis.Title = this.ItemName + "(" + this.Unit + ")";
                yaxis.Opacity = 100;

                yaxis.Interval = 10;
                yaxis.ValueFormatString = "#,0.0";

                #region Cancel Code
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
                /*
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
                else
                {
                    yaxis.AxisMaximum = 100;
                    yaxis.AxisMinimum = 0;
                    yaxis.Interval = 10;
                    yaxis.ValueFormatString = "#,0.0";
                }
                

                double _min = Convert.ToDouble(yaxis.AxisMinimum);
                double _max = Convert.ToDouble(yaxis.AxisMaximum);
                if (_max <= _min)
                {
                    //MessageBox.Show("產生 最大值 < 最小值 的問題");
                    MessageBox.Show("感知器查無資料! ");
                    yaxis.AxisMaximum = _max = 1;
                    yaxis.AxisMinimum = _min = 0;
                }
                 * */
                #endregion

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
            Title title = new Visifire.Charts.Title()
            {
                FontSize = 22,
                VerticalAlignment = System.Windows.VerticalAlignment.Bottom
            };

            // DataSeries
            DataSeries dataSeries = new DataSeries();

            if (this.Data_Sum > 200)
                dataSeries.RenderAs = RenderAs.QuickLine;
            else
                dataSeries.RenderAs = RenderAs.Line;

            dataSeries.Color = new SolidColorBrush(Colors.White);
            dataSeries.LineThickness = 1;
            dataSeries.LightingEnabled = true;
            dataSeries.SelectionEnabled = true;
            dataSeries.MovingMarkerEnabled = true;

            // Information about query data & date
            this.Description_tb.Text = "開始:" + this.Start_Date.ToString("yyyy/MM/dd") + '\n' +
                                       "結束:" + this.End_Date.ToString("yyyy/MM/dd") + '\n' +
                                       "共 " + this.Data_Sum + " 筆資料！" + '\n' +
                                       "最低" + this.ItemName + "= " + this.Range[0].ToString() + this.Unit + '\n' +
                                       "最高" + this.ItemName + "= " + this.Range[1].ToString() + this.Unit + '\n';

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
                    dataPoint.AxisXLabel = string.Format("{0:MM/dd HH:mm}", data.Timestamp);

                    // Adding to dataseries
                    dataSeries.DataPoints.Add(dataPoint);
                }

                #region Trend Lines

                //Create_Trend_Lines(chart);

                #endregion

                #region Final Chart Set

                // Chart Title
                //title.Text = this.ItemName + "計";
                title.Text = String.Format("{0:yyyy/MM/dd}", this.Start_Date) + "~" + String.Format("{0:yyyy/MM/dd}", this.End_Date)
                             + " " + this.Location + " " + this.ItemName + "計趨勢圖";

                // DataSeries Tooltips
                dataSeries.ToolTipText = "時間: #AxisXLabel\n資料: #YValue " + this.Unit;

                //!++ X軸
                Axis axisX = new Axis();
                axisX.AxisLabels = new AxisLabels();
                //axisX.AxisLabels.Enabled = false;
                axisX.AxisLabels.Enabled = true;
                axisX.AxisLabels.Angle = 90;
                axisX.Title = "時間";

                //!++ Y軸
                Axis yaxis = new Axis();
                yaxis.Title = this.ItemName + "(" + this.Unit + ")";
                yaxis.Opacity = 100;
                yaxis.Interval = 5;
                yaxis.ValueFormatString = "#,0.0";
                //yaxis.ValueFormatString = string.Empty;

                #region Cancel Code
                /*
                if (Get_Item_ID == 1)
                {
                    yaxis.Interval = 5;
                    yaxis.ValueFormatString = "#,0.0";
                }
                else if (Get_Item_ID == 4)
                {
                    yaxis.Interval = 5;
                    yaxis.ValueFormatString = "#,0.0";
                }
                */
                #endregion

                // Min & Max
                yaxis.AxisMaximum = this.Range[1];
                yaxis.AxisMinimum = this.Range[0];

                // if max <= min, don't let chart get any exception
                // I make a static value for min =0 and max = 1.
                if ((double)yaxis.AxisMaximum <= (double)yaxis.AxisMinimum)
                {
                    //MessageBox.Show("產生 最大值 < 最小值 的問題");
                    yaxis.AxisMaximum = 1;
                    yaxis.AxisMinimum = 0;
                }
                else
                {
                    // Pitch of yaxis's max and min value
                    double _pitch = (this.Range[0] + this.Range[1]) / 100;
                    if (_pitch == 0)
                        _pitch = 1;

                    // Adding a pitch of range, make chart more beautiful
                    yaxis.AxisMinimum = (double)yaxis.AxisMinimum - _pitch;
                    yaxis.AxisMaximum = (double)yaxis.AxisMaximum + _pitch;

                    // Let the max and min value has a line to show up their value.
                    //Create_MinMax_Trend_Lines(chart, this.Range[0], this.Range[1]);
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
                chart.Series.Add(dataSeries);
                chart.Theme = "Theme3";
                chart.Height = this.LayoutRoot.ActualHeight;
                Thickness tk = new Thickness() { Bottom = 1, Top = 1 };
                chart.Margin = tk;

                #endregion
            }

            return chart;
        }

        // 趨勢線
        private void Create_Trend_Lines(Chart chart)
        {
            // Trend Lines

            // Warning Lower Line
            chart.TrendLines.Add(new TrendLine()
            {
                Value = this.WarningLower,
                LineColor = new SolidColorBrush(Colors.Yellow)
            });
            // Warning Upper Line
            chart.TrendLines.Add(new TrendLine()
            {
                Value = this.WarningUpper,
                LineColor = new SolidColorBrush(Colors.Yellow)
            });
            // Alarm Lower Line
            chart.TrendLines.Add(new TrendLine()
            {
                Value = this.AlarmLower,
                LineColor = new SolidColorBrush(Colors.Red)
            });
            // Alarm Upper Line
            chart.TrendLines.Add(new TrendLine()
            {
                Value = this.AlarmUpper,
                LineColor = new SolidColorBrush(Colors.Red)
            });
        }

        private void Create_MinMax_Trend_Lines(Chart chart, double min, double max)
        {
            // min
            chart.TrendLines.Add(new TrendLine()
            {
                Value = min,
                LabelText = string.Format("{0:0.00} " + this.Unit, min),
                LabelFontColor = new SolidColorBrush(Colors.Blue),
                LineColor = new SolidColorBrush(Colors.Blue),
                Opacity = 0.3
            });

            // max
            chart.TrendLines.Add(new TrendLine()
            {
                Value = max,
                LabelText = string.Format("{0:0.00} " + this.Unit, max),
                LabelFontColor = new SolidColorBrush(Colors.Red),
                LineColor = new SolidColorBrush(Colors.Red),
                Opacity = 0.3
            });

            // average
            chart.TrendLines.Add(new TrendLine()
            {
                Value = (min + max) / 2,
                LabelText = string.Format("{0:0.00} " + this.Unit, (min + max) / 2),
                LabelFontColor = new SolidColorBrush(Colors.Green),
                LineColor = new SolidColorBrush(Colors.Green),
                Opacity = 0.3
            });

            // pitch between avg <---> max
            chart.TrendLines.Add(new TrendLine()
            {
                Value = ((min + max) / 2) + (max - ((min + max) / 2)) / 2,
                LabelText = string.Format("{0:0.00} " + this.Unit, ((min + max) / 2) + (max - ((min + max) / 2)) / 2),
                LabelFontColor = new SolidColorBrush(Colors.White),
                LineColor = new SolidColorBrush(Colors.White),
                Opacity = 0.2
            });

            // pitch between avg <---> min
            chart.TrendLines.Add(new TrendLine()
            {
                Value = ((min + max) / 2) - (((min + max) / 2) - min) / 2,
                LabelText = string.Format("{0:0.00} " + this.Unit, ((min + max) / 2) - (((min + max) / 2) - min) / 2),
                LabelFontColor = new SolidColorBrush(Colors.White),
                LineColor = new SolidColorBrush(Colors.White),
                Opacity = 0.2
            });
        }

        // Date Change Event
        private void Calender_Picker_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            Calendar calender = sender as Calendar;

            // Change current date value
            this.Start_Date = calender.SelectedDates.FirstOrDefault();
            this.End_Date = calender.SelectedDates.LastOrDefault();

            if (this.Start_Date > this.End_Date)
            {
                DateTime temp = this.Start_Date;
                this.Start_Date = this.End_Date;
                this.End_Date = temp;
            }
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
            calender.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue,
                                                             new DateTime(2014, 12, 31),
                                                             "機器未運作"));

            // Limitation Future Date
            DateTime dt = DateTime.Now.AddDays(1);
            calender.BlackoutDates.Add(new CalendarDateRange(new DateTime(dt.Year, dt.Month, dt.Day),
                                                             DateTime.MaxValue,
                                                             "無法查詢未發生的事件值"));
        }

        // 下載Btn event
        private void DownLoad_Click(object sender, RoutedEventArgs e)
        {
          //  string URL = "http://" + SaveWebConfig() + "/";

            if (Application.Current.IsRunningOutOfBrowser)
            {
                MyHyperlinkButton button = new MyHyperlinkButton();

                button.NavigateUri = new Uri(SaveWebConfig() + 
                                             @"TRDialog_DownloadForm.aspx?" +
                                             @"id=" + this.Get_Item_ID + "&" +
                                             @"start-date=" + string.Format("{0:yyyy-MM-dd hh:mm:ss}", this.Start_Date) + "&" +
                                             @"end-date=" + string.Format("{0:yyyy-MM-dd hh:mm:ss}", this.End_Date), UriKind.Absolute);

                button.TargetName = "_blank";
                button.ClickMe();
            }
            else
            {
                HtmlWindow html = HtmlPage.Window;
                html.Navigate(new Uri(@"TRDialog_DownloadForm.aspx?" +
                                      @"id=" + this.Get_Item_ID + "&" +
                                      @"start-date=" + this.Start_Date + "&" +
                                      @"end-date=" + this.End_Date,
                                      UriKind.Relative));
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

        // 視窗大小切換Event
        private void ChildWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            int cnt = this.sp_left.Children.Count;
            foreach (Chart u in this.sp_left.Children)
                u.Height = LayoutRoot.ActualHeight / cnt;
        }

        /// <summary>
        /// Saving Chart as Image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExportToImageButton_Click(object sender, RoutedEventArgs e)
        {
            Chart chart = this.sp_left.Children.OfType<Chart>().FirstOrDefault();

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
                Chart _chart = this.sp_left.Children.OfType<Chart>().FirstOrDefault();

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
            Chart chart = this.sp_left.Children.OfType<Chart>().FirstOrDefault();
            double _chartOriginalWidth = chart.Width;

            if (chart == null)
                return;
            else
            {
                #region Cancel Method 1
                //pd = new System.Windows.Printing.PrintDocument();
                //pd.PrintPage += (s, args) =>
                //    {
                //        args.PageVisual = chart;
                //    };

                //pd.Print("TRDialog Chart");
                #endregion

                #region Method 2

                this.printDocument.Print("TRDialog Chart");

                #endregion

                #region Cancel Method 3
                // Clear all the mouse move-in event
                /*
                chart.IsEnabled = false;
                chart.Theme = "Theme2";
                chart.Width = 770;

                chart.Print();

                chart.IsEnabled = true;
                chart.Theme = "Theme3";
                chart.Width = _chartOriginalWidth;
                */
                #endregion

            }
        }

        #region Print Function
        
        //double _chartOriginalWidth;
        //double _chartOriginalHeight;
        double _spOriginalWidth;
        private void printDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            Chart _chart = this.sp_left.Children.OfType<Chart>().FirstOrDefault();

            if (_chart != null)
            {
                #region Cancel Code
                //_chartOriginalWidth = _chart.Width;
                //_chartOriginalHeight = _chart.Height;

                //_chart.Width = e.PrintableArea.Width;
                //_chart.Height = e.PrintableArea.Height;
                #endregion

                e.PageVisual = this.sp_left;
                e.HasMorePages = false;
            }
            else
                MessageBox.Show("請先查詢製圖再列印!");
        }

        private void printDocument_BeginPrint(object sender, BeginPrintEventArgs e)
        {
            Chart _chart = this.sp_left.Children.OfType<Chart>().FirstOrDefault();

            _chart.IsEnabled = false;

            _spOriginalWidth = this.sp_left.Width;
            this.sp_left.Width = 770;
        }

        private void printDocument_EndPrint(object sender, EndPrintEventArgs e)
        {
            // if an error occurred, alert the user to the error
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else
            {
                Chart _chart = this.sp_left.Children.OfType<Chart>().FirstOrDefault();
                if (_chart != null)
                {
                    //_chart.Width = _chartOriginalWidth;
                    //_chart.Height = _chartOriginalHeight;
                    //_chart.Margin = new Thickness(0);

                    this.sp_left.Width = _spOriginalWidth;

                    _chart.IsEnabled = true;
                }

            }
        }
        
        #endregion
    }
}
