using Monopoly.App;

public class Banco
{
    private List<Jugador> ListaJugadores = [];
    
    public bool Transferir(object Origen, object Destino, int Monto, string Razon)
    {
        if (Destino is Jugador jugadorD)
        {
            if (Origen is Jugador jugadorO)
            {
                if (jugadorO.PagarDinero(Monto))
                {
                    jugadorD.RecibirDinero(Monto);
                    return true;
                }
                else
                    return false;
            }
            jugadorD.RecibirDinero(Monto);
            return true;
        }
        else
        {
            if (Origen is Jugador jugadorO){
                if (jugadorO.PagarDinero(Monto))
                {
                    return true;
                }
                else
                    return false;
            }
        }
        return false;
    }

    public void DestruirJugador(int IdJugador)
    {
        
    }

    public bool ComprarPropiedad(int idJugador, int idCasilla)
    {
        Jugador? jugador = null;
        Propiedad? propiedad = null;

        for (int i = 0; i < 4; i++)
        {
            if (ListaJugadores[i].GetId() == idJugador)
            {
                jugador = ListaJugadores[i];
            }
        }

        for (Nodo nodo = Juego.servidor.tablerito.GetHead(); nodo.Next != Juego.servidor.tablerito.GetHead(); nodo = nodo.Next)
        {
            if (nodo.Data.NumeroCasilla == idCasilla && nodo.Data is Propiedad propiedadObjetivo)
            {
                propiedad = propiedadObjetivo;
            }
        }
        if (Transferir(jugador, Juego.servidor.GetBanco(), propiedad.Precio, $"{jugador} ha comprado la propiedad {propiedad.Nombre} por {propiedad.Precio}"))
        {
            return true;
        }
        return false;
    }
}