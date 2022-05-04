using System.ComponentModel;


namespace OptimizatonMethods
{
    public class User
    {
        [DisplayName("Идентификатор")]
        public long Id { get; set; }

        [DisplayName("Имя пользователя")]
        public string Username { get; set; } = null!;

        [DisplayName("Пароль")]
        public string Password { get; set; } = null!;
    }
}
