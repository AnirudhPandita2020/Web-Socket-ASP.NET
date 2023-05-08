using Microsoft.EntityFrameworkCore;
using WebSocketChat.Model;

namespace WebSocketChat.Database;

/// <summary>
/// Represents the database context for the message data store.
/// </summary>
/// <author>Anirudh Pandita</author>
public class MessageDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the MessageDbContext class with the specified options.
    /// </summary>
    /// <param name="options">The options for this context.</param>
    public MessageDbContext(DbContextOptions<MessageDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Gets or sets the messages in the message data store.
    /// </summary>
    public DbSet<Message> Messages { get; set; }
}
