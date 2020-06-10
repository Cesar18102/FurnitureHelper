using Models;

using Newtonsoft.Json;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FHelper.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FurnitureView : ContentView
    {
        public static readonly BindableProperty FurnitureProperty = BindableProperty.Create(
            "Furniture", typeof(FurnitureItemDto), typeof(FurnitureItemDto)
        );

        public FurnitureItemDto Furniture
        {
            get => (FurnitureItemDto)GetValue(FurnitureProperty);
            set => SetValue(FurnitureProperty, value);
        }

        public FurnitureView(FurnitureItemDto furniture)
        {
            Furniture = furniture;

            InitializeComponent();

            Renderer.Navigated += Renderer_Navigated;
            Renderer.Focused += (sender, e) => Renderer.Unfocus();
            Renderer.Source = "http://192.168.0.108:8080/FurnitureFrontEnd/export/furniture.html";
        }

        private async void Renderer_Navigated(object sender, WebNavigatedEventArgs e)
        {
            string json = JsonConvert.SerializeObject(Furniture);
            string call = $"initRenderFurniture('{json}');";
            await Renderer.EvaluateJavaScriptAsync(call);
        }
    }
}