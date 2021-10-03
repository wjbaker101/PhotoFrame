using System.Drawing.Imaging;
using System.Linq;

namespace PhotoFrame.Helpers
{
    public static class ImageHelper
    {
        public static string GetOpenImageDialogFilter()
        {
            var codecs = ImageCodecInfo.GetImageEncoders();
            var imageFileExtensions = string.Join(";", codecs.Select(x => x.FilenameExtension));

            return $"Images Files|{imageFileExtensions}|All Files (*.*)|*.*";
        }

        public static (double width, double height) FitWithinBounds(
            double childWidth,
            double childHeight,
            double parentWidth,
            double parentHeight)
        {
            if (childWidth <= parentWidth && childHeight <= parentHeight)
                return (childWidth, childHeight);

            var childRatio = childWidth / childHeight;
            var parentRatio = parentWidth / parentHeight;

            var finalRatio = childRatio >= parentRatio
                ? parentWidth / childWidth
                : parentHeight / childHeight;

            return (childWidth * finalRatio, childHeight * finalRatio);
        }
    }
}