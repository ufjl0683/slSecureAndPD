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
using slWCFModule;

namespace slSecureLib.Forms
{
    public partial class slSetSysData : Page, slWCFModule.RemoteService.ISecureServiceCallback
    {
        slSecure.Web.SecureDBContext db = slSecure.DB.GetDB();
        string OpenDoorAutoCloseTime, DoorPasswordTime, DoorPasswordTimeCycle, OpenDoorDetectionAlarmTime;
        string EventIntrusion, EventDoorOpenOverTime, EventInvalidCard, EventExternalForce, EventDoorOpen;
        slWCFModule.MyClient client;


        public slSetSysData()
        {
            InitializeComponent();

            QuerySysParameter();
        }

        // 使用者巡覽至這個頁面時執行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                client = new MyClient("CustomBinding_ISecureService");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        async Task SaveSysParameter()
        {
            var q = await db.LoadAsync<tblSysParameter>(db.GetTblSysParameterQuery());
            foreach (tblSysParameter qq in q)
            {
                if (qq.VariableName == "OpenDoorAutoCloseTime")
                {
                    qq.VariableValue = txt_OpenDoorAutoCloseTime.Text;
                }
                else if (qq.VariableName == "DoorPasswordTime")
                {
                    qq.VariableValue = cb_DoorPasswordTime.SelectedValue.ToString();
                }
                else if (qq.VariableName == "DoorPasswordTimeCycle")
                {
                    qq.VariableValue = cb_DoorPasswordTimeCycle.SelectedValue.ToString();
                }
                else if (qq.VariableName == "OpenDoorDetectionAlarmTime")
                {
                    qq.VariableValue = txt_OpenDoorDetectionAlarmTime.Text;
                }
                else if (qq.VariableName == "EventIntrusion")
                {
                    qq.VariableValue = cb_EventIntrusion.SelectedValue.ToString();
                }                
                else if (qq.VariableName == "EventDoorOpenOverTime")
                {
                    qq.VariableValue = cb_EventDoorOpenOverTime.SelectedValue.ToString();
                }                
                else if (qq.VariableName == "EventInvalidCard")
                {
                    qq.VariableValue = cb_EventInvalidCard.SelectedValue.ToString();
                }                
                else if (qq.VariableName == "EventExternalForce")
                {
                    qq.VariableValue = cb_EventExternalForce.SelectedValue.ToString();
                }                
                else if (qq.VariableName == "EventDoorOpen")
                {
                    qq.VariableValue = cb_EventDoorOpen.SelectedValue.ToString();
                }

                try
                {
                    bool res = await db.SubmitChangesAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("MagneticCard-EventData updation failed due to " + ex.Message);
                }
            }
            MessageBox.Show("儲存成功!");
        }

        async void QuerySysParameter()
        {
            var q = await db.LoadAsync<tblSysParameter>(db.GetTblSysParameterQuery());
            foreach(tblSysParameter qq in q)
            {
                if (qq.VariableName == "OpenDoorAutoCloseTime")
                    OpenDoorAutoCloseTime = qq.VariableValue;
                else if (qq.VariableName == "DoorPasswordTime")
                    DoorPasswordTime = qq.VariableValue;
                else if (qq.VariableName == "DoorPasswordTimeCycle")
                    DoorPasswordTimeCycle = qq.VariableValue;
                else if (qq.VariableName == "OpenDoorDetectionAlarmTime")
                    OpenDoorDetectionAlarmTime = qq.VariableValue;
                else if (qq.VariableName == "DoorPasswordTime")
                    DoorPasswordTime = qq.VariableValue;
                else if (qq.VariableName == "EventIntrusion")
                    EventIntrusion = qq.VariableValue;
                else if (qq.VariableName == "EventDoorOpenOverTime")
                    EventDoorOpenOverTime = qq.VariableValue;
                else if (qq.VariableName == "EventInvalidCard")
                    EventInvalidCard = qq.VariableValue;
                else if (qq.VariableName == "EventExternalForce")
                    EventExternalForce = qq.VariableValue;
                else if (qq.VariableName == "EventDoorOpen")
                    EventDoorOpen = qq.VariableValue;

            }
            txt_OpenDoorAutoCloseTime.Text = OpenDoorAutoCloseTime;
            txt_OpenDoorDetectionAlarmTime.Text = OpenDoorDetectionAlarmTime;
            cb_DoorPasswordTime.SelectedValue = DoorPasswordTime;
            cb_DoorPasswordTimeCycle.SelectedValue = DoorPasswordTimeCycle;

            cb_EventIntrusion.SelectedValue = EventIntrusion;
            cb_EventDoorOpenOverTime.SelectedValue = EventDoorOpenOverTime;
            cb_EventInvalidCard.SelectedValue = EventInvalidCard;
            cb_EventExternalForce.SelectedValue = EventExternalForce;
            cb_EventDoorOpen.SelectedValue = EventDoorOpen;
        }
        
