using C_INFO.Interfaces;
using C_INFO.Models;
using C_INFO.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace C_INFO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        CarContext db;
        IJWTMangerRepository iJWTMangerRepository;
        public LoginController(CarContext _db, IJWTMangerRepository _iJWTMangerRepository)
        {
            db = _db;
            iJWTMangerRepository = _iJWTMangerRepository;
        }



        [HttpGet]
        public IEnumerable<RegisterTbl> Get()
        {
            return db.RegisterTbls;
        }



        [HttpPost]
        [Route("login")]
        public IActionResult Login(LoginViewModel loginViewModel)
        {
            var token = iJWTMangerRepository.Authenicate(loginViewModel, false);
            if (token == null)
            {
                return Unauthorized();
            }
            return Ok(token);
        }
        [HttpPost]
        [Route("register")]
        public IActionResult Register(RegisterViewModel registerViewModel)
        {
            LoginViewModel login = new LoginViewModel();
            login.UserName = registerViewModel.UserName;
            login.Password = registerViewModel.Password;
            login.PhoneNo = registerViewModel.PhoneNo;
            login.Email = registerViewModel.Email;
            var token = iJWTMangerRepository.Authenicate(login, true);
            if (token == null)
            {
                return Unauthorized();
            }
            return Ok(token);
        }
    }
}
