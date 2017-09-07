﻿using DotCommon.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotCommon.Test.AutoMapper
{
    [AutoMap(typeof(TestOrder))]
    public class TestUser
    {
        public int UserId { get; set; }


        public string UserName { get; set; }

        public string PhoneNumber { get; set; }
    }
}
