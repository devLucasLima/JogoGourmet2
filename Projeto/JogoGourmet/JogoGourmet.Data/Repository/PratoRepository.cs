using JogoGourmet.Core.Data;
using JogoGourmet.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JogoGourmet.Data.Repository
{
    public class PratoRepository : IPratoRepository
    {
        private readonly JogoGourmetContext _context;

        public PratoRepository(JogoGourmetContext context)
        {
            _context = context;
        }
        public IUnitOfWork UnitOfWorrk => _context;

        public async Task<Prato> ObterPorId(Guid id)
        {
            return await _context.Pratos.FindAsync(id);
        }

        public void Adicionar(Prato prato)
        {
            _context.Add(prato);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
