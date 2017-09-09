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

    struct bytes
    {
        public byte[] sequence;
    }

    const int MB = 1024 * 1024;
    const int GB = MB * 1024;

        static void Main(string[] args)
        {
            string parm;
            if (args.Length == 0)
                parm = "-c";
            else
                parm = args[0];

            if (parm == "-a")
            {
                fillmem("-c");
                fillmem("-f");
                fillmem("-r");
            }
            else
                fillmem(parm);
        }


        static void fillmem (string parm)
            {
            int Unit;
            string option;
            string filler;
            int maxthreads;

            switch (parm)
                {
                case "-r":
                    option = "r";
                    filler = "random bytes";
                    maxthreads = 4; // if too high may cause out of memory, depending on machine
                    Unit = MB * 256;                 
                    break;
                case "-f":
                case "-1":
                    option = "f";
                    filler = "1's";
                    maxthreads = 12;
                    Unit = MB * 1024;
                    break;
                case "-c":
                case "-0":
                default:
                    option = "c";
                    filler = "0's";
                    maxthreads = 2;
                    Unit = MB * 1024;
                    break;
                }

            // get info about available memory
            ComputerInfo CI = new ComputerInfo();
            ulong availablePMem = CI.AvailablePhysicalMemory;
            int Units = (int) (availablePMem / (ulong) Unit);

            bytes[] arrayofbytes;
            DateTime tstart = DateTime.Now;

            // this causes the memory to become committed to the process
            arrayofbytes = new bytes[Units];
            for (int i = 0; i < Units; i++)
                arrayofbytes[i].sequence = new byte[Unit];

            //commit last bytes of memory here
            ulong remaining = availablePMem - ((ulong) Units * (ulong) Unit );
            byte[] lastbytes = new byte[remaining] ;

            double factor = (double) Unit / (double) GB;
            string totalbytes = ((double)Units * factor + (double)lastbytes.Length / (double)GB).ToString("0.00");
            Console.WriteLine(totalbytes + " committed\n");


            RNGCryptoServiceProvider CSP = new RNGCryptoServiceProvider();
            /*
            if (option == "r")
                for (int i = 0; i < Units; i++)
                {
                    CSP.GetBytes(arrayofbytes[i].sequence);
                    if ((i * Unit) % GB == 0)
                            Console.Write(".");
                }
            else
            */
            Parallel.For(0, Units, new ParallelOptions { MaxDegreeOfParallelism = maxthreads }, i =>
             {
                 if (option == "r")
                     CSP.GetBytes(arrayofbytes[i].sequence);
                 else if (option == "f")
                     for (int n = 0; n < Unit; n++)
                         arrayofbytes[i].sequence[n] = 255;
                 else
                     Array.Clear(arrayofbytes[i].sequence, 0, Unit);

                 if ( i % (GB/Unit) == 0)
                         Console.Write("."); // progress indicator
    
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
            arrayofbytes = null;
            lastbytes = null;

            DateTime tend = DateTime.Now;
            TimeSpan t = tend.Subtract(tstart);
            Console.Write("\n" + totalbytes + " GB Filled with " + filler);
            Console.WriteLine("  in " + t.TotalSeconds.ToString("0.00") + " seconds  (" + (  Units/t.TotalSeconds).ToString("0.00") + " GB/s)");

            GC.Collect();
            GC.WaitForPendingFinalizers();

        }
    }
}

