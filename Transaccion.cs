using System;
using System.IO;

namespace Monopoly.App

{
    public class Transaccion
    {
        public int Id { get; set; }
        public DateTime FechaHora { get; set; }
        public int Turno { get; set; }
        public string Tipo { get; set; }
        public string Origen { get; set; }
        public string Destino { get; set; }
        public double Monto { get; set; }
        public string Descripcion { get; set; }

        public Transaccion(int id, DateTime fechaHora, int turno,string tipo, string origen, string destino,double monto, string descripcion)
        {
            Id = id;
            FechaHora = fechaHora;
            Turno = turno;
            Tipo = tipo;
            Origen = origen;
            Destino = destino;
            Monto = monto;
            Descripcion = descripcion;
        }

        public void GuardarEnTxt()
        {
            string ruta = "transacciones.txt";

            string transaccion =
                $"ID: {Id} | " +
                $"Fecha y hora: {FechaHora} | " +
                $"Turno: {Turno} | " +
                $"Tipo: {Tipo} | " +
                $"Origen: {Origen} | " +
                $"Destino: {Destino} | " +
                $"Monto: {Monto} | " +
                $"Descripción: {Descripcion}";

            File.AppendAllText(ruta, transaccion + Environment.NewLine);
        }
    }
}