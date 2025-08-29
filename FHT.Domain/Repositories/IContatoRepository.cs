using FHT.Domain.Entities;
using FHT.Domain.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FHT.Domain.Repositories
{
    public interface IContatoRepository : IRepository<Contato>
    {
    }
}