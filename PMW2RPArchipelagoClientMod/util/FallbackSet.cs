using System.Collections;
using System.Collections.Immutable;

namespace PMW2RPArchipelagoClientMod.util
{
    public class FallbackSet<T> : IImmutableSet<T>
    {
        private Func<IImmutableSet<T>> _mainSetGetter;
        private Func<IImmutableSet<T>> _fallbackSetGetter;

        private IImmutableSet<T> _mainSet => _mainSetGetter();
        private IImmutableSet<T> _fallbackSet => _fallbackSetGetter();

        public int Count => throw new NotImplementedException();

        public FallbackSet(Func<IImmutableSet<T>> mainSetGetter,
            Func<IImmutableSet<T>> fallbackSetGetter)
        {
            _mainSetGetter = mainSetGetter;
            _fallbackSetGetter = fallbackSetGetter;
        }

        public IImmutableSet<T> Add(T value)
        {
            return _mainSet.Add(value);
        }

        public IImmutableSet<T> Clear()
        {
            return _mainSet.Clear();
        }

        public bool Contains(T value)
        {
            return _mainSet.Contains(value) || _fallbackSet.Contains(value);
        }

        public IImmutableSet<T> Except(IEnumerable<T> other)
        {
            return _mainSet.Except(other);
        }

        public IImmutableSet<T> Intersect(IEnumerable<T> other)
        {
            return _mainSet.Intersect(other);
        }

        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            return _mainSet.Union(_fallbackSet).IsProperSubsetOf(other);
        }

        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            return _mainSet.Union(_fallbackSet).IsProperSupersetOf(other);
        }

        public bool IsSubsetOf(IEnumerable<T> other)
        {
            return _mainSet.Union(_fallbackSet).IsSubsetOf(other);
        }

        public bool IsSupersetOf(IEnumerable<T> other)
        {
            return _mainSet.Union(_fallbackSet).IsSupersetOf(other);
        }

        public bool Overlaps(IEnumerable<T> other)
        {
            return _mainSet.Union(_fallbackSet).Overlaps(other);
        }

        public IImmutableSet<T> Remove(T value)
        {
            return _mainSet.Remove(value);
        }

        public bool SetEquals(IEnumerable<T> other)
        {
            return _mainSet.Union(_fallbackSet).SetEquals(other);
        }

        public IImmutableSet<T> SymmetricExcept(IEnumerable<T> other)
        {
            return _mainSet.SymmetricExcept(other);
        }

        public bool TryGetValue(T equalValue, out T actualValue)
        {
            return _mainSet.TryGetValue(equalValue, out actualValue) || _fallbackSet.TryGetValue(equalValue, out actualValue);
        }

        public IImmutableSet<T> Union(IEnumerable<T> other)
        {
            return _mainSet.Union(_fallbackSet).Union(other);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _mainSet.Union(_fallbackSet).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _mainSet.Union(_fallbackSet).GetEnumerator();
        }
    }
}
