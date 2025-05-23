using ProductClientHub.Communication.Requests;
using ProductClientHub.Communication.Responses;
using ProductClientHub.Exceptions.ExceptionsBase;
using ProductClientHub.API.Infraestructure;

namespace ProductClientHub.API.UseCases.Tasks.Register
{
    public class RegisterTasksUserCase
    {
        public bool Execute(TaskRequest request)
        {

            var validator = new RegisterTasksValidator();

            var result = validator.Validate(request);

            if (!result.IsValid)
            {
                var errors = result.Errors.Select(failure => failure.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errors);
            }

            var repository = new TaskRepository();

            var success = repository.Add(request);
            
            return success == true;

        }
    }
}