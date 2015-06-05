using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Text.RegularExpressions;

namespace StringPatternMatching {
	public static class Utilities {
		public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action) {
			foreach (var item in enumerable) {
				action(item);
			}
		}
	}
}
