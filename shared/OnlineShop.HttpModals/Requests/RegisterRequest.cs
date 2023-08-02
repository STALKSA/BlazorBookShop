using System.ComponentModel.DataAnnotations;
#pragma warning disable CS8618

namespace OnlineShop.HttpModals.Requests;

//DTO

public class RegisterRequest
{
    //public RegisterRequest(string name, string email, string password)
    //{
    //    Name = name ?? throw new ArgumentNullException(nameof(name));
    //    Email = email ?? throw new ArgumentNullException(nameof(email));
    //    Password = password ?? throw new ArgumentNullException(nameof(password));
    //}

    [Required]
    [StringLength(20, ErrorMessage = "Имя должно быть не короче 3 символов", MinimumLength = 3)]
    public string Name { get; set; }
   
    [Required, EmailAddress]
    public string Email { get; set; }
   
    [Required]
    [StringLength(30, ErrorMessage = "Пароль должен содержать как минимум 8 символов", MinimumLength = 8)]
    public string Password { get; set; }

    [Required]
    [Compare(nameof(Password))]
    public string ConfirmedPassword { get; set; }
}
