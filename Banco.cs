public class Banco
{
    public bool Transferir(object Origen, object Destino, int Monto)
    {
        if (Origen == this)
        {
            Destino.RecibirDinero(Monto);
            return true;
        }
        else if (Destino == this)
        {
            if (Origen.PagarDinero(Monto))
            {
                return true;
            }
            else
                return false;
        }
        else
        {
            if (Origen.PagarDinero(Monto))
            {
                Destino.RecibirDinero(Monto);
                return true;
            }
            else
                return false;
        }
    }

    public void DestruirJugador(int IdJugador)
    {
        
    }
}