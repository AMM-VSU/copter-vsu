using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Scada.Comm.KP
{
    /// <summary>
    /// Форматы файлов, используемые в исследованиях
    /// </summary>
    public static class FileFormats
    {
        /// <summary>
        /// Интерфейс файлов, состоящих из записей
        /// </summary>
        public interface IFileOfRecords
        {
            object CreateRecord(byte[] buffer);
        }

        /// <summary>
        /// Интерфейс для поддержки преобразования файла в формат CSV
        /// </summary>
        public interface IConvertibleToCvs
        {
            string GetCsvHeader();
            string ConvertRecordToCvs(object record);
        }

        /// <summary>
        /// Формат файлов телеметрии, состоящих из записей типа F01Record
        /// </summary>
        public class F01FileFormat : IFileOfRecords, IConvertibleToCvs
        {
            public object CreateRecord(byte[] buffer)
            {
                return F01Record.FromBytes(buffer);
            }

            public string GetCsvHeader()
            {
                return "Time;RollGyroRate;PitchGyroRate;YawGyroRate;AccelXAxis;AccelYAxis;AccelZAxis;MagRawValueXAxis;MagRawValueYAxis;MagRawValueZAxis";
            }

            public string ConvertRecordToCvs(object record)
            {
                F01Record rec = (F01Record)record;
                return rec.ConvertRecordToCvs();
            }
        }

        /// <summary>
        /// Запись телеметрии
        /// </summary>
        [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 80)]
        public struct F01Record
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
                F01Record rec = new F01Record();
                rec.Time = BitConverter.ToInt64(buffer, 0);
                rec.RollGyroRate = BitConverter.ToDouble(buffer, 8);
                rec.PitchGyroRate = BitConverter.ToDouble(buffer, 16);
                rec.YawGyroRate = BitConverter.ToDouble(buffer, 24);
                rec.AccelXAxis = BitConverter.ToDouble(buffer, 32);
                rec.AccelYAxis = BitConverter.ToDouble(buffer, 40);
                rec.AccelZAxis = BitConverter.ToDouble(buffer, 48);
                rec.MagRawValueXAxis = BitConverter.ToDouble(buffer, 56);
                rec.MagRawValueYAxis = BitConverter.ToDouble(buffer, 64);
                rec.MagRawValueZAxis = BitConverter.ToDouble(buffer, 72);
                return rec;
            }

            public string ConvertRecordToCvs()
            {
                return new StringBuilder()
                    .Append(DateTime.FromBinary(Time).ToString("HH:mm:ss:fff")).Append(";")
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


        /// <summary>
        /// Выбрать формат файла в зависимости от расширения
        /// </summary>
        private static object ChooseFileFormat(string srcFileName)
        {
            string ext = Path.GetExtension(srcFileName).ToLower();
            if (ext == ".f01")
                return new F01FileFormat();
            else
                return null;
        }

        /// <summary>
        /// Преобразовать файл в формат CSV
        /// </summary>
        public static void ConvertFileToCsv(string srcFileName, string destFileName)
        {
            // проверка аргументов метода
            if (srcFileName == null)
                throw new ArgumentNullException("srcFileNamve");
            if (destFileName == null)
                throw new ArgumentNullException("destFileName");

            // определение и проверка формата файла
            object fileFormat = ChooseFileFormat(srcFileName);
            if (fileFormat == null)
                throw new Exception("Неизвестный формат файла.");

            IFileOfRecords iftFileOfRecords = fileFormat as IFileOfRecords;
            if (iftFileOfRecords == null)
                throw new Exception("Файл должен состоять из записей.");

            IConvertibleToCvs itfConvertibleToCvs = fileFormat as IConvertibleToCvs;
            if (itfConvertibleToCvs == null)
                throw new NotSupportedException("Формат файла не поддерживает преобразование в CSV.");

            // преобразование
            using (FileStream inStream = File.OpenRead(srcFileName))
            {
                using (StreamWriter outWriter = File.CreateText(destFileName))
                {
                    // вывод заголовка
                    outWriter.WriteLine(itfConvertibleToCvs.GetCsvHeader());

                    // вывод записей
                    byte[] buf = new byte[F01Record.RecSize];
                    while (inStream.Position < inStream.Length)
                    {
                        inStream.Read(buf, 0, F01Record.RecSize);
                        object rec = iftFileOfRecords.CreateRecord(buf);
                        outWriter.WriteLine(itfConvertibleToCvs.ConvertRecordToCvs(rec));
                    }
                }
            }
        }
    }
}
