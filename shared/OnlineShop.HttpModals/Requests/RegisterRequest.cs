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

    public string Name { get; set; }
   
    [Required, EmailAddress]
    public string Email { get; set; }
   
    [Required]

    public string Password { get; set; }
}
