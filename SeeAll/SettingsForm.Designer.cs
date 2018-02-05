namespace SeeAll
{
    partial class SettingsForm
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
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.readSettingBtn = new System.Windows.Forms.Button();
            this.readSetLabel = new System.Windows.Forms.TextBox();
            this.writeSetLabel = new System.Windows.Forms.Label();
            this.showSettindBtn = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.saveBtn = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.errViewTextBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.shiftTransitionTextBox = new System.Windows.Forms.TextBox();
            this.dbDatetimeCpuTextBox = new System.Windows.Forms.TextBox();
            this.dbLimitCpuTextBox = new System.Windows.Forms.TextBox();
            this.shiftTransitionLabel = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.timerMicroDowntimeTextBox = new System.Windows.Forms.TextBox();
            this.timerStandartCycleTextBox = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.timeSleepExceptionBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.demoCpuBtn = new System.Windows.Forms.Button();
            this.checkConnCpuBtn = new System.Windows.Forms.Button();
            this.slotCpuTextBox = new System.Windows.Forms.TextBox();
            this.rackCpuTextBox = new System.Windows.Forms.TextBox();
            this.ipTypeTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cpuTypeComboBox = new System.Windows.Forms.ComboBox();
            this.checkConnBdBtn = new System.Windows.Forms.Button();
            this.autoRunWincheckBox = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.dateTimeSettingsLabel = new System.Windows.Forms.Label();
            this.label44 = new System.Windows.Forms.Label();
            this.startByteAdrSettings = new System.Windows.Forms.TextBox();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.readSettingBtn);
            this.groupBox4.Controls.Add(this.readSetLabel);
            this.groupBox4.Controls.Add(this.writeSetLabel);
            this.groupBox4.Controls.Add(this.showSettindBtn);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Location = new System.Drawing.Point(593, 23);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(183, 90);
            this.groupBox4.TabIndex = 10;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Create limit CPU (position)";
            // 
            // readSettingBtn
            // 
            this.readSettingBtn.BackColor = System.Drawing.Color.Red;
            this.readSettingBtn.Location = new System.Drawing.Point(126, 35);
            this.readSettingBtn.Name = "readSettingBtn";
            this.readSettingBtn.Size = new System.Drawing.Size(51, 23);
            this.readSettingBtn.TabIndex = 5;
            this.readSettingBtn.Text = "Correct";
            this.readSettingBtn.UseVisualStyleBackColor = false;
            this.readSettingBtn.Click += new System.EventHandler(this.readSettingBtn_Click);
            // 
            // readSetLabel
            // 
            this.readSetLabel.Location = new System.Drawing.Point(44, 35);
            this.readSetLabel.Name = "readSetLabel";
            this.readSetLabel.Size = new System.Drawing.Size(76, 20);
            this.readSetLabel.TabIndex = 4;
            this.readSetLabel.Text = "0";
            // 
            // writeSetLabel
            // 
            this.writeSetLabel.AutoSize = true;
            this.writeSetLabel.Location = new System.Drawing.Point(44, 21);
            this.writeSetLabel.Name = "writeSetLabel";
            this.writeSetLabel.Size = new System.Drawing.Size(13, 13);
            this.writeSetLabel.TabIndex = 3;
            this.writeSetLabel.Text = "0";
            // 
            // showSettindBtn
            // 
            this.showSettindBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.showSettindBtn.Location = new System.Drawing.Point(6, 61);
            this.showSettindBtn.Name = "showSettindBtn";
            this.showSettindBtn.Size = new System.Drawing.Size(171, 23);
            this.showSettindBtn.TabIndex = 2;
            this.showSettindBtn.Text = "Show";
            this.showSettindBtn.UseVisualStyleBackColor = false;
            this.showSettindBtn.Click += new System.EventHandler(this.showSettindBtn_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(7, 42);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(36, 13);
            this.label9.TabIndex = 1;
            this.label9.Text = "Read:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 21);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "Write:";
            // 
            // saveBtn
            // 
            this.saveBtn.Location = new System.Drawing.Point(100, 251);
            this.saveBtn.Name = "saveBtn";
            this.saveBtn.Size = new System.Drawing.Size(676, 31);
            this.saveBtn.TabIndex = 9;
            this.saveBtn.Text = "Save";
            this.saveBtn.UseVisualStyleBackColor = true;
            this.saveBtn.Click += new System.EventHandler(this.saveBtn_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.errViewTextBox);
            this.groupBox3.Location = new System.Drawing.Point(12, 288);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(764, 162);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Err";
            // 
            // errViewTextBox
            // 
            this.errViewTextBox.Location = new System.Drawing.Point(10, 19);
            this.errViewTextBox.Multiline = true;
            this.errViewTextBox.Name = "errViewTextBox";
            this.errViewTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.errViewTextBox.Size = new System.Drawing.Size(748, 135);
            this.errViewTextBox.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.shiftTransitionTextBox);
            this.groupBox1.Controls.Add(this.dbDatetimeCpuTextBox);
            this.groupBox1.Controls.Add(this.dbLimitCpuTextBox);
            this.groupBox1.Controls.Add(this.shiftTransitionLabel);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.textBox3);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.timerMicroDowntimeTextBox);
            this.groupBox1.Controls.Add(this.timerStandartCycleTextBox);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.timeSleepExceptionBox);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.demoCpuBtn);
            this.groupBox1.Controls.Add(this.checkConnCpuBtn);
            this.groupBox1.Controls.Add(this.slotCpuTextBox);
            this.groupBox1.Controls.Add(this.rackCpuTextBox);
            this.groupBox1.Controls.Add(this.ipTypeTextBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cpuTypeComboBox);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(575, 221);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "CPU connection settings";
            // 
            // shiftTransitionTextBox
            // 
            this.shiftTransitionTextBox.Location = new System.Drawing.Point(419, 154);
            this.shiftTransitionTextBox.Name = "shiftTransitionTextBox";
            this.shiftTransitionTextBox.Size = new System.Drawing.Size(141, 23);
            this.shiftTransitionTextBox.TabIndex = 31;
            this.shiftTransitionTextBox.Text = "7,15,23";
            // 
            // dbDatetimeCpuTextBox
            // 
            this.dbDatetimeCpuTextBox.Location = new System.Drawing.Point(165, 183);
            this.dbDatetimeCpuTextBox.Name = "dbDatetimeCpuTextBox";
            this.dbDatetimeCpuTextBox.Size = new System.Drawing.Size(121, 23);
            this.dbDatetimeCpuTextBox.TabIndex = 30;
            this.dbDatetimeCpuTextBox.Text = "2";
            // 
            // dbLimitCpuTextBox
            // 
            this.dbLimitCpuTextBox.Location = new System.Drawing.Point(165, 154);
            this.dbLimitCpuTextBox.Name = "dbLimitCpuTextBox";
            this.dbLimitCpuTextBox.Size = new System.Drawing.Size(121, 23);
            this.dbLimitCpuTextBox.TabIndex = 29;
            this.dbLimitCpuTextBox.Text = "3";
            // 
            // shiftTransitionLabel
            // 
            this.shiftTransitionLabel.AutoSize = true;
            this.shiftTransitionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.shiftTransitionLabel.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.shiftTransitionLabel.Location = new System.Drawing.Point(302, 157);
            this.shiftTransitionLabel.Name = "shiftTransitionLabel";
            this.shiftTransitionLabel.Size = new System.Drawing.Size(119, 20);
            this.shiftTransitionLabel.TabIndex = 28;
            this.shiftTransitionLabel.Text = "Shift Transition:";
            this.shiftTransitionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label14.Location = new System.Drawing.Point(11, 186);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(139, 20);
            this.label14.TabIndex = 27;
            this.label14.Text = "DB datetime CPU:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label13.Location = new System.Drawing.Point(11, 157);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(104, 20);
            this.label13.TabIndex = 26;
            this.label13.Text = "DB limit CPU:";
            // 
            // textBox3
            // 
            this.textBox3.Enabled = false;
            this.textBox3.Location = new System.Drawing.Point(517, 96);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(43, 23);
            this.textBox3.TabIndex = 25;
            this.textBox3.Text = "9";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Enabled = false;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label12.Location = new System.Drawing.Point(301, 96);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(214, 20);
            this.label12.TabIndex = 24;
            this.label12.Text = "Threshold of response (sec.):";
            // 
            // timerMicroDowntimeTextBox
            // 
            this.timerMicroDowntimeTextBox.Location = new System.Drawing.Point(165, 124);
            this.timerMicroDowntimeTextBox.Name = "timerMicroDowntimeTextBox";
            this.timerMicroDowntimeTextBox.Size = new System.Drawing.Size(121, 23);
            this.timerMicroDowntimeTextBox.TabIndex = 23;
            this.timerMicroDowntimeTextBox.Text = "120";
            // 
            // timerStandartCycleTextBox
            // 
            this.timerStandartCycleTextBox.Location = new System.Drawing.Point(165, 96);
            this.timerStandartCycleTextBox.Name = "timerStandartCycleTextBox";
            this.timerStandartCycleTextBox.Size = new System.Drawing.Size(121, 23);
            this.timerStandartCycleTextBox.TabIndex = 22;
            this.timerStandartCycleTextBox.Text = "60";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label11.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label11.Location = new System.Drawing.Point(7, 124);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(144, 20);
            this.label11.TabIndex = 21;
            this.label11.Text = "Micro-simple (sec.):";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label10.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label10.Location = new System.Drawing.Point(7, 96);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(128, 20);
            this.label10.TabIndex = 20;
            this.label10.Text = "Cycle time (sec.):";
            // 
            // timeSleepExceptionBox
            // 
            this.timeSleepExceptionBox.Location = new System.Drawing.Point(438, 54);
            this.timeSleepExceptionBox.Name = "timeSleepExceptionBox";
            this.timeSleepExceptionBox.Size = new System.Drawing.Size(122, 23);
            this.timeSleepExceptionBox.TabIndex = 19;
            this.timeSleepExceptionBox.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(429, 31);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(146, 20);
            this.label5.TabIndex = 18;
            this.label5.Text = "Time Exception, ms";
            // 
            // demoCpuBtn
            // 
            this.demoCpuBtn.Location = new System.Drawing.Point(6, 58);
            this.demoCpuBtn.Name = "demoCpuBtn";
            this.demoCpuBtn.Size = new System.Drawing.Size(75, 36);
            this.demoCpuBtn.TabIndex = 17;
            this.demoCpuBtn.Text = "Demo";
            this.demoCpuBtn.UseVisualStyleBackColor = true;
            this.demoCpuBtn.Click += new System.EventHandler(this.demoCpuBtn_Click);
            // 
            // checkConnCpuBtn
            // 
            this.checkConnCpuBtn.BackColor = System.Drawing.SystemColors.Control;
            this.checkConnCpuBtn.Location = new System.Drawing.Point(6, 20);
            this.checkConnCpuBtn.Name = "checkConnCpuBtn";
            this.checkConnCpuBtn.Size = new System.Drawing.Size(75, 36);
            this.checkConnCpuBtn.TabIndex = 16;
            this.checkConnCpuBtn.Text = "Check";
            this.checkConnCpuBtn.UseVisualStyleBackColor = false;
            this.checkConnCpuBtn.Click += new System.EventHandler(this.checkConnCpuBtn_Click);
            // 
            // slotCpuTextBox
            // 
            this.slotCpuTextBox.Location = new System.Drawing.Point(385, 65);
            this.slotCpuTextBox.Name = "slotCpuTextBox";
            this.slotCpuTextBox.Size = new System.Drawing.Size(27, 23);
            this.slotCpuTextBox.TabIndex = 7;
            this.slotCpuTextBox.Text = "1";
            // 
            // rackCpuTextBox
            // 
            this.rackCpuTextBox.Location = new System.Drawing.Point(385, 24);
            this.rackCpuTextBox.Name = "rackCpuTextBox";
            this.rackCpuTextBox.Size = new System.Drawing.Size(27, 23);
            this.rackCpuTextBox.TabIndex = 6;
            this.rackCpuTextBox.Text = "0";
            // 
            // ipTypeTextBox
            // 
            this.ipTypeTextBox.Location = new System.Drawing.Point(165, 68);
            this.ipTypeTextBox.Name = "ipTypeTextBox";
            this.ipTypeTextBox.Size = new System.Drawing.Size(121, 23);
            this.ipTypeTextBox.TabIndex = 5;
            this.ipTypeTextBox.Text = "192.168.0.1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(301, 68);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 20);
            this.label4.TabIndex = 4;
            this.label4.Text = "Slot Cpu";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(301, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 20);
            this.label3.TabIndex = 3;
            this.label3.Text = "Rack Cpu";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(87, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Ip Cpu";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(87, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "CpuType";
            // 
            // cpuTypeComboBox
            // 
            this.cpuTypeComboBox.FormattingEnabled = true;
            this.cpuTypeComboBox.Items.AddRange(new object[] {
            "S71200",
            "S71500",
            "S7200",
            "S7300",
            "S7400"});
            this.cpuTypeComboBox.Location = new System.Drawing.Point(165, 27);
            this.cpuTypeComboBox.Name = "cpuTypeComboBox";
            this.cpuTypeComboBox.Size = new System.Drawing.Size(121, 24);
            this.cpuTypeComboBox.TabIndex = 0;
            // 
            // checkConnBdBtn
            // 
            this.checkConnBdBtn.BackColor = System.Drawing.SystemColors.Control;
            this.checkConnBdBtn.Location = new System.Drawing.Point(19, 239);
            this.checkConnBdBtn.Name = "checkConnBdBtn";
            this.checkConnBdBtn.Size = new System.Drawing.Size(75, 43);
            this.checkConnBdBtn.TabIndex = 17;
            this.checkConnBdBtn.Text = "Check SQL";
            this.checkConnBdBtn.UseVisualStyleBackColor = false;
            this.checkConnBdBtn.Click += new System.EventHandler(this.checkConnBdBtn_Click);
            // 
            // autoRunWincheckBox
            // 
            this.autoRunWincheckBox.AutoSize = true;
            this.autoRunWincheckBox.Location = new System.Drawing.Point(10, 19);
            this.autoRunWincheckBox.Name = "autoRunWincheckBox";
            this.autoRunWincheckBox.Size = new System.Drawing.Size(87, 17);
            this.autoRunWincheckBox.TabIndex = 0;
            this.autoRunWincheckBox.Text = "auto run Win";
            this.autoRunWincheckBox.UseVisualStyleBackColor = true;
            this.autoRunWincheckBox.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 39);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(73, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "true - auto run";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(86, 39);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(92, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "false - no auto run";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Firebrick;
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.autoRunWincheckBox);
            this.groupBox2.Location = new System.Drawing.Point(590, 113);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(183, 59);
            this.groupBox2.TabIndex = 18;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "system";
            this.groupBox2.Visible = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(593, 217);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(183, 28);
            this.button1.TabIndex = 25;
            this.button1.Text = "Show";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dateTimeSettingsLabel
            // 
            this.dateTimeSettingsLabel.AutoSize = true;
            this.dateTimeSettingsLabel.Location = new System.Drawing.Point(599, 200);
            this.dateTimeSettingsLabel.Name = "dateTimeSettingsLabel";
            this.dateTimeSettingsLabel.Size = new System.Drawing.Size(13, 13);
            this.dateTimeSettingsLabel.TabIndex = 24;
            this.dateTimeSettingsLabel.Text = "0";
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Location = new System.Drawing.Point(593, 176);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(67, 13);
            this.label44.TabIndex = 23;
            this.label44.Text = "startByteAdr:";
            // 
            // startByteAdrSettings
            // 
            this.startByteAdrSettings.Location = new System.Drawing.Point(676, 175);
            this.startByteAdrSettings.Name = "startByteAdrSettings";
            this.startByteAdrSettings.Size = new System.Drawing.Size(100, 20);
            this.startByteAdrSettings.TabIndex = 22;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(785, 462);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dateTimeSettingsLabel);
            this.Controls.Add(this.label44);
            this.Controls.Add(this.startByteAdrSettings);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.checkConnBdBtn);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.saveBtn);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Name = "SettingsForm";
            this.Text = "SettingsForm";
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button readSettingBtn;
        private System.Windows.Forms.TextBox readSetLabel;
        private System.Windows.Forms.Label writeSetLabel;
        private System.Windows.Forms.Button showSettindBtn;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button saveBtn;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox errViewTextBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox timeSleepExceptionBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button demoCpuBtn;
        private System.Windows.Forms.Button checkConnCpuBtn;
        private System.Windows.Forms.TextBox slotCpuTextBox;
        private System.Windows.Forms.TextBox rackCpuTextBox;
        private System.Windows.Forms.TextBox ipTypeTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cpuTypeComboBox;
        private System.Windows.Forms.Button checkConnBdBtn;
        private System.Windows.Forms.TextBox timerMicroDowntimeTextBox;
        private System.Windows.Forms.TextBox timerStandartCycleTextBox;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox dbDatetimeCpuTextBox;
        private System.Windows.Forms.TextBox dbLimitCpuTextBox;
        private System.Windows.Forms.Label shiftTransitionLabel;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox shiftTransitionTextBox;
        private System.Windows.Forms.CheckBox autoRunWincheckBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label dateTimeSettingsLabel;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.TextBox startByteAdrSettings;
    }
}