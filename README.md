

<h1 align="center">Funcional Health Tech — Banking APIs <small>(v1)</small></h1>

<p align="center">
  <img alt=".NET" src="https://img.shields.io/badge/.NET-8.0-512bd4?logo=dotnet&logoColor=white">
  <img alt="ASP.NET Core" src="https://img.shields.io/badge/ASP.NET%20Core-API-2e3a59?logo=dotnet&logoColor=white">
  <img alt="OpenAPI" src="https://img.shields.io/badge/OpenAPI-3.0-6ba539?logo=openapi-initiative&logoColor=white">
  <img alt="SQL Server" src="https://img.shields.io/badge/SQL%20Server-LocalDB%2FExpress-a61d3a?logo=microsoftsqlserver&logoColor=white">
  <img alt="Rate Limit" src="https://img.shields.io/badge/Rate%20Limit-150%20req%2Fmin-ff9f43">
  <a href="#licença"><img alt="License" src="https://img.shields.io/badge/license-MIT-0aa"></a>
</p>

<p align="center">
  APIs REST para <b>cadastro e operações bancárias</b> (clientes, contas, transferências, cobranças, compliance, auditorias etc.)<br>
  Documentação <b>OpenAPI 3.0</b> com <b>Swagger UI</b>.
</p>

<p align="center">
  <code>Swagger UI</code>: <kbd>https://localhost:62879/swagger</kbd> &nbsp;•&nbsp;
  <code>Spec</code>: <kbd>/swagger/v1/swagger.json</kbd>
</p>

<hr>

<nav>
  <p><b>Índice</b></p>
  <ol>
    <li><a href="#visão-geral">Visão Geral</a></li>
    <li><a href="#autenticação--segurança">Autenticação &amp; Segurança</a></li>
    <li><a href="#rate-limit--cabeçalhos">Rate Limit &amp; Cabeçalhos</a></li>
    <li><a href="#quickstart">Quickstart</a></li>
    <li><a href="#módulos--endpoints">Módulos &amp; Endpoints</a></li>
    <li><a href="#auditoria">Auditoria</a></li>
    <li><a href="#erros--convenções">Erros &amp; Convenções</a></li>
    <li><a href="#execução-local">Execução Local</a></li>
    <li><a href="#banco-de-dados--migrações">Banco de Dados &amp; Migrações</a></li>
    <li><a href="#lgpd">Boas Práticas de LGPD</a></li>
    <li><a href="#contribuição">Contribuição</a></li>
    <li><a href="#licença">Licença</a></li>
  </ol>
</nav>

<hr>

<h2 id="visão-geral">Visão Geral</h2>
<ul>
  <li><b>Objetivo:</b> expor endpoints claros e seguros para <i>gestão bancária</i>:
    <ul>
      <li><b>Core</b>: Cliente, Conta, Transferências, Cobrança, Comprovantes</li>
      <li><b>Governança</b>: Compliance, Auditoria</li>
      <li><b>Dados do cliente</b>: Contatos, <b>Dados Pessoais (isolados p/ LGPD)</b>, Documentos e Endereços Fiscais, Societário</li>
    </ul>
  </li>
  <li><b>Auditoria</b> completa de eventos (migrações, criação/alteração de registros, movimentação de saldo, transferências).</li>
  <li><b>Stack</b>: ASP.NET Core • EF Core (SQL Server) • AutoMapper • MediatR • JWT • CORS • Kestrel • Swagger.</li>
</ul>

<blockquote>
  <b>Nota</b>: Em <b>Debug</b>, o Swagger funciona sem <i>Basic Gate</i>. Em <b>Release</b>, além do JWT, é exigido cabeçalho <b>Basic</b> (detalhes abaixo).
</blockquote>

<h2 id="autenticação--segurança">Autenticação &amp; Segurança</h2>

<details open>
  <summary><b>JWT (login)</b> — <code>POST /api/seguranca/login</code></summary>
  <p>Body:</p>
  <pre><code class="language-json">{ "usuario": "FHT", "senha": "FHT" }</code></pre>
  <p>Resposta:</p>
  <pre><code class="language-json">{ "access_token": "&lt;jwt&gt;", "token_type": "Bearer", "expires_in": 216000 }</code></pre>
  <p>Use nas chamadas:</p>
  <pre><code>Authorization: Bearer &lt;jwt&gt;</code></pre>
</details>

<details>
  <summary><b>Basic Gate (somente em Release)</b></summary>
  <p>Além do Bearer, envie <code>Authorization: Basic base64("Auth:yyyyMMdd:FHT")</code></p>
  <pre><code># Exemplo bash
BASIC=$(printf "Auth:%(date +%Y%m%d):FHT")
curl ... -H "Authorization: Basic $(echo -n "$BASIC" | base64)" -H "Authorization: Bearer &lt;jwt&gt;"</code></pre>
  <p>No <b>Swagger</b> em Release, utilize um cliente (curl/Postman) que permita setar o <b>Basic</b>.</p>
