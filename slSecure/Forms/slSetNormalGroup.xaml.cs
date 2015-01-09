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
using slSecureLib.Forms.R13;

namespace slSecure.Forms
{
    public partial class slSetNormalGroup : Page
    {
         SecureDBContext db;
        string actType;

        public slSetNormalGroup()
        {
            InitializeComponent();
        }

        // 使用者巡覽至這個頁面時執行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        //記得在void與private之間加入async
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            QueryMagneticCardNormalGroup();
        }

        async void QueryMagneticCardNormalGroup()
        {
            db = slSecure.DB.GetDB();
            //非同步模擬成同步
            var q = await db.LoadAsync<tblMagneticCardNormalGroup>(db.GetTblMagneticCardNormalGroupQuery());
            dataGrid.ItemsSource = q;
        }

        void NewMagneticCardNormalGroup()
        {
            actType = "New";

            txt_NormalID.Text = "";
            txt_NormalName.Text = "";
            tb_Memo.Text = "";
        }

        async void AddMagneticCardNormalGroup()
        {
            db = slSecure.DB.GetDB();

            //非同步模擬成同步
            var q = await db.LoadAsync<tblMagneticCardNormalGroup>(from b in db.GetTblMagneticCardNormalGroupQuery() select b);
            tblMagneticCardNormalGroup bc = q.Last();

            db.tblMagneticCardNormalGroups.Add(

               new tblMagneticCardNormalGroup()
               {
                   NormalID = bc.NormalID + 1,
                   NormalName = txt_NormalName.Text,
                   UpdateDate = DateTime.Now,
                   Memo = tb_Memo.Text
               }
               );
            try
            {
                db.SubmitChanges();
                MessageBox.Show("Data added Successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Adding Data failed due to " + ex.Message);
            }
        }

        async void ModifyMagneticCardNormalGroup()
        {
            db = slSecure.DB.GetDB();
            var normalID = int.Parse(txt_NormalID.Text);
            //非同步模擬成同步
            var q = await db.LoadAsync<tblMagneticCardNormalGroup>(from b in db.GetTblMagneticCardNormalGroupQuery() where b.NormalID == normalID select b);
            tblMagneticCardNormalGroup bc = q.First();
            bc.NormalName = txt_NormalName.Text;
            bc.UpdateDate = DateTime.Now;
            bc.Memo = tb_Memo.Text;

            try
            {
                db.SubmitChanges();
                MessageBox.Show("Data updated successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Data updation failed due to " + ex.Message);
            }

        }

        async void DeleteMagneticCardNormalGroup()
        {
            db = slSecure.DB.GetDB();
            var normalID = int.Parse(txt_NormalID.Text);
            //非同步模擬成同步
            var q = await db.LoadAsync<tblMagneticCardNormalGroup>(from b in db.GetTblMagneticCardNormalGroupQuery() where b.NormalID == normalID select b);
            tblMagneticCardNormalGroup bc = q.First();

            db.tblMagneticCardNormalGroups.Remove(bc);
            try
            {
                db.SubmitChanges();
                MessageBox.Show("Data deleted successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Data deletion failed due to " + ex.Message);
            }
        }

        private void bu_New_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("是否確定新增定期卡群組資料?", "新增", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                NewMagneticCardNormalGroup();

                QueryMagneticCardNormalGroup();
            }
        }

        private void bu_Add_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("是否確定儲存定期卡群組資料?", "儲存", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                if (actType == "New")
                {
                    AddMagneticCardNormalGroup();
                }
                else if (actType == "Update")
                {
                    ModifyMagneticCardNormalGroup();
                }
            }
            //QueryMagneticCardNormalGroup();
            //NewMagneticCardNormalGroup();
        }

        private void bu_Del_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("是否確定刪除定期卡群組資料?", "刪除", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                DeleteMagneticCardNormalGroup();
            }

            //QueryMagneticCardNormalGroup();
        }


        private void bu_Query_Click(object sender, RoutedEventArgs e)
        {
            QueryMagneticCardNormalGroup();
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            actType = "Update";
        }

        private void bu_Back_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new slSetMagneticCard();
        }


    }
}
