﻿namespace FasTnT.Formatter.Xml.Formatters;

public static class XElementExtensions
{
    public static void AddIfNotNull(this XElement destination, XElement children)
    {
        if (children != null && !children.IsEmpty)
        {
            destination.Add(children);
        }
    }

    public static void AddIfNotNull(this XElement destination, IEnumerable<XElement> children)
    {
        if (children != null && children.Any(x => !x.IsEmpty))
        {
            destination.Add(children.Where(x => !x.IsEmpty));
        }
    }
}