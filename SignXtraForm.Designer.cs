namespace ProductionChangeRecord
{
    partial class SignXtraForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.msglabel = new System.Windows.Forms.Label();
            this.emptextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 12F);
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(118, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(451, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nhập mã nhân viên người ký duyệt và nhấn Enter";
            // 
            // msglabel
            // 
            this.msglabel.AutoSize = true;
            this.msglabel.Font = new System.Drawing.Font("Tahoma", 12F);
            this.msglabel.Location = new System.Drawing.Point(12, 165);
            this.msglabel.Name = "msglabel";
            this.msglabel.Size = new System.Drawing.Size(0, 24);
            this.msglabel.TabIndex = 0;
            // 
            // emptextBox
            // 
            this.emptextBox.Font = new System.Drawing.Font("Tahoma", 12F);
            this.emptextBox.Location = new System.Drawing.Point(165, 95);
            this.emptextBox.Name = "emptextBox";
            this.emptextBox.Size = new System.Drawing.Size(351, 32);
            this.emptextBox.TabIndex = 1;
            this.emptextBox.TextChanged += new System.EventHandler(this.emptextBox_TextChanged);
            this.emptextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.emptextBox_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(210, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(255, 22);
            this.label2.TabIndex = 0;
            this.label2.Text = "輸入審核者員工代碼並按 Enter";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SignXtraForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(712, 225);
            this.ControlBox = false;
            this.Controls.Add(this.emptextBox);
            this.Controls.Add(this.msglabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "SignXtraForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "審核者資訊 Thông tin người ký duyệt";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SignXtraForm_FormClosing);
            this.Load += new System.EventHandler(this.SignXtraForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label msglabel;
        private System.Windows.Forms.TextBox emptextBox;
        private System.Windows.Forms.Label label2;
    }
}