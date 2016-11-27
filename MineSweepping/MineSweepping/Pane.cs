using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MineSweepping
{
    public class Pane : Button
    {
        public Pane()
        {
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }

        public bool HasMine { get; set; }
        public int AroundMineCount { get; set; }
        public PaneState State { get; set; }

        /// <summary>
        /// 打开一个小方块
        /// </summary>
        public void Open()
        {
            if (this.HasMine)
            {
                //有地雷
                this.BackgroundImage = Properties.Resources.Mine;
                this.Enabled = false;
            }
            else
            {
                switch (this.AroundMineCount)
                {
                    case 0:
                        this.BackgroundImage = null;
                        this.Enabled = false;
                        this.State = PaneState.Opened;
                        break;
                    case 1:
                        this.BackgroundImage = Properties.Resources._01;
                        this.Enabled = false;
                        this.State = PaneState.Opened;
                        break;
                    case 2:
                        this.BackgroundImage = Properties.Resources._02;
                        this.Enabled = false;
                        this.State = PaneState.Opened;
                        break;
                    case 3:
                        this.BackgroundImage = Properties.Resources._03;
                        this.Enabled = false;
                        this.State = PaneState.Opened;
                        break;
                    case 4:
                        this.BackgroundImage = Properties.Resources._04;
                        this.Enabled = false;
                        this.State = PaneState.Opened;
                        break;
                    case 5:
                        this.BackgroundImage = Properties.Resources._05;
                        this.Enabled = false;
                        this.State = PaneState.Opened;
                        break;
                    case 6:
                        this.BackgroundImage = Properties.Resources._06;
                        this.Enabled = false;
                        this.State = PaneState.Opened;
                        break;
                    case 7:
                        this.BackgroundImage = Properties.Resources._07;
                        this.Enabled = false;
                        this.State = PaneState.Opened;
                        break;
                    case 8:
                        this.BackgroundImage = Properties.Resources._08;
                        this.Enabled = false;
                        this.State = PaneState.Opened;
                        break;
                }
            }
          
        }

        public void Mark()
        {
            this.BackgroundImage = Properties.Resources.Marked;
            this.State = PaneState.Marked;
        }

        public void Reset()
        {
            this.BackgroundImage = null;
            this.State = PaneState.Closed;
        }
    }
    
    public enum PaneState
    {
        Closed,         //关闭状态
        Opened,         //打开状态
        Marked,         //标记状态
    }
}
