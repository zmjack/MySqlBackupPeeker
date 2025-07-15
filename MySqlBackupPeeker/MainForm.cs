using ExcelSharp.NPOI;
using Spread;
using System.Text;
using System.Text.RegularExpressions;

namespace MySqlBackupPeeker
{
    public partial class MainForm : Form
    {
        private string _filePath;
        private Dictionary<string, string> _structure = [];
        private string[][] _tableData = [];
        private int fetchCount = 2000;

        public MainForm()
        {
            InitializeComponent();
        }

        private readonly Regex _fieldRegex = new Regex(@"  `(\w+?)` ([^\s]+).*,");
        private readonly Regex _lineRegex = new Regex(@"-- Table structure for table `(\w+?)`");
        private Dictionary<string, string> PeekTableStructure(string name)
        {
            using var file = File.OpenRead(_filePath);
            using var reader = new StreamReader(file, Encoding.UTF8, true, 1024, true);

            string? line;
            while (!reader.EndOfStream)
            {
                line = reader.ReadLine();
                if (line == $"-- Table structure for table `{name}`") break;
            }

            line = reader.ReadLine();
            if (line is null or not "--") return [];
            line = reader.ReadLine();
            if (line is null or not "") return [];

            var sb = new StringBuilder();
            while (!reader.EndOfStream)
            {
                line = reader.ReadLine();
                if (string.IsNullOrEmpty(line)) break;

                sb.AppendLine(line);
            }

            var str = sb.ToString();
            var matches = _fieldRegex.Matches(str);
            var dict = new Dictionary<string, string>();
            foreach (Match match in matches)
            {
                var fieldName = match.Groups[1].Value;
                var fieldType = match.Groups[2].Value;
                dict.Add(fieldName, fieldType);
            }
            return dict;
        }

        private enum TextBegin
        {
            None,
            String,
            Number,
        }

