using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace NETPayload
{
    public class Loader
    {
        public static unsafe int LoadAssemblyFromBytes(IntPtr args, int sizeBytes)
        {
            if (args != IntPtr.Zero && sizeBytes > 0)
            {
                Span<byte> span = new Span<byte>((void*)args, sizeBytes);
                byte[] assemblyBytes = span.ToArray();

                Assembly assembly = Assembly.Load(assemblyBytes);

                MethodInfo mainMethod = null;

                foreach (Type type in assembly.GetTypes())
                {
                    MethodInfo method = type.GetMethod("Main",
                        BindingFlags.Public | BindingFlags.Static);

                    if (method != null)
                    {
                        mainMethod = method;
                        break;
                    }
                }

                mainMethod?.Invoke(null, new object[0]);
            }

            return 0;
        }
    }
}
