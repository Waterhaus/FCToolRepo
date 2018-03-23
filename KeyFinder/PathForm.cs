using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FCTool
{
    public partial class PathForm : Form
    {
        public Form1 main;
        public PathForm()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void PathForm_Load(object sender, EventArgs e)
        {
            string[] list = SaveInfo.GetInfo();
            textBox1.Text = list[0];
            textBox2.Text = list[1]; 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<string> list = new List<string>();
            list.Add(textBox1.Text);
            list.Add(textBox2.Text);            
            SaveInfo.Save(list);
            main.UpdatePath();
            

            this.Close();
        }
    }
}
