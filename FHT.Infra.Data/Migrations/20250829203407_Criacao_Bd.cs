using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FHT.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class Criacao_Bd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AUDITORIA",
                columns: table => new
                {
                    AUDITORIA_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ENTIDADE = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ENTIDADE_ID = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ACAO = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MOTIVO = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    USUARIO_ID = table.Column<long>(type: "bigint", nullable: true),
                    USUARIO_LOGIN = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    CORRELACAO_ID = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    SESSION_ID = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    ORIGEM_IP = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    USER_AGENT = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    DADOS_ANTES = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DADOS_DEPOIS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SUCESSO = table.Column<bool>(type: "bit", nullable: false),
                    ERRO_MSG = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DATA_EVENTO = table.Column<DateTimeOffset>(type: "datetimeoffset(6)", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    DATA_INSERCAO = table.Column<DateTimeOffset>(type: "datetimeoffset(6)", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AUDITORIA", x => x.AUDITORIA_ID);
                });

            migrationBuilder.CreateTable(
                name: "CLIENTE",
                columns: table => new
                {
                    CLIENTE_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TIPO_CLIENTE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    STATUS_CLIENTE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    NOME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SALDO = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DATA_CADASTRO = table.Column<DateTimeOffset>(type: "datetimeoffset(6)", nullable: false),
                    DATA_ATUALIZACAO = table.Column<DateTimeOffset>(type: "datetimeoffset(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CLIENTE", x => x.CLIENTE_ID);
                });

            migrationBuilder.CreateTable(
                name: "COBRANCA",
                columns: table => new
                {
                    COBRANCA_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CLIENTE_ID = table.Column<long>(type: "bigint", nullable: false),
                    METODO = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SITUACAO = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PAGO = table.Column<bool>(type: "bit", nullable: false),
                    VALOR = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DESCONTO = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MULTA = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    JUROS = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    VALOR_PAGO = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DESCRICAO = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    REFERENCIA_EXTERNA = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CODIGO_BARRAS = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    LINHA_DIGITAVEL = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NOSSO_NUMERO = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PIX_TXID = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PIX_CHAVE = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    ID_TRANSACAO = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    GATEWAY = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    METADADOS = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    DATA_EMISSAO = table.Column<DateTimeOffset>(type: "datetimeoffset(6)", nullable: false),
                    DATA_VENCIMENTO = table.Column<DateTimeOffset>(type: "datetimeoffset(6)", nullable: true),
                    DATA_PAGAMENTO = table.Column<DateTimeOffset>(type: "datetimeoffset(6)", nullable: true),
                    DATA_CADASTRO = table.Column<DateTimeOffset>(type: "datetimeoffset(6)", nullable: false),
                    DATA_ATUALIZACAO = table.Column<DateTimeOffset>(type: "datetimeoffset(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COBRANCA", x => x.COBRANCA_ID);
                    table.ForeignKey(
                        name: "FK_COBRANCA_CLIENTE",
                        column: x => x.CLIENTE_ID,
                        principalTable: "CLIENTE",
                        principalColumn: "CLIENTE_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "COMPLIANCE",
                columns: table => new
                {
                    COMPLIANCE_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CLIENTE_ID = table.Column<long>(type: "bigint", nullable: false),
                    STATUS_KYC = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    NIVEL_RISCO = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    PESSOA_POLITICAMENTE_EXPOSTA = table.Column<bool>(type: "bit", nullable: false),
                    RESTRICAO_SANCOES = table.Column<bool>(type: "bit", nullable: false),
                    FONTE_ANALISE = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    OBSERVACAO = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DATA_ANALISE = table.Column<DateTimeOffset>(type: "datetimeoffset(6)", nullable: true),
                    DATA_EXPIRACAO = table.Column<DateTimeOffset>(type: "datetimeoffset(6)", nullable: true),
                    DATA_CADASTRO = table.Column<DateTimeOffset>(type: "datetimeoffset(6)", nullable: false),
                    DATA_ATUALIZACAO = table.Column<DateTimeOffset>(type: "datetimeoffset(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COMPLIANCE", x => x.COMPLIANCE_ID);
                    table.ForeignKey(
                        name: "FK_COMPLIANCE_CLIENTE",
                        column: x => x.CLIENTE_ID,
                        principalTable: "CLIENTE",
                        principalColumn: "CLIENTE_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CONTA",
                columns: table => new
                {
                    CONTA_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CLIENTE_ID = table.Column<long>(type: "bigint", nullable: false),
                    TIPO_CONTA = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    STATUS_CONTA = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AGENCIA = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    NUMERO = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DIGITO = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    SALDO = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    DATA_ABERTURA = table.Column<DateTimeOffset>(type: "datetimeoffset(6)", nullable: false),
                    DATA_ENCERRAMENTO = table.Column<DateTimeOffset>(type: "datetimeoffset(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CONTA", x => x.CONTA_ID);
                    table.ForeignKey(
                        name: "FK_CONTA_CLIENTE",
                        column: x => x.CLIENTE_ID,
                        principalTable: "CLIENTE",
                        principalColumn: "CLIENTE_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CONTATO",
                columns: table => new
                {
                    CONTATO_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CLIENTE_ID = table.Column<long>(type: "bigint", nullable: false),
                    TIPO = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    VALOR = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PRINCIPAL = table.Column<bool>(type: "bit", nullable: false),
                    OBSERVACAO = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DATA_CADASTRO = table.Column<DateTimeOffset>(type: "datetimeoffset(6)", nullable: false),
                    DATA_ATUALIZACAO = table.Column<DateTimeOffset>(type: "datetimeoffset(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CONTATO", x => x.CONTATO_ID);
                    table.ForeignKey(
                        name: "FK_CONTATO_CLIENTE",
                        column: x => x.CLIENTE_ID,
                        principalTable: "CLIENTE",
                        principalColumn: "CLIENTE_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DADO_PESSOAL",
                columns: table => new
                {
                    DADO_PESSOAL_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CLIENTE_ID = table.Column<long>(type: "bigint", nullable: false),
                    DATA_NASCIMENTO = table.Column<DateTimeOffset>(type: "datetimeoffset(6)", nullable: true),
                    NOME_SOCIAL = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    NOME_MAE = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    NOME_PAI = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ESTADO_CIVIL = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    GENERO = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    NACIONALIDADE = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    PROFISSAO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RENDA_MENSAL = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    DATA_CADASTRO = table.Column<DateTimeOffset>(type: "datetimeoffset(6)", nullable: false),
                    DATA_ATUALIZACAO = table.Column<DateTimeOffset>(type: "datetimeoffset(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DADO_PESSOAL", x => x.DADO_PESSOAL_ID);
                    table.ForeignKey(
                        name: "FK_DADO_PESSOAL_CLIENTE",
                        column: x => x.CLIENTE_ID,
                        principalTable: "CLIENTE",
                        principalColumn: "CLIENTE_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DOCUMENTO_FISCAL",
                columns: table => new
                {
                    DOCUMENTO_FISCAL_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CLIENTE_ID = table.Column<long>(type: "bigint", nullable: false),
                    TIPO_DOCUMENTO = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NUMERO = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    ORGAO_EMISSOR = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UF_EMISSOR = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    DATA_EMISSAO = table.Column<DateTimeOffset>(type: "datetimeoffset(6)", nullable: true),
                    DATA_VALIDADE = table.Column<DateTimeOffset>(type: "datetimeoffset(6)", nullable: true),
                    PRINCIPAL = table.Column<bool>(type: "bit", nullable: false),
                    DATA_CADASTRO = table.Column<DateTimeOffset>(type: "datetimeoffset(6)", nullable: false),
                    DATA_ATUALIZACAO = table.Column<DateTimeOffset>(type: "datetimeoffset(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DOCUMENTO_FISCAL", x => x.DOCUMENTO_FISCAL_ID);
                    table.ForeignKey(
                        name: "FK_DOCUMENTO_FISCAL_CLIENTE",
                        column: x => x.CLIENTE_ID,
                        principalTable: "CLIENTE",
                        principalColumn: "CLIENTE_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ENDERECO_FISCAL",
                columns: table => new
                {
                    ENDERECO_FISCAL_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CLIENTE_ID = table.Column<long>(type: "bigint", nullable: false),
                    TIPO_ENDERECO = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LOGRADOURO = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NUMERO = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    COMPLEMENTO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BAIRRO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MUNICIPIO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UF = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    CEP = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    PAIS = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    CODIGO_IBGE_MUNICIPIO = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: true),
                    PRINCIPAL = table.Column<bool>(type: "bit", nullable: false),
                    DATA_CADASTRO = table.Column<DateTimeOffset>(type: "datetimeoffset(6)", nullable: false),
                    DATA_ATUALIZACAO = table.Column<DateTimeOffset>(type: "datetimeoffset(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ENDERECO_FISCAL", x => x.ENDERECO_FISCAL_ID);
                    table.ForeignKey(
                        name: "FK_ENDERECO_FISCAL_CLIENTE",
                        column: x => x.CLIENTE_ID,
                        principalTable: "CLIENTE",
                        principalColumn: "CLIENTE_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SOCIETARIO",
                columns: table => new
                {
                    SOCIETARIO_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CLIENTE_ID = table.Column<long>(type: "bigint", nullable: false),
                    NOME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DOCUMENTO = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CARGO_FUNCAO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PARTICIPACAO_PERCENTUAL = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    REPRESENTANTE_LEGAL = table.Column<bool>(type: "bit", nullable: false),
                    DATA_ENTRADA = table.Column<DateTimeOffset>(type: "datetimeoffset(6)", nullable: true),
                    DATA_SAIDA = table.Column<DateTimeOffset>(type: "datetimeoffset(6)", nullable: true),
                    DATA_CADASTRO = table.Column<DateTimeOffset>(type: "datetimeoffset(6)", nullable: false),
                    DATA_ATUALIZACAO = table.Column<DateTimeOffset>(type: "datetimeoffset(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SOCIETARIO", x => x.SOCIETARIO_ID);
                    table.ForeignKey(
                        name: "FK_SOCIETARIO_CLIENTE",
                        column: x => x.CLIENTE_ID,
                        principalTable: "CLIENTE",
                        principalColumn: "CLIENTE_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "COMPROVANTE",
                columns: table => new
                {
                    COMPROVANTE_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    COBRANCA_ID = table.Column<long>(type: "bigint", nullable: false),
                    CLIENTE_ID = table.Column<long>(type: "bigint", nullable: false),
                    METODO = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SITUACAO_MOMENTO = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PAGO = table.Column<bool>(type: "bit", nullable: false),
                    VALOR = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VALOR_PAGO = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NUM_AUTENTICACAO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PROTOCOLO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ID_TRANSACAO = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    EMISSOR = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    HASH = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    OBSERVACOES = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ARQUIVO = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    MIME_TYPE = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DATA_PAGAMENTO = table.Column<DateTimeOffset>(type: "datetimeoffset(6)", nullable: false),
                    DATA_GERACAO = table.Column<DateTimeOffset>(type: "datetimeoffset(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COMPROVANTE", x => x.COMPROVANTE_ID);
                    table.ForeignKey(
                        name: "FK_COMPROVANTE_CLIENTE",
                        column: x => x.CLIENTE_ID,
                        principalTable: "CLIENTE",
                        principalColumn: "CLIENTE_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_COMPROVANTE_COBRANCA",
                        column: x => x.COBRANCA_ID,
                        principalTable: "COBRANCA",
                        principalColumn: "COBRANCA_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TRANSFERENCIA",
                columns: table => new
                {
                    TRANSFERENCIA_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CLIENTE_ID = table.Column<long>(type: "bigint", nullable: false),
                    CONTA_ID = table.Column<long>(type: "bigint", nullable: false),
                    TIPO = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    STATUS = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    VALOR = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DESCRICAO = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ID_TRANSACAO = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    PIX_CHAVE = table.Column<string>(type: "nvarchar(140)", maxLength: 140, nullable: true),
                    BANCO_DESTINO = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    AGENCIA_DESTINO = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CONTA_DESTINO = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    DOC_TITULAR_DEST = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    NOME_TITULAR_DEST = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CODIGO_BARRAS = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    LINHA_DIGITAVEL = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    DATA_SOL = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DATA_CONC = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MSG_ERRO = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRANSFERENCIA", x => x.TRANSFERENCIA_ID);
                    table.ForeignKey(
                        name: "FK_TRANSFERENCIA_CLIENTE",
                        column: x => x.CLIENTE_ID,
                        principalTable: "CLIENTE",
                        principalColumn: "CLIENTE_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TRANSFERENCIA_CONTA",
                        column: x => x.CONTA_ID,
                        principalTable: "CONTA",
                        principalColumn: "CONTA_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AUD_COR",
                table: "AUDITORIA",
                column: "CORRELACAO_ID");

            migrationBuilder.CreateIndex(
                name: "IX_AUD_DATA",
                table: "AUDITORIA",
                column: "DATA_EVENTO");

            migrationBuilder.CreateIndex(
                name: "IX_AUD_ENT",
                table: "AUDITORIA",
                columns: new[] { "ENTIDADE", "ENTIDADE_ID" });

            migrationBuilder.CreateIndex(
                name: "IX_AUD_USR",
                table: "AUDITORIA",
                column: "USUARIO_LOGIN");

            migrationBuilder.CreateIndex(
                name: "IX_CLIENTE_NOME",
                table: "CLIENTE",
                column: "NOME");

            migrationBuilder.CreateIndex(
                name: "IX_CLIENTE_STATUS_TIPO",
                table: "CLIENTE",
                columns: new[] { "STATUS_CLIENTE", "TIPO_CLIENTE" });

            migrationBuilder.CreateIndex(
                name: "IX_COB_CLIENTE_METODO",
                table: "COBRANCA",
                columns: new[] { "CLIENTE_ID", "METODO" });

            migrationBuilder.CreateIndex(
                name: "IX_COB_REF_EXT",
                table: "COBRANCA",
                column: "REFERENCIA_EXTERNA");

            migrationBuilder.CreateIndex(
                name: "IX_COB_SITUACAO_PAGO",
                table: "COBRANCA",
                columns: new[] { "SITUACAO", "PAGO" });

            migrationBuilder.CreateIndex(
                name: "IX_COB_VENC",
                table: "COBRANCA",
                column: "DATA_VENCIMENTO");

            migrationBuilder.CreateIndex(
                name: "IX_COMPLIANCE_CLIENTE_STATUS_RISCO",
                table: "COMPLIANCE",
                columns: new[] { "CLIENTE_ID", "STATUS_KYC", "NIVEL_RISCO" });

            migrationBuilder.CreateIndex(
                name: "IX_COMP_AUTENTICACAO",
                table: "COMPROVANTE",
                column: "NUM_AUTENTICACAO");

            migrationBuilder.CreateIndex(
                name: "IX_COMP_CLIENTE_PAGTO",
                table: "COMPROVANTE",
                columns: new[] { "CLIENTE_ID", "DATA_PAGAMENTO" });

            migrationBuilder.CreateIndex(
                name: "IX_COMP_IDTRANS",
                table: "COMPROVANTE",
                column: "ID_TRANSACAO");

            migrationBuilder.CreateIndex(
                name: "UX_COMPROVANTE_COBRANCA",
                table: "COMPROVANTE",
                column: "COBRANCA_ID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CONTA_CLIENTE",
                table: "CONTA",
                column: "CLIENTE_ID");

            migrationBuilder.CreateIndex(
                name: "UX_CONTA_BANCO",
                table: "CONTA",
                columns: new[] { "AGENCIA", "NUMERO", "DIGITO" },
                unique: true,
                filter: "[DIGITO] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CONTATO_CLIENTE_TIPO_PRINC",
                table: "CONTATO",
                columns: new[] { "CLIENTE_ID", "TIPO", "PRINCIPAL" });

            migrationBuilder.CreateIndex(
                name: "UX_DADO_PESSOAL_CLIENTE",
                table: "DADO_PESSOAL",
                column: "CLIENTE_ID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_DOCFISCAL_CLIENTE_TIPO_NUMERO",
                table: "DOCUMENTO_FISCAL",
                columns: new[] { "CLIENTE_ID", "TIPO_DOCUMENTO", "NUMERO" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_END_FISCAL_CLIENTE_TIPO_PRINC",
                table: "ENDERECO_FISCAL",
                columns: new[] { "CLIENTE_ID", "TIPO_ENDERECO", "PRINCIPAL" });

            migrationBuilder.CreateIndex(
                name: "IX_SOCIETARIO_CLIENTE",
                table: "SOCIETARIO",
                column: "CLIENTE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_TRANSF_CLIENTE_CONTA_DATA",
                table: "TRANSFERENCIA",
                columns: new[] { "CLIENTE_ID", "CONTA_ID", "DATA_SOL" });

            migrationBuilder.CreateIndex(
                name: "IX_TRANSFERENCIA_CONTA_ID",
                table: "TRANSFERENCIA",
                column: "CONTA_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AUDITORIA");

            migrationBuilder.DropTable(
                name: "COMPLIANCE");

            migrationBuilder.DropTable(
                name: "COMPROVANTE");

            migrationBuilder.DropTable(
                name: "CONTATO");

            migrationBuilder.DropTable(
                name: "DADO_PESSOAL");

            migrationBuilder.DropTable(
                name: "DOCUMENTO_FISCAL");

            migrationBuilder.DropTable(
                name: "ENDERECO_FISCAL");

            migrationBuilder.DropTable(
                name: "SOCIETARIO");

            migrationBuilder.DropTable(
                name: "TRANSFERENCIA");

            migrationBuilder.DropTable(
                name: "COBRANCA");

            migrationBuilder.DropTable(
                name: "CONTA");

            migrationBuilder.DropTable(
                name: "CLIENTE");
        }
    }
}
