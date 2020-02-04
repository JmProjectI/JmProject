using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace JMProject.Dal
{
    public class TransactionSql
    {
        public string Tsql { get; set; }
        public List<SqlParameter> SqlParameter { get; set; }
    }
}