</details>

<h2 id="rate-limit--cabeçalhos">Rate Limit &amp; Cabeçalhos</h2>
<ul>
  <li><b>Limite:</b> 150 req/min</li>
  <li><b>Headers</b>: <code>X-RateLimit-Limit</code>, <code>X-RateLimit-Remaining</code>, <code>Retry-After</code></li>
  <li><b>Correlação</b>: envie opcional <code>X-Correlation-Id</code>; se ausente, a API gera e retorna.</li>
  <li>Headers de segurança: CSP, HSTS, Referrer-Policy, Permissions-Policy, X-Content-Type-Options, X-Frame-Options, etc.</li>
</ul>

<h2 id="quickstart">Quickstart</h2>

<details open>
  <summary><b>1) Login → JWT</b></summary>
  <pre><code class="language-bash">curl -X POST "https://localhost:62879/api/seguranca/login" \
  -H "Content-Type: application/json" \
  -d '{ "usuario":"FHT", "senha":"FHT" }'</code></pre>
</details>

<details>
  <summary><b>2) Criar cliente</b></summary>
  <pre><code class="language-bash">curl -X POST "https://localhost:62879/api/clientes" \
  -H "Authorization: Bearer &lt;jwt&gt;" \
  -H "Content-Type: application/json" \
  -d '{ "nome":"Rafael Antunes dos Santos Silva", "tipo":"PessoaFisica", "status":"Ativo" }'</code></pre>
</details>

<details>
  <summary><b>3) Criar conta</b> (não envie <code>contaId</code>)</summary>
  <pre><code class="language-bash">curl -X POST "https://localhost:62879/api/contas" \
  -H "Authorization: Bearer &lt;jwt&gt;" \
  -H "Content-Type: application/json" \
  -d '{ "clienteId":1, "tipo":"Corrente", "status":"Ativa",
        "agencia":"1", "numero":"1", "digito":"1", "saldo":100 }'</code></pre>
</details>

<details>
  <summary><b>4) Listar contas (filtro opcional <code>clienteId</code>)</b></summary>
  <pre><code class="language-bash">curl -X GET "https://localhost:62879/api/contas?clienteId=1" \
  -H "Authorization: Bearer &lt;jwt&gt;" \
  -H "accept: application/json"</code></pre>
</details>

<details>
  <summary><b>5) Criar transferência</b> (debita saldo automaticamente)</summary>
  <pre><code class="language-bash">curl -X POST "https://localhost:62879/api/transferencias" \
  -H "Authorization: Bearer &lt;jwt&gt;" \
  -H "Content-Type: application/json" \
  -d '{
    "clienteId":1, "contaId":1, "tipo":"Pix", "status":"Pendente",
    "valor":100, "descricao":"teste", "identificadorTransacao":"teste",
    "pixChave":"1", "bancoDestino":"1", "agenciaDestino":"1",
    "contaDestino":"1", "documentoTitularDestino":"1",
    "nomeTitularDestino":"1", "codigoBarras":"1", "linhaDigitavel":"1"
  }'</code></pre>
  <p><b>Erro esperado</b> quando não há saldo suficiente:</p>
  <pre><code class="language-json">{ "error": "Saldo insuficiente." }</code></pre>
</details>

<details>
  <summary><b>6) Consultar transferência</b></summary>
  <pre><code class="language-bash">curl -X GET "https://localhost:62879/api/transferencias/1" \
  -H "Authorization: Bearer &lt;jwt&gt;"</code></pre>
</details>

<h2 id="módulos--endpoints">Módulos &amp; Endpoints</h2>

