using System;
using System.Drawing;
using System.Text;
using System.Text.Json;
using System.Collections;
using System.Web;

namespace TestSteg
{
    public class StegManager
    {
        private static String key = "EED8BC84E4CAA3B61CC2D30D734C3FD7";
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

        private Byte[] generateDataBytes()
        {
            String mensaje = JsonSerializer.Serialize(data); //Queremos que la RunData sea Json para facilitar el parseo al descodificar.
            Console.WriteLine("Datos en JSON");
            Console.WriteLine(mensaje);
            Byte[] mensajeEnBytes = Encoding.ASCII.GetBytes(XORencrypt(mensaje, key));
            return mensajeEnBytes;
            
        }
        private bool BitFromByte(int b, int bitNumber){
                bool bitbool = ((b >> bitNumber) & 1) != 0;
                return bitbool;
        }

        //Obtenido de: https://www.codeproject.com/Answers/312449/How-to-convert-a-bool-array-to-a-byte-and-further#answer2
        int BoolArrayToInt(bool[] bits){
            if(bits.Length > 32) throw new ArgumentException("Can only fit 32 bits in a uint");  
            int r = 0;
            for(int i = 0; i < bits.Length; i++) if(bits[i]) r |= 1 << (bits.Length - i - 1);
            return r;
        }

        //Obtenido de: https://piriysdev.wordpress.com/2014/04/01/c-sharp-xor-encryptiondecryption/
        public static string XORencrypt(string text, string key)
        {
            byte[] decrypted = Encoding.UTF8.GetBytes(text);
            byte[] encrypted = new byte[decrypted.Length];
        
            for (int i = 0; i < decrypted.Length; i++)
            {
                encrypted[i] = (byte)(decrypted[i] ^ key[i % key.Length]);
            }
        
            string xored = System.Convert.ToBase64String(encrypted);
        
            return xored;
        }
        public static string XORdecrypt(string text, string key)
        {
            var decoded = System.Convert.FromBase64String(HttpUtility.UrlDecode(text));
        
            byte[] result = new byte[decoded.Length];
        
            for (int c = 0; c < decoded.Length; c++)
            {
                result[c] = (byte)((uint)decoded[c] ^ (uint)key[c % key.Length]);
            }
            
            string dexored = Encoding.UTF8.GetString(result);
        
            return dexored;
        }
 
