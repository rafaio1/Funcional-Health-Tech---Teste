using FHT.Domain.Entities;
using FHT.Domain.Repositories;
using FHT.Infra.Data.Context;
using FHT.Infra.Data.Repository.Base.UnitOfWork;

namespace FHT.Infra.Data.Repository
{
    public class DocumentoFiscalRepository : Repository<DocumentoFiscal>, IDocumentoFiscalRepository
    {
        public DocumentoFiscalRepository(AppDbContext context) : base(context)
        {
        }
    }
}