namespace Minecraft_Server_Controll
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private Button button1;
        private Button button7;
        private TextBox textBox2;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            button1 = new Button();
            button7 = new Button();
            textBox2 = new TextBox();
            textBox1 = new TextBox();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            button5 = new Button();
            button6 = new Button();
            button8 = new Button();
            button9 = new Button();
            button10 = new Button();
            button11 = new Button();
            label1 = new Label();
            checkBox1 = new CheckBox();
            checkBox2 = new CheckBox();
            label2 = new Label();
            labelNextRestart = new Label();
            label3 = new Label();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button1.Location = new Point(812, 38);
            button1.Name = "button1";
            button1.Size = new Size(137, 25);
            button1.TabIndex = 0;
            button1.Text = "Start";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button7
            // 
            button7.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button7.Location = new Point(975, 38);
            button7.Name = "button7";
            button7.Size = new Size(137, 25);
            button7.TabIndex = 0;
            button7.Text = "Stop";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // textBox2
            // 
            textBox2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBox2.Location = new Point(12, 40);
            textBox2.Multiline = true;
            textBox2.Name = "textBox2";
            textBox2.ReadOnly = true;
            textBox2.ScrollBars = ScrollBars.Vertical;
            textBox2.Size = new Size(786, 464);
            textBox2.TabIndex = 17;
            // 
            // textBox1
            // 
            textBox1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBox1.Location = new Point(12, 563);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(673, 23);
            textBox1.TabIndex = 18;
            textBox1.KeyDown += Form1_KeyDown;
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button2.Location = new Point(726, 561);
            button2.Name = "button2";
            button2.Size = new Size(72, 25);
            button2.TabIndex = 19;
            button2.Text = "Senden";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button3.Location = new Point(812, 69);
            button3.Name = "button3";
            button3.Size = new Size(137, 25);
            button3.TabIndex = 20;
            button3.Text = "Neustart";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button4.Location = new Point(975, 69);
            button4.Name = "button4";
            button4.Size = new Size(137, 25);
            button4.TabIndex = 21;
            button4.Text = "Sichern";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button5
            // 
            button5.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button5.Location = new Point(812, 100);
            button5.Name = "button5";
            button5.Size = new Size(137, 25);
            button5.TabIndex = 22;
            button5.Text = "MCC Option";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // button6
            // 
            button6.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button6.Location = new Point(975, 100);
            button6.Name = "button6";
            button6.Size = new Size(137, 25);
            button6.TabIndex = 23;
            button6.Text = "Server Einstellung";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // button8
            // 
            button8.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button8.Location = new Point(812, 131);
            button8.Name = "button8";
            button8.Size = new Size(137, 25);
            button8.TabIndex = 24;
            button8.Text = "Whitelist Neuladen";
            button8.UseVisualStyleBackColor = true;
            button8.Click += button8_Click;
            // 
            // button9
            // 
            button9.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button9.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            button9.Location = new Point(975, 131);
            button9.Name = "button9";
            button9.Size = new Size(137, 25);
            button9.TabIndex = 25;
            button9.Text = "MCC Neuladen";
            button9.TextAlign = ContentAlignment.TopCenter;
            button9.UseVisualStyleBackColor = true;
            button9.Click += button9_Click;
            // 
            // button10
            // 
            button10.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button10.Location = new Point(812, 162);
            button10.Name = "button10";
            button10.Size = new Size(137, 25);
            button10.TabIndex = 26;
            button10.Text = "Logs Anschauen";
            button10.UseVisualStyleBackColor = true;
            button10.Click += button10_Click;
            // 
            // button11
            // 
            button11.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button11.Location = new Point(975, 162);
            button11.Name = "button11";
            button11.Size = new Size(137, 25);
            button11.TabIndex = 27;
            button11.Text = "BackUp Ordner";
            button11.UseVisualStyleBackColor = true;
            button11.Click += button11_Click;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(815, 209);
            label1.Name = "label1";
            label1.Size = new Size(297, 15);
            label1.TabIndex = 28;
            label1.Text = "----------------------------------------------------------";
            // 
            // checkBox1
            // 
            checkBox1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(996, 239);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(116, 19);
            checkBox1.TabIndex = 29;
            checkBox1.Text = "Keep Alive Server";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // checkBox2
            // 
            checkBox2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            checkBox2.AutoSize = true;
            checkBox2.Location = new Point(832, 239);
            checkBox2.Name = "checkBox2";
            checkBox2.Size = new Size(117, 19);
            checkBox2.TabIndex = 30;
            checkBox2.Text = "AutoSave System";
            checkBox2.UseVisualStyleBackColor = true;
            checkBox2.CheckedChanged += checkBox2_CheckedChanged;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Location = new Point(804, 307);
            label2.Name = "label2";
            label2.Size = new Size(38, 15);
            label2.TabIndex = 31;
            label2.Text = "label2";
            // 
            // labelNextRestart
            // 
            labelNextRestart.Location = new Point(0, 0);
            labelNextRestart.Name = "labelNextRestart";
            labelNextRestart.Size = new Size(100, 23);
            labelNextRestart.TabIndex = 0;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label3.AutoSize = true;
            label3.Location = new Point(812, 272);
            label3.Name = "label3";
            label3.Size = new Size(297, 15);
            label3.TabIndex = 32;
            label3.Text = "----------------------------------------------------------";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1124, 611);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(checkBox2);
            Controls.Add(checkBox1);
            Controls.Add(label1);
            Controls.Add(button11);
            Controls.Add(button10);
            Controls.Add(button9);
            Controls.Add(button8);
            Controls.Add(button6);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(textBox1);
            Controls.Add(button7);
            Controls.Add(textBox2);
            Controls.Add(button1);
            Cursor = Cursors.Default;
            Name = "Form1";
            Text = "MineServer Controll";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        private TextBox textBox1;
        private Button button2;
        private Button button3;
        private Button button4;
        private Button button5;
        private Button button6;
        private Button button8;
        private Button button9;
        private Button button10;
        private Button button11;
        private Label label1;
        private CheckBox checkBox1;
        private CheckBox checkBox2;
        private Label label2;
        private Label labelNextRestart;
        private Label label3;
    }
}
