using ProductClientHub.API.Infraestructure;
using ProductClientHub.Communication.Requests;
using ProductClientHub.Communication.Responses;
using ProductClientHub.Exceptions.ExceptionsBase;

namespace ProductClientHub.API.UseCases.Clients.Register
{
    public class RegisterClientUserCase
    {
        public ResponseClientJson Execute(RequestClientJson request)
        {
            var validator = new RegisterClientValidator();

            var result = validator.Validate(request);
             
            if (!result.IsValid)
            {
                var errors = result.Errors.Select(failure => failure.ErrorMessage).ToList();

                 throw new ErrorOnValidationException(errors);
            }

<<<<<<< Updated upstream

            var repository = new ClientRepository();

            var success = repository.Add(request);
=======
            var repository = new ClientRepository();

            var addSuccess = repository.Add(request);
>>>>>>> Stashed changes

            return new ResponseClientJson();
        }
    }
}
