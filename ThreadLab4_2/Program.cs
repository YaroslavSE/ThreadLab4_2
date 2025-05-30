namespace ThreadLab4_2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Waiter waiter = new Waiter();
            for (int i = 0; i < 5; i++)
            {
                int philosopherId = i;
                Thread philosopherThread = new Thread(() => Philosopher(philosopherId, waiter));
                philosopherThread.Start();
            }
        }
        static void Philosopher(int id, Waiter waiter)
        {
            int right, left;
            if (id == 4)
            {
                right = (id + 1) % 5;
                left = id;
            }
            else
            {
                right = id;
                left = (id + 1) % 5;
            }

            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("Philosopher" + id + " is thinking " + (i + 1) + " times");
                waiter.RequestToEat(id);

                Console.WriteLine("Philosopher" + id + " is eating" + (i + 1) + " times");
                waiter.DoneEating(id);

            }
        }
    }
    class Waiter
    {
        private SemaphoreSlim[] forks = new SemaphoreSlim[5];
        private object locker = new object();
        public Waiter()
        {
            for (int i = 0; i < forks.Length; i++)
            {
                forks[i] = new SemaphoreSlim(1, 1);
            }
        }
        public void RequestToEat(int id)
        {
            int right = id;
            int left = (id + 1) % 5;

            lock (locker)
            {
                while (forks[right].CurrentCount == 0 || forks[left].CurrentCount == 0)
                {
                    Monitor.Wait(locker);
                }
                forks[right].Wait();
                forks[left].Wait();
            }
        }
        public void DoneEating(int id)
        {
            int right = id;
            int left = (id + 1) % 5;
            lock (locker)
            {
                forks[right].Release();
                forks[left].Release();
                Monitor.PulseAll(locker);
            }

        }

    }
}