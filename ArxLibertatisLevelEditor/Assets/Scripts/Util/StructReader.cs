using System.IO;
using System.Runtime.InteropServices;

namespace Assets.Scripts.Util
{
    public class StructReader : BinaryReader
    {
        public StructReader(Stream baseStream) : base(baseStream) { }

        public StructReader(Stream baseStream, System.Text.Encoding encoding, bool leaveOpen) : base(baseStream, encoding, leaveOpen) { }

        public T ReadStruct<T>()
        {
            var objectLength = Marshal.SizeOf(typeof(T));
            if(BaseStream.Position+objectLength> BaseStream.Length)
            {
                throw new System.Exception("trying to read struct over end of stream");
            }
            var bytes = ReadBytes(objectLength);
            var pinnedBytes = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            var readObject = (T)Marshal.PtrToStructure(pinnedBytes.AddrOfPinnedObject(), typeof(T));
            pinnedBytes.Free();
            return readObject;
        }
    }
}
