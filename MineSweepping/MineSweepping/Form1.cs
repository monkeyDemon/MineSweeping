using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MineSweepping
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.comboBox1.Items.Add("简单");
            this.comboBox1.Items.Add("中等");
            this.comboBox1.Items.Add("困难");
        }



        /// <summary>
        /// 点击开始按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {                                                       
            //判断mineField控件中有无Pane控件
            if (this.mineField1.Controls.Count == 0) //雷区控件中无Pane控件（初次初始化）
            {
                if (this.comboBox1.Text == "简单")
                {
                    this.mineField1.Init(10, 8);//地雷的比例0.08
                }
                else if (this.comboBox1.Text == "中等")
                {
                    this.mineField1.Init(14, 24);//地雷的比例约为0.12
                }
                else
                {
                    this.mineField1.Init(16, 51);//地雷的比例约为0.2
                }
            }
            else                                                
            {
                //雷区控件中有Pane控件（游戏过程中重新初始化）
                while (this.mineField1.Controls.Count != 0)
                {
                    this.mineField1.Controls.Remove(this.mineField1.Controls[0]);
                }
                
                if (this.comboBox1.Text == "简单")
                {
                    this.mineField1.Init(10, 8);
                }
                else if (this.comboBox1.Text == "中等")
                {
                    this.mineField1.Init(14, 24);
                }
                else
                {
                    this.mineField1.Init(16, 51);
                }
            }
        }

        /// <summary>
        /// 点击结束按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            this.mineField1.DisplayAll();
        }
    }
}
