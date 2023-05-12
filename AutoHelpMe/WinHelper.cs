using AutoHelpMe.EventBus;
using OpenCvSharp;
using PInvoke;
using SageTools.Extension;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using Point = OpenCvSharp.Point;

namespace AutoHelpMe;

public class WinHelper
{
    private static readonly object Lock = new();
    private static WinHelper _winHelper;
    private readonly Random _random;
    private IntPtr _windowHandle;
    private string _windowTitle;
    private Dictionary<string, Tuple<Mat, string>> _images;

    public WinHelper()
    {
        _random = new Random();
        _images = new Dictionary<string, Tuple<Mat, string>>();
    }

    /// <summary>
    /// 实例
    /// </summary>
    public static WinHelper Instance
    {
        get { return _winHelper ??= new WinHelper(); }
    }

    /// <summary>
    /// 设置窗口句柄
    /// </summary>
    public void SetWindowHandle(IntPtr handle)
    {
        lock (Lock)
        {
            _windowHandle = handle;
        }
    }

    /// <summary>
    /// 设置窗口句柄
    /// </summary>
    public void SetWindowTitle(string title)
    {
        lock (Lock)
        {
            _windowTitle = title;
        }
    }

    /// <summary>
    /// 加载指定标题窗口
    /// </summary>
    public void LoadWindow(string windowTitle)
    {
        _windowHandle = User32.FindWindow(null, windowTitle);
        while (_windowHandle == IntPtr.Zero)
        {
            Logger.Info($"等待窗口 {windowTitle} 出现...");
            Thread.Sleep(2000);
            _windowHandle = User32.FindWindow(null, windowTitle);
        }
        Logger.Success($"窗口 {windowTitle} 加载成功");
    }

    /// <summary>
    /// 监听鼠标中键点击事件
    /// </summary>
    public void HookMouseMiddleClick()
    {
        var hookId = IntPtr.Zero;
        var handle = User32.SafeHookHandle.Null;
        using var process = Process.GetCurrentProcess();
        using var module = process.MainModule;
        handle = User32.SetWindowsHookEx(User32.WindowsHookType.WH_MOUSE_LL, (code, wParam, lParam) =>
        {
            if (code >= 0 && wParam == (IntPtr)User32.WindowMessage.WM_MBUTTONDOWN)
            {
                var hookStruct = Marshal.PtrToStructure<System.Drawing.Point>(lParam);
                var windowHandle = User32.WindowFromPoint(hookStruct);

                handle.Close();
                //通过EventBus发送获取到的Handle
                EventBusHelper.EventAggregator.GetEvent<WindowHandleEvent>().Publish(windowHandle);
            }
            return User32.CallNextHookEx(hookId, code, wParam, lParam);
        }, Kernel32.GetModuleHandle(module!.ModuleName), 0);
        hookId = handle.DangerousGetHandle();
    }

    /// <summary>
    /// 加载图片
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, Tuple<Mat, string>> LoadImages(string imgDir = "")
    {
        if (_images.Keys.Count > 0)
        {
            return _images;
        }
        // 创建字典存储所有图像
        _images = new Dictionary<string, Tuple<Mat, string>>();
        if (imgDir.IsNullOrWhiteSpace())
        {
            imgDir = IsPc ? "Resource\\Images" : "Resource\\MNQImages";
        }
        var path = Path.Combine(Directory.GetCurrentDirectory(), imgDir);
        // 获取所有文件
        var fileList = Directory.GetFiles(path);
        foreach (var filePath in fileList)
        {
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            var image = Cv2.ImRead(filePath);
            var imageInfo = new Tuple<Mat, string>(image, fileName);
            _images[fileName] = imageInfo;
        }

        return _images;
    }

    /// <summary>
    /// 截图指定窗口
    /// </summary>
    public Bitmap CaptureWindow(int delay = 50)
    {
        User32.GetWindowRect(_windowHandle, out var rect);
        var width = rect.right - rect.left;
        var height = rect.bottom - rect.top;

        var hdcSrc = User32.GetDC(_windowHandle);
        var hdcDest = Gdi32.CreateCompatibleDC(hdcSrc);
        var hBitmap = Gdi32.CreateCompatibleBitmap(hdcSrc, width, height);
        var hOld = Gdi32.SelectObject(hdcDest, hBitmap);

        Gdi32.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, 0x00CC0020);

