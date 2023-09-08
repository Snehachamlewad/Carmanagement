using C_INFO.Interfaces;
using C_INFO.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using C_INFO.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace C_INFO.ViewModels
{
        public class JWTManagerRepository : IJWTMangerRepository
        {
            Dictionary<string, string> UserRecords;



            private readonly IConfiguration configuration;
            private readonly CarContext db;



            public JWTManagerRepository(IConfiguration _configuration, CarContext _db)
            {
                db = _db;
                configuration = _configuration;
            }
            public Tokens Authenicate(LoginViewModel registerViewModel, bool IsRegister)
            {
                var _isAdmin = false;
                if (IsRegister)
                {
                    RegisterTbl tblLogin = new RegisterTbl();
                    tblLogin.UserName = registerViewModel.UserName;
                    tblLogin.Password = registerViewModel.Password;
                    tblLogin.PhoneNo = registerViewModel.PhoneNo;
                    tblLogin.Email = registerViewModel.Email;
                    db.RegisterTbls.Add(tblLogin);
                    db.SaveChanges();
                }
                /*else if (db.RegisterTbls.Any(x => x.UserName == registerViewModel.UserName && x.Password == registerViewModel.Password))
                {
                    _isRestaurant = db.RegisterTbls.Any(x => x.UserName == registerViewModel.UserName && x.Password == registerViewModel.Password && x.IsRestaurant == 1);
                }*/
                else
                {
                    _isAdmin = db.RegisterTbls.Any(x => x.UserName == registerViewModel.UserName && x.Password == registerViewModel.Password && x.IsAdmin == 1);
                }
                UserRecords = db.RegisterTbls.ToList().ToDictionary(x => x.UserName, x => x.Password);
                if (!UserRecords.Any(x => x.Key == registerViewModel.UserName && x.Value == registerViewModel.Password))
                {
                    return null;
                }



                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenkey = Encoding.UTF8.GetBytes(configuration["JWT:Key"]);



                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[] {
                new Claim(ClaimTypes.Name,registerViewModel.UserName)
                }),
                    Expires = DateTime.UtcNow.AddMinutes(10),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenkey), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return new Tokens { Token = tokenHandler.WriteToken(token), IsAdmin = _isAdmin };
            }
        }
    }
