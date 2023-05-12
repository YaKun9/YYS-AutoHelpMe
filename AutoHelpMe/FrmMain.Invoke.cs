using AutoHelpMe.Function;
using SageTools.Extension;

namespace AutoHelpMe;

public partial class FrmMain
{
    /// <summary>
    /// 设置日志控件值
    /// </summary>
    private void SetLogTextBoxText(string text, Color color)
    {
        log_richTextBox.SelectionStart = log_richTextBox.TextLength;
        log_richTextBox.SelectionLength = 0;
        log_richTextBox.SelectionColor = color;
        log_richTextBox.AppendText(text);
        log_richTextBox.SelectionColor = log_richTextBox.ForeColor;
        log_richTextBox.SelectionStart = log_richTextBox.TextLength;
        log_richTextBox.ScrollToCaret();
    }

    private void SetBtnRun(string text, bool? enable = null)
    {
        if (text.IsNotNullOrWhiteSpace())
        {
            if (text == "启动")
            {
                btn_Run.ImageOptions.SvgImage = img_启动.ImageOptions.SvgImage;
                btn_Run.ToolTip = @"启动左侧选择的任务";
            }
            else
            {
                btn_Run.ImageOptions.SvgImage = img_停止.ImageOptions.SvgImage;
                btn_Run.ToolTip = @"终止当前任务";
            }

            btn_Run.Text = text;
        }

        if (enable != null)
        {
            btn_Run.Enabled = enable.Value;
        }
    }

    private void SetBtnPause(string text, bool? enable = null)
    {
        if (text.IsNotNullOrWhiteSpace())
        {
            if (text == "暂停")
            {
                btn_暂停.ImageOptions.SvgImage = img_暂停.ImageOptions.SvgImage;
                btn_暂停.ToolTip = @"点击暂停，但是可能会影响到标记功能等，如果出现异常，需要重新启动任务";
            }
            else
            {
                btn_暂停.ImageOptions.SvgImage = img_恢复.ImageOptions.SvgImage;
                btn_暂停.ToolTip = @"点击恢复任务，继续执行";
            }
            btn_暂停.ImageOptions.SvgImage = text == "暂停" ? img_暂停.ImageOptions.SvgImage : img_恢复.ImageOptions.SvgImage;
            btn_暂停.Text = text;
        }

        if (enable != null)
        {
            btn_暂停.Enabled = enable.Value;
        }
    }

    private void SetBtnLock(bool enable)
    {
        btnLock.Enabled = enable;
    }

    private void 执行关加成()
    {
        btn_Run.Text = @"停止";
        const string value = nameof(Functions.关闭加成);
        list_functions.SelectedValue = value;
        _taskHelper = TaskHelper.NewInstance();
        _taskHelper.TaskName = value!;
        Functions.Invoke(value!, _taskHelper, num_挑战次数.Value.ToInt32());
    }

    private void 执行个人突破()
    {
        btn_Run.Text = @"停止";
        const string value = nameof(Functions.个人突破);
        list_functions.SelectedValue = value;
        _taskHelper = TaskHelper.NewInstance();
        _taskHelper.TaskName = value!;
        Functions.Invoke(value!, _taskHelper, num_突破退次数.Value.ToInt32(), checkBox_突破绿标五.Checked, checkBox_突破红标花.Checked);
    }

    private void 执行探索副本()
    {
        GlobalConst.FinishReason = string.Empty;
        btn_Run.Text = @"停止";
        const string value = nameof(Functions.探索副本);
        list_functions.SelectedValue = value;
        _taskHelper = TaskHelper.NewInstance();
        _taskHelper.TaskName = value!;
        Functions.Invoke(value!, _taskHelper, num_挑战次数.Value.ToInt32(), list_副本章节.SelectedValue.ToInt32(28), radio_副本_普通.Checked);
    }
}