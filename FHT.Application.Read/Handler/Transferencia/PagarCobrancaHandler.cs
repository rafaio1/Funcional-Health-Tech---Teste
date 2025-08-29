using System;
using System.Threading;
using System.Threading.Tasks;
using FHT.Domain.Entities;
using FHT.Application.Read.Transferencia;
using FHT.Domain.Repositories;
using FHT.Infra.Data.Core.Interfaces;
using MediatR;

namespace FHT.Application.Read.Handler.Transferencia
{
    public class CriarTransferenciaHandler : IRequestHandler<CriarTransferenciaCommand, long>
    {
        private readonly IContaRepository _contas;
        private readonly ITransferenciaRepository _transfs;
        private readonly IAuditoriaRepository _auditoria;
        private readonly IUnitOfWork _uow;

        public CriarTransferenciaHandler(
            IContaRepository contas,
            ITransferenciaRepository transfs,
            IAuditoriaRepository auditoria,
            IUnitOfWork uow)
        {
            _contas = contas;
            _transfs = transfs;
            _auditoria = auditoria;
            _uow = uow;
        }
        public async Task<long> Handle(CriarTransferenciaCommand cmd, CancellationToken ct)
        {
            if (cmd.Valor <= 0) throw new InvalidOperationException("Valor inválido.");

            var conta = await _contas.GetByIdAsync(cmd.ContaId, ct)
                        ?? throw new InvalidOperationException("Conta não encontrada.");
            if (conta.ClienteId != cmd.ClienteId)
                throw new InvalidOperationException("Conta não pertence ao cliente.");

            if (conta.Saldo < cmd.Valor)
                throw new InvalidOperationException("Saldo insuficiente.");

            var txId = $"TR-{Guid.NewGuid():N}";
            var transf = new TransferenciaBancaria
            {
                ClienteId = cmd.ClienteId,
                ContaId = cmd.ContaId,
                Tipo = cmd.Tipo,
                Status = StatusTransferencia.Concluida,
                Valor = cmd.Valor,
                Descricao = cmd.Descricao,
                IdentificadorTransacao = txId,
                PixChave = cmd.PixChave,
                BancoDestino = cmd.BancoDestino,
                AgenciaDestino = cmd.AgenciaDestino,
                ContaDestino = cmd.ContaDestino,
                DocumentoTitularDestino = cmd.DocumentoTitularDestino,
                NomeTitularDestino = cmd.NomeTitularDestino,
                CodigoBarras = cmd.CodigoBarras,
                LinhaDigitavel = cmd.LinhaDigitavel,
                DataSolicitacao = DateTime.UtcNow,
                DataConclusao = DateTime.UtcNow
            };

            conta.Saldo -= cmd.Valor;
            await _transfs.AddAsync(transf, ct);
            _contas.Update(conta);

            await _uow.CommitAsync(ct);

            return transf.TransferenciaId;
        }
    }
}
