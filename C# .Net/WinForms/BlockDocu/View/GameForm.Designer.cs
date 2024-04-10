namespace BlockDocu.View
{
    partial class GameForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            mainTable = new TableLayoutPanel();
            nextBlock = new TableLayoutPanel();
            label1 = new Label();
            label2 = new Label();
            pointLabel = new Label();
            SuspendLayout();
            // 
            // mainTable
            // 
            mainTable.ColumnCount = 4;
            mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            mainTable.Location = new Point(40, 63);
            mainTable.Name = "mainTable";
            mainTable.RowCount = 4;
            mainTable.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            mainTable.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            mainTable.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            mainTable.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            mainTable.Size = new Size(400, 400);
            mainTable.TabIndex = 0;
            // 
            // nextBlock
            // 
            nextBlock.ColumnCount = 2;
            nextBlock.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            nextBlock.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            nextBlock.Cursor = Cursors.No;
            nextBlock.Location = new Point(620, 171);
            nextBlock.Name = "nextBlock";
            nextBlock.RowCount = 2;
            nextBlock.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            nextBlock.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            nextBlock.Size = new Size(200, 200);
            nextBlock.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(620, 134);
            label1.Name = "label1";
            label1.Size = new Size(132, 30);
            label1.TabIndex = 2;
            label1.Text = "Next Block:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(40, 21);
            label2.Name = "label2";
            label2.Size = new Size(83, 30);
            label2.TabIndex = 3;
            label2.Text = "Points:";
            // 
            // pointLabel
            // 
            pointLabel.AutoSize = true;
            pointLabel.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point);
            pointLabel.Location = new Point(129, 21);
            pointLabel.Name = "pointLabel";
            pointLabel.Size = new Size(26, 30);
            pointLabel.TabIndex = 4;
            pointLabel.Text = "0";
            // 
            // GameForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(984, 561);
            Controls.Add(pointLabel);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(nextBlock);
            Controls.Add(mainTable);
            Name = "GameForm";
            Text = "Block Docu";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TableLayoutPanel mainTable;
        private TableLayoutPanel nextBlock;
        private Label label1;
        private Label label2;
        private Label pointLabel;
    }
}
