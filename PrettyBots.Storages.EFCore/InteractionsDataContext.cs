using JetBrains.Annotations;

using Microsoft.EntityFrameworkCore;

using PrettyBots.Storages.Abstraction;
using PrettyBots.Storages.Abstraction.Exceptions;
using PrettyBots.Storages.Abstraction.Model;

namespace PrettyBots.Storages.EFCore;


public class InteractionsDataContext<TUser> : DbContext
    where TUser : class, IUser
{
    protected InteractionsDataContext() { }

    protected InteractionsDataContext(DbContextOptions options) : base(options)
    {
    }

    [PublicAPI]
    public DbSet<TUser> Users { get; set; } = null!;
}