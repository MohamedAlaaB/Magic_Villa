﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Magic_Villa_Web.DTOs
{
    public class RegisterationRequest
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        
        public string Role { get; set; }
    }
}
