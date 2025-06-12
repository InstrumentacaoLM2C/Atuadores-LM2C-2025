using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atuadores_LM2C
{
    public partial class Form4 : Form
    {
        int contadorMotor = 5;
        bool motorMovendo, calibracaoConcluida, motorParou;

        public Form4()
        {
            InitializeComponent();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            panel1.Visible = false;
            panel2.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (contadorMotor == 0)
            {
                label4.Text = "A calibração foi concluida. Prossiga para a próxima página.";
                motorMovendo = false;
                calibracaoConcluida = true;
            } else
            {
                label4.Text = "O motor se moverá mais " + contadorMotor + " vezes.";
                contadorMotor--;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
           

        }
    }
}
