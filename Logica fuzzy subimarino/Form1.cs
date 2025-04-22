using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;

namespace Logica_fuzzy_subimarino
{
    public partial class Form1 : Form
    {
        double RateFission;
        double Tubine;
        double FuelPotential = 80;
        double HeatSupply;
        double CalorTubine;
        double Temperatura;
        int Carga = 3115;
        int Powermax = 3500;
        double DR;
        double DT;
        int ConvReator;
        int ConvTurbina;
        bool ativo = false;
       

        SerialPort portaSerial = new SerialPort("COM11", 9600); 
        
        public Form1()
        {
            InitializeComponent();
            portaSerial.Open();
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            RateFission = hScrollBar1.Value;
            RateFission = RateFission / 100;
            textBox1.Text = "Taxa de fissão:" + RateFission;
        }

        private void hScrollBar2_Scroll(object sender, ScrollEventArgs e)
        {
            Tubine = hScrollBar2.Value;
            Tubine = Tubine / 100;
            textBox2.Text = "Tubina:" + Tubine;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            HeatSupply = (FuelPotential/100) * 2 * RateFission;
            CalorTubine = (Tubine);
            Temperatura = HeatSupply - CalorTubine;
            DR = (((Powermax * FuelPotential * RateFission) / 7500) - Carga);
            DT = (((Powermax * Tubine) / 100) - Carga);
            textBox3.Text = "Calor gerado:" + HeatSupply;
            textBox4.Text = "Calor Tubina:" + (CalorTubine);
            textBox6.Text = "Diferença de carga turbina:" + DT;
            textBox7.Text = "Diferença de carga reator:" + DR;
            textBox10.Text ="Temperatura: " + Temperatura;

            if (portaSerial.BytesToRead > 0){
                RateFission = hScrollBar1.Value;
                RateFission = RateFission / 100;
                textBox1.Text = "Taxa de fissão:" + RateFission;
                Tubine = hScrollBar2.Value;
                Tubine = Tubine / 100;
                textBox2.Text = "Tubina:" + Tubine;
                string dado = portaSerial.ReadLine();
                string[] partes = dado.Split(';');
                textBox8.Text = partes[0];
                textBox9.Text = partes[1];
                ConvReator = (int)(double.Parse(partes[0]));
                ConvTurbina = (int)(double.Parse(partes[1]));
                textBox5.Text = ConvReator.ToString();
                hScrollBar1.Value = Math.Max(hScrollBar1.Minimum, Math.Min(hScrollBar1.Maximum,(hScrollBar1.Value + ConvReator) )); 
                hScrollBar2.Value = Math.Max(hScrollBar2.Minimum, Math.Min(hScrollBar2.Maximum, (hScrollBar2.Value + ConvTurbina))); 
                string mensagem = DR + ";" + DT + ";" + Temperatura;
                portaSerial.WriteLine(mensagem);
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            ativo = !ativo; // Alterna entre ligado e desligado

            if (ativo)
            {
                button3.BackgroundImage = Properties.Resources.Botaoligado;
                timer1.Enabled = !timer1.Enabled;
                string mensagem = DR + ";" + DT + ";" + Temperatura;
                portaSerial.WriteLine(mensagem);
            }
            else
            {
                button3.BackgroundImage = Properties.Resources.Botaodesligado;
                timer1.Enabled = !timer1.Enabled;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            pictureBox4.Parent = pictureBox5;
            pictureBox6.Parent = pictureBox3;
            pictureBox4.BackColor = Color.Transparent;
            pictureBox6.BackColor = Color.Transparent;
            int reatorcursor = Map(hScrollBar1.Value);
            int turbinacursor = Map(hScrollBar2.Value);
            pictureBox4.Location = new Point(reatorcursor, 5); // relativo ao pictureBox1 agora
            pictureBox6.Location = new Point(turbinacursor, 5); // relativo ao pictureBox1 agora
        }
        public static int Map(int value)
        {
            return (value - 0) * (415 - 15) / (10000 - 0) + 15;
        }
    }
}
