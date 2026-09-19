using System.IO.Ports;

namespace Monopoly.App
{
    public class Dado
    {
        public int IdJugador { get; private set; }
        public int Dado1 { get; private set; }
        public int Dado2 { get; private set; }

        private SerialPort arduino;

        public Dado(string puerto)
        {
            arduino = new SerialPort(puerto, 115200);
            arduino.NewLine = "\n";
            arduino.Open();
        }

        public void LeerLanzamiento()
        {
            string respuesta = arduino.ReadLine().Trim();

            // DADOS 2 4 6
            string[] valores = respuesta.Split(' ');

            IdJugador = int.Parse(valores[1]);
            Dado1 = int.Parse(valores[2]);
            Dado2 = int.Parse(valores[3]);
        }

        public int ObtenerTotal()
        {
            return Dado1 + Dado2;
        }

        public void Cerrar()
        {
            if (arduino.IsOpen)
            {
                arduino.Close();
            }
        }
    }
}