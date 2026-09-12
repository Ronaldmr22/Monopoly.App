using System;
using System.IO;

namespace Monopoly.App
{
    public class HistorialTransacciones
    {
        public Transaccion? head;
        public Transaccion? tail;
        public int size;

        private static readonly string RutaArchivo = Path.Combine(Directory.GetCurrentDirectory(),"historial_transacciones.txt");

        public HistorialTransacciones()
        {
            head = null;
            tail = null;
            size = 0;
        }

        public void InsertarTransaccion(Transaccion transaccionNueva)
        {
            if (tail == null)
            {
                head=transaccionNueva;
                tail=transaccionNueva;
            }
            else
            {
                transaccionNueva.Previous = tail;  
                tail.Next = transaccionNueva;       
                tail = transaccionNueva;            
            }
            size ++;
            GuardarTransaccion(transaccionNueva);
            
        }
        private void GuardarTransaccion(Transaccion transaccionNueva)
        {
            string texto = $"{transaccionNueva.Id}|{transaccionNueva.FechaHora}|{transaccionNueva.Turno}|{transaccionNueva.Tipo}|{transaccionNueva.Origen}|{transaccionNueva.Destino}|{transaccionNueva.Monto}|{transaccionNueva.Descripcion}";
            File.AppendAllText(RutaArchivo, texto + Environment.NewLine);
        }
        
    }
}
