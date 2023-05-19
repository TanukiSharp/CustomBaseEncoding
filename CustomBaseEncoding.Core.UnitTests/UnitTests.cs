using System.Text;

namespace CustomBaseEncoding.Core.UnitTests;

public class UnitTests
{
    private static void Test(byte[] input, string? expected)
    {
        string encoded = CustomBaseEncoding.Encode(input, Alphabets.Base62Alphabet);

        if (expected != null)
            Assert.Equal(expected, encoded);

        byte[] decoded = CustomBaseEncoding.Decode(encoded, Alphabets.Base62Alphabet);

        Assert.Equal(input, decoded);
    }

    [Fact]
    public void TestEncodingDecoding()
    {
        Test(Encoding.UTF8.GetBytes("Hello world"), "73XpUgyMzAI8sNQ");
        Test(Encoding.UTF8.GetBytes("Hello world!"), "T8dgcjRGuYUueWht");
        Test(Encoding.UTF8.GetBytes("Hello World"), "73XpUgyMwkGr29M");
        Test(Encoding.UTF8.GetBytes("Hello World!"), "T8dgcjRGkZ3aysdN");
        Test(Encoding.UTF8.GetBytes("This is a test."), "cZcNI3MKR9ies3y7yp6U");
        Test(Encoding.UTF8.GetBytes("漢字カタカナひらがな"), "3CyUfBbizaIiKEGa7ut7NONQaLOHalr7kuSJEycEE");
        Test(new byte[] { 0, 0, 0, 1, 2, 3, 4}, "00018wom");

        for (int i = 0; i < 1000; i++)
        {
            int size = Random.Shared.Next(512);
            byte[] bytes = new byte[size];
            Random.Shared.NextBytes(bytes);

            Test(bytes, null);
        }
    }
}
