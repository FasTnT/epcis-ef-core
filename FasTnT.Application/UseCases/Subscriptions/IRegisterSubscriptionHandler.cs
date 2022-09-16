﻿using FasTnT.Domain.Model.Subscriptions;

namespace FasTnT.Application.UseCases.Subscriptions;

public interface IRegisterSubscriptionHandler
{
    Task<Subscription> RegisterSubscriptionAsync(Subscription subscription, CancellationToken cancellationToken);
}
