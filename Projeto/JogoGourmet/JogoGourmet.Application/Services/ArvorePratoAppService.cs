using JogoGourmet.Application.DTOs;
using JogoGourmet.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JogoGourmet.Application.Services
{
    public class ArvorePratoAppService : IArvorePratoAppService
    {
        private readonly IArvorePratoRepository _arvorePratoRepository;
        /*pode ser injeto no startup*/
        public ArvorePratoAppService()
        {
            _arvorePratoRepository = new JogoGourmet
                .Data.Repository.ArvorePratoRepository(new Data.JogoGourmetContext());
        }

        public async Task Adicionar(ArvorePratoDto nodeDto)
        {
            var node = new ArvorePrato(
                nodeDto.PratoId,
                nodeDto.NodeComCaracteristicaId,
                nodeDto.NodeSemCaracteristicaId
            );

            _arvorePratoRepository.Adicionar(node);

            await _arvorePratoRepository.UnitOfWorrk.Commit();

            nodeDto.Id = node.Id;
        }

        public async Task Alterar(ArvorePratoDto nodeDto)
        {
            var node = await _arvorePratoRepository.ObterPorId(nodeDto.Id);

            if(nodeDto.NodeSemCaracteristicaId != null) node.SetNodeSemCaracteristicaId(nodeDto.NodeSemCaracteristicaId);
            if(nodeDto.NodeComCaracteristicaId != null) node.SetNodeComCaracteristicaId(nodeDto.NodeComCaracteristicaId);

            _arvorePratoRepository.Alterar(node);

            await _arvorePratoRepository.UnitOfWorrk.Commit();
        }

        public async Task<ArvorePratoDto> ObterPorId(Guid id)
        {
            var node = await _arvorePratoRepository.ObterPorId(id);

            return new ArvorePratoDto
            {
                Id = node.Id,
                PratoId = node.PratoId,
                NodeComCaracteristicaId = node.NodeComCaracteristicaId,
                NodeSemCaracteristicaId = node.NodeSemCaracteristicaId
            };
        }

        public void Dispose()
        {
            _arvorePratoRepository?.Dispose();
        }
    }
}
