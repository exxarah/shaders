using OpenTK.Mathematics;

namespace shaders_lib.Entities
{
    public class Light
    {
        public Vector3 Position { get; private set; }
        public Vector3 Color { get; private set; }

        public Light(Vector3 position, Vector3 color)
        {
            Position = position;
            Color = color;
        }
    }
}