using System;
using System.Drawing;

namespace Test_Steg
{
    public class ImageLoader
    {
        private Image coverImage;
        private Byte[] coverImage_bytes;
        
        public ImageLoader(string path)
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

        private Byte[] loadImageBytes()
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

        

    }
}