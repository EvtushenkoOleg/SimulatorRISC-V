using System;
using System.Collections.Generic;

namespace Simulator_RISCV 
{
    class Memory
    {
        public string Begin_stack_mem { get; } = "00007FF0";
        public string Begin_data_mem { get; } = "00001000";
        public string Code_pointer { get; } = "00000000";
        public static Dictionary<string, string[]> Registers
        {
            get; set;
        }
        public Memory()
        {
            Registers = new Dictionary<string, string[]>
            {
                { "x0", new string[] {"zero", "0x00000000"} },
                { "x1", new string[] { "ra", "0x00000000" } },
                { "x2", new string[] { "sp", "0x00007FF0" } },
                { "x3", new string[] { "gp", "0x00001000" } },
                { "x4", new string[] { "tp", "0x00000000" } },
                { "x5", new string[] { "t0", "0x00000000" } },
                { "x6", new string[] { "t1", "0x00000000" } },
                { "x7", new string[] { "t2", "0x00000000" } },
                { "x8", new string[] { "s0", "0x00000000" } },
                { "x9", new string[] { "s1", "0x00000000" } },
                { "x10", new string[] { "a0", "0x00000000" } },
                { "x11", new string[] { "a1", "0x00000000" } },
                { "x12", new string[] { "a2", "0x00000000" } },
                { "x13", new string[] { "a3", "0x00000000" } },
                { "x14", new string[] { "a4", "0x00000000" } },
                { "x15", new string[] { "a5", "0x00000000" } },
                { "x16", new string[] { "a6", "0x00000000" } },
                { "x17", new string[] { "a7", "0x00000000" } },
                { "x18", new string[] { "s2", "0x00000000" } },
                { "x19", new string[] { "s3", "0x00000000" } },
                { "x20", new string[] { "s4", "0x00000000" } },
                { "x21", new string[] { "s5", "0x00000000" } },
                { "x22", new string[] { "s6", "0x00000000" } },
                { "x23", new string[] { "s7", "0x00000000" } },
                { "x24", new string[] { "s8", "0x00000000" } },
                { "x25", new string[] { "s9", "0x00000000" } },
                { "x26", new string[] { "s10", "0x00000000" } },
                { "x27", new string[] { "s11", "0x00000000" } },
                { "x28", new string[] { "t3", "0x00000000" } },
                { "x29", new string[] { "t4", "0x00000000" } },
                { "x30", new string[] { "t5", "0x00000000" } },
                { "x31", new string[] { "t6", "0x00000000" } }
            };
        }
		
		public void Reg_init()
        {
            int i = 0;
            while (i < 31)
            {
                if (i == 2)
                    Registers["x2"][1] = "0x00007FF0";
                else
                if(i == 3)
                    Registers["x3"][1] = "0x00001000";
                else
					Registers["x" + i][1] = "0x00000000";
                i++;
            }
        }
		
        public string Read_data_byte(string address)
        {
            string row = address.Substring(2, 7) + "0";
            int offset = Convert.ToInt32(address, 16) % 16;
            string[] buf = Alg_operation.Data_seg[row].Split(' ');
            return buf[offset];
        }

        //funct load word to memory
        public string Read_data_hw(string address)
        {
            string row = address.Substring(2, 7) + "0";
            int offset = Convert.ToInt32(address, 16) % 16;
            string[] buf = Alg_operation.Data_seg[row].Split(' ');
            if (offset != 15)
                return buf[offset + 1] + buf[offset];
            else
                return Alg_operation.Data_seg[(Convert.ToInt32(row,16) + 16).ToString("X").PadLeft(8, '0')].Split(' ')[0] + buf[offset];
        }   

        //funct load word to memory
        public string Read_data_word(string address)
        {
            string row = address.Substring(2, 7) + "0";
            int offset = Convert.ToInt32(address, 16) % 16;
            string[] buf = Alg_operation.Data_seg[row.ToLower()].Split(' ');

            switch (offset)
            {
                case 13:
                    return Alg_operation.Data_seg[(Convert.ToInt32(row, 16) + 16).ToString("X").PadLeft(8, '0')].Split(' ')[0] + buf[offset + 2] + buf[offset + 1] + buf[offset];
                case 14:
                    return Alg_operation.Data_seg[(Convert.ToInt32(row, 16) + 16).ToString("X").PadLeft(8, '0')].Split(' ')[1] + Alg_operation.Data_seg[(Convert.ToInt32(row, 16) + 16).ToString("X").PadLeft(8, '0')].Split(' ')[0] + buf[offset + 1] + buf[offset];
                case 15:
                    return Alg_operation.Data_seg[(Convert.ToInt32(row, 16) + 16).ToString("X").PadLeft(8, '0')].Split(' ')[2] + Alg_operation.Data_seg[(Convert.ToInt32(row, 16) + 16).ToString("X").PadLeft(8, '0')].Split(' ')[1] + Alg_operation.Data_seg[(Convert.ToInt32(row, 16) + 16).ToString("X").PadLeft(8, '0')].Split(' ')[0] + buf[offset];
                default:
                    return buf[offset + 3] + buf[offset + 2] + buf[offset + 1] + buf[offset];
                    
            }
        }
        
