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
        private readonly IMongoCollection<TransferAccountRequest> Extract;
        public ExtractRepository(IMongoContext settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            Extract = database.GetCollection<TransferAccountRequest>(settings.CollectionName);
        }

        public Task<IEnumerable<ExtractAccountResponse>> Get(ExtractAccountRequest account)
        {
            var res = Extract.Find(extract => extract.From.Number == account.NumberAccountFrom)
                            .ToList()
                            .Select(s => new ExtractAccountResponse { 
                                NumberAccountFrom = s.From.Number,
                                NumberAccountTo = s.To.Number,
                            });
            return Task.FromResult(res);
        }

        public void SyncDb(TransferAccountRequest account)
        {
            Extract.InsertOne(account);
        }
    }
}
