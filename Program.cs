namespace Test_Steg
{
    class Program
    {
        static void Main(string[] args)
        {   
            String imagepath = "g30m3.png";
            ImageLoader loader = new ImageLoader(imagepath);
            Console.WriteLine("Imagen cargada!");
            Console.WriteLine("Numero de bytes de la imagen: " + loader.getNumberOfBytes());
        }
    }
}