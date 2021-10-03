using OpenTK.Mathematics;

namespace shaders_lib
{
    public class Entity
    {
        public TexturedModel Model { get; }
        public Vector3 Position { get; private set; }
        public Vector3 Rotation { get; private set; }
        public float Scale { get; private set; }

        public Entity(TexturedModel model, Vector3 position, Vector3 rotation, float scale)
        {
            Model = model;
            Position = position;
            Rotation = rotation;
            Scale = scale;
        }

        public Entity(TexturedModel model)
        {
            Model = model;
            Position = Vector3.Zero;
            Rotation = Vector3.Zero;
            Scale = 1f;
        }

        public Entity(TexturedModel model, Vector3 position)
        {
            Model = model;
            Position = position;
            Rotation = Vector3.Zero;
            Scale = 1f;
        }

        public void Move(float dx, float dy, float dz)
        {
            var newPos = Position;
            newPos.X += dx;
            newPos.Y += dy;
            newPos.Z += dz;
            Position = newPos;
        }

        public void Rotate(float rx, float ry, float rz)
        {
            var newRot = Rotation;
            newRot.X += rx;
            newRot.Y += ry;
            newRot.Z += rz;
            Rotation = newRot;
        }
    }
}