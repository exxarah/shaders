using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace shaders_lib.Util
{
    public static class ImgLoader
    {
        public const string TexturePath = "Assets/textures/";
        public const string MissingTexture = "missing.png";

        /// <summary>
        /// Load an image into the currently bound Texture2D
        /// </summary>
        /// <param name="path">The image to load</param>
        public static Image<Rgba32> LoadImage(string path)
        {
            Image<Rgba32> image;
            try
            {
                image = Image.Load<Rgba32>(TexturePath + path);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                image = Image.Load<Rgba32>(TexturePath + MissingTexture);
            }

            image.Mutate(x => x.Flip(FlipMode.Vertical));
            return image;
        }
    }
}