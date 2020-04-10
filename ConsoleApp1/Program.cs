using System;
using System.IO;                                            // Работа с файлами.


namespace ConsoleApp1
{
    class Program
    {
        static string file_path = "3ss.mid";
        static byte[] Read()
        {
            BinaryReader MidiReader = new BinaryReader(File.Open("3sss.mid", FileMode.Open));

            byte[] k = MidiReader.ReadBytes(500);
            foreach (int i in k)
            {
                if ((i & 0b10000000) != 0 && (i & 0b01110000) == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(i);
                    Console.ResetColor();
                } 
                else if ((i & 0b10000000) != 0 && (i & 0b00010000) != 0 && (i & 0b01100000) == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(i);
                    Console.ResetColor();
                } 
                else if ((i & 0b10000000) != 0 && (i & 0b00100000) != 0 && (i & 0b01010000) == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(i);
                    Console.ResetColor();
                } 
                else if ((i & 0b10000000) != 0 && (i & 0b00100000) != 0 && (i & 0b00010000) != 0 && (i & 0b01000000) == 0) //1011 NNNN
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(i);
                    Console.ResetColor();
                } 
                else if ((i & 0b10000000) != 0 && (i & 0b01000000) != 0 && (i & 0b00110000) == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine(i);
                    Console.ResetColor();
                } 
                else if ((i & 0b10000000) != 0 && (i & 0b01000000) != 0 && (i & 0b00010000) != 0 && (i & 0b00100000) == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(i);
                    Console.ResetColor();
                } 
                else if ((i & 0b10000000) != 0 && (i & 0b01000000) != 0 && (i & 0b00100000) != 0 && (i & 0b00010000) == 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine(i);
                    Console.ResetColor();
                } 
                else
                {
                    Console.WriteLine(i);
                }
                
                    
                /*if ((0b10010000 & i) != 0 && (0b01100000 & i) == 0)
                {
                    
                } else if((0b10000000 & i) != 0 && (0b01110000 & i) == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(i);
                    Console.ResetColor();
                } else
                {
                    Console.WriteLine(i);
                }*/
            }
            MidiReader.Close();
            return k;
        }
        static void Check()
        {
            for (int i = 0; i < 256; i++)
            {
                if ((i & 0b10000000) != 0 && (i & 0b01000000) != 0 && (i & 0b00010000) != 0 && (i & 0b00100000) == 0)
                    Console.WriteLine(i);
            }
        }
        static void Write_point()
        {
            byte k = 20;
            BinaryWriter Midi = new BinaryWriter(File.Open("3ss.mid", FileMode.OpenOrCreate));
            Midi.Seek(21, SeekOrigin.Begin);
            Midi.Write(k);
            Midi.Close();
        }
        static void MakeMidi(byte[] k)
        {
            
            BinaryWriter Midi = new BinaryWriter(File.Open("3ss.mid", FileMode.OpenOrCreate));
            
            for (int i = 0; i < k.Length; i++)
            {
                Midi.Write(k[i]);

            }
            Midi.Close();
        }

        static void MakeEvent()
        {
            BinaryWriter Midi = new BinaryWriter(File.Open("3ss.mid", FileMode.OpenOrCreate));
            Midi.Seek(22, SeekOrigin.Begin);
            byte[] p = new byte[] { 0, 0x92, 0x3C, 0x60,
                                    60, 0x82, 0x3C, 0x0,
                                    0, 0x91, 0x70, 0x60,
                                    60, 0x81, 0x70, 0x0,
                                    0, 255, 47, 0 };
            foreach (byte i in p)
            {
                Midi.Write(i);
            }
            Midi.Close();
        }

        static void Main(string[] args)
        {
            StreamMidiReader ReadMidi = new StreamMidiReader("..\\..\\..\\ReadMidi.mid");

            byte[,] k = new byte[,] { { 0, 0x90, 60, 120 }, { 60, 0x80, 60, 0 }, { 0, 0x90, 100, 120 }, { 60, 0x80, 100, 0 } };

            MThd testMThd = new MThd();
            MTrk testMTrk = new MTrk();

            testMThd = ReadMidi.getMThd();
            testMTrk.setMTrk(4, k);

            ReadMidi.Close();

            StreamMidiWriter WriteMidi = new StreamMidiWriter("..\\..\\..\\WriteMidi.mid", testMThd, testMTrk);

            WriteMidi.MakeMidi();
            WriteMidi.Close();

            StreamMidiReader test = new StreamMidiReader("..\\..\\..\\WriteMidi.mid");
            test.display();
            test.Close();
            
            Console.ReadLine();
        }
    }
}
