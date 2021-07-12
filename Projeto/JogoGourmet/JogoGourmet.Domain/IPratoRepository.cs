using JogoGourmet.Core.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JogoGourmet.Domain
{
    public interface IPratoRepository: IRepository<Prato>
    {
        Task<Prato> ObterPorId(Guid id);

        void Adicionar(Prato prato);
    }
}
