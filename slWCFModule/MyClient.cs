using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ServiceModel;
using slWCFModule.RemoteService;
using System.Threading.Tasks;
 

namespace slWCFModule
{
   
    public delegate void OnDoorEventHandler(DoorEventType  evttype,DoorBindingData bindingdata);
    public delegate void OnAlarmEventHandler(AlarmData  alammdata);
    public delegate void OnRegistHandler(MyClient client);
    public delegate void OnItemValueChangedHandler(ItemBindingData itemdata);
    public class MyClient:slWCFModule.RemoteService.ISecureServiceCallback,IDisposable
    {
        public event OnDoorEventHandler OnDoorEvent ;
        public event OnAlarmEventHandler OnAlarm ;
        public event OnRegistHandler OnRegistEvent;
        public event OnItemValueChangedHandler OnItemValueChangedEvent;
       // string GUID;
        public SecureServiceClient SecureService;
        public string Key { set; get; }
        System.Threading.Timer tmr;
        string Config;
        //public  class SecureServiceObject :slWCFModule.RemoteService.SecureServiceClient ,IDisposable
        //{
        //    System.Threading.Timer tmr;
        //    public SecureServiceObject(InstanceContext callbackInstance,string config):base( callbackInstance, config)
        //    {

        //        tmr = new System.Threading.Timer(new System.Threading.TimerCallback(Timeout), null, 1000 * 30, 1000 * 30);
               
        //    }
        //    ~SecureServiceObject()
        //    {
        //        try
        //        {
        //            tmr.Change(System.Threading.Timeout.Infinite, 0);
        //            tmr.Dispose();
        //        }
        //        catch { ;}
        //    }
        //    void Timeout(object sender)
        //    {
        //        try
        //        {
        //            if (this.State == CommunicationState.Opened)
        //            {
        //                this.ToServerHelloAsync();
        //            }
        //            else
        //            {
                        

        //                 tmr.Change(System.Threading.Timeout.Infinite, 0);
        //                 tmr.Dispose();
        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.Message);
        //            ;
        //        }
        //    }
        //    public void SayHello(string hello)
        //    {
                 
        //    }

        //    public void Dispose()
        //    {
        //        this.CloseAsync();
        //        tmr.Change(System.Threading.Timeout.Infinite, 0);
        //        tmr.Dispose();
        //    }
        //}
        public MyClient( string config):this(config,true)
            
        {

           

         
             //try
             //{

             //    this.SecureService.RegisterAsync(Guid.NewGuid().ToString());
             //    this.SecureService.RegisterCompleted += (s, a) =>
             //        {
             //            this.Key = a.Result;
             //        };
             //}
             //catch (Exception ex)
             //{
             //    Console.WriteLine(ex.Message);
             //}
        }
        public MyClient(string config,bool IsAutoRegister)
        {
            this.Config = config;
            this.SecureService = this.SecureService = new SecureServiceClient(new InstanceContext(this), config);// new SecureServiceObject(new InstanceContext(this), config);
            if (IsAutoRegister)
            {
                RegistTask();
            }

            tmr = new System.Threading.Timer(new System.Threading.TimerCallback(Timeout), null, 1000 * 120, 1000 * 30);
        }

        void RegistTask()
        {

            try
            {

                this.SecureService.RegisterAsync(Guid.NewGuid().ToString());
                this.SecureService.RegisterCompleted += (s, a) =>
                {
                    if (a.Error != null)
                    {
                    
                        MessageBox.Show(a.Error.Message);
                        return;
                    }

                    this.Key = a.Result;
                    if (this.OnRegistEvent != null)
                        this.OnRegistEvent(this);
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        void Timeout(object sender)
        {
            try
            {
                if (SecureService.State == CommunicationState.Opened)
                {
                    SecureService.ToServerHelloAsync();
                }
                else
                {
                    SecureService.CloseAsync();
                    SecureService = new SecureServiceClient(new InstanceContext(this), Config);
                    RegistTask();
                    //tmr.Change(System.Threading.Timeout.Infinite, 0);
                    //tmr.Dispose();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ;
            }
        }
        public Task<string> RegistAndGetKey()
        {
            TaskCompletionSource<string> source = new TaskCompletionSource<string>();
           
            try
            {

                this.SecureService.RegisterAsync(Guid.NewGuid().ToString());
                this.SecureService.RegisterCompleted += (s, a) =>
                {
                    this.Key = a.Result;
                    if (this.OnRegistEvent != null)
                        this.OnRegistEvent(this);
                    source.TrySetResult(a.Result);
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return source.Task;
        }
      
        ~MyClient()
        {
           
        }



        public void SayHello(string hello)
        {
           
        }

        public void Dispose()
        {
            SecureService.CloseAsync();
            tmr.Change(System.Threading.Timeout.Infinite, 0);
            tmr.Dispose();
           
        }


        public void SecureEvent(object evttype, object evtData)
        {
          
        }


        public void SecureDoorEvent(RemoteService.DoorEventType evttype, RemoteService.DoorBindingData doorBindingData)
        {
            if (this.OnDoorEvent != null)
                this.OnDoorEvent(evttype, doorBindingData);

           // throw new NotImplementedException();
        }


        public void SecureAlarm(AlarmData alarmdata)
        {
            if (this.OnAlarm != null)
                this.OnAlarm(alarmdata);
        }


        public void ItemValueChangedEvenr(ItemBindingData ItemBindingData)
        {
            if (this.OnItemValueChangedEvent != null)
                this.OnItemValueChangedEvent(ItemBindingData);
        }
    }
}
