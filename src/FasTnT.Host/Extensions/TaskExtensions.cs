﻿using System.Reflection;

namespace FasTnT.Host.Extensions;

public static class TaskExtensions
{
    public static Task<object> CastTask(this object taskObj)
    {
        if (taskObj is not Task)
        {
            return Task.FromResult(taskObj);
        }

        var resultType = taskObj.GetType().GenericTypeArguments.First();
        var castTaskMethodGeneric = typeof(TaskExtensions).GetMethod(nameof(CastTaskInner), BindingFlags.Static | BindingFlags.Public);
        var castTaskMethod = castTaskMethodGeneric.MakeGenericMethod(resultType, typeof(object));

        return castTaskMethod.Invoke(null, [taskObj]) as Task<object>;
    }

    public static async Task<TResult> CastTaskInner<T, TResult>(Task<T> task)
    {
        var result = await task as object;

        return (TResult)result;
    }
}
