using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ToDo
{
    public partial class Form3 : Form
    {
        public bool click = true;

        static string ConnectionString = @"Data Source=LAPTOP-I9URQIMV\SQLEXPRESS;Initial Catalog=ToDo;Integrated Security=True";
        SqlConnection conn = new SqlConnection(ConnectionString);
        SqlCommand com;
        SqlDataReader reader;

        Form2 f2 = new Form2();

        string CommandText1 = "Select id_list, Task.name as task, date, [Family member].name as member, progress_mark from List, Task, [Family member]" +
            "where List.id_task=Task.id_task and [Family member].id_member=List.id_member";
        string CommandText2 = "Select * from Task";

        static DataSet ds = new DataSet();
        static DataTable dt = new DataTable();
        static SqlDataAdapter dataAdapter;

        public Form3()
        {
            InitializeComponent();

            if (f2.admin == true)
            {
                ds.Clear();
                dataAdapter = new SqlDataAdapter(CommandText1, ConnectionString);
                dataAdapter.Fill(ds, "List");
                dataGridView1.DataSource = ds.Tables["List"].DefaultView;

                dataAdapter = new SqlDataAdapter(CommandText2, ConnectionString);
                dataAdapter.Fill(ds, "Task");
                dataGridView2.DataSource = ds.Tables["Task"].DefaultView;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
            Close();
        }

        private void filterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (click)
            {
                ///////////////////////////// Фильтр по дате ///////////////////////////////////////////////////////
                conn.Open();
                com = new SqlCommand("Select date from List group by date ", conn);

                reader = com.ExecuteReader();

                List<string[]> dataDate = new List<string[]>();

                while (reader.Read())
                {
                    dataDate.Add(new string[1]);

                    dataDate[dataDate.Count - 1][0] = reader[0].ToString();
                }

                reader.Close();
                conn.Close();

                foreach (var dateList in dataDate)        // dateList - переменная цикла, которая перебирает записи из дат
                {
                    byDateToolStripMenuItem.DropDownItems.Add(String.Join("", Convert.ToDateTime(dateList[0]).ToShortDateString()));
                }

                ///////////////////////////// Фильтр по делу ///////////////////////////////////////////////////////

                conn.Open();
                com = new SqlCommand("Select name from Task", conn); //, List where List.id_task=Task.id_task 

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
                    byTaskToolStripMenuItem.DropDownItems.Add(String.Join("", nameList));
                }

                ///////////////////////////// Фильтр по члену ///////////////////////////////////////////////////////

                conn.Open();
                com = new SqlCommand("Select name from [Family member]  where [Family member].id_member > 1", conn);  

                reader = com.ExecuteReader();
                List<string[]> dataMember = new List<string[]>();

                while (reader.Read())
                {
                    dataMember.Add(new string[1]);

                    dataMember[dataMember.Count - 1][0] = reader[0].ToString();
                }

                reader.Close();
                conn.Close();

                foreach (string[] memberList in dataMember)
                {
                    byMemberToolStripMenuItem.DropDownItems.Add(String.Join("", memberList));
                }
            }

            click = false;
        }

        private void byDateToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            conn.Open();

            var item = e.ClickedItem;

            com = new SqlCommand("Select id_list, Task.name as task, date, [Family member].name as member, progress_mark from  List, Task, [Family member]" +
                " where date = @item and Task.id_task=List.id_task and List.id_member=[Family member].id_member", conn);
            com.Parameters.Add("@item", SqlDbType.NVarChar).Value = item.Text;

            dataAdapter.SelectCommand = com;
            ds.Reset();
            dataAdapter.Fill(ds, "List");
            dataGridView1.DataSource = ds.Tables["List"].DefaultView;

            dataAdapter = new SqlDataAdapter(CommandText2, ConnectionString);
            dataAdapter.Fill(ds, "Task");
            dataGridView2.DataSource = ds.Tables["Task"].DefaultView;

            if (ds.Tables["List"].Rows.Count == 0)
            {
                (byDateToolStripMenuItem as ToolStripDropDownItem).HideDropDown();
                MessageBox.Show("There is no task on the indicated date");
            }

            conn.Close();
        }

        private void byTaskToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            conn.Open();

            var item = e.ClickedItem;

            com = new SqlCommand("Select id_list, Task.name as task, date, [Family member].name as member, progress_mark from" +
                " List, Task, [Family member]" +
                " where Task.name = @item and Task.id_task=List.id_task and List.id_member=[Family member].id_member", conn);
            com.Parameters.Add("@item", SqlDbType.NVarChar).Value = item.Text;

            dataAdapter.SelectCommand = com;
            ds.Reset();
            dataAdapter.Fill(ds, "List");
            dataGridView1.DataSource = ds.Tables["List"].DefaultView;


            dataAdapter = new SqlDataAdapter(CommandText2, ConnectionString);
            dataAdapter.Fill(ds, "Task");
            dataGridView2.DataSource = ds.Tables["Task"].DefaultView;

            if (ds.Tables["List"].Rows.Count == 0)
            {
                (byTaskToolStripMenuItem as ToolStripDropDownItem).HideDropDown();
                MessageBox.Show("There is no task");
            }

            conn.Close();
        }

        private void byMemberToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            conn.Open();

            var item = e.ClickedItem;

            com = new SqlCommand("Select id_list, Task.name as task, date, [Family member].name as member, progress_mark from" +
                " List, Task, [Family member]" +
                " where [Family member].name = @item and Task.id_task=List.id_task and List.id_member=[Family member].id_member", conn);
            com.Parameters.Add("@item", SqlDbType.NVarChar).Value = item.Text;

            dataAdapter.SelectCommand = com;
            ds.Reset();
            dataAdapter.Fill(ds, "List");
            dataGridView1.DataSource = ds.Tables["List"].DefaultView;

            dataAdapter = new SqlDataAdapter(CommandText2, ConnectionString);
            dataAdapter.Fill(ds, "Task");
            dataGridView2.DataSource = ds.Tables["Task"].DefaultView;

            if (ds.Tables["List"].Rows.Count == 0)
            {
                (byTaskToolStripMenuItem as ToolStripDropDownItem).HideDropDown();
                MessageBox.Show("There is no member");
            }

            conn.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            com = new SqlCommand("Select id_list, Task.name as task, date, [Family member].name as member, progress_mark from List, Task, [Family member]" +
            "where List.id_task=Task.id_task and [Family member].id_member=List.id_member", conn);
            dataAdapter.SelectCommand = com;
            ds.Reset();
            dataAdapter.Fill(ds, "List");
            dataGridView1.DataSource = ds.Tables["List"].DefaultView;

            dataAdapter = new SqlDataAdapter(CommandText2, ConnectionString);
            dataAdapter.Fill(ds, "Task");
            dataGridView2.DataSource = ds.Tables["Task"].DefaultView;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form5 form5 = new Form5();
            form5.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {

            Form6 form6 = new Form6();
            form6.Show();

        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            conn.Open();
            int row = dataGridView1.CurrentRow.Index;

            int id = int.Parse(dataGridView1[0, row].Value.ToString());
            Console.WriteLine(id);
            com = new SqlCommand("update List set progress_mark='"+dataGridView1.CurrentCell.Value.ToString()+"' where id_list="+id, conn);

            com.ExecuteNonQuery();
            conn.Close();
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            Form7 form7 = new Form7();
            form7.Show();
        }

        private void referenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form9 f9 = new Form9();
            f9.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutTheProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form8 f8 = new Form8();
            f8.Show();
        }
    }
}
