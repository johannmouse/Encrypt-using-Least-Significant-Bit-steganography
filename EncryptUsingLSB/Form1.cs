using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace EncryptUsingLSB
{
    public partial class Form1 : Form
    {
        Bitmap bmp;
        Bitmap bmpWork;
        Bitmap bmpResize;
        const string END_MARK = "vinhy9x";
        public Form1()
        {
            InitializeComponent();
        }

        private void Browse(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog
            {
                Filter = "Image Files(*.jpg; .jpeg; .gif; .bmp; .png)|*.jpg; .jpeg; .gif; *.bmp ;*.png"
            };
            if (open.ShowDialog() == DialogResult.OK)
            {
                bmp = new Bitmap(new Bitmap(open.FileName));
                if (bmp.Width <= bmp.Height && bmp.Height >= 281)
                {
                    bmpResize = new Bitmap(bmp, (int)(bmp.Width / ((float)bmp.Height / 281)), 281);
                }
                else if (bmp.Width > bmp.Height && bmp.Width >= 547)
                {
                    bmpResize = new Bitmap(bmp, 547, (int)(bmp.Height / ((float)bmp.Width / 547)));
                }
                else
                {
                    bmpResize = bmp;
                }
                pictureBox1.Image = bmpResize;
                textBox1.Text = open.FileName;
            }
        }

        private void Generate(object sender, EventArgs e)
        {
            string message = richTextBox1.Text;
            if (string.IsNullOrEmpty(message))
            {
                MessageBox.Show("Enter your message!");
                return;
            }
            Encrypt(message);
        }

        public static string StringToBinary(string message)
        {
            StringBuilder sb = new StringBuilder();

            foreach (char c in message)
            {
                sb.Append(Convert.ToString(c, 2).PadLeft(8, '0'));
            }
            return sb.ToString();
        }

        public static string BinaryToString(string binary)
        {
            try
            {
                List<Byte> byteList = new List<Byte>();

                for (int i = 0; i < binary.Length; i += 8)
                {
                    byteList.Add(Convert.ToByte(binary.Substring(i, 8), 2));
                }
                return Encoding.ASCII.GetString(byteList.ToArray());
            }
            catch
            {
                MessageBox.Show("Invalid binary string.");
                return string.Empty;
            }
        }

        private void Encrypt(string message)
        {
            if (bmp == null)
            {
                MessageBox.Show("Please choose image to hide text into");
                return;
            }
            string binaryCode = StringToBinary(message);
            binaryCode += StringToBinary(END_MARK);
            if (binaryCode.Length > bmp.Width * bmp.Height)
            {
                MessageBox.Show("The image is not big enough to hide the message. Choose another image with the size so that with x length is at least " + binaryCode.Length);
                return;
            }
            bmpWork = bmp;
            int index = 0;
            int row = 0, collumn = 0;
            StringBuilder redCode = new StringBuilder();
            while (index != binaryCode.Length)
            {
                Color color = bmpWork.GetPixel(row, collumn);
                redCode.Clear();
                redCode.Append(Convert.ToString(color.R, 2).PadLeft(8, '0'));
                redCode.Remove(7, 1).Append(binaryCode[index]);
                bmpWork.SetPixel(row, collumn, Color.FromArgb(Convert.ToInt32(redCode.ToString(), 2), color.G, color.B));
                index++;
                collumn++;
                if (collumn == bmpWork.Width)
                {
                    collumn = 0;
                    row++;
                }
            }
            if (bmpWork.Width <= bmpWork.Height && bmpWork.Height >= 281)
            {
                bmpResize = new Bitmap(bmpWork, (int)(bmpWork.Width / ((float)bmpWork.Height / 281)), 281);
            }
            else if (bmpWork.Width > bmpWork.Height && bmpWork.Width >= 547)
            {
                bmpResize = new Bitmap(bmpWork, 547, (int)(bmpWork.Height / ((float)bmpWork.Width / 547)));
            }
            else
            {
                bmpResize = bmp;
            }
            pictureBox2.Image = bmpResize;
        }

        private void Save(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog
            {
                Filter = "Image Files(*.jpg; .jpeg; .gif; .bmp; .png)|*.jpg; .jpeg; .gif; *.bmp ;*.png",
                FileName = DateTime.Now.ToString("yyyyMMddhhmmss")
            };
            if (save.ShowDialog() == DialogResult.OK)
            {
                bmpWork.Save(save.FileName);
                MessageBox.Show("Success");
            }
        }
    }
}
