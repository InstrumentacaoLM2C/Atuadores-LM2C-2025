using System;
using System.Windows.Forms;

namespace Atuadores_LM2C
{
    public partial class Form2 : Form
    {
        private ControleSerial controleSerial;

        public Form2(ControleSerial serial)
        {
            InitializeComponent();
            controleSerial = serial; // Corrigido!
            controleSerial.DadosRecebidos += ControleSerial_DadosRecebidos;
        }

        private void ControleSerial_DadosRecebidos(object sender, string dados)
        {
            // Garantir que só pegamos o primeiro caractere
            if (string.IsNullOrEmpty(dados))
                return;

            char comando = dados[0];

            Invoke((MethodInvoker)(() =>
            {
                switch (comando)
                {
                    case 'y':
                        break;
                    case 'Y':
                        break;
                    default:
                        break;
                }
            }));
        }

        private void Form2_Load(object sender, EventArgs e)
        {
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        
    }
}
