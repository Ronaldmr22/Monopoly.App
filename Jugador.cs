using Monopoly.App;

public class Jugador{
    private int Id;
    private string Nombre;
    private int Saldo = 0;
    private int Posicion = 0;
    private bool Estado = false;
    private List<Object> Propiedades = [];
    private bool TurnoPerdido = false;

    public Jugador(int Id, string Nombre)
    {
        this.Id = Id;
        this.Nombre = Nombre;
    }

        public string GetNombre()
    {
        return this.Nombre;
    }

    public int GetId()
    {
        return this.Id;
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

    public bool PagarDinero(int Dinero)
    {
        if (this.Saldo > Dinero)
        {
            this.Saldo -= Dinero;
            return true;
        }
        return false;
    }

    public void AgregarPropiedad(Object Propiedad)
    {
        Propiedades.Add(Propiedad);
    }

    public void RecibirDinero(int Dinero)
    {
        this.Saldo += Dinero;
    }
}
