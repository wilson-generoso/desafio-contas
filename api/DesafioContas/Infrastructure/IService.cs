using Desafio.Contas.Domain;

namespace Desafio.Contas.Infrastructure
{
    public interface IService<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity> Get(Guid id);
        Task Create(TEntity entity);
        Task Update(TEntity entity);
        Task Delete(Guid id);
    }
}
