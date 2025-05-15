using System;
using System.Drawing;
using System.Globalization;
using System.IO.Ports;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atuadores_LM2C
{
    public partial class Form2 : Form
    {
        private ControleSerial controleSerial;

        // Declaração de variáveis
        //------------------INICIO-----------------
        string direcao1 = "0";  // direção
        string direcao2 = "0";  // direção
        double distancia_pulsos1;  //Variavel que armazena a quantidade de pulsos que será dado pelo motor vertical
        double distancia_pulsos2;
        double velocidade_pulsos1;
        double velocidade_pulsos2;
        double velocidade_mm1, velocidade_mm2, distancia_mm1, distancia_mm2;
        double constanteCalibracao1 = 1;  //A constante de calibração default dos motores que representa a velocidade de aceleração de 2500pulsos/s
        double constanteCalibracao2 = 1;
        bool on_energizar_vertical = true;
        bool on_energizar_horizontal = true;
        bool on_sensor_vertical = false;
        bool on_sensor_horizontal = false;
        bool motorVertical = true;
        bool ligarMotor_vertical = false;
        bool ligarMotor_horizontal = false;
        int motor = 1; // Armazena qual motor está sendo utilizado
        //-----------------------FIM----------------

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
        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void RecalcularDistanciaEVelocidade()
        {
            // Recalcula os pulsos de distância e velocidade com a nova constante de calibração
            if (constanteCalibracao1 != 0)
            {
                distancia_pulsos1 = (float)Math.Round(distancia_mm1 / constanteCalibracao1);
                distancia_pulsos2 = (float)Math.Round(distancia_mm2 / constanteCalibracao1);
                velocidade_pulsos1 = (float)Math.Round(velocidade_mm1 / constanteCalibracao1);
                velocidade_pulsos2 = (float)Math.Round(velocidade_mm2 / constanteCalibracao1);
            }
        }

        private void richTextBox6_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(richTextBox6.Text))
            {
                // Substitui pontos por vírgulas para o formato brasileiro
                string inputDistancia2 = richTextBox6.Text.Replace('.', ',');

                // Tenta converter a string para float
                if (float.TryParse(inputDistancia2, NumberStyles.Any, new CultureInfo("pt-BR"), out float distancia_mm2))
                {
                    // Calcula os pulsos com base no valor convertido
                    if (constanteCalibracao2 != 0)
                        distancia_pulsos2 = (float)Math.Round(distancia_mm2 / constanteCalibracao2);

                }
                else
                {
                    MessageBox.Show("Por favor, insira um valor numérico válido.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                // Define valores padrão caso o campo fique vazio
                distancia_pulsos2 = 0;

            }
        }

        private void richTextBox5_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(richTextBox5.Text))
            {
                // Substitui pontos por vírgulas para o formato brasileiro
                string inputVelocidade2 = richTextBox5.Text.Replace('.', ',');

                // Tenta converter a string para float
                if (float.TryParse(inputVelocidade2, NumberStyles.Any, new CultureInfo("pt-BR"), out float velocidade_mm2))
                {
                    // Calcula os pulsos com base no valor convertido
                    if (constanteCalibracao2 != 0)
                        velocidade_pulsos2 = (float)Math.Round(velocidade_mm2 / constanteCalibracao2);
                }
                else
                {
                    MessageBox.Show("Por favor, insira um valor numérico válido.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                // Define valores padrão caso o campo fique vazio
                velocidade_pulsos2 = 0;
            }
        }

        private void richTextBox4_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(richTextBox6.Text))
            {
                // Substitui pontos por vírgulas para o formato brasileiro
                string inputDistancia2 = richTextBox6.Text.Replace('.', ',');

                // Tenta converter a string para float
                if (float.TryParse(inputDistancia2, NumberStyles.Any, new CultureInfo("pt-BR"), out float distancia_mm2))
                {
                    // Calcula os pulsos com base no valor convertido
                    if (constanteCalibracao2 != 0)
                        distancia_pulsos2 = (float)Math.Round(distancia_mm2 / constanteCalibracao2);

                }
                else
                {
                    MessageBox.Show("Por favor, insira um valor numérico válido.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                // Define valores padrão caso o campo fique vazio
                distancia_pulsos2 = 0;

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (on_energizar_vertical)
                {
                    controleSerial.Enviar("M#"); // Seleciona motor vertical
                    controleSerial.Enviar("A#"); // Liga ENABLE do Driver

                    button1.Text = "Acoplado";
                    button1.BackColor = Color.Green;
                    on_energizar_vertical = false;
                }
                else
                {
                    if (ligarMotor_vertical)
                    {
                        MessageBox.Show("Desligue o motor vertical antes de desenergizá-lo.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    controleSerial.Enviar("a#"); // Desliga ENABLE do Driver

                    button1.Text = "Acoplar";
                    button1.BackColor = SystemColors.Control;
                    on_energizar_vertical = true;
                }
            }
            catch
            {
                MessageBox.Show("Erro ao acoplar motor, tente novamente.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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

        private async void button2_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                direcao1 = "B";
            }
            else if (radioButton2.Checked)
            {
                direcao1 = "C";
            }
            else
            {
                MessageBox.Show("Por favor, selecione uma direção.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Verifica se os valores são válidos
            if (!VerificarTextoValido(richTextBox1) || !VerificarTextoValido(richTextBox2))
            {
                MessageBox.Show("Por favor, insira valores válidos para distância e velocidade.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            button1.Enabled = false; // Evita múltiplos cliques

            try
            {
                await Task.Run(async () =>
                {
                    // Envia comandos iniciais para preparar o motor
                    controleSerial.Enviar("M#");

                    string comando = $"T{distancia_pulsos1.ToString(CultureInfo.InvariantCulture)};" +
                                     $"{velocidade_pulsos1.ToString(CultureInfo.InvariantCulture)};" +
                                     $"{direcao1};H#";

                    controleSerial.Enviar(comando);

                    Console.WriteLine($"Comando Enviado: {comando}");

                    // Se já estava ligado, envia comando para parar
                    if (ligarMotor_vertical)
                    {
                        controleSerial.Enviar("n#");
                        Console.WriteLine("Comando Enviado: n# (Parar motor)");
                    }
                });

                // Atualiza a interface (thread principal)
                this.Invoke((Action)(() =>
                {
                    if (ligarMotor_vertical)
                    {
                        button1.Text = "Ligar";
                        button1.BackColor = Color.Gainsboro;
                        ligarMotor_vertical = false;
                        on_energizar_vertical = false;
                        MessageBox.Show("O motor vertical parou.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        button1.Text = "Ligado";
                        button1.BackColor = Color.Green;
                        ligarMotor_vertical = true;
                        on_energizar_vertical = true;
                    }
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao executar comando: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                button1.Enabled = true;
            }
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            if (!ligarMotor_vertical)
            {
                MessageBox.Show("O motor já está parado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                button3.Enabled = false; // Desativa o botão enquanto executa o comando

                await Task.Run(() =>
                {
                    // Enviar comando para parar o motor
                    controleSerial.Enviar("n#");
                    Console.WriteLine("Comando Enviado: n#");
                });

                // Atualiza a UI na thread principal
                this.Invoke((Action)(() =>
                {
                    ligarMotor_vertical = false;
                    button3.Text = "Ligar";
                    button3.BackColor = Color.Gainsboro;

                    MessageBox.Show("O motor foi parado com sucesso!", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }));
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Acesso negado à porta serial. " +
                                "Verifique se a porta já está em uso ou se você tem permissão para acessá-la.",
                                "Erro de Acesso",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("A operação não pôde ser completada. " +
                                "Verifique se a porta serial está aberta e configurada corretamente.",
                                "Erro de Operação",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            catch (IOException ex)
            {
                MessageBox.Show("Falha de comunicação ao tentar parar o motor. " +
                                "Certifique-se de que o dispositivo está conectado corretamente.\n\n" +
                                $"Detalhes do erro: {ex.Message}",
                                "Erro de Comunicação",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível parar o motor. " +
                                "Uma falha inesperada ocorreu. Tente novamente.\n\n" +
                                $"Detalhes do erro: {ex.Message}",
                                "Erro Desconhecido",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            finally
            {
                button3.Enabled = true; // Reativa o botão após a execução
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                if (on_energizar_horizontal)
                {
                    controleSerial.Enviar("R#"); // Seleciona motor vertical
                    controleSerial.Enviar("A#"); // Liga ENABLE do Driver

                    button6.Text = "Acoplado";
                    button6.BackColor = Color.Green;
                    on_energizar_horizontal = false;
                }
                else
                {
                    if (ligarMotor_horizontal)
                    {
                        MessageBox.Show("Desligue o motor vertical antes de desenergizá-lo.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    controleSerial.Enviar("a#"); // Desliga ENABLE do Driver

                    button6.Text = "Acoplar";
                    button6.BackColor = SystemColors.Control;
                    on_energizar_horizontal = true;
                }
            }
            catch
            {
                MessageBox.Show("Erro ao acoplar motor, tente novamente.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
            {
                direcao2 = "B";
            }
            else if (radioButton4.Checked)
            {
                direcao2 = "C";
            }
            else
            {
                MessageBox.Show("Por favor, selecione uma direção.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Verifica se os valores são válidos
            if (!VerificarTextoValido(richTextBox5) || !VerificarTextoValido(richTextBox6))
            {
                MessageBox.Show("Por favor, insira valores válidos para distância e velocidade.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            button5.Enabled = false; // Evita múltiplos cliques

            try
            {
                await Task.Run(async () =>
                {
                    // Envia comandos iniciais para preparar o motor
                    controleSerial.Enviar("R#");

                    string comando = $"T{distancia_pulsos2.ToString(CultureInfo.InvariantCulture)};" +
                                     $"{velocidade_pulsos2.ToString(CultureInfo.InvariantCulture)};" +
                                     $"{direcao2};H#";

                    controleSerial.Enviar(comando);

                    Console.WriteLine($"Comando Enviado: {comando}");

                    // Se já estava ligado, envia comando para parar
                    if (ligarMotor_horizontal)
                    {
                        controleSerial.Enviar("n#");
                        Console.WriteLine("Comando Enviado: n# (Parar motor)");
                    }
                });

                // Atualiza a interface (thread principal)
                this.Invoke((Action)(() =>
                {
                    if (ligarMotor_horizontal)
                    {
                        button5.Text = "Ligar";
                        button5.BackColor = SystemColors.Control;
                        ligarMotor_horizontal = false;
                        on_energizar_horizontal = false;
                        MessageBox.Show("O motor horizontal parou.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        button5.Text = "Ligado";
                        button5.BackColor = Color.Green;
                        ligarMotor_horizontal = true;
                        on_energizar_horizontal = true;
                    }
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao executar comando: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
               button5.Enabled = true;
            }
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            if (!ligarMotor_horizontal)
            {
                MessageBox.Show("O motor já está parado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                button4.Enabled = false; // Desativa o botão enquanto executa o comando

                await Task.Run(() =>
                {
                    // Enviar comando para parar o motor
                    controleSerial.Enviar("n#");
                    Console.WriteLine("Comando Enviado: n#");
                });

                // Atualiza a UI na thread principal
                this.Invoke((Action)(() =>
                {
                    ligarMotor_horizontal = false;
                    button4.Text = "Ligar";
                    button4.BackColor = SystemColors.Control;

                    MessageBox.Show("O motor foi parado com sucesso!", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }));
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Acesso negado à porta serial. " +
                                "Verifique se a porta já está em uso ou se você tem permissão para acessá-la.",
                                "Erro de Acesso",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("A operação não pôde ser completada. " +
                                "Verifique se a porta serial está aberta e configurada corretamente.",
                                "Erro de Operação",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            catch (IOException ex)
            {
                MessageBox.Show("Falha de comunicação ao tentar parar o motor. " +
                                "Certifique-se de que o dispositivo está conectado corretamente.\n\n" +
                                $"Detalhes do erro: {ex.Message}",
                                "Erro de Comunicação",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível parar o motor. " +
                                "Uma falha inesperada ocorreu. Tente novamente.\n\n" +
                                $"Detalhes do erro: {ex.Message}",
                                "Erro Desconhecido",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            finally
            {
                button4.Enabled = true; // Reativa o botão após a execução
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
    }
}
