using System;
using System.Collections.Generic;
using System.Text;

namespace JogoGourmet.Application.DTOs
{
    public class ArvorePratoDto
    {
        public Guid Id { get; set; }
        public Guid PratoId { get; set; }
        public Guid? NodeComCaracteristicaId { get; set; }
        public Guid? NodeSemCaracteristicaId { get; set; }

        public PratoDto Prato { get; set; }
        public ArvorePratoDto? NodeComCaracteristica { get; set; }
        public ArvorePratoDto? NodeSemCaracteristica { get; set; }

        //EF
        ////EF
        public ArvorePratoDto? NodePaiComCaracteristica { get; set; }
        public ArvorePratoDto? NodePaiSemCaracteristica { get; set; }


        public void SetPrato(PratoDto prato)
        {
            PratoId = prato.Id;
            Prato = prato;
        }

        public void SetNodeComCaracteristica(ArvorePratoDto node)
        {
            NodeComCaracteristicaId = node.Id;
            NodeComCaracteristica = node;
        }

        public void SetNodeSemCaracteristica(ArvorePratoDto node)
        {
            NodeSemCaracteristicaId = node.Id;
            NodeSemCaracteristica = node;
        }

    }
}
