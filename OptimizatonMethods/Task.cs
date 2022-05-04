using PropertyChanged;


namespace OptimizatonMethods
{
    [AddINotifyPropertyChangedInterface]
    public class Task
    {
        public double? Alpha { get; set; }
        public double? Beta { get; set; }
        public double? Mu { get; set; }
        public double? H { get; set; }
        public double? N { get; set; }
        public double? Lmin { get; set; }
        public double? Lmax { get; set; }
        public double? Smin { get; set; }
        public double? Smax { get; set; }
        public double? Price { get; set; }
        public double? Step { get; set; }
        public double? DimSum { get; set; }
    }
}
