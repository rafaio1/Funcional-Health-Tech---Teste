using FHT.Domain.Entities;
using FHT.Domain.Repositories;
using FHT.Infra.Data.Context;
using FHT.Infra.Data.Repository.Base.UnitOfWork;

namespace FHT.Infra.Data.Repository
{
    public class ComprovanteRepository : Repository<Comprovante>, IComprovanteRepository
    {
        public ComprovanteRepository(AppDbContext context) : base(context)
        {
        }
    }
}