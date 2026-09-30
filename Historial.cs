using System;
using System.IO;

namespace Monopoly.App
{
    // Clase encargada de almacenar y consultar el historial de transacciones de la partida.
    public class HistorialTransacciones
    {
        // Primer elemento de la lista doblemente enlazada.
        public Transaccion? head;

        // Último elemento de la lista doblemente enlazada.
        public Transaccion? tail;

        // Cantidad de transacciones almacenadas.
        public int size;

        // Ruta donde se guardará el archivo de texto generado.
        private static  string RutaArchivo { get; set;} = Path.Combine(Directory.GetCurrentDirectory(),"historial_transacciones.txt"); 

        // Inicializa el historial de transacciones vacío.
        public HistorialTransacciones()
        {
            head = null;
            tail = null;
            size = 0;
        }

        // Inserta una nueva transacción al final de la lista doblemente enlazada.
        public void InsertarTransaccion(Transaccion transaccionNueva)
        {
            // Si la lista está vacía, la nueva transacción será el primer y último elemento.
            if (tail == null)
            {
                head=transaccionNueva;
                tail=transaccionNueva;
            }
            else
            {
                // Conecta la nueva transacción con la última transacción de la lista.
                transaccionNueva.Previous = tail;  
                tail.Next = transaccionNueva;       
                tail = transaccionNueva;           
            }

            // Aumenta la cantidad de transacciones almacenadas.
            size ++;
        }

        // Recorre las transacciones desde la más antigua hasta la más reciente.
        public void RecorrerDesdeMasAntigua()
        {
            // Define el nombre del archivo y limpia su contenido anterior.
            GenerarNombre("Orden desde el más antiguo");
            File.WriteAllText(RutaArchivo, string.Empty);

            // Comienza el recorrido desde el inicio de la lista.
            Transaccion? actual = head;

            while (actual != null)
            {
                // Guarda la transacción actual y avanza hacia la siguiente.
                GuardarTransaccion(actual, 1);
                actual = actual.Next;
            }
        }
                
        // Recorre las transacciones desde la más reciente hasta la más antigua.
        public void RecorrerDesdeMasReciente()
        {
            // Define el nombre del archivo y limpia su contenido anterior.
            GenerarNombre("Orden desde el más reciente");
            File.WriteAllText(RutaArchivo, string.Empty);

            // Comienza el recorrido desde el final de la lista.
            Transaccion? actual = tail;

            while (actual != null)
            {
                // Guarda la transacción actual y retrocede hacia la anterior.
                GuardarTransaccion(actual, 2);
                actual = actual.Previous;
            }
        }

        // Busca las transacciones en las que participa un jugador específico.
        public void BuscarPorJugador(string nombreJugador)
        {
            // Define el archivo donde se guardará el resultado y limpia su contenido.
            GenerarNombre("Búsqueda por jugador");
            File.WriteAllText(RutaArchivo, string.Empty);

            // Comienza la búsqueda desde el inicio de la lista.
            Transaccion? actual =head;

            while (actual != null)
            {
                // Comprueba si el jugador aparece como origen o destino de la transacción.
                if (string.Equals(actual.Origen, nombreJugador, StringComparison.OrdinalIgnoreCase)
                    || string.Equals(actual.Destino, nombreJugador, StringComparison.OrdinalIgnoreCase))
                {
                    GuardarTransaccion(actual, 3);
                }

                // Avanza hacia la siguiente transacción.
                actual = actual.Next;
            }
        }

        // Busca todas las transacciones que coincidan con un tipo específico.
        public void BuscarPorTipo(string tipo)
        {
            // Define el archivo donde se guardará el resultado y limpia su contenido.
            GenerarNombre("Búsqueda por tipo");
            File.WriteAllText(RutaArchivo, string.Empty);

            // Comienza la búsqueda desde el inicio de la lista.
            Transaccion? actual =head;

            while(actual != null)
            {
                // Comprueba si el tipo de la transacción coincide con el solicitado.
                if (actual.GetTipo() == tipo)
                {
                    GuardarTransaccion(actual,4);
                }

                // Avanza hacia la siguiente transacción.
                actual=actual.Next;
            }
        }

        // Guarda todas las transacciones registradas en un archivo.
        public void TodasLasTransacciones()
        {
            // Define el nombre del archivo y limpia su contenido anterior.
            GenerarNombre("Todas las transacciones");
            File.WriteAllText(RutaArchivo, string.Empty);

            // Comienza el recorrido desde el inicio de la lista.
            Transaccion? actual=head;

            while(actual != null)
            {
                // Guarda cada transacción y continúa con la siguiente.
                GuardarTransaccion(actual,5);
                actual=actual.Next;
            }
        }

        // Guarda una transacción en el archivo correspondiente.
        private void GuardarTransaccion(Transaccion transaccionNueva,int identificador)
        {
            // Utiliza el identificador para determinar el nombre del archivo.
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

            // Convierte los datos de la transacción en una línea de texto separada por "|".
            string texto = $"{transaccionNueva.Id}|{transaccionNueva.FechaHora}|{transaccionNueva.Turno}|{transaccionNueva.Tipo}|{transaccionNueva.Origen}|{transaccionNueva.Destino}|{transaccionNueva.Monto}|{transaccionNueva.Descripcion}";

            // Agrega la transacción al final del archivo de texto.
            File.AppendAllText(RutaArchivo, texto + Environment.NewLine);
        }

        // Genera la ruta y el nombre del archivo donde se guardarán las transacciones.
        private void GenerarNombre(string nombreTxt)
        {
            // Obtiene la ruta principal del proyecto.
            string proyecto = Path.GetFullPath(
                Path.Combine(AppContext.BaseDirectory, "..", "..", "..")
            );

            // Crea la carpeta "Transacciones" si todavía no existe.
            string carpeta = Path.Combine(proyecto, "Transacciones");
            Directory.CreateDirectory(carpeta);

            // Define la ruta final utilizando el nombre recibido.
            RutaArchivo = Path.Combine(carpeta, $"{nombreTxt}.txt");
        }
    }
}