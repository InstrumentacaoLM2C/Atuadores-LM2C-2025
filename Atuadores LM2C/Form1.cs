using System;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Windows.Forms;

namespace Atuadores_LM2C
{
    public partial class Form1 : Form
    {
        private ControleSerial controleSerial;
        private bool serialConectada = false;

        public Form1()
        {
            InitializeComponent();
            obterPortas();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            controleSerial = new ControleSerial();
            
        }

        void obterPortas()
        {
            string[] portas = SerialPort.GetPortNames();
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(portas);
        }



        private void button1_Click(object sender, EventArgs e)
        {
            if (controleSerial == null || !controleSerial.EstaConectado)
            {
                MessageBox.Show("Por favor, conecte-se à porta serial antes de abrir",
                                "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            { 
                using (Form2 form2 = new Form2(controleSerial))
                {
                    form2.ShowDialog(); // Exibe o Form3 de forma modal
                }
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Acesso negado à porta serial. Verifique se o dispositivo está conectado corretamente ou se a porta já está em uso.",
                                "Erro de Acesso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("A operação não pôde ser completada. Verifique se a porta serial está configurada corretamente e tente novamente.",
                                "Erro de Operação", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (IOException)
            {
                MessageBox.Show("Falha de comunicação com a porta serial. Certifique-se de que o dispositivo está conectado corretamente.",
                                "Erro de Comunicação", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro inesperado ao enviar comando: {ex.Message}",
                                "Erro Desconhecido", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (controleSerial == null || !controleSerial.EstaConectado)
            {
                MessageBox.Show("Por favor, conecte-se à porta serial antes de abrir",
                                "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            { 
                using (Form3 form3 = new Form3(controleSerial))
                {
                    form3.ShowDialog(); // Exibe o Form3 de forma modal
                }
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Acesso negado à porta serial. Verifique se o dispositivo está conectado corretamente ou se a porta já está em uso.",
                                "Erro de Acesso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("A operação não pôde ser completada. Verifique se a porta serial está configurada corretamente e tente novamente.",
                                "Erro de Operação", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (IOException)
            {
                MessageBox.Show("Falha de comunicação com a porta serial. Certifique-se de que o dispositivo está conectado corretamente.",
                                "Erro de Comunicação", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro inesperado ao enviar comando: {ex.Message}",
                                "Erro Desconhecido", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            obterPortas();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            button4.Enabled = false; // Evita cliques duplos

            if (!serialConectada)
            {
                if (comboBox1.SelectedIndex == -1)
                {
                    MessageBox.Show("Selecione uma porta serial.");
                    button4.Enabled = true;
                    return;
                }

                try
                {
                    string porta = comboBox1.SelectedItem.ToString();
                    controleSerial.Conectar(porta);
                    serialConectada = true;
                    button4.Text = "Desconectar";
                    button4.BackColor = Color.Red;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao conectar: " + ex.Message);
                }
            }
            else
            {
                try
                {
                    controleSerial.Desconectar();
                    serialConectada = false;
                    button4.Text = "Conectar";
                    button4.BackColor = SystemColors.Control;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao desconectar: " + ex.Message);
                }
            }

            button4.Enabled = true;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnAjuda_Click(object sender, EventArgs e)
        {
            Form1_Ajuda form1_ajuda = new Form1_Ajuda();
            form1_ajuda.ShowDialog();
        }

        private void btnAjuda_Click_1(object sender, EventArgs e)
        {
            Form1_Ajuda form1_ajuda = new Form1_Ajuda();
            form1_ajuda.ShowDialog();
        }
    }
}
