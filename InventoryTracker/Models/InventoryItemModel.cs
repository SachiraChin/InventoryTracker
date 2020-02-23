using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using GalaSoft.MvvmLight;
using InventoryTracker.Helpers;

namespace InventoryTracker.Models
{
    public class InventoryItemModel : ObservableObject
    {
        private int _regionId;
        private RegionModel _region;
        private int _inventoryItemId;
        private string _borrowerName;
        private string _borrowerNIC;
        private string _itemSerialNumber;
        private string _itemType;
        private int _divisionId;
        private DivisionModel _division;
        private string _unit;
        private string _itemCondition;
        private bool _returned;
        private DateTimeOffset _borrowedDate = DateTimeOffset.Now;
        private DateTimeOffset? _returnedDate;

        public int InventoryItemId
        {
            get => _inventoryItemId;
            set
            {
                _inventoryItemId = value;
                RaisePropertyChanged();
            }
        }

        public string BorrowerName
        {
            get => _borrowerName;
            set
            {
                _borrowerName = value;
                RaisePropertyChanged();
            }
        }

        public string BorrowerNIC
        {
            get => _borrowerNIC;
            set
            {
                _borrowerNIC = value;
                RaisePropertyChanged();
            }
        }

        public string ItemSerialNumber
        {
            get => _itemSerialNumber;
            set
            {
                _itemSerialNumber = value;
                RaisePropertyChanged();
            }
        }

        public string ItemType
        {
            get => _itemType;
            set
            {
                _itemType = value;
                RaisePropertyChanged();
            }
        }

        public int RegionId
        {
            get => _regionId;
            set
            {
                _regionId = value;
                SetDivisions(value);
                RaisePropertyChanged();
            }
        }

        public RegionModel Region
        {
            get => _region;
            set
            {
                _region = value;
                if (value != null)
                    SetDivisions(value.RegionId);
                RaisePropertyChanged();
            }
        }

        public int DivisionId
        {
            get => _divisionId;
            set
            {
                _divisionId = value;
                RaisePropertyChanged();
            }
        }

        public DivisionModel Division
        {
            get => _division;
            set
            {
                _division = value;
                RaisePropertyChanged();
            }
        }

        public string Unit
        {
            get => _unit;
            set
            {
                _unit = value;
                RaisePropertyChanged();
            }
        }

        public string ItemCondition
        {
            get => _itemCondition;
            set
            {
                _itemCondition = value;
                RaisePropertyChanged();
            }
        }

        public bool Returned
        {
            get => _returned;
            set
            {
                _returned = value;
                RaisePropertyChanged();
                if (value)
                {
                    ReturnedDate = DateTimeOffset.Now;
                }
                else
                {
                    ReturnedDate = null;
                }
            }
        }

        public DateTimeOffset BorrowedDate
        {
            get => _borrowedDate;
            set
            {
                _borrowedDate = value;
                RaisePropertyChanged();
            }
        }

        public DateTimeOffset? ReturnedDate
        {
            get => _returnedDate;
            set
            {
                _returnedDate = value;
                RaisePropertyChanged();
            }
        }

        public List<DivisionModel> Divisions { get; set; }

        private void SetDivisions(int regionId)
        {
            var viewModelLocator = (ViewModelLocator)Application.Current.Resources["Locator"];
            var divisions = viewModelLocator?.MainViewModel?.Divisions;

            if (divisions == null)
                return;

            Divisions = divisions.Where(e => e.RegionId == regionId).ToList();
            if (Division != null && Divisions.Any(e => e.DivisionId == Division.DivisionId) == false)
            {
                Division = null;
            }
        }
    }
}
