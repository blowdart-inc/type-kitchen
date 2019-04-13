﻿// Copyright (c) Blowdart, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
namespace TypeKitchen.Tests.Fakes
{
    public interface ITwoMethodsAndProperty
    {
        void Foo();
        void Bar(int i);
        string Baz { get; }
    }
}