        private async void bu_Add_Click(object sender, RoutedEventArgs e)
        {
            await SaveSysParameter();

            QuerySysParameter();
        }

        private void bu_Return_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            client.Dispose();
        }

        public void SayHello(string hello)
        {
            //throw new NotImplementedException();
        }

        public void SecureDoorEvent(slWCFModule.RemoteService.DoorEventType evttype, slWCFModule.RemoteService.DoorBindingData doorBindingData)
        {
            //throw new NotImplementedException();
        }

        public void SecureAlarm(slWCFModule.RemoteService.AlarmData alarmdata)
        {
            //throw new NotImplementedException();
        }

        private async void bu_OpenDoorAutoCloseTime_Click(object sender, RoutedEventArgs e)
        {
            var q = await db.LoadAsync<tblSysParameter>(from b in db.GetTblSysParameterQuery() where b.VariableName == "OpenDoorAutoCloseTime" select b);
            tblSysParameter bc = q.First();
            bc.VariableValue = txt_OpenDoorAutoCloseTime.Text;

            try
            {
                bool res = await db.SubmitChangesAsync();
                MessageBox.Show("儲存成功!");   
            }
            catch (Exception ex)
            {
                MessageBox.Show("SysParameter-OpenDoorAutoCloseTimeData updation failed due to " + ex.Message);
            }
           
            //有改變，通知Server
            client.SecureService.NotifyDBChangeAsync(slWCFModule.RemoteService.DBChangedConstant.DoorOpenAutoCloseTime, txt_OpenDoorAutoCloseTime.Text);
            client.SecureService.NotifyDBChangeCompleted += (s, a) =>
            {
                if (a.Error != null)
                {
                    MessageBox.Show(a.Error.Message);
                    return;
                }
            };
        }

        private async void bu_OpenDoorDetectionAlarmTime_Click(object sender, RoutedEventArgs e)
        {
            var q = await db.LoadAsync<tblSysParameter>(from b in db.GetTblSysParameterQuery() where b.VariableName == "OpenDoorDetectionAlarmTime" select b);
            tblSysParameter bc = q.First();
            bc.VariableValue = txt_OpenDoorDetectionAlarmTime.Text;

            try
            {
                bool res = await db.SubmitChangesAsync();
                MessageBox.Show("儲存成功!");   
            }
            catch (Exception ex)
            {
                MessageBox.Show("SysParameter-OpenDoorDetectionAlarmTimeData updation failed due to " + ex.Message);
            }

            //有改變，通知Server
            client.SecureService.NotifyDBChangeAsync(slWCFModule.RemoteService.DBChangedConstant.DoorOpenAlarmTime, txt_OpenDoorDetectionAlarmTime.Text);
            client.SecureService.NotifyDBChangeCompleted += (s, a) =>
            {
                if (a.Error != null)
                {
                    MessageBox.Show(a.Error.Message);
                    return;
                }
            };
        }

