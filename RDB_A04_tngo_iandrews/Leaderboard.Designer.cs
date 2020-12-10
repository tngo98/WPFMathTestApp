
namespace RDB_A04_tngo_iandrews
{
    partial class Leaderboard
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
            this.leaderboardListBox = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // leaderboardListBox
            // 
            this.leaderboardListBox.HideSelection = false;
            this.leaderboardListBox.Location = new System.Drawing.Point(12, 12);
            this.leaderboardListBox.Name = "leaderboardListBox";
            this.leaderboardListBox.Size = new System.Drawing.Size(299, 366);
            this.leaderboardListBox.TabIndex = 0;
            this.leaderboardListBox.UseCompatibleStateImageBehavior = false;
            // 
            // Leaderboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(323, 390);
            this.Controls.Add(this.leaderboardListBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Leaderboard";
            this.Text = "Leaderboard";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView leaderboardListBox;
    }
}