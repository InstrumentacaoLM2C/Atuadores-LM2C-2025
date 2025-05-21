using System;
using System.Drawing;
using System.Windows.Forms;

namespace Atuadores_LM2C
{
    public partial class Form1_Ajuda : Form
    {
        private bool conectado = false;           // Estado da conexão serial
        private bool exemplosCarregados = false;  // Se os exemplos já foram carregados

        public Form1_Ajuda()
        {
            InitializeComponent();
        }

        private void Form1_Ajuda_Load(object sender, EventArgs e)
        {
            // ToolTips
            toolTip1.SetToolTip(button1, "Abre um formulário com opções adicionais.");
            toolTip1.SetToolTip(button2, "Abre outro formulário de controle.");
            toolTip1.SetToolTip(button3, "Carrega exemplos no menu suspenso.");
            toolTip1.SetToolTip(button4, "Conecta ou desconecta da porta serial.");
            toolTip1.SetToolTip(comboBox1, "Selecione um exemplo da lista.");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!conectado)
            {
                MessageBox.Show(
                    "Você precisa estar conectado à porta serial antes de clicar neste botão.",
                    "Erro - Não conectado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }

            MessageBox.Show(
                "Este botão abrirá a página de controle do Motor Bidirecional",
                "Ajuda - Bidirecional",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

            // Aqui você abriria seu formulário real
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!conectado)
            {
                MessageBox.Show(
                    "Você precisa estar conectado à porta serial antes de clicar neste botão.",
                    "Erro - Não conectado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }

            MessageBox.Show(
                "Este botão abrirá a página de controle do Motor Universal",
                "Ajuda - Universal",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

            // Aqui você abriria seu formulário real
        }

        private void button3_Click(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox1.Items.Add("Exemplos");
            comboBox1.Items.Add("COM20");
            comboBox1.Items.Add("COM3");
            comboBox1.Items.Add("COM10");

            exemplosCarregados = true;

            MessageBox.Show(
                "Exemplos carregados com sucesso. Agora você pode conectar e selecionar portas de comunicação da lista.",
                "Portas COM",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!exemplosCarregados)
            {
                MessageBox.Show(
                    "Você precisa escanear as portas antes de se conectar a uma delas.",
                    "Erro - Portas não escaneadas",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            conectado = !conectado;

            if (conectado)
            {
                button4.Text = "Desconectar";
                button4.BackColor = Color.Red;
                button4.ForeColor = Color.White;
                toolTip1.SetToolTip(button4, "Clique para desconectar da porta serial.");

                MessageBox.Show(
                    "Conectado com sucesso à porta serial.",
                    "Conectado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            else
            {
                button4.Text = "Conectar";
                button4.BackColor = SystemColors.Control;
                button4.ForeColor = SystemColors.ControlText;
                toolTip1.SetToolTip(button4, "Clique para conectar à porta serial.");

                MessageBox.Show(
                    "Você foi desconectado da porta serial.",
                    "Desconectado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string item = comboBox1.SelectedItem?.ToString();
            if (!string.IsNullOrEmpty(item))
            {
                MessageBox.Show($"Você selecionou: {item}", "Porta COM Selecionada com Sucesso!");
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
