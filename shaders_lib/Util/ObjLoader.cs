using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using Vector3 = OpenTK.Mathematics.Vector3;

namespace shaders_lib.Util
{
    public static class ObjLoader
    {
        public static ModelData Parse(string path)
        {
            StreamReader file = new StreamReader(path);
            string line;
            List<Vertex> vertices = new List<Vertex>();
            List<Vector2> uvs = new List<Vector2>();
            List<Vector3> normals = new List<Vector3>();
            List<int> indices = new List<int>();
            while ((line = file.ReadLine()) != null)
            {
                if (line.StartsWith("v "))
                {
                    string[] currentLine = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                    Vector3 vertex = new Vector3(
                        float.Parse(currentLine[1]),
                        float.Parse(currentLine[2]),
                        float.Parse(currentLine[3])
                    );
                    Vertex newVertex = new Vertex(vertices.Count, vertex);
                    vertices.Add(newVertex);
                } else if (line.StartsWith("vt "))
                {
                    string[] currentLine = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                    Vector2 texture = new Vector2(
                        float.Parse(currentLine[1]),
                        float.Parse(currentLine[2])
                    );
                    uvs.Add(texture);
                } else if (line.StartsWith("vn "))
                {
                    string[] currentLine = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                    Vector3 normal = new Vector3(
                        float.Parse(currentLine[1]),
                        float.Parse(currentLine[2]),
                        float.Parse(currentLine[3])
                    );
                    normals.Add(normal);
                } else if (line.StartsWith("f "))
                {
                    string[] currentLine = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                    string[] vertex1 = currentLine[1].Split("/");
                    string[] vertex2 = currentLine[2].Split("/");
                    string[] vertex3 = currentLine[3].Split("/");

                    ProcessVertex(vertex1, ref vertices, ref indices);
                    ProcessVertex(vertex2, ref vertices, ref indices);
                    ProcessVertex(vertex3, ref vertices, ref indices);
                }
            }

            vertices = RemoveUnusedVertices(vertices);
            int[] indicesArray = indices.ToArray();
            float[] verticesArray = new float[vertices.Count * 3];
            float[] texturesArray = new float[vertices.Count * 2];
            float[] normalsArray = new float[vertices.Count * 3];
            float furthest = ConvertDataToArrays(
                vertices, uvs, normals,
                ref verticesArray, ref texturesArray, ref normalsArray
                );

            ModelData data = new ModelData(verticesArray, texturesArray, normalsArray, indicesArray, furthest);
            return data;
        }
        
        private static void ProcessVertex(string[] vertex, ref List<Vertex> vertices, ref List<int> indices)
        {
            int index = int.Parse(vertex[0]) - 1;
            int textureIndex = int.Parse(vertex[1]) - 1;
            int normalIndex = int.Parse(vertex[2]) - 1;
            
            Vertex currentVertex = vertices[index];
            if (!currentVertex.IsSet)
            {
                currentVertex.TextureIndex = textureIndex;
                currentVertex.NormalIndex = normalIndex;
                indices.Add(index);
            }
            else
            {
                AlreadyProcessedVertex(textureIndex, normalIndex, ref currentVertex, ref indices, ref vertices);
            }
        }
        
        private static float ConvertDataToArrays(List<Vertex> vertices, List<Vector2> uvs, List<Vector3> normals, ref float[] verticesArray, ref float[] texturesArray, ref float[] normalsArray)
        {
            float furthestPoint = 0;
            for (int i = 0; i < vertices.Count; i++)
            {
                Vertex currentVertex = vertices[i];
                furthestPoint = Math.Max(furthestPoint, currentVertex.Length);

                Vector3 position = currentVertex.Position;
                Vector2 uvCoord = uvs[currentVertex.TextureIndex];
                Vector3 normalVector = normals[currentVertex.NormalIndex];

                verticesArray[i * 3 + 0] = position.X;
                verticesArray[i * 3 + 1] = position.Y;
                verticesArray[i * 3 + 2] = position.Z;

                texturesArray[i * 2 + 0] = uvCoord.X;
                texturesArray[i * 2 + 1] = uvCoord.Y;
                
                normalsArray[i * 3 + 0] = normalVector.X;
                normalsArray[i * 3 + 1] = normalVector.Y;
                normalsArray[i * 3 + 2] = normalVector.Z;
            }

            return furthestPoint;
        }

        private static List<Vertex> RemoveUnusedVertices(List<Vertex> vertices)
        {
            foreach (var vertex in vertices)
            {
                if (!vertex.IsSet)
                {
                    vertex.NormalIndex = 0;
                    vertex.TextureIndex = 0;
                }
            }

            return vertices;
        }

        private static void AlreadyProcessedVertex(int textureIndex, int normalIndex, ref Vertex previousVertex, ref List<int> indices, ref List<Vertex> vertices)
        {
            if (previousVertex.SameTextureAndNormal(textureIndex, normalIndex))
            {
                indices.Add(previousVertex.Index);
            }
            else
            {
                Vertex anotherVertex = previousVertex.DuplicateVertex;
                if (anotherVertex != null)
                {
                    AlreadyProcessedVertex(textureIndex, normalIndex, ref anotherVertex, ref indices, ref vertices);
                }
                else
                {
                    Vertex duplicateVertex = new Vertex(vertices.Count, previousVertex.Position);
                    duplicateVertex.TextureIndex = textureIndex;
                    duplicateVertex.NormalIndex = normalIndex;
                    previousVertex.DuplicateVertex = duplicateVertex;
                    vertices.Add(duplicateVertex);
                    indices.Add(duplicateVertex.Index);
                }
            }
        }
    }
}