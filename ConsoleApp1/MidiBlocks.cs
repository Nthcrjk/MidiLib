using System;

namespace ConsoleApp1
{
    public class MThd
    {
        public const string MThdName = "MThd";
        public const UInt32 BlockLength = 6;
        private UInt16 midiMode;
        private UInt16 countBlocks;
        private UInt16 timeMode;

        public UInt16 MidiMode
        {
            get
            {
                return midiMode;
            }
            private set
            {
                if (value == 0)
                    midiMode = value;
            }
        }
        public UInt16 CountBlocks
        {
            get
            {
                return countBlocks;
            }
            private set
            {
                if (value == 1)
                    countBlocks = value;
            }
        }
        public UInt16 TimeMode
        {
            get
            {
                return timeMode;
            }
            private set
            {
                timeMode = value;
            }
        }

        public void setMThd(UInt16 MidiMode, UInt16 CountBlocks, UInt16 TimeMode)
        {
            this.MidiMode = MidiMode;
            this.CountBlocks = CountBlocks;
            this.TimeMode = TimeMode;
        }
        public void display()
        {
            Console.WriteLine("Имя блока MThd: {0}", MThdName);
            Console.WriteLine("Длина блока MThd в байтах: {0}", BlockLength);
            Console.WriteLine("Тип миди файла: Type {0}", MidiMode);
            Console.WriteLine("Количество блоков MTrk: {0}", CountBlocks);
            Console.WriteLine("Режим времени: {0}", TimeMode);
        }
    }

    public class MTrk
    {
        abstract public class Event
        {
            protected byte byte1;
            protected byte byte2;
            protected byte byte3;
            protected byte byte4;

            public abstract byte Byte1 { get; protected set; }
            public abstract byte Byte2 { get; protected set; }
            public abstract byte Byte3 { get; protected set; }
            public abstract byte Byte4 { get; protected set; }


            public abstract void setEvent(byte byte1, byte byte2, byte byte3, byte byte4);
            public abstract void display();
        }
        private class ChannalEvent : Event
        {
            private string EventName;
            private byte Channel;

            public override byte Byte1
            {
                get
                {
                    return byte1;
                }
                protected set
                {
                    if (value >= 0 && value < 128)
                        byte1 = value;
                }
            }
            public override byte Byte2
            {
                get
                {
                    return byte2;
                }
                protected set
                {
                    if (value >= 0x80 && value <= 0xE0)
                    {
                        byte2 = value;
                        if (value >= 0x80 && value < 0x90)
                        {
                            EventName = "Note Off";
                            Channel = (byte)(value & 0b00001111);

                        }
                        if (value >= 0x90 && value < 0xA0)
                        {
                            EventName = "Note On";
                            Channel = (byte)(value & 0b00001111);
                        }
                        if (value >= 0xA0 && value < 0xB0)
                        {
                            EventName = "Polyphonic Key Pressue";
                            Channel = (byte)(value & 0b00001111);
                        }
                        if (value >= 0xB0 && value < 0xC0)
                        {
                            EventName = "Control Change";
                            Channel = (byte)(value & 0b00001111);
                        }
                        if (value >= 0xC0 && value < 0xD0)
                        {
                            EventName = "Program Change";
                            Channel = (byte)(value & 0b00001111);
                            byte2 = 200;
                        }
                        if (value >= 0xD0 && value < 0xE0)
                        {
                            EventName = "Channel Pressure";
                            Channel = (byte)(value & 0b00001111);
                            byte2 = 200;
                        }
                        if (value >= 0xE0 && value < 0xF0)
                        {
                            EventName = "Pitch Wheel Change Change";
                            Channel = (byte)(value & 0b00001111);
                        }
                    }
                }
            }
            public override byte Byte3
            {
                get
                {
                    return byte3;
                }
                protected set
                {
                    if (value >= 0 && value < 128)
                        byte3 = value;
                }
            }
            public override byte Byte4
            {
                get
                {
                    return byte4;
                }
                protected set
                {
                    if (value >= 0 && value < 128)
                        byte4 = value;
                }
            }

            public override void setEvent(byte TimeEvent, byte EventStatus, byte Param1, byte Param2)
            {
                this.Byte1 = TimeEvent;
                this.Byte2 = EventStatus;
                this.Byte3 = Param1;
                this.Byte4 = Param2;
            }
            public override void display()
            {
                Console.Write($"Время: {Byte1}, Событие: {EventName}, На канале: {Channel}, ");
                if (byte4 == 200)
                {
                    Console.WriteLine($"Первый параметр {Byte3}");
                }
                else
                {
                    Console.WriteLine($"Первый параметр {Byte3}, Второй параметр {Byte4}");
                }

            }
        }
        private class SystemEvent : Event
        {
            public override byte Byte1 { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }
            public override byte Byte2 { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }
            public override byte Byte3 { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }
            public override byte Byte4 { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }

            public override void display()
            {
                throw new NotImplementedException();
            }

            public override void setEvent(byte byte1, byte byte2, byte byte3, byte byte4)
            {
                throw new NotImplementedException();
            }
        }

        public const string MTrkName = "MTrk"; // Mtrk
        private UInt32 blockLength; // Длина блока в байтах в двух
        private UInt32 eventCount;

        public Event[] Events;
        public UInt32 BlockLength
        {
            get
            {
                return blockLength;
            }
            private set
            {
                blockLength = value;
                eventCount = (value - 4) / 4;
                Events = new Event[eventCount];
            }
        }
        public UInt32 EventCount
        {
            get
            {
                return eventCount;
            }
            private set
            {
                eventCount = value;
                blockLength = value * 4 + 4; // 4 Число байт в событии
                Events = new Event[EventCount];
            }
        }

        public void setMTrk(UInt32 EventCount, byte[,] Arr)
        {
            this.EventCount = EventCount;
            for (int i = 0; i < EventCount; i++)
            {
                ChannalEvent k = new ChannalEvent();
                k.setEvent(Arr[i, 0], Arr[i, 1], Arr[i, 2], Arr[i, 3]); 
                Events[i] = k;
            }
        }
        public void display()
        {
            Console.WriteLine($"{MTrkName}\nДлина {blockLength}");
            for (int i = 0; i < Events.Length; i++)
            {
                Events[i].display();
            }
        }
    }


}
