using WebSocketChat.Model;

namespace WebSocketChat.Database;

/// <summary>
/// Interface for a message data source.
/// </summary>
/// <author>Anirudh Pandita</author>
public interface IMessageDataSource
{
    /// <summary>
    /// Gets all messages.
    /// </summary>
    /// <returns>A list of messages.</returns>
    Task<List<Message>> GetAllMessages();

    /// <summary>
    /// Inserts a message into the data source.
    /// </summary>
    /// <param name="message">The message to insert.</param>
    Task InsertMessage(Message message);
}
