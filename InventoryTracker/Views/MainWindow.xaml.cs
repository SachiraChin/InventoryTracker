using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using InventoryTracker.Models;

namespace InventoryTracker.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _raiseBorrowedDateChange = true;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BorrowedDatePicker_OnSelectedDateChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (_raiseBorrowedDateChange == false)
                return;

            var datePicker = (DatePicker) sender;
            if (datePicker.SelectedDate == null)
                return;

            var context = (InventoryItemModel) datePicker.DataContext;
            context.BorrowedDate = new DateTimeOffset(datePicker.SelectedDate.Value);

            var parent = GetParent<DataGridCell>(datePicker);
            if (parent != null)
            {
                parent.IsEditing = false;
            }
        }


        private T GetParent<T>(DependencyObject d) where T : class
        {
            while (d != null && !(d is T))
            {
                d = VisualTreeHelper.GetParent(d);
            }
            return d as T;
        }

        private void BorrowedDatePicker_OnLoaded(object sender, RoutedEventArgs e)
        {
            _raiseBorrowedDateChange = false;

            var datePicker = (DatePicker)sender;
            if (datePicker.SelectedDate != null)
                return;

            if (datePicker.DataContext == null)
                return;

            var context = (InventoryItemModel)datePicker.DataContext;
            datePicker.SelectedDate = context.BorrowedDate.Date;

            _raiseBorrowedDateChange = true;
        }
    }
}
