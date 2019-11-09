using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace IMSLibrary.UI
{
    public class UIHelper
    {
        protected UIHelper()
        {
        }

        public static void ShowErrorMessage(string errMsg)
        {
            Xceed.Wpf.Toolkit.MessageBox.Show(errMsg, "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
        }

        public static void ShowMessage(string msg)
        {
            Xceed.Wpf.Toolkit.MessageBox.Show(msg);
        }

        public static BitmapImage ImageFromBuffer(string filePath)
        {
            Byte[] bytes = IMSLibrary.Class.Helper.ConvertToByte(filePath);
            MemoryStream stream = new MemoryStream(bytes);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = stream;
            image.EndInit();
            return image;
        }

        public static byte[] ImageToByte(System.Drawing.Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }

        public static System.Drawing.Bitmap ByteToImage(byte[] blob)
        {
            MemoryStream mStream = new MemoryStream();
            byte[] pData = blob;
            mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
            System.Drawing.Bitmap bm = new System.Drawing.Bitmap(mStream, false);
            mStream.Dispose();
            return bm;
        }
    }
}