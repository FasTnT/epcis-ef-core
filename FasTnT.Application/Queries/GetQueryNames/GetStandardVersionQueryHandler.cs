﻿using FasTnT.Application.Services;
using FasTnT.Domain.Queries;
using MediatR;

namespace FasTnT.Application.Queries;

public class GetQueryNamesQueryHandler : IRequestHandler<GetQueryNamesQuery, IEpcisResponse>
{
    private readonly IEnumerable<IEpcisQuery> _queries;

    public GetQueryNamesQueryHandler(IEnumerable<IEpcisQuery> queries)
    {
        _queries = queries;
    }

    public Task<IEpcisResponse> Handle(GetQueryNamesQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult<IEpcisResponse>(new GetQueryNamesResult(_queries.Select(x => x.Name)));
    }
}
