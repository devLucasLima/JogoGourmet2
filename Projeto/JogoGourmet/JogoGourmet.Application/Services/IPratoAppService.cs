using JogoGourmet.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JogoGourmet.Application.Services
{
    public interface IPratoAppService: IDisposable
    {
        Task<PratoDto> ObterPorId(Guid id);
        Task Adicionar(PratoDto pratoDto);
    }
}
