using FluentValidation;
using ProductClientHub.Communication.Requests;

namespace ProductClientHub.API.UseCases.Auths.Register
{
    public class RegisterAuthsValidator : AbstractValidator<AuthRequestRegister>
    {
        public RegisterAuthsValidator()
        {
            RuleFor(auth => auth.Email).EmailAddress().WithMessage("O usuário não pode ser vazio");
            RuleFor(auth => auth.Password).NotEmpty().WithMessage("A senha não é válida");      
        }
    }
}

