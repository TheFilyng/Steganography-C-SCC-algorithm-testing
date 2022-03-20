using System;
using System.Drawing;
namespace Test_Steg
{
    class Program
    {
        static void Main(string[] args)
        {   
            String imagepath = "g30m3.png";
            Image imagen = Image.FromFile(imagepath);
            ImageManager manager = new ImageManager(imagen);
            Console.WriteLine("Imagen cargada!");
            Console.WriteLine("Numero de bytes de la imagen: " + manager.getNumberOfBytes());
            Console.WriteLine("Guardando bytes a nueva imagen");
        }
    }
}