        private string[][] PeekTableData(string name)
        {
            using var file = File.OpenRead(_filePath);
            using var reader = new StreamReader(file, Encoding.UTF8, true, 1024, true);

            string? line;
            while (!reader.EndOfStream)
            {
                line = reader.ReadLine();
                if (line == $"-- Dumping data for table `{name}`") break;
            }

            line = reader.ReadLine();
            if (line is null or not "--") return [];
            line = reader.ReadLine();
            if (line is null or not "") return [];

            var rows = new List<string[]>();
            while (!reader.EndOfStream)
            {
                line = reader.ReadLine();
                if (string.IsNullOrEmpty(line)) break;

                if (line.StartsWith("INSERT INTO "))
                {
                    var tag = "VALUES (";
                    var values = line[(line.IndexOf(tag) + tag.Length - 1)..];
                    var sb = new StringBuilder();

                    var row = new List<string>();
                    using (var strReader = new StringReader(values))
                    {
                        sb.Clear();
                        var rowBegin = false;
                        var slash = false;
                        var cellBegin = TextBegin.None;

                        char c;
                        while ((c = (char)strReader.Peek()) != -1)
                        {
                            if (!rowBegin)
                            {
                                if (c == '(')
                                {
                                    rowBegin = true;
                                    strReader.Read();
                                    continue;
                                }
                                if (c == ',')
                                {
                                    strReader.Read();
                                    continue;
                                }
                                if (c == ';')
                                {
                                    break;
                                }
                                else
                                {
                                    MessageBox.Show("Unexpected character before row begin: " + c);
                                    return [];
                                }
                            }

                            if (cellBegin == TextBegin.None)
                            {
                                strReader.Read();
                                if (c == '\'')
                                {
                                    cellBegin = TextBegin.String;
                                }
                                else if (c is ',' or ')')
                                {
                                    row.Add(sb.ToString());
                                    sb.Clear();

                                    if (c == ')')
                                    {
                                        rows.Add(row.ToArray());
                                        row.Clear();
                                        rowBegin = false;
                                    }
                                }
                                else
                                {
                                    cellBegin = TextBegin.Number;
                                    sb.Append(c);
                                }
                                continue;
                            }

                            if (cellBegin == TextBegin.String)
                            {
                                strReader.Read();
                                if (slash)
                                {
                                    sb.Append($"\\{c}");
                                    slash = false;
                                    continue;
                                }

                                if (c == '\\')
                                {
                                    slash = true;
                                    continue;
                                }

                                if (c == '\'')
                                {
                                    cellBegin = TextBegin.None;
                                }
                                else
                                {
                                    sb.Append(c);
                                }
                                continue;
                            }

                            if (cellBegin == TextBegin.Number)
                            {
                                if (c is ',' or ')')
                                {
                                    cellBegin = TextBegin.None;
                                }
                                else
                                {
                                    strReader.Read();
                                    sb.Append(c);
                                }
                            }

                        }
                    }
                }
            }

            return [.. rows];
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "All files (*.sql)|*.sql";
                openFileDialog.Title = "Select a mysql backup";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    tableList.Items.Clear();

                    _filePath = openFileDialog.FileName;
                    using var file = File.OpenRead(_filePath);
                    using var reader = new StreamReader(file, Encoding.UTF8, true, 1024, true);

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine()!;
                        var match = _lineRegex.Match(line);
                        if (match.Success)
                        {
                            var name = match.Groups[1].Value;
                            tableList.Items.Add(name);
                        }
                    }
                }
            }
        }

        private void ShowTableData()
        {
            comboBox_filter.Items.Clear();

            tableDataGrid.Rows.Clear();
            tableDataGrid.Columns.Clear();
            foreach (var field in _structure)
            {
                comboBox_filter.Items.Add(field.Key);
                tableDataGrid.Columns.Add(field.Key, field.Key);
            }
            var data = GetFilteredTableData();
            foreach (var row in data)
            {
                tableDataGrid.Rows.Add(row);
            }
        }

        private void tableList_DoubleClick(object sender, EventArgs e)
        {
            var _sender = (ListBox)sender;
            if (_sender.SelectedItem is not null)
            {
                var name = _sender.SelectedItem.ToString()!;
                _structure = PeekTableStructure(name);
                _tableData = PeekTableData(name);
                ShowTableData();
            }
        }

        private IEnumerable<string[]> GetFilteredTableData()
        {
            var data = _tableData.AsEnumerable();

            if (!string.IsNullOrEmpty(comboBox_filter.Text) && !string.IsNullOrEmpty(textBox_filter_regex.Text))
            {
                var regex = new Regex(textBox_filter_regex.Text);
                var index = Array.IndexOf(_structure.Keys.ToArray(), comboBox_filter.Text);
                if (index > -1)
                {
                    data =
                        from x in data
                        where regex.Match(x[index]).Success
                        select x;
                }
                else data = [];
            }
            return fetchCount > 0 ? data.Take(fetchCount) : data;
        }

        private void comboBox_filter_SelectedValueChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(comboBox_filter.Text) && !string.IsNullOrEmpty(textBox_filter_regex.Text))
            {
                ShowTableData();
            }
        }

        private void tableDataGrid_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();
            var centerFormat = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
        }

        private void button_refresh_Click(object sender, EventArgs e)
        {
            ShowTableData();
        }

        private string? SaveFile()
        {
            using (var saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Excel (*.xlsx)|*.xlsx";
                saveFileDialog.Title = "Save part of MySQL backup as";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    return saveFileDialog.FileName;
                }
                else return null;
            }
        }
        private void button_download_Click(object sender, EventArgs e)
        {
            var fileName = SaveFile();
            if (fileName is not null)
            {
                var excel = new ExcelBook(ExcelVersion.Excel2007);
                var outputSheet = excel.CreateSheet("output");

                var sheet = new SpreadSheet("output")
                {
                    new Vert
                    {
                        new Hori
                        {
                            from s in _structure select s.Key,
                        },

                        from row in GetFilteredTableData()
                        select new Hori
                        {
                            from x in row select x
                        }
                    }
                };
                outputSheet.PrintSpreadSheet(sheet);

                excel.SaveAs(fileName);
            }
        }

        private void textBox_fetchCount_Leave(object sender, EventArgs e)
        {
            if (int.TryParse(textBox_fetchCount.Text, out int value))
            {
                if (value > 0)
                {
                    textBox_fetchCount.Text = value.ToString();
                    fetchCount = value;
                    return;
                }

            }

            textBox_fetchCount.Text = "0";
            fetchCount = 0;
        }

        private void textBox_filter_regex_Leave(object sender, EventArgs e)
        {
            ShowTableData();
        }

        private void textBox_filter_regex_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBox_filter_regex_Leave(sender, default);
            }
        }
    }
}
