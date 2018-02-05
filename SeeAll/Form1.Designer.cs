namespace SeeAll
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.startWorkIndexBtn = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.writeIndexLabel = new System.Windows.Forms.Label();
            this.readIndexLabel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.connectToSqlIndexBtn = new System.Windows.Forms.Button();
            this.connectToCpuIndexBtn = new System.Windows.Forms.Button();
            this.runIndexBtn = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.idIndexLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.settingsWorkIndexBtn = new System.Windows.Forms.Button();
            this.startAutomaticallyCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // startWorkIndexBtn
            // 
            this.startWorkIndexBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.startWorkIndexBtn.Location = new System.Drawing.Point(12, 146);
            this.startWorkIndexBtn.Name = "startWorkIndexBtn";
            this.startWorkIndexBtn.Size = new System.Drawing.Size(250, 104);
            this.startWorkIndexBtn.TabIndex = 0;
            this.startWorkIndexBtn.Text = "Start";
            this.startWorkIndexBtn.UseVisualStyleBackColor = true;
            this.startWorkIndexBtn.Click += new System.EventHandler(this.startWorkIndexBtn_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.writeIndexLabel);
            this.groupBox2.Controls.Add(this.readIndexLabel);
            this.groupBox2.Location = new System.Drawing.Point(121, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(137, 33);
            this.groupBox2.TabIndex = 20;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "position CPU";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(81, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(18, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "R:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(21, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "W:";
            // 
            // writeIndexLabel
            // 
            this.writeIndexLabel.AutoSize = true;
            this.writeIndexLabel.Location = new System.Drawing.Point(29, 16);
            this.writeIndexLabel.Name = "writeIndexLabel";
            this.writeIndexLabel.Size = new System.Drawing.Size(13, 13);
            this.writeIndexLabel.TabIndex = 10;
            this.writeIndexLabel.Text = "0";
            // 
            // readIndexLabel
            // 
            this.readIndexLabel.AutoSize = true;
            this.readIndexLabel.Location = new System.Drawing.Point(100, 16);
            this.readIndexLabel.Name = "readIndexLabel";
            this.readIndexLabel.Size = new System.Drawing.Size(13, 13);
            this.readIndexLabel.TabIndex = 11;
            this.readIndexLabel.Text = "0";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.connectToSqlIndexBtn);
            this.groupBox1.Controls.Add(this.connectToCpuIndexBtn);
            this.groupBox1.Controls.Add(this.runIndexBtn);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(102, 104);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "online";
            // 
            // connectToSqlIndexBtn
            // 
            this.connectToSqlIndexBtn.Location = new System.Drawing.Point(6, 74);
            this.connectToSqlIndexBtn.Name = "connectToSqlIndexBtn";
            this.connectToSqlIndexBtn.Size = new System.Drawing.Size(89, 23);
            this.connectToSqlIndexBtn.TabIndex = 10;
            this.connectToSqlIndexBtn.Text = "Connect SQL";
            this.connectToSqlIndexBtn.UseVisualStyleBackColor = true;
            // 
            // connectToCpuIndexBtn
            // 
            this.connectToCpuIndexBtn.Location = new System.Drawing.Point(6, 45);
            this.connectToCpuIndexBtn.Name = "connectToCpuIndexBtn";
            this.connectToCpuIndexBtn.Size = new System.Drawing.Size(89, 23);
            this.connectToCpuIndexBtn.TabIndex = 9;
            this.connectToCpuIndexBtn.Text = "Connect CPU";
            this.connectToCpuIndexBtn.UseVisualStyleBackColor = true;
            // 
            // runIndexBtn
            // 
            this.runIndexBtn.Location = new System.Drawing.Point(6, 16);
            this.runIndexBtn.Name = "runIndexBtn";
            this.runIndexBtn.Size = new System.Drawing.Size(90, 23);
            this.runIndexBtn.TabIndex = 8;
            this.runIndexBtn.Text = "Run";
            this.runIndexBtn.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(199, 120);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 20);
            this.label3.TabIndex = 18;
            this.label3.Text = "->  SQL";
            // 
            // idIndexLabel
            // 
            this.idIndexLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.idIndexLabel.AutoSize = true;
            this.idIndexLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.idIndexLabel.ForeColor = System.Drawing.Color.Blue;
            this.idIndexLabel.Location = new System.Drawing.Point(77, 120);
            this.idIndexLabel.Name = "idIndexLabel";
            this.idIndexLabel.Size = new System.Drawing.Size(120, 17);
            this.idIndexLabel.TabIndex = 17;
            this.idIndexLabel.Text = "00000000000000";
            this.idIndexLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(10, 120);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 20);
            this.label1.TabIndex = 16;
            this.label1.Text = "CPU  ->";
            // 
            // settingsWorkIndexBtn
            // 
            this.settingsWorkIndexBtn.Location = new System.Drawing.Point(121, 74);
            this.settingsWorkIndexBtn.Name = "settingsWorkIndexBtn";
            this.settingsWorkIndexBtn.Size = new System.Drawing.Size(137, 42);
            this.settingsWorkIndexBtn.TabIndex = 15;
            this.settingsWorkIndexBtn.Text = "Settings";
            this.settingsWorkIndexBtn.UseVisualStyleBackColor = true;
            this.settingsWorkIndexBtn.Click += new System.EventHandler(this.settingsWorkIndexBtn_Click);
            // 
            // startAutomaticallyCheckBox
            // 
            this.startAutomaticallyCheckBox.AutoSize = true;
            this.startAutomaticallyCheckBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.startAutomaticallyCheckBox.Location = new System.Drawing.Point(121, 51);
            this.startAutomaticallyCheckBox.Name = "startAutomaticallyCheckBox";
            this.startAutomaticallyCheckBox.Size = new System.Drawing.Size(112, 17);
            this.startAutomaticallyCheckBox.TabIndex = 21;
            this.startAutomaticallyCheckBox.Text = "Start automatically";
            this.startAutomaticallyCheckBox.UseVisualStyleBackColor = true;
            this.startAutomaticallyCheckBox.CheckedChanged += new System.EventHandler(this.startAutomaticallyCheckBox_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(274, 262);
            this.Controls.Add(this.startAutomaticallyCheckBox);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.idIndexLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.settingsWorkIndexBtn);
            this.Controls.Add(this.startWorkIndexBtn);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button startWorkIndexBtn;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label writeIndexLabel;
        private System.Windows.Forms.Label readIndexLabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label idIndexLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button settingsWorkIndexBtn;
        private System.Windows.Forms.Button runIndexBtn;
        private System.Windows.Forms.Button connectToCpuIndexBtn;
        private System.Windows.Forms.Button connectToSqlIndexBtn;
        private System.Windows.Forms.CheckBox startAutomaticallyCheckBox;
    }
}

