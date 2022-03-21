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
        private bool wasDataEncoded = false;
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
            return mensajeBits;
        }

        private int obtainEncodedMessageSize()
        {
            int tamanoMensaje = 0;
            int i = 0;
            //Extraemos tamaño del mensaje de los primeros 32 bytes
            StringBuilder sb = new StringBuilder();
            /*Al ser los primeros 32 bits codificados los que contienen el tamaño del mensaje
            tenemos que mirar los primeros 10 bytes. Del decimo byte solamente los valores R y G*/
            for (i = 0; i < 11; i++)
            {
                Color pixelActual = pixels[i];
                if(i < 10)
                {                        
                    int lsbR = pixelActual.R & 1;
                    sb.Append(lsbR);
                    int lsbG = pixelActual.G & 1;
                    sb.Append(lsbG);
                    int lsbB = pixelActual.B & 1;
                    sb.Append(lsbB);
                }
                else
                {
                    int lsbR = pixelActual.R & 1;
                    sb.Append(lsbR);
                    int lsbG = pixelActual.G & 1;
                    sb.Append(lsbG);
                }
            }
            tamanoMensaje = Convert.ToInt32(sb.ToString(), 2);
            return tamanoMensaje;
        }
        public void encodeData()
        {
            int R;
            int G;
            int B;

            String bitsMensaje = generateDataBits();
            int messageLength = bitsMensaje.Length;
            Console.WriteLine("Tamaño del mensaje: " + messageLength);
            String bitsTamano = Convert.ToString(messageLength, 2).PadLeft(32, '0');
            Console.WriteLine("Bits del tamaño del mensaje: " + bitsTamano);
            String bitsToEncode = bitsTamano+bitsMensaje; //Necesitamos el tamaño del mensaje para poder decodificar. Los primeros 32 bytes indicaran esto.
            Console.WriteLine("Datos en bits");
            Console.WriteLine(bitsToEncode);
            StringBuilder sb = new StringBuilder(bitsToEncode);
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
            wasDataEncoded = true;
            Console.WriteLine("Fin generación de stego image");
        }

        public void decodeData()
        {
            if(wasDataEncoded)
            {
                int tamanoMensaje = obtainEncodedMessageSize();
                String decodifiedMessage = "";
                Byte[] messageBytes = new Byte[tamanoMensaje];
                //Proceso de decodificacion
                StringBuilder sb = new StringBuilder();
                int contadorBits = 0;
                int indexPixel = 10;
                while (!(contadorBits==tamanoMensaje))
                {
                    Color pixelActual = pixels[indexPixel];
                    if(indexPixel==10)
                    {
                        int lsbB = pixelActual.B & 1;
                        sb.Append(lsbB);
                        contadorBits++;
                    }
                    else
                    {
                        if (contadorBits == tamanoMensaje) break;
                        int lsbR = pixelActual.R & 1;
                        contadorBits++;
                        sb.Append(lsbR);
                        
                        if (contadorBits == tamanoMensaje) break;    
                        int lsbG = pixelActual.G & 1;
                        contadorBits++;
                        sb.Append(lsbG);

                        if (contadorBits == tamanoMensaje) break;
                        int lsbB = pixelActual.B & 1;
                        contadorBits++;
                        sb.Append(lsbB);
                    }
                    /*for(int i = 0; i < tamanoMensaje; ++i)
                    {
                        messageBytes[i] = Convert.ToByte(sb.ToString().Substring(8 * i, 8), 2);
                    }
                    decodifiedMessage = Encoding.Unicode.GetString(messageBytes);
                    Console.WriteLine("Mensaje descodificado:");
                    Console.WriteLine(decodifiedMessage);*/
                }     
            Console.WriteLine("BITS DECODIFICADOS");
            Console.WriteLine(sb.ToString());   
            }
            else{
                Console.WriteLine("No se han codificado datos.");
            }
        }

        public void createStegoImage()
        {
            manager.SaveNewImage(pixels);
            Console.WriteLine("Creada la stego image.");
        }
    }
}