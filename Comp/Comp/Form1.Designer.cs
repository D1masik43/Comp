namespace Comp
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.richTextBox3 = new System.Windows.Forms.RichTextBox();
            this.richTextBox1 = new FastColoredTextBoxNS.FastColoredTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.richTextBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // richTextBox2
            // 
            this.richTextBox2.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.richTextBox2.Location = new System.Drawing.Point(612, 12);
            this.richTextBox2.MinimumSize = new System.Drawing.Size(513, 426);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.ReadOnly = true;
            this.richTextBox2.Size = new System.Drawing.Size(513, 426);
            this.richTextBox2.TabIndex = 1;
            this.richTextBox2.Text = "";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 458);
            this.button1.MinimumSize = new System.Drawing.Size(75, 23);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Compile";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // richTextBox3
            // 
            this.richTextBox3.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.richTextBox3.Location = new System.Drawing.Point(119, 458);
            this.richTextBox3.MinimumSize = new System.Drawing.Size(1006, 96);
            this.richTextBox3.Name = "richTextBox3";
            this.richTextBox3.ReadOnly = true;
            this.richTextBox3.Size = new System.Drawing.Size(1006, 96);
            this.richTextBox3.TabIndex = 3;
            this.richTextBox3.Text = "";
            // 
            // richTextBox1
            // 
            this.richTextBox1.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.richTextBox1.AutoScrollMinSize = new System.Drawing.Size(31, 18);
            this.richTextBox1.BackBrush = null;
            this.richTextBox1.CharHeight = 18;
            this.richTextBox1.CharWidth = 10;
            this.richTextBox1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.richTextBox1.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.richTextBox1.IndentBackColor = System.Drawing.Color.LightGoldenrodYellow;
            this.richTextBox1.IsReplaceMode = false;
            this.richTextBox1.Location = new System.Drawing.Point(12, 12);
            this.richTextBox1.MinimumSize = new System.Drawing.Size(513, 426);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Paddings = new System.Windows.Forms.Padding(0);
            this.richTextBox1.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.richTextBox1.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("richTextBox1.ServiceColors")));
            this.richTextBox1.Size = new System.Drawing.Size(513, 426);
            this.richTextBox1.TabIndex = 4;
            this.richTextBox1.Zoom = 100;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightGoldenrodYellow;
            this.ClientSize = new System.Drawing.Size(1154, 569);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.richTextBox3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.richTextBox2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1172, 613);
            this.Name = "Form1";
            this.Text = "Comp";
            ((System.ComponentModel.ISupportInitialize)(this.richTextBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.RichTextBox richTextBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RichTextBox richTextBox3;
        private FastColoredTextBoxNS.FastColoredTextBox richTextBox1;
    }
}

