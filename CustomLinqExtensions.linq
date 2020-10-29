<Query Kind="Program" />

void Main()
{
	LinqExtensionTest();
}


// LINQ
static IEnumerable<string> Ranks()
{
	yield return "two";
	yield return "three";
	yield return "four";
	yield return "five";
	yield return "six";
	yield return "seven";
	yield return "eight";
	yield return "nine";
	yield return "ten";
	yield return "jack";
	yield return "queen";
	yield return "king";
	yield return "ace";
}

IEnumerable<string> Suits()
{
	yield return "diamonds";
	yield return "hearts";
	yield return "spades";
	yield return "clubs";
}

IEnumerable<int> Numbers()
{
	yield return 10;
	yield return 2;
	yield return 3;
	yield return 11;
	yield return 1;
}

IEnumerable<int> NumbersV2()
{
	yield return 3;
	yield return 4;
	yield return 5;
	yield return 6;
}

IEnumerable<Person> Persons()
{
	yield return new Person{Age=23, Name="John", LibraryID=1};
	yield return new Person{Age=19, Name="Bob", LibraryID=2};
	yield return new Person{Age=21, Name="Jane", LibraryID=2};
	yield return new Person{Age=21, Name="Barry", LibraryID=3};
	yield return new Person{Age=54, Name="Collin", LibraryID=1};
	
}

IEnumerable<Library> Libraries()
{
	yield return new Library{Id = 2, Name="Town"};
	yield return new Library{Id = 1, Name="City"};
 	yield return new Library{Id = 3, Name="National"};
} 
void LinqExtensionTest()
{
	// Test actions for each extension
	var anySuits = Suits().AnyCustom(s => !s.ToLower().Contains("s"));
	var allSuits = Suits().AllCustom(s => s.Contains("s"));
	var concatSuitsRanks = Suits().ConcatCustom(Ranks());
	var intersectNumbers = Numbers().IntersectCustom(NumbersV2());
	var unionNumbers = Numbers().UnionCustom(NumbersV2());
	var selectNumbers = Numbers().UnionCustom(NumbersV2()).SelectCustom(n => n * n);
	var countNumbers = Numbers().CountCustom();
	var maxNumbers = Numbers().MaxCustom();
	var minNumbers = Numbers().MinCustom();
	var averageNumbers = Numbers().AverageCustom();
	var orderbyNumbers = Numbers().OrderByCustom(s => s);
	var orderbyAge = Persons().OrderByCustom(a => a.Age);
	var orderbydecendingAge = Persons().OrderByDecendingCustom(a => a.Age);
	
	
	var joinLibraries = Libraries().JoinCustom(Persons(),l => l.Id, p => p.LibraryID,
		(l,p) => new {LibraryId = l.Id, LibraryName = l.Name, PersonName = p.Name, PersonAge = p.Age});
	
	
	// change var to test above extension
	var testing = joinLibraries;
	Console.WriteLine(testing);
	Console.WriteLine(testing.GetType());
	

}

public static class LinqExtension
{
	public static IEnumerable<string> WhereIsUppercase(this IEnumerable<string> strings)
	{
		
		foreach (string item in strings)
			if (item == item.ToUpper())
				yield return item;
				
	}
	
	// Where
	public static IEnumerable<T> WhereCustom<T>(this IEnumerable<T> collection, Func<T, bool> predicate)
	{
		
		foreach (var item in collection)
			if (predicate(item))
				yield return item;
				
	}

	// Any
	public static bool AnyCustom<T>(this IEnumerable<T> collection, Func<T, bool> predicate)
	{
		
		// if any of the items return true, method returns true
		foreach (var item in collection)
			if (predicate(item))
				return true;
		// if non of the items return true, method returns false	
		return false;
		
	}

	// All
	public static bool AllCustom<T>(this IEnumerable<T> collection, Func<T, bool> predicate)
	{
		
		foreach (var item in collection)
		{
			// Do nothing if the predicate returns true, otherwise return false
			if (predicate(item)) {}
			else 
				return false;
		}
		// If all conditions return true, returns true at end.
		return true;
		
	}

