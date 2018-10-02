using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PS8
{
    partial class BoggleGUI
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Name = new System.Windows.Forms.Label();
            this.NameBox = new System.Windows.Forms.TextBox();
            this.Register = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.ServiceURL = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.button8 = new System.Windows.Forms.Button();
            this.button28 = new System.Windows.Forms.Button();
            this.button27 = new System.Windows.Forms.Button();
            this.button26 = new System.Windows.Forms.Button();
            this.button25 = new System.Windows.Forms.Button();
            this.button24 = new System.Windows.Forms.Button();
            this.button12 = new System.Windows.Forms.Button();
            this.button23 = new System.Windows.Forms.Button();
            this.button22 = new System.Windows.Forms.Button();
            this.button21 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button20 = new System.Windows.Forms.Button();
            this.button19 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button18 = new System.Windows.Forms.Button();
            this.button17 = new System.Windows.Forms.Button();
            this.Play = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(941, 24);
            this.menuStrip1.TabIndex = 19;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // Name
            // 
            this.Name.AutoSize = true;
            this.Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name.Location = new System.Drawing.Point(12, 90);
            this.Name.Name = "Name";
            this.Name.Size = new System.Drawing.Size(95, 24);
            this.Name.TabIndex = 20;
            this.Name.Text = "Nickname";
            this.Name.Click += new System.EventHandler(this.Name_Click);
            // 
            // NameBox
            // 
            this.NameBox.Location = new System.Drawing.Point(113, 90);
            this.NameBox.Name = "NameBox";
            this.NameBox.Size = new System.Drawing.Size(127, 20);
            this.NameBox.TabIndex = 21;
            // 
            // Register
            // 
            this.Register.Location = new System.Drawing.Point(440, 27);
            this.Register.Name = "Register";
            this.Register.Size = new System.Drawing.Size(125, 44);
            this.Register.TabIndex = 22;
            this.Register.Text = "Register";
            this.Register.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 24);
            this.label1.TabIndex = 23;
            this.label1.Text = "Domain Server";
            this.label1.Click += new System.EventHandler(this.label1_Click_1);
            // 
            // ServiceURL
            // 
            this.ServiceURL.Location = new System.Drawing.Point(153, 39);
            this.ServiceURL.Name = "ServiceURL";
            this.ServiceURL.Size = new System.Drawing.Size(281, 20);
            this.ServiceURL.TabIndex = 24;
            this.ServiceURL.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(691, 27);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(125, 47);
            this.button1.TabIndex = 25;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(16, 141);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 88.33334F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 47F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(355, 381);
            this.tableLayoutPanel1.TabIndex = 26;
            this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(569, 111);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 37);
            this.label2.TabIndex = 27;
            this.label2.Text = "Time";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(546, 161);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(95, 84);
            this.button8.TabIndex = 35;
            this.button8.Text = "button8";
            this.button8.UseVisualStyleBackColor = true;
            // 
            // button28
            // 
            this.button28.Location = new System.Drawing.Point(640, 425);
            this.button28.Name = "button28";
            this.button28.Size = new System.Drawing.Size(95, 84);
            this.button28.TabIndex = 43;
            this.button28.Text = "button16";
            this.button28.UseVisualStyleBackColor = true;
            // 
            // button27
            // 
            this.button27.Location = new System.Drawing.Point(640, 337);
            this.button27.Name = "button27";
            this.button27.Size = new System.Drawing.Size(95, 84);
            this.button27.TabIndex = 42;
            this.button27.Text = "button15";
            this.button27.UseVisualStyleBackColor = true;
            // 
            // button26
            // 
            this.button26.Location = new System.Drawing.Point(734, 337);
            this.button26.Name = "button26";
            this.button26.Size = new System.Drawing.Size(95, 84);
            this.button26.TabIndex = 41;
            this.button26.Text = "button14";
            this.button26.UseVisualStyleBackColor = true;
            // 
            // button25
            // 
            this.button25.Location = new System.Drawing.Point(828, 425);
            this.button25.Name = "button25";
            this.button25.Size = new System.Drawing.Size(95, 84);
            this.button25.TabIndex = 40;
            this.button25.Text = "button13";
            this.button25.UseVisualStyleBackColor = true;
            // 
            // button24
            // 
            this.button24.Location = new System.Drawing.Point(734, 425);
            this.button24.Name = "button24";
            this.button24.Size = new System.Drawing.Size(95, 84);
            this.button24.TabIndex = 38;
            this.button24.Text = "button11";
            this.button24.UseVisualStyleBackColor = true;
            // 
            // button12
            // 
            this.button12.Location = new System.Drawing.Point(828, 161);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(95, 84);
            this.button12.TabIndex = 39;
            this.button12.Text = "button12";
            this.button12.UseVisualStyleBackColor = true;
            // 
            // button23
            // 
            this.button23.Location = new System.Drawing.Point(546, 425);
            this.button23.Name = "button23";
            this.button23.Size = new System.Drawing.Size(95, 84);
            this.button23.TabIndex = 37;
            this.button23.Text = "button10";
            this.button23.UseVisualStyleBackColor = true;
            // 
            // button22
            // 
            this.button22.Location = new System.Drawing.Point(734, 249);
            this.button22.Name = "button22";
            this.button22.Size = new System.Drawing.Size(95, 84);
            this.button22.TabIndex = 36;
            this.button22.Text = "button9";
            this.button22.UseVisualStyleBackColor = true;
            // 
            // button21
            // 
            this.button21.Location = new System.Drawing.Point(640, 249);
            this.button21.Name = "button21";
            this.button21.Size = new System.Drawing.Size(95, 84);
            this.button21.TabIndex = 33;
            this.button21.Text = "button6";
            this.button21.UseVisualStyleBackColor = true;
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(734, 161);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(95, 84);
            this.button7.TabIndex = 34;
            this.button7.Text = "button7";
            this.button7.UseVisualStyleBackColor = true;
            // 
            // button20
            // 
            this.button20.Location = new System.Drawing.Point(828, 249);
            this.button20.Name = "button20";
            this.button20.Size = new System.Drawing.Size(95, 84);
            this.button20.TabIndex = 32;
            this.button20.Text = "button5";
            this.button20.UseVisualStyleBackColor = true;
            // 
            // button19
            // 
            this.button19.Location = new System.Drawing.Point(828, 337);
            this.button19.Name = "button19";
            this.button19.Size = new System.Drawing.Size(95, 84);
            this.button19.TabIndex = 30;
            this.button19.Text = "button2";
            this.button19.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(640, 161);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(95, 84);
            this.button3.TabIndex = 31;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button18
            // 
            this.button18.Location = new System.Drawing.Point(546, 249);
            this.button18.Name = "button18";
            this.button18.Size = new System.Drawing.Size(95, 84);
            this.button18.TabIndex = 29;
            this.button18.Text = "button1";
            this.button18.UseVisualStyleBackColor = true;
            // 
            // button17
            // 
            this.button17.Location = new System.Drawing.Point(546, 337);
            this.button17.Name = "button17";
            this.button17.Size = new System.Drawing.Size(95, 84);
            this.button17.TabIndex = 28;
            this.button17.Text = "button4";
            this.button17.UseVisualStyleBackColor = true;
            // 
            // Play
            // 
            this.Play.Location = new System.Drawing.Point(246, 80);
            this.Play.Name = "Play";
            this.Play.Size = new System.Drawing.Size(125, 49);
            this.Play.TabIndex = 44;
            this.Play.Text = "Play";
            this.Play.UseVisualStyleBackColor = true;
            // 
            // BoggleGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(941, 569);
            this.Controls.Add(this.Play);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button28);
            this.Controls.Add(this.button27);
            this.Controls.Add(this.button26);
            this.Controls.Add(this.button25);
            this.Controls.Add(this.button24);
            this.Controls.Add(this.button12);
            this.Controls.Add(this.button23);
            this.Controls.Add(this.button22);
            this.Controls.Add(this.button21);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button20);
            this.Controls.Add(this.button19);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button18);
            this.Controls.Add(this.button17);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ServiceURL);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Register);
            this.Controls.Add(this.NameBox);
            this.Controls.Add(this.Name);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "BoggleGUI";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.BoggleGUI_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.Label Name;
        private System.Windows.Forms.TextBox NameBox;
        private System.Windows.Forms.Button Register;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ServiceURL;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button28;
        private System.Windows.Forms.Button button27;
        private System.Windows.Forms.Button button26;
        private System.Windows.Forms.Button button25;
        private System.Windows.Forms.Button button24;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Button button23;
        private System.Windows.Forms.Button button22;
        private System.Windows.Forms.Button button21;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button20;
        private System.Windows.Forms.Button button19;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button18;
        private System.Windows.Forms.Button button17;
        private System.Windows.Forms.Button Play;
    }
}

