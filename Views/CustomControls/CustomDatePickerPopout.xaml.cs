using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DiveLogApplication.Views.CustomControls
{
    /// <summary>
    /// Interaction logic for CustomDatePickerPopout.xaml
    /// </summary>
    public partial class CustomDatePickerPopout : UserControl
    {
        public static readonly DependencyProperty IsPopupOpenProperty =
        DependencyProperty.Register("IsPopupOpen", typeof(bool), typeof(CustomDatePickerPopout), new PropertyMetadata(false));

        public bool IsPopupOpen
        {
            get { return (bool)GetValue(IsPopupOpenProperty); }
            set { SetValue(IsPopupOpenProperty, value); }
        }

        public CustomDatePickerPopout()
        {
            InitializeComponent();
        }
    }
}
