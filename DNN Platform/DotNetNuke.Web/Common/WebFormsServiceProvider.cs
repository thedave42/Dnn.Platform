// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information
namespace DotNetNuke.Web.Common
{
    using System;
    using System.Reflection;
    using System.Web;

    using DotNetNuke.Common;
    using DotNetNuke.Common.Extensions;
    using DotNetNuke.Instrumentation;

    using Microsoft.Extensions.DependencyInjection;

    /// <summary>Service provider implementation for <see cref="HttpRuntime.WebObjectActivator"/>.</summary>
    internal class WebFormsServiceProvider : IServiceProvider
    {
        private const BindingFlags ActivatorFlags =
            BindingFlags.Instance |
            BindingFlags.NonPublic |
            BindingFlags.Public |
            BindingFlags.CreateInstance;

        private static readonly ILog Logger = LoggerSource.Instance.GetLogger(typeof(WebFormsServiceProvider));

        private readonly IServiceProvider globalServiceProvider;

        /// <summary>Initializes a new instance of the <see cref="WebFormsServiceProvider"/> class.</summary>
        /// <param name="globalServiceProvider">The service provider to use to create scopes for each request.</param>
        public WebFormsServiceProvider(IServiceProvider globalServiceProvider)
        {
            this.globalServiceProvider = globalServiceProvider;
        }

        /// <inheritdoc />
        public object GetService(Type serviceType)
        {
            IServiceProvider provider = null;
            if (HttpContextSource.Current != null)
            {
                var scope = HttpContextSource.Current.GetScope();
                if (scope == null)
                {
                    HttpContextSource.Current.SetScope(this.globalServiceProvider);
                    scope = HttpContextSource.Current.GetScope();
                }

                provider = scope.ServiceProvider;
            }

            return provider != null && (serviceType.IsInterface || serviceType.GetConstructors().Length > 0)
                       ? ActivatorUtilities.GetServiceOrCreateInstance(provider, serviceType)
                       : Activator.CreateInstance(serviceType, ActivatorFlags, null, null, null);
        }
    }
}
