using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Threading;

namespace Windows
{
    public partial class Form3 : Form
    {
        private int i = 0;
        private bool isDrawing = false;
        //绘图时记录鼠标的位置
        private Point startPoint, oldPoint;
        //添加字段backColor用于存放背景颜色
        private Color backColor = Color.White;
        //添加枚举型字段
        private enum drawTools
        {
            Pen = 0, Line, Ellipse, Rectangle, Rubber, Hexagon, None
        };//当前使用的工具
        private drawTools drawTool = drawTools.None;
        private Color foreColor = Color.Black;//绘图使用的色彩
        private string editFileName;//当前编辑的文件名
        private Image theImage;//进行操作的位图
        private Graphics ig;//绘制位图的实例
        private System.Threading.Timer timer1;

        public Form3()
        {
            InitializeComponent();
            //创建一个新线程，并且让start在里面执行
            Thread thread = new Thread(new ThreadStart(StartTimer));
            thread.Start();
        }
        private void StartTimer()
        {
            // 创建一个计时器 时间间隔为1秒
            timer1 = new System.Threading.Timer(OnTimedEvent, null, 1000, 1000);
        }
        private void OnTimedEvent(object state)
        {
            i++;
            //将更新UI的操作委托给UI线程执行
            lab_time.Invoke(new Action(() =>
            {
                if(i>=60)
                {
                    int a = i / 60;
                    int b = i % 60;
                    lab_time.Text = "系统时间：" + a.ToString() + "分" + b.ToString() + "秒";
                }
                else
                {
                    lab_time.Text = "系统时间：" + i.ToString() + "秒";
                }
                lab_time.Refresh();
            }));
        }
        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 如果点击的是关闭按钮，则停止程序
            if (e.CloseReason == CloseReason.UserClosing)
            {
                // 停止程序
                Application.Exit();
            }
        }

        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Image Files(*.bmp,*.wmf,*.ico,*.cur,*.jgp)|*.bmp,*.wmf,*.ico,*.cur,*.jgp";
            openFileDialog1.Multiselect = false;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //修改窗口标题
                this.Text = "MyDraw\t" + openFileDialog1.FileName;
                editFileName = openFileDialog1.FileName;
                theImage = Image.FromFile(openFileDialog1.FileName);
                Graphics g = this.CreateGraphics();
                g.DrawImage(theImage, this.ClientRectangle); ig = Graphics.FromImage(theImage);
                ig.DrawImage(theImage, this.ClientRectangle);
                //ToolBar可以使用了
                toolStrip1.Enabled = true;
            }
        }

        private void 新建ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Graphics g = this.CreateGraphics();
            g.Clear(BackColor);
            toolStrip1.Enabled = true;
            //创建一个  Bitmap
            theImage = new Bitmap(this.ClientRectangle.Width, this.ClientRectangle.Height);
            editFileName = "新建文件";
            this.Text = "MyDraw\t" + editFileName;
            ig = Graphics.FromImage(theImage);
            ig.Clear(backColor);
        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "图像(*.bmp)|*.bmp";
            saveFileDialog1.FileName = editFileName;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                theImage.Save(saveFileDialog1.FileName, ImageFormat.Bmp);
                this.Text = "MyDraw\t" + saveFileDialog1.FileName;
                editFileName = saveFileDialog1.FileName;
            }
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void DrawHexagon(Graphics g, Pen p, Point center, int size)
        {
            Point[] points = new Point[6];

            for (int i = 0; i < 6; i++)
            {
                double angle_deg = 60 * i - 30;
                double angle_rad = Math.PI / 180 * angle_deg;
                points[i] = new Point(
                    (int)(center.X + size * Math.Cos(angle_rad)),
                    (int)(center.Y + size * Math.Sin(angle_rad))
                );
            }

            g.DrawPolygon(p, points);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        { 
            drawTool = drawTools.Pen;
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            drawTool = drawTools.Line;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            drawTool = drawTools.Rectangle;
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            drawTool = drawTools.Ellipse;
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            drawTool = drawTools.Rubber;
        }
        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            drawTool = drawTools.Hexagon;
        }
        private void 颜色ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                foreColor = colorDialog1.Color;
            }
        }

        private void Form3_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = this.CreateGraphics();
            if(theImage!=null)
            {
                g.Clear(Color.White);
                g.DrawImage(theImage, this.ClientRectangle);
            }
        }

        private void 帮助ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.ncst.edu.cn/");
        }
        //鼠标抬起事件0

        private void Form3_MouseUp(object sender, MouseEventArgs e)
        {
            isDrawing = false;

            switch (drawTool)
            {
                case drawTools.Line:
                    ig.DrawLine(new Pen(foreColor, 1), startPoint, new Point(e.X, e.Y));
                    break;
                case drawTools.Rectangle:
                    ig.DrawRectangle(new Pen(foreColor, 1), startPoint.X, startPoint.Y, e.X - startPoint.X, e.Y - startPoint.Y);
                    break;
                case drawTools.Ellipse:
                    ig.DrawEllipse(new Pen(foreColor, 1), startPoint.X, startPoint.Y, e.X - startPoint.X, e.Y - startPoint.Y);
                    break;
                case drawTools.Hexagon:
                    int size = Math.Max(Math.Abs(e.X - startPoint.X), Math.Abs(e.Y - startPoint.Y));
                    DrawHexagon(ig, new Pen(foreColor, 1), startPoint, size);
                    break;
            }
        }
        //鼠标左键按下事件
        private void Form3_MouseDown(object sender, MouseEventArgs e)
        {
            isDrawing = true;
            if (e.Button == MouseButtons.Left)//如果是左键按下则做
            {
                startPoint = new Point(e.X, e.Y);
                oldPoint = new Point(e.X, e.Y);
            }
        }

        private void Form3_MouseMove(object sender, MouseEventArgs e)
        {
            Graphics g;
            g = this.CreateGraphics();
            if (isDrawing)
            {
                switch (drawTool)
                {
                    case drawTools.None:
                        break;
                    case drawTools.Pen:
                        //从上一点到当前点绘制线段
                        g.DrawLine(new Pen(foreColor, 1), oldPoint, new Point(e.X, e.Y));
                        ig.DrawLine(new Pen(foreColor, 1), oldPoint, new Point(e.X, e.Y));
                        oldPoint.X = e.X;
                        oldPoint.Y = e.Y;
                        break;
                    case drawTools.Line:
                        //首先恢复此次操作之前的图像，然后再添加Line
                        this.Form3_Paint(this, new PaintEventArgs(this.CreateGraphics(), this.ClientRectangle));
                        g.DrawLine(new Pen(foreColor, 1), startPoint, new Point(e.X, e.Y));
                        break;
                    case drawTools.Rectangle:
                        //首先恢复此次操作之前的图像，然后再添加Rectangle
                        this.Form3_Paint(this, new PaintEventArgs(this.CreateGraphics(), this.ClientRectangle));
                        g.DrawRectangle(new Pen(foreColor, 1), startPoint.X, startPoint.Y, e.X - startPoint.X, e.Y - startPoint.Y);
                        break;
                    case drawTools.Ellipse:
                        //首先恢复此次操作之前的图像，然后再添加Ellipse
                        this.Form3_Paint(this, new PaintEventArgs(this.CreateGraphics(), this.ClientRectangle));
                        g.DrawEllipse(new Pen(foreColor, 1), startPoint.X, startPoint.Y, e.X - startPoint.X, e.Y - startPoint.Y);
                        break;
                    case drawTools.Rubber:
                        //用背景色绘制宽线段
                        g.DrawLine(new Pen(backColor, 20), oldPoint, new Point(e.X, e.Y));
                        ig.DrawLine(new Pen(backColor, 20), oldPoint, new Point(e.X, e.Y));
                        oldPoint.X = e.X;
                        oldPoint.Y = e.Y;
                        break;
                    case drawTools.Hexagon:
                        //首先恢复此次操作之前的图像
                        this.Form3_Paint(this, new PaintEventArgs(this.CreateGraphics(), this.ClientRectangle));
                        int size = Math.Max(Math.Abs(e.X - oldPoint.X), Math.Abs(e.Y - oldPoint.Y));
                        DrawHexagon(ig, new Pen(foreColor, 1), oldPoint, size);
                        break;
                }
            }
        }
    }
}
