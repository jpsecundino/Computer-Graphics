using Silk.NET.OpenGL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Runtime.InteropServices;

namespace AllianceEngine
{
    public sealed class Texture : IDisposable
    {
        private uint _handle;
        private GL _gl;

        public unsafe Texture(string path)
        {
            Image<Rgba32> img = (Image<Rgba32>) Image.Load(path);
            img.Mutate(x => x.Flip(FlipMode.Vertical));

            fixed (void* data = &MemoryMarshal.GetReference(img.GetPixelRowSpan(0)))
            {
                Load(data, (uint) img.Width, (uint) img.Height);
            }

            img.Dispose();
        }

        public unsafe Texture(Span<byte> data, uint width, uint height)
        {
            fixed (void* d = &data[0])
            {
                Load(d, width, height);
            }
        }

        private unsafe void Load(void* data, uint width, uint height)
        {
            _gl = Program.Gl;

            _handle = _gl.GenTexture();
            Bind();

            _gl.TexImage2D(TextureTarget.Texture2D, 0, (int) InternalFormat.Rgba, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, data);
            _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) GLEnum.ClampToEdge);
            _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) GLEnum.ClampToEdge);
            _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) GLEnum.Linear);
            _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) GLEnum.Linear);
            _gl.GenerateMipmap(TextureTarget.Texture2D);
        }

        public void Bind(TextureUnit textureSlot = TextureUnit.Texture0)
        {
            _gl.ActiveTexture(textureSlot);
            _gl.BindTexture(TextureTarget.Texture2D, _handle);
        }

        public void Dispose()
        {
            _gl.DeleteTexture(_handle);
        }
    }
}
