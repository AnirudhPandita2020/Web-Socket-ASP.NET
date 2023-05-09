using Microsoft.EntityFrameworkCore;
using WebSocketChat.Model;

namespace WebSocketChat.Database;

/// <summary>
/// Implements the IMessageDataSource interface to provide access to the message data store.
/// </summary>
/// <author>Anirudh Pandita</author>
public class MessageDataSource : IMessageDataSource
{
    private readonly MessageDbContext _context;

    /// <summary>
    /// Initializes a new instance of the MessageDataSource class.
    /// </summary>
    /// <param name="context">The MessageDbContext instance.</param>
    public MessageDataSource(MessageDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Gets all messages from the message data store.
    /// </summary>
    /// <returns>A task representing the asynchronous operation that retrieves the list of messages.</returns>
    public async Task<List<Message>> GetAllMessages() =>
        await _context.Messages.OrderByDescending(message => message.TimeStamp).ToListAsync();

    /// <summary>
    /// Inserts a message into the message data store.
    /// </summary>
    /// <param name="message">The message to insert.</param>
    /// <returns>A task representing the asynchronous operation that inserts the message.</returns>
    public async Task InsertMessage(Message message)
    {
        await _context.Messages.AddAsync(message);
        await _context.SaveChangesAsync();
    }
}