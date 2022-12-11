﻿using FasTnT.Application.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FasTnT.Sqlite;

public static class SqliteProvider
{
    public static void Configure(IServiceCollection services, string connectionString, int commandTimeout)
    {
        services.AddDbContext<EpcisContext>(o => o.UseSqlite(connectionString, x =>
        {
            x.MigrationsAssembly(typeof(SqliteProvider).Assembly.FullName);
            x.CommandTimeout(commandTimeout);
            x.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
        }));
    }
}
