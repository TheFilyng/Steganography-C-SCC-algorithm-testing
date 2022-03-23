using System.Text.Json;

namespace TestSteg
{
    public class RunData //Representa la información de la partida que ira codificada en la imagen, implementación básica (necesitará ampliación en el futuro)
    {
        
        public String username {get; set;}
        //private DateTime fecha;
        public double duracion {get; set;}//Duracion en milisegundos de la partida
        public long puntuacion {get; set;}
        public int enemigosEliminados {get; set;}
        public String nivelHash {get; set;}
        public String gameVer {get; set;}
        public bool exito {get; set;}
        
    }
}