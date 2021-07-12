using JogoGourmet.Application.DTOs;
using JogoGourmet.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JogoGourmet.Application.Services
{
    public class PratoAppService : IPratoAppService
    {
        private readonly IPratoRepository _pratoRepository;
        /*pode ser injeto no startup*/
        public PratoAppService()
        {
            _pratoRepository = new JogoGourmet
                .Data.Repository.PratoRepository(new Data.JogoGourmetContext());
        }

        public async Task<PratoDto> ObterPorId(Guid id)
        {
            var prato = await _pratoRepository.ObterPorId(id);

            return new PratoDto
            {
                Id = prato.Id,
                Caracteristica = prato.Caracteristica,
                Nome = prato.Nome
            };
        }

        public async Task Adicionar(PratoDto pratoDto)
        {
            Prato prato = new Prato(
                pratoDto.Nome,
                pratoDto.Caracteristica
            );

            _pratoRepository.Adicionar(prato);

            await _pratoRepository.UnitOfWorrk.Commit();

            pratoDto.Id = prato.Id;
        }

        public void Dispose()
        {
            _pratoRepository?.Dispose();
        }
    }
}
