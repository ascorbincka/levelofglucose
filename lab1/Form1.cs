using System;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;
using System.Data;

namespace lab1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public void fill1(double[,] matrix)
        {
            Random random = new Random();

            // заполняем первую строку (рост человека N(165, 225))
            for (int j = 0; j < 500; j++)
            {
                matrix[0, j] = random.Next(165, 226);

            }

            // заполняем вторую строку (вес человека N(62, 100))
            for (int j = 0; j < 500; j++)
            {
                matrix[1, j] = random.Next(62, 101);
            }

        }
        public void clean(double[,] matrix)
        {
            for (int j = 0; j < 500; j++)
            {
                double rost = matrix[0, j];
                double ves = matrix[1, j];
                if (rost > 195 && ves < 75)
                {
                    matrix[0, j] = -1;
                    matrix[1, j] = -1;
                    matrix[2, j] = -1;
                    matrix[3, j] = -1;
                }
                else if (rost < 175 && ves > 90)
                {
                    matrix[0, j] = -1;
                    matrix[1, j] = -1;
                    matrix[2, j] = -1;
                    matrix[3, j] = -1;
                }

            }
        }
        // создаем матрицу 4 на 500,заполненную нулями
        double[,] X = new double[4, 500];
        Random rand = new Random();

        private void button1_Click(object sender, EventArgs e)
        {
            fill1(X);
            Form3 newForm = new Form3(this);
            newForm.Show();
            string before = "";
            for (int k = 0; k < 4; k++)
            {
                for (int j = 0; j < 500; j++)
                {
                    before += X[k, j] + " ";
                }
                before += Environment.NewLine;
            }
            File.WriteAllText("text.txt", before);
            clean(X);

            string after = "";
            for (int k = 0; k < 4; k++)
            {
                for (int j = 0; j < 500; j++)
                {
                    after += X[k, j] + " ";
                }
                after += Environment.NewLine;
            }
            File.WriteAllText("text2.txt", after);
            chart1.Series.Clear();
            // подготовка графика
            var series = new Series
            {
                Name = "MySeries",
                Color = System.Drawing.Color.Blue,
                IsVisibleInLegend = false,
                ChartType = SeriesChartType.Point  //  точечный
            };

            // добавление точек
            for (int i = 0; i < 500; i++)
            {
                if (X[0, i] != -1 && X[1, i] != -1)
                {
                    series.Points.AddXY(X[1, i], X[0, i]);
                }
            }

            // Добавляем серию на Chart
            chart1.Series.Add(series); // 
            chart1.ChartAreas[0].AxisX.Title = "Вес";
            chart1.ChartAreas[0].AxisY.Title = "Рост";
        }


        private void button2_Click(object sender, EventArgs e)
        {
            // Расчет соотношения веса к росту 
            double[] vesrost = new double[500];
            int[] groups = new int[10];
            double[] doli = new double[] { 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1 };


            for (int i = 0; i < 500; i++)
            {
                if (X[0, i] != -1 && X[1, i] != -1)
                {
          
                    vesrost[i] = X[1, i] / X[0, i];
                    if (vesrost[i] >= 0 && vesrost[i] <= 0.1)
                        groups[0]++;
                    else if (vesrost[i] > 0.1 && vesrost[i] <= 0.2)
                        groups[1]++;
                    else if (vesrost[i] > 0.2 && vesrost[i] <= 0.3)
                        groups[2]++;
                    else if (vesrost[i] > 0.3 && vesrost[i] <= 0.4)
                        groups[3]++;
                    else if (vesrost[i] > 0.4 && vesrost[i] <= 0.5)
                        groups[4]++;
                    else if (vesrost[i] > 0.5 && vesrost[i] <= 0.6)
                        groups[5]++;
                    else if (vesrost[i] > 0.6 && vesrost[i] <= 0.7)
                        groups[6]++;
                    else if (vesrost[i] > 0.7 && vesrost[i] <= 0.8)
                        groups[7]++;
                    else if (vesrost[i] > 0.8 && vesrost[i] <= 0.9)
                        groups[8]++;
                    else if (vesrost[i] > 0.9 && vesrost[i] <= 1)
                        groups[9]++;
                }

            }

            chart2.Series.Clear();
            chart2.Series.Add(new Series("Ratios"));
            chart2.Series["Ratios"].ChartType = SeriesChartType.Column;


            // Добавление данных в гистограмму
            int j = 0;
            for (int i = 0; i < 10; i++)
            {
                if (vesrost[i] != -1)
                {
                    chart2.Series["Ratios"].Points.AddXY( doli[i], groups[i]);
                    j++;
                }
            }


        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (double.TryParse(textBox2.Text, out _))
            {
                int sigma = 5;// отклонение для шума
                double tau = double.Parse(textBox2.Text);//пороговое нормальное значение глюкозы
                // расчет соотношения веса к росту и добавление шума
                Random rand = new Random();

                for (int i = 0; i < 500; i++)
                {
                    if (X[0, i] != -1 && X[1, i] != -1)
                    {
                        double weightToHeightRatio = X[1, i] / X[0, i]; // соотношение веса к росту
                        double noise = rand.Next(0, sigma * sigma); // генерация  шума
                        X[2, i] = weightToHeightRatio + noise; // уровень глюкозы
                    }
                }
                int z = 0;
                int b = 0;
                for (int i = 0; i < 500; i++)
                {
                    if (X[2, i] != -1)
                    {
                        if (X[2, i] <= tau)
                        {
                            X[3, i] = 0;
                            z++;
                        }
                        else
                        {
                            X[3, i] = 1;
                            b++;
                        }
                    }
                }
                textBox1.Text = b.ToString();
                textBox3.Text = z.ToString();
                chart3.Series.Clear();
                // подготовка графика
                var series = new Series
                {
                    Name = "MySeries",
                    Color = System.Drawing.Color.Blue,
                    IsVisibleInLegend = false,
                    ChartType = SeriesChartType.Point  //  тип графика (точечный)
                };

                var lineSeries = new Series
                {
                    Name = "Horizontal Line",
                    Color = System.Drawing.Color.Red,
                    IsVisibleInLegend = false,
                    ChartType = SeriesChartType.Line
                };

                

                // добавление точек
                int sch = 0;
                for (int i = 0; i < 500; i++)
                {
                    if (X[2, i] != -1)
                    {
                        sch++;
                        series.Points.AddXY(sch, X[2, i]);
                    }
                }
                double value = double.Parse(textBox2.Text);


                // Указываем диапазон X для линии
                lineSeries.Points.AddXY(0,value);
                lineSeries.Points.AddXY(500, value);

                chart3.Series.Add(lineSeries);
                // Добавляем серию на Chart
                chart3.Series.Add(series); // 
        
             
            }
          
        }
        
    }
}


