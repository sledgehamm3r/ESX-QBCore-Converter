using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ESXtoQBCoreConverter
{
    public partial class MainForm : Form
    {
        private TextBox folderPathTextBox;
        private TextBox replaceFilePathTextBox;
        private ListBox processedFilesListBox;
        private Button browseFolderButton;
        private Button browseReplaceFileButton;
        private Button startButton;
        private Label instructionLabel;
        private Label folderPathLabel;
        private Label replaceFileLabel;
        private Label copyrightLabel;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem creditsToolStripMenuItem;
        private ToolStripMenuItem donateToolStripMenuItem;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private ColorScheme currentColorScheme = ColorScheme.Light;
        private Language currentLanguage = Language.English;

        private List<string> processedFiles;

        public MainForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "ESX / QBCore Converter";
            this.Size = new System.Drawing.Size(600, 600);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            menuStrip1 = new MenuStrip();
            creditsToolStripMenuItem = new ToolStripMenuItem();
            donateToolStripMenuItem = new ToolStripMenuItem();
            settingsToolStripMenuItem = new ToolStripMenuItem();

            creditsToolStripMenuItem.Text = "Credits";
            donateToolStripMenuItem.Text = "Donate";
            settingsToolStripMenuItem.Text = "Settings";

            creditsToolStripMenuItem.Click += CreditsToolStripMenuItem_Click;
            donateToolStripMenuItem.Click += DonateToolStripMenuItem_Click;
            settingsToolStripMenuItem.Click += SettingsToolStripMenuItem_Click;

            menuStrip1.Items.Add(creditsToolStripMenuItem);
            menuStrip1.Items.Add(donateToolStripMenuItem);
            menuStrip1.Items.Add(settingsToolStripMenuItem);
            this.Controls.Add(menuStrip1);

            instructionLabel = new Label();
            instructionLabel.Text = "Select a folder where the files will be processed:";
            instructionLabel.Location = new System.Drawing.Point(20, 50);
            instructionLabel.AutoSize = true;

            folderPathLabel = new Label();
            folderPathLabel.Text = "Folder Path:";
            folderPathLabel.Location = new System.Drawing.Point(20, 80);
            folderPathLabel.AutoSize = true;

            folderPathTextBox = new TextBox();
            folderPathTextBox.Location = new System.Drawing.Point(100, 80);
            folderPathTextBox.Size = new System.Drawing.Size(320, 20);

            browseFolderButton = new Button();
            browseFolderButton.Text = "Browse Folder";
            browseFolderButton.Location = new System.Drawing.Point(430, 78);
            browseFolderButton.Click += BrowseFolderButton_Click;

            replaceFileLabel = new Label();
            replaceFileLabel.Text = "Replacement File:";
            replaceFileLabel.Location = new System.Drawing.Point(20, 110);
            replaceFileLabel.AutoSize = true;

            replaceFilePathTextBox = new TextBox();
            replaceFilePathTextBox.Location = new System.Drawing.Point(120, 110);
            replaceFilePathTextBox.Size = new System.Drawing.Size(300, 20);

            browseReplaceFileButton = new Button();
            browseReplaceFileButton.Text = "Browse File";
            browseReplaceFileButton.Location = new System.Drawing.Point(430, 108);
            browseReplaceFileButton.Click += BrowseReplaceFileButton_Click;

            processedFilesListBox = new ListBox();
            processedFilesListBox.Location = new System.Drawing.Point(20, 150);
            processedFilesListBox.Size = new System.Drawing.Size(550, 300);

            startButton = new Button();
            startButton.Text = "Start";
            startButton.Location = new System.Drawing.Point(250, 480);
            startButton.Click += StartButton_Click;

            copyrightLabel = new Label();
            copyrightLabel.Text = "\u00a9" + " " + DateTime.Now.Year + " sledge.dev";
            copyrightLabel.AutoSize = true;
            copyrightLabel.Location = new System.Drawing.Point(20, startButton.Location.Y);

            this.Controls.Add(instructionLabel);
            this.Controls.Add(folderPathLabel);
            this.Controls.Add(folderPathTextBox);
            this.Controls.Add(browseFolderButton);
            this.Controls.Add(replaceFileLabel);
            this.Controls.Add(replaceFilePathTextBox);
            this.Controls.Add(browseReplaceFileButton);
            this.Controls.Add(processedFilesListBox);
            this.Controls.Add(startButton);
            this.Controls.Add(copyrightLabel);

            processedFiles = new List<string>();
        }

        private void CreditsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Developer: Ronny Riggs (sledge.dev)\nDiscord: sledge_hamm3r", "Credits", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DonateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://ko-fi.com/sledgedev");
        }

        private void SettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var settingsForm = new SettingsForm(currentColorScheme, currentLanguage);
            settingsForm.SettingsChanged += SettingsForm_SettingsChanged;
            settingsForm.ShowDialog();
        }

        private void SettingsForm_SettingsChanged(object sender, SettingsChangedEventArgs e)
        {
            currentColorScheme = e.ColorScheme;
            currentLanguage = e.Language;
            ApplyTheme();
            ApplyLanguage();
        }

        private void ApplyTheme()
        {
            if (currentColorScheme == ColorScheme.Dark)
            {
                this.BackColor = Color.FromArgb(30, 30, 30);
                this.ForeColor = Color.White;
            }
            else
            {
                this.BackColor = SystemColors.Control;
                this.ForeColor = SystemColors.ControlText;
            }
        }

        private void ApplyLanguage()
        {
            if (currentLanguage == Language.German)
            {
                creditsToolStripMenuItem.Text = "Anerkennungen";
                donateToolStripMenuItem.Text = "Spenden";
                settingsToolStripMenuItem.Text = "Einstellungen";
                instructionLabel.Text = "Wählen Sie einen Ordner aus, in dem die Dateien verarbeitet werden sollen:";
                folderPathLabel.Text = "Ordnerpfad:";
                replaceFileLabel.Text = "Ersatzdatei:";
                startButton.Text = "Start";
            }
            else
            {
                creditsToolStripMenuItem.Text = "Credits";
                donateToolStripMenuItem.Text = "Donate";
                settingsToolStripMenuItem.Text = "Settings";
                instructionLabel.Text = "Select a folder where the files will be processed:";
                folderPathLabel.Text = "Folder Path:";
                replaceFileLabel.Text = "Replacement File:";
                startButton.Text = "Start";
            }
        }

        private void BrowseFolderButton_Click(object sender, EventArgs e)
        {
            using (var folderBrowserDialog = new FolderBrowserDialog())
            {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    folderPathTextBox.Text = folderBrowserDialog.SelectedPath;
                }
            }
        }

        private void BrowseReplaceFileButton_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Text files (*.txt)|*.txt";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    replaceFilePathTextBox.Text = openFileDialog.FileName;
                }
            }
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            string folderPath = folderPathTextBox.Text;
            string replaceFilePath = replaceFilePathTextBox.Text;

            if (string.IsNullOrWhiteSpace(folderPath))
            {
                MessageBox.Show("Please select a folder.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(replaceFilePath))
            {
                MessageBox.Show("Please select a replacement file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            processedFiles.Clear();
            processedFilesListBox.Items.Clear();

            string[] replaceLines = File.ReadAllLines(replaceFilePath);
            if (replaceLines.Length % 2 != 0)
            {
                MessageBox.Show("The replacement file must contain pairs of search text and replacement text.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Dictionary<string, string> replaceDict = new Dictionary<string, string>();
            for (int i = 0; i < replaceLines.Length; i += 2)
            {
                replaceDict[replaceLines[i]] = replaceLines[i + 1];
            }

            string[] filesToProcess = { "*.lua" };

            foreach (string fileExtension in filesToProcess)
            {
                foreach (string filePath in Directory.GetFiles(folderPath, fileExtension, SearchOption.AllDirectories))
                {
                    string originalContent = File.ReadAllText(filePath);
                    string updatedContent = originalContent;

                    foreach (var pair in replaceDict)
                    {
                        updatedContent = updatedContent.Replace(pair.Key, pair.Value);
                    }

                    if (updatedContent != originalContent)
                    {
                        File.WriteAllText(filePath, updatedContent);
                        processedFiles.Add(filePath);
                    }
                }
            }

            foreach (string processedFile in processedFiles)
            {
                processedFilesListBox.Items.Add(processedFile);
            }

            MessageBox.Show("All files have been processed successfully.", "Finished", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }

    public enum ColorScheme
    {
        Light,
        Dark
    }

    public enum Language
    {
        English,
        German
    }

    public class SettingsForm : Form
    {
        private ComboBox colorSchemeComboBox;
        private ComboBox languageComboBox;

        public event EventHandler<SettingsChangedEventArgs> SettingsChanged;

        public SettingsForm(ColorScheme currentColorScheme, Language currentLanguage)
        {
            this.Text = "Settings";
            this.Size = new Size(300, 150);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            Label colorSchemeLabel = new Label();
            colorSchemeLabel.Text = "Color Scheme:";
            colorSchemeLabel.Location = new Point(20, 20);

            colorSchemeComboBox = new ComboBox();
            colorSchemeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            colorSchemeComboBox.Items.AddRange(Enum.GetNames(typeof(ColorScheme)));
            colorSchemeComboBox.SelectedItem = currentColorScheme.ToString();
            colorSchemeComboBox.Location = new Point(130, 20);

            Label languageLabel = new Label();
            languageLabel.Text = "Language:";
            languageLabel.Location = new Point(20, 50);

            languageComboBox = new ComboBox();
            languageComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            languageComboBox.Items.AddRange(Enum.GetNames(typeof(Language)));
            languageComboBox.SelectedItem = currentLanguage.ToString();
            languageComboBox.Location = new Point(130, 50);

            Button saveButton = new Button();
            saveButton.Text = "Save";
            saveButton.Location = new Point(110, 90);
            saveButton.Click += SaveButton_Click;

            this.Controls.Add(colorSchemeLabel);
            this.Controls.Add(colorSchemeComboBox);
            this.Controls.Add(languageLabel);
            this.Controls.Add(languageComboBox);
            this.Controls.Add(saveButton);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            var colorScheme = (ColorScheme)Enum.Parse(typeof(ColorScheme), colorSchemeComboBox.SelectedItem.ToString());
            var language = (Language)Enum.Parse(typeof(Language), languageComboBox.SelectedItem.ToString());
            SettingsChanged?.Invoke(this, new SettingsChangedEventArgs(colorScheme, language));
            this.Close();
        }
    }

    public class SettingsChangedEventArgs : EventArgs
    {
        public ColorScheme ColorScheme { get; }
        public Language Language { get; }

        public SettingsChangedEventArgs(ColorScheme colorScheme, Language language)
        {
            ColorScheme = colorScheme;
            Language = language;
        }
    }
}
