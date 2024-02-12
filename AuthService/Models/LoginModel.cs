using System.ComponentModel.DataAnnotations;

namespace AuthService.Models;

public record LoginModel
{
    [Required]
    [EmailAddress]
    public string? Email { get; init; }

    [Required]
    [StringLength(100, ErrorMessage = "La contraseña debe tener entre {2} y {1} caracteres.", MinimumLength = 6)]
    public string? Password { get; init; }
}