<table>
  <thead>
    <tr><th>Módulo</th><th>Endpoints</th><th>Notas</th></tr>
  </thead>
  <tbody>
    <tr>
      <td><b>Segurança</b></td>
      <td><code>POST /api/seguranca/login</code></td>
      <td>Retorna JWT</td>
    </tr>
    <tr>
      <td><b>Auditoria</b></td>
      <td><code>GET /api/auditorias</code> • <code>GET /api/auditorias/{id}</code></td>
      <td>Trilha de eventos</td>
    </tr>
    <tr>
      <td><b>Cliente</b></td>
      <td><code>GET/POST /api/clientes</code> • <code>GET/PUT/DELETE /api/clientes/{id}</code></td>
      <td>CRUD clientes</td>
    </tr>
    <tr>
      <td><b>Conta</b></td>
      <td><code>GET/POST /api/contas</code> • <code>GET/PUT/DELETE /api/contas/{id}</code></td>
      <td><b>POST:</b> não enviar <code>contaId</code></td>
    </tr>
    <tr>
      <td><b>Transferências</b></td>
      <td><code>POST /api/transferencias</code> • <code>GET /api/transferencias/{id}</code></td>
      <td>Debita saldo automático</td>
    </tr>
    <tr>
      <td><b>Cobrança</b></td>
      <td><code>GET/POST /api/cobrancas</code> • <code>GET /api/cobrancas/{id}</code> • <code>POST /{id}/pagar</code> • <code>POST /{id}/cancelar</code> • <code>GET /{id}/comprovante</code></td>
      <td>Gestão de cobranças</td>
    </tr>
    <tr>
      <td><b>Compliance</b></td>
      <td><code>GET/POST /api/compliances</code> • <code>GET/PUT/DELETE /api/compliances/{id}</code></td>
      <td>Registros de compliance</td>
    </tr>
    <tr>
      <td><b>Comprovantes</b></td>
      <td><code>GET /api/comprovantes/{id}</code> • <code>GET /api/comprovantes/por-cobranca/{cobrancaId}</code></td>
      <td>Consulta de comprovantes</td>
    </tr>
    <tr>
      <td><b>Contato</b></td>
      <td><code>GET/POST /api/contatos</code> • <code>GET/PUT/DELETE /api/contatos/{id}</code></td>
      <td>Contatos de clientes</td>
    </tr>
    <tr>
      <td><b>Dados Pessoais</b></td>
      <td><code>GET/POST /api/dados-pessoais</code> • <code>GET /api/dados-pessoais/{id}</code> • <code>GET /api/dados-pessoais/por-cliente/{clienteId}</code> • <code>PUT/DELETE /api/dados-pessoais/{id}</code></td>
      <td><b>Isolado</b> p/ LGPD</td>
    </tr>
    <tr>
      <td><b>Docs/Endereços Fiscais</b></td>
      <td><code>/api/documentos-fiscais</code> • <code>/api/enderecos-fiscais</code></td>
      <td>CRUDs completos</td>
    </tr>
    <tr>
      <td><b>Societário</b></td>
      <td><code>/api/societarios</code></td>
      <td>Dados societários</td>
    </tr>
  </tbody>
</table>

<h2 id="auditoria">Auditoria</h2>

<details>
  <summary>Exemplo (resumido)</summary>
  <pre><code class="language-json">[
  {
    "auditoriaId": 1,
    "entidade": "__MIGRATIONS__",
    "entidadeId": "20250829203407_Criacao_Bd",
    "motivo": "Migrations aplicadas automaticamente na inicialização da API.",
    "usuarioLogin": "master",
    "correlacaoId": "36bcd617b2604d79aa008b48da0180c6",
    "sucesso": true
  },
  {
    "auditoriaId": 4,
    "entidade": "TransferenciaBancaria",
    "entidadeId": "1",
    "acao": "Outra",
    "sucesso": true
  }
]</code></pre>
</details>

<h2 id="erros--convenções">Erros &amp; Convenções</h2>

<ul>
  <li><code>200</code> OK • <code>201</code> Created • <code>400</code> Bad Request • <code>401</code> Unauthorized • <code>404</code> Not Found • <code>422</code> Unprocessable Entity • <code>429</code> Too Many Requests • <code>500</code> Internal Server Error</li>
</ul>

<details>
  <summary>Padrão de erro (<code>ProblemDetails</code>)</summary>
  <pre><code class="language-json">{
  "type": "string",
  "title": "string",
  "status": 0,
  "detail": "string",
  "instance": "string"
}</code></pre>
</details>

<details>
  <summary>Erros comuns</summary>
  <ul>
    <li><b>401 em Release</b>: faltou o token Auth</li>
    <li><b>500 na transferência</b>: saldo insuficiente.</li>
    <li><b>422</b>: payload JSON malformado ou enum inválido.</li>
  </ul>
</details>

<h2 id="execução-local">Execução Local</h2>

<details open>
  <summary><b>Rodando</b></summary>
  <pre><code class="language-bash">dotnet restore
dotnet build
dotnet run --project src/FHT.Api</code></pre>
  <p>Acesse: <kbd>https://localhost:62879/swagger</kbd></p>
</details>

<details>
  <summary><b>Ambiente</b></summary>
  <pre><code class="language-bash">ASPNETCORE_ENVIRONMENT=Development  # Swagger sem Basic Gate em Dev
# ConnectionStrings__DefaultConnection="Server=(localdb)\MSSQLLocalDB;AttachDbFilename=&lt;repo&gt;/App_Data/FHT.mdf;Trusted_Connection=True;"</code></pre>
</details>

<h2 id="banco-de-dados--migrações">Banco de Dados &amp; Migrações</h2>

<ul>
  <li>BD em <code>App_Data</code> (SQL Server LocalDB/Express).</li>
  <li>Migrações aplicadas automaticamente na inicialização (auditoria de <code>__MIGRATIONS__</code>).</li>
</ul>

<details>
  <summary>EF Core (opcional)</summary>
  <pre><code class="language-bash">dotnet tool install --global dotnet-ef
dotnet ef migrations add Criacao_Bd --project src/FHT.Infra.Data --startup-project src/FHT.Api
dotnet ef database update --project src/FHT.Infra.Data --startup-project src/FHT.Api</code></pre>
</details>

<hr>
