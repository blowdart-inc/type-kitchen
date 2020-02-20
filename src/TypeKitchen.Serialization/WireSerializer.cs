﻿// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace TypeKitchen.Serialization
{
	public class WireSerializer : IObjectSerializer
	{
		public ReadOnlySpan<byte> ToBuffer(object instance, ITypeResolver typeResolver) => Wire.ObjectToBuffer(instance, typeResolver);
	}
}