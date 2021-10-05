namespace shaders_lib.Util
{
    public class ModelData
    {
        public float[] Vertices { get; private set; }
        public float[] Uvs { get; private set; }
        public float[] Normals { get; private set; }
        public int[] Indices { get; private set; }
        
        public float FurthestPoint { get; private set; }

        public ModelData(float[] vertices, float[] uvs, float[] normals, int[] indices, float furthestPoint)
        {
            Vertices = vertices;
            Uvs = uvs;
            Normals = normals;
            Indices = indices;
            FurthestPoint = furthestPoint;
        }
    }
}