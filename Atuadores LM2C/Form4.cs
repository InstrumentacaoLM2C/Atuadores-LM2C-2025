using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Atuadores_LM2C
{
    public partial class FormCalibrar : Form
    {

        private ControleSerial controleSerial;
        public FormCalibrar(ControleSerial serial)
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

            if (!this.IsHandleCreated || this.IsDisposed)
                return;

            try
            {
                char comando = dados[0];

                switch (comando)
                {
                    case 'y':
                        /*if (motor_ligado)
                        {
                            AtualizarInterfaceMotor(motor_ligado);

                            if (paradaPorBotao)
                            {
                                paradaPorBotao = false;
                            }
                            else
                            {

                            }
                        }*/
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {


            }
        }



        private void btnAjuda_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private async void button2_Click(object sender, EventArgs e)
        {
            // Corrige os campos se estiverem inválidos
            if (!VerificarTextoValido(richTextBox1))
            {
                richTextBox1.Text = "90";
            }

            if (!VerificarTextoValido(richTextBox3))
            {
                richTextBox3.Text = "2000";
            }

            button2.Enabled = false;

            // Verifica novamente se os campos agora estão válidos
            if (VerificarTextoValido(richTextBox1) && VerificarTextoValido(richTextBox3))
            {
                Form5 form5 = new Form5(controleSerial);
                await Task.Delay(1000);
                form5.Show();
            }
            else
            {
                MessageBox.Show("Por favor, insira apenas números inteiros válidos nos campos.", "Erro de Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            double distanciaTotal; 

            if (!string.IsNullOrWhiteSpace(richTextBox1.Text))
            {
                // Substitui pontos por vírgulas para o formato brasileiro
                string inputDistanciaTotal = richTextBox1.Text.Replace('.', ',');

                // Tenta converter a string para double
                if (double.TryParse(inputDistanciaTotal, NumberStyles.Any, new CultureInfo("pt-BR"), out double valorConvertido))
                {
                    distanciaTotal = valorConvertido; // Atualiza apenas se a conversão for bem-sucedida
                   
                }
                else
                {
                    MessageBox.Show("Por favor, insira um valor numérico válido.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
               distanciaTotal = 90; // Define um valor padrão quando o campo está vazio
            }
        }

        private void richTextBox3_TextChanged(object sender, EventArgs e)
        {
            double quantidadePassos;

            if (!string.IsNullOrWhiteSpace(richTextBox1.Text))
            {
                // Substitui pontos por vírgulas para o formato brasileiro
                string inputPassos = richTextBox1.Text.Replace('.', ',');

                // Tenta converter a string para double
                if (double.TryParse(inputPassos, NumberStyles.Any, new CultureInfo("pt-BR"), out double valorConvertido))
                {
                    quantidadePassos = valorConvertido; // Atualiza apenas se a conversão for bem-sucedida

                }
                else
                {
                    MessageBox.Show("Por favor, insira um valor numérico válido.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                quantidadePassos = 2000; // Define um valor padrão quando o campo está vazio

            }
        }

        private bool VerificarTextoValido(RichTextBox richTextBox)
        {
            // Verifica se a RichTextBox está vazia ou contém apenas espaços em branco
            if (string.IsNullOrWhiteSpace(richTextBox.Text))
            {
                return false;
            }

            // Adicionalmente, você pode verificar se o texto é um número
            // Exemplo: Verifica se o texto pode ser convertido para um número
            if (!double.TryParse(richTextBox3.Text, out _))
            {
                return false; // Não é um número válido
            }

            return true; // Texto é válido
        }

        private void FormCalibrar_Load(object sender, EventArgs e)
        {

        }
    }
}
