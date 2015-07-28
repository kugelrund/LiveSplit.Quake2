using System;
using System.Diagnostics;
using System.Text;

namespace LiveSplit.Quake2
{
    class MemoryAddress
    {
        private IntPtr address;

        public MemoryAddress(int address)
        {
            this.address = new IntPtr(address);
        }

        public int Deref(Process p)
        {
            byte[] buffer = new byte[4];
            int readBytes;
            SafeNativeMethods.ReadProcessMemory(p.Handle, address, buffer, 4, out readBytes);
            return BitConverter.ToInt32(buffer, 0);
        }

        public string Deref(Process p, int maxLength)
        {
            byte[] buffer = new byte[maxLength];
            int readBytes;
            SafeNativeMethods.ReadProcessMemory(p.Handle, address, buffer, maxLength, out readBytes);

            int lastIndex = 0;
            while (lastIndex < maxLength && buffer[lastIndex] != 0 && buffer[lastIndex] != 46) // "." in ASCII is 46
            {
                lastIndex += 1;
            }

            return Encoding.ASCII.GetString(buffer, 0, lastIndex);
        }
    }
}
