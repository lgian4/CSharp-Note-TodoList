using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;

namespace Note___TodoList
{
    /// <summary>
    /// The SqlHelper Class
    /// use for executing mysql query
    /// </summary>
    /// <remarks>
    /// This class can Execute Non Query and Execute Store Precedure
    /// </remarks>
    class SqlHelper
    {


        private const string server = "localhost";
        private const string databaseName = "todo";
        private const string userId = "root";
        private const string password = "";

        private string connectionString;

        public SqlHelper() : this(server, userId, databaseName, password)
        {
            //This will call the other constructor with the const values
        }

        public SqlHelper(string server, string userID, string databaseName, string password)
        {
            CreateConnection(server, userID, databaseName, password);
        }

        private void CreateConnection(string server, string userId, string databaseName, string password)
        {
            connectionString = String.Format("Server={0}; Uid={2}; Pwd={3};  Database={1};SslMode=none", server, databaseName, userId, password);

        }
        /// <summary>
        /// Execute Non Query to Mysql 
        /// </summary>
        /// <param name="sqlParameters">List of Mysql Parameter</param>
        /// <param name="storeProcedureName">Name of the Store Procedure</param>
        public void executeNonQuery(List<MySqlParameter> sqlParameters, string storeProcedureName)
        {
            try
            {
                using (MySqlConnection sqlConnection = new MySqlConnection(connectionString))
                {
                    using ( MySqlCommand sqlCommand = new MySqlCommand(storeProcedureName, sqlConnection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;

                        foreach (MySqlParameter sqlParameter in sqlParameters)
                        {
                            sqlCommand.Parameters.Add(sqlParameter);
                        }

                        sqlConnection.Open();
                        sqlCommand.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Execute Store Procedure that have result or return
        /// </summary>
        /// <typeparam name="T">return the Store Procedure Result</typeparam>
        /// <param name="sqlParameters">List of Mysql Parameter</param>
        /// <param name="storeProcedureName">Name of the Store Procedure</param>
        /// <returns></returns>
        public T executeStoreProcedure<T>(List<MySqlParameter> sqlParameters, string storeProcedureName)
        {
            try
            {
                using (MySqlConnection sqlConnection = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand sqlCommand = new MySqlCommand(storeProcedureName, sqlConnection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;

                        foreach (MySqlParameter sqlParameter in sqlParameters)
                        {
                            sqlCommand.Parameters.Add(sqlParameter);
                        }

                        sqlConnection.Open();
                        using (MySqlDataAdapter dataAdapter = new MySqlDataAdapter(sqlCommand))
                        {
                            using (DataSet dataSet = new DataSet())
                            {
                                dataAdapter.Fill(dataSet);
                                return (T)(object)dataSet;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }



    }
}
