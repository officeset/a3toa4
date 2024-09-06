using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Xml.Linq;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfSharp.Drawing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace pdf
{
    public partial class Form1 : Form
    {
        public string a3;
        public string a4;
        public Form1()
        {
            InitializeComponent();
        }



        public void pdfs()
        {
            try
            {
                var splitter = new PdfSplitter();


                if (radioButton1.Checked == true)
                {
                    // CheckBox被选中时的处理逻辑
                    splitter.SplitA3ToA4h(a3, a4);
                }
                if (radioButton2.Checked == true)
                {
                    // CheckBox未被选中时的处理逻辑
                    splitter.SplitA3ToA4w(a3, a4);
                }
                MessageBox.Show("分割成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生异常:{ex} ");
            }

        }







        public class PdfSplitter
        {
            public void SplitA3ToA4h(string inputPdfPath, string outputPdfPath)
            {
                // 打开A3 PDF文件
                PdfDocument inputDocument = PdfReader.Open(inputPdfPath, PdfDocumentOpenMode.Import);

                // 创建一个新的文档，用于存放分割后的A4页面
                PdfDocument outputDocument = new PdfDocument();

                foreach (PdfPage page in inputDocument.Pages)
                {
                    // 假设原始页面为A3（297x420mm），即A4的两倍高度
                    double originalWidth = page.Width;
                    double originalHeight = page.Height;

                    // 分成两个A4页面的循环
                    for (int i = 0; i < 2; i++)
                    {
                        PdfPage newPage = outputDocument.AddPage();
                        newPage.Width = originalWidth;
                        newPage.Height = originalHeight / 2; // A4高度

                        using (XGraphics gfx = XGraphics.FromPdfPage(newPage))
                        {
                            // 定义剪裁区域
                            XRect sourceRect = new XRect(0, i * originalHeight / 2, originalWidth, originalHeight / 2);

                            // 使用DrawImage方法手动裁剪和绘制页面内容
                            gfx.DrawImage(XImage.FromFile(inputPdfPath), 0, -i * originalHeight / 2, originalWidth, originalHeight);
                        }
                    }
                }

                // 保存输出文档
                outputDocument.Save(outputPdfPath);
            }





            public void SplitA3ToA4w(string inputPdfPath, string outputPdfPath)
            {
                // 打开A3 PDF文件
                PdfDocument inputDocument = PdfReader.Open(inputPdfPath, PdfDocumentOpenMode.Import);

                // 创建一个新的文档，用于存放分割后的A4页面
                PdfDocument outputDocument = new PdfDocument();

                foreach (PdfPage page in inputDocument.Pages)
                {
                    // 假设原始页面为A3（297x420mm），即A4的两倍高度
                    double originalWidth = page.Width;
                    double originalHeight = page.Height;

                    // 分成两个A4页面的循环
                    for (int i = 0; i < 2; i++)
                    {
                        PdfPage newPage = outputDocument.AddPage();
                        newPage.Width = originalWidth / 2;
                        newPage.Height = originalHeight ; // A4高度

                        using (XGraphics gfx = XGraphics.FromPdfPage(newPage))
                        {
                            XRect sourceRect = new XRect(i * originalWidth / 2, 0, originalWidth / 2, originalHeight);

                            // Manually crop and draw the page content using DrawImage method
                            gfx.DrawImage(XImage.FromFile(inputPdfPath), -i * originalWidth / 2, 0, originalWidth, originalHeight);
                        }
                    }
                }

                // 保存输出文档
                outputDocument.Save(outputPdfPath);
            }


        }




        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            //设置打开对话框的初始目录，默认目录为exe运行文件所在的路径
            //ofd.InitialDirectory = Application.StartupPath;
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            //设置打开对话框的标题
            ofd.Title = "请选择要打开的文件";
            //设置打开对话框可以多选
            ofd.Multiselect = true;
            //设置对话框打开的文件类型
            ofd.Filter = "*.pdf|*.*";
            //设置文件对话框当前选定的筛选器的索引
            ofd.FilterIndex = 2;
            //设置对话框是否记忆之前打开的目录
            ofd.RestoreDirectory = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //获取用户选择的文件完整路径
                string filePath = ofd.FileName;
                //获取对话框中所选文件的文件名和扩展名，文件名不包括路径
                string fileName = ofd.SafeFileName;
                //OutLog("用户选择的文件目录为:" + filePath);
                textBox1.Text = ofd.FileName;
                a3 = textBox1.Text;
                //OutLog("用户选择的文件名称为:" + fileName);
                //OutLog("**************选中文件的内容**************");

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();

            // 设置保存对话框的初始目录，默认目录为exe运行文件所在的路径
            //sfd.InitialDirectory = Application.StartupPath;
            
            // 设置保存对话框的标题
            sfd.Title = "请选择要保存的文件名";

            // 设置保存的文件类型
            sfd.Filter = "*.pdf|*.*";

            // 设置文件对话框当前选定的筛选器的索引
            sfd.FilterIndex = 1;
            sfd.DefaultExt = "pdf";
            sfd.AddExtension = true;
            // 设置对话框是否记忆之前打开的目录
            sfd.RestoreDirectory = true;

            // 弹出保存文件对话框，用户选择确定后返回文件路径
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                // 获取用户选择的文件路径
                string filePath = sfd.FileName;
                textBox2.Text= sfd.FileName;
                // 在此处添加代码以保存文件
                a4 = textBox2.Text;

                // 例如：调用文件保存逻辑
                // SaveFileLogic(filePath);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            pdfs();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
