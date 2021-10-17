using OpenTK.Mathematics;

namespace shaders_lib.Util
{
    public class Maths
    {
        /// <summary>
        /// Create a transformation matrix, given a translation vector, rotation values, and scale
        /// </summary>
        /// <param name="translation">The translation to apply</param>
        /// <param name="rx">The x rotation to apply</param>
        /// <param name="ry">The y rotation to apply</param>
        /// <param name="rz">The z rotation to apply</param>
        /// <param name="scale">The amount to scale by</param>
        /// <returns></returns>
        public static Matrix4 CreateTransformationMatrix(
            Vector3 translation,
            float rx, float ry, float rz,
            float scale)
        {
            Matrix4 matrix = Matrix4.Identity;
            
            matrix *= Matrix4.CreateTranslation(translation);
            matrix *= Matrix4.CreateRotationX(MathHelper.DegreesToRadians(rx));
            matrix *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians(ry));
            matrix *= Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(rz));
            matrix *= Matrix4.CreateScale(scale);

            // https://stackoverflow.com/questions/11452241/opentk-matrix-transformations
            return Matrix4.Transpose(matrix);
        }

        /// <summary>
        /// Create a transformation, given a translation vector, rotation vector, and scale
        /// </summary>
        /// <param name="translation">The translation to apply</param>
        /// <param name="rotation">The rotation to apply</param>
        /// <param name="scale">The amount to scale by</param>
        /// <returns></returns>
        public static Matrix4 CreateTransformationMatrix(
            Vector3 translation,
            Vector3 rotation,
            float scale)
        {
            return CreateTransformationMatrix(translation, rotation.X, rotation.Y, rotation.Z, scale);
        }

        public static Matrix4 CreateTransformationMatrix(
            System.Numerics.Vector3 translation,
            System.Numerics.Vector3 rotation,
            float scale)
        {
            return CreateTransformationMatrix(SystemToTkVector3(translation), SystemToTkVector3(rotation), scale);
        }

        public static System.Numerics.Vector3 TkToSystemVector3(OpenTK.Mathematics.Vector3 input)
        {
            return new System.Numerics.Vector3(input.X, input.Y, input.Z);
        }

        public static OpenTK.Mathematics.Vector3 SystemToTkVector3(System.Numerics.Vector3 input)
        {
            return new Vector3(input.X, input.Y, input.Z);
        }
    }
}