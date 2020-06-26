using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountService.Domain.Commands.Reponses;
using AccountService.Domain.Commands.Requests;
using AccountService.Domain.Entities;
using AccountService.Domain.Queries.Reponses;
using AccountService.Domain.Queries.Requests;
using AccountService.Domain.Repositories;
using AccountService.Infra.Data.Context;
using MongoDB.Driver;

namespace AccountService.Infra.Data.Repositories
{
    public class ExtractRepository : IExtractRepository
    {
        private readonly IMongoCollection<Extract> Extract;
        public ExtractRepository(IMongoContext settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            Extract = database.GetCollection<Extract>(settings.CollectionName);
        }

        public async Task<IEnumerable<Extract>> Get(long numberAccount)
        {
            var res = await Extract.FindAsync(x => x.NumberAccount == numberAccount);
            return res.ToList();
        }

        public void SyncDb(Extract account)
        {
            Extract.InsertOne(account);
        }
    }
}
