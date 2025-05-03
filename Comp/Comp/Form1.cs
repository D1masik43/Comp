using FastColoredTextBoxNS;
using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Diagnostics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
        TextStyle errorStyle = new TextStyle(null, Brushes.Red, FontStyle.Regular);


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
            autocompleteMenu.AllowTabKey = true;
            autocompleteMenu.MinFragmentLength = 1; // Show suggestions after 1 character
            richTextBox1.TextChanged += (s, e) => UpdateAutocomplete();

            // Set items for autocomplete
            autocompleteMenu.Items.SetAutocompleteItems(GetAutocompleteItems());
            this.Resize += Form1_Resize; // Attach Resize event
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                // When the form is maximized, set bounds to exclude the taskbar

                this.Bounds = Screen.FromControl(this).WorkingArea;
            }
            else if (this.WindowState == FormWindowState.Normal)
            {
                // When the form is restored to normal, remove the size constraint
                this.MaximumSize = new Size(int.MaxValue, int.MaxValue);
            }

            int width = this.ClientSize.Width;
            int height = this.ClientSize.Height;
            int paddingUp = (int)(height * 0.03);
            int paddingLeft = (int)(width * 0.08);
            int rtbWidth = (int)(width * 0.44);
            int rtbHeight = (int)(height * 0.70);
            int rtb3Width = (int)(width * 0.88);
            int rtb3Top = richTextBox1.Bottom + paddingUp;
            int rtb3Height = this.ClientSize.Height - rtb3Top - paddingUp; // subtract bottom padding too
            int upperPad = 36;
            int buttonWidth = (int)(width * 0.06);

            richTextBox1.SetBounds(12, upperPad, rtbWidth, rtbHeight);
            richTextBox2.SetBounds(12 + rtbWidth + paddingLeft, upperPad, rtbWidth, rtbHeight);
            richTextBox3.SetBounds(12 + paddingLeft, richTextBox1.Bottom + paddingUp, rtb3Width, rtb3Height);
            button1.SetBounds(14, richTextBox1.Bottom + paddingUp, buttonWidth, button1.Height);

            int but2left = (int)(richTextBox2.Left + richTextBox2.Width - button2.Width);


            button2.SetBounds(but2left, 7, (int)(width * 0.169), button2.Height);
            button3.SetBounds(12, 7, (int)(width * 0.085), button3.Height);
            button4.SetBounds(12 + button3.Width, 7, (int)(width * 0.095), button3.Height);
            /*                          ==Debug==
            string info = $"RTB1: Location=({richTextBox1.Left}, {richTextBox1.Top}), Size=({richTextBox1.Width}x{richTextBox1.Height})\n" +
              $"RTB2: Location=({richTextBox2.Left}, {richTextBox2.Top}), Size=({richTextBox2.Width}x{richTextBox2.Height})\n" +
              $"RTB3: Location=({richTextBox3.Left}, {richTextBox3.Top}), Size=({richTextBox3.Width}x{richTextBox3.Height})\n";

            richTextBox1.AppendText("\n--- RTB123 Layout Info ---\n" + info);
            */

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
            Stopwatch stopwatch = Stopwatch.StartNew();
            richTextBox2.Text = "";
            richTextBox3.Text = ""; // Clear previous logs
            int addressCounter = 0;
            bool hasErrors = false;


            foreach (var instructionArray in instructionOperands)
            {
                string instruction = instructionArray[0, 0];
                string operand = instructionArray[0, 1];
                int lineNumber = int.Parse(instructionArray[0, 2]); // Line number from instructionOperands

                string binaryCode = "";

                if (JmpInstructions.Contains(instruction))
                {
                    string labelKey = "Lab" + operand;
                    if (labels.ContainsKey(labelKey))
                    {
                        int address = labels[labelKey];
                        binaryCode = GetBinaryCodeForInstruction(instruction) + Convert.ToString(address, 2).PadLeft(12, '0');
                    }
                    else
                    {
                        richTextBox3.AppendText($"[Error] Label '{operand}' not found.\n");
                        hasErrors = true;

                        // Highlight the error line in red
                        HighlightErrorLine(lineNumber);
                        continue; // Skip this instruction
                    }
                }
                else if (!string.IsNullOrEmpty(operand))
                {
                    if (variables.ContainsKey(operand))
                    {
                        int operandValue = variables.Keys.ToList().IndexOf(operand) + instructions.Count + 1;
                        binaryCode = GetBinaryCodeForInstruction(instruction) + Convert.ToString(operandValue, 2).PadLeft(12, '0');
                    }
                    else
                    {
                        richTextBox3.AppendText($"[Error] Variable '{operand}' not declared.\n");
                        hasErrors = true;

                        // Highlight the error line in red
                        HighlightErrorLine(lineNumber);
                        continue; // Skip this instruction
                    }
                }
                else if (instructionsWithoutOperands.Contains(instruction))
                {
                    binaryCode = GetBinaryCodeForInstruction(instruction) + "000000000000";
                }
                else
                {
                    richTextBox3.AppendText($"[Error] Instruction '{instruction}' missing a valid operand.\n");
                    hasErrors = true;

                    // Highlight the error line in red
                    HighlightErrorLine(lineNumber);
                    continue; // Skip this instruction
                }
                
                string formattedOutput = FormatBinaryOutput(addressCounter, binaryCode);
                richTextBox2.AppendText(formattedOutput + "\n");
                addressCounter++;
            }

            // Add HALT
            richTextBox2.AppendText(FormatBinaryOutput(addressCounter, "0111110000000000") + "\n");
            addressCounter++;

            // Add variables
            foreach (var variable in variables)
            {
                string formattedOutput = FormatBinaryOutput(addressCounter, variable.Value.PadLeft(16, '0'));
                richTextBox2.AppendText(formattedOutput + "\n");
                addressCounter++;
            }

            // Trim final newline
            if (richTextBox2.Text.Length > 0)
                richTextBox2.Text = richTextBox2.Text.TrimEnd('\n');

            stopwatch.Stop();
            PrintResults();

            double elapsedMilliseconds = (double)stopwatch.ElapsedTicks * 1000.0 / Stopwatch.Frequency;
            richTextBox3.AppendText($"\nЧас компіляції: {elapsedMilliseconds:F3} мс\n");

            if (hasErrors)
            {
                richTextBox3.AppendText("⚠️ Увага: Деякі інструкції не згенеровані через помилки.\n");
            }
        }

        // Method to highlight error lines in richTextBox1 with red background
        private void HighlightErrorLine(int lineNumber)
        {
            // Define a style for errors
            

            // Check if the line exists in FCTB
            if (lineNumber > 0 && lineNumber <= richTextBox1.LinesCount)
            {
                var lineRange = richTextBox1.GetLine(lineNumber - 1); // FCTB is 0-based, so we subtract 1
                lineRange.ClearStyle(StyleIndex.All);
                lineRange.SetStyle(errorStyle); // Apply the error style
                
            }
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

            string[] lines = code.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);


            bool insideLabel = false;
            string currentLabel = "";

            for (int lineNumber = 0; lineNumber < lines.Length; lineNumber++) // Use lineNumber to get the line number
            {
                string line = lines[lineNumber];
                string trimmedLine = RemoveComments(line);

                if (string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith(";"))
                    continue; // Skip comments and empty lines

                // Check for Label
                Match labelMatch = labelRegex.Match(trimmedLine);
                if (labelMatch.Success)
                {
                    string labelName = "Lab" + labelMatch.Groups[1].Value;
                    labels[labelName] = instructionCounter; // Store the address of the label
                    continue;
                }

                // Check for variable dvar
                Match dvarMatch = dvarRegex.Match(trimmedLine);
                if (dvarMatch.Success)
                {
                    string varName = dvarMatch.Groups[1].Value;
                    int value = int.Parse(dvarMatch.Groups[2].Value);
                    string binaryValue = Convert.ToString(value, 2).PadLeft(16, '0'); // 12-bit representation
                    variables[varName] = binaryValue;
                    continue;
                }

                // Check for variable sdvar
                Match sdvarMatch = sdvarRegex.Match(trimmedLine);
                if (sdvarMatch.Success)
                {
                    string varName = sdvarMatch.Groups[1].Value;
                    int value = int.Parse(sdvarMatch.Groups[2].Value);

                    // Convert to 12-bit two's complement representation
                    string binaryValue = Convert.ToString(value & 0xFFFF, 2).PadLeft(16, '0');

                    variables[varName] = binaryValue;
                    continue;
                }

                // Check for variable bvar
                Match bvarMatch = bvarRegex.Match(trimmedLine);
                if (bvarMatch.Success)
                {
                    string varName = bvarMatch.Groups[1].Value;
                    string binaryString = bvarMatch.Groups[2].Value;
                    string binaryValue = binaryString.PadLeft(16, '0'); // 12-bit representation
                    variables[varName] = binaryValue;
                    continue;
                }

                // If it's an instruction
                if (IsInstruction(trimmedLine))
                {
                    Match instructionMatch = instructionRegex.Match(trimmedLine);
                    string instruction = instructionMatch.Groups[1].Value;
                    string operand = instructionMatch.Groups[2]?.Value;  // Ensure safety if there's no operand

                    // If the instruction has no operand
                    if (instructionsWithoutOperands.Contains(trimmedLine))
                    {
                        instructionOperands.Add(new string[,] { { trimmedLine, null, (lineNumber + 1).ToString() } });
                    }
                    else
                    {
                        // If the instruction has an operand, add the operand
                        instructionOperands.Add(new string[,] { { instruction, operand, (lineNumber + 1).ToString() } });
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
                "dvar", "bvar"
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
                richTextBox3.AppendText($" {Convert.ToString((index + 1 + instructions.Count) & 0xFFF, 2).PadLeft(16, '0')} : {variable.Key} -> {variable.Value}\n");
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
        string currentFilePath = null; // Keep track of the file path
        private void button2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(currentFilePath))
            {
                // Path is already known, save directly
                File.WriteAllText(currentFilePath, richTextBox2.Text);
            }
            else
            {
                // Ask the user for the path
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.OverwritePrompt = false;
                    saveFileDialog.Filter = "Program Files (*.prg)|*.prg|All Files (*.*)|*.*";
                    saveFileDialog.Title = "Вкажіть шлях до DeComp.prg";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        currentFilePath = saveFileDialog.FileName;
                        File.WriteAllText(currentFilePath, richTextBox2.Text);
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Assemly (*.Comp)|*.Comp|All Files (*.*)|*.*";
            saveDialog.Title = "Зберегти як";
            saveDialog.DefaultExt = "Comp";
            saveDialog.AddExtension = true;

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveDialog.FileName, richTextBox1.Text);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Assemly (*.Comp)|*.Comp|All Files (*.*)|*.*";
            openDialog.Title = "Відкрити";

            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.Text = File.ReadAllText(openDialog.FileName);
            }
        }
    }
}