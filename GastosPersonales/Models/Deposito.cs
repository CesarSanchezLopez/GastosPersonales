namespace GastosPersonales.Models
{
    public class Deposito
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public int FondoMonetarioId { get; set; }
        public FondoMonetario FondoMonetario { get; set; }

        public decimal Monto { get; set; }
        public string UserId { get; set; }  // Relacionado con IdentityUser
    }
}