        //funct store to memory
        public void Write_data(string address, string data)
        {
            string row = address.Substring(0, 7) + "0";
            int offset = Convert.ToInt32(address, 16) % 16;
            string[] buf = Alg_operation.Data_seg[row].Split(' ');
                switch (data.Length)
                {
                    case 2:
                        {
                            buf[offset] = data;
                            Alg_operation.Data_seg[row] = String.Join(" ", buf);
                            break;
                        }
                    case 4:
                        {
                            if (offset != 15)
                            {
                                buf[offset + 1] = data.Substring(2, 2);
                                buf[offset] = data.Substring(0, 2);
                                Alg_operation.Data_seg[row] = String.Join(" ", buf);
                            }
                            else
                            {
                                buf[offset] = data.Substring(2, 2);
                                Alg_operation.Data_seg[row] = String.Join(" ", buf);
                                string[] buf2 = Alg_operation.Data_seg[(Convert.ToInt32(row, 16) + 16).ToString("X").PadLeft(8, '0')].Split(' ');
                                buf2[0] = data.Substring(0, 2);
                                Alg_operation.Data_seg[(Convert.ToInt32(row, 16) + 16).ToString("X").PadLeft(8, '0')] = String.Join(" ", buf2);
                            }
                            break;
                        }
                    case 8:
                        {
                            switch (offset)
                            {
                                case 13:
                                    {
                                    buf[offset + 2] = data.Substring(2, 2);
                                    buf[offset + 1] = data.Substring(4, 2);
                                    buf[offset] = data.Substring(6, 2);
                                    Alg_operation.Data_seg[row] = String.Join(" ", buf);
                                    string[] buf2 = Alg_operation.Data_seg[(Convert.ToInt32(row, 16) + 16).ToString("X").PadLeft(8, '0')].Split(' ');
                                    buf2[0] = data.Substring(0, 2);
                                    Alg_operation.Data_seg[(Convert.ToInt32(row, 16) + 16).ToString("X").PadLeft(8, '0')] = String.Join(" ", buf2);
                                    break;
                                    }
                                case 14:
                                    {
                                        buf[offset + 1] = data.Substring(4, 2);
                                        buf[offset] = data.Substring(6, 2);
                                        Alg_operation.Data_seg[row] = String.Join(" ", buf);
                                        string[] buf2 = Alg_operation.Data_seg[(Convert.ToInt32(row, 16) + 16).ToString("X").PadLeft(8, '0')].Split(' ');
                                        buf2[0] = data.Substring(2, 2);
                                        buf2[1] = data.Substring(0, 2);
                                        Alg_operation.Data_seg[(Convert.ToInt32(row, 16) + 16).ToString("X").PadLeft(8, '0')] = String.Join(" ", buf2);
                                        break;
                                    }
                                case 15:
                                    {
                                        buf[offset] = data.Substring(6, 2);
                                         Alg_operation.Data_seg[row] = String.Join(" ", buf);
                                        string[] buf2 = Alg_operation.Data_seg[(Convert.ToInt32(row, 16) + 16).ToString("X").PadLeft(8, '0')].Split(' ');
                                        buf2[0] = data.Substring(4, 2);
                                        buf2[1] = data.Substring(2, 2);
                                        buf2[2] = data.Substring(0, 2);
                                        Alg_operation.Data_seg[(Convert.ToInt32(row, 16) + 16).ToString("X").PadLeft(8, '0')] = String.Join(" ", buf2);
                                        break;
                                    }
                                default:
                                    {
                                        buf[offset + 3] = data.Substring(0, 2);
                                        buf[offset + 2] = data.Substring(2, 2);
                                        buf[offset + 1] = data.Substring(4, 2);
                                        buf[offset] = data.Substring(6, 2);
                                        Alg_operation.Data_seg[row] = String.Join(" ", buf);
                                        break;
                                    }
                            }
                            break;
                        }
                }
        }

	}
}
