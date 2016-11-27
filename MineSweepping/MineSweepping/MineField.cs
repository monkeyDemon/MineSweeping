using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MineSweepping
{
    /// <summary>
    /// 地雷面板
    /// </summary>
    public partial class MineField : UserControl
    {
        public delegate void MineSweepSuccessEventHandler(object sender, EventArgs e);
        public event MineSweepSuccessEventHandler MineSweepSuccess;

        public delegate void MineSweepFailedEventHandler(object sender, EventArgs e);
        public event MineSweepFailedEventHandler MineSweepFailed;

        

        public MineField()
        {
            InitializeComponent();
            EventArgs keyEventArgs = new EventArgs();
            MineSweepFailed += new MineSweepFailedEventHandler(this.showFailed);
            MineSweepSuccess += new MineSweepSuccessEventHandler(this.showSuccess);
        }

        /// <summary>
        /// 初始化整个雷区
        /// </summary>
        /// <param name="LinePaneNum">每行或每列中的方格数量</param>
        /// <param name="mineCount">地雷的总数</param>
        public void Init(int LinePaneNum, int mineCount)
        {
            //根据指定数量初始化方格
            for (int i = 0; i < LinePaneNum*LinePaneNum; i++)
            {
                Pane pane = new Pane();
                pane.MouseDown +=new MouseEventHandler(pane_MouseDown);
                
                this.Controls.Add(pane);
            }
            //布局多有方格的位置
            this.LayoutPanes();
            //随机布雷
            this.LayMines(mineCount);
            //设置每个方格周围的地雷数
            foreach (Pane pane in this.Controls)
            {
                pane.AroundMineCount = this.GetAroundMineCount(pane);
            }
        }


        private void pane_MouseDown(object sender, MouseEventArgs e)
        {
            //检验是否将所有方格排查干净
            int sum = 0;
            int sumMark = 0;
            int MineNumber = 0;
            foreach (Pane p in this.Controls)
            {
                if (p.State == PaneState.Opened)
                {
                    sum++;
                }
                else if (p.State == PaneState.Marked)
                {
                    sum++;
                    sumMark++;
                }
                if (p.HasMine == true)
                {
                    MineNumber++;
                }
            }

            Pane pane = sender as Pane;
            if (e.Button == MouseButtons.Left)          
            {
                //点击鼠标左键
                if (pane.HasMine)
                {
                    this.DisplayAll();
                    MineSweepFailed(new object(), new EventArgs());
                }
                else
                {
                    if (sum == this.Controls.Count - 1 && sumMark == MineNumber)
                    {
                        MineSweepSuccess(new object(), new EventArgs());
                    }
                    else
                    {
                        this.DisplayAround(pane);
                    }
                }
            }
            else                                                                    
            {
                //点击鼠标右键  
                if (pane.State == PaneState.Marked)
                {
                    //取消标记
                    pane.Reset();
                }
                else
                {

                    if (sum == this.Controls.Count - 1 && sumMark == MineNumber - 1)
                    {
                        MineSweepSuccess(new object(), new EventArgs());
                    }
                    else
                    {
                        pane.Mark();
                    }
                }
            }
        }

        
        
        /// <summary>
        /// 显示雷区中所有方格的内容
        /// </summary>
        public void DisplayAll()
        {
            foreach (Pane pane in this.Controls)
            {
                if (pane.State != PaneState.Opened)
                {
                    pane.Open();
                }
            }
        }


        /// <summary>
        /// 显示当前方格周围的无雷区
        /// </summary>
        /// <param name="currentPane">当前点击的方格</param>
        public void DisplayAround(Pane currentPane)
        {
            if (currentPane.State == PaneState.Opened)
            {
                return;
            }
            else
            {
                currentPane.Open();
                RepeatPanes.Add(currentPane);
                List<Pane> panes = this.GetAroundPanes(currentPane, RepeatPanes);
                foreach (Pane p in panes)
                {
                    if (!p.HasMine&&this.GetAroundMineCount(p) == 0)
                    {
                        DisplayAround(p);
                    }
                    else
                    {
                       
                        if (p.State != PaneState.Opened &&!p.HasMine)
                        {
                            p.Open();
                        }
                    }
                }
            }
            int sum=0;
            foreach (Pane p in this.Controls)
            {
                if (p.State == PaneState.Opened || p.State == PaneState.Marked)
                {
                    sum++;
                }
            }
            if (sum == this.Controls.Count)
            {
                Form myform = new Form3();
                myform.ShowDialog();
            }
        }



        #region 获取周围地雷数

        private int GetAroundMineCount(Pane pane)
        {
            int mineCount = 0;
            List<Pane> panes = this.GetAroundPanes(pane);
            foreach (Pane p in panes)
            {
                if (p.HasMine)
                {
                    mineCount++;
                }
            }
            return mineCount;
        }

        private List<Pane> RepeatPanes = new List<Pane>();
        private List<Pane> GetAroundPanes(Pane pane)
        {
            List<Pane> result = new List<Pane>();
            int paneWidth = pane.Width;
            int paneHeight = pane.Height;
            foreach (Pane p in this.Controls)
            {
                if (p.Top == pane.Top && Math.Abs(pane.Left - p.Left) == paneWidth
                    || p.Left == pane.Left && Math.Abs(pane.Top - p.Top) == paneHeight
                    || Math.Abs(pane.Top - p.Top) == paneHeight && Math.Abs(pane.Left - p.Left) == paneWidth)
                {
                    result.Add(p);
                }
            }
            return result;
        }
        private List<Pane> GetAroundPanes(Pane pane, List<Pane> RepeatPanes)
        {
            List<Pane> result = new List<Pane>();
            int paneWidth = pane.Width;
            int paneHeight = pane.Height;
            foreach (Pane p in this.Controls)
            {
                if (p.Top == pane.Top && Math.Abs(p.Left - pane.Left) == paneWidth
                    || p.Left == pane.Left && Math.Abs(p.Top - pane.Top) == paneHeight
                    || Math.Abs(p.Left - pane.Left) == paneWidth && Math.Abs(p.Top - pane.Top) == paneHeight)
                {
                    if (RepeatPanes.Contains(p) == false)
                    {
                        result.Add(p);
                    }
                }
            }
            return result;
        }

        #endregion


        #region 摆放小方格
        private void LayoutPanes()
        {
            if (this.Controls.Count == 0)
            {
                return;
            }

            int LinePaneNum = (int)Math.Sqrt(this.Controls.Count);
            int paneWidth = this.Width / LinePaneNum;
            int paneHeight = this.Height / LinePaneNum;
            int paneIndex = 0;
            int paneTop = 0;
            int paneLeft = 0;

            for (int colIndex = 0; colIndex < LinePaneNum; colIndex++)
            {
                paneTop = colIndex * paneHeight;
                for (int rowIndex = 0; rowIndex < LinePaneNum; rowIndex++)
                {
                    paneLeft = rowIndex * paneWidth;

                    Pane pane = this.Controls[paneIndex] as Pane;
                    //设置方格的大小
                    pane.Size = new Size(paneWidth, paneHeight);
                    //设置方格的位置
                    pane.Location = new Point(paneLeft, paneTop);
                    //递增
                    paneIndex++;
                }
            }
        }

        private void MineField_SizeChanged(object sender, EventArgs e)
        {
            this.LayoutPanes();
        }

        #endregion


        //随机布置地雷
        private void LayMines(int mineCount)
        {
            Random ran = new Random();
            for (int i = 0; i < mineCount; i++)
            {
                int index = ran.Next(0, this.Controls.Count);
                Pane pane = (Pane)this.Controls[index];
                pane.HasMine = true;
            }
        }


        #region 显示胜利或失败
        private void showFailed(object sender, EventArgs e)
        {
            Form myform = new Form2();
            myform.ShowDialog();
        }

        private void showSuccess(object sender, EventArgs e)
        {
            Form myform = new Form3();
            myform.ShowDialog();
        }
        #endregion
    }
}
