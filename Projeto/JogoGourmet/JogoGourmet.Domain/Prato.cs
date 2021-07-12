using JogoGourmet.Core.DomainObjects;
using System;
using System.Collections.Generic;

namespace JogoGourmet.Domain
{
    public class Prato: Entity, IAggregateRoot
    {
        public string Nome { get; private set; }

        public string Caracteristica { get; private set; }

        //EF
        public ArvorePrato ArvorePrato { get; set; }

        public Prato(string nome, string caracteristica)
        {
            Nome = nome;
            Caracteristica = caracteristica;

            Validar();
        }

        public Prato(Guid id, string nome, string caracteristica)
        {
            Id = id;
            Nome = nome;
            Caracteristica = caracteristica;

            Validar();
        }

        public void Validar()
        {
            //Validacoes.ValidarSeVazio(Nome, "O campo Nome do prato não pode estar vazio");
            //Validacoes.ValidarSeVazio(Caracteristica, "O campo Característica do prato não pode estar vazio");
            //Validacoes.ValidarSeDiferente(Id, Guid.Empty, "O campo Id do prato não pode ser vazio");
        }

    }
}
