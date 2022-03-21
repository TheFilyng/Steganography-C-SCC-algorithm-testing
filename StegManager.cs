using System;
using System.Drawing;
using System.Text;
using System.Text.Json;

namespace TestSteg
{
    public class StegManager
    {

        private RunData data;
        private Color[] pixels;
        private ImageManager manager;
        public StegManager(RunData data, Image coverImage)
        {
            this.data = data;
            manager = new ImageManager(coverImage);
            pixels = manager.getPixels();
            Console.WriteLine("Creado StegManager");
        }

        private String generarBitsdeJson()
        {
            String mensaje = JsonSerializer.Serialize(data);
            Byte[] mensajeEnBytes = Encoding.Unicode.GetBytes(mensaje);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in mensajeEnBytes)
            {
                sb.Append(Convert.ToString(b, 2));
            }

            return sb.ToString();
        }

        public void codificarMensaje()
        {
            int R;
            int G;
            int B;

            String bitsMensaje = generarBitsdeJson();
            StringBuilder sb = new StringBuilder(bitsMensaje);
            int index = 0;
            Console.WriteLine("Comienzo generación de stego image");
            while(true)
            {   
                //Codificar en R 
                if (sb.Length == index) break;
                R = pixels[index].R;
                R = sb[index].Equals('1') ? R | 1 : R & ~1; 
                index++;             
                
                //Codificar en G
                if (sb.Length == index) break;
                G = pixels[index].G;
                G = sb[index].Equals('1') ? G | 1 : G & ~1;
                index++;

                //Codificar en B
                if (sb.Length == index) break;
                B = pixels[index].B;
                B = sb[index].Equals('1') ? B | 1 : B & ~1;
                index++;
            }
            Console.WriteLine("Fin generación de stego image");
        }

        public void crearStegoImage()
        {
            manager.SaveNewImage(pixels);
        }
    }
}