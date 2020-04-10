using System;
using System.IO;

namespace ConsoleApp1
{
    public class StreamMidiWriter
    {
        private static byte[] ENDMIDIFILE = new byte[] { 0, 255, 47, 0 };
        private MThd MidiHeaderSt;
        private MTrk MidiMtrkSt;
        private BinaryWriter MidiWriter;

        public StreamMidiWriter(string filePath, MThd MidiHeaderSt, MTrk MidiMtrkSt)
        {
            MidiWriter = new BinaryWriter(File.Open(filePath, FileMode.OpenOrCreate));
            this.MidiHeaderSt = MidiHeaderSt;
            this.MidiMtrkSt = MidiMtrkSt;
            
        }

        private byte[] ConvertToByte(String value)
        {
            byte[] ret_arr = new byte[4];
            
            ret_arr[3] = (byte)value[3];
            ret_arr[2] = (byte)value[2];
            ret_arr[1] = (byte)value[1];
            ret_arr[0] = (byte)value[0];
            
            return ret_arr;
        }
        private byte[] ConvertToByte(UInt32 first)
        {
            byte[] ret_arr = new byte[4];
            ret_arr[3] = (byte)first;
            ret_arr[2] = (byte)(first >> 8);
            ret_arr[1] = (byte)(first >> 16);
            ret_arr[0] = (byte)(first >> 24);
            return ret_arr;
        }
        private byte[] ConvertToByte(UInt16 first)
        {
            byte[] ret_arr = new byte[2];
            ret_arr[1] = (byte)first;
            ret_arr[0] = (byte)(first >> 8);
            return ret_arr;
        }
        private void MakeMThd()
        {
            MidiWriter.Write(ConvertToByte(MThd.MThdName));
            MidiWriter.Write(ConvertToByte(MThd.BlockLength));
            MidiWriter.Write(ConvertToByte(MidiHeaderSt.MidiMode));
            MidiWriter.Write(ConvertToByte(MidiHeaderSt.CountBlocks));
            MidiWriter.Write(ConvertToByte(MidiHeaderSt.TimeMode)); 
        }
        private void MakeMTrk()
        {
            MidiWriter.Write(ConvertToByte(MTrk.MTrkName));
            MidiWriter.Write(ConvertToByte(MidiMtrkSt.BlockLength));
            
            for (int i = 0; i < MidiMtrkSt.Events.Length; i++)
            {
                MidiWriter.Write(MidiMtrkSt.Events[i].Byte1);
                MidiWriter.Write(MidiMtrkSt.Events[i].Byte2);
                MidiWriter.Write(MidiMtrkSt.Events[i].Byte3);
                MidiWriter.Write(MidiMtrkSt.Events[i].Byte4);
            }
            MidiWriter.Write(ENDMIDIFILE);

            
        }
        public void MakeMidi()
        {
            MakeMThd();
            MakeMTrk();
        }
        public void Close()
        {
            MidiWriter.Close();
        }
    }
}
