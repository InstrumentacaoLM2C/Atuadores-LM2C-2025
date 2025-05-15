using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO.Ports;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atuadores_LM2C
{
    public partial class Form3 : Form
    {
        private ControleSerial controleSerial;

        //Declaração de Variáveis

        //------------- INICIO -------------

        string direcao1 = "0";  // direção
        string direcao2 = "0";  // direção
        float distancia_mm1 = 0.0f;
        float distancia_mm2 = 0.0f;
        float velocidade_mm1 = 0.0f;
        float velocidade_mm2 = 0.0f;
        float distancia_pulsos1 = 0.0f;  // Pulsos do motor vertical
        float distancia_pulsos2 = 0.0f;
        float velocidade_pulsos1 = 0.0f;
        float velocidade_pulsos2 = 0.0f;
        double constanteCalibracao1 = 1;  //A constante de calibração default dos motores que representa a velocidade de aceleração de 2500pulsos/s
        bool on_energizar_vertical = true;
        bool on_sensor_vertical = false;
        bool on_sensor_horizontal = false;
        bool motorVertical = true;
        bool ligarMotor_vertical = false;
        int motor = 1; // Armazena qual motor está sendo utilizado
        
        //------------- FIM ----------------


        public Form3(ControleSerial serial)
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
                    default:
                        break;
                }
            }));
        }
        private void Form3_Load(object sender, EventArgs e)
        {

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

                    distancia_pulsos2 = (float)Math.Round(distancia_mm1 / constanteCalibracao1);
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
            if (ligarMotor_vertical)
            {
                MessageBox.Show("Pare os motores antes de desenergizá-los.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                if (on_energizar_vertical)
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
                on_energizar_vertical = !on_energizar_vertical;
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

            if (!VerificarTextoValido(richTextBox1) || !VerificarTextoValido(richTextBox2))
            {
                MessageBox.Show("Por favor, selecione valores válidos para distância e velocidade.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                button2.Enabled = false;

                await Task.Run(() =>
                {
                    if (ligarMotor_vertical) // Se já estiver ligado, apenas para o motor
                    {
                        controleSerial.Enviar("n#");
                        Console.WriteLine("Comando Enviado: n# (Parar motor)");
                    }
                    else // Se estiver desligado, liga o motor
                    {
                        string comando = $"W{distancia_pulsos1.ToString(CultureInfo.InvariantCulture)};" +
                                        $"{velocidade_pulsos1.ToString(CultureInfo.InvariantCulture)};" +
                                        $"{distancia_pulsos2.ToString(CultureInfo.InvariantCulture)};" +
                                        $"{velocidade_pulsos2.ToString(CultureInfo.InvariantCulture)};" +
                                        $"{direcao1};H#";

                        controleSerial.Enviar(comando);
                        Console.WriteLine($"Comando Enviado: {comando}");
                    }
                });                // Atualiza a UI na thread principal
                this.Invoke((Action)(() =>
                {
                    if (ligarMotor_vertical) // Se estava ligado, agora será desligado
                    {
                        button2.Text = "Ligar";
                        button2.BackColor = SystemColors.Control;
                        ligarMotor_vertical = false;
                        on_energizar_vertical = false;
                        MessageBox.Show("O motor vertical parou! Botão Ligado", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else // Se estava desligado, agora será ligado
                    {
                        button2.Text = "Ligado";
                        button2.BackColor = Color.Green;
                        ligarMotor_vertical = true;
                        on_energizar_vertical = false;
                    }
                }));
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
                    button2.Text = "Ligar";
                    button2.BackColor = SystemColors.Control;

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
    }
}
