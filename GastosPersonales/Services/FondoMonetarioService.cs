using GastosPersonales.Data;
using GastosPersonales.Models;
using Microsoft.EntityFrameworkCore;

namespace GastosPersonales.Services
{
    public class FondoMonetarioService
    {
        private readonly ApplicationDbContext _context;

        public FondoMonetarioService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<FondoMonetario>> GetAllAsync()
        {
            return await _context.FondosMonetarios.ToListAsync();
        }

        public async Task AddAsync(FondoMonetario fondo)
        {
            _context.FondosMonetarios.Add(fondo);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(FondoMonetario fondo)
        {
            var entity = await _context.FondosMonetarios.FindAsync(fondo.Id);
            if (entity != null)
            {
                // Solo actualiza los campos necesarios
                entity.Nombre = fondo.Nombre;
                entity.Descripcion = fondo.Descripcion;

                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Fondo Monetario no encontrado para actualizar.");
            }
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.FondosMonetarios.FindAsync(id);
            if (entity != null)
            {
                _context.FondosMonetarios.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }

}
