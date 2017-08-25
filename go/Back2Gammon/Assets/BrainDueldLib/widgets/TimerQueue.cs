using System;
using System.Collections.Generic;
using System.Text;

namespace BrainDuelsLib.widgets
{
    public class TimerQueue<T>
    {
        public Queue<T> queue = new Queue<T>();

        public BrainDuelsLib.delegates.Action<T> callback;

        public void Push(T t)
        {
            queue.Enqueue(t);
        }

        public void OnTimer()
        {
            while(queue.Count > 0)
            {
                T t = queue.Dequeue();
                callback(t);
            }
        }
    }
}
