using PrettyBots.Environment;
using PrettyBots.Interactions.Abstraction.Model.Context.Errors;

namespace PrettyBots.Interactions.Abstraction.EventHandlers;

public delegate Task IncorrectUserMessageEventHandler(IIncorrectUserMessageErrorContext context);
