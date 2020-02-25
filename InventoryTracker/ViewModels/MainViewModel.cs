using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AutoMapper;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using InventoryTracker.DataContext;
using InventoryTracker.Helpers;
using InventoryTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryTracker.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string _connectionString;
        private readonly IWindowNavigationService _navigationService;
        private readonly IMapper _mapper;
        private ObservableCollection<RegionModel> _regions;

        public ObservableCollection<RegionModel> Regions
        {
            get => _regions;
            set
            {
                _regions = value;
                RaisePropertyChanged();

                var list = value.Where(e => e.RegionId > 0).ToList();
                list.Insert(0, new RegionModel());
                SearchFilteredRegions = new ObservableCollection<RegionModel>(list);
            }
        }

        private bool _enableRegions = true;

        public bool EnableRegions
        {
            get => _enableRegions;
            set
            {
                _enableRegions = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<DivisionModel> _divisions;

        public ObservableCollection<DivisionModel> Divisions
        {
            get => _divisions;
            set
            {
                _divisions = value;
                RaisePropertyChanged();
            }
        }

        private bool _enableDivisions = true;

        public bool EnableDivisions
        {
            get => _enableDivisions;
            set
            {
                _enableDivisions = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<InventoryItemModel> _inventoryItems;

        public ObservableCollection<InventoryItemModel> InventoryItems
        {
            get => _inventoryItems;
            set
            {
                _inventoryItems = value;
                RaisePropertyChanged();
            }
        }

        private bool _enableInventoryItems = true;

        public bool EnableInventoryItems
        {
            get => _enableInventoryItems;
            set
            {
                _enableInventoryItems = value;
                RaisePropertyChanged();
            }
        }

        private InventoryItemModel _selectedInventoryItem;

        public InventoryItemModel SelectedInventoryItem
        {
            get => _selectedInventoryItem;
            set
            {
                _selectedInventoryItem = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<RegionModel> _searchFilteredRegions;

        public ObservableCollection<RegionModel> SearchFilteredRegions
        {
            get => _searchFilteredRegions;
            set
            {
                _searchFilteredRegions = value;
                RaisePropertyChanged();
            }
        }

        private int? _searchRegionId;

        public int? SearchRegionId
        {
            get => _searchRegionId;
            set
            {
                _searchRegionId = value;
                RaisePropertyChanged();

                var list = Divisions.Where(e => e.RegionId == value).ToList();
                list.Insert(0, new DivisionModel());
                SearchFilteredDivisions = new ObservableCollection<DivisionModel>(list);

                if (SearchFilteredDivisions.Any(e => e.DivisionId == SearchDivisionId) == false)
                {
                    SearchDivisionId = null;
                }
            }
        }

        private ObservableCollection<DivisionModel> _searchFilteredDivisions;

        public ObservableCollection<DivisionModel> SearchFilteredDivisions
        {
            get => _searchFilteredDivisions;
            set
            {
                _searchFilteredDivisions = value;
                RaisePropertyChanged();
            }
        }

        private int? _searchDivisionId;

        public int? SearchDivisionId
        {
            get => _searchDivisionId;
            set
            {
                _searchDivisionId = value;
                RaisePropertyChanged();
            }
        }

        private string _searchText;

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand<RoutedEventArgs> ViewLoadedCommand { get; set; }
        public RelayCommand SaveRegionsCommand { get; set; }
        public RelayCommand SaveDivisionsCommand { get; set; }
        public RelayCommand SaveInventoryItemsCommand { get; set; }
        public RelayCommand SearchCommand { get; set; }
        public RelayCommand<KeyEventArgs> SearchKeyUpCommand { get; set; }

        public MainViewModel(IWindowNavigationService navigationService, IMapper mapper)
        {
            _navigationService = navigationService;
            _mapper = mapper;
            ViewLoadedCommand = new RelayCommand<RoutedEventArgs>(ViewLoadedCommandHandler);
            SaveRegionsCommand = new RelayCommand(SaveRegionsCommandHandler);
            SaveDivisionsCommand = new RelayCommand(SaveDivisionsCommandHandler);
            SaveInventoryItemsCommand = new RelayCommand(SaveInventoryItemsCommandHandler);
            SearchCommand = new RelayCommand(SearchCommandHandler);
            SearchKeyUpCommand = new RelayCommand<KeyEventArgs>(SearchKeyUpCommandHandler);
        }

        private async void ViewLoadedCommandHandler(RoutedEventArgs args)
        {
            _connectionString = (string)_navigationService.ShowDialog("DatabaseConnection");
            await LoadRegions();
            await LoadDivisions();
            await LoadInventoryItems();
        }

        private async Task LoadRegions()
        {
            EnableRegions = false;

            await using var dataContext = new InventoryDataContext(_connectionString);
            var regions = await dataContext.Regions.ToListAsync();
            Regions = new ObservableCollection<RegionModel>(_mapper.Map<List<RegionModel>>(regions));

            EnableRegions = true;
        }

        private async Task LoadDivisions()
        {
            EnableDivisions = false;

            await using var dataContext = new InventoryDataContext(_connectionString);
            var divisions = await dataContext.Divisions.ToListAsync();
            Divisions = new ObservableCollection<DivisionModel>(_mapper.Map<List<DivisionModel>>(divisions));

            foreach (var divisionModel in Divisions)
            {
                divisionModel.Region = Regions.FirstOrDefault(e => e.RegionId == divisionModel.RegionId);
            }

            EnableDivisions = true;
        }

        private async Task LoadInventoryItems(int? regionId = null, int? divisionId = null, string searchText = null)
        {
            EnableInventoryItems = false;

            await using var dataContext = new InventoryDataContext(_connectionString);
            var inventoryItems = await dataContext
                .InventoryItems
                .AsNoTracking()
                .Where(i => 
                    (regionId == null || regionId <= 0 || i.RegionId == regionId)
                    && (divisionId == null || divisionId <= 0 || i.DivisionId == divisionId)
                    && (searchText == null || searchText == ""
                                           || i.InventoryItemId.ToString().Contains(searchText)
                                           || i.BorrowerName.Contains(searchText) 
                                           || i.BorrowerNIC.Contains(searchText) 
                                           || i.ItemSerialNumber.Contains(searchText) 
                                           || i.ItemType.Contains(searchText) 
                                           || i.Region.RegionName.Contains(searchText)
                                           || i.Division.DivisionName.Contains(searchText)
                                           || i.Unit.Contains(searchText)
                                           )
                )
                .ToListAsync();
            InventoryItems = new ObservableCollection<InventoryItemModel>(_mapper.Map<List<InventoryItemModel>>(inventoryItems));

            foreach (var inventoryItemModel in InventoryItems)
            {
                inventoryItemModel.Region = Regions.FirstOrDefault(e => e.RegionId == inventoryItemModel.RegionId);
                inventoryItemModel.Division = Divisions.FirstOrDefault(e => e.DivisionId == inventoryItemModel.DivisionId);

                inventoryItemModel.ItemHash = inventoryItemModel.GetHash();
            }

            EnableInventoryItems = true;
        }

        private async void SaveRegionsCommandHandler()
        {
            EnableRegions = false;

            await using (var dataContext = new InventoryDataContext(_connectionString))
            {

                foreach (var regionModel in Regions)
                {
                    var region = _mapper.Map<Region>(regionModel);
                    if (region.RegionId > 0)
                    {
                        dataContext.Entry(region).State = EntityState.Modified;
                    }
                    else
                    {
                        dataContext.Regions.Add(region);
                    }

                    await dataContext.SaveChangesAsync();
                }
            }

            await LoadRegions();

            EnableRegions = true;
        }

        private async void SaveDivisionsCommandHandler()
        {
            EnableDivisions = false;

            await using (var dataContext = new InventoryDataContext(_connectionString))
            {

                foreach (var divisionModel in Divisions)
                {
                    var division = _mapper.Map<Division>(divisionModel);
                    if (division.Region == null || division.Region.RegionId <= 0)
                        continue;

                    division.RegionId = division.Region.RegionId;
                    division.Region = null;
                    if (division.DivisionId > 0)
                    {
                        dataContext.Entry(division).State = EntityState.Modified;
                    }
                    else
                    {
                        dataContext.Divisions.Add(division);
                    }

                    await dataContext.SaveChangesAsync();
                }
            }

            await LoadDivisions();

            EnableDivisions = true;
        }

        private async void SaveInventoryItemsCommandHandler()
        {
            EnableInventoryItems = false;

            await using (var dataContext = new InventoryDataContext(_connectionString))
            {

                foreach (var inventoryItemModel in InventoryItems)
                {
                    if (inventoryItemModel.Region == null || inventoryItemModel.Region.RegionId <= 0 || inventoryItemModel.Division == null || inventoryItemModel.Division.DivisionId <= 0)
                        continue;

                    inventoryItemModel.RegionId = inventoryItemModel.Region.RegionId;
                    inventoryItemModel.DivisionId = inventoryItemModel.Division.DivisionId;

                    var newHash = inventoryItemModel.GetHash();
                    if (newHash.Equals(inventoryItemModel.ItemHash))
                        continue;
                    
                    var inventoryItem = _mapper.Map<InventoryItem>(inventoryItemModel);

                    inventoryItem.Region = null;
                    inventoryItem.Division = null;

                    if (inventoryItem.InventoryItemId > 0)
                    {
                        dataContext.Entry(inventoryItem).State = EntityState.Modified;
                    }
                    else
                    {
                        dataContext.InventoryItems.Add(inventoryItem);
                    }

                    await dataContext.SaveChangesAsync();
                }
            }

            await LoadInventoryItems();

            EnableInventoryItems = true;
        }

        private async void SearchCommandHandler()
        {
            await LoadInventoryItems(SearchRegionId, SearchDivisionId, SearchText);
        }

        private void SearchKeyUpCommandHandler(KeyEventArgs obj)
        {
            if (obj.Key != Key.Enter)
                return;

            SearchCommandHandler();
        }
    }
}
