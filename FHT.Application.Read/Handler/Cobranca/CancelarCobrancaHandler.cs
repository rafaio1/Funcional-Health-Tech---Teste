using FHT.Application.Read.Command.Cobranca;
using FHT.Domain.Entities;
using FHT.Domain.Repositories;
using FHT.Infra.Data.Core.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FHT.Application.Read.Handler.Cobranca
{
    public class CancelarCobrancaHandler : IRequestHandler<CancelarCobrancaCommand, bool>
    {
        private readonly ICobrancaRepository _repo;
        private readonly IUnitOfWork _uow;

        public CancelarCobrancaHandler(ICobrancaRepository repo, IUnitOfWork uow)
        {
            _repo = repo; _uow = uow;
        }

        public async Task<bool> Handle(CancelarCobrancaCommand request, CancellationToken ct)
        {
            var cob = await _repo.GetByIdAsync(request.CobrancaId, ct);
            if (cob is null)
            {
                return false;
            }

            if (cob.Pago)
            {
                throw new InvalidOperationException("Não é possível cancelar uma cobrança paga.");
            }

            cob.Situacao = SituacaoCobranca.Cancelado;
            cob.DataAtualizacao = DateTimeOffset.Now;
            _repo.Update(cob);
            await _uow.CommitAsync(ct);
            return true;
        }
    }
}