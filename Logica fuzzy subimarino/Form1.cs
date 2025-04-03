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
        int FuelPotential = 160;
        int HeatSupply;
        int CalorTubine;
        int Estabilida;
        int Carga = 0;
        int Powermax = 3500;
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            HeatSupply = (FuelPotential * 2 * RateFission);
            CalorTubine = (Tubine * 75);
            Estabilida = (HeatSupply/2) - CalorTubine;
            textBox3.Text = "Calor gerado:" + (HeatSupply);
            textBox4.Text = "Calor Tubina:" + (CalorTubine);
            textBox5.Text = "Estabilidade:" + (Estabilida);
            textBox6.Text = "Diferença de carga turbina:" + (((Powermax*Tubine)/100)-Carga);
            textBox7.Text = "Diferença de carga reator:" + (((Powermax * FuelPotential * RateFission) / 7500) - Carga);
        }
    }
}