        Gdi32.SelectObject(hdcDest, hOld);
        Gdi32.DeleteDC(hdcDest);
        User32.ReleaseDC(_windowHandle, hdcSrc.HWnd);

        var bitmap = Image.FromHbitmap(hBitmap);
        Gdi32.DeleteObject(hBitmap);

        Task.Delay(delay).Wait();
        return bitmap;
    }

    /// <summary>
    /// 找图(灰度)
    /// </summary>
    /// <param name="sourceImage"></param>
    /// <param name="targetImage"></param>
    /// <param name="threshold">精度</param>
    /// <returns></returns>
    public Rectangle Find(Bitmap sourceImage, Mat targetImage, double threshold = 0.95)
    {
        try
        {
            var refMat = Mat.FromImageData(Bitmap2Byte(sourceImage), ImreadModes.AnyColor); //大图

            using var res = new Mat(refMat.Rows - targetImage.Rows + 1, refMat.Cols - targetImage.Cols + 1, MatType.CV_32FC1);
            var sourceColor = refMat.CvtColor(ColorConversionCodes.BGR2GRAY);
            var targetColor = targetImage.CvtColor(ColorConversionCodes.BGR2GRAY);

            Cv2.MatchTemplate(sourceColor, targetColor, res, TemplateMatchModes.CCoeffNormed);
            Cv2.Threshold(res, res, threshold, 1.0, ThresholdTypes.Tozero);
            Cv2.MinMaxLoc(res, out _, out var maxVal, out _, out var maxLoc);

            if (maxVal >= threshold)
            {
                return new Rectangle(maxLoc.X, maxLoc.Y, targetImage.Width, targetImage.Height);
            }
        }
        catch (Exception e)
        {
            Logger.Error(e.Message);
        }

        return Rectangle.Empty;
    }

    /// <summary>
    /// 找图(带色)
    /// </summary>
    public Rectangle FindWithColorTolerance(Bitmap sourceImage, Mat targetImage, double threshold = 0.95, int colorTolerance = 30)
    {
        try
        {
            var refMat = Mat.FromImageData(Bitmap2Byte(sourceImage), ImreadModes.AnyColor); //大图

            using var res = new Mat(refMat.Rows - targetImage.Rows + 1, refMat.Cols - targetImage.Cols + 1, MatType.CV_32FC1);

            // 确保目标图像具有与源图像相同的通道数
            if (refMat.Channels() != targetImage.Channels())
            {
                targetImage = targetImage.CvtColor(refMat.Channels() == 3 ? ColorConversionCodes.GRAY2BGR : ColorConversionCodes.BGR2GRAY);
            }

            Cv2.MatchTemplate(refMat, targetImage, res, TemplateMatchModes.CCoeffNormed);
            Cv2.Threshold(res, res, threshold, 1.0, ThresholdTypes.Tozero);
            Cv2.MinMaxLoc(res, out _, out var maxVal, out _, out var maxLoc);

            if (maxVal >= threshold)
            {
                var foundTarget = refMat.SubMat(new Rect(maxLoc.X, maxLoc.Y, targetImage.Width, targetImage.Height));
                var diff = new Mat();
                Cv2.Absdiff(targetImage, foundTarget, diff);
                var mask = diff.LessThan(colorTolerance);

                // 将多通道的掩码图像合并成一个单通道的二值图像
                var binaryMask = new Mat();
                Cv2.CvtColor(mask, binaryMask, ColorConversionCodes.BGR2GRAY);

                var validPixels = Cv2.CountNonZero(binaryMask);

                double matchRatio = (double)validPixels / (targetImage.Width * targetImage.Height);

                if (matchRatio >= threshold)
                {
                    return new Rectangle(maxLoc.X, maxLoc.Y, targetImage.Width, targetImage.Height);
                }
            }
        }
        catch (Exception e)
        {
            Logger.Error(e.Message);
        }

        return Rectangle.Empty;
    }



    /// <summary>
    /// 找图
    /// </summary>
    /// <param name="sourceImage"></param>
    /// <param name="targetImage"></param>
    /// <param name="threshold">精度</param>
    /// <returns></returns>
    public List<Rectangle> FindAll(Bitmap sourceImage, Mat targetImage, double threshold = 0.95)
    {
        var refMat = Mat.FromImageData(Bitmap2Byte(sourceImage), ImreadModes.AnyColor); //大图

        using var res = new Mat(refMat.Rows - targetImage.Rows + 1, refMat.Cols - targetImage.Cols + 1, MatType.CV_32FC1);
        var sourceColor = refMat.CvtColor(ColorConversionCodes.BGR2GRAY);
        var targetColor = targetImage.CvtColor(ColorConversionCodes.BGR2GRAY);

        Cv2.MatchTemplate(sourceColor, targetColor, res, TemplateMatchModes.CCoeffNormed);
        Cv2.Threshold(res, res, threshold, 1.0, ThresholdTypes.Tozero);
        Cv2.MinMaxLoc(res, out _, out var maxVal, out _, out var maxLoc);

        var results = new List<Rectangle>();
        while (maxVal >= threshold)
        {
            var result = new Rectangle(maxLoc.X, maxLoc.Y, targetImage.Width, targetImage.Height);
            results.Add(result);

            // 在模板匹配结果中，将已经匹配的区域设置为 0
            Cv2.Rectangle(res, new Point(result.X, result.Y), new Point(result.X + result.Width, result.Y + result.Height), Scalar.All(0), -1);
            Cv2.MinMaxLoc(res, out _, out maxVal, out _, out maxLoc);
        }
        return results;
    }

    /// <summary>
    /// 模拟鼠标点击（会随机图像区域内的坐标）
    /// </summary>
    /// <param name="rectangle"></param>
    public void Click(Rectangle rectangle)
    {
        var x = _random.Next(rectangle.Left, rectangle.Right);
        var y = _random.Next(rectangle.Top, rectangle.Bottom);

        User32.PostMessage(_windowHandle, User32.WindowMessage.WM_LBUTTONDOWN, new IntPtr(0x0001 & 0xFFFF), new IntPtr(x + (y << 16)));
        RandomDelay(100, 300);
        User32.PostMessage(_windowHandle, User32.WindowMessage.WM_LBUTTONUP, new IntPtr(0x0001 & 0xFFFF), new IntPtr(x + (y << 16)));
    }

    /// <summary>
    /// 按下ESC按键
    /// </summary>
    public void PressKeyOfEsc()
    {
        var key = new IntPtr(0x1B);
        User32.PostMessage(_windowHandle, User32.WindowMessage.WM_KEYDOWN, key, IntPtr.Zero);
        User32.PostMessage(_windowHandle, User32.WindowMessage.WM_KEYUP, key, IntPtr.Zero);
    }

    /// <summary>
    /// 随机延迟(毫秒)
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    public void RandomDelay(int min = 500, int max = 1500)
    {
        Delay(_random.Next(min, max));
    }

    /// <summary>
    /// 延迟(毫秒)
    /// </summary>
    /// <param name="time"></param>
    public void Delay(int time)
    {
        Task.Delay(time).Wait(time);
    }

    /// <summary>
    /// BitMap转Byte[]
    /// </summary>
    /// <param name="bitmap"></param>
    /// <returns></returns>
    private byte[] Bitmap2Byte(Bitmap bitmap)
    {
        using var stream = new MemoryStream();
        bitmap.Save(stream, ImageFormat.Jpeg);
        var data = new byte[stream.Length];
        stream.Seek(0, SeekOrigin.Begin);
        _ = stream.Read(data, 0, Convert.ToInt32(stream.Length));
        return data;
    }

    /// <summary>
    /// 删除指定目录下所有文件
    /// </summary>
    /// <param name="path"></param>
    /// <param name="includeSelf"></param>
    public void DeleteDirectory(string path, bool includeSelf)
    {
        if (path.IsNullOrWhiteSpace())
        {
            path = Path.Combine(Directory.GetCurrentDirectory(), "Resource");
        }
        if (!Directory.Exists(path)) return;
        foreach (var file in Directory.GetFiles(path))
        {
            try
            {
                File.Delete(file);
            }
            catch
            {
                // ignore
            }
        }
        foreach (var dir in Directory.GetDirectories(path))
        {
            DeleteDirectory(dir, true);
        }
        if (includeSelf)
        {
            Directory.Delete(path);
        }
    }

    private bool IsPc => _windowTitle == "阴阳师-网易游戏";
}