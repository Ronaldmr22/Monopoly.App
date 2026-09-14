using System.IO.Ports;

public class Dados
{
    private SerialPort serial;
    public int resultado {get; private set;}

    public Dados(string puerto)
    {
        serial = new SerialPort(puerto, 115200);
        serial.NewLine = "\n";
        serial.Open();
    }

    public int Lanzar(int jugador)
    {
        serial.WriteLine(jugador.ToString());
        string respuesta = serial.ReadLine().Trim();
        return int.Parse(respuesta);
    }
}