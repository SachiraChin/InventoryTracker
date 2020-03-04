using System;
using System.Collections.Generic;
using System.Text;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace InventoryTracker.Models
{
    public class RegionModel : ObservableObject
    {
        private int _regionId;
        private string _regionName;
        private bool _isDeleted;
        private DateTimeOffset? _deletedDate;

        public int RegionId
        {
            get => _regionId;
            set
            {
                _regionId = value;
                RaisePropertyChanged();
            }
        }

        public string RegionName
        {
            get => _regionName;
            set
            {
                _regionName = value;
                RaisePropertyChanged();
            }
        }

        public bool IsDeleted
        {
            get => _isDeleted;
            set
            {
                _isDeleted = value;
                RaisePropertyChanged();

                if (value)
                {
                    DeletedDate = DateTimeOffset.Now;
                }
                else
                {
                    DeletedDate = null;
                }
            }
        }

        public DateTimeOffset? DeletedDate
        {
            get => _deletedDate;
            set
            {
                _deletedDate = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand DeleteCommand { get; set; }
        public RelayCommand RestoreCommand { get; set; }

        public RegionModel()
        {
            DeleteCommand = new RelayCommand(DeleteCommandHandler);
            RestoreCommand = new RelayCommand(RestoreCommandHandler);
        }

        private void RestoreCommandHandler()
        {
            IsDeleted = false;
        }

        private void DeleteCommandHandler()
        {
            IsDeleted = true;
        }
    }
}
