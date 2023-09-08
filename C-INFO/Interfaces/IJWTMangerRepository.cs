using C_INFO.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace C_INFO.Interfaces
{
   
    
        public interface IJWTMangerRepository
        {
            Tokens Authenicate(LoginViewModel users, bool IsRegister);
        }
    
}
