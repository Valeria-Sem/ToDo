using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ToDo
{
    public partial class Form2 : Form
    {
        static string ConnectionString = @"Data Source=LAPTOP-I9URQIMV\SQLEXPRESS;Initial Catalog=ToDo;Integrated Security=True";
        SqlConnection conn = new SqlConnection(ConnectionString);
        SqlCommand com;
        SqlDataReader reader;

        static DataSet ds = new DataSet();
        static DataTable dt = new DataTable();
        static SqlDataAdapter dataAdapter;

        public bool admin = true;

        public Form2()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)     //вход 
        {
            Form3 frm3 = new Form3();
            conn.Open();

            com = new SqlCommand("Select * from [Family member] where login = @Login and password = @Password", conn);
            com.Parameters.Add("@Login", SqlDbType.VarChar).Value = textBox1.Text;
            com.Parameters.Add("@Password", SqlDbType.VarChar).Value = textBox2.Text;

            string login = textBox1.Text;
            string CommandText3 = "Select id_list, Task.name as task, date, [Family member].name as member, progress_mark from List, Task, [Family member]" +
         "where List.id_task=Task.id_task and [Family member].id_member=List.id_member and [Family member].name = (select name from [Family member] where login = '"+ login + "')";


            reader = com.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                int id = Convert.ToInt32(reader[0]);
                reader.Close();

                if (id == 1)                                // проверка айдишника для админа
                {
                    frm3.Show();
                    this.Hide();
                }
                else                                        // проверка айдишника для пользователя
                {
                   

                    admin = false;

                    ds.Clear();
                    dataAdapter = new SqlDataAdapter(CommandText3, ConnectionString);
                    dataAdapter.Fill(ds, "List");
                    frm3.dataGridView1.DataSource = ds.Tables["List"].DefaultView;

                    frm3.filterToolStripMenuItem.Visible = false;
                    frm3.Width = 633;
                    frm3.groupBox1.Visible = false;
                    frm3.button2.Visible = false;
                    frm3.button4.Visible = false;
                    frm3.button3.Visible = false;
                    frm3.Show();
                    this.Hide();
                }
                reader.Close();
                conn.Close();
            }
            else
            {
                MessageBox.Show("Неверно введён логин или пароль\nПовторите попытку");
                textBox1.Clear();
                textBox2.Clear();
                reader.Close();
                conn.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();
            form4.Show();
            Hide();
        }
    }
}
