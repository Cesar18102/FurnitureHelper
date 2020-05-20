using Models.Dto.PartStore;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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
/*
			string STYLE = "";
			string THREE = "";
			string LOADER = "";
			string PART_RENDER = "";
			string SCENE_RENDER = "";

            HtmlWebViewSource source = new HtmlWebViewSource();
			source.Html =
				"<html>" +
					"<head>" +
						"<meta charset=\"utf-8\"/>" +
						"<title>Furniture Helper</title>" +
						"<link rel=\"stylesheet\" href=\"" + STYLE + "\"/>" +
						"<script src=\"" + THREE + "\"></script>" +
						"<script src=\"" + LOADER + "\"></script>" +
					"</head>" +
					"<body>" +
						"<div id=\"wrapper\"></div>" +
						"<script>" +
							"let PART_RENDER = undefined;" +
							"let PART_RENDER_PROMISE = import(\"" + PART_RENDER + "\").then(module => PART_RENDER = module);" +
							"let SCENE_RENDER = undefined;" +
							"let SCENE_RENDER_PROMISE = import(\"" + SCENE_RENDER + "\").then(module => SCENE_RENDER = module);" +
							"function initRender() { initPartRenderers(); }" +
							"async function initPartRenderers() { " +
								"if(PART_RENDER == undefined) { await PART_RENDER_PROMISE; }" +
								"if(SCENE_RENDER == undefined) { await SCENE_RENDER_PROMISE; }" +
								"let wrapper = document.getElementById(\"wrapper\");" +
								"let renderInfo = SCENE_RENDER.renderScene(wrapper, \"part-renderer\");" +
								"let partRenderInfo = {" +
									"model_url : " + Part.Part.ModelUrl + ", " +
									"texture_url : " + Material.Material.TextureUrl + ", " +
									"color : " + Color.Color.Hex +
								"};" +
								"let scale = parseFloat(" + Part.Part.Scale + ");" +
								"PART_RENDER.renderPart(" +
									"partRenderInfo, renderInfo, " +
									"part => { part.geometry.scale(scale, scale, scale); }, " +
									"part => { }" +
								");" +
							"}" +
							"initRender();" +
						"</script>" +
					"</body>" +
				"</html>";*/
        }
    }
}