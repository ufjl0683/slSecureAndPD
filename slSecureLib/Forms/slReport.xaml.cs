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
using System.ServiceModel.DomainServices.Client;
using slSecure.Web;
using slSecure;
using System.Threading.Tasks;
using slSecureLib;
using System.Windows.Data;
using System.Runtime.InteropServices.Automation;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.IO;
using System.Windows.Browser;
using slWCFModule;

namespace slSecureLib.Forms
{
    public partial class slReport : Page
    {
        slSecure.Web.SecureDBContext db = slSecure.DB.GetDB();
        PagedCollectionView pageView;
        string ReportViewer = "";

        public slReport()
        {
            InitializeComponent();
            
            cb_PageSize.SelectedIndex = 0;
            cb_PageSize_Alarm.SelectedIndex = 0;
            cb_PageSize_CardCommandLog.SelectedIndex = 0;

            GetEngineRoomConfig();

            QueryReportViewer();

            QueryEngineRoomLogData();
            QueryEngineRoomAlarmData();
            QueryCardCommandLogData();

            sp_EngineRoomLog.Visibility = Visibility.Visible;
            sp_EngineRoomAlarm.Visibility = Visibility.Collapsed;
            sp_CardCommandLog.Visibility = Visibility.Collapsed;
        }

        // 使用者巡覽至這個頁面時執行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void bu_Back_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void bu_EngineRoomLog_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            dp_StartDate.SelectedDate = null;
            dp_EndDate.SelectedDate = null;
            cb_ERName.SelectedIndex = 0;
            txt_ABA.Text = "";

            sp_EngineRoomLog.Visibility = Visibility.Visible;
            sp_EngineRoomAlarm.Visibility = Visibility.Collapsed;
            sp_CardCommandLog.Visibility = Visibility.Collapsed;
            QueryEngineRoomLogData();
        }
               
        private void bu_EngineRoomAlarm_Click(object sender, RoutedEventArgs e)
        {
            dp_StartDate_Alarm.SelectedDate = null;
            dp_EndDate_Alarm.SelectedDate = null;
            cb_ERName_Alarm.SelectedIndex = 0;
            txt_ABA_Alarm.Text = "";

            sp_EngineRoomLog.Visibility = Visibility.Collapsed;
            sp_EngineRoomAlarm.Visibility = Visibility.Visible;
            sp_CardCommandLog.Visibility = Visibility.Collapsed;
            QueryEngineRoomAlarmData();
        }

        private void bu_CardCommandLog_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            dp_StartDate_CardCommandLog.SelectedDate = null;
            dp_EndDate_CardCommandLog.SelectedDate = null;
            cb_ERName_CardCommandLog.SelectedIndex = 0;
            txt_ABA_CardCommandLog.Text = "";

