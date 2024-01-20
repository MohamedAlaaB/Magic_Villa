﻿using static Magic_Villa_Utility.SD;

namespace Magic_Villa_Web.Models
{
    public class APIRequest
    {
        public ApiType ApiType { get; set; } = ApiType.GET;

        public string Url { get; set; }
        public object Data { get; set; }

        public string Token { get; set; }
    }
}
