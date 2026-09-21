using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace chessApi.Infrastructure.persistence.mongoDB.document;

public class UserDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("email")]
    public string Email { get; set; } = string.Empty;

    [BsonElement("password")]
    public string Password { get; set; } = string.Empty;

    [BsonElement("registerDate")]
    public DateTime RegisterDate { get; set; } = DateTime.UtcNow;
    
    [BsonElement("isDeleted")]
    public bool IsDeleted { get; set; }
}