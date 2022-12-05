﻿using FasTnT.Application.EfCore.Services.Queries;
using FasTnT.Application.EfCore.Services.Subscriptions;
using FasTnT.Application.EfCore.UseCases.Captures;
using FasTnT.Application.EfCore.UseCases.Queries;
using FasTnT.Application.EfCore.UseCases.Subscriptions;
using FasTnT.Application.EfCore.UseCases.TopLevelResources;
using FasTnT.Application.Services.Queries;
using FasTnT.Application.Services.Subscriptions;
using FasTnT.Application.UseCases.Captures;
using FasTnT.Application.UseCases.Queries;
using FasTnT.Application.UseCases.Subscriptions;
using FasTnT.Application.UseCases.TopLevelResources;
using FasTnT.EfCore.Store;
using FasTnT.EfCore.Store.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FasTnT.Application.EfCore;

public static class EpcisConfiguration
{
    public static IServiceCollection AddEpcisServices(this IServiceCollection services, Action<EpcisOptions> configure)
    {
        var options = new EpcisOptions();
        if (configure is not null)
        {
            configure(options);
        }

        services.AddDbContext<EpcisContext>(o =>
        {
            if (SqlProvider.SqlServer == options.Provider)
            {
                o.UseSqlServer(options.ConnectionString, x => x.MigrationsAssembly(SqlProvider.SqlServer.Assembly));
            }
            if (SqlProvider.Postgres == options.Provider)
            {
                o.UseNpgsql(options.ConnectionString, x => x.MigrationsAssembly(SqlProvider.Postgres.Assembly));
            }
            if (SqlProvider.Sqlite == options.Provider)
            {
                o.UseSqlite(options.ConnectionString, x => x.MigrationsAssembly(SqlProvider.Sqlite.Assembly));
            }
        });

        services.AddSqlServer<EpcisContext>(options.ConnectionString, opt =>
        {
            opt.EnableRetryOnFailure().CommandTimeout(options.CommandTimeout);
            opt.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
        });
        services.AddScoped<IncrementGenerator.Identity>();
        services.AddScoped<IEpcisDataSource, SimpleEventQuery>();
        services.AddScoped<IEpcisDataSource, SimpleMasterDataQuery>();
        services.AddScoped(options.CurrentUser);

        services.AddTransient<IListCaptureRequestsHandler, CaptureUseCasesHandler>();
        services.AddTransient<ICaptureRequestDetailsHandler, CaptureUseCasesHandler>();
        services.AddTransient<ICaptureRequestHandler, CaptureUseCasesHandler>();
        services.AddTransient<IListQueriesHandler, QueriesUseCasesHandler>();
        services.AddTransient<IGetQueryDetailsHandler, QueriesUseCasesHandler>();
        services.AddTransient<IStoreQueryHandler, QueriesUseCasesHandler>();
        services.AddTransient<IDeleteQueryHandler, QueriesUseCasesHandler>();
        services.AddTransient<IExecuteQueryHandler, QueriesUseCasesHandler>();
        services.AddTransient<ITriggerSubscriptionHandler, SubscriptionsUseCasesHandler>();
        services.AddTransient<IDeleteSubscriptionHandler, SubscriptionsUseCasesHandler>();
        services.AddTransient<IListSubscriptionsHandler, SubscriptionsUseCasesHandler>();
        services.AddTransient<IRegisterSubscriptionHandler, SubscriptionsUseCasesHandler>();
        services.AddTransient<IGetSubscriptionDetailsHandler, SubscriptionsUseCasesHandler>();
        services.AddTransient<IListEpcsHandler, TopLevelResourceUseCasesHandler>();
        services.AddTransient<IListBizLocationsHandler, TopLevelResourceUseCasesHandler>();
        services.AddTransient<IListBizStepsHandler, TopLevelResourceUseCasesHandler>();
        services.AddTransient<IListEventTypesHandler, TopLevelResourceUseCasesHandler>();
        services.AddTransient<IListReadPointsHandler, TopLevelResourceUseCasesHandler>();
        services.AddTransient<IListDispositionsHandler, TopLevelResourceUseCasesHandler>();

        services.AddHealthChecks().AddDbContextCheck<EpcisContext>();

        if (!services.Any(x => typeof(ISubscriptionListener).IsAssignableFrom(x.ServiceType)))
        {
            services.AddSingleton<ISubscriptionListener, NoOpSubscriptionListener>();
        }

        return services;
    }

    public static IServiceCollection AddEpcisSubscriptionServices(this IServiceCollection services, params IResultSender[] resultSenders)
    {
        services.AddTransient<ISubscriptionRunner, SubscriptionRunner>();
        services.AddSingleton<ISubscriptionService, SubscriptionService>();
        services.AddSingleton<IEnumerable<IResultSender>>(resultSenders);

        services.AddSingleton<ISubscriptionListener>(ctx => ctx.GetService<ISubscriptionService>());

        return services;
    }

    private class NoOpSubscriptionListener : ISubscriptionListener
    {
    }
}
