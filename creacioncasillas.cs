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
            Propiedad Casa1 = new Propiedad(2, 1, "Avenida Mediterráneo", 50, 25);
            Propiedad Casa2 = new Propiedad(3, 2, "Avenida Báltica", 50, 25);
            Propiedad Casa3 = new Propiedad(4, 3, "Avenida Oriental", 100, 50);
            Propiedad Casa4 = new Propiedad(5, 4, "Avenida Vermont", 100, 50);
            Propiedad Casa5 = new Propiedad(6, 5, "Avenida Connecticut", 120, 60);
            CasillaEspecial carcel = new CasillaEspecial(7, "Carcel");
            Propiedad Casa6 = new Propiedad(8, 6, "Plaza San Carlos", 140, 70);
            CasillaEvento primerevento = new CasillaEvento(9,1);
            Propiedad Casa7 = new Propiedad(10, 7, "Avenida Estados", 140, 70);
            Propiedad Casa8 = new Propiedad(11, 8, "Avenida Virginia", 160, 80);
            Propiedad Casa9 = new Propiedad(12, 9, "Plaza St. James", 180, 90);
            Propiedad Casa10= new Propiedad(13, 10, "Avenida Tenesse", 180, 90);
            CasillaEvento segundoevento = new CasillaEvento(14, 2);
            Propiedad Casa11 = new Propiedad(15, 11, "Avenida Nueva York", 200, 100);
            Propiedad Casa12 = new Propiedad(16, 12, "Avenida Kentucky", 220, 110);
            Propiedad Casa13 = new Propiedad(17, 13, "Avenida Pennsylvania", 220, 110);
            CasillaEvento tercerevento = new CasillaEvento(18, 2);
            Propiedad Casa14 = new Propiedad(19, 14, "Avenida Indiana", 240, 120);
            Propiedad Casa15 = new Propiedad(20, 15, "Avenida Illinois", 240, 120);
            Propiedad Casa16 = new Propiedad(21, 16, "Avenida Atlántico", 240, 120);
            CasillaEspecial libre = new CasillaEspecial(22, "Casilla Libre");
            Propiedad Casa17 = new Propiedad(23, 17, "Jardines Marvin", 260, 130);
            Propiedad Casa18 = new Propiedad(24, 18, "Plaza Park", 280, 140);


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