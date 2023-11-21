using OpenCvSharp;
using System.Drawing.Imaging;

namespace AutoHelpMe2.Helper
{
    public class OpenCvHelper
    {
        /// <summary>
        /// 大图找小图
        /// </summary>
        /// <param name="source">大图</param>
        /// <param name="target">小图</param>
        /// <param name="threshold">匹配度阈值,越高越精准</param>
        /// <returns></returns>
        internal static Windows.Win32.Foundation.RECT FindImage(Bitmap source, string target, double threshold = 0.9)
        {
            using var sourceMat = BitmapToMat(source);
            using var targetMat = Cv2.ImRead(target);

            using var result = new Mat(sourceMat.Rows - targetMat.Rows + 1, sourceMat.Cols - targetMat.Cols + 1, MatType.CV_32FC1);
            var sourceColor = sourceMat.CvtColor(ColorConversionCodes.BGR2GRAY);
            var targetColor = targetMat.CvtColor(ColorConversionCodes.BGR2GRAY);

            Cv2.MatchTemplate(sourceColor, targetColor, result, TemplateMatchModes.CCoeffNormed);
            Cv2.Threshold(result, result, threshold, 1.0, ThresholdTypes.Tozero);
            Cv2.MinMaxLoc(result, out _, out var maxVal, out _, out var maxLoc);
            if (maxVal > threshold)
            {
                return new Windows.Win32.Foundation.RECT(maxLoc.X, maxLoc.Y, maxLoc.X + targetMat.Width,
                    maxLoc.Y + targetMat.Height);
            }

            return new Windows.Win32.Foundation.RECT();
        }

        private static Mat BitmapToMat(Bitmap bitmap)
        {
            using var stream = new MemoryStream();
            bitmap.Save(stream, ImageFormat.Jpeg);
            var data = new byte[stream.Length];
            stream.Seek(0, SeekOrigin.Begin);
            _ = stream.Read(data, 0, Convert.ToInt32(stream.Length));
            return Mat.FromImageData(data, ImreadModes.AnyColor);
        }
    }
}