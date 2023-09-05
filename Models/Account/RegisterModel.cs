using System.ComponentModel.DataAnnotations;

namespace ShortenUrl.Models.Account
{
    public class RegisterModel : BaseAuthModel
    {
        [Required(ErrorMessage = "Не указан повтор пароля")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароль введен неверно")]
        public string ConfirmPassword { get; set; }
    }
}
