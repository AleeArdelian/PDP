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
            

            for (int i=0; i<10; i++)
            {
                Thread tid = new Thread(new ThreadStart(()=>_salesManager.BuyProducts(_mutex)));
                _threads.Add(tid);
            }

            for (int i = 0; i < 5; i++)//threadnumber/2
            {
                Thread _checkT = new Thread(new ThreadStart(() => _salesManager.Check(_mutex)));
                _checkThreads.Add(_checkT);
            }

            for (int i = 0; i < 10; i++) //threadnumber
            {
                if (i > _checkThreads.Count - 1)
                {
                    _threads[i].Start();
                }
                else
                {
                    _threads[i].Start();
                    _checkThreads[i].Start();
                    Thread.Sleep(300);
                }
            }


            //foreach (Thread t in _threads)
            //    t.Start();

            //foreach (Thread t in _threads)
            //    t.Join();

            //Thread _checkT = new Thread(new ThreadStart(() => _salesManager.Check(_mutex)));
            //_checkT.Start();

            foreach (Thread t in _threads)
                t.Join();
            foreach (Thread t in _checkThreads)
                t.Join();

            Console.ReadLine();
        }

    }

    
}
