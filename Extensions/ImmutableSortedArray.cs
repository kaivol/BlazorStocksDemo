using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace GitHubBlazor.Extensions
{
    public static class ImmutableSortedList
    {
        public static ImmutableSortedArray<T> ToImmutableSortedArray<T>(this SortedSet<T> sortedSet)
        {
            return new ImmutableSortedArray<T>(sortedSet.ToImmutableArray(), sortedSet.Comparer);
        }
    }
    
    public class ImmutableSortedArray<T> : IImmutableList<T>
    {
        private readonly ImmutableArray<T> immutableListImplementation;
        public IComparer<T> Comparer { get; }

        internal ImmutableSortedArray(
            ImmutableArray<T> immutableListImplementation, 
            IComparer<T> sortedSetComparer
            )
        {
            this.immutableListImplementation = immutableListImplementation;
            this.Comparer = sortedSetComparer;
        }

        public ImmutableArray<T>.Enumerator GetEnumerator()
        {
            return immutableListImplementation.GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return ((IEnumerable<T>)immutableListImplementation).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)immutableListImplementation).GetEnumerator();
        }

        public int Count => immutableListImplementation.Length;

        public T this[int index] => immutableListImplementation[index];

        public IImmutableList<T> Add(T value)
        {
            return immutableListImplementation.Add(value);
        }

        public IImmutableList<T> AddRange(IEnumerable<T> items)
        {
            return immutableListImplementation.AddRange(items);
        }

        public IImmutableList<T> Clear()
        {
            return immutableListImplementation.Clear();
        }

        public int IndexOf(T item, int index, int count, IEqualityComparer<T>? equalityComparer)
        {
            return immutableListImplementation.IndexOf(item, index, count, equalityComparer);
        }

        public IImmutableList<T> Insert(int index, T element)
        {
            return immutableListImplementation.Insert(index, element);
        }

        public IImmutableList<T> InsertRange(int index, IEnumerable<T> items)
        {
            return immutableListImplementation.InsertRange(index, items);
        }

        public int LastIndexOf(T item, int index, int count, IEqualityComparer<T>? equalityComparer)
        {
            return immutableListImplementation.LastIndexOf(item, index, count, equalityComparer);
        }

        public IImmutableList<T> Remove(T value, IEqualityComparer<T>? equalityComparer)
        {
            return immutableListImplementation.Remove(value, equalityComparer);
        }

        public IImmutableList<T> RemoveAll(Predicate<T> match)
        {
            return immutableListImplementation.RemoveAll(match);
        }

        public IImmutableList<T> RemoveAt(int index)
        {
            return immutableListImplementation.RemoveAt(index);
        }

        public IImmutableList<T> RemoveRange(IEnumerable<T> items, IEqualityComparer<T>? equalityComparer)
        {
            return immutableListImplementation.RemoveRange(items, equalityComparer);
        }

        public IImmutableList<T> RemoveRange(int index, int count)
        {
            return immutableListImplementation.RemoveRange(index, count);
        }

        public IImmutableList<T> Replace(T oldValue, T newValue, IEqualityComparer<T>? equalityComparer)
        {
            return immutableListImplementation.Replace(oldValue, newValue, equalityComparer);
        }

        public IImmutableList<T> SetItem(int index, T value)
        {
            return immutableListImplementation.SetItem(index, value);
        }
    }
}