using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace MuseumSystemORM.ORM.src_sql
{
    /// <summary>
    /// Represents a MS SQL Database
    /// </summary>
    public class Database
    {
        private SqlConnection Connection { get; set; }
        private SqlTransaction SqlTransaction { get; set; }
        public string Language { get; set; }

        public Database()
        {
            Connection = new SqlConnection();
            Language = "en";
        }

        public bool Connect(string connString)
        {
            if(Connection.State != ConnectionState.Open)
            {
                Connection.ConnectionString = connString;
                Connection.Open();
            }
            return true;
        }

        public bool Connect()
        {
            bool ret = true;
            if(Connection.State != ConnectionState.Open)
            {
                // connection string is stored in file App.config or Web.config
                ret = Connect(ConfigurationManager.ConnectionStrings["ConnectionStringMsSql"].ConnectionString);
            }
            return ret;
        }

        public void Close()
        {
            Connection.Close();
        }

        // Begin a transaction.
        public void BeginTransaction()
        {
            SqlTransaction = Connection.BeginTransaction(IsolationLevel.Serializable);
        }

        // End a transaction.
        public void EndTransaction()
        {
            SqlTransaction.Commit();
            Close();
        }

        // If a transaction is failed call it
        public void RollBack()
        {
            SqlTransaction.Rollback();
        }

        // Insert a record encapulated in the command.
        public int ExecuteNonQuery(SqlCommand command)
        {
            int rowNumber = 0;

            try
            {
                rowNumber = command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
            return rowNumber;
        }

        // Create command
        public SqlCommand CreateCommand(string strCommand)
        {
            SqlCommand command = new SqlCommand(strCommand, Connection);

            if (SqlTransaction != null)
            {
                command.Transaction = SqlTransaction;
            }
            return command;
        }

        // Select encapulated in the command
        public SqlDataReader Select(SqlCommand command)
        {
            SqlDataReader sqlReader = command.ExecuteReader();
            return sqlReader;
        }

    }
}
