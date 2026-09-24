namespace Monopoly.App
{
    public class NodoPropiedad
    {
        public Propiedad Dato;
        public NodoPropiedad Siguiente;

        public NodoPropiedad(Propiedad propiedad)
        {
            Dato = propiedad;
            Siguiente = null;
        }
    }


    public class ListaPropiedades
    {
        private NodoPropiedad head;
        private NodoPropiedad tail;
        private int size;

        public ListaPropiedades()
        {
            head = null;
            tail = null;
            size = 0;
        }


        public void InsertarFinal(Propiedad propiedad)
        {
            NodoPropiedad nuevo = new NodoPropiedad(propiedad);

            if (head == null)
            {
                head = nuevo;
                tail = nuevo;
            }
            else
            {
                tail.Siguiente = nuevo;
                tail = nuevo;
            }

            size++;
        }

        public bool TienePropiedad(int idPropiedad)
        {
            NodoPropiedad actual = head;
            while (actual != null)
            {
                if (actual.Dato.IdPropiedad == idPropiedad)
                {
                    return true;
                }
                actual = actual.Siguiente;
            }
            return false;
        }


        public int GetSize()
        {
            return size;
        }


        public bool EstaVacia()
        {
            return head == null;
        }


        public void Imprimir()
        {
            NodoPropiedad actual = head;

            while (actual != null)
            {
                Console.WriteLine(actual.Dato);
                actual = actual.Siguiente;
            }
        }
    }
}