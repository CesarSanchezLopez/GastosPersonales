using GastosPersonales.Data;
using GastosPersonales.Models;
using Microsoft.EntityFrameworkCore;

namespace GastosPersonales.Services
{
    public class TipoGastoService
    {
        private readonly ApplicationDbContext _context;

        public TipoGastoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TipoGasto>> GetAllAsync()
        {
            return await _context.TiposGasto.ToListAsync();
        }

        public async Task AddAsync(TipoGasto tipoGasto)
        {
            _context.TiposGasto.Add(tipoGasto);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TipoGasto tipo)
        {
            var entity = await _context.TiposGasto.FindAsync(tipo.Id);
            if (entity != null)
            {
                // Actualizar sólo las propiedades que quieres cambiar
                entity.Descripcion = tipo.Descripcion;
                // Si necesitas actualizar más propiedades, agrégalas aquí

                await _context.SaveChangesAsync();
            }
            else
            {
                // Opcional: lanzar excepción o manejar caso de entidad no encontrada
                throw new Exception("Tipo de gasto no encontrado para actualizar.");
            }
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.TiposGasto.FindAsync(id);
            if (entity != null)
            {
                _context.TiposGasto.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
