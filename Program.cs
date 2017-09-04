using System;
using System.Threading.Tasks;
using Microsoft.VisualBasic.Devices;

/// <summary>
/// allocates as much memory as possible and writes either zeros, ones, or random bytes to it
/// </summary>

namespace Gobble
{
    class Program
    {
        static Random r = new Random();
        static string option;
        static string filler;
        struct bytes
        {
            public byte[] sequence;
        }

        static bytes[] arrayofbytes;

        static void Main(string[] args)
        {
            filler = "0's";
            if (args.Length == 0)
                option = "c";
            else
                switch (args[0])
                {
                    case "-r":
                        option = "r";
                        filler = "random bytes";
                        break;
                    case "-f":
                    case "-1":
                        option = "f";
                        filler = "1's";
                        break;
                    default:
                        option = "c";
                        break;
                }


            ComputerInfo CI = new ComputerInfo();
            ulong totalMem = CI.TotalPhysicalMemory;
            ulong availablePMem = CI.AvailablePhysicalMemory;


            int MB = 1024 * 1024;
            int GB = MB * 1024;
            int TotalGBs = (int)((uint)totalMem / GB);

            int GBs = (int) (availablePMem / (ulong) GB);
            int size = TotalGBs;

            DateTime tstart = DateTime.Now;
            arrayofbytes = new bytes[GBs];
            for (int i = 0; i < GBs; i++)
                arrayofbytes[i].sequence = new byte[GB];

            //todo allocate last bytes of memory here
            ulong remaining = availablePMem - ((ulong) GBs * (ulong) GB );
            byte[] lastbytes = new byte[remaining] ;

            string totalbytes = ((double)GBs + (double)lastbytes.Length / (double)GB).ToString("0.000");
            Console.WriteLine( totalbytes + " GB committed");
    
            Parallel.For(0, GBs, new ParallelOptions { MaxDegreeOfParallelism = 16 }, i =>
             {
                 if (option == "r")
                       r.NextBytes(arrayofbytes[i].sequence);
                 else if (option =="f")
                    for (int n = 0; n < GB; n++)
                        arrayofbytes[i].sequence[n] = 255;
                 else
                    Array.Clear(arrayofbytes[i].sequence, 0, GB);
             });

            //  fill the remaining bytes
            if (option == "r")
                r.NextBytes(lastbytes);
            else if (option == "f")
                for (uint n = 0; n < remaining; n++)
                    lastbytes[n] = 255;
            else
                Array.Clear(lastbytes, 0, (int) remaining);

            DateTime tend = DateTime.Now;
            TimeSpan t = tend.Subtract(tstart);
            Console.Write("     time: " + t.TotalSeconds.ToString("0.000") + " seconds  (" + (  GBs/t.TotalSeconds).ToString("0.0") + " GB/s)\n");


            Console.WriteLine(totalbytes + " GB Filled with " + filler);


        }
    }
}

