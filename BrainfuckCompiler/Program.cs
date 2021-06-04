using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainfuckInterpreter {
    class Program {
        public static uint[] Cells;
        public static List<int> PointerLoop;
        public static string Input = "";
        public static bool stepThrough;
        public static bool writeLogs;
        public static bool waitOutput;
        static uint Pointer = 0;
        static string output;
        static void Main(string[] args) {
            Cells = new uint[30000];
            PointerLoop = new List<int>();

            string fileName = "";

            while(string.IsNullOrEmpty(Input)) {
                Console.WriteLine("File name:");
                fileName = Console.ReadLine();

                if(File.Exists(System.Reflection.Assembly.GetExecutingAssembly().Location.Replace("\\BrainfuckCompiler.exe", "") + $"/{fileName}")) {
                    Input = File.ReadAllText(System.Reflection.Assembly.GetExecutingAssembly().Location.Replace("\\BrainfuckCompiler.exe", "") + $"/{fileName}");
                }
            }

            Console.WriteLine("Manual step through code? y/n");
            string checkStep = Console.ReadLine();
            bool outputCheck = false;
            if(checkStep == "y") {
                stepThrough = true;
                outputCheck = true;
            } else {
                Console.WriteLine("Display program info anyway? y/n");
                string checkWrite = Console.ReadLine();

                if(checkWrite == "y") {
                    writeLogs = true;
                    outputCheck = true;
                }
            }

            if(outputCheck) {
                Console.WriteLine("Save all outputs until the end? y/n");
                string waitCheck = Console.ReadLine();
                if(waitCheck == "y") {
                    waitOutput = true;
                }
            }

            if(string.IsNullOrEmpty(Input)) {
                Console.WriteLine("No input given. Ending program");
                return;
            }

            for(int i = 0; i < Input.Length; i++) {
                switch(Input[i]) {
                    case '>':
                        if(Pointer == 29999) {
                            Pointer = 0;
                            break;
                        }
                        Pointer++;
                        break;
                    case '<':
                        if(Pointer == 0) {
                            Pointer = 29999;
                            break;
                        }
                        Pointer--;
                        break;
                    case '+':
                        Cells[Pointer]++;
                        break;
                    case '-':
                        Cells[Pointer]--;
                        break;
                    case '.':
                        if(waitOutput) {
                            output += (char)Cells[Pointer];
                        } else {
                            Console.Write((char)Cells[Pointer]);
                        }
                        break;
                    case ',':
                        Console.Write('\n');
                        Console.WriteLine("Give me something!");
                        Cells[Pointer] = (uint)Console.Read();
                        break;
                    case '[':
                        if(Cells[Pointer] != 0) {
                            PointerLoop.Add(i);
                            break;
                        }
                        for(int x = i; x < Input.Length; x++) {
                            if(Input[x] == ']') {
                                i = x + 1;
                                break;
                            }
                        }
                        break;
                    case ']':
                        if(Cells[Pointer] == 0) {
                            PointerLoop.Remove(PointerLoop[PointerLoop.Count - 1]);
                            break;
                        }
                        i = PointerLoop[PointerLoop.Count - 1];
                        break;
                }
                if(writeLogs) {
                    Console.WriteLine($"Command:{Input[i]}|Pointer Loc:{Pointer}|Cell Value:{Cells[Pointer]}|Cell Hexcode:{(char)Cells[Pointer]}");
                }
                if(stepThrough) {
                    Console.WriteLine($"Command:{Input[i]}|Pointer Loc:{Pointer}|Cell Value:{Cells[Pointer]}|Cell Hexcode:{(char)Cells[Pointer]}");
                    Console.ReadLine();
                }
            }

            if(waitOutput) {
                Console.WriteLine($"Output: {output}");
            }
            Console.WriteLine("Program Complete");
            Console.ReadLine();
        }
    }
}
