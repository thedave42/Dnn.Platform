﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information

using System;

using DotNetNuke.Framework;

namespace DotNetNuke.Common.Internal
{
    public class TestableGlobals : ServiceLocator<IGlobals, TestableGlobals>
    {
        protected override Func<IGlobals> GetFactory()
        {
            return () => new GlobalsImpl();
        }
    }
}
