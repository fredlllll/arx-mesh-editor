using System.IO;
using System.Runtime.InteropServices;

namespace Assets.Scripts.Util
{
    public class StructWriter : BinaryWriter
    {
        public StructWriter(Stream baseStream) : base(baseStream)
        {

        }

        public StructWriter(Stream baseStream, System.Text.Encoding encoding, bool leaveOpen) : base(baseStream, encoding, leaveOpen) { }

        public void WriteStruct<T>(T obj)
        {
            var objectLength = Marshal.SizeOf(typeof(T));
            var objectBuffer = Marshal.AllocHGlobal(objectLength);
            Marshal.StructureToPtr(obj, objectBuffer, false);
            var objectBytes = new byte[objectLength];
            //TODO: can i maybe use the objectBuffer as byte[] directly without copying?
            Marshal.Copy(objectBuffer, objectBytes, 0, objectBytes.Length);
            Marshal.FreeHGlobal(objectBuffer);
            Write(objectBytes);
        }
    }
}
