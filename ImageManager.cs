using System;
using System.Drawing;

namespace Test_Steg
{
    public class ImageManager
    {
        private Image coverImage;
        private Byte[] coverImage_bytes;
        
        public ImageManager(string path)
        {
          try
          {
            coverImage = Image.FromFile(path);
            coverImage_bytes = loadImageBytes();
          }
          catch (FileNotFoundException e)
          {      
              Console.WriteLine(e);
          }
        }

        public Byte[] getImageBytes()
        {
          return coverImage_bytes;
        }

        public Byte[] loadImageBytes()
        {
          using (var stream = new MemoryStream())
          {
            coverImage.Save(stream, coverImage.RawFormat);
            return stream.ToArray();
          }
        }

        public int getNumberOfBytes()
        {
          return coverImage_bytes.Length;
        }

        public void SaveNewImage(Byte[] imageBytes)
        {
          Image newImage;
          String newName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
          using (var stream = new MemoryStream(imageBytes))
          {
            newImage = Image.FromStream(stream);
            newImage.Save(newName, System.Drawing.Imaging.ImageFormat.Png);
          }
          
        }

        

    }
}