using Core.Enumerations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Core.Entities.Mongo;
public class Notes
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("_id")]
    public string? Id { get; set; }

    [BsonElement("title")]
    public string? Title { get; set; }

    [BsonElement("state")]
    public double State { get; set; }

    [BsonElement("creationDate")]
    public string? CreationDate { get; set; }

    [BsonElement("lastUpdateDate")]
    public string? LastUpdateDate { get; set; }

    [BsonElement("creatorUser")]
    public string? CreatorUser { get; set; }

    [BsonElement("updaterUser")]
    public string? UpdaterUser { get; set; }

    [BsonElement("listId")]
    public string? ListId { get; set; }

}