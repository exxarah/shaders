using System;
using OpenTK.Mathematics;

namespace shaders_lib.Entities
{
    public class Camera
    {
        private Vector3 _front = -Vector3.UnitZ;
        private Vector3 _up = Vector3.UnitY;
        private Vector3 _right = Vector3.UnitX;
        
        private float _pitch;
        private float _roll = MathHelper.PiOver2;
        private float _yaw = MathHelper.PiOver2;
        private float _fov = MathHelper.PiOver2;

        public Vector3 Position { get; private set; }

        public float Pitch
        {
            get => MathHelper.RadiansToDegrees(_pitch);
            set
            {
                var angle = MathHelper.Clamp(value, -89f, 89f);
                _pitch = MathHelper.DegreesToRadians(angle);
                UpdateVectors();
            }
        }
        public float Roll
        {
            get => MathHelper.RadiansToDegrees(_roll);
            private set
            {
                _roll = MathHelper.DegreesToRadians(value);
                UpdateVectors();
            }
        }
        public float Yaw
        {
            get => MathHelper.RadiansToDegrees(_yaw);
            set
            {
                var angle = MathHelper.Clamp(value, 1f, 45f);
                UpdateVectors();
            }
        }

        public Vector2i Size;
        public float AspectRatio => Size.X / (float)Size.Y;

        public float Fov
        {
            get => MathHelper.RadiansToDegrees(_fov);
            set
            {
                var angle = MathHelper.Clamp(value, 1f, 45f);
                _fov = MathHelper.DegreesToRadians(angle);
            }
        }
        public float NearPlane { get; private set; }
        public float FarPlane { get; private set; }

        public Vector3 Front => _front;
        public Vector3 Up => _up;
        public Vector3 Right => _right;

        public Camera(Vector3 position, Vector2i displaySize)
        {
            Position = position;
            Size = displaySize;
            NearPlane = 0.01f;
            FarPlane = 1000f;
        }

        /// <summary>
        /// Function for ease of use, moves this camera relative to itself
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
        /// Get the ViewMatrix for this camera
        /// </summary>
        /// <returns>The Matrix4 ViewMatrix</returns>
        public Matrix4 GetViewMatrix()
        {
            return Matrix4.Transpose(Matrix4.LookAt(Position, Position + Front, Up));
        }

        /// <summary>
        /// Get the ProjectionMatrix for this camera
        /// </summary>
        /// <returns>The Matrix4 ProjectionMatrix</returns>
        public Matrix4 GetProjectionMatrix()
        {
            return Matrix4.Transpose(Matrix4.CreatePerspectiveFieldOfView(_fov, AspectRatio, NearPlane, FarPlane));
        }

        /// <summary>
        /// Update the direction of vectors, called automatically in properties when Rotations are changed.
        /// </summary>
        private void UpdateVectors()
        {
            _front.X = MathF.Cos(_pitch) * MathF.Cos(_yaw);
            _front.Y = MathF.Sin(_pitch);
            _front.Z = MathF.Cos(_pitch) * MathF.Sin(_yaw);

            _front = Vector3.Normalize(_front);
            _right = Vector3.Normalize(Vector3.Cross(_front, Vector3.UnitY));
            _up = Vector3.Normalize(Vector3.Cross(_right, _front));
        }
    }
}