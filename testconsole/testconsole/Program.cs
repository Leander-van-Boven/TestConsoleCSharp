using System;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace testconsole
{
    public enum Status
    {
        None,
        Busy,
        Done
    }

    class Program
    {
        static Status busy = Status.None;
        static Status IsBusy
        {
            get => busy;
            set
            {
                if (value == Status.Done)
                    Console.WriteLine("IsBusy.Set -> busy = Status.Done");
                busy = value;
            }
        }
        static BigInteger bigint = 0;
        
        static async Task Main(string[] args)
        {
            Console.WriteLine("Start");

            // Choose either -> Thread should definitely be async
            new Thread(PollBusy).Start();
            //PollBusyTask().Start();

            await RunAsync();
            Console.WriteLine($"End, IsBusy: {IsBusy}");
            Console.ReadLine();
        }

        static Task PollBusyTask()  => new Task(()
            => {
                    while (IsBusy != Status.Done)
                    {
                        Console.WriteLine($"Busy: {IsBusy}");
                        Thread.Sleep(1000);
                    }
                });

        static void PollBusy()
        {
            while (IsBusy != Status.Done)
            {
                Console.WriteLine($"[PollBusy()] Busy: {IsBusy}");
                Thread.Sleep(1000);
            }
        }

        static async Task RunAsync()
        {
            if (IsBusy == Status.Done)
                return;

            try
            {
                IsBusy = Status.Busy;
                await DoRandomHeavyCalc();
            }
            finally
            {
                IsBusy = Status.Done;
            }
        }

        static Task DoRandomHeavyCalc()
        {
            return Task.Run(() =>
            {
                for (long i = 0; i < 100000000; i++)
                {
                    bigint += (i % 250) ^ 50 % 100;
                }
            });
        }
    }
}
