using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Atuadores_LM2C
{
    public partial class Form3 : Form
    {
        //Declaração de Variáveis

        //------------- INICIO -------------

        //variaveis de controle e declaração de objetos

        private ControleSerial controleSerial;
        private bool paradaPorBotao = false;



        string direcao = "";  // direção
        float distancia_mm1 = 0.0f;
        float velocidade_mm1 = 0.0f;
        float distancia_pulsos1 = 0.0f;  // Pulsos do motor vertical
        float distancia_pulsos2 = 0.0f;
        float velocidade_pulsos1 = 0.0f;
        float velocidade_pulsos2 = 0.0f;
        double constanteCalibracao1 = 1;  //A constante de calibração default dos motores que representa a velocidade de aceleração de 2500pulsos/s
        bool motor_energizado = true;
        bool motor_ligado = false;

        //------------- FIM ----------------

        // CancellationTokenSource para controlar cancelamento das tasks/threads do form
        private CancellationTokenSource cancelamentoForm = new CancellationTokenSource();


        public Form3(ControleSerial serial)
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
                richTextBox4.AppendText(dados);
              
                char comando = dados[0];

                switch (comando)
                {
                    case 'y':
                        if (motor_ligado)
                        {
                            AtualizarInterfaceMotor(motor_ligado);

                            if (paradaPorBotao)
                            {
                                paradaPorBotao = false;
                            }
                            else
                            {

                            }
                        }
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                
                    
            }
        }




        private void Form3_Load(object sender, EventArgs e)
        {
            this.FormClosing += Form3_FormClosing;
        }

        private void CancelarTarefasForm()
        {
            if (cancelamentoForm != null && !cancelamentoForm.IsCancellationRequested)
                cancelamentoForm.Cancel();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void RecalcularDistanciaEVelocidade()
        {
            // Recalcula os pulsos de distância e velocidade com a nova constante de calibração
            if (constanteCalibracao1 != 0)
            {
                distancia_pulsos1 = (float)Math.Round(distancia_mm1 / constanteCalibracao1);
                distancia_pulsos2 = (float)Math.Round(distancia_mm1 / constanteCalibracao1);
                velocidade_pulsos1 = (float)Math.Round(velocidade_mm1 / constanteCalibracao1);
                velocidade_pulsos2 = (float)Math.Round(velocidade_mm1 / constanteCalibracao1);
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(richTextBox1.Text))
            {
                // Substitui pontos por vírgulas para o formato brasileiro
                string inputConstanteCalibracao1 = richTextBox1.Text.Replace('.', ',');

                // Tenta converter a string para double
                if (double.TryParse(inputConstanteCalibracao1, NumberStyles.Any, new CultureInfo("pt-BR"), out double valorConvertido))
                {
                    constanteCalibracao1 = valorConvertido; // Atualiza apenas se a conversão for bem-sucedida

                    // Agora recalcula as variáveis de distância e velocidade com a nova constante de calibração
                    RecalcularDistanciaEVelocidade();
                }
                else
                {
                    MessageBox.Show("Por favor, insira um valor numérico válido.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                constanteCalibracao1 = 1; // Define um valor padrão quando o campo está vazio

                // Recalcula as variáveis de distância e velocidade com o valor padrão
                RecalcularDistanciaEVelocidade();
            }
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(richTextBox2.Text))
            {
                // Substitui pontos por vírgulas para o formato brasileiro
                string inputVelocidade1 = richTextBox2.Text.Replace('.', ',');


                // Tenta converter a string para float
                if (float.TryParse(inputVelocidade1, NumberStyles.Any, new CultureInfo("pt-BR"), out float velocidade_mm1))
                {
                    // Calcula os pulsos com base no valor convertido
                    if (constanteCalibracao1 != 0)
                        velocidade_pulsos1 = (float)Math.Round(velocidade_mm1 / constanteCalibracao1);

                        velocidade_pulsos2 = (float)Math.Round(velocidade_mm1 / constanteCalibracao1);
                }
                else
                {
                    MessageBox.Show("Por favor, insira um valor numérico válido.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                // Define valores padrão caso o campo fique vazio
                velocidade_pulsos1 = 0;
                velocidade_pulsos2 = 0;
            }
        }

        private void richTextBox3_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(richTextBox3.Text))
            {
                // Substitui pontos por vírgulas para o formato brasileiro
                string inputDistancia1 = richTextBox3.Text.Replace('.', ',');

                // Tenta converter a string para float
                if (float.TryParse(inputDistancia1, NumberStyles.Any, new CultureInfo("pt-BR"), out float distancia_mm1))
                {
                    // Calcula os pulsos com base no valor convertido
                    if (constanteCalibracao1 != 0)
                        distancia_pulsos1 = (float)Math.Round(distancia_mm1 / constanteCalibracao1);

                        distancia_pulsos2 = distancia_pulsos1;
                }
                else
                {
                    MessageBox.Show("Por favor, insira um valor numérico válido.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                // Define valores padrão caso o campo fique vazio
                distancia_pulsos1 = 0;
                distancia_pulsos2 = 0;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private bool VerificarTextoValido(RichTextBox richTextBox)
        {
            // Verifica se a RichTextBox está vazia ou contém apenas espaços em branco
            if (string.IsNullOrWhiteSpace(richTextBox.Text))
            {
                return false; // Não é válido
            }

            // Adicionalmente, você pode verificar se o texto é um número
            // Exemplo: Verifica se o texto pode ser convertido para um número
            if (!double.TryParse(richTextBox.Text, out _))
            {
                return false; // Não é um número válido
            }

            return true; // Texto é válido
        }

        private void AtualizarInterfaceMotor(bool ligado)
        {
            if (!ligado)
            {
                button2.Text = "Parar";
                button2.BackColor = Color.Red;
                button2.Enabled = true;
                motor_ligado = true;
                button1.Text = "Acoplado";
                button1.BackColor = Color.Green;
                button1.Enabled = false;
                motor_energizado = true;
            }
            else
            {
                button2.Text = "Ligar";
                button2.BackColor = SystemColors.Control;
                button2.Enabled = true;
                motor_ligado = false;
                button1.Text = "Acoplar";
                button1.BackColor = SystemColors.Control;
                button1.Enabled = true;
                motor_energizado = false;
                MessageBox.Show("O motor vertical parou!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked && !radioButton2.Checked)
            {
                direcao = "B";
            }
            else if (!radioButton1.Checked && radioButton2.Checked)
            {
                direcao = "C";
            }
            else
            {
                MessageBox.Show("Por favor, selecione uma direção.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!VerificarTextoValido(richTextBox1) || !VerificarTextoValido(richTextBox2))
            {
                MessageBox.Show("Por favor, selecione valores válidos para distância e velocidade.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            button2.Enabled = false; // Evita múltiplos cliques
            

            try
            {
                string comando = string.Format(
                    CultureInfo.InvariantCulture,
                    "W{0};{1};{2};{3};{4};H#",
                    distancia_pulsos1,
                    velocidade_pulsos1,
                    distancia_pulsos2,
                    velocidade_pulsos2,
                    direcao
                );

                await Task.Run(() =>
                {
                    controleSerial.Enviar("m#");
                    controleSerial.Enviar(comando);

                    Console.WriteLine($"Comando Enviado: {comando}");

                    if (motor_ligado)
                    {
                        controleSerial.Enviar("n#");
                        paradaPorBotao = true; // <-- Indicando que foi manual
                        Console.WriteLine("Comando Enviado: n# (Parar motor)");
                    }
                });

                // Atualiza interface na thread principal
                AtualizarInterfaceMotor(motor_ligado);

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
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (motor_ligado)
            {
                MessageBox.Show("Pare os motores antes de desenergizá-los.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                if (motor_energizado)
                {
                    // Enviar comando para energizar o motor
                    controleSerial.Enviar("#A");
                    button1.Text = "Acoplado";
                    button1.BackColor = Color.Green;
                }
                else
                {
                    // Enviar comando para desenergizar o motor
                    controleSerial.Enviar("#a");
                    button1.Text = "Acoplar";
                    button1.BackColor = SystemColors.Control;
                }

                // Inverter o estado da variável
                motor_energizado = !motor_energizado;
            }
            catch (OperationCanceledException)
            {
                // Task cancelada, sem ação necessária
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
                MessageBox.Show($"Erro inesperado: {ex.Message}",
                                "Erro Desconhecido", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void richTextBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                controleSerial.DadosRecebidos -= ControleSerial_DadosRecebidos; // remover handler

                if (motor_ligado)
                {
                    controleSerial.Enviar("n#");  // Parar motor
                    motor_ligado = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao tentar parar o motor antes de fechar o formulário: " + ex.Message);
            }

            CancelarTarefasForm();
        }

        private void btnAjuda_Click(object sender, EventArgs e)
        {
            Form3_Ajuda form3_ajuda = new Form3_Ajuda();
            form3_ajuda.ShowDialog();
        }
    }
}
