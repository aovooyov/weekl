using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Weekl.Core.Entities;

namespace Weekl.Core.Repository
{
    public abstract class BaseRepository
    {
        public string ConnectionString { get; set; }

        public BaseRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        protected SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        protected SqlParameter GetParameter<T>(string name, T? val) where T : struct
        {
            var sqlParameter1 = new SqlParameter();
            var str = name;
            sqlParameter1.ParameterName = str;
            var sqlParameter2 = sqlParameter1;
            if (val.HasValue)
            {
                sqlParameter2.Value = val.Value;
            }
            else
            {
                sqlParameter2.Value = default(T);
                var sqlDbType = sqlParameter2.SqlDbType;
                sqlParameter2.Value = DBNull.Value;
                sqlParameter2.SqlDbType = sqlDbType;
            }
            return sqlParameter2;
        }

        protected SqlParameter GetParameter(string name, string val)
        {
            var sqlParameter = new SqlParameter();
            var str = name;
            sqlParameter.ParameterName = str;
            var obj = string.IsNullOrEmpty(val) || val.Trim().Length == 0 ? DBNull.Value : (object)val.Trim();
            sqlParameter.Value = obj;
            return sqlParameter;
        }

        protected SqlParameter GetParameter(string name, string val, SqlDbType sqlDbType)
        {
            var sqlParameter = new SqlParameter();
            var str = name;
            sqlParameter.ParameterName = str;
            var obj = string.IsNullOrEmpty(val) || val.Trim().Length == 0 ? DBNull.Value : (object)val.Trim();
            sqlParameter.Value = obj;
            var num = (int)sqlDbType;
            sqlParameter.SqlDbType = (SqlDbType)num;
            return sqlParameter;
        }

        protected SqlParameter GetParameter(string name, string val, bool allowWhiteSpaces)
        {
            SqlParameter sqlParameter1;
            if (allowWhiteSpaces)
            {
                var sqlParameter2 = new SqlParameter();
                var str = name;
                sqlParameter2.ParameterName = str;
                var obj = string.IsNullOrEmpty(val) ? DBNull.Value : (object)val;
                sqlParameter2.Value = obj;
                sqlParameter1 = sqlParameter2;
            }
            else
            {
                sqlParameter1 = new SqlParameter();
                var str = name;
                sqlParameter1.ParameterName = str;
                var obj = string.IsNullOrEmpty(val) || val.Trim().Length == 0 ? DBNull.Value : (object)val.Trim();
                sqlParameter1.Value = obj;
            }
            return sqlParameter1;
        }

        protected SqlParameter GetParameter<T>(string name, T val, ParameterDirection direction = ParameterDirection.Input) where T : struct
        {
            var sqlParameter = new SqlParameter(name, val);
            var num = (int)direction;
            sqlParameter.Direction = (ParameterDirection)num;
            return sqlParameter;
        }

        protected SqlParameter GetParameter(string name, SqlDbType type, ParameterDirection direction = ParameterDirection.Output)
        {
            if (type == SqlDbType.NVarChar)
            {
                var sqlParameter = new SqlParameter(name, type);
                var num1 = (int)direction;
                sqlParameter.Direction = (ParameterDirection)num1;
                var num2 = 8000;
                sqlParameter.Size = num2;
                return sqlParameter;
            }
            var sqlParameter1 = new SqlParameter(name, type);
            var num = (int)direction;
            sqlParameter1.Direction = (ParameterDirection)num;
            return sqlParameter1;
        }

        protected SqlParameter GetParameter(string name, byte[] val)
        {
            var sqlParameter = new SqlParameter();
            var str = name;
            sqlParameter.ParameterName = str;
            var num = 7;
            sqlParameter.SqlDbType = (SqlDbType)num;
            var obj = val == null || val.Length == 0 ? DBNull.Value : (object)val;
            sqlParameter.Value = obj;
            return sqlParameter;
        }

        protected SqlCommand GetCommand(string text, CommandType commandType, params SqlParameter[] parameters)
        {
            var connection = GetConnection();
            var sqlCommand1 = new SqlCommand(text, connection);
            var num1 = (int)commandType;
            sqlCommand1.CommandType = (CommandType)num1;
            var num2 = 14400;
            sqlCommand1.CommandTimeout = num2;
            var sqlCommand2 = sqlCommand1;
            if (commandType == CommandType.StoredProcedure && parameters != null && (uint)parameters.Length > 0U)
                sqlCommand2.Parameters.AddRange(parameters);
            return sqlCommand2;
        }

