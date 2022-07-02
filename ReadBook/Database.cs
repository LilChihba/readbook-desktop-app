using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace ReadBook
{
    public class Database
    {
        SqlConnection connection = new SqlConnection(@"workstation id=mydbreadbook.mssql.somee.com;packet size=4096;user id=danila-yurov_SQLLogin_1;pwd=788domkbj4;data source=mydbreadbook.mssql.somee.com;persist security info=False;initial catalog=mydbreadbook");

        public void Open()
        {
            if(connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
        }

        public void Close()
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }

        public ConnectionState GetState()
        {
            return connection.State;
        }

        public SqlConnection Get()
        {
            return connection;
        }
    }
}
