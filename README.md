# Gobble 

Gobbles up memory and writes zeros, ones, or random bytes

This program was written mostly for research purposes.  It may be useful for security reasons as overwriting memory may reduce the risk of sensitive information being left in memory and subjected to malware or cold-boot attacks. 

Usage:

Gobble [option]

Options    
- -c fills the memory with binary zeros
- -f fills the memory with binary ones
- -r fills the memory with random bytes
- -a fills the memory with zeros, then ones, then random bytes

Notes: 

-c is the fastest option.  If no valid option is specified then -c is used as the default.  

Multiple rounds can be performed by adding a number between 1 and 999 after the operation.  For example: -r3 fills the memory with random bytes three times.  

All memory occupied by this program is freed when the process is complete.  Usually completed in a few seconds. 

This program will only clear memory that is not in use by the OS or other programs.  To ensure that sensitive data or encryption keys are overwritten, close all programs and services that are using those keys before running gobble.  Again, gobble only clears memory that is not currently allocated to a program or OS.


Caution:

It is inherently risky to the stability of the OS to attempt to allocate 100% of the available RAM.  You may experience out of memory conditions, slow performance, or hangs while this program is running.  Running with -c or no option is the least risky as this usually only runs for few seconds. 

There is no guarantee that all sensitive data will be removed from memory by this program.   

Comments on use of this program for security purposes:

If you are using this program to reduce the risk of attacks that involve accessing RAM to find senstive information, you may find these comments of interest. 

Exploitation of keys stored in RAM are the primary methods used to access the contents of encrypted drives and partitions without having the encryption keys or passwords.  This applies to Truecrypt, Veracrypt, bitlocker, ...

Types of attacks on RAM.

- Active malware - it is possible, malware running at elevated priviledges can access and record the contents of RAM. 

- Cold-boot - there are several known attacks involving physically accessing the content of RAM after the computer has been powered off for a short time.  See https://en.wikipedia.org/wiki/Cold_boot_attack.

- DMA attack - this involves attaching a physical device to a computer, either internally, or through certain types of ports, such as the IEEE 1394 firewire ports.  see https://en.wikipedia.org/wiki/DMA_attack 
 
- Memory images on disk - There are several tools used by attackers to search files on disk that contain contents of RAM.  Pagefiles, dump files, swap files, and hibernation files all contain images of the content of portions of RAM.  

All of the above, except possibly the malware attack, require physical access to the equipment.  Exploiting memory contents from disk files such as paging and dump files are probably the most likely exposures. There are methods to protect against these exploitations that involve encrypting the files or not using them.  

The DMA and cold-boot attacks would almost certainly require the attacker to access your computer while it is running with sensitive data in memory.  

Running gobble with any options or defaults to overwrite memory would likely suppress cold-boot and DMA attacks as long as the memory was overwritten before the contents were exploited. There is also a probability that running gobble would cause the page files to expunge sensitive data, but there is no guarantee.  

The gobble -a option overwrites the free memory with zeros, then ones, then random bytes.  This is most likely overkill, but there are labratory methods of recovering data from hard disks that have been overwritten by statistically analyzing bit by bit for residual charges on the surface of the media. It is theoretically possible, that such an analysis of memory could be performed to reveal the contents of overwritten memory.  However, the charges would likely degrade quickly in ram making it extremely difficult to recover overwritten data.  gobble -a should thwart such an attempt.  

Exploitations that involve searching RAM, either manually or with specialized software, involve looking for sequences of bytes with high entropy.  Gobble -a uses a cyrptographically secure random number generator, which will cause the memory to filled with high entropy data.  Large amounts of high entropy data in memory, that is of no value, will likely confuse any attempts to locate keys.

It is my opinion that a simple reboot of a computer will not sufficiently clear sensitive data and keys that were stored in memory since the power to the memory is not actually removed long enough for the memory's contents to dissipate.  The OS will restart and allocate memory as needed, but until the older memory is actually reused, its contents may still be recoverable.  Most articles on cold-boot attacks agree that shutting down and removing power will clear the memory given enough time, which may be seconds, to hours.

Use this code at your own risk.  There are no guarantees and it has not been proven to be 100% effective against all attacks.




		