namespace TronClient.Http
{
    public interface IHttpClient
    {
        Task<T> PostDeserializingResponseAsync<T>(string uri, object? req);
    }
}