        protected SqlCommand GetCommand(string text, SqlConnection connection, SqlTransaction transaction, CommandType commandType, params SqlParameter[] parameters)
        {
            var sqlCommand1 = new SqlCommand(text, connection, transaction);
            var num1 = (int)commandType;
            sqlCommand1.CommandType = (CommandType)num1;
            var num2 = 14400;
            sqlCommand1.CommandTimeout = num2;
            var sqlCommand2 = sqlCommand1;
            if (commandType == CommandType.StoredProcedure && parameters != null && (uint)parameters.Length > 0U)
                sqlCommand2.Parameters.AddRange(parameters);
            return sqlCommand2;
        }

        protected int ExecuteNonQuery(string text, params SqlParameter[] parameters)
        {
            using (var command = GetCommand(text, CommandType.StoredProcedure, parameters))
            {
                if (command.Connection.State == ConnectionState.Open)
                    return command.ExecuteNonQuery();
                using (command.Connection)
                {   
                    command.Connection.Open();
                    return command.ExecuteNonQuery();
                }
            }
        }

        protected int ExecuteNonQuery(string text, SqlTransaction transaction, params SqlParameter[] parameters)
        {
            using (var command = GetCommand(text, transaction.Connection, transaction, CommandType.StoredProcedure, parameters))
            {
                if (command.Connection.State == ConnectionState.Open)
                    return command.ExecuteNonQuery();
                using (command.Connection)
                {
                    command.Connection.Open();
                    return command.ExecuteNonQuery();
                }
            }
        }

        protected T ExecuteScalar<T>(string text, T defVal, params SqlParameter[] parameters)
        {
            using (var command = GetCommand(text, CommandType.Text, parameters))
            {
                object obj;
                if (command.Connection.State == ConnectionState.Open)
                {
                    obj = command.ExecuteScalar();
                }
                else
                {
                    using (command.Connection)
                    {
                        command.Connection.Open();
                        obj = command.ExecuteScalar();
                    }
                }
                return obj == null || obj == DBNull.Value ? defVal : (T)obj;
            }
        }

        protected SqlDataReader GetReader(string text, CommandType commandType, params SqlParameter[] parameters)
        {
            var command = GetCommand(text, commandType, parameters);
            var behavior1 = CommandBehavior.Default;
            if (command.Connection.State == ConnectionState.Open)
                return command.ExecuteReader(behavior1);
            command.Connection.Open();
            var behavior2 = behavior1 | CommandBehavior.CloseConnection;
            return command.ExecuteReader(behavior2);
        }

        protected T Get<T>(DbDataReader reader) where T : IBaseModel, new()
        {
            var instance = Activator.CreateInstance<T>();
            if (!reader.HasRows)
                return default(T);
            instance.Fill(reader);
            return instance;
        }

        protected T Get<T>(string text, CommandType commandType, params SqlParameter[] parameters) where T : IBaseModel, new()
        {
            using (var reader = GetReader(text, commandType, parameters))
            {
                reader.Read();
                return Get<T>(reader);
            }
        }

        protected T Get<T>(DbDataReader reader, bool closeReader) where T : IBaseModel, new()
        {
            reader.Read();
            var obj = Get<T>(reader);
            if (closeReader)
                reader.Close();
            return obj;
        }

        protected List<T> GetList<T>(string text, CommandType commandType, params SqlParameter[] parameters) where T : IBaseModel, new()
        {
            using (var reader = GetReader(text, commandType, parameters))
                return GetList<T>(reader, true);
        }

        protected List<T> GetList<T>(DbDataReader reader, bool closeReader = true) where T : IBaseModel, new()
        {
            var objList = new List<T>();
            while (reader.Read())
                objList.Add(Get<T>(reader));
            if (closeReader)
                reader.Close();
            return objList;
        }

        protected DataTable GetTable(string text, CommandType commandType, params SqlParameter[] parameters)
        {
            using (var reader = GetReader(text, commandType, parameters))
            {
                var dataTable = new DataTable();
                dataTable.Load(reader);
                return dataTable;
            }
        }

        protected SqlTransaction Transaction()
        {
            var connection = GetConnection();
            connection.Open();

            return connection.BeginTransaction();
        }
    }
}
