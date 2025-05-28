namespace CodingTimeTracker
{
    partial class MainForm
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
            this.btnStartStop = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.cbTicket = new System.Windows.Forms.ComboBox();
            this.cbSpace = new System.Windows.Forms.ComboBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.tWork = new System.Windows.Forms.Timer(this.components);
            this.lblHr = new System.Windows.Forms.Label();
            this.lblMin = new System.Windows.Forms.Label();
            this.lblSec = new System.Windows.Forms.Label();
            this.lblUser = new System.Windows.Forms.Label();
            this.btnGetSpaces = new System.Windows.Forms.Button();
            this.txtComment = new System.Windows.Forms.TextBox();
            this.lblComment = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnStartStop
            // 
            this.btnStartStop.FlatAppearance.BorderColor = System.Drawing.Color.SlateGray;
            this.btnStartStop.Location = new System.Drawing.Point(12, 321);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(288, 37);
            this.btnStartStop.TabIndex = 0;
            this.btnStartStop.Text = "StartStop";
            this.btnStartStop.UseVisualStyleBackColor = true;
            this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
            // 
            // btnNew
            // 
            this.btnNew.Location = new System.Drawing.Point(254, 12);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(46, 23);
            this.btnNew.TabIndex = 1;
            this.btnNew.Text = "New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(234, 364);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(66, 23);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // cbTicket
            // 
            this.cbTicket.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTicket.FormattingEnabled = true;
            this.cbTicket.Location = new System.Drawing.Point(12, 272);
            this.cbTicket.Name = "cbTicket";
            this.cbTicket.Size = new System.Drawing.Size(288, 21);
            this.cbTicket.TabIndex = 3;
            // 
            // cbSpace
            // 
            this.cbSpace.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSpace.FormattingEnabled = true;
            this.cbSpace.Location = new System.Drawing.Point(12, 223);
            this.cbSpace.Name = "cbSpace";
            this.cbSpace.Size = new System.Drawing.Size(288, 21);
            this.cbSpace.TabIndex = 4;
            this.cbSpace.SelectedIndexChanged += new System.EventHandler(this.cbSpace_SelectedIndexChanged);
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(12, 137);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(288, 20);
            this.txtDescription.TabIndex = 5;
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Font = new System.Drawing.Font("Webdings", 11F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescription.Location = new System.Drawing.Point(9, 117);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(89, 17);
            this.lblDescription.TabIndex = 6;
            this.lblDescription.Text = "Description";
            // 
            // tWork
            // 
            this.tWork.Interval = 1;
            this.tWork.Tick += new System.EventHandler(this.tWork_Tick);
            // 
            // lblHr
            // 
            this.lblHr.AutoSize = true;
            this.lblHr.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHr.Location = new System.Drawing.Point(62, 55);
            this.lblHr.Name = "lblHr";
            this.lblHr.Size = new System.Drawing.Size(66, 31);
            this.lblHr.TabIndex = 7;
            this.lblHr.Text = "00  :";
            // 
            // lblMin
            // 
            this.lblMin.AutoSize = true;
            this.lblMin.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMin.Location = new System.Drawing.Point(134, 55);
            this.lblMin.Name = "lblMin";
            this.lblMin.Size = new System.Drawing.Size(66, 31);
            this.lblMin.TabIndex = 8;
            this.lblMin.Text = "00  :";
            // 
            // lblSec
            // 
            this.lblSec.AutoSize = true;
            this.lblSec.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSec.Location = new System.Drawing.Point(206, 55);
            this.lblSec.Name = "lblSec";
            this.lblSec.Size = new System.Drawing.Size(51, 31);
            this.lblSec.TabIndex = 9;
            this.lblSec.Text = "00 ";
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.Location = new System.Drawing.Point(12, 13);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(34, 13);
            this.lblUser.TabIndex = 10;
            this.lblUser.Text = "Hello:";
            // 
            // btnGetSpaces
            // 
            this.btnGetSpaces.Location = new System.Drawing.Point(12, 364);
            this.btnGetSpaces.Name = "btnGetSpaces";
            this.btnGetSpaces.Size = new System.Drawing.Size(80, 23);
            this.btnGetSpaces.TabIndex = 11;
            this.btnGetSpaces.Text = "Get Spaces";
            this.btnGetSpaces.UseVisualStyleBackColor = true;
            this.btnGetSpaces.Click += new System.EventHandler(this.btnGetSpaces_Click);
            // 
            // txtComment
            // 
            this.txtComment.Location = new System.Drawing.Point(12, 180);
            this.txtComment.Name = "txtComment";
            this.txtComment.Size = new System.Drawing.Size(288, 20);
            this.txtComment.TabIndex = 12;
            // 
            // lblComment
            // 
            this.lblComment.AutoSize = true;
            this.lblComment.Font = new System.Drawing.Font("Webdings", 11F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblComment.Location = new System.Drawing.Point(9, 160);
            this.lblComment.Name = "lblComment";
            this.lblComment.Size = new System.Drawing.Size(78, 17);
            this.lblComment.TabIndex = 13;
            this.lblComment.Text = "Comment";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(312, 402);
            this.Controls.Add(this.lblComment);
            this.Controls.Add(this.txtComment);
            this.Controls.Add(this.btnGetSpaces);
            this.Controls.Add(this.lblUser);
            this.Controls.Add(this.lblSec);
            this.Controls.Add(this.lblMin);
            this.Controls.Add(this.lblHr);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.cbSpace);
            this.Controls.Add(this.cbTicket);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.btnStartStop);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "MainForm";
            this.Text = "Main";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStartStop;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.ComboBox cbTicket;
        private System.Windows.Forms.ComboBox cbSpace;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Timer tWork;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblHr;
        private System.Windows.Forms.Label lblMin;
        private System.Windows.Forms.Label lblSec;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.Button btnGetSpaces;
        private System.Windows.Forms.TextBox txtComment;
        private System.Windows.Forms.Label lblComment;
    }
}

