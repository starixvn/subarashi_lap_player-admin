namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.JsonConverter
{
    using System.Buffers;
    using System.Text;
    using System.Text.Json;
    using Core.JsonConverter;

    public class EmptyStringToNullConverterTests
    {
        [Fact]
        public void CanConvert_ShouldReturnTrue_ForStringType()
        {
            var converter = new EmptyStringToNullConverter();
            var result = converter.CanConvert(typeof(string));
            Assert.True(result);
        }

        [Fact]
        public void Read_ShouldReturnNull_WhenEmptyString()
        {
            var json = "\"\"";
            var bytes = Encoding.UTF8.GetBytes(json);
            var reader = new Utf8JsonReader(bytes);
            reader.Read();
            var converter = new EmptyStringToNullConverter();
            var value = converter.Read(ref reader, typeof(string), new JsonSerializerOptions());
            Assert.Null(value);
        }

        [Fact]
        public void Read_ShouldReturnValue_WhenNonEmptyString()
        {
            var json = "\"abc\"";
            var bytes = Encoding.UTF8.GetBytes(json);
            var reader = new Utf8JsonReader(bytes);
            reader.Read();
            var converter = new EmptyStringToNullConverter();
            var value = converter.Read(ref reader, typeof(string), new JsonSerializerOptions());
            Assert.Equal("abc", value);
        }

        [Fact]
        public void Write_ShouldWriteSameValue()
        {
            var converter = new EmptyStringToNullConverter();
            var buffer = new ArrayBufferWriter<byte>();
            using (var writer = new Utf8JsonWriter(buffer))
            {
                converter.Write(writer, "abc", new JsonSerializerOptions());
            }

            var written = Encoding.UTF8.GetString(buffer.WrittenSpan);
            Assert.Equal("\"abc\"", written);
        }
    }
}