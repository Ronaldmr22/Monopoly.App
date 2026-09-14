using System;

namespace Monopoly.App
{
    public class Transaccion
    {
        public int Id;
        public DateTime FechaHora;
        public int Turno;  
        public string Tipo;
        public string Origen;
        public string Destino;
        public int Monto;
        public string Descripcion;
        public Transaccion? Next;
        public Transaccion? Previous;
        public bool Mostrada;

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
            Next = null;
            Previous = null;
            Mostrada=false;

        }
        public DateTime GetFechaHora()
        {
            return this.FechaHora;
        }
        public int GetJugador()
        {
            return this.Id;
        }
        public string GetTipo()
        {
            return this.Tipo;
        }
        public Transaccion? GetNext()
        {
            return this.Next;
        }
        public Transaccion? GetPrevious()
        {
            return this.Previous;
        }

    }
}