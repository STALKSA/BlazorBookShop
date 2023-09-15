using System.ComponentModel.DataAnnotations;
#pragma warning disable CS8618

namespace OnlineShop.HttpModals.Requests
{
    public class LoginByPassRequest
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(30, ErrorMessage = "Пароль минимум 8 символов", MinimumLength = 8)]
        public string Password { get; set; }
    }

    public class LoginByCodeRequest
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public Guid CodeId { get; set; }
        [Required]
        public string Code { get; set; }
    }
}
