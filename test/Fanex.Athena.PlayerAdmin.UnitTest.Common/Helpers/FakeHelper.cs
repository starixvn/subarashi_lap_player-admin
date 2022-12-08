using AutoFixture;
using AutoFixture.Dsl;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers
{
    public static class FakeHelper
    {
        private static Fixture _fixture = new Fixture();

        public static T Create<T>(Expression<Func<T, object>> propertyPicker, object value)
        {
            return _fixture.Build<T>().With(propertyPicker, value).Create();
        }

        public static T Create<T>()
           => _fixture.Create<T>();


        public static IEnumerable<T> CreateMany<T>()        
            => _fixture.CreateMany<T>();
        

        public static IEnumerable<T> CreateMany<T>(Expression<Func<T, object>> propertyPicker, object value)
        {
            var builder = _fixture.Build<T>();
            builder.With(_ => propertyPicker, value);

            return builder.CreateMany();
        }

        public static IEnumerable<T> CreateMany<T>(int count)        
            => _fixture.CreateMany<T>(count);
        

        public static IEnumerable<T> CreateMany<T>(Expression<Func<T, object>> propertyPicker, object value, int count)
        {
            var builder = _fixture.Build<T>();
            builder.With(_ => propertyPicker, value);

            return builder.CreateMany(count);
        }

        public static ICustomizationComposer<T> GetFakeBuilder<T>()
            => (new Fixture()).Build<T>();

        public static DateTime TruncateMiliseconds(DateTime dateTime)
        {
            return dateTime.AddTicks(-(dateTime.Ticks % TimeSpan.TicksPerSecond));
        }
    }
}
