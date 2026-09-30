public class Casilla
{
    protected int numeroCasilla;

    public Casilla(int numeroCasilla)
    {
        this.numeroCasilla = numeroCasilla;
    }

    public int NumeroCasilla
    {
        get{ return this.numeroCasilla;}
    }

    public void Explotar()
    {
        Console.WriteLine("La casilla explotó");
    }
}


public class Propiedad : Casilla
{
    private int id_propiedad;
    private String nombre;
    private int precio;
    private int alquiler;
    // Propietario propietario;

    public Propiedad(int numeroCasilla, int id_propiedad, String nombre, int precio, int alquiler)
    : base(numeroCasilla)
    {
        this.id_propiedad = id_propiedad;
        this.nombre = nombre;
        this.precio = precio;
        this.alquiler = alquiler;
    }
    public int Precio
    {
        get
        {return this.precio; }
    }
    public int Alquiler
    {
        get
        { return this.alquiler;}
    }
    public string Nombre
    {
        get
        {return this.nombre;}
    }
    public int IdPropiedad
    {
        get
        {return this.id_propiedad;}
    }
}


public class CasillaEspecial : Casilla
{
    private string nombre;
    public CasillaEspecial(int numeroCasilla, String nombre)
    : base(numeroCasilla)
    {
        this.nombre = nombre;
    }

    public String Nombre
    {
        get
        { return this.nombre;}
    }
}


public class CasillaEvento : Casilla
{
    private int idEvento;
    public CasillaEvento(int numeroCasilla, int idEvento)
    : base(numeroCasilla)
    {
        this.idEvento = idEvento;
    }
    public int IdEvento
    {
        get
        {return this.idEvento;}
    }
}
public class Nodo
{
    private Casilla data;
    private Nodo? next;
    private Nodo? previous;

    public Nodo(Casilla casilla)
    {
        data = casilla;
        next = null;
        previous = null;
    }
    public Casilla Data
    {
        get { return this.data; }

    }
    public Nodo Next
    {
        get { return this.next; }
        set { this.next = value; }
    }
    public Nodo Previous
    {
        get { return this.previous; }
        set { this.previous = value; }
    }

}

public class Tablero_LL
{
    private Nodo? head;

    

    public Nodo GetHead()
    {
        return head;
    }

    public void AgregarCasilla(Casilla casilla)
    {
        Nodo nodito = new Nodo(casilla);
        if (head == null)
        {
            nodito.Next = nodito;
            nodito.Previous = nodito;
            head = nodito;
            return;
        }
        Nodo tail = head.Previous;
        nodito.Next = head;
        nodito.Previous = tail;
        tail.Next = nodito;
        head.Previous = nodito;


    }

    public Casilla? BuscarCasilla(int numCasilla)
    {
        if (head == null)
        {
            return null;
        }

        Nodo actual = head;

        do
        {
            if (actual.Data.NumeroCasilla == numCasilla)
            {
                return actual.Data;
            }

            actual = actual.Next;

        } while (actual != head);

        return null;
    }
}
