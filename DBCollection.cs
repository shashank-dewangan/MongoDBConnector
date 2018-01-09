using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MongoDBConnector
{
    public abstract class DBCollection : IDBCollection
    {
        public virtual void DisplayRecord(List<BsonDocument> records)
        {
            throw new NotImplementedException();
        }

        public virtual void DisplayRecords(IMongoCollection<BsonDocument> collection)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<BsonDocument> Find(string findCriteriaInJson, IMongoCollection<BsonDocument> _collection)
        {
            if (string.IsNullOrWhiteSpace(findCriteriaInJson))
                throw new ArgumentException("findCriteriaInJson cannot be null or empty");
            return (IEnumerable<BsonDocument>)_collection.Find<BsonDocument>((FilterDefinition<BsonDocument>)findCriteriaInJson, (FindOptions)null).ToListAsync<BsonDocument>(new CancellationToken()).Result;
        }
        
        public virtual List<BsonDocument> GetRecordByName(string name, IMongoCollection<BsonDocument> _collection)
        {
            throw new NotImplementedException();
        }

        public virtual List<BsonDocument> GetRecordByNameAlias(string name, string alias, IMongoCollection<BsonDocument> _collection)
        {
            throw new NotImplementedException();
        }
    }
}
