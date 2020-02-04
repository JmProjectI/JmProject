using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;
namespace JMProject.Dal
{
    internal class DbSql
    {
        // Fields
        private string conStr;

        // Methods
        internal DbSql()
        {
            this.conStr = ConfigurationManager.ConnectionStrings["SqlConString"].ToString();
            
        }

        internal DataSet ExecuteDataSet(string SQLString)
        {
            return this.ExecuteDataSet(SQLString, null);
        }

        internal DataSet ExecuteDataSet(string SQLString, List<SqlParameter> cmdParms)
        {
            DataSet result;
            using (SqlConnection connection = new SqlConnection(this.conStr))
            {
                SqlCommand cmd = new SqlCommand(SQLString, connection);
                try
                {
                    if (cmdParms != null)
                    {
                        foreach (SqlParameter parameter in cmdParms)
                        {
                            if (((parameter.Direction == ParameterDirection.InputOutput) || (parameter.Direction == ParameterDirection.Input)) && (parameter.Value == null))
                            {
                                parameter.Value = DBNull.Value;
                            }
                            cmd.Parameters.Add(parameter);
                        }
                    }
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    sda.Fill(ds, "ds");
                    result = ds;
                }
                catch (SqlException e)
                {
                    throw e;
                }
            }
            return result;
        }

        internal DataTable ExecuteDataTable(string SQLString)
        {
            return this.ExecuteDataTable(SQLString, null);
        }

        internal DataTable ExecuteDataTable(string SQLString, List<SqlParameter> cmdParms)
        {
            DataTable result;
            using (SqlConnection connection = new SqlConnection(this.conStr))
            {
                SqlCommand cmd = new SqlCommand(SQLString, connection);
                try
                {
                    if (cmdParms != null)
                    {
                        foreach (SqlParameter parameter in cmdParms)
                        {
                            if (((parameter.Direction == ParameterDirection.InputOutput) || (parameter.Direction == ParameterDirection.Input)) && (parameter.Value == null))
                            {
                                parameter.Value = DBNull.Value;
                            }
                            cmd.Parameters.Add(parameter);
                        }
                    }
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    result = dt;
                }
                catch (SqlException e)
                {
                    throw e;
                }
            }
            return result;
        }

        internal int ExecuteNonQuery(string SQLString)
        {
            return this.ExecuteNonQuery(SQLString, null);
        }

        internal int ExecuteNonQuery(string SQLString, List<SqlParameter> cmdParms)
        {
            int result;
            using (SqlConnection connection = new SqlConnection(this.conStr))
            {
                SqlCommand cmd = new SqlCommand(SQLString, connection);
                try
                {
                    connection.Open();
                    if (cmdParms != null)
                    {
                        foreach (SqlParameter parameter in cmdParms)
                        {
                            if (((parameter.Direction == ParameterDirection.InputOutput) || (parameter.Direction == ParameterDirection.Input)) && (parameter.Value == null))
                            {
                                parameter.Value = DBNull.Value;
                            }
                            cmd.Parameters.Add(parameter);
                        }
                    }
                    int rows = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    result = rows;
                }
                catch (SqlException e)
                {
                    throw e;
                }
            }
            return result;
        }

        internal DataSet ExecuteProcDataSet(string storedProcName, List<SqlParameter> parameters)
        {
            using (SqlConnection connection = new SqlConnection(this.conStr))
            {
                DataSet dataSet = new DataSet();
                connection.Open();
                SqlDataAdapter sqlDA = new SqlDataAdapter();
                sqlDA.SelectCommand = this.GetCommand(connection, storedProcName, parameters);
                sqlDA.Fill(dataSet, "ds");
                connection.Close();
                return dataSet;
            }
        }

        internal DataTable ExecuteProcDataTable(string storedProcName, List<SqlParameter> parameters)
        {
            using (SqlConnection connection = new SqlConnection(this.conStr))
            {
                DataTable dt = new DataTable();
                connection.Open();
                SqlDataAdapter sqlDA = new SqlDataAdapter();
                sqlDA.SelectCommand = this.GetCommand(connection, storedProcName, parameters);
                sqlDA.Fill(dt);
                connection.Close();
                return dt;
            }
        }

        internal int ExecuteProcNonQuery(string storedProcName, List<SqlParameter> parameters, out int rowsAffected)
        {
            using (SqlConnection connection = new SqlConnection(this.conStr))
            {
                connection.Open();
                SqlCommand command = this.GetCommandandAddResult(connection, storedProcName, parameters);
                rowsAffected = command.ExecuteNonQuery();
                return (int)command.Parameters["ReturnValue"].Value;
            }
        }

        internal object ExecuteProcScalar(string storedProcName, List<SqlParameter> parameters)
        {
            using (SqlConnection connection = new SqlConnection(this.conStr))
            {
                connection.Open();
                SqlCommand command = this.GetCommand(connection, storedProcName, parameters);
                command.CommandType = CommandType.StoredProcedure;
                object obj = command.ExecuteScalar();
                if (object.Equals(obj, null) || object.Equals(obj, DBNull.Value))
                {
                    return null;
                }
                return obj;
            }
        }

