using System.Text;

namespace CustomBaseEncoding.Core;

public sealed class Alphabets
{
    public const string Base58AsciiOrderAlphabet = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";
    public const string Base58FlickrAlphabet = "123456789abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ";
    public const string Base62Alphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
}

public sealed class CustomBaseEncoding
{
    public static string Encode(byte[] input, string alphabet)
    {
        if (alphabet.Length < 2)
            throw new ArgumentException("Alphabet does not contain enough characters, at least 2 are required.", nameof(alphabet));

        if (input.Length == 0)
            return string.Empty;

        int carry;

        List<int> digits = new() { 0 };

        for (int i = 0; i < input.Length; i++)
        {
            for (int j = 0; j < digits.Count; j++)
                digits[j] <<= 8;

            digits[0] += input[i];

            carry = 0;

            for (int j = 0; j < digits.Count; j++)
            {
                digits[j] += carry;
                carry = digits[j] / alphabet.Length;
                digits[j] %= alphabet.Length;
            }

            while (carry > 0)
            {
                digits.Add(carry % alphabet.Length);
                carry /= alphabet.Length;
            }
        }

        for (int i = 0; input[i] == 0 && i < input.Length - 1; i++)
            digits.Add(0);

        StringBuilder result = new();

        for (int i = digits.Count - 1; i >= 0; i--)
        {
            char c = alphabet[digits[i]];
            result.Append(c);
        }

        return result.ToString();
    }

    public static byte[] Decode(string input, string alphabet)
    {
        if (alphabet.Length < 2)
            throw new ArgumentException("Alphabet does not contain enough characters, at least 2 are required.", nameof(alphabet));

        if (input.Length == 0)
            return Array.Empty<byte>();

        for (int i = 0; i < input.Length; i++)
        {
            if (alphabet.Contains(input[i]) == false)
                throw new ArgumentException($"Base{alphabet.Length} alphabet '{alphabet}' does not contain character '{input[i]}'.");
        }

        List<int> bytes = new() { 0 };
        int carry;

        for (int i = 0; i < input.Length; i++)
        {
            char c = input[i];


            for (int j = 0; j < bytes.Count; j++)
                bytes[j] *= alphabet.Length;

            bytes[0] += alphabet.IndexOf(c);

            carry = 0;

            for (int j = 0; j < bytes.Count; j++)
            {
                bytes[j] += carry;
                carry = bytes[j] >> 8;
                bytes[j] &= 0xFF;
            }

            while (carry > 0)
            {
                bytes.Add(carry & 0xFF);
                carry >>= 8;
            }
        }

        for (int i = 0; input[i] == alphabet[0] && i < input.Length - 1; i++)
            bytes.Add(0);

        var result = new byte[bytes.Count];

        for (int i = 0; i < bytes.Count; i++)
            result[i] = (byte)bytes[bytes.Count - i - 1];

        return result;
    }
}
