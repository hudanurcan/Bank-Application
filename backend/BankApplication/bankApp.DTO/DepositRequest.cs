﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bankApp.DTO
{
    public class DepositRequest
    {
        public decimal Amount { get; set; }  // Yatırılacak tutar
        public int UserId { get; set; }      
    }
}
