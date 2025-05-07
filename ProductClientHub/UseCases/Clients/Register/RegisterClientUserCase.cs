using ProductClientHub.API.Communication.Requests;
using ProductClientHub.Communication.Responses;

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
                 throw new ArgumentException("Erro nos dados recebidos");
            }
            return new ResponseClientJson();
        }
    }
}
