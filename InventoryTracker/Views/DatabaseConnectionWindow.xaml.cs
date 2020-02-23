using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using InventoryTracker.ViewModels;

namespace InventoryTracker.Views
{
    /// <summary>
    /// Interaction logic for DatabaseConnectionWindow.xaml
    /// </summary>
    public partial class DatabaseConnectionWindow : Window
    {
        private static readonly Regex NumberOnlyRegex = new Regex("[^0-9.-]+");
        private bool _allowClose = false;

        public DatabaseConnectionWindow()
        {
            InitializeComponent();
            ((DatabaseConnectionViewModel)this.DataContext).CloseDialog += OnCloseDialog;
            this.Closing += OnClosing;
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            if (_allowClose)
                return;

            e.Cancel = true;
        }

        private void OnCloseDialog(object? sender, EventArgs e)
        {
            _allowClose = true;
            this.Close();
        }

        private static bool IsTextAllowed(string text)
        {
            return !NumberOnlyRegex.IsMatch(text);
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void TextBoxPasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                var text = (string)e.DataObject.GetData(typeof(string));
                if (!IsTextAllowed(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            var dataContext = (DatabaseConnectionViewModel) this.DataContext;
            dataContext.SqlServerConnectionInfo.Password = ((PasswordBox) sender).Password;
        }
    }
}
