using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Models.Dto.PartStore;

namespace FHelper.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PartView : ContentView
    {
        public static readonly BindableProperty PartProperty = BindableProperty.Create(
            "Part", typeof(PartPositionDto), typeof(PartPositionDto)
        );

        public PartPositionDto Part
        {
            get => (PartPositionDto)GetValue(PartProperty);
            set => SetValue(PartProperty, value);
        }

        public static readonly BindableProperty MaterialProperty = BindableProperty.Create(
            "Material", typeof(MaterialPositionDto), typeof(MaterialPositionDto)
        );

        public MaterialPositionDto Material
        {
            get => (MaterialPositionDto)GetValue(MaterialProperty);
            set => SetValue(MaterialProperty, value);
        }

        public static readonly BindableProperty ColorProperty = BindableProperty.Create(
            "Color", typeof(ColorPositionDto), typeof(ColorPositionDto)
        );

        public ColorPositionDto Color
        {
            get => (ColorPositionDto)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        public PartView(PartPositionDto part, MaterialPositionDto material, ColorPositionDto color)
        {
            Part = part;
            Material = material;
            Color = color;

            InitializeComponent();

            Renderer.Navigated += Renderer_Navigated;
            Renderer.Focused += (sender, e) => Renderer.Unfocus();
            Renderer.Source = "http://192.168.0.108:8080/FurnitureFrontEnd/export/part.html";
        }

        private async void Renderer_Navigated(object sender, WebNavigatedEventArgs e)
        {
            string model = $"'{Part.Part.ModelUrl}'";
            string texture = $"'{Material.Material.TextureUrl}'";
            string hex = $"'{Color.Color.Hex}'";
            string scale = $"'{Part.Part.Scale.ToString().Replace(",", ".")}'";

            string call = $"initPartRenderer({model}, {texture}, {hex}, {scale});";
            await Renderer.EvaluateJavaScriptAsync(call);
        }
    }
}