namespace Spreadsheet_CSigvartson
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoCellTextChangeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoCellTextChangeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cellToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeCellBackgroundColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveSpreadhseetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadSpreadhseetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(0, 27);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(284, 229);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridView1_CellBeginEdit);
            this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEndEdit);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.cellToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(284, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoCellTextChangeToolStripMenuItem,
            this.redoCellTextChangeToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
            // 
            // undoCellTextChangeToolStripMenuItem
            // 
            this.undoCellTextChangeToolStripMenuItem.Name = "undoCellTextChangeToolStripMenuItem";
            this.undoCellTextChangeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.undoCellTextChangeToolStripMenuItem.Text = "Undo";
            this.undoCellTextChangeToolStripMenuItem.Click += new System.EventHandler(this.undoCellTextChangeToolStripMenuItem_Click);
            // 
            // redoCellTextChangeToolStripMenuItem
            // 
            this.redoCellTextChangeToolStripMenuItem.Name = "redoCellTextChangeToolStripMenuItem";
            this.redoCellTextChangeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.redoCellTextChangeToolStripMenuItem.Text = "Redo";
            this.redoCellTextChangeToolStripMenuItem.Click += new System.EventHandler(this.redoCellTextChangeToolStripMenuItem_Click);
            // 
            // cellToolStripMenuItem
            // 
            this.cellToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.changeCellBackgroundColorToolStripMenuItem});
            this.cellToolStripMenuItem.Name = "cellToolStripMenuItem";
            this.cellToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.cellToolStripMenuItem.Text = "Cell";
            // 
            // changeCellBackgroundColorToolStripMenuItem
            // 
            this.changeCellBackgroundColorToolStripMenuItem.Name = "changeCellBackgroundColorToolStripMenuItem";
            this.changeCellBackgroundColorToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.changeCellBackgroundColorToolStripMenuItem.Text = "Change cell background color...";
            this.changeCellBackgroundColorToolStripMenuItem.Click += new System.EventHandler(this.changeCellBackgroundColorToolStripMenuItem_Click);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveSpreadhseetToolStripMenuItem,
            this.loadSpreadhseetToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // saveSpreadhseetToolStripMenuItem
            // 
            this.saveSpreadhseetToolStripMenuItem.Name = "saveSpreadhseetToolStripMenuItem";
            this.saveSpreadhseetToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.saveSpreadhseetToolStripMenuItem.Text = "Save spreadhseet";
            this.saveSpreadhseetToolStripMenuItem.Click += new System.EventHandler(this.saveSpreadhseetToolStripMenuItem_Click);
            // 
            // loadSpreadhseetToolStripMenuItem
            // 
            this.loadSpreadhseetToolStripMenuItem.Name = "loadSpreadhseetToolStripMenuItem";
            this.loadSpreadhseetToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.loadSpreadhseetToolStripMenuItem.Text = "Load spreadhseet";
            this.loadSpreadhseetToolStripMenuItem.Click += new System.EventHandler(this.loadSpreadhseetToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoCellTextChangeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redoCellTextChangeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem cellToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeCellBackgroundColorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveSpreadhseetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadSpreadhseetToolStripMenuItem;
    }
}

