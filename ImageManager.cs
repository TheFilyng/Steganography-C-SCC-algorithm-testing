using System;
using System.Drawing;

namespace TestSteg
{
    public sealed class ImageManager
    {
        private Bitmap coverImage; //Para poder tratar los pixeles/bytes sin compresión procesamos la imagen como Bitmap.
        private int coverImage_numBytes;
        private Color[] coverImage_colors;
        
        public ImageManager(Image coverImage)
        {
          this.coverImage = new Bitmap(coverImage);
          coverImage_colors = generateColorArray();
          coverImage_numBytes = this.coverImage.Width*this.coverImage.Height*3;
        }

        public int getNumberOfBytes()
        {
          return coverImage_numBytes;
        }

        public Color[] getPixels()
        {
          return coverImage_colors;
        }

        public void SaveNewImage(Color[] newPixeles)
        {
          Bitmap newImage = new Bitmap(coverImage.Width, coverImage.Height);
          String newName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
          int numPixel = 0;
          int width = coverImage.Width;
          int height = coverImage.Height;
          int i = 0;
          int j = 0;
          for (i = 0; i < width; i++)
          {
            for (j = 0; j < height; j++)
            {
              newImage.SetPixel(i, j, newPixeles[numPixel]);
              numPixel++;
            }
          }

          //Guardar el bitmap como png
          newImage.Save(newName, System.Drawing.Imaging.ImageFormat.Png);  
        }
        
        private Color[] generateColorArray() //Crea un array de pixeles, cada pixel tiene el valor A, R, G, B.
        {
          int numPixel = 0;
          int width = coverImage.Width;
          int height = coverImage.Height;
          int i = 0;
          int j = 0;
          Color[] colors = new Color[width*height];
          for (i = 0; i < width; i++)
          {
            for (j = 0; j < height; j++)
            {
              Color pixel = coverImage.GetPixel(i, j);
              colors[numPixel] = pixel;
              numPixel++;
            }
          }
          return colors;
        }
        

    }
}