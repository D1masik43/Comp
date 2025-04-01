﻿using FastColoredTextBoxNS;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Diagnostics;

namespace Comp
{
    public partial class Form1 : Form
    {
        private AutocompleteMenu autocompleteMenu;

        // Define text styles
        TextStyle commentStyle = new TextStyle(Brushes.Green, null, FontStyle.Italic);
        TextStyle labelStyle = new TextStyle(Brushes.Blue, null, FontStyle.Bold);
        TextStyle instructionStyle = new TextStyle(Brushes.Black, null, FontStyle.Bold);
        TextStyle jmpArgumentStyle = new TextStyle(Brushes.Blue, null, FontStyle.Bold);
        TextStyle normalArgumentStyle = new TextStyle(Brushes.Purple, null, FontStyle.Bold);

        Regex labelRegex = new Regex(@"Lab\s+(\w+)\s*:"); // Початок Lab
        Regex varRegex = new Regex(@"dvar\s+(\w+)\s*=\s*(-?\d+)");  // dvar name = -123
        Regex dvarRegex = new Regex(@"dvar\s+(\w+)\s*=\s*(\d+)");  // dvar name = 123
        Regex sdvarRegex = new Regex(@"dvar\s+(\w+)\s*=\s*(-\d+)");  // dvar name = -123
        Regex bvarRegex = new Regex(@"bvar\s+(\w+)\s*=\s*(\d+)");  // bvar name = 10
        Regex instructionRegex = new Regex(@"(\w+)\s+(\S+.*)?"); // Інструкція з операндом


        public Form1()
        {
            InitializeComponent();
            richTextBox1.TextChanged += fastColoredTextBox1_TextChanged;
            // Initialize Autocomplete Menu
            autocompleteMenu = new AutocompleteMenu(richTextBox1);
            autocompleteMenu.MinFragmentLength = 1; // Show suggestions after 1 character
            richTextBox1.TextChanged += (s, e) => UpdateAutocomplete();

            // Set items for autocomplete
            autocompleteMenu.Items.SetAutocompleteItems(GetAutocompleteItems());
        }
        private void UpdateAutocomplete()
        {
            // Combine instructions with dynamically found labels and variables
            List<string> autocompleteItems = new List<string>(GetAssemblyInstructions());
            autocompleteItems.AddRange(ExtractLabelsAndVariables());

            // Update autocomplete list
            autocompleteMenu.Items.SetAutocompleteItems(autocompleteItems);
        }
        private List<string> GetAssemblyInstructions()
        {
            return new List<string>
        {
            "LOAD", "STORE", "ADD", "SUB", "AND", "OR", "XOR", "NOT",
            "INPUT", "OUTPUT", "HALT", "JNZ", "JZ", "JP", "JM",
            "JNC", "JC", "JMP", "LSL", "LSR", "ASL", "ASR",
            "ROL", "ROR", "RCL", "RCR",
            "dvar", "bvar", "Lab"
        };
        }
        private List<string> ExtractLabelsAndVariables()
        {
            List<string> foundItems = new List<string>();
            string text = richTextBox1.Text;

       

            // Find labels
            foreach (Match match in labelRegex.Matches(text))
            {
                if (!foundItems.Contains(match.Groups[1].Value))
                    foundItems.Add(match.Groups[1].Value); 
            }

            // Find variables
            foreach (Match match in varRegex.Matches(text))
            {
                if (!foundItems.Contains(match.Groups[1].Value))
                    foundItems.Add(match.Groups[1].Value);
            }

            return foundItems;
        }

        private List<string> GetAutocompleteItems()
        {
            return new List<string>
        {
            // Assembly Instructions
            "LOAD", "STORE", "ADD", "SUB", "AND", "OR", "XOR", "NOT",
            "INPUT", "OUTPUT", "HALT", "JNZ", "JZ", "JP", "JM",
            "JNC", "JC", "JMP", "LSL", "LSR", "ASL", "ASR",
            "ROL", "ROR", "RCL", "RCR",
            // Other elements (Variables, Labels)
            "dvar", "bvar", "Lab"
        };
        }
        private void fastColoredTextBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            e.ChangedRange.ClearStyle(StyleIndex.All);