	// Concat
	public static IEnumerable<T> ConcatCustom<T>(this IEnumerable<T> collection, IEnumerable<T> collection2)
	{
		
		foreach (var item in collection)
			yield return item;
			
		foreach (var item in collection2)
			yield return item;
			
	}
	
	// Intersect
	public static IEnumerable<T> IntersectCustom<T>(this IEnumerable<T> collection, IEnumerable<T> collection2)
	{
		
		foreach (var item in collection)
			if(collection2.Contains(item))
				yield return item;
			
	}
	
	// Union
	public static IEnumerable<T> UnionCustom<T>(this IEnumerable<T> collection, IEnumerable<T> collection2)
	{
		
		foreach (var item in collection)
			yield return item;

		foreach (var item in collection2)
			if(!collection.Contains(item))
				yield return item;
	
	}
	
	// Select
	
	public static IEnumerable<TR> SelectCustom<T, TR>(this IEnumerable<T> collection, Func<T, TR> select)
	{
		
		foreach (var item in collection)
				yield return select(item);
				
	
	}


	// Count
	
	public static int CountCustom<T>(this IEnumerable<T> collection)
	{
		
		int count = new int();
		foreach (var item in collection)
			count++;
		
		return count;
		
	}
	
	
	// Max 
	public static T MaxCustom<T>(this IEnumerable<T> collection) where T : IComparable
	{
		
		var max = collection.FirstOrDefault();
		
		// Check if each item is bigger than the max, if it is then set the max to current item.
		foreach (var item in collection)
			if(item.CompareTo(max) == 1)
				max = item;

		return max;
		
	}
	
	// Min

	public static T MinCustom<T>(this IEnumerable<T> collection) where T : IComparable
	{
		
		var min = collection.FirstOrDefault();
		
		foreach (var item in collection)
			if(item.CompareTo(min) == 1)
				min = item;
				
		return min;
		
	}
	
	// Average
	
	public static double AverageCustom<T>(this IEnumerable<T> collection)
	{
		
		double sum = new Double();
		double count = new Double();

		foreach (var item in collection)
		{
			var itemDouble = Convert.ToDouble(item);
			sum += itemDouble;
			count++;
		}

		return sum / count;

	}

	// Join



	// Orderby

	private static IEnumerable<T> OrderByCustomInternal<T, TR>(this IEnumerable<T> collection, Func<T,TR> selectFunc, int compareValue) where TR : IComparable
	{
		
		List<T> list1 = collection.ToList();
		List<T> list2 = new List<T>();
		int counter = list1.Count();
		
		for (int i = 0; i < counter; i++)
		{
			var min = list1.FirstOrDefault();

			foreach (var item in list1)
				if (selectFunc(item).CompareTo(selectFunc(min)) == compareValue)
					min = item;
			
			list1.Remove(min);
			list2.Add(min);
			
		}
		// Returns the new ordered list
		foreach (var item in list2)
			yield return item;
		
			
	}
	
	// Order By
	public static IEnumerable<T> OrderByCustom<T,TR>(this IEnumerable<T> collection, Func<T,TR> selectFunc) where TR : IComparable
	{
		return collection.OrderByCustomInternal(selectFunc, -1);
	}
	
	// Order By Decending
	public static IEnumerable<T> OrderByDecendingCustom<T, TR>(this IEnumerable<T> collection, Func<T, TR> selectFunc) where TR : IComparable
	{
		return collection.OrderByCustomInternal(selectFunc, 1);
	}
	
	// Join
	public static IEnumerable<TResult> JoinCustom<TOuter, TSelector, TInner, TResult>(
		this IEnumerable<TOuter> outerCollection, 
		IEnumerable<TInner> innerCollection, 
		Func<TOuter, TSelector> outerSelectorFunc, 
		Func<TInner, TSelector> innerSelectorFunc,
		Func<TOuter, TInner, TResult> selectorFunc)
	{
		foreach (var outerItem in outerCollection)
			foreach (var innerItem in innerCollection)
				if(outerSelectorFunc(outerItem).Equals(innerSelectorFunc(innerItem)))
					yield return selectorFunc(outerItem,innerItem);
	}
	
}

public class Person 
{
	public string Name { get; set; }
	public int Age { get; set; }
	public int LibraryID { get; set; }
}

public class Library
{
	public string Name { get; set; }
	public int Id { get; set; }
}



