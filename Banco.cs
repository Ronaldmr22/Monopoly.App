using Monopoly.App;

public class Banco
{
    public List<Jugador> ListaJugadores = [];
    public int IdTransaccion = 1;
    
    public bool Transferir(object Origen, object Destino, int Monto, string tipo, string Razon)
    {
        if (Destino is Jugador jugadorD)
        {
            if (Origen is Jugador jugadorO)
            {
                if (jugadorO.PagarDinero(Monto))
                {
                    Transaccion transaccion1 = new Transaccion(IdTransaccion, DateTime.Now, Juego.turno, tipo, jugadorO.GetNombre(), jugadorD.GetNombre(), Monto, Razon);
                    Juego.servidor.GetHistorialTransacciones().InsertarTransaccion(transaccion1);
                    jugadorD.RecibirDinero(Monto);
                    return true;
                }
                else
                {
                    Transaccion transaccion1 = new Transaccion(IdTransaccion, DateTime.Now, Juego.turno, tipo, jugadorO.GetNombre(), jugadorD.GetNombre(), jugadorO.GetSaldo(), Razon);
                    Juego.servidor.GetHistorialTransacciones().InsertarTransaccion(transaccion1);
                    jugadorD.RecibirDinero(jugadorO.GetSaldo());
                    DestruirJugador(jugadorO);
                    return false;
                }
            }
            Transaccion transaccion = new Transaccion(IdTransaccion, DateTime.Now, Juego.turno, tipo, "Banco", jugadorD.GetNombre(), Monto, Razon);
            Juego.servidor.GetHistorialTransacciones().InsertarTransaccion(transaccion);
            jugadorD.RecibirDinero(Monto);
            return true;
        }
        else
        {
            if (Origen is Jugador jugadorO){ 
                if (jugadorO.PagarDinero(Monto))
                {
                    Transaccion transaccion = new Transaccion(IdTransaccion, DateTime.Now, Juego.turno, tipo, jugadorO.GetNombre(), "Banco", Monto, Razon);
                    Juego.servidor.GetHistorialTransacciones().InsertarTransaccion(transaccion);
                    return true;
                }
                else
                {
                    Transaccion transaccion = new Transaccion(IdTransaccion, DateTime.Now, Juego.turno, tipo, jugadorO.GetNombre(), "Banco", jugadorO.GetSaldo(), Razon);
                    Juego.servidor.GetHistorialTransacciones().InsertarTransaccion(transaccion);
                    DestruirJugador(jugadorO);
                    return false;
                }
            }
        }
        return false;
    }

    public void DestruirJugador(Jugador jugador)
    {

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

        for (Nodo nodo = Juego.servidor.GetTablero().GetHead(); nodo.Next != Juego.servidor.GetTablero().GetHead(); nodo = nodo.Next)
        {
            if (nodo.Data.NumeroCasilla == idCasilla && nodo.Data is Propiedad propiedadObjetivo)
            {
                propiedad = propiedadObjetivo;
            }
        }
        if (jugador.GetSaldo() < propiedad.Precio)
        {
            return false;
        }
        else
        {
            Transferir(jugador, Juego.servidor.GetBanco(), propiedad.Precio, "Compra de propiedad", $"{jugador.GetNombre()} ha comprado la propiedad {propiedad.Nombre} por {propiedad.Precio}");
            return true;
        }
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
        for (Nodo nodo = Juego.servidor.GetTablero().GetHead(); nodo.Next != Juego.servidor.GetTablero().GetHead(); nodo = nodo.Next)
        {
            if (nodo.Data.NumeroCasilla == idPropiedad && nodo.Data is Propiedad propiedadObjetivo)
            {
                propiedad = propiedadObjetivo;
            }
        }
        Transferir(jugador, Juego.servidor.GetBanco(), propiedad.Precio, "Cobro de alquiler", $"{jugador.GetNombre()} le ha pagado renta a {dueño.GetNombre()} por una cantidad de {propiedad.Precio}");
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

    public int TirarDados(int jugador)
    {
        Dados dados = new Dados("COM5");
        return dados.Lanzar(jugador);

    }

}