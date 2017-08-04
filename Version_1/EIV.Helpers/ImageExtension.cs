/// <summary>
/// 
/// </summary>
namespace EIV.Helpers
{
    using Microsoft.Win32;
    using System.IO;

    public static class ImageExtension
    {
        public static MemoryStream LoadImage(string imageFileName)
        {
            if (string.IsNullOrEmpty(imageFileName))
            {
                return null;
            }
            MemoryStream ms = new MemoryStream();
            using (FileStream fs = File.OpenRead(imageFileName))
            {
                fs.CopyTo(ms);
            }

            return ms;
        }

        public static string GetMimeType(string imageFileName)
        {
            string mimeType = "application/unknown";

            if (string.IsNullOrEmpty(imageFileName))
            {
                return mimeType;
            }

            FileInfo fileInfo = new FileInfo(imageFileName);
            if (fileInfo == null)
            {
                return mimeType;
            }

            RegistryKey regKey = Registry.ClassesRoot.OpenSubKey(
                fileInfo.Extension.ToLower()
                );

            if (regKey != null)
            {
                object contentType = regKey.GetValue("Content Type");

                if (contentType != null)
                    mimeType = contentType.ToString();
            }

            return mimeType;
        }
    }
}