using FHT.Application.Read.Command.Cobranca;
using FHT.Domain.Entities;
using FHT.Domain.Repositories;
using FHT.Infra.Data.Core.Interfaces;
using MediatR;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FHT.Application.Read.Handler.Cobranca
{
    public class PagarCobrancaHandler : IRequestHandler<PagarCobrancaCommand, long>
    {
        private readonly ICobrancaRepository _cobRepo;
        private readonly IComprovanteRepository _compRepo;
        private readonly IUnitOfWork _uow;

        public PagarCobrancaHandler(ICobrancaRepository cobRepo, IComprovanteRepository compRepo, IUnitOfWork uow)
        {
            _cobRepo = cobRepo; _compRepo = compRepo; _uow = uow;
        }

        public async Task<long> Handle(PagarCobrancaCommand request, CancellationToken ct)
        {
            var cob = await _cobRepo.GetByIdAsync(request.CobrancaId, ct)
                      ?? throw new InvalidOperationException("Cobrança não encontrada.");

            if (cob.Pago || cob.Situacao == SituacaoCobranca.Pago)
            {
                throw new InvalidOperationException("Cobrança já está paga.");
            }

            var valorPago = request.ValorPago ?? cob.Valor;
            if (valorPago <= 0)
            {
                throw new ArgumentException("Valor pago inválido.");
            }

            cob.Pago = true;
            cob.Situacao = SituacaoCobranca.Pago;
            cob.DataPagamento = DateTimeOffset.Now;
            cob.ValorPago = valorPago;
            cob.DataAtualizacao = DateTimeOffset.Now;

            _cobRepo.Update(cob);

            string autenticacao = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(
                $"{cob.CobrancaId}|{valorPago}|{cob.DataPagamento:O}|{Guid.NewGuid()}"))).Substring(0, 32);

            Comprovante comp = new Comprovante
            {
                CobrancaId = cob.CobrancaId,
                ClienteId = cob.ClienteId,
                Metodo = cob.Metodo,
                SituacaoNoMomento = cob.Situacao,
                Pago = true,
                Valor = cob.Valor,
                ValorPago = valorPago,
                NumeroAutenticacao = autenticacao,
                IdentificadorTransacao = request.IdentificadorTransacao,
                Emissor = request.Emissor ?? cob.Gateway,
                DataPagamento = cob.DataPagamento.Value,
                DataGeracao = DateTimeOffset.Now
            };

            await _compRepo.AddAsync(comp, ct);
            await _uow.CommitAsync(ct);

            return comp.ComprovanteId;
        }
    }
}