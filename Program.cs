using System;
using System.Threading.Tasks;
using Microsoft.VisualBasic.Devices;
using System.Security.Cryptography;


/// <summary>
/// allocates as much memory as possible and writes either zeros, ones, or random bytes to it
/// </summary>

namespace Gobble
{
    class Program
    {
    static string option;
     struct bytes
        {
            public byte[] sequence;
        }


        static void Main(string[] args)
        {
            int MB = 1024 * 1024;
            int Unit = MB * 1024;
            int GB = MB * 1024;

            RNGCryptoServiceProvider CSP = new RNGCryptoServiceProvider();
            ComputerInfo CI = new ComputerInfo();

            string filler;
            int maxthreads;

            
            filler = "0's";
            maxthreads = 2;
            if (args.Length == 0)
                option = "c";
            else
                switch (args[0])
                {
                    case "-r":
                        option = "r";
                        filler = "random bytes";
                        maxthreads = 1;   // CSP chokes on multiple threads
                        Unit = MB * 256;                 
                        break;
                    case "-f":
                    case "-1":
                        option = "f";
                        filler = "1's";
                        maxthreads = 12;
                        Unit = MB * 1024;
                        break;
                    default:
                        option = "c";
                        maxthreads = 2;
                        Unit = MB * 1024;
                        break;
                }


            
            ulong totalMem = CI.TotalPhysicalMemory;
            ulong availablePMem = CI.AvailablePhysicalMemory;

            int TotalGBs = (int)((uint)totalMem / Unit);

            int Units = (int) (availablePMem / (ulong) Unit);
            int size = TotalGBs;
            bytes[] arrayofbytes;
            DateTime tstart = DateTime.Now;
            arrayofbytes = new bytes[Units];
            for (int i = 0; i < Units; i++)
                arrayofbytes[i].sequence = new byte[Unit];

            //todo allocate last bytes of memory here
            ulong remaining = availablePMem - ((ulong) Units * (ulong) Unit );
            byte[] lastbytes = new byte[remaining] ;

            double factor = (double) Unit / (double) GB;
            string totalbytes = ((double)Units * factor + (double)lastbytes.Length / (double)GB).ToString("0.00");
            Console.WriteLine(totalbytes + " committed\n");
            if (option == "r")
                for (int i = 0; i < Units; i++)
                {
                    CSP.GetBytes(arrayofbytes[i].sequence);
                    if ((i * Unit) % GB == 0)
                            Console.Write(".");
                }
            else
                Parallel.For(0, Units, new ParallelOptions { MaxDegreeOfParallelism = maxthreads }, i =>
             {

                 if (option == "f")
                     for (int n = 0; n < Unit; n++)
                         arrayofbytes[i].sequence[n] = 255;
                 else
                     Array.Clear(arrayofbytes[i].sequence, 0, Unit);

                 Console.Write(".");
             });

            //  fill the remaining bytes
            if (option == "r")
                CSP.GetBytes(lastbytes);
            else if (option == "f")
                for (uint n = 0; n < remaining; n++)
                    lastbytes[n] = 255;
            else
                Array.Clear(lastbytes, 0, (int) remaining);

            CSP.Dispose();

            DateTime tend = DateTime.Now;
            TimeSpan t = tend.Subtract(tstart);
            Console.Write("\n" + totalbytes + " GB Filled with " + filler);
            Console.Write("  in " + t.TotalSeconds.ToString("0.00") + " seconds  (" + (  Units/t.TotalSeconds).ToString("0.00") + " GB/s)");
      

        }
    }
}

