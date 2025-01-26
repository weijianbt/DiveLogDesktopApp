using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;

namespace DiveLogApplication.Utilities
{
    public static class DialogHelper
    {
        public static bool? ShowCenteredDialog(Window parentWindow, Window dialog)
        {
            if (parentWindow == null || dialog == null)
            {
                throw new ArgumentNullException("Parent window or dialog cannot be null");
            }

            dialog.WindowStartupLocation = WindowStartupLocation.Manual;
            dialog.UpdateLayout();

            // Set default width and height if not set
            if (double.IsNaN(dialog.Width) || double.IsNaN(dialog.Height))
            {
                dialog.Width = 400;
                dialog.Height = 300;
            }

            // Get the handle of the parent window
            var hwnd = new WindowInteropHelper(parentWindow).Handle;

            // Get the screen the parent window is located on
            var screen = Screen.FromHandle(hwnd);
            var workingArea = screen.WorkingArea;

            // Get the bounds of the parent window, including if it's maximized
            var mainWindowLeft = parentWindow.Left;
            var mainWindowTop = parentWindow.Top;
            var mainWindowWidth = parentWindow.Width;
            var mainWindowHeight = parentWindow.Height;

            // Center the dialog relative to the parent window
            dialog.Left = mainWindowLeft + (mainWindowWidth - dialog.Width) / 2;
            dialog.Top = mainWindowTop + (mainWindowHeight - dialog.Height) / 2;

            // Show the dialog and return its result
            return dialog.ShowDialog();
        }
    }
}
