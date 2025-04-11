#!/usr/bin/env python3

import socket
import struct

def ip_to_int(ip_str):
    """
    Convert an IPv4 string in any known partial/legacy/decimal/hex/octal format
    to a 32-bit integer.
    """
    # socket.inet_aton will parse standard dotted notation, plus some partial forms (on many systems).
    # However, if your Python build doesn't parse partial forms, you may need manual logic.
    packed_ip = socket.inet_aton(ip_str)
    # Unpack the 4 bytes into a single 32-bit int
    return struct.unpack("!I", packed_ip)[0]

def int_to_ip(n):
    """
    Convert a 32-bit integer to a standard dotted-decimal IPv4 string.
    """
    return ".".join(str((n >> shift) & 0xFF) for shift in (24, 16, 8, 0))

def generate_all_formats(ip_str):
    """
    Generate a dictionary of different IP string formats from the original IP input.
    """
    ip_val = ip_to_int(ip_str)  # Convert to 32-bit integer

    # Break it down into four octets
    a = (ip_val >> 24) & 0xFF
    b = (ip_val >> 16) & 0xFF
    c = (ip_val >> 8) & 0xFF
    d = ip_val & 0xFF
    
    # Typical dotted-decimal
    dotted = int_to_ip(ip_val)

    # Single decimal
    decimal_repr = str(ip_val)

    # Hex (0x prefix)
    hex_repr = hex(ip_val)

    # Octal (0o prefix in Python)
    octal_repr = oct(ip_val)

    # Partial forms
    # 1-part: a 32-bit decimal (same as decimal_repr, but let's list it anyway)
    partial_1 = f"{ip_val}"
    
    # 2-part: a.(b << 16 | c << 8 | d)
    #   i.e., first octet, then everything else in decimal
    partial_2 = f"{a}.{(b << 16) + (c << 8) + d}"
    
    # 3-part: a.b.(c << 8 | d)
    partial_3 = f"{a}.{b}.{(c << 8) + d}"
    
    # 4-part: a.b.c.d (the usual dotted-decimal)
    partial_4 = f"{a}.{b}.{c}.{d}"
    
    return {
        "original_input": ip_str,
        "32_bit_integer": ip_val,
        "dotted_decimal": dotted,
        "decimal": decimal_repr,
        "hex": hex_repr,
        "octal": octal_repr,
        "partial_1": partial_1,
        "partial_2": partial_2,
        "partial_3": partial_3,
        "partial_4": partial_4
    }

def main():
    import sys
    
    if len(sys.argv) < 2:
        print(f"Usage: {sys.argv[0]} <IP_in_any_format>")
        print("Example: ./ipconverter.py 192.201")
        sys.exit(1)
    
    ip_str = sys.argv[1]
    
    try:
        results = generate_all_formats(ip_str)
        for k, v in results.items():
            print(f"{k:20} => {v}")
    except OSError:
        print(f"Error: '{ip_str}' could not be parsed as an IP address on this system.")
        sys.exit(2)

if __name__ == "__main__":
    main()
