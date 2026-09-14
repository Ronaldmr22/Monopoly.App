using System;
using System.IO;

namespace Monopoly.App
{
    public class HistorialTransacciones
    {
        public Transaccion? head;
        public Transaccion? tail;
        public int size;

        private static  string RutaArchivo { get; set;} = Path.Combine(Directory.GetCurrentDirectory(),"historial_transacciones.txt"); 

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
        }

        public void RecorrerDesdeMasAntigua()
        {
            Transaccion? actual = tail;
            while (actual != null)
            {
                GuardarTransaccion(actual,1);
                actual=actual.Previous;
            }
        }
        
        public void RecorrerDesdeMasReciente()
        {
            Transaccion? actual = head;
            while (actual != null)
            {
                GuardarTransaccion(actual,2);
                actual=actual.Next;
            }
        }

        public void BuscarPorJugador(int jugador)
        {
            Transaccion? actual =head;
            while (actual != null)
            {
                if (actual.GetJugador() == jugador)
                {
                    GuardarTransaccion(actual,3);
                }
                actual=actual.Next;
            }
        }

        public void BuscarPorTipo(string tipo)
        {
            Transaccion? actual =head;
            while(actual != null)
            {
                if (actual.GetTipo() == tipo)
                {
                    GuardarTransaccion(actual,2);
                }
                actual=actual.Next;
            }
        }

        public void TodasLasTransacciones()
        {
            Transaccion? actual=head;
            while(actual != null)
            {
                GuardarTransaccion(actual,5);
                actual=actual.Next;
            }
        }
        private void GuardarTransaccion(Transaccion transaccionNueva,int identificador)
        {
            switch (identificador)
            {
                case 1:
                    GenerarNombre("Orden desde el más antiguo");
                    break;
                
                case 2:
                    GenerarNombre("Orden desde el más reciente");
                    break;

                case 3:
                    GenerarNombre("Búsqueda por jugador");
                    break;
                
                case 4:
                    GenerarNombre("Búsqueda por tipo");
                    break;
                
                case 5:
                    GenerarNombre("Todas las transacciones");
                    break;
                default:
                    GenerarNombre("Historial");
                    break;
                       
            
            
            }
            string texto = $"{transaccionNueva.Id}|{transaccionNueva.FechaHora}|{transaccionNueva.Turno}|{transaccionNueva.Tipo}|{transaccionNueva.Origen}|{transaccionNueva.Destino}|{transaccionNueva.Monto}|{transaccionNueva.Descripcion}";
            File.AppendAllText(RutaArchivo, texto + Environment.NewLine);
        }

        private void GenerarNombre(string nombreTxt)
        {
            RutaArchivo=Path.Combine(Directory.GetCurrentDirectory(),nombreTxt); 
        }
    }
}
