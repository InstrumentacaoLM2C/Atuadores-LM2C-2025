using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atuadores_LM2C
{
    public partial class Form7 : Form
    {

        //Declaração de Variáveis

        //------------- INICIO -------------

        //variaveis de controle e declaração de objetos

        private ControleSerial controleSerial;
        private bool paradaPorBotao = false;



        string direcao = "";  // direção
        string direcao2 = "";
        float distancia_mm1 = 0.0f;
        float velocidade_mm1 = 0.0f;
        float distancia_pulsos1 = 0.0f;  // Pulsos do motor vertical
        float distancia_pulsos2 = 0.0f;
        float velocidade_pulsos1 = 0.0f;
        float velocidade_pulsos2 = 0.0f;
        float distancia_pulsos3 = 0.0f;  // Pulsos do motor vertical
        float distancia_pulsos4 = 0.0f;
        float velocidade_pulsos3 = 0.0f;
        float velocidade_pulsos4 = 0.0f;
        double constanteCalibracao1 = 1;  //A constante de calibração default dos motores que representa a velocidade de aceleração de 2500pulsos/s
        double constanteCalibracao2 = 1;
        bool motor_energizado = false;
        bool motor_energizado_falha = false;
        bool motor_energizado_universal = false;
        bool motor_ligado = false;
        bool motor_ligado_falha = false;
        bool motor_ligado_universal = false;

        //------------- FIM ----------------

        // CancellationTokenSource para controlar cancelamento das tasks/threads do form

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
                if (float.TryParse(inputVelocidade1, NumberStyles.Any, new CultureInfo("pt-BR"), out float velocidade))
                {
                    // Calcula os pulsos com base no valor convertido
                    if (constanteCalibracao1 != 0)
                        velocidade_pulsos1 = (float)Math.Round(velocidade / constanteCalibracao1);

                        velocidade_pulsos2 = (float)Math.Round(velocidade / constanteCalibracao1);
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
                if (float.TryParse(inputDistancia1, NumberStyles.Any, new CultureInfo("pt-BR"), out float distancia))
                {
                    // Calcula os pulsos com base no valor convertido
                    if (constanteCalibracao1 != 0)
                        distancia_pulsos1 = (float)Math.Round(distancia / constanteCalibracao1);

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
                motor_energizado_falha = false;
                MessageBox.Show("Os motores pararam!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void AtualizarInterfaceMotorFalha(bool ligado)
        {
            if (!ligado)
            {
                button5.Text = "Parar";
                button5.BackColor = Color.Red;
                button5.Enabled = true;
                motor_ligado_falha = true;
                button6.Text = "Acoplado";
                button6.BackColor = Color.Green;
                button6.Enabled = false;
                motor_energizado_falha = true;
            }
            else
            {
                button5.Text = "Ligar";
                button5.BackColor = SystemColors.Control;
                button5.Enabled = true;
                motor_ligado_falha = false;
                button6.Text = "Acoplar";
                button6.BackColor = SystemColors.Control;
                button6.Enabled = true;
                motor_energizado_falha = false;
                MessageBox.Show("Os motores pararam!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void AtualizarInterfaceMotorUniversal(bool ligado)
        {
            if (!ligado)
            {
                button3.Text = "Parar";
                button3.BackColor = Color.Red;
                button3.Enabled = true;
                motor_ligado_universal = true;
                button4.Text = "Acoplados";
                button4.BackColor = Color.Green;
                button4.Enabled = false;
                motor_energizado_universal = true;
            }
            else
            {
                button3.Text = "Ligar Simultâneo";
                button3.BackColor = SystemColors.Control;
                button3.Enabled = true;
                motor_ligado_universal = false;
                button4.Text = "Acoplar Simultâneo";
                button4.BackColor = SystemColors.Control;
                button4.Enabled = true;
                motor_energizado_universal = false;
                MessageBox.Show("Os motores pararam!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            ''//RecalcularDistanciaEVelocidade();

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

            button2.Enabled = false; // Bloqueia múltiplos cliques

            try
            {
                if (!motor_ligado)
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

                    Console.WriteLine($"Comando: {comando}");

                    await Task.Run(() =>
                    {
                        controleSerial.Enviar("m#");
                        controleSerial.Enviar("A#");
                        controleSerial.Enviar(comando);
                        Console.WriteLine($"Comando Enviado: {comando}");
                    });

                    motor_ligado = true;
                    AtualizarInterfaceMotor(false); // Passa FALSE para atualizar corretamente o botão para "Parar"
                }
                else
                {
                    await Task.Run(() =>
                    {
                        controleSerial.Enviar("m#");
                        controleSerial.Enviar("n#");
                        controleSerial.Enviar("a#");
                        paradaPorBotao = true;
                        Console.WriteLine("Comando Enviado: n# (Parar motor)");
                    });

                    motor_ligado = false;
                    AtualizarInterfaceMotor(true); // Passa TRUE para atualizar corretamente o botão para "Ligar"
                }
            }
            catch (OperationCanceledException)
            {
                // Ignorar cancelamento
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao comunicar com a porta serial: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                button2.Enabled = true; // Reabilita o botão
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
                if (motor_energizado == false)
                {
                    // Enviar comando para energizar o motor
                    controleSerial.Enviar("m#");
                    controleSerial.Enviar("A#");
                    button1.Text = "Acoplado";
                    button1.BackColor = Color.Green;
                }
                else
                {
                    // Enviar comando para desenergizar o motor
                    controleSerial.Enviar("m#");
                    controleSerial.Enviar("a#");
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

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void richTextBox6_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(richTextBox6.Text))
            {
                // Substitui pontos por vírgulas para o formato brasileiro
                string inputConstanteCalibracao2 = richTextBox6.Text.Replace('.', ',');

                // Tenta converter a string para double
                if (double.TryParse(inputConstanteCalibracao2, NumberStyles.Any, new CultureInfo("pt-BR"), out double valorConvertido))
                {
                    constanteCalibracao2 = valorConvertido; // Atualiza apenas se a conversão for bem-sucedida

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
                constanteCalibracao2 = 1; // Define um valor padrão quando o campo está vazio

                // Recalcula as variáveis de distância e velocidade com o valor padrão
                RecalcularDistanciaEVelocidade();
            }
        }

        private void richTextBox5_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(richTextBox5.Text))
            {
                // Substitui pontos por vírgulas para o formato brasileiro
                string inputVelocidade2 = richTextBox5.Text.Replace('.', ',');


                // Tenta converter a string para float
                if (float.TryParse(inputVelocidade2, NumberStyles.Any, new CultureInfo("pt-BR"), out float velocidade))
                {
                    // Calcula os pulsos com base no valor convertido
                    if (constanteCalibracao1 != 0)
                        velocidade_pulsos3 = (float)Math.Round(velocidade / constanteCalibracao2);

                    velocidade_pulsos4 = velocidade_pulsos3;

                }
                else
                {
                    MessageBox.Show("Por favor, insira um valor numérico válido.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                // Define valores padrão caso o campo fique vazio
                velocidade_pulsos3 = 0;
                velocidade_pulsos4 = 0;
            }
        }

        private void richTextBox4_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(richTextBox4.Text))
            {
                // Substitui pontos por vírgulas para o formato brasileiro
                string inputDistancia2 = richTextBox4.Text.Replace('.', ',');

                // Tenta converter a string para float
                if (float.TryParse(inputDistancia2, NumberStyles.Any, new CultureInfo("pt-BR"), out float distancia))
                {
                    // Calcula os pulsos com base no valor convertido
                    if (constanteCalibracao2 != 0)
                        distancia_pulsos3 = (float)Math.Round(distancia / constanteCalibracao2);

                    distancia_pulsos4 = distancia_pulsos3;

                    
                }
                else
                {
                    MessageBox.Show("Por favor, insira um valor numérico válido.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                // Define valores padrão caso o campo fique vazio
                distancia_pulsos3 = 0;
                distancia_pulsos4 = 0;
            }

        }

        private async void button5_Click(object sender, EventArgs e)
        {
            //RecalcularDistanciaEVelocidade();

            if (radioButton3.Checked && !radioButton4.Checked)
            {
                direcao2 = "B";
            }
            else if (!radioButton3.Checked && radioButton4.Checked)
            {
                direcao2 = "C";
            }
            else
            {
                MessageBox.Show("Por favor, selecione uma direção.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!VerificarTextoValido(richTextBox4) || !VerificarTextoValido(richTextBox5))
            {
                MessageBox.Show("Por favor, selecione valores válidos para distância e velocidade.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            button5.Enabled = false; // Bloqueia múltiplos cliques

            try
            {
                if (!motor_ligado_falha)
                {

                    string comando = string.Format(
                        CultureInfo.InvariantCulture,
                        "L{0};{1};{2};{3};{4};H#",
                        distancia_pulsos3,
                        velocidade_pulsos3,
                        distancia_pulsos4,
                        velocidade_pulsos4,
                        direcao2
                    );

                    Console.WriteLine($"Comando: {comando}");

                    await Task.Run(() =>
                    {
                        controleSerial.Enviar("l#");
                        controleSerial.Enviar("A#");
                        controleSerial.Enviar(comando);
                        Console.WriteLine($"Comando Enviado: {comando}");
                    });

                    motor_ligado_falha = true;
                    AtualizarInterfaceMotorFalha(false); // Passa FALSE para atualizar corretamente o botão para "Parar"
                }
                else
                {
                    await Task.Run(() =>
                    {
                        controleSerial.Enviar("l#");
                        controleSerial.Enviar("n#");
                        controleSerial.Enviar("a#");
                        paradaPorBotao = true;
                        Console.WriteLine("Comando Enviado: n# (Parar motor)");
                    });

                    motor_ligado_falha = false;
                    AtualizarInterfaceMotorFalha(true); // Passa TRUE para atualizar corretamente o botão para "Ligar"
                }
            }
            catch (OperationCanceledException)
            {
                // Ignorar cancelamento
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao comunicar com a porta serial: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                button5.Enabled = true; // Reabilita o botão
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (motor_ligado_falha)
            {
                MessageBox.Show("Pare os motores antes de desenergizá-los.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                if (motor_energizado_falha == false)
                {
                    // Enviar comando para energizar o motor
                    controleSerial.Enviar("l#");
                    controleSerial.Enviar("A#");
                    button6.Text = "Acoplado";
                    button6.BackColor = Color.Green;
                }
                else
                {
                    // Enviar comando para desenergizar o motor
                    controleSerial.Enviar("l#");
                    controleSerial.Enviar("a#");
                    button6.Text = "Acoplar";
                    button6.BackColor = SystemColors.Control;
                }

                // Inverter o estado da variável
                motor_energizado_falha = !motor_energizado_falha;
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

        private async void button3_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked && !radioButton2.Checked)
            {
                direcao = "B";
            }
            else if (!radioButton1.Checked && radioButton2.Checked)
            {
                direcao = "C";
            }
            if (radioButton3.Checked && !radioButton4.Checked)
            {
                direcao2 = "B";
            }
            else if (!radioButton3.Checked && radioButton4.Checked)
            {
                direcao2 = "C";
            }
            else
            {
                MessageBox.Show("Por favor, selecione uma direção.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!VerificarTextoValido(richTextBox1) || !VerificarTextoValido(richTextBox2) || !VerificarTextoValido(richTextBox4) || !VerificarTextoValido(richTextBox5))
            {
                MessageBox.Show("Por favor, selecione valores válidos para distância e velocidade.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            button3.Enabled = false; // Bloqueia múltiplos cliques

            try
            {
                if (!motor_ligado_universal)
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

                    string comando2 = string.Format(
                        CultureInfo.InvariantCulture,
                        "L{0};{1};{2};{3};{4};H#",
                        distancia_pulsos3,
                        velocidade_pulsos3,
                        distancia_pulsos4,
                        velocidade_pulsos4,
                        direcao2
                    );

                    await Task.Run(() =>
                    {
                        controleSerial.Enviar("m#");
                        controleSerial.Enviar("A#");
                        controleSerial.Enviar("l#");
                        controleSerial.Enviar("A#");
                        controleSerial.Enviar(comando);
                        controleSerial.Enviar(comando2);
                        Console.WriteLine($"Comando Enviado: {comando}");
                    });

                    motor_ligado_universal = true;
                    AtualizarInterfaceMotorUniversal(false); // Passa FALSE para atualizar corretamente o botão para "Parar"
                }
                else
                {
                    await Task.Run(() =>
                    {
                        controleSerial.Enviar("m#");
                        controleSerial.Enviar("n#");
                        controleSerial.Enviar("a#");
                        controleSerial.Enviar("l#");
                        controleSerial.Enviar("n#");
                        controleSerial.Enviar("a#");
                        paradaPorBotao = true;
                        Console.WriteLine("Comando Enviado: n# (Parar motor)");
                    });

                    motor_ligado_universal = false;
                    AtualizarInterfaceMotorUniversal(true); // Passa TRUE para atualizar corretamente o botão para "Ligar"
                }

              
            }
            catch (OperationCanceledException)
            {
                // Ignorar cancelamento
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao comunicar com a porta serial: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                button3.Enabled = true; // Reabilita o botão
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (motor_ligado_universal)
            {
                MessageBox.Show("Pare os motores antes de desenergizá-los.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                if (motor_energizado_universal == false)
                {
                    // Enviar comando para energizar o motor
                    controleSerial.Enviar("l#");
                    controleSerial.Enviar("A#");
                    controleSerial.Enviar("m#");
                    controleSerial.Enviar("A#");
                    button4.Text = "Acoplado";
                    button4.BackColor = Color.Green;
                }
                else
                {
                    // Enviar comando para desenergizar o motor
                    controleSerial.Enviar("l#");
                    controleSerial.Enviar("a#");
                    controleSerial.Enviar("m#");
                    controleSerial.Enviar("a#");
                    button4.Text = "Acoplar";
                    button4.BackColor = SystemColors.Control;
                }

                // Inverter o estado da variável
                motor_energizado_universal = !motor_energizado_universal;
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
    }
}
