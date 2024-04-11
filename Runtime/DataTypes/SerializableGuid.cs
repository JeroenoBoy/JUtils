using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace JUtils
{
    /// <summary>
    /// A lightweight guid class that can be serialized
    /// </summary>
    [Serializable]
    public struct SerializableGuid : IEquatable<SerializableGuid>
    {
        [SerializeField] private ulong _a;
        [SerializeField] private ulong _b;

        /// <summary>
        /// First half of the guid
        /// </summary>
        public ulong a => _a;

        /// <summary>
        /// Second half of the guid
        /// </summary>
        public ulong b => _b;


        /// <summary>
        /// Parse a guid from the given string
        /// </summary>
        public static SerializableGuid Parse(string guid)
        {
            Guid systemGuid = new(guid);
            byte[] byteArray = systemGuid.ToByteArray();
            ulong a = 0;
            ulong b = 0;

            for (int i = 0; i < 8; i++) {
                a += (ulong)byteArray[i] << (i * 8);
            }

            for (int i = 8; i < 16; i++) {
                b += (ulong)byteArray[i] << (i * 8);
            }

            return new SerializableGuid(a, b);
        }


        /// <summary>
        /// Parse a guid from the given string
        /// </summary>
        public static bool TryParse(string guid, out SerializableGuid output)
        {
            if (!Guid.TryParse(guid, out Guid systemGuid)) {
                output = default;
                return false;
            }

            byte[] byteArray = systemGuid.ToByteArray();
            ulong a = 0;
            ulong b = 0;

            for (int i = 0; i < 8; i++) {
                a += (ulong)byteArray[i] << (i * 8);
            }

            for (int i = 8; i < 16; i++) {
                b += (ulong)byteArray[i] << (i * 8);
            }

            output = new SerializableGuid(a, b);
            return true;
        }


        /// <summary>
        /// Generate a new random guid
        /// </summary>
        public static SerializableGuid Random()
        {
            return Parse(Guid.NewGuid().ToString());
        }


        public SerializableGuid(ulong a, ulong b)
        {
            _a = a;
            _b = b;
        }


        /// <summary>
        /// Get the byte ath the given index. Index must be between 0 and 15.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetByte(int index)
        {
            ulong i = (ulong)index / 8;
            return (byte)(((_a << (56 - index * 8)) >> 56) * (1 - i) + ((_b << (120 - index * 8)) >> 56) * i);
        }


        /// <summary>
        /// Get the 4 bits at the given index. The index must be between 0 and 31
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetHexChar(int index)
        {
            ulong i = (ulong)index / 16;
            return (byte)(((_a << (60 - index * 4)) >> 60) * (1 - i) + ((_b << (124 - index * 4)) >> 60) * i);
        }


        public bool Equals(SerializableGuid other)
        {
            return _a == other._a && _b == other._b;
        }


        public override bool Equals(object obj)
        {
            return obj is SerializableGuid other && Equals(other);
        }


        public override int GetHashCode()
        {
            return HashCode.Combine(_a, _b);
        }


        public override string ToString()
        {
            return
                $"{GetHexChar(0):X}{GetHexChar(1):X}{GetHexChar(2):X}{GetHexChar(3):X}{GetHexChar(4):X}{GetHexChar(5):X}{GetHexChar(6):X}{GetHexChar(7):X}-"
                + $"{GetHexChar(8):X}{GetHexChar(9):X}{GetHexChar(10):X}{GetHexChar(11):X}-"
                + $"{GetHexChar(12):X}{GetHexChar(13):X}{GetHexChar(14):X}{GetHexChar(15):X}-"
                + $"{GetHexChar(16):X}{GetHexChar(17):X}{GetHexChar(18):X}{GetHexChar(19):X}-"
                + $"{GetHexChar(20):X}{GetHexChar(21):X}{GetHexChar(22):X}{GetHexChar(23):X}{GetHexChar(24):X}{GetHexChar(25):X}{GetHexChar(26):X}{GetHexChar(27):X}{GetHexChar(28):X}{GetHexChar(29):X}{GetHexChar(30):X}{GetHexChar(31):X}";
        }


        public static bool operator ==(SerializableGuid a, SerializableGuid b)
        {
            return a.Equals(b);
        }


        public static bool operator !=(SerializableGuid a, SerializableGuid b)
        {
            return !a.Equals(b);
        }
    }
}