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

namespace slSecure.Forms
{
    public partial class User : Page
    {

        slSecure.Web.SecureDBContext db;
        tblUserGroup[] userGroups;
        public User()
        {
            InitializeComponent();
        }

        // 使用者巡覽至這個頁面時執行。
        protected   async override void OnNavigatedTo(NavigationEventArgs e)
        {
           
        }

        private   void tblUserDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {

            if (e.HasError)
            {
                System.Windows.MessageBox.Show(e.Error.ToString(), "Load Error", System.Windows.MessageBoxButton.OK);
                e.MarkErrorAsHandled();
            }

             

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            tblUserDomainDataSource.SubmitChanges();
        }

        private void tblUserGroupDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {

            if (e.HasError)
            {
                System.Windows.MessageBox.Show(e.Error.ToString(), "Load Error", System.Windows.MessageBoxButton.OK);
                e.MarkErrorAsHandled();
            }
        }

        private async void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
 

            //EntityQuery<tblUserGroup> q = db.GetTblUserGroupQuery();
            //var res = await db.LoadAsync<tblUserGroup>(q);

            //userGroups = res.ToArray();

            //(this.Resources["userGroups"] as MyUserGroupList) = userGroups;
        }

        private void btnSave_Copy_Click(object sender, RoutedEventArgs e)
        {
          //  this.tblUserDataGrid.ItemsSource= 
            this.tblUserDomainDataSource.DataView.Add(new tblUser() { GroupID = 1, UserID = "", Enable = true, Password = "", Memo = "", UpdateDate = DateTime.Now, UserName = "" });
        }

        private void btnSave_Copy1_Click(object sender, RoutedEventArgs e)
        {
            if(this.tblUserDataGrid.SelectedItem==null)
                return;
            this.tblUserDomainDataSource.DataView.Remove(this.tblUserDataGrid.SelectedItem);
        }

    }


    public class MyUserGroupList
    {
        slSecure.Web.SecureDBContext _Context;

        public MyUserGroupList()
        {
            Context = new SecureDBContext(); // slSecure.DB.GetDB();
        }
        public slSecure.Web.SecureDBContext Context
        {


            set
            {
                _Context = value;
                _Context.Load(_Context.GetTblUserGroupQuery());


            }
        }

        public EntitySet<slSecure.Web.tblUserGroup> List
        {
            get
            {
                return _Context.tblUserGroups;
            }
        }

    }
}
