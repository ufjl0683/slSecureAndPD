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

namespace Common
{
    public interface ICommon
    {

          string GetAppTiTle();
          string GetUserName();
          string GetUserID();
          void SetMain(object main);
          void Navigate(Uri uri);
    }
}
