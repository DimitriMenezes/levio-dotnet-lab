﻿using StockMarket.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.Service.Abstract
{
    public interface IAuthenticationService
    {
        Task<ResultModel> Login(LoginModel model);
    }
}
