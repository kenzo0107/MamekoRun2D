using System;
using System.Linq;
using System.Collections;
using System.Reflection;

public class AotSafe{
	
	/// <summary>
	/// Fors the each.
	/// </summary>
	/// <param name="enumerable">Enumerable.</param>
	/// <param name="action">Action.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public static void ForEach<T>(object enumerable, Action<T> action){
		if (enumerable == null){
			return;
		}
		Type listType = enumerable.GetType().GetInterfaces().First(x => !(x.IsGenericType) && x == typeof(IEnumerable));
		if (listType == null){
			throw new ArgumentException("Object does not implement IEnumerable interface", "enumerable");
		}

		MethodInfo method = listType.GetMethod("GetEnumerator");
		if (method == null){
			throw new InvalidOperationException("Failed to get 'GetEnumberator()' method info from IEnumerable type");
		}

		IEnumerator enumerator = null;

		try{
			enumerator = (IEnumerator)method.Invoke(enumerable, null);
			if (enumerator is IEnumerator){
				while (enumerator.MoveNext()){
					action((T)enumerator.Current);
				}
			}else{
				UnityEngine.Debug.Log(string.Format("{0}.GetEnumerator() returned '{1}' instead of IEnumerator.",
					enumerable.ToString(),
					enumerator.GetType().Name));
			}
		}
		finally{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable != null){
				disposable.Dispose();
			}
		}
	}
}