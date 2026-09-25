namespace Monopoly.App
{
    public class CartaEvento
    {
        private int id_carta;
        private string descripcion;
        

        public CartaEvento(int id_carta, string descripcion)
        {
            this.id_carta = id_carta;
    
            this.descripcion = descripcion;
        }
        public virtual void EjecutarEvento(Jugador jugador, Banco banco)
        {
            Console.WriteLine("Se ejecutó el evento de la carta: " + this.descripcion);
        }
        

        public int IdCarta
        {
            get { return this.id_carta; }
        }

        public string Descripcion
        {
            get { return this.descripcion; }
        }

       
    }
     public class CartaGanarDinero : CartaEvento
    {
        private int monto;
    public CartaGanarDinero(int id_carta, string descripcion, int monto)
    : base(id_carta, descripcion)
    {
        this.monto = monto;
    }
    public int Monto
      {
        get
        {return this.monto; }
      }
    public override void EjecutarEvento(Jugador jugador, Banco banco)
    {
    banco.Transferir(banco, jugador, this.monto, "Jugador gana dinero", "Evento de carta");
    }
}

 public class CartaPerderDinero : CartaEvento
    {
        private int monto;
    public CartaPerderDinero(int id_carta, string descripcion, int monto)
    : base(id_carta, descripcion)
    {
        this.monto = monto;
    }
    public int Monto
      {
        get
        {return this.monto; }
      }
    public override void EjecutarEvento(Jugador jugador, Banco banco)
    {
        banco.Transferir(jugador, banco, this.monto, "Jugador pierde dinero", "Evento de carta");
    }
  }
   public class CartaPerderTurno : CartaEvento
    {
        private int monto;
    public CartaPerderTurno(int id_carta, string descripcion)
    : base(id_carta, descripcion)
    {
      
    }
   
    public override void EjecutarEvento(Jugador jugador, Banco banco)
    {
        jugador.SetTurnoPerdido(true);
    }
  

  public class CartaMoverseDeCasilla : CartaEvento
    {
        private int movimiento;
    public CartaMoverseDeCasilla(int id_carta, string descripcion, int movimiento)
    : base(id_carta, descripcion)
    {
      this.movimiento = movimiento;
    }
   public int Movimiento
      {
        get
        {return this.movimiento; }
      }
    public override void EjecutarEvento(Jugador jugador, Banco banco)
    {
        banco.MoverJugador(jugador.GetId(), this.movimiento);
          }
    }
  }
}