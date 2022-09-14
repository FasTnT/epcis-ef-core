﻿using FasTnT.Domain.Infrastructure.Exceptions;
using FasTnT.Domain.Model.Subscriptions;
using FasTnT.Features.v1_2.Endpoints.Interfaces;

namespace FasTnT.Features.v1_2.Communication.Parsers;

public static class XmlQueryParser
{
    public static object Parse(XElement queryElement)
    {
        return queryElement?.Name?.LocalName switch
        {
            "Poll" => ParsePollQuery(queryElement),
            "GetVendorVersion" => new GetVendorVersion(),
            "GetStandardVersion" => new GetStandardVersion(),
            "GetQueryNames" => ParseGetQueryNames(),
            "Subscribe" => ParseSubscribe(queryElement),
            "Unsubscribe" => ParseUnsubscribe(queryElement),
            "GetSubscriptionIDs" => ParseGetSubscriptionIds(queryElement),
            _ => throw new EpcisException(ExceptionType.ValidationException, "Unknown Query element")
        };
    }

    public static Poll ParsePollQuery(XElement element)
    {
        var queryName = element.Element("queryName").Value;
        var parameters = ParseQueryParameters(element.Element("params")?.Elements()).ToArray();

        return new(queryName, parameters);
    }

    public static Unsubscribe ParseUnsubscribe(XElement element)
    {
        return new (element.Element("subscriptionID").Value);
    }

    public static Subscribe ParseSubscribe(XElement element)
    {
        var subscription = new Subscription
        {
            Name = element.Element("subscriptionID").Value,
            QueryName = element.Element("queryName").Value,
            Destination = element.Element("dest").Value,
            Trigger = element.Element("controls")?.Element("trigger")?.Value,
            ReportIfEmpty = bool.Parse(element.Element("controls").Element("reportIfEmpty").Value),
            InitialRecordTime = DateTime.TryParse(element.Element("controls")?.Element("initialRecordTime")?.Value ?? string.Empty, out DateTime date) ? date : default(DateTime?),
            Parameters = ParseQueryParameters(element.Element("params")?.Elements()).ToList(),
            Schedule = ParseQuerySchedule(element.Element("controls")?.Element("schedule"))
        };

        return new(subscription);
    }

    public static GetQueryNames ParseGetQueryNames() => new();

    private static IEnumerable<SubscriptionParameter> ParseQueryParameters(IEnumerable<XElement> elements)
    {
        foreach (var element in elements ?? Array.Empty<XElement>())
        {
            var name = element.Element("name")?.Value?.Trim();
            var values = element.Element("value").HasElements
                ? element.Element("value").Elements().Select(x => x.Value)
                : new[] { element.Element("value").Value };

            yield return new() { Name = name, Values = values.ToArray() };
        }
    }

    public static GetSubscriptionIDs ParseGetSubscriptionIds(XElement element)
    {
        return new(element.Element("queryName")?.Value);
    }

    private static SubscriptionSchedule ParseQuerySchedule(XElement element)
    {
        if (element == null || element.IsEmpty)
        {
            return default;
        }

        return new()
        {
            Second = element.Element("second")?.Value ?? string.Empty,
            Minute = element.Element("minute")?.Value ?? string.Empty,
            Hour = element.Element("hour")?.Value ?? string.Empty,
            Month = element.Element("month")?.Value ?? string.Empty,
            DayOfMonth = element.Element("dayOfMonth")?.Value ?? string.Empty,
            DayOfWeek = element.Element("dayOfWeek")?.Value ?? string.Empty
        };
    }
}
