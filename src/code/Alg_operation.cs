using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Simulator_RISCV
{
    class Alg_operation : INotifyPropertyChanged
    {
        public bool Stage;
        string pc;
        string Name_op { get; set; } = "";
        string Op1 { get; set; } = "";
        string Number1 { get; set; } = "";
        string Op2 { get; set; } = "";
        string Number2 { get; set; } = "";
        string Op3 { get; set; } = "";
        int a;
        int b;

        public string PC
        {
            get { return pc; }
            set
            {
                pc = value;
                RaisePropertyChanged("PC");
            }
        }
        string file_name;
        public string File_name
        {
            get { return file_name; }
            set
            {
                file_name = value;
                RaisePropertyChanged("File_name");
            }
        }
        string console;
        public string Console
        {
            get { return console; }
            set
            {
                console = value;
                RaisePropertyChanged("Console");
            }
        }
        int clk;
        public int CLK
        {
            get { return clk; }
            set
            {
                clk = value;
                RaisePropertyChanged("CLK");
            }
        }

        public static Dictionary<string, string> Data_seg
        {
            get; set;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Alg_operation()
        {
            PC = "00000000";
            Stage = true;
            File_name = "";
        }

        public string Execute(string inst)
        {
            uint inp1;
            uint inp2;
            try
            {
                Name_op = inst.Split(' ')[0].ToLower();
                Op1 = inst.Split(' ')[1];
                Op2 = inst.Split(' ')[2];
                Op3 = inst.Split(' ')[3];
            }

            catch (Exception)
            {
                Op3 = "";
            }
            switch (Name_op)
            {
                case ("lui"):
                    Number1 = (Op2 + "000").PadLeft(8, '0');
                    PC = Convert.ToString(Convert.ToUInt32(PC, 16) + 4, 16).PadLeft(8, '0');
                    if (Op1 != "x0")
                        return "0x" + Number1.ToUpper() + " " + Op1;
                    return "";

                case ("auipc"):
                    Number1 = Convert.ToString(Convert.ToUInt32(PC, 16) + Convert.ToInt32(Op2, 16), 16).PadLeft(8, '0');
                    PC = Convert.ToString((Convert.ToUInt32(PC, 16) + 4), 16).PadLeft(8, '0');
                    if (Op1 != "x0")
                        return "0x" + Number1.ToUpper() + " " + Op1;
                    return "";

                case ("jal"):
                    Number1 = (Convert.ToUInt32(Op2, 16) + Convert.ToUInt32(PC, 16)).ToString("X");
                    Number2 = Convert.ToString(Convert.ToUInt32(PC, 16) + 4, 16).PadLeft(8, '0').ToUpper();
                    PC = Number1.PadLeft(8, '0');
                    if (Op1 != "x0")
                        return "0x" + Number2 + " " + Op1;
                    return "";

                case ("jalr"):
                    Number1 = (Convert.ToUInt32(Memory.Registers[Op2][1], 16) + Convert.ToUInt32(Op3, 16)).ToString("X");
                    Number2 = "0x" + Convert.ToString(Convert.ToUInt32(PC, 16) + 4, 16).PadLeft(8, '0').ToUpper();
                    PC = Number1.PadLeft(8, '0');
                    if (Op1 != "x0")
                        return Number2 + " " + Op1;
                    return "";

                case ("beq"):
                    Number1 = Memory.Registers[Op1][1].Substring(2, 8);
                    Number2 = Memory.Registers[Op2][1].Substring(2, 8);
                    if (Number1 == Number2)
                        PC = (Convert.ToUInt32(PC, 16) + Convert.ToUInt32(Op3, 16)).ToString("X").PadLeft(8, '0');
                    else
                        PC = Convert.ToString(Convert.ToUInt32(PC, 16) + 4, 16).PadLeft(8, '0');
                    return "";

                case ("bne"):
                    Number1 = Memory.Registers[Op1][1].Substring(2, 8);
                    Number2 = Memory.Registers[Op2][1].Substring(2, 8);
                    if (Number1 != Number2)
                        PC = (Convert.ToUInt32(PC, 16) + Convert.ToUInt32(Op3, 16)).ToString("X").PadLeft(8, '0');
                    else
                        PC = Convert.ToString(Convert.ToUInt32(PC, 16) + 4, 16).PadLeft(8, '0');
                    return "";

                case ("blt"):
                    Number1 = Memory.Registers[Op1][1].Substring(2, 8);
                    Number2 = Memory.Registers[Op2][1].Substring(2, 8);
                    if (Op3[0] > '8')
                        Op3 = Op3.PadLeft(8, 'F');
                    if (String.Compare(Number1, Number2) < 0)
                        PC = (Convert.ToUInt32(PC, 16) + Convert.ToUInt32(Op3, 16)).ToString("X").PadLeft(8, '0');
                    else
                        PC = Convert.ToString(Convert.ToUInt32(PC, 16) + 4, 16).PadLeft(8, '0');
                    return "";

                case ("bge"):
                    Number1 = Memory.Registers[Op1][1].Substring(2, 8);
                    Number2 = Memory.Registers[Op2][1].Substring(2, 8);
                    if (String.Compare(Number1, Number2) > 0 || String.Compare(Number1, Number2) == 0)
                        PC = (Convert.ToUInt32(PC, 16) + Convert.ToUInt32(Op3, 16)).ToString("X").PadLeft(8, '0');
                    else
                        PC = Convert.ToString(Convert.ToUInt32(PC, 16) + 4, 16).PadLeft(8, '0');
                    return "";

                case ("bltu"):
                    Number1 = Memory.Registers[Op1][1].Substring(2, 8);
                    Number2 = Memory.Registers[Op2][1].Substring(2, 8);
                    if (Number1[0] >= '8')
                    {
                        inp1 = Convert.ToUInt32(Number1, 16);
                        inp1 = ~inp1 + 1;
                    }
                    else
                    {
                        inp1 = Convert.ToUInt32(Number1, 16);
                    }

                    if (Number2[0] >= '8')
                    {
                        inp2 = Convert.ToUInt32(Number2, 16);
                        inp2 = ~inp2 + 1;
                    }
                    else
                    {
                        inp2 = Convert.ToUInt32(Number2, 16);
                    }
                    if (inp1 < inp2)
                    {
                        PC = (Convert.ToUInt32(PC, 16) + Convert.ToUInt32(Op3, 16)).ToString("X").PadLeft(8, '0');
                    }
                    else
                        PC = Convert.ToString(Convert.ToUInt32(PC, 16) + 4, 16).PadLeft(8, '0');
                    return "";

                case ("bgeu"):
                    Number1 = Memory.Registers[Op1][1].Substring(2, 8);
                    Number2 = Memory.Registers[Op2][1].Substring(2, 8);
                    if (Number1[0] >= '8')
                    {
                        inp1 = Convert.ToUInt32(Number1, 16);
                        inp1 = ~inp1 + 1;
                    }
                    else
                    {
                        inp1 = Convert.ToUInt32(Number1, 16);
                    }

                    if (Number2[0] >= '8')
                    {
                        inp2 = Convert.ToUInt32(Number2, 16);
                        inp2 = ~inp2 + 1;
                    }
                    else
                    {
                        inp2 = Convert.ToUInt32(Number2, 16);
                    }
                    if (inp1 >= inp2)
                    {
                        PC = (Convert.ToUInt32(PC, 16) + Convert.ToUInt32(Op3, 16)).ToString("X").PadLeft(8, '0');
                    }
                    else
                        PC = Convert.ToString(Convert.ToUInt32(PC, 16) + 4, 16).PadLeft(8, '0');
                    return "";

                case ("lb"):
                    Number2 = Memory.Registers[Op2][1].Substring(2, 8);//rs1
                    if (Op3[0] >= '8')
                        Op3 = Op3.PadLeft(8, 'F');
                    Number2 = Convert.ToString(Convert.ToUInt32(Number2, 16) + Convert.ToUInt32(Op3, 16), 16).PadLeft(8, '0');
                    PC = Convert.ToString(Convert.ToUInt32(PC, 16) + 4, 16).PadLeft(8, '0');
                    return "0x" + Number2.ToUpper() + " " + Op1;

                case ("lh"):
                    Number2 = Memory.Registers[Op2][1].Substring(2, 8);
                    if (Op3[0] >= '8')
                        Op3 = Op3.PadLeft(8, 'F');
                    Number2 = Convert.ToString(Convert.ToUInt32(Number2, 16) + Convert.ToUInt32(Op3, 16), 16).PadLeft(8, '0');
                    PC = Convert.ToString(Convert.ToUInt32(PC, 16) + 4, 16).PadLeft(8, '0');
                    return "0x" + Number2.ToUpper() + " " + Op1;

                case ("lw"):
                    Number2 = Memory.Registers[Op2][1].Substring(2, 8);//rs1
                    if (Op3[0] >= '8')
                        Op3 = Op3.PadLeft(8, 'f');
                    Number2 = Convert.ToString(Convert.ToUInt32(Number2, 16) + Convert.ToUInt32(Op3, 16), 16).PadLeft(8, '0');
                    PC = Convert.ToString(Convert.ToUInt32(PC, 16) + 4, 16).PadLeft(8, '0');
                    return "0x" + Number2.ToUpper() + " " + Op1;

                case ("lbu"):
                    Number2 = Memory.Registers[Op2][1].Substring(2, 8);//rs1
                    if (Op3[0] >= '8')
                        Op3 = Op3.PadLeft(8, 'F');
                    Number2 = Convert.ToString(Convert.ToUInt32(Number2, 16) + Convert.ToUInt32(Op3, 16), 16).PadLeft(8, '0');
                    PC = Convert.ToString(Convert.ToUInt32(PC, 16) + 4, 16).PadLeft(8, '0');
                    return "0x" + Number2.PadLeft(8, '0').ToUpper() + " " + Op1;

                case ("lhu"):
                    Number2 = Memory.Registers[Op2][1].Substring(2, 8);
                    if (Op3[0] >= '8')
                        Op3 = Op3.PadLeft(8, 'F');
                    Number2 = Convert.ToString(Convert.ToUInt32(Number2, 16) + Convert.ToUInt32(Op3, 16), 16).PadLeft(8, '0');
                    PC = Convert.ToString(Convert.ToUInt32(PC, 16) + 4, 16).PadLeft(8, '0');
                    return "0x" + Number2.ToUpper() + " " + Op1;

                case ("sb"):
                    Number2 = Memory.Registers[Op2][1].Substring(2, 8);
                    Number2 = Convert.ToString(Convert.ToUInt32(Number2, 16) + Convert.ToUInt32(Op3, 16), 16).PadLeft(8, '0');
                    Number1 = Memory.Registers[Op1][1].Substring(8, 2);
                    PC = Convert.ToString(Convert.ToUInt32(PC, 16) + 4, 16).PadLeft(8, '0');
                    return Number2 + " " + Number1;

                case ("sh"):
                    Number2 = Memory.Registers[Op2][1].Substring(2, 8);
                    Number2 = Convert.ToString(Convert.ToUInt32(Number2, 16) + Convert.ToUInt32(Op3, 16), 16).PadLeft(8, '0');
                    Number1 = Memory.Registers[Op1][1].Substring(6, 4);
                    PC = Convert.ToString(Convert.ToUInt32(PC, 16) + 4, 16).PadLeft(8, '0');
                    return Number2 + " " + Number1;

                case ("sw"):
                    Number2 = Memory.Registers[Op2][1].Substring(2, 8);
                    Number2 = Convert.ToString(Convert.ToUInt32(Number2, 16) + Convert.ToUInt32(Op3, 16), 16).PadLeft(8, '0');
                    Number1 = Memory.Registers[Op1][1].Substring(2, 8);
                    PC = Convert.ToString(Convert.ToUInt32(PC, 16) + 4, 16).PadLeft(8, '0');
                    return Number2 + " " + Number1;

                case ("addi"):
                    Op2 = Memory.Registers[Op2][1].Substring(2, 8);
                    if (Op3[0] >= '8')
                        Op3 = Op3.PadLeft(8, 'F');
                    try
                    {
                        a = Convert.ToInt32(Op2, 16);
                        b = Convert.ToInt32(Op3, 16);
                        Number1 = Convert.ToString(a + b, 16).PadLeft(8, '0');
                        PC = Convert.ToString(Convert.ToUInt32(PC, 16) + 4, 16).PadLeft(8, '0');
                        return "0x" + Number1.ToUpper() + " " + Op1;
                    }
                    catch
                    {
                        PC = "00000000";
                        return "Возникло переполнение при выполнении оперции" + Name_op;
                    }

                case ("slti"):
                    Op2 = Memory.Registers[Op2][1].Substring(2, 8);
                    if (Op3[0] >= '8')
                        Op3 = Op3.PadLeft(8, 'f');
                    try
                    {
                        a = Convert.ToInt32(Op2, 16);
                        b = Convert.ToInt32(Op3, 16);
                        PC = Convert.ToString(Convert.ToUInt32(PC, 16) + 4, 16).PadLeft(8, '0');
                        if (a < b)
                            return "0x" + "00000001" + " " + Op1;
                        else
                            return "0x" + "00000000" + " " + Op1;
                    }
                    catch
                    {
                        PC = "00000000";
                        return "Возникло переполнение при выполнении оперции" + Name_op;
                    }

                case ("sltiu"):
                    Number2 = Memory.Registers[Op2][1].Substring(2, 8);
                    if(Op3.PadLeft(3, '0') == "001")
                        if (Number2 == "00000000")
                            Number1 = "00000001";
                        else
                            Number1 = "00000000";
                    else
                    {
                        if (Number2[0] >= '8')
                        {
                            inp1 = Convert.ToUInt32(Number2, 16);
                            inp1 = ~inp1 + 1;
                        }
                        else
                            inp1 = Convert.ToUInt32(Number2, 16);
                        if (Op3[0] >= '8')
                        {
                            inp2 = Convert.ToUInt32(Op3, 16);
                            inp2 = ~inp2 + 1;
                        }
                        else
                            inp2 = Convert.ToUInt32(Op3, 16);

                        if (inp1 < inp2)
                            Number1 = "00000001";
                        else
                            Number1 = "00000000";
                    }
                    PC = Convert.ToString(Convert.ToUInt32(PC, 16) + 4, 16).PadLeft(8, '0');
                    return "0x" + Number1.ToUpper() + " " + Op1;

                case ("xori"):
                    Number2 = Memory.Registers[Op2][1].Substring(2, 8);
                    PC = Convert.ToString(Convert.ToUInt32(PC, 16) + 4, 16).PadLeft(8, '0');
                    if (Op3 == "ffffffff")
                    {
                        Number2 = (~Convert.ToUInt32(Number2, 16)).ToString("X");
                        return "0x" + Number2.ToUpper() + " " + Op1;
                    }
                    if (Op3[0] >= '8')
                        Op3 = Op3.PadLeft(8, 'F');
                    else
                        Op3 = Op3.PadLeft(8, '0');
                    Number1 = (Convert.ToUInt32(Number2, 16) ^ Convert.ToUInt32(Op3, 16)).ToString("X").PadLeft(8, '0').ToUpper();
                    return "0x" + Number1 + " " + Op1;

                case ("ori"):
                    Number2 = Memory.Registers[Op2][1].Substring(2, 8);
                    if (Op3[0] >= '8')
                        Op3 = Op3.PadLeft(8, 'F');
                    else
                        Op3 = Op3.PadLeft(8, '0');
                    PC = Convert.ToString((Convert.ToUInt32(PC, 16) + 4), 16).PadLeft(8, '0');
                    Number1 = (Convert.ToInt32(Number2, 16) | Convert.ToInt32(Op3, 16)).ToString("X").PadLeft(8, '0').ToUpper();
                    return "0x" + Number1 + " " + Op1;

                case ("andi"):
                    Number2 = Memory.Registers[Op2][1].Substring(2, 8);
                    if (Op3[0] >= '8')
                        Op3 = Op3.PadLeft(8, 'F');
                    else
                        Op3 = Op3.PadLeft(8, '0');
                    PC = Convert.ToString(Convert.ToUInt32(PC, 16) + 4, 16).PadLeft(8, '0');
                    Number1 = (Convert.ToInt32(Number2, 16) & Convert.ToInt32(Op3, 16)).ToString("X").PadLeft(8, '0').ToUpper();
                    return "0x" + Number1 + " " + Op1;

                case ("slli"):
                    Number2 = Memory.Registers[Op2][1].Substring(2, 8);
                    if (Op3[0] >= '8')
                        Op3 = Op3.PadLeft(8, 'F');
                    PC = Convert.ToString((Convert.ToUInt32(PC, 16) + 4), 16).PadLeft(8, '0');
                    Number1 = (Convert.ToUInt32(Number2, 16) << Convert.ToInt32(Op3, 16)).ToString("X").PadLeft(8, '0').ToUpper();
                    return "0x" + Number1 + " " + Op1;

                case ("srli"):
                    Number2 = Memory.Registers[Op2][1].Substring(2, 8);
                    if (Op3[0] >= '8')
                        Op3 = Op3.PadLeft(8, 'F');
                    PC = Convert.ToString((Convert.ToUInt32(PC, 16) + 4), 16).PadLeft(8, '0');
                    Number1 = (Convert.ToUInt32(Number2, 16) >> Convert.ToInt32(Op3, 16)).ToString("X").PadLeft(8, '0').ToUpper();
                    return "0x" + Number1 + " " + Op1;

                case ("srai"):///
                    Number2 = Memory.Registers[Op2][1].Substring(2, 8);
                    if (Op3[0] >= '8')
                        Op3 = Op3.PadLeft(8, 'F');
                    Number1 = Convert.ToString(Convert.ToUInt32(Number2, 16) >> Convert.ToInt32(Op3, 16), 16);
                    if (Number2[0] >= '8')
                    {
                        Number1 = Convert.ToString(Convert.ToUInt32(Number1, 16), 2).PadLeft(32, '0').Substring(Convert.ToInt32(Op3, 16), 32 - Convert.ToInt32(Op3, 16)).PadLeft(32, '1');
                        Number1 = Convert.ToString(Convert.ToUInt32(Number1, 2), 16).PadLeft(8, '0');
                    }
                    PC = Convert.ToString(Convert.ToUInt32(PC, 16) + 4, 16).PadLeft(8, '0');
                    return "0x" + Number1.ToUpper() + " " + Op1;

                case ("add"):
                    Number1 = Memory.Registers[Op2][1].Substring(2, 8);
                    Number2 = Memory.Registers[Op3][1].Substring(2, 8);
                    try
                    {
                        a = Convert.ToInt32(Number1, 16);
                        b = Convert.ToInt32(Number2, 16);
                        Number1 = Convert.ToString(a + b, 16).PadLeft(8, '0');
                        PC = Convert.ToString(Convert.ToUInt32(PC, 16) + 4, 16).PadLeft(8, '0');
                        return "0x" + Number1.PadLeft(8, '0').ToUpper() + " " + Op1;
                    }
                    catch
                    {
                        PC = "00000000";
                        return "Возникло переполнение при выполнении оперции" + Name_op;
                    }

                case ("sub"):
                    Number1 = Memory.Registers[Op2][1].Substring(2, 8);
                    Number2 = Memory.Registers[Op3][1].Substring(2, 8);
                    try
                    {
                        Number1 = Convert.ToString(Convert.ToInt32(Number1, 16) - Convert.ToInt32(Number2, 16), 16);
                        PC = Convert.ToString(Convert.ToUInt32(PC, 16) + 4, 16).PadLeft(8, '0');
                        return "0x" + Number1.PadLeft(8, '0').ToUpper() + " " + Op1;
                    }
                    catch (Exception)
                    {
                        PC = "00000000";
                        return "Возникло переполнение при выполнении оперции" + Name_op;
                    }

                case ("sll"):
                    Number1 = Memory.Registers[Op2][1].Substring(2, 8);
                    Number2 = Memory.Registers[Op3][1].Substring(2, 8);
                    PC = Convert.ToString(Convert.ToUInt32(PC, 16) + 4, 16).PadLeft(8, '0');
                    Number1 = (Convert.ToUInt32(Number1, 16) << Convert.ToInt32(Number2, 16)).ToString("X").PadLeft(8, '0').ToUpper();
                    return "0x" + Number1 + " " + Op1;

                case ("slt"):
                    try
                    {
                        Number1 = Memory.Registers[Op2][1].Substring(2, 8);
                        Number2 = Memory.Registers[Op3][1].Substring(2, 8);
                        a = Convert.ToInt32(Number1, 16);
                        b = Convert.ToInt32(Number2, 16);
                        PC = Convert.ToString(Convert.ToUInt32(PC, 16) + 4, 16).PadLeft(8, '0');
                        if (a < b)
                            return "0x00000001" + " " + Op1;
                        else
                            return "0x00000000" + " " + Op1;
                    }
                        
                    catch
                    {
                        PC = "00000000";
                        return "Возникло переполнение при выполнении оперции" + Name_op;
                    }

                case ("sltu"):
                    Number1 = Memory.Registers[Op2][1].Substring(2, 8);
                    Number2 = Memory.Registers[Op3][1].Substring(2, 8);
                    if (Number1 == "00000000")
                        Number1 = "00000001";
                    else
                    {

                        if (Number1[0] >= '8')
                        {
                            inp1 = Convert.ToUInt32(Number1, 16);
                            inp1 = ~inp1 + 1;
                        }
                        else
                        {
                            inp1 = Convert.ToUInt32(Number1, 16);
                        }

                        if (Number2[0] >= '8')
                        {
                            inp2 = Convert.ToUInt32(Number2, 16);
                            inp2 = ~inp2 + 1;
                        }
                        else
                        {
                            inp2 = Convert.ToUInt32(Number2, 16);
                        }

                        if (inp1 < inp2)
                            Number1 = "00000001";
                        else
                            Number1 = "00000000";
                    }
                    PC = Convert.ToString(Convert.ToUInt32(PC, 16) + 4, 16).PadLeft(8, '0');
                    return "0x" + Number1 + " " + Op1;

                case ("xor"):
                    Number1 = Memory.Registers[Op2][1].Substring(2, 8);
                    Number2 = Memory.Registers[Op3][1].Substring(2, 8);
                    PC = Convert.ToString(Convert.ToUInt32(PC, 16) + 4, 16).PadLeft(8, '0');
                    Number1 = (Convert.ToUInt32(Number1, 16) ^ Convert.ToUInt32(Number2, 16)).ToString("X").PadLeft(8, '0').ToUpper();
                    return "0x" + Number1 + " " + Op1;

                case ("srl"):
                    Number1 = Memory.Registers[Op2][1].Substring(2, 8);
                    Number2 = Memory.Registers[Op3][1].Substring(2, 8);
                    PC = Convert.ToString((Convert.ToUInt32(PC, 16) + 4), 16).PadLeft(8, '0');
                    Number1 = (Convert.ToUInt32(Number1, 16) >> Convert.ToInt32(Number2, 16)).ToString("X").PadLeft(8, '0').ToUpper();
                    return "0x" + Number1 + " " + Op1;

                case ("sra"):
                    Number1 = Memory.Registers[Op2][1].Substring(2, 8);
                    Number2 = Memory.Registers[Op3][1].Substring(2, 8);
                    Number1 = Convert.ToString(Convert.ToUInt32(Number1, 16) >> Convert.ToInt32(Number2, 16), 16);
                    if (Number2[0] >= '8')
                    {
                        Number1 = Convert.ToString(Convert.ToUInt32(Number1, 16), 2).PadLeft(32, '0').Substring(Convert.ToInt32(Op3, 16), 32 - Convert.ToInt32(Op3, 16)).PadLeft(32, '1');
                        Number1 = Convert.ToString(Convert.ToUInt32(Number1, 2), 16).PadLeft(8, '0');
                    }
                    PC = Convert.ToString(Convert.ToUInt32(PC, 16) + 4, 16).PadLeft(8, '0');
                    return "0x" + Number1.PadLeft(8, '0') + " " + Op1;

                case ("or"):
                    Number1 = Memory.Registers[Op2][1].Substring(2, 8);
                    Number2 = Memory.Registers[Op3][1].Substring(2, 8);
                    PC = Convert.ToString(Convert.ToUInt32(PC, 16) + 4, 16).PadLeft(8, '0');
                    Number1 = (Convert.ToUInt32(Number1, 16) | Convert.ToUInt32(Number2, 16)).ToString("X").PadLeft(8, '0').ToUpper();
                    return "0x" + Number1 + " " + Op1;

                case ("and"):
                    Number1 = Memory.Registers[Op2][1].Substring(2, 8);
                    Number2 = Memory.Registers[Op3][1].Substring(2, 8);
                    PC = Convert.ToString(Convert.ToUInt32(PC, 16) + 4, 16).PadLeft(8, '0');
                    Number1 = (Convert.ToUInt32(Number1, 16) & Convert.ToUInt32(Number2, 16)).ToString("X").PadLeft(8, '0').ToUpper();
                    return "0x" + Number1 + " " + Op1;

                case "ecall":
                    Number1 = Memory.Registers["x10"][1].Substring(2, 8);
                    if (Convert.ToInt32(Number1, 16) != 10)
                    {
                        PC = Convert.ToString(Convert.ToUInt32(PC, 16) + 4, 16).PadLeft(8, '0');
                        switch (Convert.ToInt32(Number1, 16))
                        {
                            case 1:
                                {
                                    Number2 = Memory.Registers["x11"][1].Substring(2, 8);
                                    Console += Convert.ToInt32(Number2, 16).ToString();
                                    return "";
                                }
                            case 11:
                                {
                                    Number2 = Memory.Registers["x11"][1].Substring(8, 2);
                                    byte buf = Convert.ToByte(Convert.ToInt32(Number2, 16));
                                    Console += Encoding.GetEncoding(1251).GetString((new byte[] { buf }));
                                    return "";
                                }
                            default:
                                {
                                    Console += "\necall don't worked with a0 = " + Number1;
                                    return "ecall exit";
                                }
                        }
                    }
                    else
                    {
                        Console += "\nДостигнут конец программы!! :D";
                        PC = "00000000";
                        return "ecall exit";
                    }
                default:
                    Console += "\nДостигнут конец программы!! :D";
                    PC = "00000000";
                    return "";
            }
        }
    }

}