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


class Propiedad : Casilla
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


class CasillaEspecial : Casilla
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


class CasillaEvento : Casilla
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

    public Tablero_LL()
    {
        
            CasillaEspecial salida= new CasillaEspecial(1,"Salida");
            Propiedad Casa1 = new Propiedad(2, 1, "Avenida Mediterráneo", 50, 25);
            Propiedad Casa2 = new Propiedad(3, 2, "Avenida Báltica", 50, 25);
            Propiedad Casa3 = new Propiedad(4, 3, "Avenida Oriental", 100, 50);
            Propiedad Casa4 = new Propiedad(5, 4, "Avenida Vermont", 100, 50);
            Propiedad Casa5 = new Propiedad(6, 5, "Avenida Connecticut", 120, 60);
            CasillaEspecial carcel = new CasillaEspecial(7, "Carcel");
            Propiedad Casa6 = new Propiedad(8, 6, "Plaza San Carlos", 140, 70);
            CasillaEvento primerevento = new CasillaEvento(9,1);
            Propiedad Casa7 = new Propiedad(10, 7, "Avenida Estados", 140, 70);
            Propiedad Casa8 = new Propiedad(11, 8, "Avenida Virginia", 160, 80);
            Propiedad Casa9 = new Propiedad(12, 9, "Plaza St. James", 180, 90);
            Propiedad Casa10= new Propiedad(13, 10, "Avenida Tenesse", 180, 90);
            CasillaEvento segundoevento = new CasillaEvento(14, 2);
            Propiedad Casa11 = new Propiedad(15, 11, "Avenida Nueva York", 200, 100);
            Propiedad Casa12 = new Propiedad(16, 12, "Avenida Kentucky", 220, 110);
            Propiedad Casa13 = new Propiedad(17, 13, "Avenida Pennsylvania", 220, 110);
            CasillaEvento tercerevento = new CasillaEvento(18, 2);
            Propiedad Casa14 = new Propiedad(19, 14, "Avenida Indiana", 240, 120);
            Propiedad Casa15 = new Propiedad(20, 15, "Avenida Illinois", 240, 120);
            Propiedad Casa16 = new Propiedad(21, 16, "Avenida Atlántico", 240, 120);
            CasillaEspecial libre = new CasillaEspecial(22, "Casilla Libre");
            Propiedad Casa17 = new Propiedad(23, 17, "Jardines Marvin", 260, 130);
            Propiedad Casa18 = new Propiedad(24, 18, "Plaza Park", 280, 140);


            this.AgregarCasilla(salida);
            this.AgregarCasilla(Casa1);
            this.AgregarCasilla(Casa2);
            this.AgregarCasilla(Casa3);
            this.AgregarCasilla(Casa4);
            this.AgregarCasilla(Casa5);
            this.AgregarCasilla(carcel);
            this.AgregarCasilla(Casa6);
            this.AgregarCasilla(primerevento);
            this.AgregarCasilla(Casa7);
            this.AgregarCasilla(Casa8);
            this.AgregarCasilla(Casa9);
            this.AgregarCasilla(Casa10);
            this.AgregarCasilla(segundoevento);
            this.AgregarCasilla(Casa11);
            this.AgregarCasilla(Casa12);
            this.AgregarCasilla(Casa13);
            this.AgregarCasilla(tercerevento);
            this.AgregarCasilla(Casa14);
            this.AgregarCasilla(Casa15);
            this.AgregarCasilla(Casa16);
            this.AgregarCasilla(libre);
            this.AgregarCasilla(Casa17);
            this.AgregarCasilla(Casa18);

            this.ImprimirTablero();
    }

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
        public void ImprimirTablero()//esto se puede borrar luego, es solo para ver que se agregaron las casillas
        {
            if (head == null) return;

            Nodo actual = head;
            do
            {
                // 'actual' es el Nodo, 'actual.Data' es la Casilla, 'actual.Next' es el siguiente Nodo
                Console.WriteLine($"Casilla: {actual.Data.NumeroCasilla} -> Siguiente Casilla: {actual.Next.Data.NumeroCasilla}");
                actual = actual.Next;
            } while (actual != head);
        }
}