using Assets.Scripts.ArxNative.IO.PK;
using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Assets.Scripts.ArxNative.IO
{
    public static class ArxIO
    {
        static ArxIO()
        {
            ArxIONative.Init();
        }

        public static byte[] Unpack(byte[] bytes)
        {
            uint inSize = (uint)bytes.Length;

            var pinnedBytes = GCHandle.Alloc(bytes, GCHandleType.Pinned);

            ArxIONative.UnpackAlloc(pinnedBytes.AddrOfPinnedObject(), inSize, out IntPtr outPtr, out uint outSize);

            pinnedBytes.Free();

            var outBytes = new byte[outSize];
            Marshal.Copy(outPtr, outBytes, 0, (int)outSize);

            ArxIONative.UnpackFree(outPtr);

            return outBytes;
        }

        public static byte[] Pack(byte[] bytes)
        {
            var packed = Implode.DoImplode(bytes);
            Debug.Log("Packed from " + bytes.Length + " bytes to " + packed.Length + " bytes");
            return packed;
            /*uint inSize = (uint)bytes.Length;
            uint outSize = inSize * 500;

            var pinnedBytes = GCHandle.Alloc(bytes, GCHandleType.Pinned);

            var result = ArxIONative.PackAlloc(pinnedBytes.AddrOfPinnedObject(), inSize, out IntPtr outPtr, outSize);
            pinnedBytes.Free();
            if (result != ImplodeResult.IMPLODE_SUCCESS)
            {
                Debug.LogError("Error packing level data: " + result.ToString());

                return null;
            }

            var outBytes = new byte[outSize];
            Marshal.Copy(outPtr, outBytes, 0, (int)outSize);

            ArxIONative.PackFree(outPtr);

            return outBytes;*/

            //return bytes; //because arxio doesnt support that yet im just gonna save uncompressed
        }
    }

    /*public enum ImplodeResult
    {
        IMPLODE_SUCCESS = 0, // No errors occurred
        IMPLODE_INVALID_DICTSIZE = 1, // An invalid dictionary size was selected
        IMPLODE_INVALID_MODE = 2, // An invalid mode for literal bytes was selected
        IMPLODE_BUFFER_TOO_SMALL = 4 // Output buffer is too small
    };*/

    public static class ArxIONative
    {
        [DllImport("ArxIO", EntryPoint = "ArxIO_init", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Init();
        [DllImport("ArxIO", EntryPoint = "ArxIO_getError", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetError(IntPtr outMessage, int size);
        [DllImport("ArxIO", EntryPoint = "ArxIO_getLogLine", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetLogLine(IntPtr outMessage, int size);
        [DllImport("ArxIO", EntryPoint = "ArxIO_unpack_alloc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void UnpackAlloc(IntPtr in_, uint inSize, out IntPtr out_, out uint outSize);
        [DllImport("ArxIO", EntryPoint = "ArxIO_unpack_free", CallingConvention = CallingConvention.Cdecl)]
        public static extern void UnpackFree(IntPtr buffer);

        /*[DllImport("ArxLibertatisEditorIO", EntryPoint = "ArxIO_pack_alloc", CallingConvention = CallingConvention.Cdecl)]
        public static extern ImplodeResult PackAlloc(IntPtr in_, uint inSize, out IntPtr out_, uint outSize);
        [DllImport("ArxLibertatisEditorIO", EntryPoint = "ArxIO_pack_free", CallingConvention = CallingConvention.Cdecl)]
        public static extern void PackFree(IntPtr buffer);*/
    }
}
