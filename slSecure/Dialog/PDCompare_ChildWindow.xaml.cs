using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Visifire.Charts;
using slSecure.Web;
using System.ServiceModel.DomainServices.Client;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Printing;

namespace slSecure.Dialog
{
    public partial class PDCompare_ChildWindow : ChildWindow
    {
        #region Def Database obj

        private SecureDBContext DBContext = new SecureDBContext();

        #endregion

        #region Def Compare Collection

        private ObservableCollection<Sensor> _Compare_Collection;

        public ObservableCollection<Sensor> Compare_Collection
        {
            get { return this._Compare_Collection; }
            set { this._Compare_Collection = value; }
        }

        private int _Sensor_Quantity;

        public int Sensor_Quantity
        {
            get { return this._Sensor_Quantity; }
            set { this._Sensor_Quantity = value; }
        }

        #endregion

        #region Def Date obj

        private string format_Start_Date;

        private string format_End_Date;

        private DateTime _Start_Date;

        public DateTime Start_Date
        {
            get { return _Start_Date; }
            set { _Start_Date = value; }
        }

        private DateTime _End_Date;

        public DateTime End_Date
        {
            get { return _End_Date; }
            set { _End_Date = value; }
        }

        #endregion

        #region Definition of Print Obj

        PrintDocument printDocument = new PrintDocument();

        #endregion

        // Flow 1
        public PDCompare_ChildWindow(ObservableCollection<Sensor> compare_collection, DateTime start_date, DateTime end_date, int sensor_quantity)
        {
            this.InitializeComponent();

            this._Compare_Collection = compare_collection;
            this._Start_Date = start_date;
            this._End_Date = end_date;
            this._Sensor_Quantity = sensor_quantity;

            // Print
            // wire up all the events needed for printing
            printDocument.BeginPrint += new EventHandler<BeginPrintEventArgs>(printDocument_BeginPrint);
            printDocument.PrintPage += new EventHandler<PrintPageEventArgs>(printDocument_PrintPage);
            printDocument.EndPrint += new EventHandler<EndPrintEventArgs>(printDocument_EndPrint);

            Create_Chart();

        }

