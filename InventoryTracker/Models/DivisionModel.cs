using System;
using System.Collections.Generic;
using System.Text;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace InventoryTracker.Models
{
    public class DivisionModel : ObservableObject
    {
        private int _divisionId;
        private string _divisionName;
        private int _regionId;
        private RegionModel _region;
        private bool _isDeleted;
        private DateTimeOffset? _deletedDate;

        public int DivisionId
        {
            get => _divisionId;
            set
            {
                _divisionId = value;
                RaisePropertyChanged();
            }
        }

        public string DivisionName
        {
            get => _divisionName;
            set
            {
                _divisionName = value;
                RaisePropertyChanged();
            }
        }

        public int RegionId
        {
            get => _regionId;
            set
            {
                _regionId = value;
                RaisePropertyChanged();
            }
        }

        public RegionModel Region
        {
            get => _region;
            set
            {
                _region = value;
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

        public DivisionModel()
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
