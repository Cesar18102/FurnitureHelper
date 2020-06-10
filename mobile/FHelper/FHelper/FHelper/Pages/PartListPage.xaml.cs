using System.Threading.Tasks;

using Autofac;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using FHelper.Views;

using Services;
using Services.Declaration;

using Models.Dto.PartStore;

namespace FHelper.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PartListPage : ContentPage
    {
        private static readonly IPartService PartService = ServicesHolder.Dependencies.Resolve<IPartService>();

        public PartListPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            PartListScroll.Children.Clear();
            await LoadPartList();
        }

        public async Task LoadPartList()
        {
            IsBusy = true;

            PartStoreDto parts = await PartService.GetParts();

            foreach (PartPositionDto part in parts.Positions)
                foreach(MaterialPositionDto material in part.MaterialPositions)
                    foreach(ColorPositionDto color in material.ColorPositions)
                    {
                        PartView view = new PartView(part, material, color);
                        PartListScroll.Children.Add(view);
                    }

            IsBusy = false;
        }
    }
}