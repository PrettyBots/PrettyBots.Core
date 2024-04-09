using System.Security.Cryptography;

namespace PrettyBots.Environment.Test;

public class TestMessage : IUserMessage
{
    public long UserId { get; } = RandomNumberGenerator.GetInt32(0, 1000000);
}