using AutoHelpMe.EventBus;
using AutoHelpMe.Function;
using DevExpress.XtraEditors;
using PInvoke;
using SageTools.Extension;
using System.Text;

namespace AutoHelpMe
{
    public partial class FrmMain : XtraForm
    {
        private TaskHelper _taskHelper;

        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            #region 控制状态初始化

            btn_Run.Enabled = false;
            txt_公告.Text = @"本程序仅供交流技术使用，严禁外传，更严禁倒卖！使用中如果遇到问题，请反馈给开发者。使用时务必以管理员身份运行。";

            #endregion 控制状态初始化

            #region 绑定功能框的值

            var functions = new List<FunctionDto>()
            {
                new() { Text = "御魂/业原火/日轮之陨", ActionName = nameof(Functions.御魂单刷) },
                new() { Text = "御灵", ActionName = nameof(Functions.御灵) },
                new() { Text = "探索副本", ActionName = nameof(Functions.探索副本) },
                new() { Text = "结界突破", ActionName = nameof(Functions.个人突破) },
                new() { Text = "斗技-挂机", ActionName = nameof(Functions.斗技挂机) },
                new() { Text = "关闭加成", ActionName = nameof(Functions.关闭加成) },
                new() { Text = "当前爬塔活动(言灵)", ActionName = nameof(Functions.活动) },
                new() { Text = "秘闻", ActionName = nameof(Functions.秘闻) },
                new() { Text = "抽卡-草纸十连", ActionName = nameof(Functions.抽草纸) },
            };
            list_functions.DataSource = functions;
            list_functions.DisplayMember = "Text";
            list_functions.ValueMember = "ActionName";

            list_副本章节.DataSource = new List<FunctionDto>()
            {
                new() { Text = "第二十八章", ActionName = "28" },
                new() { Text = "第二十七章", ActionName = "27" },
                new() { Text = "第二十六章", ActionName = "26" },
                new() { Text = "第二十五章", ActionName = "25" },
            };
            list_副本章节.DisplayMember = "Text";
            list_副本章节.ValueMember = "ActionName";
            list_副本章节.SelectedIndex = 0;

            #endregion 绑定功能框的值

            #region EventBus

            //日志订阅
            EventBusHelper.EventAggregator.GetEvent<PrintLogEvent>().Subscribe(log =>
            {
                Invoke(SetLogTextBoxText, log.Item1, log.Item2);
            });
            //查找窗口订阅
            EventBusHelper.EventAggregator.GetEvent<WindowHandleEvent>().Subscribe(handle =>
            {
                var text = User32.GetWindowText(handle);
                WinHelper.Instance.SetWindowHandle(handle);
                WinHelper.Instance.SetWindowTitle(text);
                btn_Run.Enabled = true;
                Logger.Info($"已选择窗口，名为：{text}，如果选错了，请点锁定重选");
            });

            EventBusHelper.EventAggregator.GetEvent<TaskOperateEvent>().Subscribe(operate =>
            {
                switch (operate)
                {
                    case TaskOperateType.Start:
                        {
                            Invoke(SetBtnRun, "停止", true);
                            Invoke(SetBtnPause, null, true);
                        }
                        break;

                    case TaskOperateType.Stop: //任务结束
                        Invoke(SetBtnRun, "启动", true);
                        Invoke(SetBtnPause, null, false);
                        _taskHelper?.Restore();
                        _taskHelper?.Stop();

                        if (checkBox_关闭加成.Checked && list_functions.SelectedValue.ToString() != nameof(Functions.关闭加成))
                        {
                            Invoke(执行关加成);
                        }

                        if (GlobalConst.FinishReason == "副本结界票满" && GlobalConst.LastTask == nameof(Functions.探索副本))
                        {
                            Invoke(执行个人突破);
                        }
                        if (GlobalConst.FinishReason == "副本结界票满" && GlobalConst.LastTask == nameof(Functions.个人突破))
                        {
                            Invoke(执行个人突破);
                        }
                        break;

                    case TaskOperateType.NoAuth:
                        Invoke(SetBtnRun, "启动", false);
                        Invoke(SetBtnPause, null, false);
                        Invoke(SetBtnLock, false);

                        _taskHelper?.Restore();
                        _taskHelper?.Stop();

                        break;
                }
            });

