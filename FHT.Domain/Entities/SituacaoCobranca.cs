namespace FHT.Domain.Entities
{
    public enum SituacaoCobranca
    {
        Pendente,
        AguardandoPagamento,
        Pago,
        Cancelado,
        Expirado,
        Estornado,
        EmProcessamento,
        Falhou
    }
}
