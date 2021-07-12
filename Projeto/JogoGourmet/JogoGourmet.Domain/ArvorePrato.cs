using JogoGourmet.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace JogoGourmet.Domain
{
    public class ArvorePrato: Entity, IAggregateRoot
    { 
        public Guid PratoId { get; private set; }
        public Guid? NodeComCaracteristicaId { get; private set; }
        public Guid? NodeSemCaracteristicaId { get; private set; }

        public Prato Prato { get; private set; }        
        public ArvorePrato? NodeComCaracteristica { get; private set; }
        public ArvorePrato? NodeSemCaracteristica { get; private set; }

        ////EF
        public ArvorePrato? NodePaiComCaracteristica { get; set; }
        public ArvorePrato? NodePaiSemCaracteristica { get; set; }


        public ArvorePrato()
        { }

        public ArvorePrato(Guid pratoId, Guid? nodeComCaracteristicaId, Guid? nodeSemCaracteristicaId)
        {
            PratoId = pratoId;
            NodeComCaracteristicaId = nodeComCaracteristicaId;
            NodeSemCaracteristicaId = nodeSemCaracteristicaId;
        }

        public ArvorePrato(Guid id, Guid pratoId, Guid? nodeComCaracteristicaId, Guid? nodeSemCaracteristicaId)
        {
            Id = id;
            PratoId = pratoId;
            NodeComCaracteristicaId = nodeComCaracteristicaId;
            NodeSemCaracteristicaId = nodeSemCaracteristicaId;
        }

        public void SetNodeComCaracteristicaId(Guid? id)
        {
            NodeComCaracteristicaId = id;
        }

        public void SetNodeSemCaracteristicaId(Guid? id)
        {
            NodeSemCaracteristicaId = id;
        }

    }
}
