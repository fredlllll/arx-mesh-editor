using System.IO;
using System.Runtime.InteropServices;

namespace Assets.Scripts.Util
{
    public class StructWriter : BinaryWriter
    {
        public StructWriter(Stream output) : base(output) { }

        public StructWriter(Stream output, System.Text.Encoding encoding) : base(output, encoding) { }

        public StructWriter(Stream output, System.Text.Encoding encoding, bool leaveOpen) : base(output, encoding, leaveOpen) { }

        public void WriteStruct<T>(T obj)
        {
            var objectLength = Marshal.SizeOf(typeof(T));
            var objectBuffer = Marshal.AllocHGlobal(objectLength);
            var objectBytes = new byte[objectLength];
            Marshal.Copy(objectBytes, 0, objectBuffer, objectLength);//manually zeroing buffer because the runtime might only partially write char arrays and memory isnt zeroed
            Marshal.StructureToPtr(obj, objectBuffer, true);
            Marshal.Copy(objectBuffer, objectBytes, 0, objectBytes.Length);
            Marshal.FreeHGlobal(objectBuffer);
            Write(objectBytes);
        }
    }
}
