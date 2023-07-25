using FundoNotApplication.Entities;
using FundoNotApplication.Interface;
using FundoNotApplication.Models;
using FundoNotApplication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nest;
using System.Security.Claims;

namespace FundoNotApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
        public class UserController : ControllerBase
        {
            private readonly IUser _userServices;
            private readonly ILogger<UserController> _logger;
            private readonly IElasticClient _elasticClient;

            public UserController(IUser userServices, ILogger<UserController> logger, IElasticClient elasticClient)
            {
                _userServices = userServices;
                _logger = logger;
                _elasticClient = elasticClient;
            }

            [HttpPost]
            [Route("Register")]
            public IActionResult Register(UserRegistration newUser)
            {
                try
                {
                    UserEntity user = _userServices.Register(newUser);
                    if (user != null)
                    {
                        // Index user data in Elasticsearch
                        var indexResponse = _elasticClient.IndexDocument(user);

                        if (!indexResponse.IsValid)
                        {
                            // Handle Elasticsearch indexing failure
                            return BadRequest(new { success = false, message = "User Registration Unsuccessful" });
                        }

                        return Ok(new { success = true, message = "User Registration Successful", data = user });
                    }
                    else
                    {
                        return BadRequest(new { success = false, message = "User Registration Unsuccessful" });
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    throw;
                }
            }

            [HttpPost]
        [Route("Login")]
        public IActionResult Login(string email, string password)
        {
            try
            {
                _logger.LogError("Invalid User");
                string result = _userServices.LogIn(email, password);
                if (result != null)
                {
                    return Ok(new { success = true, message = "Login Successfull", data = result });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Login UnSuccessfull" });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }

        }

        [HttpPost]
        [Route("ForgetPassword")]
        public IActionResult ForgetPassword(string email)
        {
            try
            {
                bool result = _userServices.ForgetPassword(email);

                if (result)
                    return Ok(new { success = true, message = "Reset Email Sent " });
                else
                    return BadRequest(new { success = false, message = "Something went wrong" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }

        }
        [Authorize]
        [HttpPut]
        [Route("ResetPassword")]
        public IActionResult ResetPassword(string newPassword, string comfirmPassword)
        {
            try
            {
                string emailId = User.FindFirstValue(ClaimTypes.Email);
                bool result = _userServices.ResetPassword(newPassword, emailId, comfirmPassword);

                if (result)
                    return Ok(new { success = true, message = "Password Updated" });
                else
                    return BadRequest(new { success = false, message = "Something went wrong" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
            
        }
    }
}
