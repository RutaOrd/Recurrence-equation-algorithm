using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace lab2
{
    class CustomData
    {
        public int TNum;
        public int TResult;
    }
    class Program
    {
       
        static int n = 6000;
        static int[] v = new int[n];
        static int[] w = new int[n];
        static int[] F = new int[99];


        static void Main(string[] args)
        {

       
            for (int i = 0; i < n; i++)
            {
                w[i] = i + 9;
                v[i] = i + 9;
            }
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            int rResult = Fr(10, w, v);
            stopWatch.Stop();
           // Console.WriteLine("\nPanaudojant rekursiją: {0}", rResult);
            Console.WriteLine("Teisiogiai panaudojant rekursiją: Duomenų kiekis ={0}, Išnaudotas laikas={1}",
                w.Length, stopWatch.ElapsedTicks);
            //*************************
            stopWatch.Reset();
            stopWatch.Start();
            int pResult = ParallelFd(10);
            stopWatch.Stop();
          //  Console.WriteLine("\nLygiagretaus: {0}", pResult);
            Console.WriteLine("Lygiagretus: Duomenų kiekis ={0}, Išnaudotas laikas={1}",
                w.Length, stopWatch.ElapsedTicks);

        }

        //lygiagretus
        static int ParallelFd(int W, int max = 0)
        {
            int result = 0;
            if (W > 0)
            {
                int countCPU = 4;
                Task[] tasks = new Task[countCPU];
                for (int j = 0; j < countCPU; j++)
                {
                    tasks[j] = Task.Factory.StartNew(
                        (Object p) =>
                        {
                            var data = p as CustomData;
                            if (data == null)
                            {
                                return;
                            }
                            data.TResult = F1(W, data.TNum, countCPU);
                        }, new CustomData() { TNum = j });
                }
                Task.WaitAll(tasks);
                for (int i = 0; i < countCPU; i++)
                {
                    if ((tasks[i].AsyncState as CustomData).TResult > result)
                    {
                        result = (tasks[i].AsyncState as CustomData).TResult;
                    }
                }
                return result;
            }
            else
            {
                return 0;
            }

        }
        static int F1(int W, int pradzia, int countCPU, int max = 0) {
            if (W <= 0) {
                return 0;
            }
          //  int max = int.MinValue;
            for (int i = pradzia; i < n; i+=countCPU)
            {
                int part = F1(W - w[i], pradzia, countCPU);
                //  if (w[i] < W) max = Math.Max(max, Fr(W - w[i]) + v[i]);
                if (w[i] <= W && part + v[i] >= max) {
                    max = part + v[i];
                }

            }
            return max == int.MinValue ? 0 : max;

        }



        //panaudojant rekursija
        static int Fr(int W, int[] w, int[] v, int max = 0)
        {
            if (W <= 0)
            {
                return 0;
            }
            //int max = int.MinValue;
            //for (int i = 0; i < n; i++)
            //{
            //    if (w[i] < W) max = Math.Max(max, Fr(W - w[i]) + v[i]);

            ////}
            //return max == int.MinValue ? 0 : max;
            for (int i = 0; i < n; i++)
            {
                int part = Fr(W - w[i], w, v, max);
                if (w[i] <= W && part + v[i] > max)
                {
                    max = part + v[i];
                }
            }
            return max;

        }

    }
}
