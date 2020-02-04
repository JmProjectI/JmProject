using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Collections;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Dal
{
    public class DBHelperSql
    {
        // Fields
        private DbSql db;

        // Methods
        public DBHelperSql()
        {
            this.db = new DbSql();
        }

        private List<SqlParameter> GetOleDb(List<object> sp)
        {
            List<SqlParameter> sqlparemeter = new List<SqlParameter>();
            foreach (object item in sp)
            {
                sqlparemeter.Add((SqlParameter)item);
            }
            return sqlparemeter;
        }

        public virtual int Delete(string tsql)
        {
            return this.db.ExecuteNonQuery(tsql);
        }

        public virtual int Delete<T>(string where) where T : new()
        {
            int result;
            T tp = default(T);
            Type type = ((tp == null) ? Activator.CreateInstance<T>() : default(T)).GetType();
            try
            {
                StringBuilder sqlstr = new StringBuilder();
                sqlstr.Append("delete from ").Append(type.Name);
                StringBuilder sw = new StringBuilder();
                if ((where != null) && (where.Trim() != ""))
                {
                    sw.Append(" where ").Append(where);
                }
                sqlstr.Append(sw.ToString());
                result = this.db.ExecuteNonQuery(sqlstr.ToString());
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
            return result;
        }

        public virtual int Delete<T>(T model) where T : new()
        {
            int result;
            if (model == null)
            {
                throw new NotImplementedException("\"model\"不能为\"\"null\"");
            }
            Type type = model.GetType();
            try
            {
                StringBuilder sqlstr = new StringBuilder();
                sqlstr.Append("delete from ").Append(type.Name);
                StringBuilder sw = new StringBuilder();
                PropertyInfo[] vs = type.GetProperties();
                List<SqlParameter> sqlParameter = new List<SqlParameter>();
                foreach (PropertyInfo item in vs)
                {
                    if (ValidateAttribute.IsPrimaryKey(item))
                    {
                        if (sw.Length <= 0)
                        {
                            sw.Append(" where [" + item.Name + "] = @" + item.Name);
                        }
                        else
                        {
                            sw.Append(" and [" + item.Name + "] = @" + item.Name);
                        }
                        object parameter = item.GetValue(model, null);
                        if (parameter != null && parameter.ToString() != "00000000-0000-0000-0000-000000000000")
                        {
                            sqlParameter.Add(new SqlParameter("@" + item.Name, parameter));
                        }
                        else
                        {
                            sqlParameter.Add(new SqlParameter("@" + item.Name, DBNull.Value));
                        }
                    }
                }
                sqlstr.Append(sw.ToString());
                result = this.db.ExecuteNonQuery(sqlstr.ToString(), sqlParameter);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
            return result;
        }

        public virtual T GetRow<T>(T model) where T : new()
        {
            T result;
            if (model == null)
            {
                throw new NotImplementedException("\"model\"不能为\"\"null\"");
            }
            T tp = default(T);
            tp = (tp == null) ? Activator.CreateInstance<T>() : default(T);
            Type type = tp.GetType();
            try
            {
                string sqlstr = "select * from " + type.Name;
                List<SqlParameter> sqlParameter = new List<SqlParameter>();
                StringBuilder sw = new StringBuilder();
                PropertyInfo[] vs = type.GetProperties();
                foreach (PropertyInfo item in vs)
                {
                    object parameter = item.GetValue(model, null);
                    if (parameter != null && parameter.ToString() != "0" && parameter.ToString() != "00000000-0000-0000-0000-000000000000")
                    {
                        sqlParameter.Add(new SqlParameter("@" + item.Name, parameter));
                        if (sw.Length <= 0)
                        {
                            sw.Append(" where [" + item.Name + "] = @" + item.Name);
                        }
                        else
                        {
                            sw.Append(" and [" + item.Name + "] = @" + item.Name);
                        }
                    }
                }
                sqlstr = sqlstr + sw.ToString();
                DataTable data = this.db.ExecuteDataTable(sqlstr, sqlParameter);
                foreach (DataRow item in data.Rows)
                {
                    for (int i = 0; i < item.ItemArray.Length; i++)
                    {
                        if (!Convert.IsDBNull(item.ItemArray[i]))
                        {
                            tp.GetType().GetProperty(data.Columns[i].ColumnName).SetValue(tp, item.ItemArray[i], null);
                        }
                    }
                }
                result = tp;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
            return result;
        }

        public virtual T GetRow<T>(string tsql) where T : new()
        {
            T result;
            T tp = default(T);
            tp = (tp == null) ? Activator.CreateInstance<T>() : default(T);
            Type type = tp.GetType();
            try
            {
                string sqlstr = tsql;
                DataTable data = this.db.ExecuteDataTable(sqlstr);
                foreach (DataRow item in data.Rows)
                {
                    for (int i = 0; i < item.ItemArray.Length; i++)
                    {
                        if (!Convert.IsDBNull(item.ItemArray[i]))
                        {
                            tp.GetType().GetProperty(data.Columns[i].ColumnName).SetValue(tp, item.ItemArray[i], null);
                        }
                    }
                }
                result = tp;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
            return result;
        }

        public virtual object GetScalar(string tsql)
        {
            object result;
            try
            {
                result = this.db.ExecuteScalar(tsql);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
            return result;
        }

        public virtual object GetScalar<T>(T model, string cname) where T : new()
        {
            object result;
            if (model == null)
            {
                throw new NotImplementedException("\"model\"不能为\"\"null\"");
            }
            T tp = default(T);
            Type type = ((tp == null) ? Activator.CreateInstance<T>() : default(T)).GetType();
            try
            {
                string tsql = "select " + cname + " from " + type.Name;
                List<SqlParameter> sqlParameter = new List<SqlParameter>();
                StringBuilder sw = new StringBuilder();
                PropertyInfo[] vs = type.GetProperties();
                foreach (PropertyInfo item in vs)
                {
                    object parameter = item.GetValue(model, null);
                    if (parameter != null && parameter.ToString() != "00000000-0000-0000-0000-000000000000")
                    {
                        sqlParameter.Add(new SqlParameter("@" + item.Name, parameter));
                        if (sw.Length <= 0)
                        {
                            sw.Append(" where [" + item.Name + "] = @" + item.Name);
                        }
                        else
                        {
                            sw.Append(" and [" + item.Name + "] = @" + item.Name);
                        }
                    }
                }
                tsql = tsql + sw.ToString();
                result = this.db.ExecuteScalar(tsql, sqlParameter);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
            return result;
        }

        public virtual int Insert<T>(T model) where T : new()
        {
            int result;
            if (model == null)
            {
                throw new NotImplementedException("\"model\"不能为\"\"null\"");
            }
            Type type = model.GetType();
            try
            {
                StringBuilder sqlstr = new StringBuilder();
                sqlstr.Append("insert into ").Append(type.Name);
                StringBuilder sp = new StringBuilder();
                sp.Append(" (");
                StringBuilder sv = new StringBuilder();
                sv.Append(" values (");
                List<SqlParameter> sqlParameter = new List<SqlParameter>();
                PropertyInfo[] vs = type.GetProperties();
                foreach (PropertyInfo item in vs)
                {
                    if (!ValidateAttribute.IsIdentity(item))
                    {
                        if (Array.IndexOf<PropertyInfo>(vs, item) != (vs.Length - 1))
                        {
                            sp.Append("[" + item.Name + "]").Append(", ");
                            sv.Append("@").Append(item.Name).Append(", ");
                        }
                        else
                        {
                            sp.Append("[" + item.Name + "]");
                            sv.Append("@").Append(item.Name);
                        }
                        object parameter = item.GetValue(model, null);
                        if (parameter != null && parameter.ToString() != "00000000-0000-0000-0000-000000000000")
                        {
                            sqlParameter.Add(new SqlParameter("@" + item.Name, parameter));
                        }
                        else
                        {
                            sqlParameter.Add(new SqlParameter("@" + item.Name, DBNull.Value));
                        }
                    }
                }
                sp.Append(")");
                sv.Append(")");
                sqlstr.Append(sp.ToString()).Append(sv);
                result = this.db.ExecuteNonQuery(sqlstr.ToString(), sqlParameter);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
            return result;
        }

        public virtual int Insert(string tsql)
        {
            int result;
            try
            {
                result = this.db.ExecuteNonQuery(tsql);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
            return result;
        }

        public virtual int Insert<T>(T model, bool bl) where T : new()
        {
            int result;
            if (model == null)
            {
                throw new NotImplementedException("\"model\"不能为\"\"null\"");
            }
            Type type = model.GetType();
            try
            {
                StringBuilder sqlstr = new StringBuilder();
                sqlstr.Append("insert into ").Append(type.Name);
                StringBuilder sp = new StringBuilder();
                sp.Append(" (");
                StringBuilder sv = new StringBuilder();
                sv.Append(" values (");
                List<SqlParameter> sqlParameter = new List<SqlParameter>();
                PropertyInfo[] vs = type.GetProperties();
                foreach (PropertyInfo item in vs)
                {
                    if (!ValidateAttribute.IsIdentity(item))
                    {
                        object parameter = item.GetValue(model, null);
                        if ((parameter != null && parameter.ToString() != "00000000-0000-0000-0000-000000000000") || !bl)
                        {
                            sqlParameter.Add(new SqlParameter("@" + item.Name, parameter));
                            if (Array.IndexOf<PropertyInfo>(vs, item) != (vs.Length - 1))
                            {
                                sp.Append("[" + item.Name + "]").Append(", ");
                                sv.Append("@").Append(item.Name).Append(", ");
                            }
                            else
                            {
                                sp.Append("[" + item.Name + "]");
                                sv.Append("@").Append(item.Name);
                            }
                        }
                    }
                }
                sp.Append(")");
                sv.Append(")");
                sqlstr.Append(sp.ToString()).Append(sv);
                result = this.db.ExecuteNonQuery(sqlstr.ToString(), sqlParameter);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
            return result;
        }

        public virtual bool IsExists<T>(T model) where T : new()
        {
            bool result;
            if (model == null)
            {
                throw new NotImplementedException("\"model\"不能为\"\"null\"");
            }
            T tp = default(T);
            Type type = ((tp == null) ? Activator.CreateInstance<T>() : default(T)).GetType();
            try
            {
                string tsql = "select count(*) from " + type.Name;
                List<SqlParameter> sqlParameter = new List<SqlParameter>();
                StringBuilder sw = new StringBuilder();
                PropertyInfo[] vs = type.GetProperties();
                foreach (PropertyInfo item in vs)
                {
                    object parameter = item.GetValue(model, null);
                    if (parameter != null && parameter.ToString() != "00000000-0000-0000-0000-000000000000")
                    {
                        sqlParameter.Add(new SqlParameter("@" + item.Name, parameter));
                        if (sw.Length <= 0)
                        {
                            sw.Append(" where [" + item.Name + "] = @" + item.Name);
                        }
                        else
                        {
                            sw.Append(" and [" + item.Name + "] = @" + item.Name);
                        }
                    }
                }
                tsql = tsql + sw.ToString();
                if (Convert.ToInt32(this.db.ExecuteScalar(tsql, sqlParameter)) < 1)
                {
                    return false;
                }
                result = true;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
            return result;
        }

        public virtual bool IsExists(string tsql)
        {
            bool result;
            try
            {
                if (Convert.ToInt32(this.db.ExecuteScalar(tsql)) < 1)
                {
                    return false;
                }
                result = true;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
            return result;
        }

        public virtual int ProExecNonQuery(string sqlstr, List<object> sp, out int count)
        {
            int result;
            try
            {
                result = this.db.ExecuteProcNonQuery(sqlstr, this.GetOleDb(sp), out count);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
            return result;
        }

        public virtual object ProExecScalar(string sqlstr, List<object> sp)
        {
            object result;
            try
            {
                result = this.db.ExecuteProcScalar(sqlstr, this.GetOleDb(sp));
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
            return result;
        }

        public virtual List<T> ProExecSelect<T>(string sqlstr, List<object> sp) where T : new()
        {
            List<T> error = new List<T>();
            T tp = default(T);
            tp = (tp == null) ? Activator.CreateInstance<T>() : default(T);
            Type type = tp.GetType();
            try
            {
                DataTable data = this.db.ExecuteProcDataTable(sqlstr, this.GetOleDb(sp));
                if (data == null)
                {
                    return null;
                }
                foreach (DataRow item in data.Rows)
                {
                    tp = default(T);
                    T v = (tp == null) ? Activator.CreateInstance<T>() : (tp = default(T));
                    for (int i = 0; i < item.ItemArray.Length; i++)
                    {
                        if (!Convert.IsDBNull(item.ItemArray[i]))
                        {
                            v.GetType().GetProperty(data.Columns[i].ColumnName).SetValue(v, item.ItemArray[i], null);
                        }
                    }
                    error.Add(v);
                }
            }
            catch (Exception ex)
            {
                error = null;
                throw new NotImplementedException(ex.Message);
            }
            return error;
        }

        public virtual DataTable ProExecSelect(string sqlstr, List<object> sp)
        {
            DataTable result;
            try
            {
                result = this.db.ExecuteProcDataTable(sqlstr, this.GetOleDb(sp));
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
            return result;
        }

        public virtual List<T> Select<T>() where T : new()
        {
            List<T> error = new List<T>();
            T tp = default(T);
            tp = (tp == null) ? Activator.CreateInstance<T>() : default(T);
            Type type = tp.GetType();
            try
            {
                string sqlstr = "select * from " + type.Name;
                DataTable data = this.db.ExecuteDataTable(sqlstr);
                foreach (DataRow item in data.Rows)
                {
                    tp = default(T);
                    T v = (tp == null) ? Activator.CreateInstance<T>() : (tp = default(T));
                    for (int i = 0; i < item.ItemArray.Length; i++)
                    {
                        if (!Convert.IsDBNull(item.ItemArray[i]))
                        {
                            v.GetType().GetProperty(data.Columns[i].ColumnName).SetValue(v, item.ItemArray[i], null);
                        }
                    }
                    error.Add(v);
                }
            }
            catch (Exception ex)
            {
                error = null;
                throw new NotImplementedException(ex.Message);
            }
            return error;
        }

        public virtual List<T> Select<T>(T model) where T : new()
        {
            if (model == null)
            {
                throw new NotImplementedException("\"model\"不能为\"\"null\"");
            }
            List<T> error = new List<T>();
            T tp = default(T);
            tp = (tp == null) ? Activator.CreateInstance<T>() : default(T);
            Type type = tp.GetType();
            try
            {
                string sqlstr = "select * from " + type.Name;
                List<SqlParameter> sqlParameter = new List<SqlParameter>();
                StringBuilder sw = new StringBuilder();
                PropertyInfo[] vs = type.GetProperties();
                foreach (PropertyInfo item in vs)
                {
                    object parameter = item.GetValue(model, null);
                    if (parameter != null && parameter.ToString() != "00000000-0000-0000-0000-000000000000")
                    {
                        sqlParameter.Add(new SqlParameter("@" + item.Name, parameter));
                        if (sw.Length <= 0)
                        {
                            sw.Append(" where [" + item.Name + "] = @" + item.Name);
                        }
                        else
                        {
                            sw.Append(" and [" + item.Name + "] = @" + item.Name);
                        }
                    }
                }
                sqlstr = sqlstr + sw.ToString();
                DataTable data = this.db.ExecuteDataTable(sqlstr, sqlParameter);
                foreach (DataRow item in data.Rows)
                {
                    tp = default(T);
                    T v = (tp == null) ? Activator.CreateInstance<T>() : (tp = default(T));
                    for (int i = 0; i < item.ItemArray.Length; i++)
                    {
                        if (!Convert.IsDBNull(item.ItemArray[i]))
                        {
                            v.GetType().GetProperty(data.Columns[i].ColumnName).SetValue(v, item.ItemArray[i], null);
                        }
                    }
                    error.Add(v);
                }
            }
            catch (Exception ex)
            {
                error = null;
                throw new NotImplementedException(ex.Message);
            }
            return error;
        }
        public virtual List<T> Select<T>(string sqlstr) where T : new()
        {
            List<T> error = new List<T>();
            T tp = default(T);
            tp = (tp == null) ? Activator.CreateInstance<T>() : default(T);
            Type type = tp.GetType();
            try
            {
                DataTable data = this.db.ExecuteDataTable(sqlstr);
                foreach (DataRow item in data.Rows)
                {
                    tp = default(T);
                    T v = (tp == null) ? Activator.CreateInstance<T>() : (tp = default(T));
                    for (int i = 0; i < item.ItemArray.Length; i++)
                    {
                        if (!Convert.IsDBNull(item.ItemArray[i]))
                        {
                            v.GetType().GetProperty(data.Columns[i].ColumnName).SetValue(v, item.ItemArray[i], null);
                        }
                    }
                    error.Add(v);
                }
            }
            catch (Exception ex)
            {
                error = null;
                throw new NotImplementedException(ex.Message);
            }
            return error;
        }

        public virtual DataTable Select(string tsql)
        {
            DataTable result;
            try
            {
                result = this.db.ExecuteDataTable(tsql);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
            return result;
        }

        public virtual DataTable Select(string tsql, List<object> sp)
        {
            DataTable result;
            try
            {
                result = this.db.ExecuteDataTable(tsql, this.GetOleDb(sp));
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
            return result;
        }

        public virtual bool TransactionSame<T>(List<T> model) where T : new()
        {
            bool result;
            T tp = default(T);
            Type type = ((tp == null) ? Activator.CreateInstance<T>() : default(T)).GetType();
            List<TransactionSql> tab = new List<TransactionSql>();
            try
            {
                foreach (T tmo in model)
                {
                    if (tmo != null)
                    {
                        StringBuilder sqlstr = new StringBuilder();
                        sqlstr.Append("insert into ").Append(type.Name);
                        StringBuilder sp = new StringBuilder();
                        sp.Append(" (");
                        StringBuilder sv = new StringBuilder();
                        sv.Append(" values (");
                        List<SqlParameter> sqlParameter = new List<SqlParameter>();
                        PropertyInfo[] vs = type.GetProperties();
                        foreach (PropertyInfo item in vs)
                        {
                            if (!ValidateAttribute.IsIdentity(item))
                            {
                                if (Array.IndexOf<PropertyInfo>(vs, item) != (vs.Length - 1))
                                {
                                    sp.Append("[" + item.Name + "]").Append(", ");
                                    sv.Append("@").Append(item.Name).Append(", ");
                                }
                                else
                                {
                                    sp.Append("[" + item.Name + "]");
                                    sv.Append("@").Append(item.Name);
                                }
                                object parameter = item.GetValue(tmo, null);
                                if (parameter != null && parameter.ToString() != "00000000-0000-0000-0000-000000000000")
                                {
                                    sqlParameter.Add(new SqlParameter("@" + item.Name, parameter));
                                }
                                else
                                {
                                    sqlParameter.Add(new SqlParameter("@" + item.Name, DBNull.Value));
                                }

                                //object parameter = item.GetValue(model, null);
                                //if (parameter != null && parameter.ToString() != "00000000-0000-0000-0000-000000000000")
                                //{
                                //    sqlParameter.Add(new SqlParameter("@" + item.Name, parameter));
                                //    if (Array.IndexOf<PropertyInfo>(vs, item) != (vs.Length - 1))
                                //    {
                                //        sp.Append("[" + item.Name + "]").Append(", ");
                                //        sv.Append("@").Append(item.Name).Append(", ");
                                //    }
                                //    else
                                //    {
                                //        sp.Append("[" + item.Name + "]");
                                //        sv.Append("@").Append(item.Name);
                                //    }
                                //}
                            }
                        }
                        sp.Append(")");
                        sv.Append(")");
                        sqlstr.Append(sp.ToString()).Append(sv);

                        TransactionSql tsmodel = new TransactionSql()
                        {
                            Tsql = sqlstr.ToString(),
                            SqlParameter = sqlParameter
                        };
                        tab.Add(tsmodel);
                    }
                }
                this.db.ExecuteSqlTran(tab);
                result = true;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
            return result;
        }

        public virtual bool Transaction<T>(List<T> model) where T : new()
        {
            bool result;
            T tp = default(T);
            Type type = ((tp == null) ? Activator.CreateInstance<T>() : default(T)).GetType();
            Dictionary<string, object> tab = new Dictionary<string, object>();
            try
            {
                foreach (T tmo in model)
                {
                    if (tmo != null)
                    {
                        StringBuilder sqlstr = new StringBuilder();
                        sqlstr.Append("insert into ").Append(type.Name);
                        StringBuilder sp = new StringBuilder();
                        sp.Append(" (");
                        StringBuilder sv = new StringBuilder();
                        sv.Append(" values (");
                        List<SqlParameter> sqlParameter = new List<SqlParameter>();
                        PropertyInfo[] vs = type.GetProperties();
                        foreach (PropertyInfo item in vs)
                        {
                            if (!ValidateAttribute.IsIdentity(item))
                            {
                                object parameter = item.GetValue(tmo, null);
                                if (parameter != null && parameter.ToString() != "00000000-0000-0000-0000-000000000000")
                                {
                                    sqlParameter.Add(new SqlParameter("@" + item.Name, parameter));
                                    if (Array.IndexOf<PropertyInfo>(vs, item) != (vs.Length - 1))
                                    {
                                        sp.Append("[" + item.Name + "]").Append(", ");
                                        sv.Append("@").Append(item.Name).Append(", ");
                                    }
                                    else
                                    {
                                        sp.Append("[" + item.Name + "]");
                                        sv.Append("@").Append(item.Name);
                                    }
                                }
                            }
                        }
                        sp.Append(")");
                        sv.Append(")");
                        sqlstr.Append(sp.ToString()).Append(sv);

                        tab.Add(sqlstr.ToString(), sqlParameter);
                    }
                }
                this.db.ExecuteSqlTran(tab);
                result = true;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
            return result;
        }

        public virtual bool Transaction(Dictionary<string, object> tsqls)
        {
            bool result;
            try
            {
                this.db.ExecuteSqlTran(tsqls);
                result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public virtual bool Transaction(Hashtable tab)
        {
            bool result;
            try
            {
                this.db.ExecuteSqlTran(tab);
                result = true;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
            return result;
        }

        public virtual int Update(string tsql)
        {
            int num2;
            try
            {
                num2 = this.db.ExecuteNonQuery(tsql);
            }
            catch (Exception exception)
            {
                throw new NotImplementedException(exception.Message);
            }
            return num2;
        }

        public virtual int Update<T>(T model) where T : new()
        {
            int result;
            if (model == null)
            {
                throw new NotImplementedException("\"model\"不能为\"\"null\"");
            }
            Type type = model.GetType();
            try
            {
                StringBuilder sqlstr = new StringBuilder();
                sqlstr.Append("update ").Append(type.Name).Append(" set ");
                StringBuilder sp = new StringBuilder();
                StringBuilder sw = new StringBuilder();
                List<SqlParameter> sqlParameter = new List<SqlParameter>();
                PropertyInfo[] vs = type.GetProperties();
                foreach (PropertyInfo item in vs)
                {
                    if (ValidateAttribute.IsPrimaryKey(item))
                    {
                        if (sw.Length <= 0)
                        {
                            sw.Append(" where [" + item.Name + "] = @" + item.Name);
                        }
                        else
                        {
                            sw.Append(" and [" + item.Name + "] = @" + item.Name);
                        }
                    }
                    else if (!ValidateAttribute.IsIdentity(item))
                    {
                        if (Array.IndexOf<PropertyInfo>(vs, item) != (vs.Length - 1))
                        {
                            sp.Append("[" + item.Name + "] = @" + item.Name + ", ");
                        }
                        else
                        {
                            sp.Append("[" + item.Name + "] = @" + item.Name);
                        }
                    }
                    object parameter = item.GetValue(model, null);
                    if (parameter != null && parameter.ToString() != "00000000-0000-0000-0000-000000000000")
                    {
                        sqlParameter.Add(new SqlParameter("@" + item.Name, parameter));
                    }
                    else
                    {
                        sqlParameter.Add(new SqlParameter("@" + item.Name, DBNull.Value));
                    }
                }
                sqlstr.Append(sp.ToString()).Append(sw.ToString());
                result = this.db.ExecuteNonQuery(sqlstr.ToString(), sqlParameter);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="where"></param>
        /// <param name="isp">TRUE 主键条件</param>
        /// <param name="bl">TRUE 不更改</param>
        /// <returns></returns>
        public virtual int Update<T>(T model, string where, bool isp, bool bl) where T : new()
        {
            int result;
            if (model == null)
            {
                throw new NotImplementedException("\"model\"不能为\"\"null\"");
            }
            Type type = model.GetType();
            try
            {
                StringBuilder sqlstr = new StringBuilder();
                sqlstr.Append("update ").Append(type.Name).Append(" set ");
                StringBuilder sp = new StringBuilder();
                StringBuilder sw = new StringBuilder();
                if ((where != null) && (where.Trim() != ""))
                {
                    sw.Append(" where ").Append(where);
                }
                List<SqlParameter> sqlParameter = new List<SqlParameter>();
                PropertyInfo[] vs = type.GetProperties();
                foreach (PropertyInfo item in vs)
                {
                    if ((item.GetValue(model, null) != null) || !bl)
                    {
                        if (ValidateAttribute.IsPrimaryKey(item))
                        {
                            if (isp)
                            {
                                if (sw.Length <= 0)
                                {
                                    sw.Append(" where [" + item.Name + "] = @" + item.Name);
                                }
                                else
                                {
                                    sw.Append(" and [" + item.Name + "] = @" + item.Name);
                                }
                            }
                        }
                        else if (!ValidateAttribute.IsIdentity(item))
                        {
                            if (Array.IndexOf<PropertyInfo>(vs, item) != (vs.Length - 1))
                            {
                                sp.Append("[" + item.Name + "] = @" + item.Name + ", ");
                            }
                            else
                            {
                                sp.Append("[" + item.Name + "] = @" + item.Name);
                            }
                        }
                        object parameter = item.GetValue(model, null);
                        if (parameter != null && parameter.ToString() != "00000000-0000-0000-0000-000000000000")
                        {
                            sqlParameter.Add(new SqlParameter("@" + item.Name, parameter));
                        }
                        else
                        {
                            sqlParameter.Add(new SqlParameter("@" + item.Name, DBNull.Value));
                        }
                    }
                }
                sqlstr.Append(sp.ToString().TrimEnd().TrimEnd(',')).Append(sw.ToString());
                result = this.db.ExecuteNonQuery(sqlstr.ToString(), sqlParameter);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
            return result;
        }
    }
}