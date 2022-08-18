namespace Mesi.Io.SilentProtocol.Application
{
    public record User(string Id, string Name);
    
    public interface IRequestContext
    {
        User User();
    }
}