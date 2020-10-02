// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information
namespace DotNetNuke.Common.Extensions
{
    using System;
    using System.Collections;
    using System.Web;

    using Microsoft.Extensions.DependencyInjection;

    public static class HttpContextDependencyInjectionExtensions
    {
        private static readonly Type ScopeKey = typeof(IServiceScope);

        public static IServiceScope GetScope(this HttpContextBase httpContext)
        {
            return GetScope(httpContext.Items);
        }

        public static IServiceScope GetScope(this HttpContext httpContext)
        {
            return GetScope(httpContext.Items);
        }

        internal static void SetScope(this HttpContextBase httpContext, IServiceProvider provider)
        {
            SetScope(httpContext.Items, provider);
        }

        internal static void SetScope(this HttpContext httpContext, IServiceProvider provider)
        {
            SetScope(httpContext.Items, provider);
        }

        internal static void ClearScope(this HttpContextBase httpContext)
        {
            ClearScope(httpContext.Items);
        }

        internal static void ClearScope(this HttpContext httpContext)
        {
            ClearScope(httpContext.Items);
        }

        internal static IServiceScope GetScope(IDictionary contextItems)
        {
            if (!contextItems.Contains(ScopeKey))
            {
                return null;
            }

            return contextItems[ScopeKey] is IServiceScope scope ? scope : null;
        }

        private static void SetScope(IDictionary items, IServiceProvider provider)
        {
            lock (items.SyncRoot)
            {
                if (items.Contains(ScopeKey))
                {
                    return;
                }

                items[ScopeKey] = provider.CreateScope();
            }
        }

        private static void ClearScope(IDictionary items)
        {
            IDisposable scope;
            lock (items.SyncRoot)
            {
                scope = items[ScopeKey] as IDisposable;
                items.Remove(ScopeKey);
            }

            scope?.Dispose();
        }
    }
}
