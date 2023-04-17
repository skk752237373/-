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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();  
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //开始时将密码隐藏
            textpassword.UseSystemPasswordChar = true;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        //登录按钮
        private void btn_log_Click(object sender, EventArgs e)
        {
            //新建登录错误信息窗体
            Form f2 = new Form();
            //窗体标题
            f2.Text = "错误";
            //将新疆的窗体居中显示
            f2.StartPosition = FormStartPosition.CenterScreen;
            Label l1 = new Label();
            //错误信息展示
            l1.Text = "账号密码错误";
            l1.Size = new Size(500, 200);
            //设置label大小和位置
            l1.Location = new Point(70,100);
            //设置l1中字体属性
            l1.Font = new Font("宋体", 14, FontStyle.Bold);

            f2.Controls.Add(l1);

            Form3 f3 = new Form3();
            string s1 = "123456";
            string s2 = "1111";
            //对输入的账号密码信息进行匹配
            if ((textBox1.Text==s1)&&(textpassword.Text==s2))
            {
                //匹配成功，关闭登陆页面，进入主页面
                this.Hide();
                f3.Show();
            }
            else
            {
                f2.Show();//匹配失败，显示错误信息；
                f2.TopMost = true;//将新建的错误信息窗体展示在最顶端
            }
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 如果点击的是关闭按钮，则停止程序
            if (e.CloseReason == CloseReason.UserClosing)
            {
                // 停止程序
                Application.Exit();
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            //点击取消按钮关闭程序
            Application.Exit();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //隐藏密码按钮点击
            if (checkBox1.Checked == true)
            {
                textpassword.UseSystemPasswordChar = true;
            }
            else
            {
                textpassword.UseSystemPasswordChar = false;
            }
        }
    }
}
