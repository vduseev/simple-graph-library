using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphLibrary
{
    public partial class AddGraphForm : Form
    {
        public AddGraphForm()
        {
            InitializeComponent();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AddBTN_Click(object sender, EventArgs e)
        {
            // read amount of vertices
            string name = textBox3.Text;
                
            char[]      delimeters = {' '};
            string[]    names = textBox1.Text.Split(delimeters);
            string[]    lines = textBox2.Lines;
                
            int n = names.Length;

            int[,] matrix = new int[n, n];
                
            for (int i = 0; i < n - 1; i++)
                for (int j = i + 1; j < n; j++)
                    if (names[i] == names[j])
                    {
                        MessageBox.Show("В поле имён вершин задано две одинаковые вершины в позициях " + 
                            (i + 1).ToString() + " и " + (j + 1).ToString());
                        return;
                    }

            if (names.Length > 0 && names.Length == lines.Length && textBox3.Text != "")
            {
                for (int i = 0; i < n; i++)
                {
                    string[] line = lines[i].Split(delimeters);

                    if (line.Length == n)
                    {
                        for (int j = 0; j < n; j++)
                        {
                            // read matrix element
                            if (line[j] != "~")
                                matrix[i, j] = int.Parse(line[j]);
                            else
                                matrix[i, j] = GGraph.INFINITY;
                        }
                    }
                    else
                    {
                        MessageBox.Show("В строке " + (i + 1).ToString() 
                                + " находится " + line.Length 
                                + " элементов. Это не совпадает с заданным числом вершин");
                        return;
                    }
                }

                GGraph graph = new GGraph(names, matrix, name);
                InternalAPI.Add(graph);

                this.Close();
            }
            else
                MessageBox.Show("Число вершин не совпадает с размерностью матрицы/nИли меньше единицы");
        }

        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {
            textBox1.Text = "";
            textBox1.ForeColor = Color.Black;
        }

        private void textBox2_MouseClick(object sender, MouseEventArgs e)
        {
            textBox2.Text = "";
            textBox2.ForeColor = Color.Black;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            char[]      delimeters = { ' ' };
            string[]    vertices   = textBox1.Text.Split(delimeters);

            label2.Text = vertices.Length.ToString();
        }
    }
}
