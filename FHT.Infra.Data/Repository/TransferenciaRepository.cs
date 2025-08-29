using FHT.Domain.Entities;
using FHT.Domain.Repositories;
using FHT.Infra.Data.Context;

namespace FHT.Infra.Data.Repository
{
    public class TransferenciaRepository : Base.UnitOfWork.Repository<TransferenciaBancaria>, ITransferenciaRepository
    {
        public TransferenciaRepository(AppDbContext ctx) : base(ctx) { }
    }
}
