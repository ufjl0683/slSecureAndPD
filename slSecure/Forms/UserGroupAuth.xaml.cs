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
using Common;


namespace slSecure.Forms
{
    public partial class UserGroupAuth : Page
    {

        SecureDBContext db = Common.DB.GetDB();
        tblUserGroupMenu[] menus;
        public UserGroupAuth()
        {
            InitializeComponent();
        }

        // 使用者巡覽至這個頁面時執行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
           
        }

        private async  void tblUserGroupDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {

            if (e.HasError)
            {
                System.Windows.MessageBox.Show(e.Error.ToString(), "Load Error", System.Windows.MessageBoxButton.OK);
                e.MarkErrorAsHandled();
            }
            else
            {
                this.cboUserGroup.SelectedIndex = 0;

                var q = await  db.LoadAsync <tblUserGroupMenu>(db.GetTblUserGroupMenuQuery());
                menus = q.ToArray();
              
                //this.tblUserGroupMenuDomainDataSource.AutoLoad = true;
                //this.tblUserGroupMenuDomainDataSource.Load();
                Filter();
            }
        }

        void Filter()
        {
          tblUserGroupMenuDataGrid.ItemsSource = menus.Where(n => n.GroupID == int.Parse(cboUserGroup.SelectedValue.ToString()));
        }

        private void tblUserGroupMenuDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {

            if (e.HasError)
            {
                System.Windows.MessageBox.Show(e.Error.ToString(), "Load Error", System.Windows.MessageBoxButton.OK);
                e.MarkErrorAsHandled();
            }
        }

        private void cboUserGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (menus == null)
                return;
            Filter();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (db.HasChanges)
            {
                db.SubmitChanges();
                MessageBox.Show("OK!");
            }
            //if (this.tblUserGroupMenuDomainDataSource.HasChanges)
            //    this.tblUserGroupMenuDomainDataSource.SubmitChanges();
        }

        private void tblUserGroupMenuDomainDataSource_LoadingData(object sender, LoadingDataEventArgs e)
        {
            //if (this.tblUserGroupMenuDomainDataSource.HasChanges)
            //{
            //    e.Cancel = true;
            //    MessageBox.Show("請先完成儲存動作!");
            //}

        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            if (db.HasChanges)
                db.SubmitChanges();
        }

       

     

       
    }
}
