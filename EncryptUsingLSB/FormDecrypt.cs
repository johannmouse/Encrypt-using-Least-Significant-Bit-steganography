using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace EncryptUsingLSB
{
    public partial class FormEncrypt : Form
    {
        Bitmap bmp;
        Bitmap bmpWork;
        Bitmap bmpResize;
        const string END_MARK = "vinhy9x";
        string R_text = string.Empty;
        string G_text = string.Empty;
        string B_text = string.Empty;
        enum Page { R, G, B };
        Page currentPage = Page.R;
        public FormEncrypt()
        {
            InitializeComponent();
            Bitmap bmpIcon = Base64StringToBitmap("/9j/4AAQSkZJRgABAQEAYABgAAD/4QEFRXhpZgAATU0AKgAAAAgACwEAAAMAAAABAMgAAAEBAAMAAAABAMgAAAECAAMAAAADAAAAkgEGAAMAAAABAAIAAAEVAAMAAAABAAMAAAEaAAUAAAABAAAAmAEbAAUAAAABAAAAoAEoAAMAAAABAAIAAAExAAIAAAALAAAAqAEyAAIAAAAUAAAAs4dpAAQAAAABAAAAxwAAAAAACAAIAAgAEk+AAAAnEAAST4AAACcQUGhvdG9TY2FwZQAyMDE4OjExOjIzIDE1OjQ2OjE1AAAEkAAABwAAAAQwMjIxoAEAAwAAAAH//wAAoAIABAAAAAEAAAOEoAMABAAAAAEAAAOEAAAAAP/hDk1odHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+Cjx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IlhNUCBDb3JlIDQuMS4xLUV4aXYyIj4KIDxyZGY6UkRGIHhtbG5zOnJkZj0iaHR0cDovL3d3dy53My5vcmcvMTk5OS8wMi8yMi1yZGYtc3ludGF4LW5zIyI+CiAgPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIKICAgIHhtbG5zOnhhcE1NPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvbW0vIgogICAgeG1sbnM6c3RFdnQ9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZUV2ZW50IyIKICAgIHhtbG5zOmRjPSJodHRwOi8vcHVybC5vcmcvZGMvZWxlbWVudHMvMS4xLyIKICAgIHhtbG5zOnBob3Rvc2hvcD0iaHR0cDovL25zLmFkb2JlLmNvbS9waG90b3Nob3AvMS4wLyIKICAgIHhtbG5zOnhhcD0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLyIKICAgeGFwTU06RG9jdW1lbnRJRD0iRTg3QkEwMTRDQzY1RENDQTg3NzA1MzQyMEQ4QkUwQTciCiAgIHhhcE1NOkluc3RhbmNlSUQ9InhtcC5paWQ6NzBDODFEM0RGQ0VFRTgxMTlCRjM4MkI1NDE1MjkwQTEiCiAgIHhhcE1NOk9yaWdpbmFsRG9jdW1lbnRJRD0iRTg3QkEwMTRDQzY1RENDQTg3NzA1MzQyMEQ4QkUwQTciCiAgIGRjOmZvcm1hdD0iaW1hZ2UvanBlZyIKICAgcGhvdG9zaG9wOkxlZ2FjeUlQVENEaWdlc3Q9IjJEQzc3MkY3MTkwRkI3REI1QjY4QTlENEVGRUY5MjA2IgogICBwaG90b3Nob3A6Q29sb3JNb2RlPSIzIgogICB4YXA6Q3JlYXRlRGF0ZT0iMjAxOC0xMS0yM1QxNTozNjoyOCswNzowMCIKICAgeGFwOk1vZGlmeURhdGU9IjIwMTgtMTEtMjNUMTU6NDY6MTUrMDc6MDAiCiAgIHhhcDpNZXRhZGF0YURhdGU9IjIwMTgtMTEtMjNUMTU6NDY6MTUrMDc6MDAiCiAgIHhhcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENTNiAoV2luZG93cykiPgogICA8eGFwTU06SGlzdG9yeT4KICAgIDxyZGY6U2VxPgogICAgIDxyZGY6bGkKICAgICAgc3RFdnQ6YWN0aW9uPSJzYXZlZCIKICAgICAgc3RFdnQ6aW5zdGFuY2VJRD0ieG1wLmlpZDo0NDJFNEQyMkZDRUVFODExOTg5RkQ3MjYzNEEyMTMwOSIKICAgICAgc3RFdnQ6d2hlbj0iMjAxOC0xMS0yM1QxNTo0NTozMCswNzowMCIKICAgICAgc3RFdnQ6c29mdHdhcmVBZ2VudD0iQWRvYmUgUGhvdG9zaG9wIENTNiAoV2luZG93cykiCiAgICAgIHN0RXZ0OmNoYW5nZWQ9Ii8iLz4KICAgICA8cmRmOmxpCiAgICAgIHN0RXZ0OmFjdGlvbj0ic2F2ZWQiCiAgICAgIHN0RXZ0Omluc3RhbmNlSUQ9InhtcC5paWQ6NzBDODFEM0RGQ0VFRTgxMTlCRjM4MkI1NDE1MjkwQTEiCiAgICAgIHN0RXZ0OndoZW49IjIwMTgtMTEtMjNUMTU6NDY6MTUrMDc6MDAiCiAgICAgIHN0RXZ0OnNvZnR3YXJlQWdlbnQ9IkFkb2JlIFBob3Rvc2hvcCBDUzYgKFdpbmRvd3MpIgogICAgICBzdEV2dDpjaGFuZ2VkPSIvIi8+CiAgICA8L3JkZjpTZXE+CiAgIDwveGFwTU06SGlzdG9yeT4KICA8L3JkZjpEZXNjcmlwdGlvbj4KIDwvcmRmOlJERj4KPC94OnhtcG1ldGE+CiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAKPD94cGFja2V0IGVuZD0idyI/Pv/tACxQaG90b3Nob3AgMy4wADhCSU0EBAAAAAAADxwBWgADGyVHHAIAAAIAAQD/2wBDAAEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQH/2wBDAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQH/wAARCAAyADIDASIAAhEBAxEB/8QAHwAAAQUBAQEBAQEAAAAAAAAAAAECAwQFBgcICQoL/8QAtRAAAgEDAwIEAwUFBAQAAAF9AQIDAAQRBRIhMUEGE1FhByJxFDKBkaEII0KxwRVS0fAkM2JyggkKFhcYGRolJicoKSo0NTY3ODk6Q0RFRkdISUpTVFVWV1hZWmNkZWZnaGlqc3R1dnd4eXqDhIWGh4iJipKTlJWWl5iZmqKjpKWmp6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uHi4+Tl5ufo6erx8vP09fb3+Pn6/8QAHwEAAwEBAQEBAQEBAQAAAAAAAAECAwQFBgcICQoL/8QAtREAAgECBAQDBAcFBAQAAQJ3AAECAxEEBSExBhJBUQdhcRMiMoEIFEKRobHBCSMzUvAVYnLRChYkNOEl8RcYGRomJygpKjU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hp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");
            Icon = Icon.FromHandle(bmpIcon.GetHicon());
            Bitmap bmpBrowse = Base64StringToBitmap("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");
            pictureBoxInput.Image = bmpBrowse;
            buttonR.BackColor = Color.LightGray;
        }

        private void Generate(object sender, EventArgs e)
        {
            if (bmp == null)
            {
                MessageBox.Show("Please choose image to hide messages into");
                return;
            }
            if (currentPage == Page.R) R_text = richTextBoxMessage.Text;
            else if (currentPage == Page.G) G_text = richTextBoxMessage.Text;
            else if (currentPage == Page.B) B_text = richTextBoxMessage.Text;
            if (string.IsNullOrEmpty(R_text) && string.IsNullOrEmpty(G_text) && string.IsNullOrEmpty(B_text))
            {
                MessageBox.Show("Choose at least 1 message to hide.");
                return;
            }
            if (!string.IsNullOrEmpty(R_text)) R_text = ToASCII(R_text);
            if (!string.IsNullOrEmpty(G_text)) G_text = ToASCII(G_text);
            if (!string.IsNullOrEmpty(B_text)) B_text = ToASCII(B_text);
            Encrypt(R_text, G_text, B_text);
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

        private void Encrypt(string R_text, string G_text, string B_text)
        {
            string binary_R = StringToBinary(R_text);
            int binaryLengh_R = binary_R.Length;
            string numberOfDigit_R = GetNumberOfDigit(binaryLengh_R);
            string head_R = StringToBinary(binaryLengh_R.ToString());
            binary_R = numberOfDigit_R + head_R + binary_R;

            string binary_G = StringToBinary(G_text);
            int binaryLengh_G = binary_G.Length;
            string numberOfDigit_G = GetNumberOfDigit(binaryLengh_G);
            string head_G = StringToBinary(binaryLengh_G.ToString());
            binary_G = numberOfDigit_G + head_G + binary_G;

            string binary_B = StringToBinary(B_text);
            int binaryLengh_B = binary_B.Length;
            string numberOfDigit_B = GetNumberOfDigit(binaryLengh_B);
            string head_B = StringToBinary(binaryLengh_B.ToString());
            binary_B = numberOfDigit_B + head_B + binary_B;

            int longestMessage = MaxNum(binary_R.Length, binary_G.Length, binary_B.Length);
            if (longestMessage > bmp.Width * bmp.Height)
            {
                MessageBox.Show("The image is not big enough to hide the message. Choose another image with the size so that with x length is at least " + longestMessage);
                return;
            }
            bmpWork = bmp;

            //R
            if (!string.IsNullOrEmpty(R_text))
            {
                binary_R += StringToBinary(END_MARK);
                int index_R = 0;
                int row_R = 0, column_R = 0;
                StringBuilder R_code = new StringBuilder();
                while (index_R != binary_R.Length)
                {
                    Color color = bmpWork.GetPixel(column_R, row_R);
                    R_code.Clear();
                    R_code.Append(Convert.ToString(color.R, 2).PadLeft(8, '0'));
                    R_code.Remove(7, 1).Append(binary_R[index_R]);
                    bmpWork.SetPixel(column_R, row_R, Color.FromArgb(Convert.ToInt32(R_code.ToString(), 2), color.G, color.B));
                    index_R++;
                    column_R++;
                    if (column_R == bmpWork.Width)
                    {
                        column_R = 0;
                        row_R++;
                    }
                }
            }

            //G
            if (!string.IsNullOrEmpty(G_text))
            {
                binary_G += StringToBinary(END_MARK);
                int index_G = 0;
                int row_G = 0, column_G = 0;
                StringBuilder G_code = new StringBuilder();
                while (index_G != binary_G.Length)
                {
                    Color color = bmpWork.GetPixel(column_G, row_G);
                    G_code.Clear();
                    G_code.Append(Convert.ToString(color.G, 2).PadLeft(8, '0'));
                    G_code.Remove(7, 1).Append(binary_G[index_G]);
                    bmpWork.SetPixel(column_G, row_G, Color.FromArgb(color.R, Convert.ToInt32(G_code.ToString(), 2), color.B));
                    index_G++;
                    column_G++;
                    if (column_G == bmpWork.Width)
                    {
                        column_G = 0;
                        row_G++;
                    }
                }
            }

            //B
            if (!string.IsNullOrEmpty(B_text))
            {
                binary_B += StringToBinary(END_MARK);
                int index_B = 0;
                int row_B = 0, column_B = 0;
                StringBuilder B_code = new StringBuilder();
                while (index_B != binary_B.Length)
                {
                    Color color = bmpWork.GetPixel(column_B, row_B);
                    B_code.Clear();
                    B_code.Append(Convert.ToString(color.B, 2).PadLeft(8, '0'));
                    B_code.Remove(7, 1).Append(binary_B[index_B]);
                    bmpWork.SetPixel(column_B, row_B, Color.FromArgb(color.R, color.G, Convert.ToInt32(B_code.ToString(), 2)));
                    index_B++;
                    column_B++;
                    if (column_B == bmpWork.Width)
                    {
                        column_B = 0;
                        row_B++;
                    }
                }
            }
            if (bmpWork.Width <= bmpWork.Height && bmpWork.Height >= 300)
            {
                bmpResize = new Bitmap(bmpWork, (int)(bmpWork.Width / ((float)bmpWork.Height / 300)), 300);
            }
            else if (bmpWork.Width > bmpWork.Height && bmpWork.Width >= 300)
            {
                bmpResize = new Bitmap(bmpWork, 300, (int)(bmpWork.Height / ((float)bmpWork.Width / 300)));
            }
            else
            {
                bmpResize = bmp;
            }
            pictureBoxOutput.Image = bmpResize;
        }
        private string GetNumberOfDigit(int binaryLengh)
        {
            if (binaryLengh >= 10 && binaryLengh < 100) return StringToBinary("2");
            else if (binaryLengh >= 100 && binaryLengh < 1000) return StringToBinary("3");
            else if (binaryLengh >= 1000 && binaryLengh < 10000) return StringToBinary("4");
            else if (binaryLengh >= 10000 && binaryLengh < 100000) return StringToBinary("5");
            else if (binaryLengh >= 100000 && binaryLengh < 1000000) return StringToBinary("6");
            else if (binaryLengh >= 1000000 && binaryLengh < 10000000) return StringToBinary("7");
            else if (binaryLengh >= 10000000 && binaryLengh < 100000000) return StringToBinary("8");
            else if (binaryLengh >= 100000000 && binaryLengh < 1000000000) return StringToBinary("9");
            else return StringToBinary("1");
        }
        private int MaxNum(int num1, int num2, int num3)
        {
            int max = num1;
            if (num2 > max) max = num2;
            if (num3 > max) max = num3;
            return max;
        }
        private void Save(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog
            {
                Filter = "Image Files(*.jpg; .jpeg; .gif; .bmp; .png)|*.jpg; .jpeg; .gif; *.bmp ;*.png",
                FileName = DateTime.Now.ToString("yyyyMMddhhmmss"),
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
            List<string> Vietnamese = new List<string>() { "à", "á", "ả", "ã", "ạ", "ă", "ằ", "ắ", "ẳ", "ẵ", "ặ", "â", "ầ", "ấ", "ẩ", "ẫ", "ậ", "đ", "è", "é", "ẻ", "ẽ", "ẹ", "ê", "ề", "ế", "ể", "ễ", "ệ", "ì", "í", "ỉ", "ĩ", "ị", "ò", "ó", "ỏ", "õ", "ọ", "ô", "ồ", "ố", "ổ", "ỗ", "ộ", "ơ", "ờ", "ớ", "ở", "ỡ", "ợ", "ù", "ú", "ủ", "ũ", "ụ", "ư", "ừ", "ứ", "ử", "ữ", "ự", "ỳ", "ý", "ỷ", "ỹ", "ỵ", "À", "Á", "Ả", "Ã", "Ạ", "Ă", "Ằ", "Ắ", "Ẳ", "Ẵ", "Ặ", "Â", "Ầ", "Ấ", "Ẩ", "Ẫ", "Ậ", "Đ", "È", "É", "Ẻ", "Ẽ", "Ẹ", "Ê", "Ề", "Ế", "Ể", "Ễ", "Ệ", "Ì", "Í", "Ỉ", "Ĩ", "Ị", "Ò", "Ó", "Ỏ", "Õ", "Ọ", "Ô", "Ồ", "Ố", "Ổ", "Ỗ", "Ộ", "Ơ", "Ờ", "Ớ", "Ở", "Ỡ", "Ợ", "Ù", "Ú", "Ủ", "Ũ", "Ụ", "Ư", "Ừ", "Ứ", "Ử", "Ữ", "Ự", "Ỳ", "Ý", "Ỷ", "Ỹ", "Ỵ" };
            List<string> ASCII = new List<string>() { "a{0}", "a{1}", "a{2}", "a{3}", "a{4}", "a{5}", "a{6}", "a{7}", "a{8}", "a{9}", "a{10}", "a{11}", "a{12}", "a{13}", "a{14}", "a{15}", "a{16}", "d{0}", "e{0}", "e{1}", "e{2}", "e{3}", "e{4}", "e{5}", "e{6}", "e{7}", "e{8}", "e{9}", "e{10}", "i{0}", "i{1}", "i{2}", "i{3}", "i{4}", "o{0}", "o{1}", "o{2}", "o{3}", "o{4}", "o{5}", "o{6}", "o{7}", "o{8}", "o{9}", "o{10}", "o{11}", "o{12}", "o{13}", "o{14}", "o{15}", "o{16}", "u{0}", "u{1}", "u{2}", "u{3}", "u{4}", "u{5}", "u{6}", "u{7}", "u{8}", "u{9}", "u{10}", "y{0}", "y{1}", "y{2}", "y{3}", "y{4}", "A{0}", "A{1}", "A{2}", "A{3}", "A{4}", "A{5}", "A{6}", "A{7}", "A{8}", "A{9}", "A{10}", "A{11}", "A{12}", "A{13}", "A{14}", "A{15}", "A{16}", "D{0}", "E{0}", "E{1}", "E{2}", "E{3}", "E{4}", "E{5}", "E{6}", "E{7}", "E{8}", "E{9}", "E{10}", "I{0}", "I{1}", "I{2}", "I{3}", "I{4}", "O{0}", "O{1}", "O{2}", "O{3}", "O{4}", "O{5}", "O{6}", "O{7}", "O{8}", "O{9}", "O{10}", "O{11}", "O{12}", "O{13}", "O{14}", "O{15}", "O{16}", "U{0}", "U{1}", "U{2}", "U{3}", "U{4}", "U{5}", "U{6}", "U{7}", "U{8}", "U{9}", "U{10}", "Y{0}", "Y{1}", "Y{2}", "Y{3}", "Y{4}" };
            for (int index = 0; index < Vietnamese.Count; index++)
            {
                builder.Replace(Vietnamese[index], ASCII[index]);
            }
            string result = Regex.Replace(builder.ToString(), @"[^\u0000-\u007F]+", " [?] ");
            return result;
        }

        private static Bitmap Base64StringToBitmap(string base64String)
        {
            byte[] byteBuffer = Convert.FromBase64String(base64String);
            MemoryStream memoryStream = new MemoryStream(byteBuffer)
            {
                Position = 0
            };
            Bitmap bmpReturn = (Bitmap)Image.FromStream(memoryStream);
            memoryStream.Close();
            return bmpReturn;
        }

        private void View_R(object sender, EventArgs e)
        {
            if (currentPage != Page.R)
            {
                if (currentPage == Page.G) G_text = richTextBoxMessage.Text;
                else if (currentPage == Page.B) B_text = richTextBoxMessage.Text;
                richTextBoxMessage.Text = R_text;
                buttonR.BackColor = Color.LightGray;
                buttonG.BackColor = Color.White;
                buttonB.BackColor = Color.White;
                currentPage = Page.R;
            }
        }

        private void View_G(object sender, EventArgs e)
        {
            if (currentPage != Page.G)
            {
                if (currentPage == Page.R) R_text = richTextBoxMessage.Text;
                else if (currentPage == Page.B) B_text = richTextBoxMessage.Text;
                richTextBoxMessage.Text = G_text;
                buttonR.BackColor = Color.White;
                buttonG.BackColor = Color.LightGray;
                buttonB.BackColor = Color.White;
                currentPage = Page.G;
            }
        }

        private void View_B(object sender, EventArgs e)
        {
            if (currentPage != Page.B)
            {
                if (currentPage == Page.R) R_text = richTextBoxMessage.Text;
                else if (currentPage == Page.G) G_text = richTextBoxMessage.Text;
                richTextBoxMessage.Text = B_text;
                buttonR.BackColor = Color.White;
                buttonG.BackColor = Color.White;
                buttonB.BackColor = Color.LightGray;
                currentPage = Page.B;
            }
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
                if (bmp.Width <= bmp.Height && bmp.Height >= 300)
                {
                    bmpResize = new Bitmap(bmp, (int)(bmp.Width / ((float)bmp.Height / 300)), 300);
                }
                else if (bmp.Width > bmp.Height && bmp.Width >= 300)
                {
                    bmpResize = new Bitmap(bmp, 300, (int)(bmp.Height / ((float)bmp.Width / 300)));
                }
                else
                {
                    bmpResize = bmp;
                }
                pictureBoxInput.Image = bmpResize;
            }
        }

        private void Direct(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.facebook.com/thonchienvodich");
        }
    }
}
