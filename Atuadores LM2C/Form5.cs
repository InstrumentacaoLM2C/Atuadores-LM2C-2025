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
    public partial class Form5 : Form
    {

        int contadorAcionamento = 5;
        private ControleSerial controleSerial;


        public Form5(ControleSerial serial)
        {
            InitializeComponent();
            controleSerial = serial ?? throw new ArgumentNullException(nameof(serial));
            controleSerial.DadosRecebidos += ControleSerial_DadosRecebidos;
            
        }
        private void Form5_Load(object sender, EventArgs e)
        {
            richTextBox3.Enabled = false;
            button1.Enabled = false;
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
                        
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {


            }
#pragma warning restore CS0168 // A variável foi declarada, mas nunca foi usada
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form6 form6 = new Form6(controleSerial);
            form6.Show();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox3_TextChanged(object sender, EventArgs e)
        {
            double distanciaPercorrida;

            if (!string.IsNullOrWhiteSpace(richTextBox3.Text))
            {
                // Substitui pontos por vírgulas para o formato brasileiro
                string inputDistanciaTotal = richTextBox3.Text.Replace('.', ',');

                // Tenta converter a string para double
                if (double.TryParse(inputDistanciaTotal, NumberStyles.Any, new CultureInfo("pt-BR"), out double valorConvertido))
                {
                    distanciaPercorrida = valorConvertido; // Atualiza apenas se a conversão for bem-sucedida

                }
                else
                {
                    MessageBox.Show("Por favor, insira um valor numérico válido.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                distanciaPercorrida = 0; // Define um valor padrão quando o campo está vazio
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            contadorAcionamento--;
            if (contadorAcionamento != 1)
            {
                label3.Text = "Faltam " + contadorAcionamento.ToString() + " acionamentos.";
            } else
            {
                label3.Text = "Falta " + contadorAcionamento.ToString() + " acionamento.";
            }


            button2.Enabled = false; // Evita múltiplos cliques

            if (contadorAcionamento > 0)
            {
                try
                {
                    string comando = string.Format(
                        CultureInfo.InvariantCulture,
                        "W{0};{1};{2};{3};{4};H#",
                        2000,
                        5000,
                        2000,
                        5000,
                        0
                    );

                    await Task.Run(() =>
                    {
                        controleSerial.Enviar("m#");
                        controleSerial.Enviar(comando);

                        Console.WriteLine($"Comando Enviado: {comando}");

                        /*if (motor_ligado)
                        {
                            controleSerial.Enviar("n#");
                            paradaPorBotao = true; // <-- Indicando que foi manual
                            Console.WriteLine("Comando Enviado: n# (Parar motor)");
                        }*/
                    });

                    // Atualiza interface na thread principal
                    //AtualizarInterfaceMotor(motor_ligado);

                }
                catch (OperationCanceledException)
                {
                    // Task foi cancelada, pode ignorar
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao comunicar com a porta serial: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    button2.Enabled = true;
                    button1.Enabled = false;
                }
            } else
            {
                button2.Enabled = false;
                button1.Enabled = true;
            }

        }


    }
}
