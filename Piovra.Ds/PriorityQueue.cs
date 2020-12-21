using System;

namespace Piovra.Ds {
    public class PriorityQueue<T> where T : IComparable<T> {
        readonly BinaryHeap<T> _heap;

        PriorityQueue(BinaryHeap<T> heap) => _heap = ARG.NotNull(heap, nameof(heap));

        public static PriorityQueue<T> Max(int capacity = INITIAL_CAPACITY) => new PriorityQueue<T>(new BinaryHeap<T>(capacity, true));
        public static PriorityQueue<T> Min(int capacity = INITIAL_CAPACITY) => new PriorityQueue<T>(new BinaryHeap<T>(capacity, false));        

        public const int INITIAL_CAPACITY = 1;

        public bool IsMax => _heap.NonIncreasing;

        public bool IsEmpty() => _heap.IsEmpty();        

        public void Enqueue(T item) {            
            _heap.Add(item);
        }

        public T Dequeue() {
            var result = _heap.Top();
            _heap.Pop();
            return result;
        }

        public T Peek() {
            return _heap.Top();
        }
    }
}