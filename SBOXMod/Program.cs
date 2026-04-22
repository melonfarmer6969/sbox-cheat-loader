using System;
using System.Reflection;
using System.Runtime.Loader;

public static class Program
{
    public static void Main()
    {
        try
        {
            try
            {
                RegisterWithTypeLibrary(Assembly.GetExecutingAssembly());
                LogWithTitle("Loaded types :D");
            }
            catch (Exception ex)
            {
                LogWithTitle("Failed to load types :(" + Environment.NewLine + ex.Message);
            }

            try
            {
                AddConsoleCommands(Assembly.GetExecutingAssembly());
                LogWithTitle("Loaded console commands :D");
            }
            catch (Exception ex)
            {
                LogWithTitle("Failed to load commands :(" + Environment.NewLine + ex.Message);
            }
        }
        catch (Exception ex)
        {
            LogWithTitle("Unexpected error: " + ex.Message);
        }
    }



    private static void LogWithTitle(string text)
    {
        Console.WriteLine("[POOP] " + text);
    }

    private static Type GetTypeFromAssembly(string assemblyName, string typeName)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var assembly in assemblies)
        {
            if (assembly.GetName().Name == assemblyName)
            {
                return assembly.GetType(typeName);
            }
        }
        throw new Exception($"Assembly '{assemblyName}' not found or does not contain type '{typeName}'.");
    }

    internal static void RegisterWithTypeLibrary(Assembly assembly)
    {
        var gameType = GetTypeFromAssembly("Sandbox.Engine", "Sandbox.Game");
        if (gameType == null)
            throw new Exception("Type 'Sandbox.Game' not found in assembly 'Sandbox.Engine'.");

        // Get the TypeLibrary property
        var typeLibraryProperty = gameType.GetProperty("TypeLibrary",
            BindingFlags.Static | BindingFlags.Public);

        if (typeLibraryProperty == null)
            throw new Exception("Property 'TypeLibrary' not found on 'Sandbox.Game'.");

        var typeLibraryInstance = typeLibraryProperty.GetValue(null);

        if (typeLibraryInstance == null)
            throw new Exception("'TypeLibrary' property returned null.");

        var addAssemblyMethod = typeLibraryInstance.GetType().GetMethod(
            "AddAssembly",
            BindingFlags.Instance | BindingFlags.NonPublic,
            null,
            new Type[] { typeof(Assembly), typeof(bool) },
            null
        );

        if (addAssemblyMethod == null)
            throw new Exception("Method 'AddAssembly(Assembly, bool)' not found on TypeLibrary instance.");

        addAssemblyMethod.Invoke(typeLibraryInstance, new object[] { assembly, true });
    }

    internal static void AddConsoleCommands(Assembly assembly)
    {
        Assembly sandboxEngineAssembly = null;
        foreach (var loadedAssembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            if (loadedAssembly.GetName().Name == "Sandbox.Engine")
            {
                sandboxEngineAssembly = loadedAssembly;
                break;
            }
        }

        if (sandboxEngineAssembly == null)
            throw new Exception("Assembly 'Sandbox.Engine' not loaded in the current AppDomain.");

        var conVarSystemType = sandboxEngineAssembly.GetType("Sandbox.ConVarSystem");
        if (conVarSystemType == null)
            throw new Exception("Type 'Sandbox.ConVarSystem' not found in assembly 'Sandbox.Engine'.");

        var addAssemblyMethod = conVarSystemType.GetMethod(
            "AddAssembly",
            BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public,
            null,
            new Type[] { typeof(Assembly), typeof(string), typeof(string) },
            null
        );

        if (addAssemblyMethod == null)
            throw new Exception("Method 'AddAssembly(Assembly, string, string)' not found on 'Sandbox.ConVarSystem'.");

        addAssemblyMethod.Invoke(null, new object[] { assembly, "game", null });
    }
}
