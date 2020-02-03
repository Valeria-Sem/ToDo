using System;
using System.Windows.Forms;

namespace ToDo
{
    public partial class Form9 : Form
    {
        public Form9()
        {
            InitializeComponent();
            string path = Application.StartupPath + @"\todo.html";
            webBrowser1.Navigate(path);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
