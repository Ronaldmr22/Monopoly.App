using Monopoly.App;

public class Banco
{
    public List<Jugador> ListaJugadores = [];
    
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
                {
                    jugadorD.RecibirDinero(jugadorO.GetSaldo());
                    DestruirJugador(jugadorO.GetId());
                    return false;
                }
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
        Jugador? jugador = null;

        for (int i = 0; i < 4; i++)
        {
            if (ListaJugadores[i].GetId() == IdJugador)
            {
                jugador = ListaJugadores[i];
                break;
            }
        }

        ListaJugadores.Remove(jugador);
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
                break;
            }
        }

        for (Nodo nodo = Juego.servidor.tablerito.GetHead(); nodo.Next != Juego.servidor.tablerito.GetHead(); nodo = nodo.Next)
        {
            if (nodo.Data.NumeroCasilla == idCasilla && nodo.Data is Propiedad propiedadObjetivo)
            {
                propiedad = propiedadObjetivo;
            }
        }
        if (Transferir(jugador, Juego.servidor.GetBanco(), propiedad.Precio, $"{jugador.GetNombre()} ha comprado la propiedad {propiedad.Nombre} por {propiedad.Precio}"))
        {
            return true;
        }
        return false;
    }

    public int CobrarAlquiler(int idJugador, int idPropiedad, int idDueño)
    {
        Jugador? jugador = null;
        Propiedad? propiedad = null;
        Jugador? dueño = null;

        for (int i = 0; i < 4; i++)
        {
            if (ListaJugadores[i].GetId() == idJugador)
            {
                jugador = ListaJugadores[i];
                break;
            }
        }
        for (int i = 0; i < 4; i++)
        {
            if (ListaJugadores[i].GetId() == idDueño)
            {
                dueño = ListaJugadores[i];
                break;
            }
        }
        for (Nodo nodo = Juego.servidor.tablerito.GetHead(); nodo.Next != Juego.servidor.tablerito.GetHead(); nodo = nodo.Next)
        {
            if (nodo.Data.NumeroCasilla == idPropiedad && nodo.Data is Propiedad propiedadObjetivo)
            {
                propiedad = propiedadObjetivo;
            }
        }
        Transferir(jugador, Juego.servidor.GetBanco(), propiedad.Precio, $"{jugador.GetNombre()} le ha pagado renta a {dueño.GetNombre()} por una cantidad de {propiedad.Precio}");
        return jugador.GetSaldo();
    }

    public void AgregarJugador(string nombreJugador, int id)
    {
        if (ListaJugadores.Count() < 4)
        {
            Jugador jugador = new Jugador(id, nombreJugador);
            ListaJugadores.Add(jugador);
        }
        else
        {
            ///Se ha alcanzado el máximo de jugadores, no se puede agregar más
        }
    }

    public string Getinfo()
    {
        return "1";
    }
}