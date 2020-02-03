using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ToDo
{
    public partial class Form4 : Form
    {
        static string ConnectionString = @"Data Source=LAPTOP-I9URQIMV\SQLEXPRESS;Initial Catalog=ToDo;Integrated Security=True";
        SqlConnection conn = new SqlConnection(ConnectionString);
        SqlCommand com;
        SqlDataReader reader;

        public Form4()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == null || textBox1.Text == "" || textBox2.Text == null || textBox2.Text == "" || textBox3.Text == null || textBox3.Text == "")
            {
                MessageBox.Show("Error");
            }
            else
            {
                conn.Open();

                com = new SqlCommand("Select count (*) from [Family member]", conn);
                com = new SqlCommand("Select top 1 id_member from [Family member] order by id_member desc", conn);

                reader = com.ExecuteReader();

                reader.Read();
                int id = Convert.ToInt32(reader[0]) + 1;
                reader.Close();

                com = new SqlCommand("INSERT INTO [Family member] VALUES (@id, @Name, @Login ,@Password)", conn);
                com.Parameters.Add("@id", SqlDbType.Int).Value = id;
                com.Parameters.Add("@Name", SqlDbType.VarChar).Value = textBox1.Text;
                com.Parameters.Add("@Login", SqlDbType.VarChar).Value = textBox2.Text;
                com.Parameters.Add("@Password", SqlDbType.VarChar).Value = textBox3.Text;
                com.ExecuteNonQuery();

                conn.Close();
                reader.Close();
                this.Close();
                Form2 frm2 = new Form2();
                frm2.Show();
            }
        }
    }
}
