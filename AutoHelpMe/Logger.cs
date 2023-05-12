using AutoHelpMe.EventBus;

namespace AutoHelpMe;

public static class Logger
{
    #region 基础类型

    public static void DeBug(string log,bool printLevel=false)
    {
        var level = printLevel ? " | debug | " : " ";
        log = $"{DateTime.Now:HH:mm:ss}{level}{log}{Environment.NewLine}";
        EventBusHelper.EventAggregator.GetEvent<PrintLogEvent>().Publish(new Tuple<string, Color>(log, Color.LightBlue));
    }

    public static void Info(string log, bool printLevel = false)
    {
        var level = printLevel ? " | info | " : " ";
        log = $"{DateTime.Now:HH:mm:ss}{level}{log}{Environment.NewLine}";
        EventBusHelper.EventAggregator.GetEvent<PrintLogEvent>().Publish(new Tuple<string, Color>(log, Color.LightGray));
    }

    public static void Success(string log, bool printLevel = false)
    {
        var level = printLevel ? " | success | " : " ";
        log = $"{DateTime.Now:HH:mm:ss}{level} {log}{Environment.NewLine}";
        EventBusHelper.EventAggregator.GetEvent<PrintLogEvent>().Publish(new Tuple<string, Color>(log, Color.LightGreen));
    }

    public static void Warn(string log, bool printLevel = false)
    {
        var level = printLevel ? " | warn | " : " ";
        log = $"{DateTime.Now:HH:mm:ss}{level}{log}{Environment.NewLine}";
        EventBusHelper.EventAggregator.GetEvent<PrintLogEvent>().Publish(new Tuple<string, Color>(log, Color.DarkOrange));
    }

    public static void Error(string log, bool printLevel = false)
    {
        var level = printLevel ? " | error | " : " ";
        log = $"{DateTime.Now:HH:mm:ss}{level}{log}{Environment.NewLine}";
        EventBusHelper.EventAggregator.GetEvent<PrintLogEvent>().Publish(new Tuple<string, Color>(log, Color.Red));
    }

    #endregion 基础类型

    #region 快捷打印

    /// <summary>
    /// 快捷打印挑战次数(参数大于等于0才会打印)
    /// </summary>
    /// <param name="succ">成功次数</param>
    /// <param name="fail">失败次数</param>
    /// <param name="maxCount">最大次数</param>
    public static void PrintChallengeCount(int succ = 0, int fail = 0, int maxCount = 0)
    {
        var list = new List<string>();
        if (succ > 0)
        {
            list.Add($"成功{succ}次");
        }

        if (fail > 0)
        {
            list.Add($"失败{fail}次");
        }

        if (maxCount > 0 && succ > 0)
        {
            list.Add(maxCount - succ > 0 ? $"剩余{maxCount - succ}次" : "已刷完");
        }

        if (list.Any())
        {
            Logger.Success(string.Join('，', list));
        }
       
    }

    #endregion 快捷打印
}