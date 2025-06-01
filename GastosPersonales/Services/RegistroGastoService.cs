using GastosPersonales.Data;
using GastosPersonales.Models;
using Microsoft.EntityFrameworkCore;

namespace GastosPersonales.Services
{
    public class RegistroGastoService
    {
        private readonly ApplicationDbContext _context;

        public RegistroGastoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> AddAsync(RegistroGastoEncabezado encabezado)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var errores = new List<string>();

                foreach (var detalle in encabezado.Detalles)
                {
                    var presupuesto = await _context.Presupuestos.FirstOrDefaultAsync(p =>
                        p.UserId == encabezado.UserId &&
                        p.TipoGastoId == detalle.TipoGastoId &&
                        p.Mes == encabezado.Fecha.Month &&
                        p.Año == encabezado.Fecha.Year);

                    var totalGastado = await _context.RegistroGastoDetalles
                        .Include(d => d.RegistroGastoEncabezado)
                        .Where(d => d.TipoGastoId == detalle.TipoGastoId &&
                                    d.RegistroGastoEncabezado.UserId == encabezado.UserId &&
                                    d.RegistroGastoEncabezado.Fecha.Month == encabezado.Fecha.Month &&
                                    d.RegistroGastoEncabezado.Fecha.Year == encabezado.Fecha.Year)
                        .SumAsync(d => d.Monto);

                    var disponible = presupuesto?.Monto ?? 0;
                    var futuroTotal = totalGastado + detalle.Monto;

                    if (futuroTotal > disponible)
                    {
                        errores.Add($"Presupuesto sobregirado para {detalle.TipoGasto?.Descripcion ?? "tipo de gasto ID " + detalle.TipoGastoId}: " +
                                    $"Presupuesto: {disponible}, Gastado + nuevo: {futuroTotal}");
                    }
                }

                if (errores.Any())
                    return string.Join("\n", errores);

                _context.RegistroGastoEncabezados.Add(encabezado);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return "OK";
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<List<RegistroGastoEncabezado>> GetAllByUserAsync(string userId)
        {
            return await _context.RegistroGastoEncabezados
                .Where(r => r.UserId == userId)
                .Include(r => r.FondoMonetario)
                .Include(r => r.Detalles)
                    .ThenInclude(d => d.TipoGasto)
                .OrderByDescending(r => r.Fecha)
                .ToListAsync();
        }

        public async Task<RegistroGastoEncabezado> GetByIdAsync(int id)
        {
            return await _context.RegistroGastoEncabezados
                .Include(r => r.FondoMonetario)
                .Include(r => r.Detalles)
                    .ThenInclude(d => d.TipoGasto)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task DeleteAsync(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var registro = await _context.RegistroGastoEncabezados
                    .Include(r => r.Detalles)
                    .FirstOrDefaultAsync(r => r.Id == id);

                if (registro != null)
                {
                    // Eliminar detalles primero
                    _context.RegistroGastoDetalles.RemoveRange(registro.Detalles);
                    // Eliminar encabezado
                    _context.RegistroGastoEncabezados.Remove(registro);

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        //Para pagina consulta de movimientos
        public async Task<List<RegistroGastoEncabezado>> GetByUserAndDateRangeAsync(string userId, DateTime desde, DateTime hasta)
        {
            return await _context.RegistroGastoEncabezados
                .Where(r => r.UserId == userId && r.Fecha >= desde && r.Fecha <= hasta)
                .Include(r => r.FondoMonetario)
                .Include(r => r.Detalles)
                    .ThenInclude(d => d.TipoGasto)
                .OrderByDescending(r => r.Fecha)
                .ToListAsync();
        }
        //Para Grafico de Gastos
        public async Task<List<RegistroGastoEncabezado>> GetGastosAsync()
        {
            return await _context.RegistroGastoEncabezados
              .Include(r => r.Detalles)
              .ThenInclude(d => d.TipoGasto)
                .ToListAsync();
        }
        public async Task<List<TipoGasto>> GetTiposGastoAsync()
        {
            return await _context.TiposGasto.ToListAsync();
        }
    }
}
