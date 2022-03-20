using System;
using System.Drawing;

namespace Test_Steg
{
    public sealed class ImageManager
    {
        private Image coverImage;
        private Byte[] coverImage_bytes; //Puede no ser necesario una vez se haga la esteganografia. El coverImage_colors ya tiene los bytes internamente
        private Color[] coverImage_colors;
        
        public ImageManager(Image coverImage)
        {
          try
          {
            this.coverImage = coverImage;
            coverImage_bytes = loadImageBytes();
            coverImage_colors = generateColorArray();
          }
          catch (FileNotFoundException e)
          {      
              Console.WriteLine(e);
          }
        }

        public int getNumberOfBytes()
        {
          return coverImage_bytes.Length;
        }

        public Color[] getPixels()
        {
          return coverImage_colors;
        }

        private Byte[] loadImageBytes()
        {
          using (var stream = new MemoryStream())
          {
            coverImage.Save(stream, coverImage.RawFormat);
            return stream.ToArray();
          }
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
        
        private Color[] generateColorArray()
        {
          Bitmap imgBmp = new Bitmap(coverImage);
          int numPixel = 0;
          Color[] colors = new Color[0];
          for (int i = 0; i < coverImage.Width; i++)
          {
            for (int j = 0; j < coverImage.Height; j++)
            {
              numPixel = i*j;
              colors[numPixel] = imgBmp.GetPixel(i, j);
            }
          }
          return colors;
        }
        

    }
}