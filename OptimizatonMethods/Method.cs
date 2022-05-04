using System.ComponentModel;


namespace OptimizatonMethods
{
    public class Method
    {
        [DisplayName("Идентификатор")]
        public long Id { get; set; }

        [DisplayName("Название метода")]
        public string? Name { get; set; }

        [DisplayName("Активен")]
        public string? Activated { get; set; }
    }
}
