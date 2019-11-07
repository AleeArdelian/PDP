using Non_cooperative_threads.Service;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Non_cooperative_threads
{
    class Program
    {
        static void Main(string[] args)
        {
            SalesManager _salesManager = new SalesManager();
            Mutex _mutex = new Mutex();
            int _threadNumber = 30;
            List<Thread> _threads = new List<Thread>();
            List<Thread> _checkThreads = new List<Thread>();

            _salesManager.PopulateRandom();

            Thread _checkT = new Thread(new ThreadStart(() =>
           {
               for (int i = 0; i <= 5; i++)
               {
                   _salesManager.Check(_mutex);
                   Thread.Sleep(2);
               }
           }));

            for (int i=0; i<10; i++)
            {
                Thread tid1 = new Thread(new ThreadStart(()=>_salesManager.BuyProducts(_mutex)));
                _threads.Add(tid1);
            }

            
            _checkT.Start();
            foreach (Thread t in _threads)
                t.Start();

            foreach (Thread t in _threads)
                t.Join();
            _checkT.Join();

            Console.ReadLine();
        }

    }

    
}

