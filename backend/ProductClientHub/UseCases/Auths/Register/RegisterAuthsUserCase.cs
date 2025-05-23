using ProductClientHub.Communication.Requests;
using ProductClientHub.Communication.Responses;
using ProductClientHub.Exceptions.ExceptionsBase;

namespace ProductClientHub.API.UseCases.Auths.Register
{
    public class RegisterAuthsUserCase
    {
        public AuthResponse Execute(AuthRequest request)
        {

            var validator = new RegisterAuthsValidator();

            var result = validator.Validate(request);

            if (!result.IsValid)
            {
                var errors = result.Errors.Select(failure => failure.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errors);
            }
            return new AuthResponse();

        }
    }
}
