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
        double FuelPotential = 100;
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
        private Bitmap originalImage;
        SerialPort portaSerial = new SerialPort("COM11", 9600);

        public Form1()
        {
            InitializeComponent();
            portaSerial.Open();
            originalImage = new Bitmap("D:\\facudade\\sexto_periodo\\Inteligencia_artificial\\Logica_fuzzy\\Logica fuzzy subimarino\\Logica fuzzy subimarino\\Imgs\\ponteiro.png");
            pictureBox7.BackgroundImage = originalImage;
            pictureBox8.BackgroundImage = originalImage;
            RotateImage(-90, EventArgs.Empty);
            RotateImage1(-90, EventArgs.Empty);
            label5.Parent = pictureBox10;
            label5.BackColor = Color.Transparent;
            label5.Location = new Point(15,30);
            label6.Parent = pictureBox11;
            label6.BackColor = Color.Transparent;
            label6.Location = new Point(15, 30);
            pictureBox4.Parent = pictureBox5;
            pictureBox6.Parent = pictureBox3;
            pictureBox7.Parent = pictureBox1;
            pictureBox8.Parent = pictureBox2;
            pictureBox4.BackColor = Color.Transparent;
            pictureBox6.BackColor = Color.Transparent;
            pictureBox7.BackColor = Color.Transparent;
            pictureBox8.BackColor = Color.Transparent;
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            RateFission = hScrollBar1.Value;
            RateFission = RateFission / 100;
        }

        private void hScrollBar2_Scroll(object sender, ScrollEventArgs e)
        {
            Tubine = hScrollBar2.Value;
            Tubine = Tubine / 100;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DR = (((Powermax * FuelPotential * RateFission) / 7500) - Carga);
            DT = (((Powermax * Tubine) / 100) - Carga);

            if (portaSerial.BytesToRead > 0){
                RateFission = hScrollBar1.Value;
                RateFission = RateFission / 100;
                Tubine = hScrollBar2.Value;
                Tubine = Tubine / 100;
                string dado = portaSerial.ReadLine();
                string[] partes = dado.Split(';');
                ConvReator = (int)(double.Parse(partes[0]));
                ConvTurbina = (int)(double.Parse(partes[1]));
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
            HeatSupply = (FuelPotential / 100) * 2 * RateFission;
            CalorTubine = (Tubine);
            Temperatura = HeatSupply - CalorTubine;
            label5.Text = RateFission.ToString("0.00") + " %";
            label6.Text = Tubine.ToString("0.00") + " %";
            Medidortemp(Temperatura,panel1, panel2, panel3, panel4, panel5, panel6, panel7, panel8, panel9, panel10, panel11, panel12, panel13, panel14);
            int reatorcursor = Map(hScrollBar2.Value);
            int turbinacursor = Map(hScrollBar1.Value);
            float anguloreator = Map1(hScrollBar1.Value);
            float anguloturbina = Map1(hScrollBar2.Value);
            RotateImage(anguloreator, EventArgs.Empty);
            RotateImage1(anguloturbina, EventArgs.Empty);
            pictureBox4.Location = new Point(reatorcursor, 5); // relativo ao pictureBox1 agora
            pictureBox6.Location = new Point(turbinacursor, 5); // relativo ao pictureBox1 agora
            pictureBox7.Location = new Point(160,55); // relativo ao pictureBox1 agora
            pictureBox8.Location = new Point(160, 55); // relativo ao pictureBox1 agora
        }

        public static int Map(int value)
        {
            return (value  * 400 / 10000) + 15;
        }
        public static float Map1(float value)
        {
            return (value * 169 / 10000 ) - 90;
        }
        private static int Medidortemp(double graus, Panel panel1, Panel panel2, Panel panel3, Panel panel4, Panel panel5, Panel panel6, Panel panel7, Panel panel8,
            Panel panel9, Panel panel10, Panel panel11, Panel panel12, Panel panel13, Panel panel14)
        {
            if (graus <= 0)
            {
                graus = 0;
            }
            else if (graus > 100)
            {
                graus = 100;
            }

            if (graus > 1)
            {
                panel1.Visible = true;
            }
            else 
            {
                panel1.Visible = false;
            }

            if (graus > 8)
            {
                panel2.Visible = true;
            }
            else 
            {
                panel2.Visible = false;
            }
            if (graus > 15)
            {
                panel3.Visible = true;
            }
            else
            {
                panel3.Visible = false;
            }
            if (graus > 22)
            {
                panel4.Visible = true;
            }
            else
            {
                panel4.Visible = false;
            }
            if (graus > 29)
            {
                panel5.Visible = true;
            }
            else
            {
                panel5.Visible = false;
            }
            if (graus > 36)
            {
                panel6.Visible = true;
            }
            else
            {
                panel6.Visible = false;
            }
            if (graus > 43)
            {
                panel7.Visible = true;
            }
            else
            {
                panel7.Visible = false;
            }
            if (graus > 50)
            {
                panel8.Visible = true;
            }
            else
            {
                panel8.Visible = false;
            }
            if (graus > 57)
            {
                panel9.Visible = true;
            }
            else
            {
                panel9.Visible = false;
            }
            if (graus > 64)
            {
                panel10.Visible = true;
            }
            else
            {
                panel10.Visible = false;
            }
            if (graus > 71)
            {
                panel11.Visible = true;
            }
            else
            {
                panel11.Visible = false;
            }
            if (graus > 78)
            {
                panel12.Visible = true;
            }
            else
            {
                panel12.Visible = false;
            }
            if (graus > 85)
            {
                panel13.Visible = true;
            }
            else
            {
                panel13.Visible = false;
            }
            if (graus > 92)
            {
                panel14.Visible = true;
            }
            else
            {
                panel14.Visible = false;
            }
            return 0;
        }

        private void RotateImage(float angle, EventArgs e)
        {
            if (angle >= 360) angle = 0;

            // Cria uma nova imagem girada
            Bitmap rotatedImage = new Bitmap(originalImage.Height+500, originalImage.Height+500);
            using (Graphics g = Graphics.FromImage(rotatedImage))
            {
                g.TranslateTransform(originalImage.Height/2, 145);
                g.RotateTransform(angle);
                g.TranslateTransform(-originalImage.Width/2, -145);
                g.DrawImage(originalImage, new Point(10,0));
            }
            pictureBox7.BackgroundImage = rotatedImage;
        }
        private void RotateImage1(float angle, EventArgs e)
        {
            if (angle >= 360) angle = 0;

            // Cria uma nova imagem girada
            Bitmap rotatedImage = new Bitmap(originalImage.Height + 500, originalImage.Height + 500);
            using (Graphics g = Graphics.FromImage(rotatedImage))
            {
                g.TranslateTransform(originalImage.Height / 2, 145);
                g.RotateTransform(angle);
                g.TranslateTransform(-originalImage.Width / 2, -145);
                g.DrawImage(originalImage, new Point(10, 0));
            }
            pictureBox8.BackgroundImage = rotatedImage;
        }
    }
}
