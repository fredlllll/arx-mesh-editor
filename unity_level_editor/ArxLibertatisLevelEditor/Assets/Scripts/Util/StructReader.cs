using System.IO;
using System.Runtime.InteropServices;

namespace Assets.Scripts.Util
{
    public class StructReader : BinaryReader
    {
        public StructReader(Stream baseStream) : base(baseStream)
        {

        }

        public T ReadStruct<T>()
        {
            var objectLength = Marshal.SizeOf(typeof(T));
            var bytes = ReadBytes(objectLength);
            var pinnedBytes = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            var readObject = (T)Marshal.PtrToStructure(pinnedBytes.AddrOfPinnedObject(), typeof(T));
            pinnedBytes.Free();
            return readObject;
        }
    }
}
