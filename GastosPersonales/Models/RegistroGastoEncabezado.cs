using System.ComponentModel.DataAnnotations.Schema;

namespace GastosPersonales.Models
{
    //[Table("RegistroGastoEncabezado")]
    public class RegistroGastoEncabezado
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public int FondoMonetarioId { get; set; }
        public FondoMonetario FondoMonetario { get; set; }

        public string Observaciones { get; set; }
        public string NombreComercio { get; set; }
        public string TipoDocumento { get; set; } // Comprobante, Factura, Otro

        public string UserId { get; set; }  // Relacionado con IdentityUser

        public List<RegistroGastoDetalle> Detalles { get; set; }
    }
}
