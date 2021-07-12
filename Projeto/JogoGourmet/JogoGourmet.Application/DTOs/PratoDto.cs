using System;
using System.Collections.Generic;
using System.Text;

namespace JogoGourmet.Application.DTOs
{
    public class PratoDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }

        public string Caracteristica { get; set; }

        //EF
        public ArvorePratoDto ArvorePrato { get; set; }

    }
}
