using Monopoly.App;

class Juego
{
    public static Servidor? servidor = null;


    public static void CrearServidor()
    {
        Tablero_LL tablerito = new Tablero_LL();
        Banco banco = new Banco();
        servidor = new Servidor(1, banco, tablerito);
    }
}