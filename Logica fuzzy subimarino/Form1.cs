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
        double Estabilida;
        int Carga = 3115;
        int Powermax = 3500;
        double DR;
        double DT;
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
            Estabilida = (HeatSupply/2) - CalorTubine;
            Temperatura = HeatSupply - CalorTubine;
            DR = (((Powermax * FuelPotential * RateFission) / 7500) - Carga);
            DT = (((Powermax * Tubine) / 100) - Carga);
            textBox3.Text = "Calor gerado:" + HeatSupply;
            textBox4.Text = "Calor Tubina:" + (CalorTubine);
            textBox5.Text = "Estabilidade:" + Estabilida;
            textBox6.Text = "Diferença de carga turbina:" + DT;
            textBox7.Text = "Diferença de carga reator:" + DR;
            textBox10.Text ="Temperatura: " + Temperatura;

            if (portaSerial.BytesToRead > 0){
                string dado = portaSerial.ReadLine();
                string[] partes = dado.Split(';');
                textBox8.Text = partes[0];
                textBox9.Text = partes[1];
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string mensagem = DR + ";" + DT + ";" + Temperatura;
            portaSerial.WriteLine(mensagem);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }
    }
}
