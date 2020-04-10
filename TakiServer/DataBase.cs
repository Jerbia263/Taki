using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace TakiServer
{
    class DataBase
    {
        private String connectionString;
        private SqlConnection connection;
        private SqlDataReader dataReader;

        public DataBase()
        {
            connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='C: \Users\Amir Jerbi\Documents\DataBaseSql.mdf1';Integrated Security=True;Connect Timeout=30";
            connection = new SqlConnection(connectionString);            
            dataReader = null;
        }

        public void SetUser(string name)
        {
            SqlCommand command = new SqlCommand();
            string sqlt = "INSERT INTO UsersSql(Name) VALUES('" + name + "')";
            command.CommandText = sqlt;
            connection.Open();
            command.ExecuteScalar();
            connection.Close();
        }
        public void IncreaseUserWins(string name)
        {
            SqlCommand command = new SqlCommand(); 
            string data = "";
            string sqlt = "SELECT * FROM UsersSql WHERE Name= '" + name + "'; ";
            command.CommandText = sqlt;
            connection.Open();
            command.ExecuteReader();

            while (dataReader.Read())
            {
                data = dataReader["NumOfWins"].ToString();
            }

            int wins = Int32.Parse(data);
            wins++;
            sqlt = "UPDATE UsersSql Set NumOfWins = " + wins + " WHERE Name ='" + name + "';";
            command.CommandText = sqlt;
            command.ExecuteScalar();
            connection.Close();
        }

        public bool isNameExists(string name)
        {
            SqlCommand command = new SqlCommand(); 
            string data = "";
            string sqlt = "SELECT * FROM UsersSql WHERE Name= '" + name + "'; ";
            command.CommandText = sqlt;
            connection.Open();
            command.ExecuteReader();

            while (dataReader.Read())
            {
                data = dataReader["Name"].ToString();
            }
            connection.Close();

            if (data == "")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
