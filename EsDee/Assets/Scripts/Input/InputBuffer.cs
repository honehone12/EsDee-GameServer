using Unity.Collections;

namespace EsDee
{
    public struct InputBuffer : System.IDisposable
    {
        NativeQueue<byte> queue;
        int maxQueue;

        public InputBuffer(int maxQueueCount)
        {
            queue = new(Allocator.Persistent);
            maxQueue = maxQueueCount;
        }

        public void Enqueue(byte bits)
        {
            if (queue.Count > maxQueue)
            {
                _ = queue.Dequeue();
            }

            queue.Enqueue(bits);
        }

        public byte DequeueOrDefault()
        {
            if (queue.Count == 0)
            {
                return default;
            }

            return queue.Dequeue();
        }

        public void Dispose()
        {
            if (queue.IsCreated)
            {
                queue.Dispose();
            }
        }
    }
}
