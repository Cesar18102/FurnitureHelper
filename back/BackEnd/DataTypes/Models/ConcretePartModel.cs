namespace Models
{
    public class ConcretePartModel : PartModel, IModel
    {
        public MaterialModel SelectedMaterial { get; private set; }
        public PartColorModel SelectedColor { get; private set; }
    }
}