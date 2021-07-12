using System.Threading.Tasks;

namespace JogoGourmet.Core.Data
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}
