# Gobble 

Gobbles up memory and writes zeros, ones, or random bytes

This program was written mostly for research purposes.  It may be useful for security reasons as overwriting memory may reduce the risk of sensitive information being left in memory and subjected to malware or cold-boot attacks. 

Usage:

Gobble <option>

Options 	-c		fills the memory with binary zeros
		-f		fills the memory with binary ones
		-r		fills the memory with random bytes

Notes: 

-c is the fastest option.  If no valid option is specified then -c is used. 

-r writes random bytes which can take quite some time.  It may take 3 minutes or longer to write generaate and write random bytes to 8GB of memory on a typical machine.

All memory occupied by this program is freed when the process is complete.  Usually a few seconds for options -c and -f.  

This program will only clear memory that is not in use by the OS or other programs.


Caution:

It is inherently risky to the stability of the OS to attempt to allocate 100% of the available RAM.  You may experience out of memory conditions, slow performance, or hangs while this program is running.  Running with -c or no option is the least risky as this usually only runs for few seconds. 

There is no guarantee that all sensitive data will be removed from memory by this program.   



		