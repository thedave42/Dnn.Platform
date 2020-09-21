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

    /// <summary>
    /// Service provider implementation for <see cref="HttpRuntime.WebObjectActivator"/>.
    /// </summary>
    internal class WebFormsServiceProvider : IServiceProvider
    {
    	private static readonly ILog Logger = LoggerSource.Instance.GetLogger(typeof(WebFormsServiceProvider));
    
        private const BindingFlags ActivatorFlags =
            BindingFlags.Instance |
            BindingFlags.NonPublic |
            BindingFlags.Public |
            BindingFlags.CreateInstance;

        /// <inheritdoc />
        public object GetService(Type serviceType)
        {
        	bool hasHttpContext;
        	bool hasScope;
            IServiceProvider provider = null;
            if (HttpContextSource.Current == null) {
            	hasHttpContext = false;
            	hasScope = false;
            } else {
            	hasHttpContext = true;
            	var scope = HttpContextSource.Current.GetScope();
            	if (scope == null) {
            		hasScope = false;
            	} else {
            		hasScope = true;
            		provider = scope.ServiceProvider;
            	}
            }
            
            Logger.WarnFormat("Resolving {0} with {1} (context: {2}, scope: {3}, interface: {4}, constructors: {5})", serviceType, provider, hasHttpContext, hasScope, serviceType.IsInterface, serviceType.GetConstructors().Length);

            return provider != null && (serviceType.IsInterface || serviceType.GetConstructors().Length > 0)
                ? ActivatorUtilities.GetServiceOrCreateInstance(provider, serviceType)
                : Activator.CreateInstance(serviceType, ActivatorFlags, null, null, null);
        }
    }
}
