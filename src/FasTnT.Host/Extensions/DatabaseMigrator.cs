﻿using FasTnT.EfCore.Store;
using Microsoft.EntityFrameworkCore;

namespace FasTnT.Host.Extensions;

public static class DatabaseMigrator
{
    public static IApplicationBuilder ApplyMigrations(this IApplicationBuilder application)
    {
        using var scope = application.ApplicationServices.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<EpcisContext>();

        context.Database.Migrate();

        return application;
    }
}
