using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Dao;
using Server.Exceptions;
using Server.Models;
using Server.Security;

namespace Server.Controllers
{
    [Route("login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserDao userDao;
        private readonly IPasswordHasher passwordHasher;
        private readonly ITokenGenerator tokenGenerator;

        public LoginController(IUserDao userDao, IPasswordHasher passwordHasher, ITokenGenerator tokenGenerator)
        {
            this.userDao = userDao;
            this.passwordHasher = passwordHasher;
            this.tokenGenerator = tokenGenerator;
        }

        [HttpPost]
        public IActionResult Login(UserLogin userLogin)
        {
            IActionResult badResult = Unauthorized("Username or password is incorrect.");
            User? user;
            try
            {
                user = userDao.GetUserByUsername(userLogin.Username);
            }
            catch (DaoException)
            {
                return badResult;
            }

            if (user == null || passwordHasher.VerifyMatch(userLogin.Password, user.PasswordHash) == false)
            {
                return badResult;
            }

            UserAuthentication userAuthentication = GetUserAuthentication(user);
            return Ok(userAuthentication);
        }

        [HttpPost("register")]
        public IActionResult Register(UserLogin userLogin)
        {
            string errorMessage = "An error occurred and user was not created";
            IActionResult badResult = BadRequest(errorMessage);

            if (string.IsNullOrWhiteSpace(userLogin.Username) || string.IsNullOrWhiteSpace(userLogin.Password))
            {
                return BadRequest("Invalid username or password.");
            }

            try
            {
                User? existingUser = userDao.GetUserByUsername(userLogin.Username);
                if (existingUser != null)
                {
                    return Conflict("Username already taken.");
                }
            }
            catch (DaoException)
            {
                return StatusCode(500, errorMessage);
            }

            User user;
            try
            {
                string hashedPassword = passwordHasher.HashPassword(userLogin.Password);
                user = userDao.CreateUser(userLogin.Username, hashedPassword);
            }
            catch (DaoException)
            {
                return StatusCode(500, errorMessage);
            }

            if (user != null)
            {
                UserAuthentication userAuthentication = GetUserAuthentication(user);
                return Created("/login", userAuthentication);
            }

            return badResult;
        }

        private UserAuthentication GetUserAuthentication(User user)
        {
            string token = tokenGenerator.GenerateToken(user.Id, user.Username);
            UserAuthentication userAuthentication = new UserAuthentication()
            {
                Id = user.Id,
                Username = user.Username,
                Token = token
            };
            return userAuthentication;
        }

        [Authorize]
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("You did the thing!");
        }
    }
}
