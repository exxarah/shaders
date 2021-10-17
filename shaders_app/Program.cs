namespace shaders_app
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            using var window = new Window(1600, 900, "Shaders", 60);
            window.Run();
        }
    }
}