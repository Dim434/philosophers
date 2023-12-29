namespace ConsoleApp1;

class Philosopher(int id, Mutex leftFork, Mutex rightFork, bool isLast)
{
    public void Dine()
    {
        while (true)
        {
            Think();

            // Последний философ берет сначала левую вилку, затем правую
            if (isLast)
            {
                leftFork.WaitOne();
                rightFork.WaitOne();
            }
            else
            {
                rightFork.WaitOne();
                leftFork.WaitOne();
            }

            Eat();

            leftFork.ReleaseMutex();
            rightFork.ReleaseMutex();
        }
    }

    private void Think()
    {
        Console.WriteLine($"Philosopher {id} is thinking.");
        Thread.Sleep(new Random().Next(1000, 2000));
    }

    private void Eat()
    {
        Console.WriteLine($"Philosopher {id} is eating.");
        Thread.Sleep(new Random().Next(1000, 2000));
    }
}


class Program
{
    static void Main(string[] args)
    {
        //Мьютексы: Каждая вилка представлена мьютексом. Философы должны захватить две вилки, чтобы начать есть.
        const int numPhilosophers = 100;
        var forks = new Mutex[numPhilosophers];
        var philosophers = new Philosopher[numPhilosophers];
        var threads = new Thread[numPhilosophers];

        for (var i = 0; i < numPhilosophers; i++)
        {
            forks[i] = new Mutex();
        }

        for (var i = 0; i < numPhilosophers; i++)
        {
            var leftFork = forks[i];
            var rightFork = forks[(i + 1) % numPhilosophers];
            philosophers[i] = new Philosopher(i, leftFork, rightFork, i == numPhilosophers - 1);
            threads[i] = new Thread(philosophers[i].Dine);
            threads[i].Start();
        }
    }
}