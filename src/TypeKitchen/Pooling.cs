﻿// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.ObjectPool;

namespace TypeKitchen
{
	public static class Pooling
	{
		public static class StringBuilderPool
		{
			private static readonly ObjectPool<StringBuilder> Pool =
				new LeakTrackingObjectPool<StringBuilder>(new DefaultObjectPool<StringBuilder>(
					new StringBuilderPooledObjectPolicy()
				));

			public static StringBuilder Get()
			{
				return Pool.Get();
			}

			public static void Return(StringBuilder obj)
			{
				Pool.Return(obj);
			}

			public static string Scoped(Action<StringBuilder> closure)
			{
				var sb = Pool.Get();
				try
				{
					closure(sb);
					return sb.ToString();
				}
				finally
				{
					Pool.Return(sb);
				}
			}

			public static string Scoped(Action<StringBuilder> closure, int startIndex, int length)
			{
				var sb = Pool.Get();
				try
				{
					closure(sb);
					return sb.ToString(startIndex, length);
				}
				finally
				{
					Pool.Return(sb);
				}
			}
		}

		public static class ListPool<T>
		{
			private static readonly ObjectPool<List<T>> Pool =
				new LeakTrackingObjectPool<List<T>>(
					new DefaultObjectPool<List<T>>(new CollectionObjectPolicy<List<T>, T>())
				);

			public static List<T> Get()
			{
				return Pool.Get();
			}

			public static void Return(List<T> obj)
			{
				Pool.Return(obj);
			}
		}

		public static class HashSetPool<T>
		{
			private static readonly ObjectPool<HashSet<T>> Pool =
				new LeakTrackingObjectPool<HashSet<T>>(
					new DefaultObjectPool<HashSet<T>>(
						new CollectionObjectPolicy<HashSet<T>, T>())
				);

			public static HashSet<T> Get()
			{
				return Pool.Get();
			}

			public static void Return(HashSet<T> obj)
			{
				Pool.Return(obj);
			}
		}

		public static class HashSetPool
		{
			private static readonly ObjectPool<HashSet<string>> Pool =
				new LeakTrackingObjectPool<HashSet<string>>(
					new DefaultObjectPool<HashSet<string>>(
						new StringSetPolicy<OrdinalIgnoreCaseComparer>())
				);

			public static HashSet<string> Get()
			{
				return Pool.Get();
			}

			public static void Return(HashSet<string> obj)
			{
				Pool.Return(obj);
			}
		}

		#region Policies

		/// <summary>
		///     The default policy provided by Microsoft uses new T() constraint,
		///     which silently defers to Activator.CreateInstance.
		/// </summary>
		private class DefaultObjectPolicy<T> : IPooledObjectPolicy<T>
		{
			public T Create()
			{
				return CreateNew();
			}

			public bool Return(T obj)
			{
				return true;
			}

			private static T CreateNew()
			{
				return Instancing.CreateInstance<T>();
			}
		}

		private class CollectionObjectPolicy<TCollection, TElement> : IPooledObjectPolicy<TCollection>
			where TCollection : ICollection<TElement>
		{
			public TCollection Create()
			{
				return Instancing.CreateInstance<TCollection>();
			}

			public bool Return(TCollection collection)
			{
				collection.Clear();
				return collection.Count == 0;
			}
		}

		private interface IStringSetComparer : IEqualityComparer<string>
		{
			StringComparer Comparer { get; }
		}

		// ReSharper disable once ClassNeverInstantiated.Local
		private class OrdinalIgnoreCaseComparer : IStringSetComparer
		{
			public bool Equals(string x, string y)
			{
				return Comparer.Equals(x, y);
			}

			public int GetHashCode(string obj)
			{
				return Comparer.GetHashCode(obj);
			}

			public StringComparer Comparer => StringComparer.OrdinalIgnoreCase;
		}

		private class StringSetPolicy<T> : IPooledObjectPolicy<HashSet<string>> where T : IStringSetComparer
		{
			public HashSet<string> Create()
			{
				return new HashSet<string>(
					Instancing.CreateInstance<T>());
			}

			public bool Return(HashSet<string> set)
			{
				set.Clear();
				return set.Count == 0;
			}
		}

		#endregion
	}
}