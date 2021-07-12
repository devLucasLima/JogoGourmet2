using JogoGourmet.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JogoGourmet.Application.Services
{
    public interface IArvorePratoAppService: IDisposable
    {
        Task<ArvorePratoDto> ObterPorId(Guid id);
        Task Adicionar(ArvorePratoDto nodeDto);
        Task Alterar(ArvorePratoDto nodeDto);
    }
}
