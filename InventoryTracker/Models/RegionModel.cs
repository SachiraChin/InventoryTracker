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
    }
}
