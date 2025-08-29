using FHT.Application.Read.Command.Cobranca;
using FHT.Domain.Repositories;
using FHT.Infra.Data.Core.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FHT.Application.Read.Handler.Cobranca
{
    public class CriarCobrancaHandler : IRequestHandler<CriarCobrancaCommand, long>
    {
        private readonly ICobrancaRepository _repo;
        private readonly IUnitOfWork _uow;

        public CriarCobrancaHandler(ICobrancaRepository repo, IUnitOfWork uow)
        {
            _repo = repo; _uow = uow;
        }

        public async Task<long> Handle(CriarCobrancaCommand request, CancellationToken ct)
        {
            if (request.Valor <= 0)
            {
                throw new ArgumentException("Valor da cobrança deve ser maior que zero.");
            }

            Domain.Entities.Cobranca ent = new Domain.Entities.Cobranca
            {
                ClienteId = request.ClienteId,
                Metodo = (Domain.Entities.MetodoCobranca)request.Metodo,
                Situacao = Domain.Entities.SituacaoCobranca.AguardandoPagamento,
                Valor = request.Valor,
                DataEmissao = DateTimeOffset.Now,
                DataVencimento = request.DataVencimento,
                ReferenciaExterna = request.ReferenciaExterna,
                Descricao = request.Descricao,
                Pago = false
            };

            await _repo.AddAsync(ent, ct);
            await _uow.CommitAsync(ct);
            return ent.CobrancaId;
        }
    }
}