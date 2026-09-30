using System;

namespace Monopoly.App
{
    // Clase que representa una transacción realizada durante la partida.
    public class Transaccion
    {
        // Datos que identifican y describen la transacción.
        public int Id;
        public DateTime FechaHora;
        public int Turno;  
        public string Tipo;
        public string Origen;
        public string Destino;
        public int Monto;
        public string Descripcion;

        // Referencias utilizadas para formar la lista doblemente enlazada de transacciones.
        public Transaccion? Next;
        public Transaccion? Previous;

        // Indica si la transacción ya fue mostrada.
        public bool Mostrada;

        // Constructor que recibe y almacena toda la información de la transacción.
        public Transaccion(int id, DateTime fechaHora, int turno,string tipo, string origen, string destino,int monto, string descripcion)
        {
            Id = id;
            FechaHora = fechaHora;
            Turno = turno;
            Tipo = tipo;
            Origen = origen;
            Destino = destino;
            Monto = monto;
            Descripcion = descripcion;

            // Al crear la transacción todavía no está conectada con otras transacciones.
            Next = null;
            Previous = null;

            // Inicialmente la transacción no ha sido mostrada.
            Mostrada=false;

        }

        // Devuelve la fecha y hora en que se realizó la transacción.
        public DateTime GetFechaHora()
        {
            return this.FechaHora;
        }

        // Devuelve el identificador de la transacción.
        public int GetJugador()
        {
            return this.Id;
        }

        // Devuelve el tipo de transacción realizada.
        public string GetTipo()
        {
            return this.Tipo;
        }

        // Devuelve la siguiente transacción de la lista doblemente enlazada.
        public Transaccion? GetNext()
        {
            return this.Next;
        }

        // Devuelve la transacción anterior de la lista doblemente enlazada.
        public Transaccion? GetPrevious()
        {
            return this.Previous;
        }

    }
}