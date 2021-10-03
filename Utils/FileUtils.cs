namespace PhotoFrame.Utils
{
    public static class FileUtils
    {
        public static string FormatFileSize(long bytes)
        {
            if (bytes < 1000)
                return $"{bytes} Bytes";
            
            if (bytes < 1024000)
                return $"{bytes / 1024.0:F2} KiB";

            if (bytes < 1048576000)
                return $"{bytes / 1048576.0:F2} MiB";
            
            return $"{bytes / 1073741824.0:F2} GiB";
        }
    }
}