using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.util
{
    public class FallbackDictionary<T> : IImmutableDictionary<T, bool>
    {
        private Func<IImmutableDictionary<T, bool>> _mainDictionaryGetter;
        private Func<IImmutableDictionary<T, bool>> _fallbackDictionaryGetter;

        private IImmutableDictionary<T, bool> _mainDictionary => _mainDictionaryGetter();
        private IImmutableDictionary<T, bool> _fallbackDictionary => _fallbackDictionaryGetter();

        public FallbackDictionary(Func<IImmutableDictionary<T, bool>> mainDictionaryGetter,
            Func<IImmutableDictionary<T, bool>> fallbackDictionaryGetter)
        {
            _mainDictionaryGetter = mainDictionaryGetter;
            _fallbackDictionaryGetter = fallbackDictionaryGetter;
        } 

        public bool this[T key] => _mainDictionary[key] || _fallbackDictionary[key];

        public IEnumerable<T> Keys => _mainDictionary.Keys;

        public IEnumerable<bool> Values => _mainDictionary.Values;

        public int Count => _mainDictionary.Count;

        public IImmutableDictionary<T, bool> Add(T key, bool value)
        {
            return _mainDictionary.Add(key, value);
        }

        public IImmutableDictionary<T, bool> AddRange(IEnumerable<KeyValuePair<T, bool>> pairs)
        {
            return _mainDictionary.AddRange(pairs);
        }

        public IImmutableDictionary<T, bool> Clear()
        {
            return _mainDictionary.Clear();
        }

        public bool Contains(KeyValuePair<T, bool> pair)
        {
            return _mainDictionary.Contains(pair) || _fallbackDictionary.Contains(pair);
        }

        public bool ContainsKey(T key)
        {
            return _mainDictionary.ContainsKey(key) || _fallbackDictionary.ContainsKey(key);
        }

        public IEnumerator<KeyValuePair<T, bool>> GetEnumerator()
        {
            return _mainDictionary.GetEnumerator();
        }

        public IImmutableDictionary<T, bool> Remove(T key)
        {
            return _mainDictionary.Remove(key);
        }

        public IImmutableDictionary<T, bool> RemoveRange(IEnumerable<T> keys)
        {
            return _mainDictionary.RemoveRange(keys);
        }

        public IImmutableDictionary<T, bool> SetItem(T key, bool value)
        {
            return _mainDictionary.SetItem(key, value);
        }

        public IImmutableDictionary<T, bool> SetItems(IEnumerable<KeyValuePair<T, bool>> items)
        {
            return _mainDictionary.SetItems(items);
        }

        public bool TryGetKey(T equalKey, out T actualKey)
        {
            return _mainDictionary.TryGetKey(equalKey, out actualKey) || _fallbackDictionary.TryGetKey(equalKey, out actualKey);
        }

        public bool TryGetValue(T key, [MaybeNullWhen(false)] out bool value)
        {
            bool success = _mainDictionary.TryGetValue(key, out value);
            if (success || value)
            {
                return true;
            }
            return _fallbackDictionary.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _mainDictionary.GetEnumerator();
        }
    }
}
