using System.Threading.Tasks;
using System.Collections.Generic;

using Autofac;

using FHelper.Views;

using Models;

using Services;
using Services.Declaration;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FHelper.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FurnitureListPage : ContentPage
    {
        private static readonly IFurnitureService FurnitureService = ServicesHolder.Dependencies.Resolve<IFurnitureService>();

        public FurnitureListPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            FurnitureListScroll.Children.Clear();
            await LoadFurnitureList();
        }

        public async Task LoadFurnitureList()
        {
            IsBusy = true;

            IEnumerable<FurnitureItemDto> furnitureItems = await FurnitureService.GetFurnitureItems();

            foreach (FurnitureItemDto furnitureItem in furnitureItems)
            {
                FurnitureView view = new FurnitureView(furnitureItem);
                FurnitureListScroll.Children.Add(view);
            }

            IsBusy = false;
        }
    }
}