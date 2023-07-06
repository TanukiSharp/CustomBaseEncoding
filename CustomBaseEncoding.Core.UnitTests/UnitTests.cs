using System.Text;

namespace CustomBaseEncoding.Core.UnitTests;

public class UnitTests
{
    private static void Test(byte[] input, string alphabet, string? expected = null)
    {
        string encoded = CustomBaseEncoding.Encode(input, alphabet);

        if (expected != null)
            Assert.Equal(expected, encoded);

        byte[] decoded = CustomBaseEncoding.Decode(encoded, alphabet);

        Assert.Equal(input, decoded);
    }

    [Fact]
    public void TestBase62EncodingDecoding()
    {
        Test(Encoding.UTF8.GetBytes("Hello world"), Alphabets.Base62Alphabet, "73XpUgyMzAI8sNQ");
        Test(Encoding.UTF8.GetBytes("Hello world!"), Alphabets.Base62Alphabet, "T8dgcjRGuYUueWht");
        Test(Encoding.UTF8.GetBytes("Hello World"), Alphabets.Base62Alphabet, "73XpUgyMwkGr29M");
        Test(Encoding.UTF8.GetBytes("Hello World!"), Alphabets.Base62Alphabet, "T8dgcjRGkZ3aysdN");
        Test(Encoding.UTF8.GetBytes("This is a test."), Alphabets.Base62Alphabet, "cZcNI3MKR9ies3y7yp6U");
        Test(Encoding.UTF8.GetBytes("漢字カタカナひらがな"), Alphabets.Base62Alphabet, "3CyUfBbizaIiKEGa7ut7NONQaLOHalr7kuSJEycEE");
        Test(new byte[] { 0, 0, 0, 1, 2, 3, 4}, Alphabets.Base62Alphabet, "00018wom");
    }

    [Theory]
    [InlineData(Alphabets.Base58AsciiOrderAlphabet)]
    [InlineData(Alphabets.Base58FlickrAlphabet)]
    [InlineData(Alphabets.Base62Alphabet)]
    public void TestEncodingDecoding(string alphabet)
    {
        for (int i = 0; i < 1000; i++)
        {
            int size = Random.Shared.Next(512);
            byte[] bytes = new byte[size];
            Random.Shared.NextBytes(bytes);

            Test(bytes, alphabet);
        }
    }

    [Fact]
    public void TestBase2EncodingDecoding()
    {
        Test(new byte[] { 97 }, "01", "1100001");
        Test(new byte[] { 0b101, 0b101_1101 }, "01", "10101011101");
    }
}
