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

        private String generateDataBits()
        {
            String mensaje = JsonSerializer.Serialize(data); //Queremos que la RunData sea Json para facilitar el parseo al descodificar.
            String mensajeBits = "";
            Console.WriteLine("Datos en JSON");
            Console.WriteLine(mensaje);
            Byte[] mensajeEnBytes = Encoding.Unicode.GetBytes(mensaje);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in mensajeEnBytes)
            {
                sb.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
            }
            mensajeBits = sb.ToString();
            Console.WriteLine("Datos en bits");
            Console.WriteLine(mensajeBits);
            return mensajeBits;
        }

        public void encodeData()
        {
            int R;
            int G;
            int B;

            String bitsMensaje = generateDataBits();
            StringBuilder sb = new StringBuilder(bitsMensaje);
            int indexPixel = 0;
            int indexString = 0;
            Console.WriteLine("Comienzo generación de stego image");
            while(true)
            {   

                R = pixels[indexPixel].R;
                G = pixels[indexPixel].G;
                B = pixels[indexPixel].B;

                //Codificar en R 
                if (sb.Length == indexString) break;
                R = sb[indexString++].Equals('1') ? R | 1 : R & ~1;              
                //Codificar en G
                if (sb.Length == indexString) break;
                G = sb[indexString++].Equals('1') ? G | 1 : G & ~1;
                //Codificar en B
                if (sb.Length == indexString) break;
                B = sb[indexString++].Equals('1') ? B | 1 : B & ~1;

                pixels[indexPixel] = Color.FromArgb(R,G,B);

                indexPixel++;

            }
            Console.WriteLine("Fin generación de stego image");
        }

        public void createStegoImage()
        {
            manager.SaveNewImage(pixels);
            Console.WriteLine("Creada la stego image.");
        }
    }
}