        // Flow 2
        private void Create_Chart()
        {
            // Default Initialize Value
            // Definition
            Double Data_Sum = 0;
            Double[] Range = new Double[2];
            Double Min = 0;
            Double Max = 0;

            // Chart
            Visifire.Charts.Chart chart = new Visifire.Charts.Chart();
            

            #region Chart Legend
            /*
            chart.Legends.Add(new Legend()
            {
                Title = "圖例",
                IsEnabled = false,
                TitleBackground = null,
                TitleFontSize = 22,
                TitleFontColor = new SolidColorBrush(Colors.Black),
                FontSize = 13,
                MarkerSize = 20,
                VerticalAlignment = VerticalAlignment.Top,
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Right,
                HorizontalContentAlignment = HorizontalAlignment.Right,
                Background = new SolidColorBrush(Colors.White)
            });
            */
            #endregion

            // Title
            Visifire.Charts.Title title = new Visifire.Charts.Title()
            {
                FontSize = 22,
                VerticalAlignment = System.Windows.VerticalAlignment.Bottom
            };

            foreach (var _sensor in this.Compare_Collection)
            {
                string item_name = _sensor.ItemName;
                string item_unit = _sensor.Unit;
                string ER_name = _sensor.ERName;

                var rawData =
                    from n in this.DBContext.GetTblAIItem1HourLogQuery()
                    where n.ItemID == _sensor.ItemID &&
                          n.Timestamp >= this.Start_Date &&
                          n.Timestamp <= this.End_Date
                    orderby n.Timestamp ascending
                    select n;

                LoadOperation<tblAIItem1HourLog> lo_rawData
                    = this.DBContext.Load<tblAIItem1HourLog>(rawData);

                lo_rawData.Completed += (o_raw, e_raw) =>
                {
                    // Sum
                    Data_Sum += lo_rawData.Entities.Count();

                    if (Data_Sum != 0)
                    {
                        #region Range Min Max

                        // Min & Max
                        var min = lo_rawData.Entities.Min(s => s.Value);
                        var max = lo_rawData.Entities.Max(s => s.Value);

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

                        #endregion

                        #region Data Series

                        DataSeries dataSeries = new DataSeries();

                        if (Data_Sum > 200)
                            dataSeries.RenderAs = RenderAs.QuickLine;
                        else
                            dataSeries.RenderAs = RenderAs.Line;

                        dataSeries.LineThickness = 2;
                        dataSeries.LightingEnabled = true;
                        dataSeries.LineStyle = LineStyles.Solid;
                        dataSeries.SelectionEnabled = true;
                        dataSeries.MovingMarkerEnabled = true;
                        dataSeries.ShowInLegend = false; // Show Legend 
                        dataSeries.IncludeDataPointsInLegend = false;
                        dataSeries.LegendText = item_name + '\n' +
                                                ER_name;
                        #endregion

                        #region Adding DataPoints

                        foreach (tblAIItem1HourLog data in lo_rawData.Entities)
                        {
                            // DataPoint
                            DataPoint dataPoint = new DataPoint();

                            if (this._Compare_Collection.Count == 1)
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

                        #endregion

                        #region Tooltips

                        // DataSeries Tooltips
                        dataSeries.ToolTipText =
                            "感知器類型: " + item_name + '\n' +
                            "地區: " + ER_name + '\n' +
                            "時間: #AxisXLabel" + "\n" +
                            "資料: #YValue " + item_unit;

                        #endregion

                        // Adding
                        chart.Series.Add(dataSeries);
                    }

                    #region Detail Information

                    string _data_Sum = "資料量: " + string.Format("{0:0.00}", Data_Sum);
                    this.sum_tb.Text = _data_Sum;
                    string _data_Min = "最小值: " + string.Format("{0:0.00}", Range[0]);
                    this.min_tb.Text = _data_Min;
                    string _data_Max = "最大值: " + string.Format("{0:0.00}", Range[1]);
                    this.max_tb.Text = _data_Max;

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
                    chart.ScrollingEnabled = false;
                    chart.LightingEnabled = true;
                    chart.IndicatorEnabled = true;
                    chart.AnimationEnabled = true;
                    chart.AnimatedUpdate = true;
                    chart.ZoomingEnabled = false;
                    chart.ShadowEnabled = false;
                    chart.Theme = "Theme3";
                    //chart.Height = this.Chart_sp.ActualHeight;
                    Thickness tk = new Thickness() { Bottom = 1, Top = 1 };
                    chart.Margin = tk;

                    #endregion

                };
                this.DBContext.tblAIItem1HourLogs.Clear();

            }

            chart.Rendered += chart_Rendered;

            // Add chart into container
            this.Chart_Container.Children.Add(chart);
        }

        private void chart_Rendered(object sender, EventArgs e)
        {
            Chart me = sender as Chart;
            if (me.Series.Count == this.Sensor_Quantity)
            {
                me.UpdateLayout();
                this.Legends_sp.Children.Clear();
                Reset_DataSeriesColors();
            }

            // Chart render will fired many times while any visualization
            // of chart been changed or fired.
            //if (this.Legends_sp.Children.Count == 0)
            //{
            //    // Let Series Colors support 6 colors.
            //    Reset_DataSeriesColors();
            //}
        }

        // Flow 3
        private void Reset_DataSeriesColors()
        {
            Chart me = this.Chart_Container.Children.OfType<Chart>().FirstOrDefault();
            TextBlock legendContent;
            StackPanel legendContainer;
            Rectangle legendRec;

            if (me != null)
            {
                int _count = 0;
                foreach (DataSeries _dataSeries in me.Series)
                {
                    _dataSeries.Tag = _count;
                    _count++;
                }

                foreach (DataSeries _dataSeries in me.Series)
                {
                    switch (Convert.ToInt32(_dataSeries.Tag))
                    {
                        case 0:
                            _dataSeries.Color = new SolidColorBrush(Colors.Red);
                            legendContainer = new StackPanel()
                            {
                                Orientation = Orientation.Horizontal, 
                                Margin = new Thickness(0,0,10,0)
                            };
                            legendRec = new Rectangle()
                            {
                                Width = 30,
                                Height = 30,
                                Fill = new SolidColorBrush(System.Windows.Media.Colors.Red),
                                VerticalAlignment = VerticalAlignment.Center,
                                HorizontalAlignment = HorizontalAlignment.Left,
                                Margin = new Thickness(5, 0, 10, 5)
                            };
                            legendContent = new TextBlock()
                            {
                                FontSize = 15,
                                Text = _dataSeries.LegendText,
                                Foreground = new SolidColorBrush(Colors.Red),
                                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                                HorizontalAlignment = System.Windows.HorizontalAlignment.Right,
                            };
                            legendContainer.Children.Add(legendRec);
                            legendContainer.Children.Add(legendContent);
                            this.Legends_sp.Children.Add(legendContainer);
                            break;

                        case 1:
                            _dataSeries.Color = new SolidColorBrush(Colors.Magenta);
                            legendContainer = new StackPanel()
                            {
                                Orientation = Orientation.Horizontal,
                                Margin = new Thickness(0, 0, 10, 0)
                            };
                            legendRec = new Rectangle()
                            {
                                Width = 30,
                                Height = 30,
                                Fill = new SolidColorBrush(System.Windows.Media.Colors.Magenta),
                                VerticalAlignment = VerticalAlignment.Center,
                                HorizontalAlignment = HorizontalAlignment.Left,
                                Margin = new Thickness(5, 0, 10, 5)
                            };
                            legendContent = new TextBlock()
                            {
                                FontSize = 15,
                                Text = _dataSeries.LegendText,
                                Foreground = new SolidColorBrush(Colors.Magenta),
                                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                                HorizontalAlignment = System.Windows.HorizontalAlignment.Right,
                            };
                            legendContainer.Children.Add(legendRec);
                            legendContainer.Children.Add(legendContent);
                            this.Legends_sp.Children.Add(legendContainer);
                            break;

                        case 2:
                            _dataSeries.Color = new SolidColorBrush(Colors.Yellow);
                            legendContainer = new StackPanel()
                            {
                                Orientation = Orientation.Horizontal,
                                Margin = new Thickness(0, 0, 10, 0)
                            };
                            legendRec = new Rectangle()
                            {
                                Width = 30,
                                Height = 30,
                                Fill = new SolidColorBrush(System.Windows.Media.Colors.Yellow),
                                VerticalAlignment = VerticalAlignment.Center,
                                HorizontalAlignment = HorizontalAlignment.Left,
                                Margin = new Thickness(5, 0, 10, 5)
                            };
                            legendContent = new TextBlock()
                            {
                                FontSize = 15,
                                Text = _dataSeries.LegendText,
                                Foreground = new SolidColorBrush(Colors.Yellow),
                                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                                HorizontalAlignment = System.Windows.HorizontalAlignment.Right,
                            };
                            legendContainer.Children.Add(legendRec);
                            legendContainer.Children.Add(legendContent);
                            this.Legends_sp.Children.Add(legendContainer);
                            break;

                        case 3:
                            _dataSeries.Color = new SolidColorBrush(Colors.Green);
                            legendContainer = new StackPanel()
                            {
                                Orientation = Orientation.Horizontal,
                                Margin = new Thickness(0, 0, 10, 0)
                            };
                            legendRec = new Rectangle()
                            {
                                Width = 30,
                                Height = 30,
                                Fill = new SolidColorBrush(System.Windows.Media.Colors.Green),
                                VerticalAlignment = VerticalAlignment.Center,
                                HorizontalAlignment = HorizontalAlignment.Left,
                                Margin = new Thickness(5, 0, 10, 5)
                            };
                            legendContent = new TextBlock()
                            {
                                FontSize = 15,
                                Text = _dataSeries.LegendText,
                                Foreground = new SolidColorBrush(Colors.Green),
                                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                                HorizontalAlignment = System.Windows.HorizontalAlignment.Right,
                            };
                            legendContainer.Children.Add(legendRec);
                            legendContainer.Children.Add(legendContent);
                            this.Legends_sp.Children.Add(legendContainer);
                            break;

                        case 4:
                            _dataSeries.Color = new SolidColorBrush(Colors.Blue);
                            legendContainer = new StackPanel()
                            {
                                Orientation = Orientation.Horizontal,
                                Margin = new Thickness(0, 0, 10, 0)
                            };
                            legendRec = new Rectangle()
                            {
                                Width = 30,
                                Height = 30,
                                Fill = new SolidColorBrush(System.Windows.Media.Colors.Blue),
                                VerticalAlignment = VerticalAlignment.Center,
                                HorizontalAlignment = HorizontalAlignment.Left,
                                Margin = new Thickness(5, 0, 10, 5)
                            };
                            legendContent = new TextBlock()
                            {
                                FontSize = 15,
                                Text = _dataSeries.LegendText,
                                Foreground = new SolidColorBrush(Colors.Blue),
                                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                                HorizontalAlignment = System.Windows.HorizontalAlignment.Right,
                            };
                            legendContainer.Children.Add(legendRec);
                            legendContainer.Children.Add(legendContent);
                            this.Legends_sp.Children.Add(legendContainer);
                            break;

                        case 5:
                            _dataSeries.Color = new SolidColorBrush(Colors.Cyan);
                            legendContainer = new StackPanel()
                            {
                                Orientation = Orientation.Horizontal,
                                Margin = new Thickness(0, 0, 10, 0)
                            };
                            legendRec = new Rectangle()
                            {
                                Width = 30,
                                Height = 30,
                                Fill = new SolidColorBrush(System.Windows.Media.Colors.Cyan),
                                VerticalAlignment = VerticalAlignment.Center,
                                HorizontalAlignment = HorizontalAlignment.Left,
                                Margin = new Thickness(5, 0, 10, 5)
                            };
                            legendContent = new TextBlock()
                            {
                                FontSize = 15,
                                Text = _dataSeries.LegendText,
                                Foreground = new SolidColorBrush(Colors.Cyan),
                                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                                HorizontalAlignment = System.Windows.HorizontalAlignment.Right,
                            };
                            legendContainer.Children.Add(legendRec);
                            legendContainer.Children.Add(legendContent);
                            this.Legends_sp.Children.Add(legendContainer);
                            break;

                        case 6:
                            _dataSeries.Color = new SolidColorBrush(Colors.Purple);
                            legendContainer = new StackPanel()
                            {
                                Orientation = Orientation.Horizontal,
                                Margin = new Thickness(0, 0, 10, 0)
                            };
                            legendRec = new Rectangle()
                            {
                                Width = 30,
                                Height = 30,
                                Fill = new SolidColorBrush(System.Windows.Media.Colors.Purple),
                                VerticalAlignment = VerticalAlignment.Center,
                                HorizontalAlignment = HorizontalAlignment.Left,
                                Margin = new Thickness(5, 0, 10, 5)
                            };
                            legendContent = new TextBlock()
                            {
                                FontSize = 15,
                                Text = _dataSeries.LegendText,
                                Foreground = new SolidColorBrush(Colors.Purple),
                                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                                HorizontalAlignment = System.Windows.HorizontalAlignment.Right,
                            };
                            legendContainer.Children.Add(legendRec);
                            legendContainer.Children.Add(legendContent);
                            this.Legends_sp.Children.Add(legendContainer);
                            break;

                        case 7:
                            _dataSeries.Color = new SolidColorBrush(Colors.Gray);
                            legendContainer = new StackPanel()
                            {
                                Orientation = Orientation.Horizontal,
                                Margin = new Thickness(0, 0, 10, 0)
                            };
                            legendRec = new Rectangle()
                            {
                                Width = 30,
                                Height = 30,
                                Fill = new SolidColorBrush(System.Windows.Media.Colors.Gray),
                                VerticalAlignment = VerticalAlignment.Center,
                                HorizontalAlignment = HorizontalAlignment.Left,
                                Margin = new Thickness(5, 0, 10, 5)
                            };
                            legendContent = new TextBlock()
                            {
                                FontSize = 15,
                                Text = _dataSeries.LegendText,
                                Foreground = new SolidColorBrush(Colors.Gray),
                                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                                HorizontalAlignment = System.Windows.HorizontalAlignment.Right,
                            };
                            legendContainer.Children.Add(legendRec);
                            legendContainer.Children.Add(legendContent);
                            this.Legends_sp.Children.Add(legendContainer);
                            break;

                        case 8:
                            _dataSeries.Color = new SolidColorBrush(Colors.Brown);
                            legendContainer = new StackPanel()
                            {
                                Orientation = Orientation.Horizontal,
                                Margin = new Thickness(0, 0, 10, 0)
                            };
                            legendRec = new Rectangle()
                            {
                                Width = 30,
                                Height = 30,
                                Fill = new SolidColorBrush(System.Windows.Media.Colors.Brown),
                                VerticalAlignment = VerticalAlignment.Center,
                                HorizontalAlignment = HorizontalAlignment.Left,
                                Margin = new Thickness(5, 0, 10, 5)
                            };
                            legendContent = new TextBlock()
                            {
                                FontSize = 15,
                                Text = _dataSeries.LegendText,
                                Foreground = new SolidColorBrush(Colors.Brown),
                                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                                HorizontalAlignment = System.Windows.HorizontalAlignment.Right,
                            };
                            legendContainer.Children.Add(legendRec);
                            legendContainer.Children.Add(legendContent);
                            this.Legends_sp.Children.Add(legendContainer);
                            break;

                        case 9:
                            _dataSeries.Color = new SolidColorBrush(Colors.Orange);
                            legendContainer = new StackPanel()
                            {
                                Orientation = Orientation.Horizontal,
                                Margin = new Thickness(0, 0, 10, 0)
                            };
                            legendRec = new Rectangle()
                            {
                                Width = 30,
                                Height = 30,
                                Fill = new SolidColorBrush(System.Windows.Media.Colors.Orange),
                                VerticalAlignment = VerticalAlignment.Center,
                                HorizontalAlignment = HorizontalAlignment.Left,
                                Margin = new Thickness(5, 0, 10, 5)
                            };
                            legendContent = new TextBlock()
                            {
                                FontSize = 15,
                                Text = _dataSeries.LegendText,
                                Foreground = new SolidColorBrush(Colors.Orange),
                                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                                HorizontalAlignment = System.Windows.HorizontalAlignment.Right,
                            };
                            legendContainer.Children.Add(legendRec);
                            legendContainer.Children.Add(legendContent);
                            this.Legends_sp.Children.Add(legendContainer);
                            break;

                        case 10:
                            _dataSeries.Color = new SolidColorBrush(Colors.LightGray);
                            legendContainer = new StackPanel()
                            {
                                Orientation = Orientation.Horizontal,
                                Margin = new Thickness(0, 0, 10, 0)
                            };
                            legendRec = new Rectangle()
                            {
                                Width = 30,
                                Height = 30,
                                Fill = new SolidColorBrush(System.Windows.Media.Colors.LightGray),
                                VerticalAlignment = VerticalAlignment.Center,
                                HorizontalAlignment = HorizontalAlignment.Left,
                                Margin = new Thickness(5, 0, 10, 5)
                            };
                            legendContent = new TextBlock()
                            {
                                FontSize = 15,
                                Text = _dataSeries.LegendText,
                                Foreground = new SolidColorBrush(Colors.LightGray),
                                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                                HorizontalAlignment = System.Windows.HorizontalAlignment.Right,
                            };
                            legendContainer.Children.Add(legendRec);
                            legendContainer.Children.Add(legendContent);
                            this.Legends_sp.Children.Add(legendContainer);
                            break;

                        case 11:
                            _dataSeries.Color = new SolidColorBrush(Colors.Black);
                            legendContainer = new StackPanel()
                            {
                                Orientation = Orientation.Horizontal,
                                Margin = new Thickness(0, 0, 10, 0)
                            };
                            legendRec = new Rectangle()
                            {
                                Width = 30,
                                Height = 30,
                                Fill = new SolidColorBrush(System.Windows.Media.Colors.Black),
                                VerticalAlignment = VerticalAlignment.Center,
                                HorizontalAlignment = HorizontalAlignment.Left,
                                Margin = new Thickness(5, 0, 10, 5)
                            };
                            legendContent = new TextBlock()
                            {
                                FontSize = 15,
                                Text = _dataSeries.LegendText,
                                Foreground = new SolidColorBrush(Colors.Black),
                                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                                HorizontalAlignment = System.Windows.HorizontalAlignment.Right,
                            };
                            legendContainer.Children.Add(legendRec);
                            legendContainer.Children.Add(legendContent);
                            this.Legends_sp.Children.Add(legendContainer);
                            break;

                        case 12:
                            _dataSeries.Color = new SolidColorBrush(Colors.DarkGray);
                            legendContainer = new StackPanel()
                            {
                                Orientation = Orientation.Horizontal,
                                Margin = new Thickness(0, 0, 10, 0)
                            };
                            legendRec = new Rectangle()
                            {
                                Width = 30,
                                Height = 30,
                                Fill = new SolidColorBrush(System.Windows.Media.Colors.DarkGray),
                                VerticalAlignment = VerticalAlignment.Center,
                                HorizontalAlignment = HorizontalAlignment.Left,
                                Margin = new Thickness(5, 0, 10, 5)
                            };
                            legendContent = new TextBlock()
                            {
                                FontSize = 15,
                                Text = _dataSeries.LegendText,
                                Foreground = new SolidColorBrush(Colors.DarkGray),
                                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                                HorizontalAlignment = System.Windows.HorizontalAlignment.Right,
                            };
                            legendContainer.Children.Add(legendRec);
                            legendContainer.Children.Add(legendContent);
                            this.Legends_sp.Children.Add(legendContainer);
                            break;

                        default:
                            //* Let Color be automaticly set in.
                            break;
                    }

                }
            }
        }

        private void Download_btn_Click(object sender, RoutedEventArgs e)
        {
            int itemID = 0;
            foreach (Sensor item in this._Compare_Collection)
            {
                if (item != null)
                {
                    itemID = Convert.ToInt32(item.ItemID);

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
        }

        // For OutOfBrowser purpose 
        public class MyHyperlinkButton : HyperlinkButton
        {
            public void ClickMe()
            {
                base.OnClick();
            }
        }

        private void ExportToImageButton_Click(object sender, RoutedEventArgs e)
        {
            //Chart _chart = this.Chart_Container.Children.OfType<Chart>().FirstOrDefault();

            Grid _grid = this.Chart_Grid;

            //if (_chart == null)
            //    return;

            // Select a location for saving the file
            SaveFileDialog saveDialog = new SaveFileDialog();

            // Set the current file name filter string that appear in the "Save as file  
            // type".
            saveDialog.Filter = "BMP (*.bmp)|*.bmp";

            // Set the default file name extension.
            saveDialog.DefaultExt = ".bmp";

            if (saveDialog.ShowDialog() == true)
            {
                if (_grid != null)
                {
                    WriteableBitmap wb = new WriteableBitmap(_grid, null);
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
            Chart _chart = this.Chart_Container.Children.OfType<Chart>().FirstOrDefault();
            //double _GridOriginalWidth = this.Chart_Grid.Width;

            if (_chart == null) { return; }
            else
            {
                this.printDocument.Print("TRDialog Chart");
                _chart.IsEnabled = false;
            }
        }

        #region Print Function

        double _gridOriginalWidth;
        private void printDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            Chart _chart = this.Chart_Container.Children.OfType<Chart>().FirstOrDefault();

            if (_chart != null)
            {
                //Content_Below();

                e.PageVisual = this.Chart_Grid;
                //e.PageVisual = Content_Below();
                e.HasMorePages = false;
            }
            else { MessageBox.Show("請先查詢製圖再列印!"); }
        }

        private StackPanel Content_Below()
        {
            StackPanel sp = new StackPanel()
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness(2),
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            
            // min
            sp.Children.Add(new TextBlock()
            {
                FontSize = 18,
                Text = "最小值: " + this.min_tb.Text
            });

            // max
            sp.Children.Add(new TextBlock()
            {
                FontSize = 18,
                Text = "最大值" + this.max_tb.Text
            });

            // Total
            sp.Children.Add(new TextBlock()
            {
                FontSize = 18,
                Text = "總資料量" + this.sum_tb.Text
            });

            //this.Chart_Grid.Children.Add(sp);

            return sp;
        }

        private void printDocument_BeginPrint(object sender, BeginPrintEventArgs e)
        {
            // Change the current grid size to 770 fit the A4 papper size.
            //_gridOriginalWidth = this.Chart_Container.Width;
            //this.Chart_Container.Width = 775;
        }

        private void printDocument_EndPrint(object sender, EndPrintEventArgs e)
        {
            // if an error occurred, alert the user to the error
            if (e.Error != null) { MessageBox.Show(e.Error.Message); }
            else
            {
                Chart _chart = this.Chart_Grid.Children.OfType<Chart>().FirstOrDefault();
                if (_chart != null)
                {
                    //_chart.Width = _chartOriginalWidth;
                    //_chart.Height = _chartOriginalHeight;
                    //_chart.Margin = new Thickness(0);

                    // Change back the grid's original state
                    //this.Chart_Container.Width = _gridOriginalWidth;
                    _chart.IsEnabled = true;
                }
            }
        }

        #endregion
    }
}