        public void encodeData()
        {
            int R = 0, G = 0, B = 0;
            int bit = 0;
            int cicloRGB = 0;       
            Byte[] mensajeEnBytes = generateDataBytes();
            int tamanoMensaje = mensajeEnBytes.Length;
            int pixelActual = 0;
            bool pixelModificado = false;
            //Codificaci??n del tama??o del mensaje, en los primeros 32 bits.
            for (int i = 31; i >= 0; i--)
            {
                pixelModificado = false;
                bit = (BitFromByte(tamanoMensaje, i) == true) ? 1 : 0; //Obtenemos el bit del caracter
                Console.Write(bit);
                if(cicloRGB == 0) //Codificamos el bit en R
                {
                    R = pixels[pixelActual].R;                        
                    R = (bit == 1) ? R | 1 : R & ~1;
                    //Console.WriteLine("Estamos en R " + i);
                    cicloRGB++;
                }
                else if (cicloRGB == 1) //Codificamos el bit en G
                {
                    G = pixels[pixelActual].G;                          
                    G = (bit == 1) ? G | 1 : G & ~1;
                    //Console.WriteLine("Estamos en G " + i);

                    cicloRGB++;
                }
                else
                {                   
                    B = pixels[pixelActual].B;                          
                    B = (bit == 1) ? B | 1 : B & ~1;
                    //Se han modificado los bits de cada uno de los canales del pixel, por lo que insertamos el nuevo pixel en el array de pixeles
                    pixels[pixelActual] = Color.FromArgb(255,R,G,B);
                    pixelActual++;
                    pixelModificado = true;
                    cicloRGB = 0;

                }         
            }
            if(!pixelModificado) //Si la ultima iteraci??n no pasa por el caso del canal B
            {
                B = pixels[pixelActual].B;                          
                pixels[pixelActual] = Color.FromArgb(255,R,G,B);
                pixelActual++;
                pixelModificado = false;
            }
            
            
            //Codificaci??n del mensaje
            cicloRGB = 0;
            Console.WriteLine();
            Console.WriteLine("Comienzo de codificaci??n");
            foreach (Byte b in mensajeEnBytes)
            {
                for (int i = 7; i >= 0; i--)
                {
                    pixelModificado = false;
                    bit = (BitFromByte((int)b, i) == true) ? 1 : 0; //Obtenemos el bit del caracter
                    if(cicloRGB == 0) //Codificamos el bit en R
                    {
                        R = pixels[pixelActual].R;                        
                        R = (bit == 1) ? R | 1 : R & ~1;
                        cicloRGB++;
                    }
                    else if (cicloRGB == 1) //Codificamos el bit en G
                    {
                        G = pixels[pixelActual].G;                          
                        G = (bit == 1) ? G | 1 : G & ~1;
                        cicloRGB++;
                    }
                    else
                    {                   
                        B = pixels[pixelActual].B;                          
                        B = (bit == 1) ? B | 1 : B & ~1;
                        //Se han modificado los bits de cada uno de los canales del pixel, por lo que insertamos el nuevo pixel en el array de pixeles
                        pixels[pixelActual] = Color.FromArgb(255,R,G,B);
                        pixelActual++;
                        cicloRGB = 0;
                    }         
                }
                if(!pixelModificado) //Si la ultima iteraci??n no pasa por el caso del canal B
                {
                    B = pixels[pixelActual].B;                          
                    pixels[pixelActual] = Color.FromArgb(255,R,G,B);
                    pixelActual++;
                    pixelModificado = false;
                    cicloRGB = 0;
                }
            }
            wasDataEncoded = true;
            Console.WriteLine();
            Console.WriteLine("Mensaje codificado");
            for (int j = 0; j < 11; j++)
            {
                Console.Write(pixels[j].R & 1);
                Console.Write(pixels[j].G & 1);
                Console.Write(pixels[j].B & 1);
            }
        }

        public void decodeData()
        {
            if(wasDataEncoded)
            {
                //Proceso de obtenci??n del tama??o del mensaje de los primeros 11 pixeles
                int i;
                int indexBit = 0;
                int indexPixel = 0;
                int tamanoMensaje = 0;
                Color pixelActual;

                //Primero obtenemos el tama??o del mensaje
                bool[] bitsMensaje = new bool[32];
                while(indexBit < 32)
                {
                    pixelActual = pixels[indexPixel];
                    bitsMensaje[indexBit++] = ((pixelActual.R & 1) == 1);

                    bitsMensaje[indexBit++] = ((pixelActual.G & 1) == 1);

                    if(indexBit==32) break;
                    bitsMensaje[indexBit++] = ((pixelActual.B & 1) == 1);

                    indexPixel++;
                }
                indexPixel++;
                tamanoMensaje = BoolArrayToInt(bitsMensaje);
                Console.WriteLine("\n" + tamanoMensaje);

                //Obtenemos el mensaje
                String mensajeDecoded = "";
                int numeroDeBitsMensaje = tamanoMensaje*8;
                bool[] caracterBits = new bool[8];
                for (i = 0; i < tamanoMensaje; i++)
                {
                    char caracter;
                    indexBit = 0;
                    while(indexBit < 8)
                    {
                        pixelActual = pixels[indexPixel];
                        caracterBits[indexBit++] = ((pixelActual.R & 1) == 1);

                        caracterBits[indexBit++] = ((pixelActual.G & 1) == 1);

                        if(indexBit==8) break;
                        caracterBits[indexBit++] = ((pixelActual.B & 1) == 1);

                        indexPixel++;
                    }
                    indexPixel++;
                    //Obtenemos el valor ascii  
                    caracter = Convert.ToChar(BoolArrayToInt(caracterBits));
                    //Lo a??adimos al mensaje
                    mensajeDecoded = mensajeDecoded + caracter;
                }
                Console.WriteLine(XORdecrypt(mensajeDecoded, key));
            }
            else
            {
                Console.WriteLine("No se ha codificado ning??n mensaje");
            }
        }

        public void createStegoImage()
        {
            manager.SaveNewImage(pixels);
            Console.WriteLine("Creada la stego image.");
        }
    }
}