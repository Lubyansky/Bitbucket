using System.ComponentModel.DataAnnotations;

namespace Bitbucket.Models.Account
{
    public class RegisterModel : BaseAuthModel
    {
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароль введен неверно")]
        public string ConfirmPassword { get; set; }
    }
}
