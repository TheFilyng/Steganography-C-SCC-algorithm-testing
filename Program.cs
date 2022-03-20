namespace Test_Steg
{
    class Program
    {
        static void Main(string[] args)
        {   
            String imagepath = "g30m3.png";
            ImageManager manager = new ImageManager(imagepath);
            Console.WriteLine("Imagen cargada!");
            Console.WriteLine("Numero de bytes de la imagen: " + manager.getNumberOfBytes());
            Console.WriteLine("Guardando bytes a nueva imagen");
            manager.SaveNewImage(manager.getImageBytes());
        }
    }
}