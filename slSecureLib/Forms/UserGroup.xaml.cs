﻿using System;
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

namespace slSecureLib.Forms
{
    public partial class UserGroup : Page
    {
        public UserGroup()
        {
            InitializeComponent();
        }

        // 使用者巡覽至這個頁面時執行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            
        }

        private void tblUserGroupDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {

            if (e.HasError)
            {
                System.Windows.MessageBox.Show(e.Error.ToString(), "Load Error", System.Windows.MessageBoxButton.OK);
                e.MarkErrorAsHandled();
            }
        }

    }
}