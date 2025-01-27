using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace lab1
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        public Form3(Form1 f)
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string before = File.ReadAllText("text.txt");
           textBox3.Text = before;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string after = File.ReadAllText("text2.txt");
            textBox1.Text = after;
        }
    }
}
