using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        Graphics g;
        int[,] snake = new int[257, 2];
        int length = 1;
        Timer t;
        Random r;
        int[] dot = new int[2];
        Keys curKey = Keys.W;
        int dXOld = 2, dYOld = 2;

        public Form1()
        {
            InitializeComponent();
            g = pictureBox1.CreateGraphics();
            t = new Timer();
            t.Interval = 250;
            r = new Random(); t.Tick += Step;
        }

        private void Step(object sender, EventArgs e)
        {
            int dX = 0, dY = 0;
            switch (curKey)
            {
                case Keys.W: dY = -1; break;
                case Keys.D: dX = 1; break;
                case Keys.S: dY = 1; break;
                case Keys.A: dX = -1; break;
            }

            if ((dXOld == -dX && dX != 0) || (dYOld == -dY && dY != 0))
            {
                dX = dXOld;
                dY = dYOld;
            }

            for (int i = length - 1; i >= 0; i--)
            {
                snake[i + 1, 0] = snake[i, 0];
                snake[i + 1, 1] = snake[i, 1];
            }
            snake[0, 0] = snake[1, 0] + dX;
            snake[0, 1] = snake[1, 1] + dY;


            if (snake[0, 0] == dot[0] && snake[0, 1] == dot[1])
            {
                length++;
                dot[0] = r.Next(16);
                dot[1] = r.Next(16);
            }

            dXOld = dX;
            dYOld = dY;


            if (snake[0, 0] < 0 || snake[0, 0] > 16 || snake[0, 1] < 0 || snake[0, 1] > 16)
            {
                t.Stop();
                MessageBox.Show("GAME OVER\nYOUR LENGTH - " + length.ToString());
                return;
            }

            for (int i = 1; i < length; i++)
            {
                if (snake[0, 0] == snake[i, 0] && snake[0, 1] == snake[i, 1])
                {
                    t.Stop();
                    MessageBox.Show("GAME OVER\nYOUR LENGTH - " + length.ToString());
                    return;
                }
            }

            g.Clear(Color.White);
            ColorConverter k = new ColorConverter();
            String s;
            Brush b;
            for (int i = 0; i < length; i++)
            {
                s = "#FF" + GetRandomHexNumber(6);
                b = new SolidBrush((Color)k.ConvertFromString(s));
                g.FillRectangle(b, snake[i, 0] * 32, snake[i, 1] * 32, 32, 32);
            }
            s = "#FF" + GetRandomHexNumber(6);
            b = new SolidBrush((Color)k.ConvertFromString(s));
            g.FillRectangle(b, dot[0] * 32, dot[1] * 32, 32, 32);

        }


        public string GetRandomHexNumber(int digits)
        {
            byte[] buffer = new byte[digits / 2];
            r.NextBytes(buffer);
            string result = String.Concat(buffer.Select(x => x.ToString("X2")).ToArray());
            if (digits % 2 == 0)
                return result;
            return result + r.Next(16).ToString("X");
        }


        private void button1_Click(object sender, EventArgs e)
        {
            length = 1;
            snake[0, 0] = 8;
            snake[0, 1] = 12;
            curKey = Keys.W;

            dot[0] = r.Next(16);
            dot[1] = r.Next(16);
            t.Start();

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (t.Enabled && (e.KeyCode == Keys.W || e.KeyCode == Keys.A || e.KeyCode == Keys.S || e.KeyCode == Keys.D))
            {
                curKey = e.KeyCode;
            }
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            t.Interval = trackBar1.Value;
        }
    }
}
