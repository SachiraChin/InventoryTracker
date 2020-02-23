using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;

namespace InventoryTracker.Helpers
{
    public interface IWindowNavigationService : INavigationService
    {
        void ShowWindow(string key);
        object ShowDialog(string key);
    }

    public interface IDialogValueProvider
    {
        object GetValue();
        event EventHandler CloseDialog;
    }

    public class WindowNavigationService : IWindowNavigationService
    {
        private string _currentPageKey;
        private readonly Dictionary<string, Type> _windowsDictionary = new Dictionary<string, Type>();
        private readonly Dictionary<string, Func<ViewModelBase>> _viewModelsDictionary = new Dictionary<string, Func<ViewModelBase>>();
        private readonly Dictionary<string, Func<IDialogValueProvider>> _dialogValueProviderDictionary = new Dictionary<string, Func<IDialogValueProvider>>();

        public void GoBack()
        {

        }

        public void NavigateTo(string pageKey)
        {
            ShowWindow(pageKey);
        }

        public void NavigateTo(string pageKey, object parameter)
        {
            ShowWindow(pageKey);
        }

        public object ShowDialog(string key)
        {
            var window = GetWindow(key);
            window.ShowDialog();

            var dialogProvider = _dialogValueProviderDictionary[key];
            return dialogProvider().GetValue();
        }

        public void ShowWindow(string key)
        {
            var window = GetWindow(key);
            window.Show();
        }

        public void RegisterWindow<TWindow>(string key, Func<ViewModelBase> viewModel, Func<IDialogValueProvider> dialogValueProvider = null) where TWindow : Window
        {
            if (_windowsDictionary.ContainsKey(key))
                throw new Exception("Key already exists.");

            _windowsDictionary.Add(key, typeof(TWindow));
            _viewModelsDictionary.Add(key, viewModel);
            if (dialogValueProvider != null)
            {
                _dialogValueProviderDictionary.Add(key, dialogValueProvider);
            }
        }

        public string CurrentPageKey => _currentPageKey;

        private Window GetWindow(string pageKey)
        {
            if (_windowsDictionary.ContainsKey(pageKey) == false)
                throw new Exception("Key not found.");

            var windowType = _windowsDictionary[pageKey];
            var window = (Window) Activator.CreateInstance(windowType);
            _currentPageKey = pageKey;
            return window;
        }
    }
}
