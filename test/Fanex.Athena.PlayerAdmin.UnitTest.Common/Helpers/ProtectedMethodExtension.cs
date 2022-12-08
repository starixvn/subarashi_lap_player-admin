using NSubstitute;
using System;
using System.Reflection;

namespace Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers
{
    public static class ProtectedMethodExtension
    {
        public static object SubstituteProtected(this object substitute, string memberName, params object[] args)
        {
            var method = substitute
                .GetType()
                .GetMethod(memberName, BindingFlags.Instance | BindingFlags.NonPublic);

            if (!method.IsVirtual)
            {
                throw new Exception("Must be a virtual member");
            }
            return method.Invoke(substitute, args);
        }

        public static object InvokeProtected(this object obj, string memberName, params object[] args)
        {
            var method = obj
                .GetType()
                .GetMethod(memberName, BindingFlags.Instance | BindingFlags.NonPublic);

            return method.Invoke(obj, args);
        }
    }
}
