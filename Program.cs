using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core;
using MongoDB.Driver.Linq;
using MongoDB.Bson.Serialization;
using System.Threading;
using System.Configuration;

namespace MongoDBConnector
{
    class Program
    {
        static IDBCollection objDBCollection;
        static Program()
        {
            objDBCollection = new SuperHeroDBCollection();
        }
        static void Main(string[] args)
        {
            //format : mongodb://user:pwd@server/Database
            string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();

            MongoClient mClient = null;

            try
            {
                mClient = new MongoClient(connectionString);
                var database = mClient.GetDatabase("MyDatabase");
                var collection = database.GetCollection<BsonDocument>("Avengers");

                objDBCollection.DisplayRecords(collection);

                //Display the count
                var query = collection.AsQueryable<BsonDocument>().ToList();
                Console.WriteLine(query.Count);

                InsertOneOptions option = new InsertOneOptions();

                //Insert a record
                var foundResult = objDBCollection.GetRecordByName("Wonda Maximoff", collection);

                if (foundResult.Count == 0)
                {
                    BsonDocument doc = new BsonDocument();
                    doc.Add(new BsonElement("name", "Wonda Maximoff"));
                    doc.Add(new BsonElement("alias", "Scarlet Witch"));
                    doc.Add(new BsonElement("superpower", true));

                    collection.InsertOneAsync(doc);
                }
                //Insert Many record
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

                //BsonDocument[] multiData = objDBCollection.MultipleInputDataToInsert();

                collection.InsertManyAsync(multiData);

                //Update existing record
                var filter = Builders<BsonDocument>.Filter.Eq("name", "Tony Stark");
                var update = Builders<BsonDocument>.Update.Set("superpower", false);

                collection.UpdateOneAsync(filter, update);

                objDBCollection.DisplayRecords(collection);

                var filterToDelete = Builders<BsonDocument>.Filter.Eq("name", "Wonda Maximoff");

                //Delete a record
                collection.DeleteOneAsync(filterToDelete);

                objDBCollection.DisplayRecords(collection);

                //Find with single parameter
                var result = objDBCollection.GetRecordByName("Tony Stark", collection);

                objDBCollection.DisplayRecord(result);

                ////Find with multiple parameter
                var result2 = objDBCollection.GetRecordByNameAlias("Steve Rogers", "Captain America", collection);

                objDBCollection.DisplayRecord(result2);

                var filter1 = Builders<BsonDocument>.Filter.Eq("name", "Peter Parker");
                var filter2 = Builders<BsonDocument>.Filter.Eq("name", "Clint Barton");
                var filter3 = Builders<BsonDocument>.Filter.Eq("name", "Bucky Barnes");

                collection.DeleteManyAsync(filter1);
                collection.DeleteManyAsync(filter2);
                collection.DeleteManyAsync(filter3);

            }
            catch (MongoClientException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (MongoServerException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }

        }

    }
}
