namespace chessApi.Infrastructure.persistence.mongoDB.mapper;

public interface IMapper<Document, Model>
{
    static abstract Document ToDocument(Model model);
    static abstract Model ToModel(Document document);
}