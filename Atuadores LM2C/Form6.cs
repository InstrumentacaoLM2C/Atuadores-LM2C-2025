using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Atuadores_LM2C
{
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();
        }

        private void Form6_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Exemplo de dados fixos
            List<float> distancias = new List<float> { 10, 20, 30, 40, 50 }; // eixo X
            List<float> passos = new List<float> { 2000, 4000, 6000, 8000, 10000 }; // eixo Y

            // Limpa o gráfico
            chart1.Series.Clear();

            // Cria a série dos dados experimentais (pontos)
            Series seriePontos = new Series("Dados Medidos")
            {
                ChartType = SeriesChartType.Point,
                MarkerSize = 8,
                Color = System.Drawing.Color.Blue
            };

            // Adiciona os pontos ao gráfico
            for (int i = 0; i < distancias.Count; i++)
            {
                seriePontos.Points.AddXY(distancias[i], passos[i]);
            }

            chart1.Series.Add(seriePontos);

            // Cria a série da reta de calibração (gráfico linear)
            Series serieReta = new Series("Reta de Calibração")
            {
                ChartType = SeriesChartType.Line,
                BorderWidth = 2,
                Color = System.Drawing.Color.Red
            };

            // Supondo uma reta y = m * x
            float m = 200; // exemplo de coeficiente angular

            float minX = distancias[0];
            float maxX = distancias[distancias.Count - 1];

            serieReta.Points.AddXY(minX, m * minX);
            serieReta.Points.AddXY(maxX, m * maxX);

            chart1.Series.Add(serieReta);

            // Ajusta os títulos dos eixos
            chart1.ChartAreas[0].AxisX.Title = "Distância (mm)";
            chart1.ChartAreas[0].AxisY.Title = "Passos";
        }
    }
}
