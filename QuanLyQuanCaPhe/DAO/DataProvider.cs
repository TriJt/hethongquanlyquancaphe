using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCaPhe.DAO
{
    class DataProvider
    {
        private string connectionSTR = @"Data Source=LAPTOP-5HF4V9U4\DOANWINDOW;Initial Catalog=quanlyquanan;Integrated Security=True";

        private static DataProvider instance ;

        internal static DataProvider Instance { get {   if(instance == null) instance = new DataProvider(); return  DataProvider.instance; }

        private set => DataProvider.instance = value; }

        private DataProvider() {}

        public DataTable ExecuteQuery(string query,object [] parameter = null)
        {


            DataTable data = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionSTR))
            {
                connection.Open();
                // câu truy vấn sẽ thực thi 
                SqlCommand command = new SqlCommand(query, connection);


                if(parameter !=null )
                {
                    string[] listPara = query.Split(' ');
                    int i = 0; 
                    foreach (string item in listPara)
                    {
                        if(item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, parameter[i]);
                            i++; 
                        }
                    }
                }

                // trung gian để lấy thông tin ra 
                SqlDataAdapter adapter = new SqlDataAdapter(command);

                adapter.Fill(data);

                connection.Close();
            }
            return data; 
        }

        // số dòng thành công trong database 
        public int ExecuteNonQuery(string query, object[] parameter = null)
        {

            int data = 0; 

            using (SqlConnection connection = new SqlConnection(connectionSTR))
            {
                connection.Open();
                // câu truy vấn sẽ thực thi 
                SqlCommand command = new SqlCommand(query, connection);


                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                    }
                }
                data = command.ExecuteNonQuery();  

                connection.Close();
            }
            return data;
        }

        // đếm số lượng của dòng trong database 
        public object ExecuteScalar(string query, object[] parameter = null)
        {

            object data = 0;

            using (SqlConnection connection = new SqlConnection(connectionSTR))
            {
                connection.Open();
                // câu truy vấn sẽ thực thi 
                SqlCommand command = new SqlCommand(query, connection);


                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                    }
                }
                data = command.ExecuteScalar();

                connection.Close();
            }
            return data;
        }

    }
}
