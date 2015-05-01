using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Scada.Comm.KP
{
    public static class FileFormats
    {
        public interface IConvertibleToCvs
        {
            //static string ConvertHeaderToCsv();
            string ConvertRecordToCvs();
        }

        [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 80)]
        public struct F01Record : IConvertibleToCvs
        {
            public const int RecSize = 80;

            [FieldOffset(0)]
            public long Time;
            [FieldOffset(8)]
            public double RollGyroRate;
            [FieldOffset(16)]
            public double PitchGyroRate;
            [FieldOffset(24)]
            public double YawGyroRate;
            [FieldOffset(32)]
            public double AccelXAxis;
            [FieldOffset(40)]
            public double AccelYAxis;
            [FieldOffset(48)]
            public double AccelZAxis;
            [FieldOffset(56)]
            public double MagRawValueXAxis;
            [FieldOffset(64)]
            public double MagRawValueYAxis;
            [FieldOffset(72)]
            public double MagRawValueZAxis;

            //[FieldOffset(0)]
            //[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 80)]
            //public byte[] Bytes;
            //[FieldOffset(8)]
            //[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R8, SizeConst = 9)]
            //public double[] Doubles;

            public void SetFieldByIndex(int index, double value)
            {
                switch (index)
                {
                    case 0:
                        RollGyroRate = value;
                        break;
                    case 1:
                        PitchGyroRate = value;
                        break;
                    case 2:
                        YawGyroRate = value;
                        break;
                    case 3:
                        AccelXAxis = value;
                        break;
                    case 4:
                        AccelYAxis = value;
                        break;
                    case 5:
                        AccelZAxis = value;
                        break;
                    case 6:
                        MagRawValueXAxis = value;
                        break;
                    case 7:
                        MagRawValueYAxis = value;
                        break;
                    case 8:
                        MagRawValueZAxis = value;
                        break;
                }
            }

            public byte[] GetBytes()
            {
                byte[] bytes = new byte[RecSize];
                BitConverter.GetBytes(Time).CopyTo(bytes, 0);
                BitConverter.GetBytes(RollGyroRate).CopyTo(bytes, 8);
                BitConverter.GetBytes(PitchGyroRate).CopyTo(bytes, 16);
                BitConverter.GetBytes(YawGyroRate).CopyTo(bytes, 24);
                BitConverter.GetBytes(AccelXAxis).CopyTo(bytes, 32);
                BitConverter.GetBytes(AccelYAxis).CopyTo(bytes, 40);
                BitConverter.GetBytes(AccelZAxis).CopyTo(bytes, 48);
                BitConverter.GetBytes(MagRawValueXAxis).CopyTo(bytes, 56);
                BitConverter.GetBytes(MagRawValueYAxis).CopyTo(bytes, 64);
                BitConverter.GetBytes(MagRawValueZAxis).CopyTo(bytes, 72);
                return bytes;
            }

            public static F01Record FromBytes(byte[] buffer)
            {
                return new F01Record();
            }

            //public static string ConvertHeaderToCsv()
            //{
            //    return "Time;RollGyroRate;PitchGyroRate;YawGyroRate;AccelXAxis;AccelYAxis;AccelZAxis;MagRawValueXAxis;MagRawValueYAxis;MagRawValueZAxis";
            //}

            public string ConvertRecordToCvs()
            {
                return new StringBuilder()
                    .Append(DateTime.FromBinary(Time).ToLongTimeString()).Append(";")
                    .Append(RollGyroRate).Append(";")
                    .Append(PitchGyroRate).Append(";")
                    .Append(YawGyroRate).Append(";")
                    .Append(AccelXAxis).Append(";")
                    .Append(AccelYAxis).Append(";")
                    .Append(AccelZAxis).Append(";")
                    .Append(MagRawValueXAxis).Append(";")
                    .Append(MagRawValueYAxis).Append(";")
                    .Append(MagRawValueZAxis).Append(";")
                    .ToString();
            }
        }


        public static void ConvertF0ToCsv(string srcFileName, string destFileName)
        {
            using (FileStream inStream = File.OpenRead(srcFileName))
            {
                using (StreamWriter outWriter = File.CreateText(destFileName))
                {
                    // вывод заголовка
                    //outWriter.WriteLine(F01Record.ConvertHeaderToCsv());

                    // вывод записей
                    byte[] buf = new byte[F01Record.RecSize];
                    while (inStream.Position < inStream.Length)
                    {
                        inStream.Read(buf, 0, F01Record.RecSize);
                        F01Record rec = F01Record.FromBytes(buf);
                        outWriter.WriteLine(rec.ConvertRecordToCvs());
                    }
                }
            }
        }
    }
}
