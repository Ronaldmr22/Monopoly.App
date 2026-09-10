using System;
using System.IO.Ports;

namespace Monopoly.App
{
    public class Dado
    {
        public int Dado1 { get; private set; }
        public int Dado2 { get; private set; }

        private SerialPort arduino;

        public Dado(string puerto)
        {
            arduino = new SerialPort(puerto, 9600);
            arduino.Open();
        }

        public void Lanzar()
        {
            arduino.WriteLine("TIRAR");

            string respuesta = arduino.ReadLine();

            string[] valores = respuesta.Split(' ');

            Dado1 = int.Parse(valores[0]);
            Dado2 = int.Parse(valores[1]);
        }

        public int ObtenerTotal()
        {
            return Dado1 + Dado2;
        }
    }
}