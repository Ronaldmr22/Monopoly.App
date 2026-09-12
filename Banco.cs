using Monopoly.App;

public class Banco
{
    private List<Jugador> ListaJugadores = [];
    
    public bool Transferir(object Origen, object Destino, int Monto, string Razon)
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

    public bool ComprarPropiedad(int idJugador, int idCasilla)
    {
        Jugador jugador;
        Propiedad propiedad;

        for (int i = 0; i < 4; i++)
        {
            if (ListaJugadores[i].GetId() == idJugador)
            {
                jugador = ListaJugadores[i];
            }
        }

        for (Nodo nodo = Juego.servidor.tablerito.GetHead(); nodo.Next != Juego.servidor.tablerito.GetHead(); nodo = nodo.Next)
        {
            if (nodo.Data.NumeroCasilla == idCasilla)
            {
                propiedad = nodo.Data;
            }
        }
        if (Transferir())
        {
            
        }
    }
}