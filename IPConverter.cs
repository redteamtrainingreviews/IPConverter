using System;
using System.Net;

// Compile:  csc IPConverter.cs

namespace IPConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: IPConverter <IP_in_any_format>");
                return;
            }
            
            string input = args[0];
            try
            {
                uint ipVal = ParseToUInt32(input);
                PrintAllFormats(ipVal, input);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to parse IP '{input}': {ex.Message}");
            }
        }

        // Attempt to parse the IP in various ways (hex, decimal, dotted, partial, etc.)
        public static uint ParseToUInt32(string ipString)
        {
            // 1) Try standard dotted parse with .NET (handles IPv4, but not partial forms)
            //    If that fails, we keep going.
            if (IPAddress.TryParse(ipString, out IPAddress? ipAddr))
            {
                byte[] bytes = ipAddr.GetAddressBytes();
                if (bytes.Length == 4)
                {
                    return (uint)(bytes[0] << 24 | bytes[1] << 16 | bytes[2] << 8 | bytes[3]);
                }
                throw new FormatException("Only IPv4 addresses are supported.");
            }

            // 2) Try manual parse of hex or decimal or partial

            // If it starts with "0x", interpret as hex
            if (ipString.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                return Convert.ToUInt32(ipString.Substring(2), 16);
            }
            
            // If it starts with "0" or "0o" you might interpret as octal, but let's skip or do:
            if (ipString.StartsWith("0o", StringComparison.OrdinalIgnoreCase))
            {
                return Convert.ToUInt32(ipString.Substring(2), 8);
            }
            else if (ipString.StartsWith("0") && ipString.Length > 1)
            {
                // Potentially interpret as octal
                // But careful with "0" alone. Let's do it:
                return Convert.ToUInt32(ipString, 8);
            }

            // 3) Maybe it has dot(s). Let's parse partial forms: A, A.B, A.B.C, A.B.C.D
            // We'll do a quick manual approach:
            string[] parts = ipString.Split('.');
            if (parts.Length == 1)
            {
                // Single decimal? e.g. 3232235818
                return Convert.ToUInt32(parts[0]);
            }
            else if (parts.Length == 2)
            {
                // A.B => A is the high octet, B is the remaining 24 bits
                uint a = Convert.ToUInt32(parts[0]);
                uint b = Convert.ToUInt32(parts[1]);
                return (a << 24) | (b & 0xFFFFFF);
            }
            else if (parts.Length == 3)
            {
                // A.B.C => A is first octet, B is second, C is last 16 bits
                uint a = Convert.ToUInt32(parts[0]) & 0xFF;
                uint b = Convert.ToUInt32(parts[1]) & 0xFF;
                uint c = Convert.ToUInt32(parts[2]) & 0xFFFF;
                return (a << 24) | (b << 16) | (c & 0xFFFF);
            }
            else if (parts.Length == 4)
            {
                // A.B.C.D => normal dotted
                uint a = Convert.ToUInt32(parts[0]) & 0xFF;
                uint b = Convert.ToUInt32(parts[1]) & 0xFF;
                uint c = Convert.ToUInt32(parts[2]) & 0xFF;
                uint d = Convert.ToUInt32(parts[3]) & 0xFF;
                return (a << 24) | (b << 16) | (c << 8) | d;
            }
            else
            {
                throw new FormatException("Too many dots in IP address string.");
            }
        }

        public static void PrintAllFormats(uint ipVal, string originalInput)
        {
            byte a = (byte)((ipVal >> 24) & 0xFF);
            byte b = (byte)((ipVal >> 16) & 0xFF);
            byte c = (byte)((ipVal >> 8)  & 0xFF);
            byte d = (byte)( ipVal        & 0xFF);

            // Standard dotted
            string dotted = $"{a}.{b}.{c}.{d}";

            // Single decimal
            string decimalForm = ipVal.ToString();

            // Hex with 0x
            string hexForm = $"0x{ipVal:X}";

            // Octal with 0o (C# doesn't do the prefix by default, but we'll just show a string)
            string octalForm = "0o" + Convert.ToString(ipVal, 8);

            // Partial forms
            // partial_1 => entire number as decimal
            string partial1 = ipVal.ToString();
            
            // partial_2 => A.(rest)
            uint rest2 = (uint)((b << 16) | (c << 8) | d);
            string partial2 = $"{a}.{rest2}";

            // partial_3 => A.B.(rest)
            uint rest3 = (uint)((c << 8) | d);
            string partial3 = $"{a}.{b}.{rest3}";
            
            // partial_4 => A.B.C.D
            string partial4 = $"{a}.{b}.{c}.{d}";

            Console.WriteLine($"Original Input  : {originalInput}");
            Console.WriteLine($"32-bit Integer  : {ipVal} (unsigned)");
            Console.WriteLine($"Dotted Decimal  : {dotted}");
            Console.WriteLine($"Decimal (full)  : {decimalForm}");
            Console.WriteLine($"Hexadecimal     : {hexForm}");
            Console.WriteLine($"Octal           : {octalForm}");
            Console.WriteLine($"Partial 1       : {partial1}");
            Console.WriteLine($"Partial 2       : {partial2}");
            Console.WriteLine($"Partial 3       : {partial3}");
            Console.WriteLine($"Partial 4       : {partial4}");
        }
    }
}
