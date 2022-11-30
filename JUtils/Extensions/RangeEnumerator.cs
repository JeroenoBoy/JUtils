using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace JUtils.Extensions
{
    public static class RangeExtensions
    {
        public static RangeEnumerator GetEnumerator(this Range range) => new (range);
        public static RangeEnumerator GetEnumerator(this int end) => new (0, end);
    }
    
    
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
                }
                else {
                    _startIndex = _index = range.Start.Value-1;
                    _end = range.End.Value-1;
                }
                
            }

            else if (range.End.IsFromEnd) {
                _direction = -1;
                _startIndex = _index = range.Start.Value;
                _end = range.End.Value;
            }

            else {
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

            _startIndex = _index = start-1;
            _end = end;
        }


        public bool MoveNext() => _direction == 1 ? _index++ < _end : _index --> _end;
        public void Reset() => _index = _startIndex;


        public int Current => _index;
        object IEnumerator.Current => Current;


        public void Dispose() { }
    }
}
