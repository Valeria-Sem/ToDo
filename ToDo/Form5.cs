using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ToDo
{
    public partial class Form5 : Form
    {
        bool click = true;
        bool click2 = true;

        static string ConnectionString = @"Data Source=LAPTOP-I9URQIMV\SQLEXPRESS;Initial Catalog=ToDo;Integrated Security=True";
        SqlConnection conn = new SqlConnection(ConnectionString);
        SqlCommand com;
        SqlDataReader reader;
        
        static DataSet ds = new DataSet();
        static DataTable dt = new DataTable();
        static SqlDataAdapter dataAdapter;

        string CommandText1 = "Select id_list, Task.name as task, date, [Family member].name as member, progress_mark from List, Task, [Family member]" +
           "where List.id_task=Task.id_task and [Family member].id_member=List.id_member";
        string CommandText2 = "Select * from Task";



        public Form5()
        {
            InitializeComponent();
        }

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            if (click)
            {
                conn.Open();
                com = new SqlCommand("Select name from Task", conn);

                reader = com.ExecuteReader();

                List<string[]> dataName = new List<string[]>();

                while (reader.Read())
                {
                    dataName.Add(new string[1]);

                    dataName[dataName.Count - 1][0] = reader[0].ToString();
                }
                reader.Close();
                conn.Close();

                foreach (string[] nameList in dataName)       
                {
                    comboBox1.Items.Add(String.Join("", nameList));
                }

            }

            click = false;
        }

        private void comboBox2_DropDown(object sender, EventArgs e)
        {  
            if (click2)
            {
                conn.Open();
                com = new SqlCommand("Select name from [Family member] where [Family member].id_member > 1", conn);

                reader = com.ExecuteReader();

                List<string[]> dataName = new List<string[]>();

                while (reader.Read())
                {
                    dataName.Add(new string[1]);

                    dataName[dataName.Count - 1][0] = reader[0].ToString();
                }
                reader.Close();
                conn.Close();

                foreach (string[] nameList in dataName)
                {
                    comboBox2.Items.Add(String.Join("", nameList));
                }
            }
            click2 = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            conn.Open();

            com = new SqlCommand("Select count (id_list, Task.name, date, [Family member].name, progress_mark) from List, Task, [Family member]" +
            "where List.id_task=Task.id_task and [Family member].id_member=List.id_member", conn);
            com = new SqlCommand("Select top 1 id_list from List order by id_list desc", conn);

            reader = com.ExecuteReader();
            reader.Read();
            int id = Convert.ToInt32(reader[0]) + 1;
            reader.Close();

            com = new SqlCommand("Select id_task from Task where name = '"+ comboBox1.Text + "'", conn);
            reader = com.ExecuteReader();
            reader.Read();
            int idTask = Convert.ToInt32(reader[0]);
            reader.Close();

            com = new SqlCommand("Select id_member from [Family member] where name = '" + comboBox2.Text + "'", conn);
            reader = com.ExecuteReader();
            reader.Read();
            int idMember = Convert.ToInt32(reader[0]);
            reader.Close();

            com = new SqlCommand("INSERT INTO List VALUES (@id, @tName, @mName, @Date, @Mark )", conn);
            com.Parameters.Add("@id", SqlDbType.Int).Value = id;
            com.Parameters.Add("@tName", SqlDbType.Int).Value = idTask; 
            com.Parameters.Add("@mName", SqlDbType.Int).Value = idMember; 
            com.Parameters.Add("@Date", SqlDbType.Date).Value = dateTimePicker1.Value.Date;
            com.Parameters.Add("@Mark", SqlDbType.Bit).Value = false;

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
