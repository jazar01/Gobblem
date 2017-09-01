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
        struct bytes
        {
            public byte[] sequence;
        }

        static bytes[] arrayofbytes;

        static void Main(string[] args)
        {
            ComputerInfo CI = new ComputerInfo();
            ulong totalMem = CI.TotalPhysicalMemory;

            int MB = 1024 * 1024;
            int GB = MB * 1024;
            int TotalGBs = (int)((uint)totalMem / GB);

            int GBs = 29;
            int size = TotalGBs;
            int n = MB;
            DateTime tstart = DateTime.Now;
            arrayofbytes = new bytes[GBs];
            for (int i = 0; i < GBs; i++)
                arrayofbytes[i].sequence = new byte[GB];
     
            /*
            byte[] barray1 = new byte[GB];
            byte[] barray2 = new byte[GB];
            byte[] barray3 = new byte[GB];
            byte[] barray4 = new byte[GB];
            byte[] barray5 = new byte[GB];
            byte[] barray6 = new byte[GB];
            byte[] barray7 = new byte[GB];
            byte[] barray8 = new byte[GB];
            byte[] barray9 = new byte[GB];
            byte[] barray10 = new byte[GB];
            byte[] barray11 = new byte[GB];
            byte[] barray12 = new byte[GB];
            byte[] barray13 = new byte[GB];
            byte[] barray14 = new byte[GB];
            byte[] barray15 = new byte[GB];
            byte[] barray16 = new byte[GB];
            */



            //for (int i = 0; i < size; i++)
            //           Parallel.For(0, size, i =>
            //            { bufptr[i] = new byte[n]; });



            Console.WriteLine("Mem committed");
            /*
            r.NextBytes(barray1);
            r.NextBytes(barray2);
            r.NextBytes(barray3);
            r.NextBytes(barray4);
            r.NextBytes(barray5);
            r.NextBytes(barray6);
            r.NextBytes(barray7);
            r.NextBytes(barray8);
            r.NextBytes(barray9);
            r.NextBytes(barray10);
            r.NextBytes(barray11);
            r.NextBytes(barray12);
            r.NextBytes(barray13);
            r.NextBytes(barray14);
            r.NextBytes(barray15);
            r.NextBytes(barray16);

            */

            
            Parallel.For(0, GBs, new ParallelOptions { MaxDegreeOfParallelism = 4 }, i =>
             {
                 //arrayofbytes[i].sequence = new byte[GB];
                 //r.NextBytes(arrayofbytes[i].sequence);
                 for (int r = 0; r < GB; r++)
                     arrayofbytes[i].sequence[r] = 255;
                 // Array.Clear(arrayofbytes[i].sequence, 0, GB);
             });

            DateTime tend = DateTime.Now;
            TimeSpan t = tend.Subtract(tstart);
            Console.Write("     time: " + t.TotalSeconds.ToString("0.000") + " seconds  (" + (  GBs/t.TotalSeconds).ToString("0.0") + " GB/s)\n");


            //       long totalsize = size * n;
            Console.WriteLine(GBs + " GB Filled");

            //System.Threading.Thread.Sleep(30000);

            /*        for (int i = 0; i < size; i++)
                     {
                         int x = r.Next(0, size);
                         int y = r.Next(0, n);
                         byte bytexy = bufptr[y][x];
                     }




                     // System.Threading.Thread.Sleep(10000);
                 }

                 private static void Randomfill(int j, int k)
                 {
                     // need to make this faster
                     r.NextBytes(bufptr[j,k]);
                     for (int i = 0; i < j; i++)
                         for (int n = 0; n < k; n++)
                             bufptr[i,n] = (byte) r.Next(0, 255);
                 }
             */

        }
    }
}

