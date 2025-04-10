namespace Xeovis.UI.Models
{
    public class ProgressBarData
    {
        public double PrimaryValue { get; set; }
        public double SecondaryValue { get; set; }
        public string PrimaryLabel { get; set; }
        public string SecondaryLabel { get; set; }

        public ProgressBarData(double primaryValue, string primaryLabel, double secondaryValue = double.NaN, string secondaryLabel = null)
        {
            PrimaryValue = primaryValue;
            SecondaryValue = secondaryValue;
            PrimaryLabel = primaryLabel;
            SecondaryLabel = secondaryLabel;
        }
    }
}
