using System;
using System.Collections.Generic;

namespace Generator {
	public static class Extensions {

		public static void ForEach<T>(
			this IEnumerable<T> iterator,
			Action<T> method
		) {
			foreach(T obj in iterator) {
				method(obj);
			}
		}

		public static void ForEach<T,Y>(
			this IEnumerable<T> iterator,
			Func<T, Y> method
		) {
			iterator.ForEach(x => { method(x); });
		}

		public static bool IsNullOrEmpty(this string str) {
			return string.IsNullOrEmpty(str);
		}

		public static bool IsNullOrWhitespace(this string str) {
			return string.IsNullOrWhiteSpace(str);
		}
	}
}
