using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace testconsole
{
    class Program
    {
        static bool busy = false;
        static bool IsBusy
        {
            get => busy;
            set
            {
                if (!value)
                    Console.WriteLine("busy = false");
                busy = value;
            }
        }
        static BigInteger bigint = 0;
        
        static void Main(string[] args)
        {
            Console.WriteLine("Start");
            Run();
            PollBusy();
            Console.WriteLine($"End, isbusy: {IsBusy}");
            Console.ReadLine();
        }

        static async void Run()
        {
            await RunAsync();
            //Console.WriteLine($"Run Done, IsBusy: {IsBusy}");
        }

        static async void PollBusy()
        {
            await Task.Run(() =>
            {
                while (IsBusy)
                {
                    Console.WriteLine($"Busy: {IsBusy}");
                    Thread.Sleep(1000);
                }
            });
        }

        static async Task RunAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                await Task.Run(() =>
                {
                    for (long i = 0; i < 100000000; i++)
                    {
                        bigint += (i % 250) ^ 50 % 100;
                    }                  
                });
            }
            finally
            {
                IsBusy = false;
            }
        }
    }

    class TestClass
    {
        public Task<bool> ReturnTrue() => Task.Run(() => true);
    }
}
