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

using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;

using System.ServiceModel.DomainServices.Client;
using slSecure.Web;
using slSecure;
using System.Threading.Tasks;

namespace slSecureLib
{
    public class TreeViewModel : INotifyPropertyChanged
    {
        TreeViewModel(string name, string id, bool iscanselect)
        {
            Name = name;
            ID = id;
            Children = new List<TreeViewModel>();
            IsCanSelect = iscanselect;
        }

        #region Properties

   
        public string Name { get; private set; }
        public string ID { get; private set; }
        public List<TreeViewModel> Children { get; private set; }
        public bool IsInitiallySelected { get; private set; }
        public bool IsCanSelect { get; set; }
        
        //控制一開始Checkbox為「勾選」或「不勾選」的狀態
        bool? _isChecked = false;
        TreeViewModel _parent;

        #region IsChecked

        public bool? IsChecked
        {
            get { return _isChecked; }
            set { SetIsChecked(value, true, true); }
        }

        void SetIsChecked(bool? value, bool updateChildren, bool updateParent)
        {
            if (!IsCanSelect)
                return;
            if (value == _isChecked) return;

            _isChecked = value;

            if (updateChildren && _isChecked.HasValue) Children.ForEach(c => c.SetIsChecked(_isChecked, true, false));

            if (updateParent && _parent != null)
                _parent.VerifyCheckedState();

            NotifyPropertyChanged("IsChecked");
        }

        void VerifyCheckedState()
        {
            bool? state = null;

            for (int i = 0; i < Children.Count; ++i)
            {
                bool? current = Children[i].IsChecked;
                if (i == 0)
                {
                    state = current;
                }
                else if (state != current)
                {
                    state = null;
                    break;
                }
            }

            SetIsChecked(state, false, true);
        }

        #endregion

        #endregion

        void Initialize()
        {
            foreach (TreeViewModel child in Children)
            {
                child._parent = this;
                child.Initialize();
            }
        }

        public  async static Task< List<TreeViewModel>> SetTree(string topLevelName, bool IsCanSelect)
        {
            List<TreeViewModel> treeView = new List<TreeViewModel>();
            TreeViewModel tv = new TreeViewModel(topLevelName, string.Empty, IsCanSelect);

            treeView.Add(tv);
            //Perform recursive method to build treeview 

            #region Test Data
            //Doing this below for this example, you should do it dynamically 
            //***************************************************
            slSecure.Web.SecureDBContext db;
            db = slSecure.DB.GetDB();

            var ERNameData = await db.LoadAsync<tblEngineRoomConfig>(db.GetTblEngineRoomConfigIncludeQuery());
            string sERName, sReadCardName, sControlID;
            foreach (var tempERNameData in ERNameData)
            {
                sERName = tempERNameData.ERName;
                TreeViewModel tr_sERName = new TreeViewModel(sERName, string.Empty, IsCanSelect);
                if ((sERName != "泰山機房B1-UPS室") && (sERName != "龍潭堵機房") && (sERName != "觀音山機房"))
                {
                    tv.Children.Add(tr_sERName);
                }

                foreach (var tempEntranceGuardData in tempERNameData.tblEntranceGuardConfig)
                {
                    foreach (var tempControllerConfigData in tempERNameData.tblControllerConfig)
                    {
                        if ((bool)tempControllerConfigData.IsEnable)
                        {
                            if (tempControllerConfigData.EntranceCode == tempEntranceGuardData.EntranceCode && (tempControllerConfigData.ControlType == 1 || tempControllerConfigData.ControlType == 2))
                            {
                                sControlID = tempControllerConfigData.ControlID;
                                sReadCardName = tempEntranceGuardData.Memo;

                                tr_sERName.Children.Add(new TreeViewModel(sReadCardName, tempControllerConfigData.ControlID, IsCanSelect));
                            }
                        }
                    }
                }
            }
            //***************************************************
            #endregion

            tv.Initialize();

            return treeView;
        }

        public static List<string> GetTree(TreeViewModel tv)
        {
            List<string> selected = new List<string>();
            getTreeNode(tv, selected);
            //select = recursive method to check each tree view item for selection (if required)


            return selected;

            //***********************************************************
            //From your window capture selected your treeview control like:   TreeViewModel root = (TreeViewModel)TreeViewControl.Items[0];
            //                                                                List<string> selected = new List<string>(TreeViewModel.GetTree());
            //***********************************************************
        }

        public static void SetTree(TreeViewModel tv, List<string> IDList)
        {
            if (tv.Children == null || tv.Children.Count == 0)
            {
                tv.IsChecked = IDList.Contains(tv.ID);
            }
            else
            {
                foreach (TreeViewModel subTv in tv.Children)
                {
                    bool CanSelect = subTv.IsCanSelect;
                    subTv.IsCanSelect = tv.IsCanSelect;
                    SetTree(subTv, IDList);
                    subTv.IsCanSelect = CanSelect;
                }
            }
        }

        static void getTreeNode(TreeViewModel tv, List<string> list)
        {
            if (tv.Children == null || tv.Children.Count == 0)
            {
                if (tv.IsChecked.HasValue && tv.IsChecked.Value)
                {
                    list.Add(tv.ID);
                }
            }
            else
            {
                foreach (TreeViewModel subtv in tv.Children)
                {
                    getTreeNode(subtv, list);
                }
            }
        }


        #region INotifyPropertyChanged Members

        void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
