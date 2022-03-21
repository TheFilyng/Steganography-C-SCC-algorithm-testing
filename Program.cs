using System;
using System.Drawing;
using System.Text;
using System.Text.Json;
namespace TestSteg
{
    class Program
    {
        static void Main(string[] args)
        {   
            String imagepath = "g30m3.png";
            Image imagen = Image.FromFile(imagepath);
            RunData data = new RunData()
            {
                username = "Abarruti",
                duracion = 300000000,
                puntuacion = 123142542343,
                enemigosEliminados = 23,
                nivelHash = "027351de5c7ea8e7c5fb602564808b6d",
                gameVer = "2021.1.04f",
                exito = true
            };
            StegManager manager = new StegManager(data, imagen);
            manager.encodeData();
            manager.createStegoImage();
        }
    }
}