        internal object ExecuteScalar(string SQLString)
        {
            return this.ExecuteScalar(SQLString, null);
        }

        internal object ExecuteScalar(string SQLString, List<SqlParameter> cmdParms)
        {
            object result;
            using (SqlConnection connection = new SqlConnection(this.conStr))
            {
                SqlCommand cmd = new SqlCommand(SQLString, connection);
                try
                {
                    connection.Open();
                    if (cmdParms != null)
                    {
                        foreach (SqlParameter parameter in cmdParms)
                        {
                            if (((parameter.Direction == ParameterDirection.InputOutput) || (parameter.Direction == ParameterDirection.Input)) && (parameter.Value == null))
                            {
                                parameter.Value = DBNull.Value;
                            }
                            cmd.Parameters.Add(parameter);
                        }
                    }
                    object obj = cmd.ExecuteScalar();
                    if (object.Equals(obj, null) || object.Equals(obj, DBNull.Value))
                    {
                        return null;
                    }
                    result = obj;
                }
                catch (SqlException e)
                {
                    connection.Close();
                    throw e;
                }
            }
            return result;
        }

        internal void ExecuteSqlTran(Dictionary<string, object> SQLStringList)
        {
            using (SqlConnection connection = new SqlConnection(this.conStr))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandTimeout = 0;
                    command.Transaction = transaction;
                    try
                    {
                        foreach (KeyValuePair<string, object> entry in SQLStringList)
                        {
                            string str = entry.Key.ToString();
                            command.CommandText = str;
                            SqlParameter[] parameterArray = (SqlParameter[])entry.Value;
                            if (parameterArray != null)
                            {
                                foreach (SqlParameter parameter in parameterArray)
                                {
                                    if (((parameter.Direction == ParameterDirection.InputOutput) || (parameter.Direction == ParameterDirection.Input)) && (parameter.Value == null))
                                    {
                                        parameter.Value = DBNull.Value;
                                    }
                                    command.Parameters.Add(parameter);
                                }
                            }
                            command.ExecuteNonQuery();
                            command.Parameters.Clear();
                        }
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        internal void ExecuteSqlTran(List<TransactionSql> SQLStringList)
        {
            using (SqlConnection connection = new SqlConnection(this.conStr))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandTimeout = 0;
                    command.Transaction = transaction;
                    try
                    {
                        foreach (TransactionSql entry in SQLStringList)
                        {
                            string str = entry.Tsql;
                            command.CommandText = str;
                            SqlParameter[] parameterArray = entry.SqlParameter.ToArray();
                            if (parameterArray != null)
                            {
                                foreach (SqlParameter parameter in parameterArray)
                                {
                                    if (((parameter.Direction == ParameterDirection.InputOutput) || (parameter.Direction == ParameterDirection.Input)) && (parameter.Value == null))
                                    {
                                        parameter.Value = DBNull.Value;
                                    }
                                    command.Parameters.Add(parameter);
                                }
                            }
                            command.ExecuteNonQuery();
                            command.Parameters.Clear();
                        }
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        internal void ExecuteSqlTran(Hashtable SQLStringList)
        {
            using (SqlConnection connection = new SqlConnection(this.conStr))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandTimeout = 0;
                    command.Transaction = transaction;
                    try
                    {
                        foreach (DictionaryEntry entry in SQLStringList)
                        {
                            string str = entry.Key.ToString();
                            command.CommandText = str;
                            SqlParameter[] parameterArray = (SqlParameter[])entry.Value;
                            if (parameterArray != null)
                            {
                                foreach (SqlParameter parameter in parameterArray)
                                {
                                    if (((parameter.Direction == ParameterDirection.InputOutput) || (parameter.Direction == ParameterDirection.Input)) && (parameter.Value == null))
                                    {
                                        parameter.Value = DBNull.Value;
                                    }
                                    command.Parameters.Add(parameter);
                                }
                            }
                            command.ExecuteNonQuery();
                            command.Parameters.Clear();
                        }
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        private SqlCommand GetCommand(SqlConnection connection, string storedProcName, List<SqlParameter> parameters)
        {
            SqlCommand command = new SqlCommand(storedProcName, connection);
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 0;
            foreach (SqlParameter parameter in parameters)
            {
                if (parameter != null)
                {
                    if (((parameter.Direction == ParameterDirection.InputOutput) || (parameter.Direction == ParameterDirection.Input)) && (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    command.Parameters.Add(parameter);
                }
            }
            return command;
        }

        private SqlCommand GetCommandandAddResult(SqlConnection connection, string storedProcName, List<SqlParameter> parameters)
        {
            SqlCommand command = this.GetCommand(connection, storedProcName, parameters);
            command.CommandTimeout = 0;
            command.Parameters.Add(new SqlParameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.ReturnValue, false, 0, 0, string.Empty, DataRowVersion.Default, null));
            return command;
        }
    }
}
