using System;
using System.Collections.Generic;
using System.Text;

namespace IBEXBle.Core
{
    public class LittleEndian
    {
        public static ushort ToUint16(byte[] bytes)
        {
            return ToUint16(bytes, 0);
        }
        public static ushort ToUint16(byte[] bytes, int index)
        {
            ushort result;
            if (BitConverter.IsLittleEndian) //현재 컴퓨터가 LittleEndian일때
            {
                result = BitConverter.ToUInt16(bytes, index);
            }
            else
            {
                result = BitConverter.ToUInt16(bytes, index);
                byte[] b = new byte[2];
                b = BitConverter.GetBytes(result);
                Array.Reverse(b);
                result = BitConverter.ToUInt16(b, 0);
            }
            return result;
        }

        public static int ToInt32(byte[] bytes)
        {
            return ToInt32(bytes, 0);
        }

        public static int ToInt32(byte[] bytes, int index)
        {
            int result;
            if (BitConverter.IsLittleEndian) //현재 컴퓨터가 LittleEndian일때
            {
                result = BitConverter.ToInt32(bytes, index);
            }
            else
            {
                result = BitConverter.ToInt32(bytes, index);
                byte[] b = new byte[4];
                b = BitConverter.GetBytes(result);
                Array.Reverse(b);
                result = BitConverter.ToInt32(b, 0);
            }
            return result;
        }

        public static uint ToUInt32(byte[] bytes)
        {
            return ToUInt32(bytes, 0);
        }

        public static uint ToUInt32(byte[] bytes, int index)
        {
            uint result;
            if (BitConverter.IsLittleEndian) //현재 컴퓨터가 LittleEndian일때
            {
                result = BitConverter.ToUInt32(bytes, index);
            }
            else
            {
                result = BitConverter.ToUInt32(bytes, index);
                byte[] b = new byte[4];
                b = BitConverter.GetBytes(result);
                Array.Reverse(b);
                result = BitConverter.ToUInt32(b, 0);
            }
            return result;
        }

        public static byte[] GetBytes(UInt16 uint16)
        {
            byte[] result;
            result = BitConverter.GetBytes(uint16);
            if (!BitConverter.IsLittleEndian) { Array.Reverse(result); }
            return result;
        }

        public static byte[] GetBytes(Int32 int32)
        {
            byte[] result;
            result = BitConverter.GetBytes(int32);
            if (!BitConverter.IsLittleEndian) { Array.Reverse(result); }
            return result;
        }

        public static byte[] GetBytes(UInt32 uint32)
        {
            byte[] result;
            result = BitConverter.GetBytes(uint32);
            if (!BitConverter.IsLittleEndian) { Array.Reverse(result); }
            return result;
        }
    }
}
