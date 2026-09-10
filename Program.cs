using System;
using System.IO;

namespace Monopoly.App
{
    class Program
    {
        static void Main(string[] args)
        {

            Tablero_LL tablerito = new Tablero_LL();

            CasillaEspecial salida= new CasillaEspecial(1,"Salida");
            Propiedad Casa1 = new Propiedad(2, 1, "Casa 1", 200, 50);
            Propiedad Casa2 = new Propiedad(3, 2, "Casa 2", 200, 50);
            Propiedad Casa3 = new Propiedad(4, 3, "Casa 3", 200, 50);
            Propiedad Casa4 = new Propiedad(5, 4, "Casa 4", 200, 50);
            Propiedad Casa5 = new Propiedad(6, 5, "Casa 5", 200, 50);
            CasillaEspecial carcel = new CasillaEspecial(7, "Carcel");
            Propiedad Casa6 = new Propiedad(8, 6, "Casa 6", 200, 50);
            CasillaEvento primerevento = new CasillaEvento(9,1);
            Propiedad Casa7 = new Propiedad(10, 7, "Casa 7", 200, 50);
            Propiedad Casa8 = new Propiedad(11, 8, "Casa 8", 200, 50);
            Propiedad Casa9 = new Propiedad(12, 9, "Casa 9", 200, 50);
            Propiedad Casa10= new Propiedad(13, 10, "Casa 10", 200, 50);
            CasillaEvento segundoevento = new CasillaEvento(14, 2);
            Propiedad Casa11 = new Propiedad(15, 11, "Casa 11", 200, 50);
            Propiedad Casa12 = new Propiedad(16, 12, "Casa 12", 200, 50);
            Propiedad Casa13 = new Propiedad(17, 13, "Casa 13", 200, 50);
            CasillaEvento tercerevento = new CasillaEvento(18, 2);
            Propiedad Casa14 = new Propiedad(19, 14, "Casa 14", 200, 50);
            Propiedad Casa15 = new Propiedad(20, 15, "Casa 15", 200, 50);
            Propiedad Casa16 = new Propiedad(21, 16, "Casa 16", 200, 50);
            CasillaEspecial libre = new CasillaEspecial(22, "Casilla Libre");
            Propiedad Casa17 = new Propiedad(23, 17, "Casa 17", 200, 50);
            Propiedad Casa18 = new Propiedad(24, 18, "Casa 18", 200, 50);


            tablerito.AgregarCasilla(salida);
            tablerito.AgregarCasilla(Casa1);
            tablerito.AgregarCasilla(Casa2);
            tablerito.AgregarCasilla(Casa3);
            tablerito.AgregarCasilla(Casa4);
            tablerito.AgregarCasilla(Casa5);
            tablerito.AgregarCasilla(carcel);
            tablerito.AgregarCasilla(Casa6);
            tablerito.AgregarCasilla(primerevento);
            tablerito.AgregarCasilla(Casa7);
            tablerito.AgregarCasilla(Casa8);
            tablerito.AgregarCasilla(Casa9);
            tablerito.AgregarCasilla(Casa10);
            tablerito.AgregarCasilla(segundoevento);
            tablerito.AgregarCasilla(Casa11);
            tablerito.AgregarCasilla(Casa12);
            tablerito.AgregarCasilla(Casa13);
            tablerito.AgregarCasilla(tercerevento);
            tablerito.AgregarCasilla(Casa14);
            tablerito.AgregarCasilla(Casa15);
            tablerito.AgregarCasilla(Casa16);
            tablerito.AgregarCasilla(libre);
            tablerito.AgregarCasilla(Casa17);
            tablerito.AgregarCasilla(Casa18);

            tablerito.ImprimirTablero();


            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}