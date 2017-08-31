using System;
using System.Threading.Tasks;
using Microsoft.VisualBasic.Devices;

/// <summary>
/// allocates as much memory as possible and writes random bytes to it.
/// under development do not use at this time.  it may lockup windows
/// </summary>

namespace Gobble
{
    class Program
    {
        static Random r = new Random();
        static byte[,] bufptr; 
        static void Main(string[] args)
        {
            ComputerInfo CI = new ComputerInfo();
            ulong totalMem = CI.TotalPhysicalMemory;

            // TODO make some random increments to fool key scanners

            int MB = 1024 * 1024;
            int TotalMBs = (int) ((uint)totalMem / MB);

            int size = TotalMBs * 1000;
            int n = 256 ;

            bufptr = new byte[size, n];

            //for (int i = 0; i < size; i++)
 //           Parallel.For(0, size, i =>
 //            { bufptr[i] = new byte[n]; });

                

            Console.WriteLine("Mem committed");

            Parallel.For(0, size, i =>
             {
                 Randomfill(i,n);
             });


     //       long totalsize = size * n;
            Console.WriteLine("Filled array with random bytes");

   /*        for (int i = 0; i < size; i++)
            {
                int x = r.Next(0, size);
                int y = r.Next(0, n);
                byte bytexy = bufptr[y][x];
            }
   */



            // System.Threading.Thread.Sleep(10000);
        }

        private static void Randomfill(int j, int k)
        {
            for (int i = 0; i < j; i++)
                for (int n = 0; n < k; n++)
                    bufptr[i,n] = (byte) r.Next(0, 255);
        }
                
       
    }
}
