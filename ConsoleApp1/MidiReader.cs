using System;
using System.Text;
using System.IO;

namespace ConsoleApp1
{
    public class StreamMidiReader
    {
        
        private MThd MidiHeaderSt;
        private MTrk MidiMtrkSt;

        private BinaryReader MidiReader;

        public StreamMidiReader(string filePath)
        {
            MidiReader = new BinaryReader(File.Open(filePath, FileMode.Open));
            MidiHeaderSt = new MThd();
            MidiMtrkSt = new MTrk();

            MidiReader.ReadBytes(8);
            MidiHeaderSt.setMThd(ReadUInt16(), ReadUInt16(), ReadUInt16());

            MidiReader.ReadBytes(4);
            UInt32 blockLength = ReadUInt32();

            byte[,] arr = new byte[(blockLength - 4) / 4, 4];

            for (int i = 0; i < (blockLength - 4) / 4; i++)
                for (int k = 0; k < 4; k++)
                    arr[i, k] = MidiReader.ReadByte();

            MidiMtrkSt.setMTrk((blockLength - 4) / 4, arr);
        }

        private string ReadMidiBlockName()
        {
            return Encoding.Default.GetString(MidiReader.ReadBytes(4));
        }
        private UInt32 ReadUInt32()
        {
            UInt32 ret_num = 0;
            for(int i = 4; i > 0; i--)
            {
                ret_num = ((UInt32)MidiReader.ReadByte()) * ((UInt32)Math.Pow(10, i - 1));
            }
            return ret_num;
        }
        private UInt16 ReadUInt16()
        {
            UInt16 ret_num = 0;
            for (int i = 2; i > 0; i--)
            {
                ret_num = (UInt16)((UInt16)MidiReader.ReadByte() * Math.Pow(10, i - 1));
            }
            return ret_num;
        }
        public MThd getMThd()
        {
            return MidiHeaderSt;
        }
        public MTrk getMTrk()
        {
            return MidiMtrkSt;
        }
        public void display()
        {
            MidiHeaderSt.display();
            MidiMtrkSt.display();
        }
        public void Close()
        {
            MidiReader.Close();
        }
    } 
}
