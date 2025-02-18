using FluentValidation;
using TaskManagerApp.DTO;

namespace TaskManagerApp.Validator
{
    public class RegisterValidator : AbstractValidator<RegisterDto>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email alanı boş olamaz")
                .EmailAddress().WithMessage("Geçerli bir email adresi giriniz");


            RuleFor(x => x.Password)
               .NotEmpty().WithMessage("Şifre boş olamaz.")
               .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır.")
               .Matches(@"[A-Z]").WithMessage("Şifreniz en az bir büyük harf içermelidir.")
               .Matches(@"\d").WithMessage("Şifreniz en az bir rakam (0-9) içermelidir.")
               .Matches(@"[!@#$%^&*(),.?""{}|<>]").WithMessage("Şifreniz en az bir özel karakter içermelidir.");
        }
    }
}
