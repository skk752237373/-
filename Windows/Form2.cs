using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Windows
{
    public partial class Form2 : Form
    {
        private Timer timer1;
        public Form2()
        {
            InitializeComponent();
            //将窗体设置为无边框
            this.FormBorderStyle = FormBorderStyle.None;
            // 创建一个计时器并设置时间间隔
            timer1 = new Timer();
            timer1.Interval = 4000;

            // 绑定计时器事件处理程序
            timer1.Tick += OnTimedEvent;

            // 启动计时器
            timer1.Start();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            //使label标题在背景图片下透明
            label1.BackColor = Color.Transparent;
            label1.Parent = pictureBox1;
        }
        private void OnTimedEvent(object sender, EventArgs e)
        {
            // 停止计时器
            timer1.Stop();

            // 关闭当前窗体
            this.Close();

        }
    }
}
