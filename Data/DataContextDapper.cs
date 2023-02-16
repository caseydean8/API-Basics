





// video Database connection 7:50
using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DotnetAPI.Data
{

    class DataContextDapper
    {
        private readonly IConfiguration _config;

        public DataContextDapper(IConfiguration config)
        {
            _config = config;
        }

        public IEnumerable<T> LoadData<T>(string sql)
        {
            // interface System.Data.IDbConnection
            // Represents an open connection to a data source, and is implemented by .NET data providers 
            // that access relational databases\.
            IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return dbConnection.Query<T>(sql);
        }

        public T LoadDataSingle<T>(string sql)
        {
            IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return dbConnection.QuerySingle<T>(sql);
        }

        public bool ExecuteSql(string sql)
        {
            IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return dbConnection.Execute(sql) > 0;
        }

        public int ExecuteSqlWithRowCount(string sql)
        {
            IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return dbConnection.Execute(sql);
        }

        // Authorization
        public bool ExecuteSqlWithParameters(string sql, List<SqlParameter> parameters)
        {
            // class Microsoft.Data.SqlClient.SqlCommand
            // Represents a Transact-SQL statement or stored procedure to execute against a SQL Server database\. This class cannot be inherited\.
            SqlCommand commandWithParams = new SqlCommand(sql);

            foreach (SqlParameter parameter in parameters)
            {
                // SqlParameter SqlParameterCollection.Add(SqlParameter value) (+ 4 overloads)
                // Adds the specified `SqlParameter` object to the `SqlParameterCollection`\.
                commandWithParams.Parameters.Add(parameter);
            }
            // class Microsoft.Data.SqlClient.SqlConnection
            // Represents a connection to a SQL Server database. This class cannot be inherited.
            SqlConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            dbConnection.Open();

            commandWithParams.Connection = dbConnection;

            // int SqlCommand.ExecuteNonQuery()
            // Executes a Transact-SQL statement against the connection and returns the number of rows affected\.
            int rowsAffected = commandWithParams.ExecuteNonQuery();

            dbConnection.Close();

            return rowsAffected > 0;
        }
    }
}










