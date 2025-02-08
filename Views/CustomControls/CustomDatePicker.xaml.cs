using DiveLogApplication.Utilities;
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
using DiveLogApplication.ViewModels;
using DiveLogApplication.Core;
using DiveLogApplication.Utilities.TimePickerHelper;
using System.ComponentModel;

namespace DiveLogApplication.Views.CustomControls
{
    /// <summary>
    /// Interaction logic for CustomDatePicker.xaml
    /// </summary>
    public partial class CustomDatePicker : UserControl, IDataErrorInfo, INotifyPropertyChanged
    {
        public static readonly DependencyProperty HasValidationErrorProperty = 
            DependencyProperty.Register(
                nameof(HasValidationError),
                typeof(bool),
                typeof(CustomDatePicker),
                new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
                );

        public bool HasValidationError
        {
            get => (bool)GetValue(HasValidationErrorProperty);
            set => SetValue(HasValidationErrorProperty, value);
        }

        public static readonly DependencyProperty HourProperty =
            DependencyProperty.Register(
                nameof(Hour),
                typeof(int),
                typeof(CustomDatePicker),
                new FrameworkPropertyMetadata(1,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault
                    )
                );


        public int Hour
        {
            get => (int)GetValue(HourProperty);
            set => SetValue(HourProperty, value);
        }

        public static readonly DependencyProperty MinuteProperty =
            DependencyProperty.Register(
                nameof(Minute),
                typeof(int),
                typeof(CustomDatePicker),
                new FrameworkPropertyMetadata(1,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault
                    //null
                    //CoerceMinuteValue
                    )
                );

        public int Minute
        {
            get => (int)GetValue(MinuteProperty);
            set => SetValue(MinuteProperty, value);
        }

        public static readonly DependencyProperty SelectedDayOrNightProperty =
            DependencyProperty.Register(
                nameof(SelectedDayOrNight),
                typeof(DayOrNight),
                typeof(CustomDatePicker),
                new FrameworkPropertyMetadata(DayOrNight.AM, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public DayOrNight SelectedDayOrNight
        {
            get => (DayOrNight)GetValue(SelectedDayOrNightProperty);
            set => SetValue(SelectedDayOrNightProperty, value);
        }

        public List<int> Hours => TimePickerHelper.Hours;
        public List<int> Minutes => TimePickerHelper.Minutes;
        public List<DayOrNight> DayOrNightOptions => TimePickerHelper.DayOrNights;

        public CustomDatePicker()
        {
            InitializeComponent();
        }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;

                if (columnName == nameof(Hour))
                {
                    if (Hour < 1 || Hour > 12)
                        error = "Hour must be between 1 and 12.";
                }
                else if (columnName == nameof(Minute))
                {
                    if (Minute < 0 || Minute > 59)
                        error =  "Minute must be between 0 and 59.";
                }

                // Check if there are any validation errors
                bool hasError = !string.IsNullOrEmpty(error);
                if (HasValidationError != hasError)
                {
                    HasValidationError = hasError;
                }

                return error;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //private static void OnHourChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    var control = (CustomDatePicker)d;
        //    if (control.Hour < 1 || control.Hour > 12)
        //    {
        //        Validation.MarkInvalid(
        //            BindingOperations.GetBindingExpression(control, HourProperty),
        //            new ValidationError(new ExceptionValidationRule(), control)
        //            {
        //                ErrorContent = "Hour must be between 1 and 12."
        //            });
        //    }
        //    else
        //    {
        //        Validation.ClearInvalid(BindingOperations.GetBindingExpression(control, HourProperty));
        //    }
        //}

        //private static void OnMinuteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    var control = (CustomDatePicker)d;
        //    if (control.Minute < 0 || control.Minute > 0)
        //    {
        //        Validation.MarkInvalid(
        //            BindingOperations.GetBindingExpression(control, MinuteProperty),
        //            new ValidationError(new ExceptionValidationRule(), control)
        //            {
        //                ErrorContent = "Minute must be between 0 and 59"
        //            });
        //    }
        //    else
        //    {
        //        Validation.ClearInvalid(BindingOperations.GetBindingExpression(control, MinuteProperty));
        //    }
        //}

        //// Coerce callback for Hour (limits value between 1–12)
        //private static object CoerceHourValue(DependencyObject d, object baseValue)
        //{
        //    int hour = (int)baseValue;
        //    return hour <= 12 && hour >= 1 ? hour : 0;
        //}

        //// Coerce callback for Hour (limits value between 1–12)
        //private static object CoerceMinuteValue(DependencyObject d, object baseValue)
        //{
        //    int minute = (int)baseValue;
        //    return minute <= 59 && minute >= 0 ? minute : 0;
        //}

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    if (DataContext is AddNewDiveEntryViewModel viewModel)
        //    {
        //        if (sender is Button button)
        //        {
        //            // Get the button's top-left corner in physical screen coordinates.
        //            var screenPosition = button.PointToScreen(new Point(0, 0));

        //            // Convert the physical pixel coordinates to device-independent units.
        //            var presentationSource = PresentationSource.FromVisual(button);
        //            if (presentationSource != null)
        //            {
        //                // Transform from device pixels to WPF (DIU) units.
        //                var transform = presentationSource.CompositionTarget.TransformFromDevice;
        //                var wpfPosition = transform.Transform(screenPosition);

        //                viewModel.TimePickerLocationLeft = wpfPosition.X - 110;
        //                // Note: button.ActualHeight is already in DIU
        //                viewModel.TimePickerLocationTop = wpfPosition.Y + button.ActualHeight;

        //                Console.WriteLine($"WPF Position: {wpfPosition}");
        //            }
        //            else
        //            {
        //                // Fallback if PresentationSource is null.
        //                viewModel.TimePickerLocationLeft = screenPosition.X;
        //                viewModel.TimePickerLocationTop = screenPosition.Y + button.ActualHeight;
        //            }
        //        }
        //    }
        //}

    }
}

