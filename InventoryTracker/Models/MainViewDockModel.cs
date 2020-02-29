using System;
using System.Collections.Generic;
using System.Text;
using GalaSoft.MvvmLight;

namespace InventoryTracker.Models
{
    public class MainViewDockModel : ObservableObject
    {
        private bool _showRightPane;
        private int _inventoryGridColumnSpan;
        private bool _showMiddleButton;
        private int _leftPaneGridsColumnSpan;
        private bool _showRightButton;

        public bool ShowRightPane
        {
            get => _showRightPane;
            set
            {
                _showRightPane = value;
                RaisePropertyChanged();

                if (value)
                {
                    InventoryGridColumnSpan = 1;
                    ShowMiddleButton = true;
                    LeftPaneGridsColumnSpan = 2;
                    ShowRightButton = false;
                }
                else
                {
                    InventoryGridColumnSpan = 3;
                    ShowMiddleButton = false;
                    LeftPaneGridsColumnSpan = 1;
                    ShowRightButton = true;
                }
            }
        }

        public int InventoryGridColumnSpan
        {
            get => _inventoryGridColumnSpan;
            set
            {
                _inventoryGridColumnSpan = value;
                RaisePropertyChanged();
            }
        }

        public bool ShowMiddleButton
        {
            get => _showMiddleButton;
            set
            {
                _showMiddleButton = value;
                RaisePropertyChanged();
            }
        }

        public int LeftPaneGridsColumnSpan
        {
            get => _leftPaneGridsColumnSpan;
            set
            {
                _leftPaneGridsColumnSpan = value;
                RaisePropertyChanged();
            }
        }

        public bool ShowRightButton
        {
            get => _showRightButton;
            set
            {
                _showRightButton = value;
                RaisePropertyChanged();
            }
        }

        public MainViewDockModel(bool showRightPane)
        {
            ShowRightPane = showRightPane;
        }
    }
}
