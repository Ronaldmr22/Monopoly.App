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

    public void Lanzar(string jugador)
    {
        serial.WriteLine(jugador);
        string respuesta = serial.ReadLine().Trim();
        resultado =  int.Parse(respuesta);
    }
}