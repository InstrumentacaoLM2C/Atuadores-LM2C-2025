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
    public partial class Form7 : Form
    {
        private ControleSerial controleSerial;
        public Form7(ControleSerial serial)
        {
            InitializeComponent();
            controleSerial = serial ?? throw new ArgumentNullException(nameof(serial));
            controleSerial.DadosRecebidos += ControleSerial_DadosRecebidos;
        }

        private void ControleSerial_DadosRecebidos(object sender, string dados)
        {
            if (string.IsNullOrEmpty(dados))
                return;

            if (this.InvokeRequired)
            {
                if (!this.IsHandleCreated || this.IsDisposed)
                    return;

                try
                {
                    this.Invoke((MethodInvoker)(() => ControleSerial_DadosRecebidos(sender, dados)));
                }
                catch (InvalidOperationException)
                {
                    return;
                }
                return;
            }

            try
            {
                char comando = dados[0];

                switch (comando)
                {
                    
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
            }
#pragma warning restore CS0168 // A variável foi declarada, mas nunca foi usada

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form7_Load(object sender, EventArgs e)
        {

        }
    }
}
