using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace picture2bitmap
{
    public partial class Form1 : Form
    {

        Image image { get; set; }
        Bitmap bmap { get; set; }
        string filename = null;
        int bll = 768 / 2;
        public Form1()
        {
            InitializeComponent();
            this.Text = "Picture2BitMap";
            openFileDialog1.Filter = "JPEG(*.jpeg,*.jpg)|*.jp*|Bitmap(*.bmp)|*.bmp|PNG(*.png)|*.png|All files(*.*)|*.*";
            saveFileDialog1.Filter = "JPEG(*.jpeg,*.jpg)|*.jpg|Bitmap(*.bmp)|*.bmp|PNG(*.png)|*.png|All files(*.*)|*.*";
            numericUpDown3.Value = trackBar1.Value = bll;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            filename = openFileDialog1.FileName;
            //MessageBox.Show("File opened");
            image = Image.FromFile(filename);
            pictureBox1.Image = image;
            if (checkBox1.Checked)
            {
                numericUpDown1.Value = numericUpDown1.Maximum = image.Height;
                numericUpDown2.Value = numericUpDown2.Maximum = image.Width;
                trackBar2.Maximum = image.Height;
                trackBar3.Maximum = image.Width;
            }
            this.Text = filename + " - Picture2BitMap";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (filename != null)
            {
                int h = (int)numericUpDown1.Value;
                int w = (int)numericUpDown2.Value;
                Bitmap bitmap;// = new Bitmap(w, h);
                bitmap = (Bitmap)image;
                if ((bitmap.PixelFormat != System.Drawing.Imaging.PixelFormat.Indexed) &&
                    (bitmap.PixelFormat != System.Drawing.Imaging.PixelFormat.Format1bppIndexed) &&
                    (bitmap.PixelFormat != System.Drawing.Imaging.PixelFormat.Format4bppIndexed) &&
                    (bitmap.PixelFormat != System.Drawing.Imaging.PixelFormat.Format8bppIndexed))
                {

                    for (int i = 0; i < bitmap.Height; i++)
                        for (int j = 0; j < bitmap.Width; j++)
                        {
                            Color color = bitmap.GetPixel(j, i);
                            byte r = color.R;
                            byte g = color.G;
                            byte b = color.B;
                            byte c = 0;
                            if (r + b + g > bll)
                                c = 255;
                            bitmap.SetPixel(j, i, Color.FromArgb(c, c, c));
                        }
                }
                //pictureBox1.Image = bitmap;
                int x = 0;
                int y = 0;
                //int xmax = x;
                //int ymax = y;
                if (checkBox2.Checked)
                {
                    int height = bitmap.Height;
                    int width = bitmap.Width;
                    x = width / 2 - w / 2;
                    y = height / 2 - h / 2;
                    //xmax = width / 2 + x/2;
                    //ymax = height / 2 + y/2;
                }
                else
                {
                    x = 0;
                    y = 0;
                    //xmax = x;
                    //ymax = y;
                }
                //MessageBox.Show(x.ToString() + " " + y.ToString() + " " + w.ToString() + " " + h.ToString());
                Bitmap cloneBitmap = bitmap.Clone(new Rectangle(x, y, w, h), System.Drawing.Imaging.PixelFormat.Format1bppIndexed);
                pictureBox1.Image = cloneBitmap;
                bmap = cloneBitmap;

            }
            else
            {
                MessageBox.Show("No image");
            }
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            bll = (int)numericUpDown3.Value;
            trackBar1.Value = (int)numericUpDown3.Value;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            bll = trackBar1.Value;
            numericUpDown3.Value = trackBar1.Value;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            filename = saveFileDialog1.FileName;
            pictureBox1.Image.Save(filename);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            /*buffer = Clipboard.GetText();*/
            Clipboard.SetText(richTextBox1.Text);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = null;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = bmap;//(Bitmap)pictureBox1.Image;
            if (bitmap != null)
            /*if (bitmap.PixelFormat != System.Drawing.Imaging.PixelFormat.Format1bppIndexed)*/
            {
                int h = bitmap.Height;
                int w = bitmap.Width;
                int[,] map = new int[w + 1, h / 8 + 1];/*w, h / 8*/
                for (int j = 0; j < h - 7; j += 8)
                {
                    for (int i = 0; i < w; i++)
                    {
                        //MessageBox.Show(i.ToString() + " " + j.ToString());
                        map[i, j/8] += bitmap.GetPixel(i, j + 0).R / 255 * 1;
                        map[i, j/8] += bitmap.GetPixel(i, j + 1).R / 255 * 2;
                        map[i, j/8] += bitmap.GetPixel(i, j + 2).R / 255 * 2 * 2;
                        map[i, j/8] += bitmap.GetPixel(i, j + 3).R / 255 * 2 * 2 * 2;
                        map[i, j/8] += bitmap.GetPixel(i, j + 4).R / 255 * 2 * 2 * 2 * 2;
                        map[i, j/8] += bitmap.GetPixel(i, j + 5).R / 255 * 2 * 2 * 2 * 2 * 2;
                        map[i, j/8] += bitmap.GetPixel(i, j + 6).R / 255 * 2 * 2 * 2 * 2 * 2 * 2;
                        map[i, j/8] += bitmap.GetPixel(i, j + 7).R / 255 * 2 * 2 * 2 * 2 * 2 * 2 * 2;
                        if (!checkBox3.Checked)
                            map[i, j / 8] = 255 - map[i, j / 8];
                        richTextBox1.Text += "0x" + map[i, j/8].ToString("x2") + ", ";
                    }
                    richTextBox1.Text +="\n";
                }
            }
            /*else
            {
                MessageBox.Show("Firstly click button \"Black make\"");
            }*/
            else
            {
                MessageBox.Show("Firstly click button \"Make Black\"");
            }
        }
    }
}
