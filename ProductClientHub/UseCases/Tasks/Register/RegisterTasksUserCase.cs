using ProductClientHub.Communication.Requests;
using ProductClientHub.Communication.Responses;
using ProductClientHub.Exceptions.ExceptionsBase;

namespace ProductClientHub.API.UseCases.Tasks.Register
{
    public class RegisterTasksUserCase
    {
        public TaskResponse Execute(TaskRequest request)
        {

            var validator = new RegisterTasksValidator();

            var result = validator.Validate(request);

            if (!result.IsValid)
            {
                var errors = result.Errors.Select(failure => failure.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errors);
            }
            return new TaskResponse();

        }
    }
}