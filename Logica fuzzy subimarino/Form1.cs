using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Logica_fuzzy_subimarino
{
    public partial class Form1 : Form
    {
        int RateFission;
        int Tubine;
        public Form1()
        {
            InitializeComponent();
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            RateFission = hScrollBar1.Value;
            textBox1.Text = "Taxa de fissão:"+RateFission.ToString();
        }

        private void hScrollBar2_Scroll(object sender, ScrollEventArgs e)
        {
            Tubine = hScrollBar2.Value;
            textBox2.Text = "Tubina:" + Tubine.ToString();
        }
    }
}
