using MongoDB.Driver;

namespace Stock.API.Services
{
    public class MongoDbService
    {
        readonly IMongoDatabase _Db;
        

        public MongoDbService(IConfiguration configuration)
        {
            MongoClient client = new MongoClient(configuration.GetConnectionString("MongoDB"));
            _Db = client.GetDatabase("StockAPIDB");
        }

        public IMongoCollection<T> GetCollection<T>()
        {
            return _Db.GetCollection<T>(typeof(T).Name.ToLowerInvariant());
        }
    }
}
