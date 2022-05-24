﻿using PSW.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSW.DTO
{
    public class TermResponse
    {
        public string Id { get; set; }
        public string DateTimeTerm { get; set; }
        public User TermUser { get; set; }
        public Doctor TermDoctor { get; set; }

        public TermResponse() { }
    }
}