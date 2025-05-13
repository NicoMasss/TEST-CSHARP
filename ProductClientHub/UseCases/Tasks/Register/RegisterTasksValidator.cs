using FluentValidation;
using ProductClientHub.Communication.Requests;

namespace ProductClientHub.API.UseCases.Tasks.Register
{
    public class RegisterTasksValidator : AbstractValidator<TaskRequest>
    {
        public RegisterTasksValidator()
        {
            RuleFor(task => task.Title).NotEmpty().WithMessage("O titulo não pode ser vazio");
            RuleFor(task => task.Description).NotEmpty().WithMessage("A descrição não é válida");
            RuleFor(task => task.Status).NotEmpty().WithMessage("O Status não pode ser vazio");
        } 
    }
}
