namespace GastosPersonales.Models
{
    public class Presupuesto
    {
        public int Id { get; set; }
        public string UserId { get; set; }  // Relacionado con IdentityUser
        public int TipoGastoId { get; set; }
        public TipoGasto TipoGasto { get; set; }

        public decimal Monto { get; set; }
        public int Mes { get; set; }   // Mes numérico (1-12)
        public int Año { get; set; }

        // Puedes agregar navegación al usuario si quieres
        // public IdentityUser User { get; set; }
    }
}
