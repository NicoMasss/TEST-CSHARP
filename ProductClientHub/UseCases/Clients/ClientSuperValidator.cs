using System.Data;
using System.Security.Authentication.ExtendedProtection;
using FluentValidation;
using ProductClientHub.Communication.Requests;

namespace ProductClientHub.API.UseCases.Clients
{
    public class ClientSuperValidator : AbstractValidator<RequestClientJson>
    {
        public ClientSuperValidator(string cenario) 
        {
            if (cenario == "register") 
            {
                RuleFor(client => client.Name).NotEmpty().WithMessage("O nome não pode ser vazio");
                RuleFor(client => client.Email).EmailAddress().WithMessage("O e-mail não é válido");
                RuleFor(client => client.Password).NotEmpty().WithMessage("A senha não pode ser vazia");
            }

            //if (cenario == "update")
            //{
            //    //TODO 
            //}

            //if (cenario == "get") 
            //{
            //    Rulefor
            //}
        }
    }
}
