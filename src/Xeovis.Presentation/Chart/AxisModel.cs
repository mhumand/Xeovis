namespace Xeovis.Presentation
{
    public class AxisModel
    {
        public double MinLimit { get; set; }
        public double MaxLimit { get; set; }
        public string Label { get; set; }
        public Guid Id { get; set; }
        public bool IsLabelVisible { get; set; }
        public bool IsLimitLocked { get; set; }
        public bool IsMajorTickVisible { get; set; }
        public bool IsMinorTickVisible { get; set; }
        public bool IsMajorGridLineVisible { get; set; }
        public bool IsMinorGridLineVisible { get; set ; }

        public AxisModel()
        {
            Label = "Axis";
            MinLimit = 0;
            MaxLimit = 1;
            Id = Guid.NewGuid();
            IsLabelVisible = true;
            IsLimitLocked = false;
            IsMajorTickVisible = true;
            IsMinorTickVisible = true;
            IsMajorGridLineVisible = true;
            IsMinorGridLineVisible = true;
        }

        public AxisModel(string label, double minLimit = 0, double maxLimit = 1) : this()
        {
            Label = label;
            MinLimit = minLimit;
            MaxLimit = maxLimit;
        }
    }
}
