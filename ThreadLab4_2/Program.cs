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
        private bool[] forks = new bool[5];
        private object locker = new object();
        public void RequestToEat(int id)
        {
            int right = id;
            int left = (id + 1) % 5;

            lock (locker) {
                while (forks[right] || forks[left])
                {
                    Monitor.Wait(locker);
                }
                forks[right] = true;
                forks[left] = true;
            }
        }
        public void DoneEating(int id)
        {
            int right = id;
            int left = (id + 1) % 5;
            lock (locker)
            {
                forks[right] = false;
                forks[left] = false;
                Monitor.PulseAll(locker);
            }

        }

    }
}
