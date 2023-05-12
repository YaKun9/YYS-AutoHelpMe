using SageTools.Extension;

namespace AutoHelpMe;

public static class Extensions
{
    /// <summary>
    /// List AddRange拓展
    /// </summary>
    /// <param name="arr"></param>
    /// <param name="arr2"></param>
    /// <returns></returns>
    public static List<string> AddExt(this List<string> arr, IList<string> arr2)
    {
        if (arr.IsNullOrEmpty() || arr2.IsNullOrEmpty()) return arr;
        arr.AddRange(arr2);
        return arr;
    }
}