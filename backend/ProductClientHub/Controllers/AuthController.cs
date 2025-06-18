using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductClientHub.API.Helpers;
using ProductClientHub.API.Infraestructure;
using ProductClientHub.Communication.Requests;
using ProductClientHub.Communication.Responses;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;


namespace ProductClientHub.API.Controllers
{
    [Route("api/Auth")]
    [ApiController]
    public class AuthController(IHttpClientFactory httpClientFactory, IConfiguration config) : ControllerBase
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient();
        private readonly IConfiguration _config = config;
        private static readonly Dictionary<string, string> GoogleAccessTokens = new Dictionary<string, string>();

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorManagerJson), StatusCodes.Status400BadRequest)]
        public IActionResult Login([FromBody] AuthRequestLogin request)
        {
            var repository = new UserAdminRepository();
            var client = repository.GetByEmail(request.Email);

            bool senhaValida = BCrypt.Net.BCrypt.Verify(request.Password, client.passwordHash);

            if (senhaValida)
            {
                var token = JwtTokenGenerator.Generate(client.Id, client.user.Email);
                return Ok(new AuthResponse { Token = token });
            }

            return Unauthorized();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("RegisterAdm")]
        public IActionResult Register([FromBody] AuthRequestRegister request)
        {
            var repository = new UserAdminRepository();
            var success = repository.Add(request);
            return Created(string.Empty, success);
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return Ok(new { message = "Logout realizado com sucesso." });
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public IActionResult GetAll()
        {
            var repository = new UserAdminRepository();
            var success = repository.Get();
            return Created(string.Empty, success);
        }

        [AllowAnonymous]
        [HttpGet("google/login")]
        public IActionResult GoogleLogin()
        {
            var props = new AuthenticationProperties { RedirectUri = "/api/Auth/google/callback" };
            return Challenge(props, GoogleDefaults.AuthenticationScheme);
        }

        [AllowAnonymous]
        [HttpGet("google/callback")]
        public async Task<IActionResult> GoogleCallback()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!result.Succeeded)
                return Unauthorized();

            var accessToken = result.Properties.GetTokenValue("access_token");
            var email = result.Principal.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
                return BadRequest("Email não encontrado.");

            var repository = new UserAdminRepository();
            repository.SaveGoogleAccessToken(email, accessToken);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("3fA6vM8zP7sK2xL9cQ1tE0uWmZrYvBxG");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Email, email)
            }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = "ProductClientHub",
                Audience = "ProductClientHub",
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);

            return Redirect($"http://localhost:5500/homepage.html#token={jwt}&email={email}");
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("google/calendar/create-event")]
        public async Task<IActionResult> CreateGoogleCalendarEvent([FromBody] CalendarEventDto eventDto)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(userEmail))
                return Unauthorized("Email do usuário não encontrado.");

            var repository = new UserAdminRepository();
            var accessToken = repository.GetGoogleAccessToken(userEmail);

            if (string.IsNullOrEmpty(accessToken))
                return Unauthorized("Token do Google não encontrado para este usuário.");


            var credential = GoogleCredential.FromAccessToken(accessToken);

            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Seu App"
            });

            var start = eventDto.End.AddHours(-1);

            var @event = new Google.Apis.Calendar.v3.Data.Event
            {
                Summary = eventDto.Title ?? "Sem título",
                Description = eventDto.Description ?? "",
                Start = new Google.Apis.Calendar.v3.Data.EventDateTime
                {
                    DateTime = start,
                    TimeZone = "America/Sao_Paulo"
                },
                End = new Google.Apis.Calendar.v3.Data.EventDateTime
                {
                    DateTime = eventDto.End,
                    TimeZone = "America/Sao_Paulo"
                }
            };

            try
            {
                var request = service.Events.Insert(@event, "primary");
                await request.ExecuteAsync();
                return Ok("Evento criado com sucesso.");
            }
            catch (Google.GoogleApiException ex)
            {
                return StatusCode(500, $"Erro ao criar evento no Google Calendar: {ex.Message}");
            }
            
        }
    }
}
