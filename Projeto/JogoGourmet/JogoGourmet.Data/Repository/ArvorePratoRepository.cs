using JogoGourmet.Core.Data;
using JogoGourmet.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JogoGourmet.Data.Repository
{
    public class ArvorePratoRepository: IArvorePratoRepository
    {
        private readonly JogoGourmetContext _context;

        public ArvorePratoRepository(JogoGourmetContext context)
        {
            _context = context;
        }
        public IUnitOfWork UnitOfWorrk => _context;

        public async Task<ArvorePrato> ObterPorId(Guid? id)
        {
            var result =  await _context.ArvorePratos.FindAsync(id);

            return result;
        }

        public void Adicionar(ArvorePrato node)
        {
            _context.Add(node);
        }

        public void Alterar(ArvorePrato node)
        {
             _context.Update(node);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
