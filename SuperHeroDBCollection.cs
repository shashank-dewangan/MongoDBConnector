using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBConnector
{
    public class SuperHeroDBCollection : DBCollection
    {
        public ObjectId _id { get; set; }
        public string name { get; set; }
        public string alias { get; set; }
        public bool superpower { get; set; }
        public string extracolumn { get; set; }

        public List<SuperHeroDBCollection> _lstDBCollection { get; set; }

        public override void DisplayRecords(IMongoCollection<BsonDocument> collection)
        {
            var query = collection.AsQueryable<BsonDocument>().ToList();

            foreach (var item in query)
            {
                var myObj = BsonSerializer.Deserialize<SuperHeroDBCollection>(item);
                Console.WriteLine(myObj.name + " aka " + myObj.alias + " " + (myObj.superpower ? "has super power" : ""));
            }
        }
        public override void DisplayRecord(List<BsonDocument> records)
        {
            foreach (var record in records)
            {
                var myObj = BsonSerializer.Deserialize<SuperHeroDBCollection>(record);
                Console.WriteLine();
                Console.WriteLine(" Found record for : " + myObj.name + " aka " + myObj.alias + " " + (myObj.superpower ? "has super power" : ""));
            }
        }
        public override BsonDocument[] MultipleInputDataToInsert()
        {
            BsonDocument doc1 = new BsonDocument();
            doc1.Add(new BsonElement("name", "Peter Parker"));
            doc1.Add(new BsonElement("alias", "Spiderman"));
            doc1.Add(new BsonElement("superpower", true));

            BsonDocument doc2 = new BsonDocument();
            doc2.Add(new BsonElement("name", "Clint Barton"));
            doc2.Add(new BsonElement("alias", "Hawkeye"));
            doc2.Add(new BsonElement("superpower", false));

            BsonDocument doc3 = new BsonDocument();
            doc3.Add(new BsonElement("name", "Bucky Barnes"));
            doc3.Add(new BsonElement("alias", "Winter Soldier"));
            doc3.Add(new BsonElement("superpower", true));

            BsonDocument[] multiData = { doc1, doc2, doc3 };

            return multiData;
        }
    }
}
