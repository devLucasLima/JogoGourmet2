using JogoGourmet.Core.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JogoGourmet.Domain
{
    public interface IArvorePratoRepository: IRepository<ArvorePrato>
    {
        Task<ArvorePrato> ObterPorId(Guid? Id);

        void Adicionar(ArvorePrato node);

        void Alterar(ArvorePrato node);
    }
}
