using System;
using System.Collections.Generic;
using System.Text;

namespace JogoGourmet.Core.DomainObjects
{
    public class Validacoes
    {
        public static void ValidarSeIgual(object obj1, object obj2, string mensagem)
        {
            if (!obj1.Equals(obj2))
                throw new DomainException(mensagem);
        }

        public static void ValidarSeDiferente(object obj1, object obj2, string mensagem)
        {
            if (obj1.Equals(obj2))
                throw new DomainException(mensagem);
        }

        public static void ValidarCaracteres(decimal valor, decimal minimo, decimal maximo, string mensagem)
        {
            if (valor > maximo || valor < minimo)
                throw new DomainException(mensagem);
        }

        public static void ValidarSeVazio(string valor, string mensagem)
        {
            if (valor == null || valor.Trim().Length == 0)
                throw new DomainException(mensagem);
        }
    }
}
