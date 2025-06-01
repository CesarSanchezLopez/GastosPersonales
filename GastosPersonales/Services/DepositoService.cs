using GastosPersonales.Data;
using GastosPersonales.Models;
using Microsoft.EntityFrameworkCore;

namespace GastosPersonales.Services
{
    public class DepositoService
    {
        private readonly ApplicationDbContext _context;

        public DepositoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Deposito>> GetAllAsync()
        {
            return await _context.Depositos
                .Include(d => d.FondoMonetario)
                .ToListAsync();
        }

        public async Task AddAsync(Deposito deposito)
        {
            _context.Depositos.Add(deposito);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Depositos.FindAsync(id);
            if (entity != null)
            {
                _context.Depositos.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        //Para pagina consulta de movimientos
        public async Task<List<Deposito>> GetByUserAndDateRangeAsync(string userId, DateTime desde, DateTime hasta)
        {
            return await _context.Depositos
                .Include(d => d.FondoMonetario)
                .Where(d => d.UserId == userId && d.Fecha >= desde && d.Fecha <= hasta)
                .OrderByDescending(d => d.Fecha)
                .ToListAsync();
        }
    }
}