        private async void bu_SetDoorPassword_Click(object sender, RoutedEventArgs e)
        {

            var q = await db.LoadAsync<tblSysParameter>(from b in db.GetTblSysParameterQuery() where b.VariableName == "DoorPasswordTimeCycle" select b );
            tblSysParameter bc = q.First();
            bc.VariableValue = cb_DoorPasswordTimeCycle.SelectedValue.ToString();

            try
            {
                bool res = await db.SubmitChangesAsync();
                MessageBox.Show("儲存成功!");   
            }
            catch (Exception ex)
            {
                MessageBox.Show("SysParameter-DoorPasswordTimeCycleData updation failed due to " + ex.Message);
            }

            ////有改變，通知Server
            client.SecureService.NotifyDBChangeAsync(slWCFModule.RemoteService.DBChangedConstant.DoorPasswordTimeCycle, cb_DoorPasswordTimeCycle.SelectedValue.ToString());
            client.SecureService.NotifyDBChangeCompleted += (s, a) =>
            {
                if (a.Error != null)
                {
                    MessageBox.Show(a.Error.Message);
                    return;
                }
                
            };
                        

        }

        private async void bu_SetEventCCTV_Click(object sender, RoutedEventArgs e)
        {
            var q = await db.LoadAsync<tblSysParameter>(db.GetTblSysParameterQuery());
            foreach (tblSysParameter qq in q)
            {
                if (qq.VariableName == "EventIntrusion")
                {
                    qq.VariableValue = cb_EventIntrusion.SelectedValue.ToString();
                }
                else if (qq.VariableName == "EventDoorOpenOverTime")
                {
                    qq.VariableValue = cb_EventDoorOpenOverTime.SelectedValue.ToString();
                }
                else if (qq.VariableName == "EventInvalidCard")
                {
                    qq.VariableValue = cb_EventInvalidCard.SelectedValue.ToString();
                }
                else if (qq.VariableName == "EventExternalForce")
                {
                    qq.VariableValue = cb_EventExternalForce.SelectedValue.ToString();
                }
                else if (qq.VariableName == "EventDoorOpen")
                {
                    qq.VariableValue = cb_EventDoorOpen.SelectedValue.ToString();
                }

                try
                {
                    bool res = await db.SubmitChangesAsync();   
                }
                catch (Exception ex)
                {
                    MessageBox.Show("SysParameter-SetEventCCTVData updation failed due to " + ex.Message);
                }
            }

            MessageBox.Show("儲存成功!");

            //有改變，通知Server
            client.SecureService.NotifyDBChangeAsync(slWCFModule.RemoteService.DBChangedConstant.EventIntrusion, cb_EventIntrusion.SelectedValue.ToString());
            client.SecureService.NotifyDBChangeCompleted += (s, a) =>
            {
                if (a.Error != null)
                {
                    MessageBox.Show(a.Error.Message);
                    return;
                }

            };
            //有改變，通知Server
            client.SecureService.NotifyDBChangeAsync(slWCFModule.RemoteService.DBChangedConstant.EventDoorOpenOverTime, cb_EventDoorOpenOverTime.SelectedValue.ToString());
            client.SecureService.NotifyDBChangeCompleted += (s, a) =>
            {
                if (a.Error != null)
                {
                    MessageBox.Show(a.Error.Message);
                    return;
                }

            };
            //有改變，通知Server
            client.SecureService.NotifyDBChangeAsync(slWCFModule.RemoteService.DBChangedConstant.EventInvalidCard, cb_EventInvalidCard.SelectedValue.ToString());
            client.SecureService.NotifyDBChangeCompleted += (s, a) =>
            {
                if (a.Error != null)
                {
                    MessageBox.Show(a.Error.Message);
                    return;
                }

            };
            //有改變，通知Server
            client.SecureService.NotifyDBChangeAsync(slWCFModule.RemoteService.DBChangedConstant.EventExternalForce, cb_EventExternalForce.SelectedValue.ToString());
            client.SecureService.NotifyDBChangeCompleted += (s, a) =>
            {
                if (a.Error != null)
                {
                    MessageBox.Show(a.Error.Message);
                    return;
                }

            };
            //有改變，通知Server
            client.SecureService.NotifyDBChangeAsync(slWCFModule.RemoteService.DBChangedConstant.EventDoorOpen, cb_EventDoorOpen.SelectedValue.ToString());
            client.SecureService.NotifyDBChangeCompleted += (s, a) =>
            {
                if (a.Error != null)
                {
                    MessageBox.Show(a.Error.Message);
                    return;
                }
                
            };
            
        }


        public void ItemValueChangedEvenr(slWCFModule.RemoteService.ItemBindingData ItemBindingData)
        {
            //throw new NotImplementedException();
        }
    }
}
