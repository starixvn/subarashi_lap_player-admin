namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.JsonConverter
{
    using System.Buffers;
    using System.Text;
    using System.Text.Json;
    using Core.JsonConverter;

    public class StringToDateTimeConverterTests
    {
        [Fact]
        public void Read_ShouldParse_SimpleDate()
        {
            var json = "\"2024-05-01\"";
            var bytes = Encoding.UTF8.GetBytes(json);
            var reader = new Utf8JsonReader(bytes);
            reader.Read();
            var converter = new StringToDateTimeConverter();
            var value = converter.Read(ref reader, typeof(DateTime), new JsonSerializerOptions());
            Assert.Equal(new DateTime(2024, 5, 1), value);
        }

        [Fact]
        public void Read_ShouldThrow_ForInvalidDate()
        {
            var json = "\"not-a-date\"";
            var bytes = Encoding.UTF8.GetBytes(json);
            var reader = new Utf8JsonReader(bytes);
            reader.Read();
            var converter = new StringToDateTimeConverter();
            try
            {
                converter.Read(ref reader, typeof(DateTime), new JsonSerializerOptions());
                Assert.True(false);
            }
            catch (JsonException)
            {
                Assert.True(true);
            }
        }

        [Fact]
        public void Write_ShouldFormat_AsIsoWithoutOffset_WhenUnspecified()
        {
            var converter = new StringToDateTimeConverter();
            var buffer = new ArrayBufferWriter<byte>();
            using (var writer = new Utf8JsonWriter(buffer))
            {
                var dt = DateTime.SpecifyKind(new DateTime(2024, 5, 1, 14, 30, 0), DateTimeKind.Unspecified);
                converter.Write(writer, dt, new JsonSerializerOptions());
            }

            var written = Encoding.UTF8.GetString(buffer.WrittenSpan);
            Assert.Equal("\"2024-05-01T14:30:00\"", written);
        }
    }
}