            // Highlight comments
            e.ChangedRange.SetStyle(commentStyle, @";.*");

            // Highlight labels (word followed by a colon)
            e.ChangedRange.SetStyle(labelStyle, @"\b\w+\s*:");

            // Highlight instructions
            string instructionPattern = @"\b(LOAD|STORE|ADD|SUB|AND|OR|XOR|NOT|INPUT|OUTPUT|HALT|JNZ|JZ|JP|JM|JNC|JC|JMP|LSL|LSR|ASL|ASR|ROL|ROR|RCL|RCR)\b";
            e.ChangedRange.SetStyle(instructionStyle, instructionPattern, RegexOptions.IgnoreCase);

            // Highlight instruction arguments (differentiating jump and non-jump instructions)
            foreach (var line in e.ChangedRange.Text.Split('\n'))
            {
                var words = line.Trim().Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (words.Length > 1)
                {
                    string instruction = words[0].ToUpper();
                    string argument = words[1];

                    // If instruction is a jump instruction, color its argument blue
                    if (JmpInstructions.Contains(instruction))
                    {
                        e.ChangedRange.SetStyle(jmpArgumentStyle, $@"\b{argument}\b");
                    }
                    else if (IsInstruction(instruction)) // If it's another instruction, color argument purple
                    {
                        e.ChangedRange.SetStyle(normalArgumentStyle, $@"\b{argument}\b");
                    }
                    else if (IsVariable(instruction)) // If it's another instruction, color argument purple
                    {
                        e.ChangedRange.SetStyle(normalArgumentStyle, $@"\b{argument}\b");
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ParseCode(richTextBox1.Text);
            GenerateBinaryCode();
        }
        private void GenerateBinaryCode()
        {
            Stopwatch stopwatch = Stopwatch.StartNew(); // Start timing
            richTextBox2.Text = ""; // Очистити перед виводом
            int addressCounter = 0;

            // Спочатку записуємо інструкції
            foreach (var instructionArray in instructionOperands)
            {
                string instruction = instructionArray[0, 0]; // Інструкція
                string operand = instructionArray[0, 1];     // Операнд

                string binaryCode = "";
                if (JmpInstructions.Contains(instruction))
                {
                    int address = labels.ContainsKey("Lab" + operand) ? labels["Lab" + operand] : 0;
                    binaryCode = GetBinaryCodeForInstruction(instruction) + Convert.ToString(address, 2).PadLeft(12, '0');
                }
                else if (!string.IsNullOrEmpty(operand))
                {
                    int operandValue = variables.Keys.ToList().IndexOf(operand) + instructions.Count + 1;
                    binaryCode = GetBinaryCodeForInstruction(instruction) + Convert.ToString(operandValue, 2).PadLeft(12, '0');
                }
                else if (instructionsWithoutOperands.Contains(instruction))
                {
                    binaryCode = GetBinaryCodeForInstruction(instruction) + "000000000000"; // Без операнда
                }

                string formattedOutput = FormatBinaryOutput(addressCounter, binaryCode);
                richTextBox2.AppendText(formattedOutput + "\n");
                addressCounter++;
            }

            // Додаємо код для HALT
            richTextBox2.AppendText(FormatBinaryOutput(addressCounter, "0111110000000000") + "\n");
            addressCounter++;

            // Потім записуємо змінні
            foreach (var variable in variables)
            {
                string formattedOutput = FormatBinaryOutput(addressCounter, variable.Value.PadLeft(16, '0'));
                richTextBox2.AppendText(formattedOutput + "\n");
                addressCounter++;
            }

            // Видаляємо останній символ (перенос рядка)
            if (richTextBox2.Text.Length > 0)
            {
                richTextBox2.Text = richTextBox2.Text.TrimEnd('\n');
            }
            stopwatch.Stop(); // Stop timing

            PrintResults();
            long elapsedNanoseconds = (stopwatch.ElapsedTicks * 1_000_000L) / Stopwatch.Frequency;

            // Print the build time in richTextBox3
            richTextBox3.AppendText($"\nЧас компіляції: {elapsedNanoseconds} мкрс\n");
        }

        private string FormatBinaryOutput(int address, string binaryCode)
        {
            string addressBinary = Convert.ToString(address, 2).PadLeft(12, '0');
            return $" {addressBinary.Substring(0, 4)} {addressBinary.Substring(4, 4)} {addressBinary.Substring(8, 4)}  {binaryCode.Substring(0, 4)} {binaryCode.Substring(4, 4)} {binaryCode.Substring(8, 4)} {binaryCode.Substring(12, 4)}";
        }



        private string GetBinaryCodeForInstruction(string instruction)
        {
            switch (instruction.ToUpper())
            {
                case "LOAD":
                    return "0000";
                case "STORE":
                    return "0001";
                case "ADD":
                    return "0010";
                case "SUB":
                    return "0011";
                case "AND":
                    return "0100";
                case "OR":
                    return "0101";
                case "XOR":
                    return "0110";
                case "NOT":
                    return "011100";
                case "INPUT":
                    return "011101";
                case "OUTPUT":
                    return "011110";
                case "HALT":
                    return "011111";
                case "JNZ":
                    return "1000";
                case "JZ":
                    return "1001";
                case "JP":
                    return "1010";
                case "JM":
                    return "1011";
                case "JNC":
                    return "1100";
                case "JC":
                    return "1101";
                case "JMP":
                    return "1110";
                case "LSL":
                    return "1111000";
                case "LSR":
                    return "1111001";
                case "ASL":
                    return "1111010";
                case "ASR":
                    return "1111011";
                case "ROL":
                    return "1111100";
                case "ROR":
                    return "1111101";
                case "RCL":
                    return "1111110";
                case "RCR":
                    return "1111111";
                default:
                    return "0000"; // Якщо інструкція не знайдена, повертаємо стандартний код
            }
        }

        private Dictionary<string, int> labels = new Dictionary<string, int>(); // Lab -> Адреса
        private List<string> instructions = new List<string>(); // Інструкції
        private Dictionary<string, string> variables = new Dictionary<string, string>(); // {ім'я -> бінарне значення}
        private List<string[,]> instructionOperands = new List<string[,]>(); // Список масивів {інструкція, операнд}
        private int instructionCounter = 0; // Лічильник інструкцій
        private string RemoveComments(string line)
        {
            int commentIndex = line.IndexOf(';');
            return commentIndex >= 0 ? line.Substring(0, commentIndex).Trim() : line.Trim();
        }

        public void ParseCode(string code)
        {
          
            labels.Clear();
            instructions.Clear();
            variables.Clear();
            instructionOperands.Clear();
            instructionCounter = 0;

            string[] lines = code.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

           

            bool insideLabel = false;
            string currentLabel = "";

            foreach (string line in lines)
            {
                string trimmedLine = RemoveComments(line);

                if (string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith(";"))
                    continue; // Пропускаємо коментарі та порожні рядки

                // Перевірка на Lab
                Match labelMatch = labelRegex.Match(trimmedLine);
                if (labelMatch.Success)
                {
                    string labelName = "Lab" + labelMatch.Groups[1].Value;
                    labels[labelName] = instructionCounter; // Запам'ятовуємо адресу Lab
                    continue;
                }

                // Перевірка на змінну dvar
                Match dvarMatch = dvarRegex.Match(trimmedLine);
                if (dvarMatch.Success)
                {
                    string varName = dvarMatch.Groups[1].Value;
                    int value = int.Parse(dvarMatch.Groups[2].Value);
                    string binaryValue = Convert.ToString(value, 2).PadLeft(12, '0'); // 12-бітне представлення
                    variables[varName] = binaryValue;
                    continue;
                }

                // Перевірка на змінну sdvar
                Match sdvarMatch = sdvarRegex.Match(trimmedLine);
                if (sdvarMatch.Success)
                {
                    string varName = sdvarMatch.Groups[1].Value;
                    int value = int.Parse(sdvarMatch.Groups[2].Value);

                    // Перетворення у 12-бітне двійкове представлення (two’s complement)
                    string binaryValue = Convert.ToString(value & 0xFFF, 2).PadLeft(12, '0');

                    variables[varName] = binaryValue;
                    continue;
                }


                // Перевірка на змінну bvar
                Match bvarMatch = bvarRegex.Match(trimmedLine);
                if (bvarMatch.Success)
                {
                    string varName = bvarMatch.Groups[1].Value;
                    string binaryString = bvarMatch.Groups[2].Value;
                    string binaryValue = binaryString.PadLeft(12, '0'); // 12-бітне представлення
                    variables[varName] = binaryValue;
                    continue;
                }

                // Якщо це інструкція
                if (IsInstruction(trimmedLine))
                {
                    Match instructionMatch = instructionRegex.Match(trimmedLine);
                    string instruction = instructionMatch.Groups[1].Value;
                    string operand = instructionMatch.Groups[2]?.Value;  // Забезпечуємо безпеку, якщо операнда немає

                    // Якщо інструкція без операнда
                    if (instructionsWithoutOperands.Contains(trimmedLine))
                    {
                        instructionOperands.Add(new string[,] { { trimmedLine, null } });
                    }
                    else
                    {
                        // Якщо інструкція має операнд, додаємо операнд
                        instructionOperands.Add(new string[,] { { instruction, operand } });
                    }

                    instructions.Add(instruction);
                    instructionCounter++;
                }





            }
    
        }

        private bool IsInstruction(string line)
        {
            string[] instructionSet =
            {
                "LOAD", "STORE", "ADD", "SUB", "AND", "OR", "XOR", "NOT", "INPUT", "OUTPUT", "HALT",
                "JNZ", "JZ", "JP", "JM", "JNC", "JC", "JMP", "LSL", "LSR", "ASL", "ASR",
                "ROL", "ROR", "RCL", "RCR"
            };

            return instructionSet.Any(inst => line.StartsWith(inst, StringComparison.OrdinalIgnoreCase));
        }
        private bool IsVariable(string line)
        {
            string[] instructionSet =
            {
                "dvar", "svar"
            };

            return instructionSet.Any(inst => line.StartsWith(inst, StringComparison.OrdinalIgnoreCase));
        }
        // Список інструкцій без операндів
        private HashSet<string> instructionsWithoutOperands = new HashSet<string>
        {
            "NOT", "INPUT", "OUTPUT", "HALT", "JNZ", "JZ", "JP", "JM", "JNC", "JC", "JMP",
            "LSL", "LSR", "ASL", "ASR", "ROL", "ROR", "RCL", "RCR"
        };
        private HashSet<string> JmpInstructions = new HashSet<string>
        {
            "JNZ", "JZ", "JP", "JM", "JNC", "JC", "JMP"
        };

        private void PrintResults()
        {
            richTextBox3.Text = ""; // Очистити перед виводом

            richTextBox3.AppendText($"Кількість інструкцій: {instructions.Count}\n");
            richTextBox3.AppendText($"\nКількість змінних: {variables.Count}\n");

            richTextBox3.AppendText("\nМітки (Lab) та їх адреси:\n");
            foreach (var label in labels)
            {
                richTextBox3.AppendText($"{label.Key} -> {label.Value}\n");
            }

            richTextBox3.AppendText("\nЗмінні ==> адреса : ім`я -> значення  :\n");
            int index = 0;
            foreach (var variable in variables)
            {
                richTextBox3.AppendText($" {Convert.ToString((index + 1 + instructions.Count) & 0xFFF, 2).PadLeft(12, '0')} : {variable.Key} -> {variable.Value}\n");
                index++;
            }

            richTextBox3.AppendText("\nІнструкції з операндами:\n");
            foreach (var instructionArray in instructionOperands)
            {
                string instruction = instructionArray[0, 0];
                string operand = instructionArray[0, 1];

                if (operand == null)
                {
                    richTextBox3.AppendText($"{instruction} -> (операнд ігнорується)\n");
                }
                else
                {
                    richTextBox3.AppendText($"{instruction} -> {operand}\n");
                }
            }
        }
    }
}
