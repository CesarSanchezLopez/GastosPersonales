using System.ComponentModel.DataAnnotations.Schema;

namespace GastosPersonales.Models
{
    //[Table("RegistroGastoDetalle")]
    public class RegistroGastoDetalle
    {
        public int Id { get; set; }
        public int RegistroGastoEncabezadoId { get; set; }
        public RegistroGastoEncabezado RegistroGastoEncabezado { get; set; }

        public int TipoGastoId { get; set; }
        public TipoGasto TipoGasto { get; set; }

        public decimal Monto { get; set; }
    }

}
