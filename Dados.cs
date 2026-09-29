using System.IO.Ports;

namespace Monopoly.App
{
    public class Dado
    {
        // ID del jugador que realizó el lanzamiento.
        public int IdJugador { get; private set; }

        // Valores obtenidos en cada dado.
        public int Dado1 { get; private set; }
        public int Dado2 { get; private set; }

        // Puerto serial utilizado para comunicarse con el hardware.
        private SerialPort rasp;

        // Contructor: Recibe el nombre del puerto serial, por ejemplo "COM9".
        public Dado(string puerto)
        {
            rasp = new SerialPort(puerto, 115200);

            // Define el salto de línea utilizado para identificar el final de cada mensaje recibido.
            rasp.NewLine = "\n";
            // Abre el puerto serial.
            rasp.Open();
        }

        // Lee un lanzamiento enviado por el hardware.
        public void LeerLanzamiento()
        {
            // Espera hasta recibir una línea por el puerto serial.
            string respuesta = rasp.ReadLine().Trim();


            // DADOS <IdJugador> <Dado1> <Dado2>
            // valores[0] = "DADOS"
            // valores[1] = "2"
            // valores[2] = "4"
            // valores[3] = "6"
            string[] valores = respuesta.Split(' ');

            // Convierte los valores recibidos de texto a enteros.
            IdJugador = int.Parse(valores[1]);
            Dado1 = int.Parse(valores[2]);
            Dado2 = int.Parse(valores[3]);
        }

        // Retorna la suma de los dos dados.
        public int ObtenerTotal()
        {
            return Dado1 + Dado2;
        }

        // Cierra la comunicación serial con el hardware.
        public void Cerrar()
        {
            // Verifica que el puerto esté abierto antes de cerrarlo.
            if (rasp.IsOpen)
            {
                rasp.Close();
            }
        }
    }
}