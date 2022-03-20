using System;
using System.Drawing;

namespace Test_Steg
{
    public class StegManager
    {

        String mensaje;
        Color[] pixels;

        public StegManager()
        {
            this.mensaje = "";
            pixels = new Color[0];
        }
        public StegManager(String mensaje)
        {
            this.mensaje = mensaje;
            pixels = new Color[0];
        }

        public void setMensaje(String mensaje){
            this.mensaje = mensaje;
        }

        public void codificarMensaje(Image coverImage)
        {
            ImageManager manager = new ImageManager(coverImage);
            byte R;
            byte G;
            byte B;
            pixels = manager.getPixels();
            for (int i = 0; i < pixels.Length; i++)
            {
                //Obtenemos cada canal (byte) del pixel
                R = pixels[i].R;
                G = pixels[i].G;
                B = pixels[i].B;

                //TODO: Convertir mensaje a codificar a bits
                //TODO: Realizar algoritmo de SCC
                
            }
        }
    }
}