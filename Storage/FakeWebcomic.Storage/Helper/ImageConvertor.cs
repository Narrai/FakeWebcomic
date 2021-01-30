using System.Drawing;
using System.IO;
public class ImageConvertor
{
    public ImageConvertor()
    {
    }
    public Image ConvertByteArrayToImage(byte[] byteArrayIn)
    {
        using (MemoryStream ms = new MemoryStream(byteArrayIn))
        {
            return Image.FromStream(ms);
        }
    }

    public byte[] ConvertImageToByteArray(Image image, string extension)
    {
        using (var memoryStream = new MemoryStream())
        {
            switch (extension)
            {
                case ".jpeg":
                case ".jpg":
                    image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    break;
                case ".png":
                    image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                    break;
                case ".gif":
                    image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Gif);
                    break;
            }
            return memoryStream.ToArray();
        }
    }
}
