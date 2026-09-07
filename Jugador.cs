using Monopoly.App;

public class Jugador{
    private int Id;
    private string Nombre;
    private int Saldo = 0;
    private int Posicion = 0;
    private bool Estado = false;
    private List<Object> Propiedades = [];

    public Jugador(int Id, string Nombre)
    {
        this.Id = Id;
        this.Nombre = Nombre;
    }

    public int GetSaldo()
    {
        return this.Saldo;
    }

    public void SetSaldo(int Saldo)
    {
        this.Saldo = Saldo;
    }

    public int GetPosicion()
    {
        return this.Posicion;
    }

    public void SetPosicion(int Posicion)
    {
        this.Posicion = Posicion;
    }

    public bool GetEstado()
    {
        return this.Estado;
    }

    public void SetEstado(bool Estado)
    {
        this.Estado = Estado;
    }

    public List<Object> GetPropiedades()
    {
        return this.Propiedades;
    }

    public void SetPropiedades(List<Object> Propiedades)
    {
        this.Propiedades = Propiedades;
    }

    public string GetInfo()
    {
        return $"Nombre: {Nombre}, Id: {Id}, Saldo: {Saldo}, Posicion: {Posicion}, Estado: {Estado}, Propiedades: {Propiedades}";
    }

    public static void CrearJugador(int Id, string Nombre)
    {
        if (Servidor.GetJugador(3) == null)
        {
            Jugador jugador = new Jugador(Id, Nombre);
            Servidor.AgregarJugador(jugador);
        }
        else
        {
            ///Enviar un error que se muestre en la pantalla diciendo que solo cuatro jugadores son posibles
        }
    }

    public bool PagarAlquiler(int Cobro)
    {
        if (this.Saldo > Cobro)
        {
            this.Saldo -= Cobro;
            return true;
        }
        return false;
    }

    public void AgregarPropiedad(Object Propiedad)
    {
        Propiedades.Add(Propiedad);
    }
}
