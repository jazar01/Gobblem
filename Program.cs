using System;
using Microsoft.VisualBasic.Devices;

/// <summary>
/// allocates as much memory as possible and writes random bytes to it.
/// </summary>

namespace Gobble
{
    class Program
    {
        static Random r = new Random();
        static void Main(string[] args)
        {
            ComputerInfo CI = new ComputerInfo();
            ulong totalMem = CI.TotalPhysicalMemory;

            // TODO change allocations to totalmem

            // TODO make some random increments to fool key scanners

            int MB = 1024 * 1024;
            int TotalMBs = (int) (totalMem / (ulong) MB);

            int size = TotalMBs * 4096;
            int n = 256 ;

            byte[][] bufptr = new byte[size][];

            for (int i = 0; i < size - 1; i++)
                bufptr[i] = new byte[n];

            // todo change to parallel loop
            for (int i = 0; i < size; i++)
                Randomfill(ref bufptr[i], n);


            long totalsize = size * n;
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

        private static void Randomfill(ref byte[] p, int length)
        {
            for (int i = 0; i < length; i++)
                p[i] = (byte) r.Next(0, 255);
        }
                
       
    }
}
