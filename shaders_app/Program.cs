namespace shaders_app
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            using var window = new Window(800, 600, "Shaders");
            window.Run();
        }
    }
}