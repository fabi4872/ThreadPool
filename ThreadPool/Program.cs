class Program
{
    // Definir path
    const string path = @"F:\Hilos\";

    // Definir el número de tareas a realizar en paralelo
    static int numTasks = 50;

    // Setear en false que todas las tareas finalizaron
    static ManualResetEvent allTasksCompleted = new ManualResetEvent(false);

    static void Main(string[] args)
    {
        Console.WriteLine("Iniciando tareas...");

        for (int i = 0; i < numTasks; i++)
        {
            // Asignar una tarea al ThreadPool
            ThreadPool.QueueUserWorkItem(DoTask, i);
        }

        // Esperar a que todas las tareas terminen
        allTasksCompleted.WaitOne();

        Console.WriteLine("Todas las tareas han terminado.");
    }

    static void DoTask(object data)
    {
        // Simular una tarea que lleva tiempo
        Thread.Sleep(1000);

        // Castear data
        int i = (int)data;

        // Definir archivo
        using (var sw = new StreamWriter(path + i + ".txt"))
        {
            // Thread.CurrentThread.ManagedThreadId (devuelve número de hilo que está ejecutando la función)
            sw.WriteLine($"Tarea: {i}. Hilo de ejecución: {Thread.CurrentThread.ManagedThreadId}");
        }
        Console.WriteLine($"Tarea {i}, completada en el hilo {Thread.CurrentThread.ManagedThreadId}");

        // Verificar si todas las tareas han terminado
        if (Interlocked.Decrement(ref numTasks) == 0)
        {
            // Señalizar el evento ManualResetEvent, indicando que todas las tareas han terminado su ejecución
            allTasksCompleted.Set();
        }
    }
}
