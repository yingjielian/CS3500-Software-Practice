namespace PS8
{
    partial class BoggleGame
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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.Player1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.Score1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.Player2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.Score2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.TextEntry = new System.Windows.Forms.ToolStripTextBox();
            this.TimeLeft = new System.Windows.Forms.ToolStripLabel();
            this.TimeLabel = new System.Windows.Forms.ToolStripLabel();
            this.Buttons = new System.Windows.Forms.FlowLayoutPanel();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.button12 = new System.Windows.Forms.Button();
            this.button13 = new System.Windows.Forms.Button();
            this.button14 = new System.Windows.Forms.Button();
            this.button15 = new System.Windows.Forms.Button();
            this.button16 = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.Buttons.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Player1,
            this.Score1,
            this.Player2,
            this.Score2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 423);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(368, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // Player1
            // 
            this.Player1.Name = "Player1";
            this.Player1.Size = new System.Drawing.Size(50, 17);
            this.Player1.Text = "Player_1";
            // 
            // Score1
            // 
            this.Score1.Name = "Score1";
            this.Score1.Size = new System.Drawing.Size(36, 17);
            this.Score1.Text = "Score";
            // 
            // Player2
            // 
            this.Player2.Name = "Player2";
            this.Player2.Size = new System.Drawing.Size(50, 17);
            this.Player2.Text = "Player_2";
            // 
            // Score2
            // 
            this.Score2.Name = "Score2";
            this.Score2.Size = new System.Drawing.Size(36, 17);
            this.Score2.Text = "Score";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.TextEntry,
            this.TimeLeft,
            this.TimeLabel});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(368, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(36, 22);
            this.toolStripLabel1.Text = "Word";
            // 
            // TextEntry
            // 
            this.TextEntry.Name = "TextEntry";
            this.TextEntry.Size = new System.Drawing.Size(130, 25);
            this.TextEntry.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextEntryClick);
            // 
            // TimeLeft
            // 
            this.TimeLeft.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.TimeLeft.Name = "TimeLeft";
            this.TimeLeft.Size = new System.Drawing.Size(28, 22);
            this.TimeLeft.Text = "0:00";
            // 
            // TimeLabel
            // 
            this.TimeLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.TimeLabel.Name = "TimeLabel";
            this.TimeLabel.Size = new System.Drawing.Size(37, 22);
            this.TimeLabel.Text = "Time:";
            // 
            // Buttons
            // 
            this.Buttons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Buttons.Controls.Add(this.button1);
            this.Buttons.Controls.Add(this.button2);
            this.Buttons.Controls.Add(this.button3);
            this.Buttons.Controls.Add(this.button4);
            this.Buttons.Controls.Add(this.button5);
            this.Buttons.Controls.Add(this.button6);
            this.Buttons.Controls.Add(this.button7);
            this.Buttons.Controls.Add(this.button8);
            this.Buttons.Controls.Add(this.button9);
            this.Buttons.Controls.Add(this.button10);
            this.Buttons.Controls.Add(this.button11);
            this.Buttons.Controls.Add(this.button12);
            this.Buttons.Controls.Add(this.button13);
            this.Buttons.Controls.Add(this.button14);
            this.Buttons.Controls.Add(this.button15);
            this.Buttons.Controls.Add(this.button16);
            this.Buttons.Location = new System.Drawing.Point(4, 52);
            this.Buttons.Name = "Buttons";
            this.Buttons.Size = new System.Drawing.Size(364, 364);
            this.Buttons.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(3, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(85, 85);
            this.button1.TabIndex = 0;
            this.button1.Text = "A";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(94, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(85, 85);
            this.button2.TabIndex = 1;
            this.button2.Text = "A";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Location = new System.Drawing.Point(185, 3);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(85, 85);
            this.button3.TabIndex = 2;
            this.button3.Text = "A";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.Location = new System.Drawing.Point(276, 3);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(85, 85);
            this.button4.TabIndex = 3;
            this.button4.Text = "A";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button5.Location = new System.Drawing.Point(3, 94);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(85, 85);
            this.button5.TabIndex = 4;
            this.button5.Text = "A";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            this.button6.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button6.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button6.Location = new System.Drawing.Point(94, 94);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(85, 85);
            this.button6.TabIndex = 5;
            this.button6.Text = "A";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // button7
            // 
            this.button7.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button7.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button7.Location = new System.Drawing.Point(185, 94);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(85, 85);
            this.button7.TabIndex = 6;
            this.button7.Text = "A";
            this.button7.UseVisualStyleBackColor = true;
            // 
            // button8
            // 
            this.button8.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button8.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button8.Location = new System.Drawing.Point(276, 94);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(85, 85);
            this.button8.TabIndex = 7;
            this.button8.Text = "A";
            this.button8.UseVisualStyleBackColor = true;
            // 
            // button9
            // 
            this.button9.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button9.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button9.Location = new System.Drawing.Point(3, 185);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(85, 85);
            this.button9.TabIndex = 8;
            this.button9.Text = "A";
            this.button9.UseVisualStyleBackColor = true;
            // 
            // button10
            // 
            this.button10.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button10.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button10.Location = new System.Drawing.Point(94, 185);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(85, 85);
            this.button10.TabIndex = 9;
            this.button10.Text = "A";
            this.button10.UseVisualStyleBackColor = true;
            // 
            // button11
            // 
            this.button11.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button11.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button11.Location = new System.Drawing.Point(185, 185);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(85, 85);
            this.button11.TabIndex = 10;
            this.button11.Text = "A";
            this.button11.UseVisualStyleBackColor = true;
            // 
            // button12
            // 
            this.button12.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button12.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button12.Location = new System.Drawing.Point(276, 185);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(85, 85);
            this.button12.TabIndex = 11;
            this.button12.Text = "A";
            this.button12.UseVisualStyleBackColor = true;
            // 
            // button13
            // 
            this.button13.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button13.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button13.Location = new System.Drawing.Point(3, 276);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(85, 85);
            this.button13.TabIndex = 12;
            this.button13.Text = "A";
            this.button13.UseVisualStyleBackColor = true;
            // 
            // button14
            // 
            this.button14.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button14.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button14.Location = new System.Drawing.Point(94, 276);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(85, 85);
            this.button14.TabIndex = 13;
            this.button14.Text = "A";
            this.button14.UseVisualStyleBackColor = true;
            // 
            // button15
            // 
            this.button15.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button15.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button15.Location = new System.Drawing.Point(185, 276);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(85, 85);
            this.button15.TabIndex = 14;
            this.button15.Text = "A";
            this.button15.UseVisualStyleBackColor = true;
            // 
            // button16
            // 
            this.button16.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button16.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button16.Location = new System.Drawing.Point(276, 276);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(85, 85);
            this.button16.TabIndex = 15;
            this.button16.Text = "A";
            this.button16.UseVisualStyleBackColor = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(368, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // BoggleGame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(368, 445);
            this.Controls.Add(this.Buttons);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Name = "BoggleGame";
            this.Text = "Boggle";
            this.Load += new System.EventHandler(this.BoggleG);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.Buttons.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel Player1;
        private System.Windows.Forms.ToolStripStatusLabel Score1;
        private System.Windows.Forms.ToolStripStatusLabel Player2;
        private System.Windows.Forms.ToolStripStatusLabel Score2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripTextBox TextEntry;
        private System.Windows.Forms.ToolStripLabel TimeLabel;
        private System.Windows.Forms.ToolStripLabel TimeLeft;
        private System.Windows.Forms.FlowLayoutPanel Buttons;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.Button button14;
        private System.Windows.Forms.Button button15;
        private System.Windows.Forms.Button button16;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
    }
}