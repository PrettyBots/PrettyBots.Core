using System.Collections.Concurrent;
using System.Reflection;

using Microsoft.Extensions.Logging;

using PrettyBots.Environment;
using PrettyBots.Environment.Model;
using PrettyBots.Environment.Telegram;
using PrettyBots.Interactions.Abstraction;
using PrettyBots.Interactions.Abstraction.Services;
using PrettyBots.Storages.Abstraction;

using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace PrettyBots.Interactions.Telegram;

public class TelegramInteractionService : InteractionService<TelegramUserMessage>
{
    private static ConcurrentDictionary<long, MediaGroupHandlerContext> _userContexts = new ConcurrentDictionary<long, MediaGroupHandlerContext>();

    public TelegramInteractionService(IEnvironment environment) : base(environment)
    {
        Loader.LoadResponseParsers(Assembly.GetAssembly(typeof(TelegramInteractionService))!);
    }

    public TelegramInteractionService(
        ILogger<InteractionService<TelegramUserMessage>> logger, IEnvironment environment,
        IEntitiesLoader loader, ILoadedEntitiesRegistry registry, IConfigurationService config,
        IStorageProvider storage)
        : base(logger, environment, loader, registry, config, storage)
    {
        Loader.LoadResponseParsers(Assembly.GetAssembly(typeof(TelegramInteractionService))!);
    }

    public async Task<bool> HandleMediaMessage(Update update)
    {
        if (update.Message?.Type != MessageType.Photo) {
            return false;
        }
        
        Message response = update.Message;
        if (response.Type == MessageType.Photo) {
            PhotoEntity photo = new PhotoEntity() {
                Caption = response.Caption,
                Sizes = new List<PhotoEntitySize>(response.Photo!.Select(p => 
                    new PhotoEntitySize() {
                        Height   = p.Height,
                        Width    = p.Width,
                        FileId   = p.FileId,
                        FileSize = p.FileSize,
                    }
                )),
            };
            TelegramUserMessage userMessage = new TelegramUserMessage(update, null, photo);
            await HandleUserMessageAsync(userMessage);
        } else {
            throw new NotImplementedException();
        }

        return true;
    }

    public bool HandleMediaGroupMessage(Update update)
    {
        if (update.Message?.MediaGroupId is null) {
            return false;
        }
        
        Message response = update.Message;
        PhotoEntity photo;

        if (_userContexts.TryGetValue(response.From!.Id, out MediaGroupHandlerContext? context)) {
            if (context.MediaGroupId != response.MediaGroupId!) {
                _userContexts.TryRemove(response.From!.Id, out _);
                context = null;
            } else {
                context.FuckDurovTimer.Change(TimeSpan.FromSeconds(5), Timeout.InfiniteTimeSpan);
            }
        }
        
        if (response.Type == MessageType.Photo) {
            photo = new PhotoEntity() {
                Caption = response.Caption,
                Sizes = new List<PhotoEntitySize>(response.Photo!.Select(p => 
                    new PhotoEntitySize() {
                        Height   = p.Height,
                        Width    = p.Width,
                        FileId   = p.FileId,
                        FileSize = p.FileSize,
                    }
                )),
            };
        } else {
            throw new NotImplementedException();
        }

        if (context is null) {
            context = new MediaGroupHandlerContext(new List<IMediaEntity>(), new List<Update>(), 
                update.Message.MediaGroupId, this, response.From.Id);

            _userContexts.TryAdd(update.Message.From!.Id, context);
        }
        
        context.MediaEntities.Add(photo);
        context.Updates.Add(update);
        
        return true;
    }
    
    private class MediaGroupHandlerContext
    {
        public List<IMediaEntity> MediaEntities  { get; set; } 
        public List<Update> Updates { get; set; }
        public string MediaGroupId { get; set; }

        public Timer FuckDurovTimer;

        public MediaGroupHandlerContext(List<IMediaEntity> mediaEntities, List<Update> updates, string mediaGroupId,
            TelegramInteractionService service, long userId)
        {
            MediaEntities = mediaEntities;
            MediaGroupId  = mediaGroupId;
            Updates  = updates;
            FuckDurovTimer = new Timer(async t => {
                Tuple<TelegramInteractionService, long> state = (Tuple<TelegramInteractionService, long>)t!;
                TelegramUserMessage userMessage = new TelegramUserMessage(Updates.Last(), MediaEntities);
                await state.Item1.HandleUserMessageAsync(userMessage);
            }, new Tuple<TelegramInteractionService, long>(service, userId), TimeSpan.FromSeconds(5), Timeout.InfiniteTimeSpan);
        }
    }
}