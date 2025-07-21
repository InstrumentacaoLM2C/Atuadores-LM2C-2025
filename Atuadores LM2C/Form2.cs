using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Atuadores_LM2C
{
    public partial class Form2 : Form
    {
        private ControleSerial controleSerial;
        private bool paradaPorBotao1 = false;
        private bool paradaPorBotao2 = false;

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

        //flags acionamento do motor
        bool vertical_energizado = false;
        bool horizontal_energizado = false;
        bool vertical_ligado = false;
        bool horizontal_ligado = false;
        //-----------------------FIM----------------

        // CancellationTokenSource para controlar cancelamento das tasks/threads do form
        private CancellationTokenSource cancelamentoForm = new CancellationTokenSource();

        public Form2(ControleSerial serial)
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
                    case 'y':
                        if (vertical_ligado == true)
                        {
                            vertical_ligado = false;
                            AtualizarInterfaceMotorVertical(vertical_ligado);

                            if (paradaPorBotao1 == true)
                            {
                                paradaPorBotao1 = false;
                            }
                            else
                            {

                            }
                        }
                        break;

                    case 'Y':
                        if (horizontal_ligado == true)
                        {
                            horizontal_ligado = false;
                            AtualizarInterfaceMotorHorizontal(horizontal_ligado);

                            if (paradaPorBotao2 == true)
                            {
                                paradaPorBotao2 = false;
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
#pragma warning restore CS0168 // A variável foi declarada, mas nunca foi usada
                  
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.FormClosing += Form2_FormClosing;
            richTextBox2.Enabled = false;
            richTextBox3.Enabled = false;
            richTextBox4.Enabled = false;
            richTextBox5.Enabled = false;

        }

        private void CancelarTarefasForm()
        {
            if (cancelamentoForm != null && !cancelamentoForm.IsCancellationRequested)
                cancelamentoForm.Cancel();
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

        private void RecalcularDistanciaEVelocidade1()
        {
            // Recalcula os pulsos de distância e velocidade com a nova constante de calibração
            if (constanteCalibracao1 != 0)
            {
                distancia_pulsos1 = (float)(distancia_mm1 / constanteCalibracao1);
                velocidade_pulsos1 = (float)(velocidade_mm1 / constanteCalibracao1);
                Console.WriteLine("Velocidade recalculada: " + velocidade_pulsos1); 
            }
        }
        private void RecalcularDistanciaEVelocidade2()
        {
            
            if (constanteCalibracao2 != 0)
            {
                distancia_pulsos2 = (float)(distancia_mm2 / constanteCalibracao2);
                velocidade_pulsos2 = (float)(velocidade_mm2 / constanteCalibracao2);
            }
        }

        private void richTextBox6_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(richTextBox6.Text))
            {
                // Substitui pontos por vírgulas para o formato brasileiro
                string inputConstanteCalibracao2 = richTextBox6.Text.Replace('.', ',');
                richTextBox5.Enabled = true;

                // Tenta converter a string para double
                if (double.TryParse(inputConstanteCalibracao2, NumberStyles.Any, new CultureInfo("pt-BR"), out double valorConvertido))
                {
                    constanteCalibracao2 = valorConvertido; // Atualiza apenas se a conversão for bem-sucedida

                    // Agora recalcula as variáveis de distância e velocidade com a nova constante de calibração
                    RecalcularDistanciaEVelocidade2();
                }
                else
                {
                    MessageBox.Show("Por favor, insira um valor numérico válido.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                constanteCalibracao2 = 1; // Define um valor padrão quando o campo está vazio
                richTextBox5.Enabled = false;

                // Recalcula as variáveis de distância e velocidade com o valor padrão
                RecalcularDistanciaEVelocidade2();
            }
        }

        private void richTextBox5_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(richTextBox5.Text))
            {
                // Substitui pontos por vírgulas para o formato brasileiro
                string inputVelocidade2 = richTextBox5.Text.Replace('.', ',');
                richTextBox4.Enabled = true;

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
                richTextBox4.Enabled = false;
            }
        }

        private void richTextBox4_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(richTextBox4.Text))
            {
                // Substitui pontos por vírgulas para o formato brasileiro
                string inputDistancia2 = richTextBox4.Text.Replace('.', ',');

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
            if (vertical_ligado)
            {
                MessageBox.Show("Pare os motores antes de desenergizá-los.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                controleSerial.Enviar("R#"); // Seleciona motor vertical

                if (vertical_energizado == false)
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
                vertical_energizado = !vertical_energizado;
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

     
        private void AtualizarInterfaceMotorVertical(bool vertical)
        {
            if (vertical == true)
            {
                button2.Text = "Parar";
                button2.BackColor = Color.Red;
                button2.Enabled = true;
                button1.Text = "Acoplado";
                button1.BackColor = Color.Green;
                button1.Enabled = false;
            }
            else
            {
                button2.Text = "Ligar";
                button2.BackColor = SystemColors.Control;
                button2.Enabled = true;
                button1.Text = "Acoplar";
                button1.BackColor = SystemColors.Control;
                button1.Enabled = true;
                MessageBox.Show("O motor vertical parou!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void AtualizarInterfaceMotorHorizontal(bool ligado)
        {
            if (ligado == true)
            {
                button5.Text = "Parar";
                button5.BackColor = Color.Red;
                button5.Enabled = true;
                button6.Text = "Acoplado";
                button6.BackColor = Color.Green;
                button6.Enabled = false;
            }
            else
            {
                button5.Text = "Ligar";
                button5.BackColor = SystemColors.Control;
                button5.Enabled = true;
                button6.Text = "Acoplar";
                button6.BackColor = SystemColors.Control;
                button6.Enabled = true;
                MessageBox.Show("O motor horizontal parou!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private async void button2_Click(object sender, EventArgs e)
        {

            if (radioButton1.Checked && !radioButton2.Checked)
            {
                direcao1 = "C";
            }
            else if (!radioButton1.Checked && radioButton2.Checked)
            {
                direcao1 = "B";
            }

            else
            {
                MessageBox.Show("Por favor, selecione uma direção.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!VerificarTextoValido(richTextBox1) || !VerificarTextoValido(richTextBox2) || !VerificarTextoValido(richTextBox1))
            {
                MessageBox.Show("Por favor, selecione valores válidos para distância e velocidade.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            button2.Enabled = false; // Bloqueia múltiplos cliques

            try
            {
                if (vertical_ligado == false)
                {                  
                    string comando = string.Format(
                    CultureInfo.InvariantCulture,
                    "T{0};{1};{2};H#",
                    distancia_pulsos1,
                    velocidade_pulsos1,
                    direcao1
                    );

                    Console.WriteLine($"Comando: {comando}");

                    await Task.Run(() =>
                    {
                        controleSerial.Enviar("R#");
                        controleSerial.Enviar("A#");
                        controleSerial.Enviar(comando);
                        Console.WriteLine($"Comando Enviado: {comando}");
                    });

                    vertical_ligado = true;
                    AtualizarInterfaceMotorVertical(vertical_ligado); 
                }
                else
                {
                    await Task.Run(() =>
                    {
                        controleSerial.Enviar("R#");
                        controleSerial.Enviar("n#");
                        controleSerial.Enviar("a#");
                        paradaPorBotao1 = true;
                        Console.WriteLine("Comando Enviado: n# (Parar motor)");
                    });

                    vertical_ligado = false;
                    AtualizarInterfaceMotorVertical(vertical_ligado); 
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




        private void button6_Click(object sender, EventArgs e)
        {
            if (horizontal_ligado)
            {
                MessageBox.Show("Pare os motores antes de desenergizá-los.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                controleSerial.Enviar("M#"); // Seleciona motor horizontal

                if (horizontal_energizado == false)
                {
                    // Enviar comando para energizar o motor
                    controleSerial.Enviar("#A");
                    button6.Text = "Acoplado";
                    button6.BackColor = Color.Green;
                }
                else
                {
                    // Enviar comando para desenergizar o motor
                    controleSerial.Enviar("#a");
                    button6.Text = "Acoplar";
                    button6.BackColor = SystemColors.Control;
                }

                // Inverter o estado da variável
                horizontal_energizado = !horizontal_energizado;
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

        private async void button5_Click(object sender, EventArgs e)
        {

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

            if (!VerificarTextoValido(richTextBox5) || !VerificarTextoValido(richTextBox4) || !VerificarTextoValido(richTextBox6))
            {
                MessageBox.Show("Por favor, selecione valores válidos para distância e velocidade.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            button5.Enabled = false; // Bloqueia múltiplos cliques

            try
            {
                if (horizontal_ligado == false)
                {
                    string comando = string.Format(
                    CultureInfo.InvariantCulture,
                    "T{0};{1};{2};H#",
                    distancia_pulsos2,
                    velocidade_pulsos2,
                    direcao2
                    );

                    Console.WriteLine($"Comando: {comando}");

                    await Task.Run(() =>
                    {
                        controleSerial.Enviar("M#");
                        controleSerial.Enviar("A#");
                        controleSerial.Enviar(comando);
                        Console.WriteLine($"Comando Enviado: {comando}");
                    });

                    horizontal_ligado = true;
                    AtualizarInterfaceMotorHorizontal(horizontal_ligado); // Passa FALSE para atualizar corretamente o botão para "Parar"
                }
                else
                {
                    await Task.Run(() =>
                    {
                        controleSerial.Enviar("M#");
                        controleSerial.Enviar("n#");
                        controleSerial.Enviar("a#");
                        paradaPorBotao2 = true;
                        Console.WriteLine("Comando Enviado: n# (Parar motor)");
                    });

                    horizontal_ligado = false;
                    AtualizarInterfaceMotorHorizontal(horizontal_ligado); // Passa TRUE para atualizar corretamente o botão para "Ligar"
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

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(richTextBox2.Text))
            {
                // Substitui pontos por vírgulas para o formato brasileiro
                string inputVelocidade1 = richTextBox2.Text.Replace('.', ',');
                richTextBox3.Enabled = true;

                // Tenta converter a string para float
                if (float.TryParse(inputVelocidade1, NumberStyles.Any, new CultureInfo("pt-BR"), out float velocidade_mm1))
                {
                    // Calcula os pulsos com base no valor convertido
                    if (constanteCalibracao1 != 0)
                    {
                        velocidade_pulsos1 = (float)Math.Round(velocidade_mm1 / constanteCalibracao1);
                        Console.WriteLine("Velocidade dividida pela constante: " + velocidade_pulsos1);
                    }
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
                richTextBox3.Enabled = false;
            }
        }

        private void btnAjuda_Click(object sender, EventArgs e)
        {
            Form2_Ajuda form2_ajuda = new Form2_Ajuda();
            form2_ajuda.ShowDialog();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {

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
                    {
                        distancia_pulsos1 = (float)Math.Round(distancia_mm1 / constanteCalibracao1);
                    }

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

                richTextBox2.Enabled = true;

                // Tenta converter a string para double
                if (double.TryParse(inputConstanteCalibracao1, NumberStyles.Any, new CultureInfo("pt-BR"), out double valorConvertido))
                {
                    constanteCalibracao1 = valorConvertido; // Atualiza apenas se a conversão for bem-sucedida

                    // Agora recalcula as variáveis de distância e velocidade com a nova constante de calibração
                    RecalcularDistanciaEVelocidade1();
                }
                else
                {
                    MessageBox.Show("Por favor, insira um valor numérico válido.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                constanteCalibracao1 = 1; // Define um valor padrão quando o campo está vazio
                richTextBox2.Enabled = false;

                // Recalcula as variáveis de distância e velocidade com o valor padrão
                RecalcularDistanciaEVelocidade1();
            }
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                controleSerial.DadosRecebidos -= ControleSerial_DadosRecebidos; // remover handler

                if (vertical_ligado || horizontal_ligado)
                {
                    controleSerial.Enviar("n#");  // Parar motor
                    vertical_ligado = false;
                    horizontal_ligado = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao tentar parar o motor antes de fechar o formulário: " + ex.Message);
            }

            CancelarTarefasForm();
        }
    }
}