            sp_EngineRoomLog.Visibility = Visibility.Collapsed;
            sp_EngineRoomAlarm.Visibility = Visibility.Collapsed;
            sp_CardCommandLog.Visibility = Visibility.Visible;
            QueryCardCommandLogData();
        }

        //查詢機房門禁歷史資料  
        async void QueryEngineRoomLogData()
        {
            dp_StartDate.SelectedDate = null;
            dp_EndDate.SelectedDate = null;
            cb_ERName.SelectedIndex = 0;
            txt_ABA.Text ="";

            var q = await db.LoadAsync<vwEngineRoomLog>(db.GetVwEngineRoomLogQuery());

            var a = q.OrderByDescending(qq => qq.FlowID).Where
                (qq => (qq.TypeID == 8 && qq.TypeCode == 1) || 
                       (qq.TypeID == 8 && qq.TypeCode == 7) || 
                       (qq.TypeID == 8 && qq.TypeCode == 8) || 
                       (qq.TypeID == 8 && qq.TypeCode == 17));

            //分頁，但會選取DataGrid第一筆
            pageView = new PagedCollectionView(a);
            dataGrid_EngineRoomLog.ItemsSource = pageView;
        }

        //查詢機房門禁警報歷史資料  
        async void QueryEngineRoomAlarmData()
        {
            var q = await db.LoadAsync<vwEngineRoomLog>(db.GetVwEngineRoomLogQuery());

            var a = q.OrderByDescending(qq => qq.FlowID).Where
                (qq => (qq.TypeID == 8 && qq.TypeCode == 4) ||
                       (qq.TypeID == 8 && qq.TypeCode == 14) ||
                       (qq.TypeID == 8 && qq.TypeCode == 19) ||
                       (qq.TypeID == 8 && qq.TypeCode == 20) ||
                       (qq.TypeID == 8 && qq.TypeCode == 11) ||
                       (qq.TypeID == 8 && qq.TypeCode == 30));

            //分頁，但會選取DataGrid第一筆
            pageView = new PagedCollectionView(a);
            dataGrid_EngineRoomAlarm.ItemsSource = pageView;
        }

        //查詢讀卡機指令歷史資料  
        async void QueryCardCommandLogData()
        {
            var q = await db.LoadAsync<vwCardCommandLog>(db.GetVwCardCommandLogQuery());

            var a = q.OrderByDescending(qq => qq.FlowID);

            //分頁，但會選取DataGrid第一筆
            pageView = new PagedCollectionView(a);
            dataGrid_CardCommandLog.ItemsSource = pageView;
        }

        async void QueryReportViewer()
        {
            var q = await db.LoadAsync<tblSysParameter>(from b in db.GetTblSysParameterQuery() where b.VariableName == "ReportViewer" select b);

            tblSysParameter bc = q.First();
            ReportViewer = bc.VariableValue;
        }

        //取得機房名稱清單下拉選單
        async void GetEngineRoomConfig()
        {
            var q = await db.LoadAsync<tblEngineRoomConfig>
                (from b in db.GetTblEngineRoomConfigQuery()
                 select b);

            List<string> lEngineRoomList = new List<string>();
            lEngineRoomList.Add("請選擇");
            foreach (var MCData in q)
            {
                lEngineRoomList.Add(MCData.ERName);
            }

            cb_ERName.ItemsSource = lEngineRoomList;
            cb_ERName_Alarm.ItemsSource = lEngineRoomList;
            cb_ERName_CardCommandLog.ItemsSource = lEngineRoomList;

            cb_ERName.SelectedIndex = 0;
            cb_ERName_Alarm.SelectedIndex = 0;
            cb_ERName_CardCommandLog.SelectedIndex = 0;
        }

        private void bu_Excel_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid_EngineRoomLog.ItemsSource != null)
            {
                ////目前應用程式是否為Out of Browser執行
                //if (Application.Current.IsRunningOutOfBrowser)
                //{
                //    //透過OOB匯出excel檔(不用此方法，要避免電腦沒有安裝Excel的問題)
                //    dataGrid_EngineRoomLog.ExportToExcel();
                //}
                //else
                //{
                //    dataGrid_EngineRoomLog.ToCSV();
                //}
                dataGrid_EngineRoomLog.ToCSV();
            }
        }

        private void bu_Excel_Alarm_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid_EngineRoomAlarm.ItemsSource != null)
            {
                ////目前應用程式是否為Out of Browser執行
                //if (Application.Current.IsRunningOutOfBrowser)
                //{
                //    //透過OOB匯出excel檔(不用此方法，要避免電腦沒有安裝Excel的問題)
                //    dataGrid_EngineRoomAlarm.ExportToExcel();
                //}
                //else
                //{
                //    dataGrid_EngineRoomAlarm.ToCSV();
                //}
                dataGrid_EngineRoomAlarm.ToCSV();
            }
        }

        private void bu_Excel_CardCommandLog_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid_CardCommandLog.ItemsSource != null)
            {
                ////目前應用程式是否為Out of Browser執行
                //if (Application.Current.IsRunningOutOfBrowser)
                //{
                //    //透過OOB匯出excel檔(不用此方法，要避免電腦沒有安裝Excel的問題)
                //    dataGrid_CardCommandLog.ExportToExcel();
                //}
                //else
                //{
                //    dataGrid_CardCommandLog.ToCSV();
                //}
                dataGrid_CardCommandLog.ToCSV();
            }
        }

        private async void bu_Query_Click(object sender, RoutedEventArgs e)
        {
            var q = await db.LoadAsync<vwEngineRoomLog>(db.GetVwEngineRoomLogQuery());
            //dataGrid_EngineRoomLog.ItemsSource = q;

            var b = q.OrderByDescending(qq => qq.FlowID).Where
                (qq => (qq.TypeID == 8 && qq.TypeCode == 1) ||
                       (qq.TypeID == 8 && qq.TypeCode == 7) ||
                       (qq.TypeID == 8 && qq.TypeCode == 8) ||
                       (qq.TypeID == 8 && qq.TypeCode == 17));

            if (!string.IsNullOrEmpty(txt_ABA.Text))
            {
                b = b.Where(a => a.ABA == txt_ABA.Text);
            }

            if (cb_ERName.SelectedIndex != 0)
            {
                b = b.Where(a => a.ERName == cb_ERName.SelectedValue.ToString());
            }

            if (dp_StartDate.SelectedDate != null || dp_EndDate.SelectedDate != null)
            {
                bool flag = Comm.checkTimes(dp_StartDate.SelectedDate.ToString(), dp_EndDate.SelectedDate.ToString());

                if (flag == true)
                {
                    b = b.Where(a => ((a.StartTime >= Comm.strTime) && (a.StartTime <= Comm.endTime)));
                }
            }

            //分頁，但會選取DataGrid第一筆
            pageView = new PagedCollectionView(b);

            dataGrid_EngineRoomLog.ItemsSource = pageView;
            
        }

        private async void bu_Query_Alarm_Click(object sender, RoutedEventArgs e)
        {
            var q = await db.LoadAsync<vwEngineRoomLog>(db.GetVwEngineRoomLogQuery());
            //dataGrid_EngineRoomLog.ItemsSource = q;

            var b = q.OrderByDescending(qq => qq.FlowID).Where
                (qq => (qq.TypeID == 8 && qq.TypeCode == 4) ||
                       (qq.TypeID == 8 && qq.TypeCode == 14) ||
                       (qq.TypeID == 8 && qq.TypeCode == 19) ||
                       (qq.TypeID == 8 && qq.TypeCode == 20) ||
                       (qq.TypeID == 8 && qq.TypeCode == 11) ||
                       (qq.TypeID == 8 && qq.TypeCode == 30));

            if (!string.IsNullOrEmpty(txt_ABA_Alarm.Text))
            {
                b = b.Where(a => a.ABA == txt_ABA_Alarm.Text);
            }

            if (cb_ERName_Alarm.SelectedIndex != 0)
            {
                b = b.Where(a => a.ERName == cb_ERName_Alarm.SelectedValue.ToString());
            }


            if (dp_StartDate_Alarm.SelectedDate != null || dp_EndDate_Alarm.SelectedDate != null)
            {
                bool flag = Comm.checkTimes(dp_StartDate_Alarm.SelectedDate.ToString(), dp_EndDate_Alarm.SelectedDate.ToString());

                if (flag == true)
                {
                    b = b.Where(a => ((a.StartTime >= Comm.strTime) && (a.StartTime <= Comm.endTime)));
                }

            }

            //分頁，但會選取DataGrid第一筆
            pageView = new PagedCollectionView(b);

            dataGrid_EngineRoomAlarm.ItemsSource = pageView;

        }

        private async void bu_Query_CardCommandLog_Click(object sender, RoutedEventArgs e)
        {
            var q = await db.LoadAsync<vwCardCommandLog>(db.GetVwCardCommandLogQuery());
            var b = q.OrderByDescending(qq => qq.FlowID).Where(qq=>1==1);

            if (!string.IsNullOrEmpty(txt_ABA_CardCommandLog.Text))
            {
                b = b.Where(a => a.ABA == txt_ABA_CardCommandLog.Text.ToString());
            }

            if (cb_ERName_CardCommandLog.SelectedIndex != 0)
            {
                b = b.Where(a => a.ERName == cb_ERName_CardCommandLog.SelectedValue.ToString());
            }

            if (dp_StartDate_CardCommandLog.SelectedDate != null || dp_EndDate_CardCommandLog.SelectedDate != null)
            {
                bool flag = Comm.checkTimes(dp_StartDate_CardCommandLog.SelectedDate.ToString(), dp_EndDate_CardCommandLog.SelectedDate.ToString());

                if (flag == true)
                {
                    b = b.Where(a => ((a.Timestamp >= Comm.strTime) && (a.Timestamp <= Comm.endTime)));
                }

            }

            //分頁，但會選取DataGrid第一筆
            pageView = new PagedCollectionView(b);

            dataGrid_CardCommandLog.ItemsSource = pageView;

        }

        private void bu_ReportPrint_Click(object sender, RoutedEventArgs e)
        {
            string tmpABA="",tmpERName="",tmpStrTime="",tmpEndTime="";
            if (!string.IsNullOrEmpty(txt_ABA.Text))
            {
                tmpABA = txt_ABA.Text;
            }

            if (cb_ERName.SelectedIndex != 0)
            {
                tmpERName = cb_ERName.SelectedValue.ToString();
            }


            if (dp_StartDate.SelectedDate != null || dp_EndDate.SelectedDate != null)
            {
                bool flag = Comm.checkTimes(dp_StartDate.SelectedDate.ToString(), dp_EndDate.SelectedDate.ToString());

                if (flag == true)
                {
                    tmpStrTime = Comm.strTime.ToString("yyyy-MM-dd HH:mm:ss");
                    tmpEndTime = Comm.endTime.ToString("yyyy-MM-dd HH:mm:ss");
                }
            }

            //透過HttpUtility.UrlEncode(參數名)對中文參數進行編碼   
            //var filePath = "http://" + Application.Current.Host.Source.Host + ":" + Application.Current.Host.Source.Port + 
            var filePath = ReportViewer +     
                 "/rp_vwEngineRoomLogPage.aspx?" +
                 "ERName=" + HttpUtility.UrlEncode(tmpERName) +
                 "&ABA=" + HttpUtility.UrlEncode(tmpABA) +
                 "&StrTime=" + HttpUtility.UrlEncode(tmpStrTime) +
                 "&EndTime=" + HttpUtility.UrlEncode(tmpEndTime);

            //Uri myURI = new Uri(System.Windows.Browser.HtmlPage.Document.DocumentUri, filePath);
            //System.Windows.Browser.HtmlPage.Window.Navigate(myURI, "_blank");
            //if (App.Current.IsRunningOutOfBrowser)
            //{
                MyHyperlinkButton button = new MyHyperlinkButton();
                button.NavigateUri = new Uri(filePath);
                button.TargetName = "_blank";
                button.ClickMe();
            //}
            //else
            //{
            //    System.Windows.Browser.HtmlPage.Window.Navigate(new Uri(System.Windows.Browser.HtmlPage.Document.DocumentUri, filePath), "_blank");
            //} 
        }

        private void bu_ReportPrint_Alarm_Click(object sender, RoutedEventArgs e)
        {
            string tmpABA = "", tmpERName = "", tmpStrTime = "", tmpEndTime = "";
            if (!string.IsNullOrEmpty(txt_ABA_Alarm.Text))
            {
                tmpABA = txt_ABA_Alarm.Text;
            }

            if (cb_ERName_Alarm.SelectedIndex != 0)
            {
                tmpERName = cb_ERName_Alarm.SelectedValue.ToString();
            }


            if (dp_StartDate_Alarm.SelectedDate != null || dp_EndDate_Alarm.SelectedDate != null)
            {
                bool flag = Comm.checkTimes(dp_StartDate_Alarm.SelectedDate.ToString(), dp_EndDate_Alarm.SelectedDate.ToString());

                if (flag == true)
                {
                    tmpStrTime = Comm.strTime.ToString("yyyy-MM-dd HH:mm:ss");
                    tmpEndTime = Comm.endTime.ToString("yyyy-MM-dd HH:mm:ss");
                }
            }

            //透過HttpUtility.UrlEncode(參數名)對中文參數進行編碼   
            //var filePath = "http://" + Application.Current.Host.Source.Host + ":" + Application.Current.Host.Source.Port + 
            var filePath = ReportViewer +   
                 "/rp_vwEngineRoomLog_AlarmPage.aspx?" +
                 "ERName=" + HttpUtility.UrlEncode(tmpERName) + 
                 "&ABA=" + HttpUtility.UrlEncode(tmpABA) +
                 "&StrTime=" + HttpUtility.UrlEncode(tmpStrTime) +
                 "&EndTime=" + HttpUtility.UrlEncode(tmpEndTime);

            //Uri myURI = new Uri(System.Windows.Browser.HtmlPage.Document.DocumentUri, filePath);
            //System.Windows.Browser.HtmlPage.Window.Navigate(myURI, "_blank");
            //if (App.Current.IsRunningOutOfBrowser)
            //{
                MyHyperlinkButton button = new MyHyperlinkButton();
                button.NavigateUri = new Uri(filePath);
                button.TargetName = "_blank";
                button.ClickMe();
            //}
            //else
            //{
            //    System.Windows.Browser.HtmlPage.Window.Navigate(new Uri(System.Windows.Browser.HtmlPage.Document.DocumentUri, filePath), "_blank");
            //} 
        }

        private void bu_ReportPrint_CardCommandLog_Click(object sender, RoutedEventArgs e)
        {
            string tmpABA = "", tmpERName = "", tmpStrTime = "", tmpEndTime = "";
            if (!string.IsNullOrEmpty(txt_ABA_CardCommandLog.Text))
            {
                tmpABA = txt_ABA_CardCommandLog.Text;
            }

            if (cb_ERName_CardCommandLog.SelectedIndex != 0)
            {
                tmpERName = cb_ERName_CardCommandLog.SelectedValue.ToString();
            }


            if (dp_StartDate_CardCommandLog.SelectedDate != null || dp_EndDate_CardCommandLog.SelectedDate != null)
            {
                bool flag = Comm.checkTimes(dp_StartDate_CardCommandLog.SelectedDate.ToString(), dp_EndDate_CardCommandLog.SelectedDate.ToString());

                if (flag == true)
                {
                    tmpStrTime = Comm.strTime.ToString("yyyy-MM-dd HH:mm:ss");
                    tmpEndTime = Comm.endTime.ToString("yyyy-MM-dd HH:mm:ss");
                }
            }

            //透過HttpUtility.UrlEncode(參數名)對中文參數進行編碼   
            //var filePath = "http://" + Application.Current.Host.Source.Host + ":" + Application.Current.Host.Source.Port + 
            var filePath = ReportViewer +        
                 "/rp_vwCardCommandLogPage.aspx?" +
                 "ERName=" + HttpUtility.UrlEncode(tmpERName) +
                 "&ABA=" + HttpUtility.UrlEncode(tmpABA) +
                 "&StrTime=" + HttpUtility.UrlEncode(tmpStrTime) +
                 "&EndTime=" + HttpUtility.UrlEncode(tmpEndTime);

            //Uri myURI = new Uri(System.Windows.Browser.HtmlPage.Document.DocumentUri, filePath);
            //System.Windows.Browser.HtmlPage.Window.Navigate(myURI, "_blank");
            //if (App.Current.IsRunningOutOfBrowser)
            //{
                MyHyperlinkButton button = new MyHyperlinkButton();
                button.NavigateUri = new Uri(filePath);
                button.TargetName = "_blank";
                button.ClickMe();
            //}
            //else
            //{
            //    System.Windows.Browser.HtmlPage.Window.Navigate(new Uri(System.Windows.Browser.HtmlPage.Document.DocumentUri, filePath), "_blank");
            //} 
        }

        private void cb_PageSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dp_DG_EngineRoomLog.PageSize = int.Parse(cb_PageSize.SelectedValue.ToString()); 
        }

        private void cb_PageSize_Alarm_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dp_DG_EngineRoomAlarm.PageSize = int.Parse(cb_PageSize_Alarm.SelectedValue.ToString()); 
        }

        private void cb_PageSize_CardCommandLog_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dp_DG_CardCommandLog.PageSize = int.Parse(cb_PageSize_CardCommandLog.SelectedValue.ToString()); 
        }

    }
}
