using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoringEngineTest.UnitTestUtils
{
    public class Results<T>
    {
        private readonly Queue<Func<T>> values = new Queue<Func<T>>();
        public Results(T result) { values.Enqueue(() => result); }
        public Results<T> Then(T value) { return Then(() => value); }
        public Results<T> Then(Func<T> value)
        {
            values.Enqueue(value);
            return this;
        }
        public T Next() { return values.Dequeue()(); }
    }
}
