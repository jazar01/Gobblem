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
            string operation;
            int count = 1;

            string parm;
            if (args.Length == 0)
                operation = "c";
            else
            {
                parm = args[0].ToLower();
                if (parm.Length > 2)
                    if (int.TryParse(parm.Substring(2), out int reps))
                        if (reps < 1 || reps > 999)
                            Console.WriteLine("Count not valid, defaulting to 1");
                        else
                            count = reps;
                    else
                        Console.WriteLine("Count not valid, defaulting to 1");

                operation = parm.Substring(1, 1);

            }

            if (operation == "a")
            {
                for (int i = 0; i < count; i++)
                {
                    fillmem("c");
                    fillmem("f");
                    fillmem("r");
                }
            }
            else
                for (int i = 0; i < count; i++)
                {
                    fillmem(operation);
                }
        }




        static void fillmem(string operation)
        {
            int Unit;
            string filler;
            int maxthreads;

            switch (operation)
            {
                case "r":
                    filler = "random bytes";
                    maxthreads = 8; // if too high may cause out of memory, depending on machine
                    Unit = MB * 1024;
                    break;
                case "f":
                case "1":
                    filler = "1's";
                    maxthreads = 8;
                    Unit = MB * 1024;
                    break;
                case "c":
                case "0":
                default:
                    filler = "0's";
                    maxthreads = 8;
                    Unit = MB * 1024;
                    break;
            }


            System.Threading.Thread.MemoryBarrier();
            // get info about available memory
            ComputerInfo CI = new ComputerInfo();
            ulong availablePMem = CI.AvailablePhysicalMemory;
            int Units = (int)(availablePMem / (ulong)Unit);
            DateTime tstart;
            string totalbytes;
            DoFill(operation, Unit, filler, maxthreads, availablePMem, Units, out tstart, out totalbytes);

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.WaitForFullGCComplete();


            DateTime tend = DateTime.Now;
            TimeSpan t = tend.Subtract(tstart);
            Console.Write("\n" + totalbytes + " GB Filled with " + filler);
            Console.WriteLine(" in " + t.TotalSeconds.ToString("0.00") + " seconds (" + ((availablePMem / GB) / t.TotalSeconds).ToString("0.00") + " GB/s) \n");

            //System.Threading.Thread.Sleep(6000);
        }

        private static void DoFill(string operation, int Unit, string filler, int maxthreads, ulong availablePMem, int Units, out DateTime tstart, out string totalbytes)
        {
            bytes[] arrayofbytes;
            tstart = DateTime.Now;
            byte[] lastbytes;
            ulong remaining;

            // this causes the memory to become committed to the process
            arrayofbytes = new bytes[Units];
            for (int i = 0; i < Units; i++)
                arrayofbytes[i].sequence = new byte[Unit];

            //commit last bytes of memory here
            remaining = availablePMem - ((ulong)Units * (ulong)Unit);
            lastbytes = new byte[remaining];

            double factor = (double)Unit / (double)GB;
            totalbytes = ((double)Units * factor + (double)lastbytes.Length / (double)GB).ToString("0.00");
            Console.WriteLine(totalbytes + " GB committed to be filled with " + filler);

            System.Threading.Thread.MemoryBarrier();

            // RNGCryptoServiceProvider CSP = new RNGCryptoServiceProvider();
            // RNGCryptoServiceProvider CSP = new RNGCryptoServiceProvider();
            // RandomNumberGenerator rnd = RNGCryptoServiceProvider.Create();

            Random rnd = new Random();

            // runs fill operations on segments of memory in parallel to reduce elapsed time
            Parallel.For(0, Units, new ParallelOptions { MaxDegreeOfParallelism = maxthreads }, i =>
            {
            // Console.WriteLine("debug ->  i=" + i + " sequence length=" + arrayofbytes[i].sequence.Length);



            if (operation == "r")
            {
                // Console.Write("x");
                // CSP.GetBytes(arrayofbytes[i].sequence);
                // rnd.NextBytes(arrayofbytes[i].sequence);
                rnd.NextBytes(arrayofbytes[i].sequence);
                }
                else if (operation == "f")
                    for (int n = 0; n < Unit; n++)
                        arrayofbytes[i].sequence[n] = 255;
                else
                    Array.Clear(arrayofbytes[i].sequence, 0, Unit);

                if (i % (GB / Unit) == 0)
                    Console.Write("."); // progress indicator
            });


            //  fill the remaining bytes
            if (operation == "r")
                rnd.NextBytes(lastbytes);
            // CSP.GetBytes(lastbytes);
            else if (operation == "f")
                for (uint n = 0; n < remaining; n++)
                    lastbytes[n] = 255;
            else
                Array.Clear(lastbytes, 0, (int)remaining);

            //CSP.Dispose();
            //rnd.Dispose();
            arrayofbytes = null;
            lastbytes = null;
        }
    }
}


