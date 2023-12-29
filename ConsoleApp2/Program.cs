namespace ConsoleApp2;

internal class Philosopher(int id, Semaphore semaphore, Mutex leftFork, Mutex rightFork)
{
    public void Dine()
    {
        while (true)
        {
            Think();
            semaphore.WaitOne();
            Eat();
            semaphore.Release();
        }
    }

    private void Think()
    {
        Console.WriteLine($"Philosopher {id} is thinking.");
        Thread.Sleep(RandomTime());
    }

    private void Eat()
    {
        leftFork.WaitOne();
        rightFork.WaitOne();

        Console.WriteLine($"Philosopher {id} is eating.");
        Thread.Sleep(RandomTime());

        leftFork.ReleaseMutex();
        rightFork.ReleaseMutex();
    }

    private int RandomTime()
    {
        return new Random().Next(1000, 2000);
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Семафор: Ограничивает количество философов, которые могут одновременно пытаться есть. В этом примере он установлен на numPhilosophers - 1, чтобы всегда была хотя бы одна вилка свободна.
        // Мьютексы: Аналогично первому, должны захватить чтобы есть
        const int numPhilosophers = 5;
        var semaphore = new Semaphore(numPhilosophers - 1, numPhilosophers - 1);
        var forks = new Mutex[numPhilosophers];
        var philosophers = new Philosopher[numPhilosophers];
        var threads = new Thread[numPhilosophers];

        for (int i = 0; i < numPhilosophers; i++)
        {
            forks[i] = new Mutex();
        }

        for (int i = 0; i < numPhilosophers; i++)
        {
            var leftFork = forks[i];
            var rightFork = forks[(i + 1) % numPhilosophers];

            philosophers[i] = new Philosopher(i, semaphore, leftFork, rightFork);
            threads[i] = new Thread(philosophers[i].Dine);
            threads[i].Start();
        }
    }
}