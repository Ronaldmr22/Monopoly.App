namespace Monopoly.App
{
    public class Historial_Transacciones
    {
        public Transaccion head;
        public Transaccion tail;
        public int size;

        public Historial_Transacciones()
        {
            head = null;
            tail = null;
            size = 0;
        }

        public void InsertarTransaccion(Transaccion transaccion_nueva)
        {
            if (tail == null)
            {
                head=transaccion_nueva;
                tail=transaccion_nueva;
            }
            else
            {
                tail.Next=transaccion_nueva;
                tail.Previous=tail;
                tail=transaccion_nueva;  
            }
            size ++;
        }
    }
}
