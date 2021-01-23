namespace Mesi.Io.SilentProtocol.Domain
{
    public interface ISilentProtocolEntryFactory
    {
        /// <summary>
        /// Creates a new <see cref="SilentProtocolEntry"/>
        /// </summary>
        /// <param name="suspect"></param>
        /// <param name="entry"></param>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        SilentProtocolEntry Create(string suspect, string entry, string timeStamp);
    }
}