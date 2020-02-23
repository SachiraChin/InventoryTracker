using System;
using System.Collections.Generic;
using System.Text;
using GalaSoft.MvvmLight;

namespace InventoryTracker.Models
{
    public class DivisionModel : ObservableObject
    {
        private int _divisionId;
        private string _divisionName;
        private int _regionId;
        private RegionModel _region;

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
    }
}
