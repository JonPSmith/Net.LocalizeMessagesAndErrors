﻿// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using LocalizeMessagesAndErrors;

namespace Test.StubClasses;

public class StubSimpleLocalizerOptions<TResource> : SimpleLocalizerOptions
{
    public StubSimpleLocalizerOptions()
    {
        ResourceType = typeof(TResource);
    }
}