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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Atuadores_LM2C
{
    public partial class Form3_Ajuda : Form
    {
        private bool distanciaEditada = false;
        private bool velocidadeEditada = false;
        private bool constanteEditada = false;
        private bool motorAcoplado = false;
        private bool motorLigado = false;
        public Form3_Ajuda()
        {
            InitializeComponent();
        }

        private void Form3_Ajuda_Load(object sender, EventArgs e)
        {
            // ToolTips
            toolTip1.SetToolTip(richTextBox1, "Inserir a constante de calibração.");
            toolTip1.SetToolTip(richTextBox2, "Inserir a velocidade que o motor irá percorrer.");
            toolTip1.SetToolTip(richTextBox3, "Inserir a distância que o motor irá percorrer.");
            toolTip1.SetToolTip(radioButton1, "Seleciona a direção para subir.");
            toolTip1.SetToolTip(radioButton2, "Seleciona a direção para descer.");
            toolTip1.SetToolTip(button1, "Acopla o motor, impedindo o giro dele de forma manual.");
            toolTip1.SetToolTip(button2, "Liga o motor com base nas informações inseridas pelo usuário.");

            // Exemplos nos campos
            richTextBox1.Text = "Ex: 200,0";
            richTextBox2.Text = "Ex: 50,0";
            richTextBox3.Text = "Ex: 100,0";

            this.ActiveControl = null;
        }

        private bool TextoEhNumero(string texto)
        {
            // Aceita ponto ou vírgula como separador decimal
            texto = texto.Replace(',', '.');
            return double.TryParse(texto, NumberStyles.Float, CultureInfo.InvariantCulture, out _);
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                MessageBox.Show("Direção de subida selecionada. O motor se moverá para cima.");
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                MessageBox.Show("Direção de descida selecionada. O motor se moverá para baixo.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string erro = "";

            if (!TextoEhNumero(richTextBox1.Text))
                erro += "Constante de calibração inválida.\n";
            if (!TextoEhNumero(richTextBox2.Text))
                erro += "Velocidade inválida.\n";
            if (!TextoEhNumero(richTextBox3.Text))
                erro += "Distância inválida.\n";
            if (!radioButton1.Checked && !radioButton2.Checked)
                erro += "Selecione uma direção (subida ou descida).\n";

            if (!string.IsNullOrEmpty(erro))
            {
                MessageBox.Show("Erro ao tentar ligar o motor:\n" + erro, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!motorLigado)
            {
                // Ligar motor
                motorLigado = true;

                // Acoplar automaticamente
                if (!motorAcoplado)
                {
                    motorAcoplado = true;
                    button1.Text = "Acoplado";
                    button1.BackColor = Color.LightGreen;
                }

                button2.Text = "Parar";
                button2.BackColor = Color.Red;

                MessageBox.Show("Motor ligado com sucesso!");
            }
            else
            {
                // Parar motor
                motorLigado = false;
                button2.Text = "Ligar Motor";
                button2.BackColor = SystemColors.Control;

                MessageBox.Show("Motor desligado.");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (motorLigado)
            {
                MessageBox.Show("Não é possível desacoplar o motor enquanto ele estiver ligado.");
                return;
            }

            motorAcoplado = !motorAcoplado;

            if (motorAcoplado)
            {
                button1.Text = "Acoplado";
                button1.BackColor = Color.LightGreen;
                MessageBox.Show("O motor foi acoplado. Agora ele não pode ser girado manualmente.");
            }
            else
            {
                button1.Text = "Acoplar Motor";
                button1.BackColor = SystemColors.Control;
                MessageBox.Show("O motor foi desacoplado. Ele pode ser girado manualmente.");
            }
        }

        private void richTextBox1_Enter(object sender, EventArgs e)
        {
            if (!constanteEditada)
            {
                richTextBox1.Clear();
                constanteEditada = true;
            }
        }

        private void richTextBox2_Enter(object sender, EventArgs e)
        {
            if (!velocidadeEditada)
            {
                richTextBox2.Clear();
                velocidadeEditada = true;
            }
        }

        private void richTextBox3_Enter(object sender, EventArgs e)
        {
            if (!distanciaEditada)
            {
                richTextBox3.Clear();
                distanciaEditada = true;
            }
        }
    }
}
