using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductClientHub.API.Controllers;
using ProductClientHub.API.Infraestructure;
using ProductClientHub.Communication.Requests;

public class AuthControllerTests
{
    private readonly Mock<IUserAdminRepository> _mockRepo;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _mockRepo = new Mock<IUserAdminRepository>();
        var httpClientFactory = new Mock<IHttpClientFactory>();
        var config = new Mock<IConfiguration>();
        _controller = new AuthController(httpClientFactory.Object, config.Object);
    }

    [Fact]
    public void Login_ComCredenciaisValidas_DeveRetornarToken()
    {
        var email = "teste@gmail.com";
        var senha = "teste1";
        var senhaHash = BCrypt.Net.BCrypt.HashPassword(senha);

        _mockRepo.Setup(repo => repo.GetByEmail(email))
            .Returns(new AuthEntity
            {
                Id = Guid.NewGuid(),
                user = new UserAdmin { Email = email },
                passwordHash = senhaHash
            });

        var result = _controller.Login(new AuthRequestLogin
        {
            Email = email,
            Password = senha
        });

        Assert.IsType<OkObjectResult>(result);
    }
}
