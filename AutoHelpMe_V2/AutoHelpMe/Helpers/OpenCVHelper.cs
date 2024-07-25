using System.IO;
using OpenCvSharp;
using Serilog;
using Point = OpenCvSharp.Point;
using Rect = OpenCvSharp.Rect;

namespace AutoHelpMe.Helpers;

public class OpenCvHelper
{
    /// <summary>
    /// 在大图中查找小图，并返回最佳匹配位置和匹配度。
    /// </summary>
    /// <param name="bigImage">大图的 Mat 对象。</param>
    /// <param name="smallImage">小图的 Mat 对象。</param>
    /// <param name="threshold">匹配度阈值（默认值为 0.8）。如果匹配度大于或等于此值，则认为匹配成功。</param>
    /// <param name="showMatchedImage">是否展示匹配图像，这在Debug阶段非常有用</param>
    /// <returns>一个元组，包含最佳匹配位置和匹配度。如果未找到匹配位置，则返回 (Point(0, 0), matchValue)。</returns>
    public static (Point matchLocation, double matchValue) FindTemplateInImage(Mat bigImage, Mat smallImage, double threshold = 0.8, bool showMatchedImage = false)
    {
        var small = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "small.png");
        smallImage = Cv2.ImRead(small, ImreadModes.Color);
        // 创建输出结果的 Mat
        var result = new Mat();
        Cv2.MatchTemplate(bigImage, smallImage, result, TemplateMatchModes.CCoeffNormed);

        // 查找最佳匹配位置
        Cv2.MinMaxLoc(result, out var minVal, out var maxVal, out var minLoc, out var maxLoc);
        if (maxVal >= threshold)
        {
            if (showMatchedImage)
            {
                // 绘制矩形框
                var matchRect = new Rect(maxLoc.X, maxLoc.Y, smallImage.Width, smallImage.Height);
                Cv2.Rectangle(bigImage, matchRect, Scalar.Red, 2);
                // 显示结果
                Cv2.ImShow("Matched Image", bigImage);
                Cv2.WaitKey(0);
                Cv2.DestroyAllWindows();
            }
            LogHelper.Debug($"找图成功 X:{maxLoc.X} Y:{maxLoc.Y} W:{smallImage.Width} H:{smallImage.Height}");
            return (maxLoc, maxVal);
        }
        return (new Point(0, 0), maxVal);
    }
}