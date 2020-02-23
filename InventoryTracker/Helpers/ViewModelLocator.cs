using AutoMapper;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using InventoryTracker.DataContext;
using InventoryTracker.Models;
using InventoryTracker.ViewModels;
using InventoryTracker.Views;

namespace InventoryTracker.Helpers
{
    public class ViewModelLocator
    {
        public MainViewModel MainViewModel => SimpleIoc.Default.GetInstance<MainViewModel>();
        public DatabaseConnectionViewModel DatabaseConnectionViewModel => SimpleIoc.Default.GetInstance<DatabaseConnectionViewModel>();

        public ViewModelLocator()
        {
            var navigationService = new WindowNavigationService();
            navigationService.RegisterWindow<MainWindow>("Main", () => MainViewModel);
            navigationService.RegisterWindow<DatabaseConnectionWindow>("DatabaseConnection", () => DatabaseConnectionViewModel, () => DatabaseConnectionViewModel);

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<DatabaseConnectionViewModel>();
            SimpleIoc.Default.Register<IWindowNavigationService>(() => navigationService);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Region, RegionModel>();
                cfg.CreateMap<Division, DivisionModel>();
                cfg.CreateMap<InventoryItem, InventoryItemModel>();

                cfg.CreateMap<RegionModel, Region>();
                cfg.CreateMap<DivisionModel, Division>();
                cfg.CreateMap<InventoryItemModel, InventoryItem>();
            });
            SimpleIoc.Default.Register<IMapper>(() => config.CreateMapper());
        }
    }
}
