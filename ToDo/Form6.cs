using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ToDo
{
    public partial class Form6 : Form
    {
        bool click = true;
        bool click2 = true;

        static string ConnectionString = @"Data Source=LAPTOP-I9URQIMV\SQLEXPRESS;Initial Catalog=ToDo;Integrated Security=True";
        SqlConnection conn = new SqlConnection(ConnectionString);
        SqlCommand com;
        SqlDataReader reader;

        static DataSet ds = new DataSet();
        static DataTable dt = new DataTable();

        public Form6()
        {
            InitializeComponent();
        }

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            if (click2)
            {
                conn.Open();
                com = new SqlCommand("Select name from [Family member], List where [Family member].id_member=List.id_member and [Family member].id_member > 1 and List.progress_mark = 'true' group by [Family member].name", conn);

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
            click2 = false;
        }

        private void comboBox2_DropDown(object sender, EventArgs e)
        {
            if (click)
            {
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
                    comboBox2.Items.Add(String.Join("", Convert.ToDateTime(dateList[0]).ToShortDateString()));
                }
            }
            click = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            conn.Open();
            com = new SqlCommand("Select count(progress_mark) from List where progress_mark='true' and id_member=(" +
                "select id_member from [Family member] where name='" + comboBox1.Text+ "') and date='" + comboBox2.Text + "'", conn);
            reader = com.ExecuteReader();
            reader.Read();
            int colvo_do = Convert.ToInt32(reader[0]);
            reader.Close();
            com = new SqlCommand("Select count(progress_mark) from List where progress_mark='false' and id_member=(" +
                "select id_member from [Family member] where name='" + comboBox1.Text + "') and date='" + comboBox2.Text + "'", conn);
            reader = com.ExecuteReader();
            reader.Read();
            int colvo_not_do = Convert.ToInt32(reader[0]);
            reader.Close();
            if (chart1.Series.Count != 0)
            {
                chart1.ChartAreas.Clear();
                chart1.Series.Clear();
            }
            chart1.ChartAreas.Add("area");
            chart1.ChartAreas[0].AxisY.Interval = 1;
            chart1.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
            chart1.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0;
            chart1.ChartAreas[0].CursorX.IsUserEnabled = true;
            chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chart1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
            chart1.Series.Add("Completed tasks");
            chart1.Series.Add("Outstanding tasks");

            chart1.Series["Completed tasks"].ChartType = SeriesChartType.Column;

            chart1.Series["Completed tasks"].Points.AddXY(comboBox1.Text, colvo_do);
            chart1.Series["Completed tasks"].Points.Last().Label = colvo_do.ToString();
            chart1.Series["Completed tasks"].Points.Last().Color = System.Drawing.Color.Yellow;
            chart1.Series["Outstanding tasks"].Points.AddXY(comboBox1.Text, colvo_not_do);
            chart1.Series["Outstanding tasks"].Points.Last().Label = colvo_not_do.ToString();

            chart1.Series["Outstanding tasks"].Points.Last().Color = System.Drawing.Color.Red;

            conn.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
