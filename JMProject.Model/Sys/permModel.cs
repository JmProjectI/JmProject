﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JMProject.Model.Sys
{
    [Serializable]
    public class permModel
    {
        public string KeyCode { get; set; }//操作码
        public bool IsValid { get; set; }//是否验证
    }
}
