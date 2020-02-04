using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
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
            message = ToASCII(message);
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

        private string ToASCII(string message)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(message);

            //lower case
            builder.Replace("à", "a{0}");
            builder.Replace("á", "a{1}");
            builder.Replace("ả", "a{2}");
            builder.Replace("ã", "a{3}");
            builder.Replace("ạ", "a{4}");

            builder.Replace("ă", "a{5}");
            builder.Replace("ằ", "a{6}");
            builder.Replace("ắ", "a{7}");
            builder.Replace("ẳ", "a{8}");
            builder.Replace("ẵ", "a{9}");
            builder.Replace("ặ", "a{10}");

            builder.Replace("â", "a{11}");
            builder.Replace("ầ", "a{12}");
            builder.Replace("ấ", "a{13}");
            builder.Replace("ẩ", "a{14}");
            builder.Replace("ẫ", "a{15}");
            builder.Replace("ậ", "a{16}");

            builder.Replace("đ", "d{0}");

            builder.Replace("è", "e{0}");
            builder.Replace("é", "e{1}");
            builder.Replace("ẻ", "e{2}");
            builder.Replace("ẽ", "e{3}");
            builder.Replace("ẹ", "e{4}");

            builder.Replace("ê", "e{5}");
            builder.Replace("ề", "e{6}");
            builder.Replace("ế", "e{7}");
            builder.Replace("ể", "e{8}");
            builder.Replace("ễ", "e{9}");
            builder.Replace("ệ", "e{10}");

            builder.Replace("ì", "i{0}");
            builder.Replace("í", "i{1}");
            builder.Replace("ỉ", "i{2}");
            builder.Replace("ĩ", "i{3}");
            builder.Replace("ị", "i{4}");

            builder.Replace("ò", "o{0}");
            builder.Replace("ó", "o{1}");
            builder.Replace("ỏ", "o{2}");
            builder.Replace("õ", "o{3}");
            builder.Replace("ọ", "o{4}");

            builder.Replace("ô", "o{5}");
            builder.Replace("ồ", "o{6}");
            builder.Replace("ố", "o{7}");
            builder.Replace("ổ", "o{8}");
            builder.Replace("ỗ", "o{9}");
            builder.Replace("ộ", "o{10}");

            builder.Replace("ơ", "o{11}");
            builder.Replace("ờ", "o{12}");
            builder.Replace("ớ", "o{13}");
            builder.Replace("ở", "o{14}");
            builder.Replace("ỡ", "o{15}");
            builder.Replace("ợ", "o{16}");

            builder.Replace("ù", "u{0}");
            builder.Replace("ú", "u{1}");
            builder.Replace("ủ", "u{2}");
            builder.Replace("ũ", "u{3}");
            builder.Replace("ụ", "u{4}");

            builder.Replace("ư", "u{5}");
            builder.Replace("ừ", "u{6}");
            builder.Replace("ứ", "u{7}");
            builder.Replace("ử", "u{8}");
            builder.Replace("ữ", "u{9}");
            builder.Replace("ự", "u{10}");

            builder.Replace("ỳ", "y{0}");
            builder.Replace("ý", "y{1}");
            builder.Replace("ỷ", "y{2}");
            builder.Replace("ỹ", "y{3}");
            builder.Replace("ỵ", "y{4}");

            // upper case
            builder.Replace("À", "A{0}");
            builder.Replace("Á", "A{1}");
            builder.Replace("Ả", "A{2}");
            builder.Replace("Ã", "A{3}");
            builder.Replace("Ạ", "A{4}");

            builder.Replace("Ă", "A{5}");
            builder.Replace("Ằ", "A{6}");
            builder.Replace("Ắ", "A{7}");
            builder.Replace("Ẳ", "A{8}");
            builder.Replace("Ẵ", "A{9}");
            builder.Replace("Ặ", "A{10}");

            builder.Replace("Â", "A{11}");
            builder.Replace("Ầ", "A{12}");
            builder.Replace("Ấ", "A{13}");
            builder.Replace("Ẩ", "A{14}");
            builder.Replace("Ẫ", "A{15}");
            builder.Replace("Ậ", "A{16}");

            builder.Replace("Đ", "D{0}");

            builder.Replace("È", "E{0}");
            builder.Replace("É", "E{1}");
            builder.Replace("Ẻ", "E{2}");
            builder.Replace("Ẽ", "E{3}");
            builder.Replace("Ẹ", "E{4}");

            builder.Replace("Ê", "E{5}");
            builder.Replace("Ề", "E{6}");
            builder.Replace("Ế", "E{7}");
            builder.Replace("Ể", "E{8}");
            builder.Replace("Ễ", "E{9}");
            builder.Replace("Ệ", "E{10}");

            builder.Replace("Ì", "I{0}");
            builder.Replace("Í", "I{1}");
            builder.Replace("Ỉ", "I{2}");
            builder.Replace("Ĩ", "I{3}");
            builder.Replace("Ị", "I{4}");

            builder.Replace("Ò", "O{0}");
            builder.Replace("Ó", "O{1}");
            builder.Replace("Ỏ", "O{2}");
            builder.Replace("Õ", "O{3}");
            builder.Replace("Ọ", "O{4}");

            builder.Replace("Ô", "O{5}");
            builder.Replace("Ồ", "O{6}");
            builder.Replace("Ố", "O{7}");
            builder.Replace("Ổ", "O{8}");
            builder.Replace("Ỗ", "O{9}");
            builder.Replace("Ộ", "O{10}");

            builder.Replace("Ơ", "O{11}");
            builder.Replace("Ờ", "O{12}");
            builder.Replace("Ớ", "O{13}");
            builder.Replace("Ở", "O{14}");
            builder.Replace("Ỡ", "O{15}");
            builder.Replace("Ợ", "O{16}");

            builder.Replace("Ù", "U{0}");
            builder.Replace("Ú", "U{1}");
            builder.Replace("Ủ", "U{2}");
            builder.Replace("Ũ", "U{3}");
            builder.Replace("Ụ", "U{4}");

            builder.Replace("Ư", "U{5}");
            builder.Replace("Ừ", "U{6}");
            builder.Replace("Ứ", "U{7}");
            builder.Replace("Ử", "U{8}");
            builder.Replace("Ữ", "U{9}");
            builder.Replace("Ự", "U{10}");

            builder.Replace("Ỳ", "Y{0}");
            builder.Replace("Ý", "Y{1}");
            builder.Replace("Ỷ", "Y{2}");
            builder.Replace("Ỹ", "Y{3}");
            builder.Replace("Ỵ", "Y{4}");

            string result = Regex.Replace(builder.ToString(), @"[^\u0000-\u007F]+", " [?] ");
            return result;
        }
    }
}
