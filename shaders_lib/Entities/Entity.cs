using System;
using OpenTK.Mathematics;

namespace shaders_lib.Entities
{
    /// <summary>
    /// Represents an in-game object to be drawn, contains positional data and a TexturedModel reference
    /// </summary>
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

        /// <summary>
        /// Function for ease of use, moves this entity relative to itself
        /// </summary>
        /// <param name="dx">The x amount to move by</param>
        /// <param name="dy">The y amount to move by</param>
        /// <param name="dz">The z amount to move by</param>
        public void Move(float dx, float dy, float dz)
        {
            var newPos = Position;
            newPos.X += dx;
            newPos.Y += dy;
            newPos.Z += dz;
            Position = newPos;
        }

        /// <summary>
        /// Function for ease of use, rotates this entity relative to itself
        /// </summary>
        /// <param name="rx">The x amount to rotate by</param>
        /// <param name="ry">The y amount to rotate by</param>
        /// <param name="rz">The z amount to rotate by</param>
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