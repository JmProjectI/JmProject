using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace JMProject.Dal.TbColAttribute
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PrimaryKey : Attribute
    {
        
    }
}
