using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Test2.Models
{
    public class ChartQueue<T> : IEnumerable<T>
    {
        private QueueNode<T> Head;
        private QueueNode<T> Tail;

        private uint? buffer;
        public uint? Buffer 
        { 
            get => buffer; 
            set
            {
                buffer = value;
                while(buffer < Count)
                    Dequeue();
            }
        }
        public ChartQueue()
        {
            Head = Tail = new NullNode<T>();
            Count = 0;
            Buffer = null;
        }
        public ChartQueue(uint buffer)
        {
            Head = Tail = new NullNode<T>();
            Count = 0;
            Buffer = buffer;
        }
        public void Enqueue(T value)
        {
            if (Head is NullNode<T>)
                Head = Tail = new ValueNode<T>(value);
            else
            {
                Tail.Next = new ValueNode<T>(value);
                Tail = Tail.Next;
            }
            if(Buffer != null && Count == Buffer)
            {
                Dequeue();
            }
            Count++;
        }

        public T Dequeue()
        {
            var answ = Peek();
            Head = Head.Next;
            Count--;
            return answ;
        }

        public T Peek()
        {
            if (Head is NullNode<T>)
                return default(T);
            else return ((ValueNode<T>)Head).Value;
        }

        public T PeekEnd()
        {
            if (Tail is NullNode<T>)
                return default(T);
            else return ((ValueNode<T>)Tail).Value;
        }

        public IEnumerator<T> GetEnumerator()
        {
            var pointer = Head;
            while (pointer.Next != null)
            {
                if(pointer is ValueNode<T>)
                    yield return (pointer as ValueNode<T>).Value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count { get; private set; }

        private abstract class QueueNode<T>
        {
            public QueueNode<T> Next { get; set; }
        }

        private class ValueNode<T> : QueueNode<T>
        {
            public T Value;
            public new ValueNode<T> Next { get; set; }
            public ValueNode(T value)
            {
                Value = value;
            }
            public ValueNode() { }
        }

        private class NullNode<T> : QueueNode<T>
        {
            public NullNode() : base() { }

            public new NullNode<T> Next
            {
                get => throw new NullReferenceException();
                set => throw new NullReferenceException();
            }
        }

    }
}
