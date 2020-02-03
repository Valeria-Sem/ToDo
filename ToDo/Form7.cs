using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ToDo
{
    public partial class Form7 : Form
    {
        static string ConnectionString = @"Data Source=LAPTOP-I9URQIMV\SQLEXPRESS;Initial Catalog=ToDo;Integrated Security=True";
        SqlConnection conn = new SqlConnection(ConnectionString);
        SqlCommand com;
        SqlDataReader reader;

        static DataSet ds = new DataSet();
        static DataTable dt = new DataTable();
        static SqlDataAdapter dataAdapter;

        string CommandText1 = "Select id_list, Task.name, date, [Family member].name, progress_mark from List, Task, [Family member]" +
           "where List.id_task=Task.id_task and [Family member].id_member=List.id_member";
        string CommandText2 = "Select * from Task";

        public Form7()
        {
            InitializeComponent();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            conn.Open();

            com = new SqlCommand("Select count (id_task) from Task", conn);
            com = new SqlCommand("Select top 1 id_task from Task order by id_task desc", conn);

            reader = com.ExecuteReader();
            reader.Read();
            int id = Convert.ToInt32(reader[0]) + 1;
            reader.Close();

            com = new SqlCommand("INSERT INTO Task VALUES (@id, @name)", conn);
            com.Parameters.Add("@id", SqlDbType.Int).Value = id;
            com.Parameters.Add("@name", SqlDbType.NVarChar).Value = textBox1.Text;

            com.ExecuteNonQuery();

            Form3 f3 = new Form3();
            dataAdapter = new SqlDataAdapter(CommandText1, ConnectionString);
            dataAdapter.Fill(ds, "List");
            f3.dataGridView1.DataSource = ds.Tables["List"].DefaultView;

            dataAdapter = new SqlDataAdapter(CommandText2, ConnectionString);
            dataAdapter.Fill(ds, "Task");
            f3.dataGridView2.DataSource = ds.Tables["Task"].DefaultView;

            conn.Close();
            Close();
        }
    }
}
