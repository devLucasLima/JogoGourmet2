using JogoGourmet.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace JogoGourmet.Core.Data
{
    public interface IRepository<T>: IDisposable where T : IAggregateRoot
    {
        IUnitOfWork UnitOfWorrk { get; }
    }
}