            #endregion EventBus
        }

        private void btn_启动_Click(object sender, EventArgs e)
        {
            var screen = WinHelper.Instance.CaptureWindow();
            pictureBox1.Image = screen;
            var value = list_functions.SelectedValue.ToString();
            if (value.IsNullOrWhiteSpace())
            {
                Logger.Error("请先选择要执行的功能");
                return;
            }

            if (btn_Run.Text == @"启动")
            {
                btn_Run.Text = @"停止";
                _taskHelper = TaskHelper.NewInstance();
                _taskHelper.TaskName = value!;

                switch (value)
                {
                    case nameof(Functions.个人突破):
                        {
                            if (radio_个突.Checked)
                            {
                                value = nameof(Functions.个人突破);
                            }
                            else if (radio_寮突.Checked)
                            {
                                value = nameof(Functions.寮突破);
                            }
                            else
                            {
                                value = nameof(Functions.道馆);
                            }
                            _taskHelper.TaskName = value!;
                            Functions.Invoke(value!, _taskHelper, num_突破退次数.Value.ToInt32(), checkBox_突破绿标五.Checked, checkBox_突破红标花.Checked);
                        }
                        break;

                    case nameof(Functions.斗技挂机):
                        {
                            Functions.Invoke(value!, _taskHelper, num_挑战次数.Value.ToInt32(), checkBox_斗技挂机死亡退出.Checked);
                        }
                        break;

                    case nameof(Functions.探索副本):
                        {
                            Functions.Invoke(value!, _taskHelper, num_挑战次数.Value.ToInt32(), list_副本章节.SelectedValue.ToInt32(28), radio_副本_普通.Checked);
                        }
                        break;

                    case nameof(Functions.御魂单刷):
                        {
                            if (radio_御魂单刷.Checked)
                            {
                                value = nameof(Functions.御魂单刷);
                            }
                            else if (radio_御魂打手.Checked)
                            {
                                value = nameof(Functions.御魂打手);
                            }
                            else
                            {
                                value = nameof(Functions.御魂司机);
                            }
                            _taskHelper.TaskName = value!;
                            Functions.Invoke(value!, _taskHelper, num_挑战次数.Value.ToInt32());
                        }
                        break;

                    default:
                        {
                            Functions.Invoke(value!, _taskHelper, num_挑战次数.Value.ToInt32());
                        }
                        break;
                }
            }
            else
            {
                btn_Run.Enabled = false;
                btn_暂停.Enabled = false;
                _taskHelper.Restore();
                _taskHelper.Stop();
            }
        }

        private void btn_锁定_Click(object sender, EventArgs e)
        {
            Logger.Info("开始选择窗口，将鼠标放在游戏/模拟器窗口上，然后按下鼠标中键(滚轮)");
            WinHelper.Instance.HookMouseMiddleClick();
        }

        private void btn_暂停_Click(object sender, EventArgs e)
        {
            if (btn_暂停.Text == @"暂停")
            {
                Invoke(SetBtnPause, "恢复", true);
                Logger.Info("已暂停");
                _taskHelper.Pause();
            }
            else
            {
                Invoke(SetBtnPause, "暂停", true);
                Logger.Info("已恢复");
                _taskHelper.Restore();
            }
        }

        private void btn_授权_Click(object sender, EventArgs e)
        {
            const string text = "请自行实现授权机制";
            const string caption = "尚未实现";
            new FrmAuth(caption, text).ShowDialog(this);
        }

        private void btn_更新_Click(object sender, EventArgs e)
        {
            XtraMessageBox.Show(this, "当前已是最新版本(其实这功能还没实现)", "当前版本：1.0");
        }

        private void btn_关于_Click(object sender, EventArgs e)
        {
            new FrmAbout().ShowDialog(this);
        }
    }
}