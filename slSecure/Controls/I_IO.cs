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

namespace slSecure.Controls
{
    public interface I_IO
    {
         void SetBlind(bool IsBlind);
         bool IsSelect { get; set; }

    }
}
