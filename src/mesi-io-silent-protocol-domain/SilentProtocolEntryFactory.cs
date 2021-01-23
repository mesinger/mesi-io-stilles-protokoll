using System;

namespace Mesi.Io.SilentProtocol.Domain
{
    /// <inheritdoc />
    public class SilentProtocolEntryFactory : ISilentProtocolEntryFactory
    {
        /// <inheritdoc />
        public SilentProtocolEntry Create(string suspect, string entry, string timeStamp)
        {
            if (string.IsNullOrWhiteSpace(suspect))
            {
                throw new ArgumentException("Suspect may not be null or whitespace for a new silent protocol entry");
            }
            
            if (string.IsNullOrWhiteSpace(entry))
            {
                throw new ArgumentException("Entry may not be null or whitespace for a new silent protocol entry");
            }
            
            if (string.IsNullOrWhiteSpace(timeStamp))
            {
                throw new ArgumentException("Timestamp may not be null or whitespace for a new silent protocol entry");
            }

            return new(Guid.NewGuid().ToString(), suspect, entry, timeStamp, DateTime.UtcNow);
        }
    }
}