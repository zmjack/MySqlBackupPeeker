namespace MySqlBackupPeeker
{
    partial class MainForm
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
            tableDataGrid = new DataGridView();
            tableList = new ListBox();
            menuStrip1 = new MenuStrip();
            toolStripMenuItem1 = new ToolStripMenuItem();
            textBox_fetchCount = new TextBox();
            label1 = new Label();
            label2 = new Label();
            comboBox_filter = new ComboBox();
            label3 = new Label();
            textBox_filter_regex = new TextBox();
            button_refresh = new Button();
            button_download = new Button();
            ((System.ComponentModel.ISupportInitialize)tableDataGrid).BeginInit();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // tableDataGrid
            // 
            tableDataGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableDataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            tableDataGrid.Location = new Point(252, 34);
            tableDataGrid.Name = "tableDataGrid";
            tableDataGrid.Size = new Size(935, 723);
            tableDataGrid.TabIndex = 0;
            tableDataGrid.RowPostPaint += tableDataGrid_RowPostPaint;
            // 
            // tableList
            // 
            tableList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            tableList.FormattingEnabled = true;
            tableList.ItemHeight = 17;
            tableList.Location = new Point(-1, 34);
            tableList.Margin = new Padding(0);
            tableList.Name = "tableList";
            tableList.Size = new Size(250, 718);
            tableList.TabIndex = 1;
            tableList.DoubleClick += tableList_DoubleClick;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { toolStripMenuItem1 });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1184, 25);
            menuStrip1.TabIndex = 2;
            menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(68, 21);
            toolStripMenuItem1.Text = "打开备份";
            toolStripMenuItem1.Click += toolStripMenuItem1_Click;
            // 
            // textBox_fetchCount
            // 
            textBox_fetchCount.Location = new Point(314, 7);
            textBox_fetchCount.Name = "textBox_fetchCount";
            textBox_fetchCount.Size = new Size(100, 23);
            textBox_fetchCount.TabIndex = 3;
            textBox_fetchCount.Text = "2000";
            textBox_fetchCount.Leave += textBox_fetchCount_Leave;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(252, 10);
            label1.Name = "label1";
            label1.Size = new Size(56, 17);
            label1.TabIndex = 4;
            label1.Text = "最大行数";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(420, 10);
            label2.Name = "label2";
            label2.Size = new Size(32, 17);
            label2.TabIndex = 5;
            label2.Text = "筛选";
            // 
            // comboBox_filter
            // 
            comboBox_filter.FormattingEnabled = true;
            comboBox_filter.Location = new Point(458, 7);
            comboBox_filter.Name = "comboBox_filter";
            comboBox_filter.Size = new Size(121, 25);
            comboBox_filter.TabIndex = 6;
            comboBox_filter.SelectedValueChanged += comboBox_filter_SelectedValueChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(585, 10);
            label3.Name = "label3";
            label3.Size = new Size(80, 17);
            label3.TabIndex = 5;
            label3.Text = "等于（正则）";
            // 
            // textBox_filter_regex
            // 
            textBox_filter_regex.Location = new Point(671, 7);
            textBox_filter_regex.Name = "textBox_filter_regex";
            textBox_filter_regex.Size = new Size(223, 23);
            textBox_filter_regex.TabIndex = 7;
            textBox_filter_regex.KeyDown += textBox_filter_regex_KeyDown;
            textBox_filter_regex.Leave += textBox_filter_regex_Leave;
            // 
            // button_refresh
            // 
            button_refresh.Location = new Point(900, 7);
            button_refresh.Name = "button_refresh";
            button_refresh.Size = new Size(75, 23);
            button_refresh.TabIndex = 8;
            button_refresh.Text = "刷新";
            button_refresh.UseVisualStyleBackColor = true;
            button_refresh.Click += button_refresh_Click;
            // 
            // button_download
            // 
            button_download.Location = new Point(981, 7);
            button_download.Name = "button_download";
            button_download.Size = new Size(75, 23);
            button_download.TabIndex = 9;
            button_download.Text = "另存为";
            button_download.UseVisualStyleBackColor = true;
            button_download.Click += button_download_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1184, 761);
            Controls.Add(button_download);
            Controls.Add(button_refresh);
            Controls.Add(textBox_filter_regex);
            Controls.Add(comboBox_filter);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(textBox_fetchCount);
            Controls.Add(tableList);
            Controls.Add(tableDataGrid);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "MainForm";
            Text = "MySql Backup Peeker";
            ((System.ComponentModel.ISupportInitialize)tableDataGrid).EndInit();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView tableDataGrid;
        private ListBox tableList;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem toolStripMenuItem1;
        private TextBox textBox_fetchCount;
        private Label label1;
        private Label label2;
        private ComboBox comboBox_filter;
        private Label label3;
        private TextBox textBox_filter_regex;
        private Button button_refresh;
        private Button button_download;
    }
}
