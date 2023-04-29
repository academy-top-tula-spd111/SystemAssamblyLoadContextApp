using System.Reflection;
using System.Runtime.Loader;


Square(10);
GC.Collect();
GC.WaitForPendingFinalizers();

Console.WriteLine();

foreach (Assembly assm in AppDomain.CurrentDomain.GetAssemblies())
{
    Console.WriteLine(assm.GetName().Name);
}

void Square(int number)
{
    var context = new AssemblyLoadContext("Square", true);
    context.Unloading += Context_Unloading;

    var assemblyPath = Path.Combine(Directory.GetCurrentDirectory(), "SquareLib.dll");
    Assembly assembly = context.LoadFromAssemblyPath(assemblyPath);

    var type = assembly.GetType("SquareLib.Program");
    if(type != null )
    {
        var methodSquare = type.GetMethod("Square", BindingFlags.Static | BindingFlags.NonPublic);
        var result = methodSquare?.Invoke(null, new object[] { number });
        if(result is int)
        {
            Console.WriteLine($"{number}^2 = {result}");
        }
    }

    foreach(Assembly assm in AppDomain.CurrentDomain.GetAssemblies())
    {
        Console.WriteLine(assm.GetName().Name);
    }

    context.Unload();
}

void Context_Unloading(AssemblyLoadContext context)
{
    Console.WriteLine("Lib Square unloaded");
}