namespace PrettyBots.Environment.Responses;

public class DateResponse : IUserResponse
{
    public DateTime Date { get; set; }

    public IEnvironment Environment { get; set; } = null!;

    public DateResponse(DateTime date)
    {
        Date = date;
    }
}