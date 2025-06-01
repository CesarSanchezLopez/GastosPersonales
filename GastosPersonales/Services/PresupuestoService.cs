using GastosPersonales.Data;
using GastosPersonales.Models;
using Microsoft.EntityFrameworkCore;

namespace GastosPersonales.Services
{
    public class PresupuestoService
    {
        private readonly ApplicationDbContext _context;

        public PresupuestoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Presupuesto>> GetAllAsync(string userId)
        {
            return await _context.Presupuestos
                .Where(p => p.UserId == userId)
                .Include(p => p.TipoGasto)
                .ToListAsync();
        }

        public async Task AddAsync(Presupuesto presupuesto)
        {
            var existing = await _context.Presupuestos
                .FirstOrDefaultAsync(p => p.UserId == presupuesto.UserId &&
                                          p.TipoGastoId == presupuesto.TipoGastoId &&
                                          p.Mes == presupuesto.Mes &&
                                          p.Año == presupuesto.Año);
            if (existing != null)
            {
                throw new Exception("Ya existe un presupuesto para este tipo de gasto en el mes y año especificados.");
            }

            _context.Presupuestos.Add(presupuesto);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var presupuesto = await _context.Presupuestos.FindAsync(id);
            if (presupuesto != null)
            {
                _context.Presupuestos.Remove(presupuesto);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Presupuesto>> GetPresupuestosAsync()
        {
            return await _context.Presupuestos
                .Include(p => p.TipoGasto)
                .ToListAsync();
        }
    }
}
