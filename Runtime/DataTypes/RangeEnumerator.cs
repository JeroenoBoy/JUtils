using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JUtils
{
    /// <summary>
    /// Used to turn a range or int to a RangeEnumerator so it can be used in a Foreach loop
    /// </summary>
    /// <example><code lang="CSharp">
    /// namespace Example
    /// {
    ///     public class RangeExample : MonoBehaviour
    ///     {
    ///         private void Start()
    ///         {
    ///             foreach (int i in 10) {} // Loop from 0 > 9
    ///             foreach (int i in ..10) {} // Loop from 0 > 10
    ///             foreach (int i in 10..) {} // Loop from 10 > 0
    ///             foreach (int i in 20..^10) {} // Loop from 20 > 10
    ///             foreach (int i in ^0..10) {} // Loop from 0 > 9
    ///             foreach (int i in ^10..^0) {} // Loop from 9 > 0
    ///         }
    ///     }
    /// }
    /// </code></example>
    public static class RangeExtensions
    {
        public static RangeEnumerator GetEnumerator(this Range range)
        {
            return new RangeEnumerator(range);
        }


        public static RangeEnumerator GetEnumerator(this int end)
        {
            return new RangeEnumerator(0, end);
        }
    }


    /// <summary>
    /// Allows for complex enumerations for Range types
    /// </summary>
    public class RangeEnumerator : IEnumerator<int>
    {
        private readonly int _startIndex;
        private readonly int _direction = 1;
        private readonly int _end;

        private int _index;


        internal RangeEnumerator(Range range)
        {
            if (range.Start.IsFromEnd) {
                if (range.End.IsFromEnd) {
                    _direction = -1;
                    _startIndex = _index = range.Start.Value;
                    _end = range.End.Value;
                } else {
                    _startIndex = _index = range.Start.Value - 1;
                    _end = range.End.Value - 1;
                }
            } else if (range.End.IsFromEnd) {
                _direction = -1;
                _startIndex = _index = range.Start.Value + 1;
                _end = range.End.Value;
            } else {
                _startIndex = _index = range.Start.Value - 1;
                _end = range.End.Value;
            }

            if ((int)Mathf.Sign(_index - _end) == _direction) {
                throw new ArgumentException("Indexes direction did not match target direction");
            }
        }


        internal RangeEnumerator(int start, int end)
        {
            if (start > end) throw new ArgumentException("Start must not be greater than end");

            _startIndex = _index = start - 1;
            _end = end - 1;
        }


        public bool MoveNext()
        {
            return _direction == 1 ? _index++ < _end : _index-- > _end;
        }


        public void Reset()
        {
            _index = _startIndex;
        }


        public int Current => _index;
        object IEnumerator.Current => Current;


        public void Dispose() { }
    }
}