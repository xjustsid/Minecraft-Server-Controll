using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Timers;
using System.Net;
using System.Net.Sockets;

namespace Minecraft_Server_Controll
{
    public partial class Form1 : Form
    {
        private Process process;
        private string workingDirectory;
        private string arguments;
        private string backupFolderPath;
        private string autoRestartType;
        private bool autoSaveEnabled;
        private int restartInterval;
        private string restartTimes;
        private string[] restartTimesArray;
        private int maxBackupCount;
        private string labelStatus;
        private System.Timers.Timer restartTimer;
        private bool programStarted = false;
        private bool backupInProgress = false;

        private bool IsServerRunning()
        {
            return process != null && !process.HasExited;

        }

        public Form1()
        {
            InitializeComponent();
            textBox2.ReadOnly = true;
            LoadConfiguration();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            programStarted = true;

            // Rufe die Methode UpdateNextRestartTime() auf, um die Zeit im Label zu aktualisieren
            UpdateNextRestartTime();

            // Starte die automatische Sicherung und den Neustart
            nextAutoBackup();
        }

        private void LoadConfiguration()
        {
            LoadConfigurationFromFile();

        }

        private void LoadConfigurationFromFile()
        {
            CheckAndCreateConfiguration();

            string configFilePath = Path.Combine(backupFolderPath, "config.txt");
            string[] lines = File.ReadAllLines(configFilePath);
            bool hasRestartTimes = false;
            foreach (string line in lines)
            {
                if (line.StartsWith("server_pfad="))
                {
                    workingDirectory = line.Substring("server_pfad=".Length);
                }
                else if (line.StartsWith("StartCommand="))
                {
                    arguments = line.Substring("StartCommand=".Length);
                    if (!arguments.Contains("/k"))
                    {
                        arguments = "cmd.exe /k " + arguments;
                    }
                }
                else if (line.StartsWith("keepalive="))
                {
                    string keepAliveValue = line.Substring("keepalive=".Length).ToLower();
                    checkBox1.Checked = (keepAliveValue == "true");
                }
                else if (line.StartsWith("auto_restart="))
                {
                    autoRestartType = line.Substring("auto_restart=".Length);
                }
                else if (line.StartsWith("auto_save_enabled="))
                {
                    string autoSaveEnabledValue = line.Substring("auto_save_enabled=".Length).ToLower();
                    autoSaveEnabled = (autoSaveEnabledValue == "true");
                }
                else if (line.StartsWith("restart_interval="))
                {
                    int.TryParse(line.Substring("restart_interval=".Length), out restartInterval);
                }
                else if (line.StartsWith("restart_times="))
                {
                    restartTimes = line.Substring("restart_times=".Length);
                    restartTimesArray = restartTimes.Split(',');
                    hasRestartTimes = true;
                }
                else if (line.StartsWith("max_backup_count="))
                {
                    int.TryParse(line.Substring("max_backup_count=".Length), out maxBackupCount);
                }
            }

            if (autoSaveEnabled && hasRestartTimes)
            {
                UpdateNextRestartTime();

            }

            // Set the text and checked state of checkBox2 based on autoSaveEnabled and autoRestartType
            if (autoSaveEnabled)
            {
                if (autoRestartType.ToLower() == "interval")
                {
                    checkBox2.Checked = true;
                    checkBox2.Text = "AutoSave (Intervall)";
                }
                else if (autoRestartType.ToLower() == "time")
                {
                    checkBox2.Checked = true;
                    checkBox2.Text = "AutoSave (Zeit)";
                }
                else
                {
                    checkBox2.Checked = false;
                    checkBox2.Text = "AutoSave deaktiviert";
                }
            }
            else
            {
                checkBox2.Checked = false;
                checkBox2.Text = "AutoSave deaktiviert";
            }

            SaveConfigurationInternal();
            UpdateNextRestartTime();
        }
        private void SaveConfigurationInternal()
        {
            string configFilePath = Path.Combine(backupFolderPath, "config.txt");
            string[] lines = File.ReadAllLines(configFilePath);

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("keepalive="))
                {
                    lines[i] = "keepalive=" + (checkBox1.Checked ? "true" : "false");
                }
                else if (lines[i].StartsWith("auto_restart="))
                {
                    if (checkBox2.Checked)
                    {
                        lines[i] = "auto_restart=" + (checkBox2.Text.Contains("Intervall") ? "interval" : "time");
                    }
                    else
                    {
                        lines[i] = "auto_restart=disabled";
                    }
                }
                if (lines[i].StartsWith("auto_save_enabled="))
                {
                    lines[i] = "auto_save_enabled=" + (checkBox2.Checked ? "true" : "false");
                }
                if (lines[i].StartsWith("max_backup_count="))
                {
                    lines[i] = "max_backup_count=" + maxBackupCount.ToString();
                }

                if (autoSaveEnabled)
                {
                    UpdateNextRestartTime();
                }
            }

            File.WriteAllLines(configFilePath, lines);
        }

        private void CreateDefaultConfigFile(string filePath)
        {
            string defaultConfig = $@"# Konfigurationsdatei für MineServerControl
# Bitte tragen Sie die entsprechenden Werte ein

#Pfad zum Server
server_pfad=C:\Pfad\zum\Server\{workingDirectory}

#Startparameter für den Server (bsp für einen Server Fabric Server mit 4GB zuweisung)
StartCommand=/k java -Xmx4G -jar fabric-server-launch.jar nogui

# Dies ist der Standardordner, in dem alles gespeichert wird
MCC_Backup={backupFolderPath}

#Pfad zu den Logs-Backups
logs_backups={Path.Combine(backupFolderPath, "logs_backups")}

#Pfad zu den Session-Logs
session_log={Path.Combine(backupFolderPath, "session_log")}

#Pfad zu den World-Backups
world_backup={Path.Combine(backupFolderPath, "world_backup")}

# Keep Alive
# Wenn true, wird der Server neu gestartet, wenn er nicht läuft. Ideal, um zu verhindern, dass er nach einem Absturz aus bleibt.
keepalive=false

# AutoSave System
# Optionen: interval|time
# Wenn auto_restart auf ""interval"" gesetzt ist, wird der Server alle X Stunden neu gestartet.
# Wenn auto_restart auf ""time"" gesetzt ist, wird der Server zu den in angegebenen Zeiten neu gestartet.
auto_restart=disabled

# Wenn Sie möchten, dass das AutoSave-System aktiviert ist, setzen Sie diese Option auf ""true/false"".
auto_save_enabled=false

# Wenn Sie möchten, dass der Server zu bestimmten Intervallen neu gestartet wird, geben Sie die Anzahl der Stunden zwischen den Neustarts an.
# Ganzzahl: Neustart alle X Stunden
restart_interval=3

# Wenn Sie möchten, dass der Server zu bestimmten Tageszeiten neu gestartet wird, verwenden Sie die Option ""time"" und geben Sie hier die Zeiten an.
# Kommagetrennte Zeiten 00:00,06:00,12:00,18:00 usw.
restart_times=00:00,06:00,12:00,18:00

# Wie viele Backups behalten werden sollen, bevor alte Backups gelöscht werden. Geben Sie eine Ganzzahl an.
max_backup_count=5";

            File.WriteAllText(filePath, defaultConfig);
        }

        private void CheckAndCreateConfiguration()
        {
            backupFolderPath = Path.Combine(Application.StartupPath, "MCC_Backup");
            string configFilePath = Path.Combine(backupFolderPath, "config.txt");
            string logsBackupFolderPath = Path.Combine(backupFolderPath, "logs_backup");
            string sessionLogFolderPath = Path.Combine(backupFolderPath, "session_log");
            string worldBackupFolderPath = Path.Combine(backupFolderPath, "world_backup");

            if (!File.Exists(configFilePath) || !Directory.Exists(logsBackupFolderPath) || !Directory.Exists(sessionLogFolderPath) || !Directory.Exists(worldBackupFolderPath))
            {
                MessageBox.Show("Die Konfigurationsdatei oder die erforderlichen Ordner existieren nicht. Bitte bearbeiten Sie die Konfigurationsdatei und starten Sie das Programm erneut.");
                Directory.CreateDirectory(logsBackupFolderPath);
                Directory.CreateDirectory(sessionLogFolderPath);
                Directory.CreateDirectory(worldBackupFolderPath);
                CreateDefaultConfigFile(configFilePath);
                Environment.Exit(0);
            }
        }
        private void StartServer()
        {

            process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = arguments;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.OutputDataReceived += ServerOutputDataReceived;
            process.ErrorDataReceived += ServerErrorDataReceived;
            process.StartInfo.WorkingDirectory = workingDirectory;
            UpdateNextRestartTime();
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();


        }
        private void StopServer()
        {
            if (process != null)
            {
                ClearTextBox();
                process.StandardInput.WriteLine("stop");
                process.StandardInput.Flush();
                process.Kill();
                ClearTextBox();
                Debug.WriteLine("STOPMETHODE");
            }
            else
            {
                // Hier kannst du eine Meldung ausgeben, dass der Server nicht gestartet wurde
                MessageBox.Show("Der Server wurde noch nicht gestartet.");
            }
        }


        private void UpdateNextRestartTime()
        {
            DateTime nextRestartTime = GetNextRestartTime();

            if (!autoSaveEnabled)
            {
                UpdateNextRestartTimeLabel("Nächste geplante Neustartzeit: AutoSave deaktiviert");
                return;
            }

            if (autoRestartType.ToLower() == "interval")
            {
                // Aktualisiere die nächste Neustartzeit basierend auf dem aktuellen Zeitpunkt und dem Neustellintervall
                DateTime nextRestart = DateTime.Now.AddHours(restartInterval);
                UpdateNextRestartTimeLabel("Nächste geplante Neustartzeit: Interval= " + nextRestart.ToString("HH:mm"));
            }
            else if (autoRestartType.ToLower() == "time")
            {
                UpdateNextRestartTimeLabel("Nächste geplante Neustartzeit: Zeit= " + nextRestartTime.ToString("HH:mm"));
            }
        }
        // Methode, um das Label für die nächste geplante Neustartzeit im Haupt-UI-Thread zu aktualisieren
        private void UpdateNextRestartTimeLabel(string labelText)
        {
            if (label2.InvokeRequired)
            {
                label2.Invoke((MethodInvoker)(() => UpdateNextRestartTimeLabel(labelText)));
            }
            else
            {
                label2.Text = labelText;
            }
        }


        private DateTime GetNextRestartTime()
        {
            DateTime now = DateTime.Now;
            DateTime nextRestartTime = DateTime.MaxValue;

            if (autoSaveEnabled && autoRestartType.ToLower() == "time" && restartTimesArray != null && restartTimesArray.Length > 0)
            {
                Array.Sort(restartTimesArray); // Sortiere die Restart-Zeiten in aufsteigender Reihenfolge

                foreach (string restartTime in restartTimesArray)
                {
                    if (DateTime.TryParseExact(restartTime, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime scheduledTime))
                    {
                        DateTime nextScheduledTime = new DateTime(now.Year, now.Month, now.Day, scheduledTime.Hour, scheduledTime.Minute, 0);

                        if (nextScheduledTime > now)
                        {
                            nextRestartTime = nextScheduledTime;
                            break; // Breche die Schleife ab, wenn die nächste Zeit gefunden wurde
                        }
                    }
                }

                if (nextRestartTime == DateTime.MaxValue)
                {
                    DateTime tomorrow = now.AddDays(1);
                    foreach (string restartTime in restartTimesArray)
                    {
                        if (DateTime.TryParseExact(restartTime, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime scheduledTime))
                        {
                            DateTime nextScheduledTime = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, scheduledTime.Hour, scheduledTime.Minute, 0);

                            if (nextScheduledTime > now)
                            {
                                nextRestartTime = nextScheduledTime;
                                break; // Breche die Schleife ab, wenn die nächste Zeit gefunden wurde
                            }
                        }
                    }
                }
            }
            else if (autoSaveEnabled && autoRestartType.ToLower() == "interval" && restartInterval > 0)
            {
                int intervalHours = restartInterval;
                DateTime nextIntervalRestartTime = now.AddHours(intervalHours);

                if (nextIntervalRestartTime > now)
                {
                    nextRestartTime = nextIntervalRestartTime;
                }
            }

            return nextRestartTime;
        }


        private void ClearTextBox()
        {
            if (textBox2.InvokeRequired)
            {
                textBox2.Invoke((MethodInvoker)(ClearTextBox));
            }
            else
            {
                textBox2.Clear();
            }
        }

        private void AppendText(string text)
        {
            if (textBox2.InvokeRequired)
            {
                textBox2.Invoke((MethodInvoker)(() => AppendText(text)));
            }
            else
            {
                textBox2.AppendText(text + Environment.NewLine);
                textBox2.ScrollToCaret();
            }
        }

        private void ServerOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                Invoke((MethodInvoker)(() =>
                {
                    AppendText(e.Data);
                }));
            }
        }

        private void ServerErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                Invoke((MethodInvoker)(() =>
                {
                    AppendText(e.Data);
                }));
            }
        }

        private void ExecuteCommand(string command)
        {
            if (!string.IsNullOrEmpty(command))
            {
                if (command.ToLower() == "stop")
                {
                    ClearTextBox();
                    process.StandardInput.WriteLine(command);
                    process.StandardInput.Flush();
                    process.Kill();
                    ClearTextBox();
                }
                else
                {
                    process.StandardInput.WriteLine(command);
                    process.StandardInput.Flush();
                }
            }
        }

        private void SendCommandToServer(string command)
        {
            string commandWithPrefix = "/" + command; // Füge das "/"-Präfix zum Befehl hinzu

            // Sende den Befehl an den Server
            process.StandardInput.WriteLine(commandWithPrefix);
            process.StandardInput.Flush();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button2.PerformClick();
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
        }

        private void BackupLogsFolder()
        {
            string sourceFolderPath = Path.Combine(workingDirectory, "logs");
            string destinationFolderPath = Path.Combine(backupFolderPath, "logs_backup");

            if (Directory.Exists(sourceFolderPath))
            {
                // Erstelle einen eindeutigen Ordner für dieses Backup mit Datum und Uhrzeit
                string backupFolderName = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string backupDestinationFolderPath = Path.Combine(destinationFolderPath, backupFolderName);
                Directory.CreateDirectory(backupDestinationFolderPath);

                // Kopiere alle Dateien im Quellordner in den Zielordner (Backup-Ordner)
                foreach (string file in Directory.GetFiles(sourceFolderPath))
                {
                    string fileName = Path.GetFileName(file);
                    string destinationFilePath = Path.Combine(backupDestinationFolderPath, fileName);
                    File.Copy(file, destinationFilePath, true);
                }


            }
            else
            {
                MessageBox.Show("Der 'logs'-Ordner existiert nicht im Arbeitsverzeichnis.");
            }
        }
        private void CopyFolder(string sourceFolder, string destinationFolder)
        {
            Directory.CreateDirectory(destinationFolder);

            // Kopiere alle Dateien im Quellordner in den Zielordner
            foreach (string file in Directory.GetFiles(sourceFolder))
            {
                string fileName = Path.GetFileName(file);
                string destinationFilePath = Path.Combine(destinationFolder, fileName);
                File.Copy(file, destinationFilePath, true);
            }

            // Kopiere alle Unterordner rekursiv in den Zielordner
            foreach (string subFolder in Directory.GetDirectories(sourceFolder))
            {
                string folderName = Path.GetFileName(subFolder);
                string destinationSubFolder = Path.Combine(destinationFolder, folderName);
                CopyFolder(subFolder, destinationSubFolder);
            }


        }
        private void BackupWorldFolder()
        {
            string sourceFolderPath = Path.Combine(workingDirectory, "world");
            string destinationFolderPath = Path.Combine(backupFolderPath, "world_backup");

            if (Directory.Exists(sourceFolderPath))
            {
                // Erstelle einen eindeutigen Ordner für dieses Backup mit Datum und Uhrzeit
                string backupFolderName = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string backupDestinationFolderPath = Path.Combine(destinationFolderPath, backupFolderName);
                CopyFolder(sourceFolderPath, backupDestinationFolderPath);


            }
            else
            {
                MessageBox.Show("Der 'world'-Ordner existiert nicht im Arbeitsverzeichnis.");
            }
        }

        private async void PerformBackup()
        {
            // Stoppe den Server
            StopServer();
            while (IsServerRunning())
            {
                await Task.Delay(100); // Kurze Pause, um der Benutzeroberfläche zu ermöglichen, zu reagieren
            }

            // Warte zusätzliche 5 Sekunden, um sicherzustellen, dass alle Prozesse beendet wurden
            await Task.Delay(5000);

            // Führe das Backup durch
            await Task.Run(() =>
            {
                BackupLogsFolder();
                BackupWorldFolder();
            });
            await Task.Delay(100);
            UpdateNextRestartTime();
            await Task.Delay(100);
            LoadConfigurationFromFile();
            await Task.Delay(100);
            // Starte den Server wieder
            StartServer();
            // Aktualisiere das Label für die nächste geplante Neustartzeit

            // Lösche alte Backups
            DeleteOldBackups("world_backup", maxBackupCount);
            DeleteOldBackups("logs_backup", maxBackupCount);
        }
        private void nextAutoBackup()
        {
            if (!autoSaveEnabled)
            {
                // AutoSave ist deaktiviert, daher kein Backup oder Neustart geplant.
                return;
            }

            DateTime now = DateTime.Now;
            DateTime nextActionTime = GetNextRestartTime();

            if (nextActionTime == DateTime.MaxValue)
            {
                // Es wurden keine gültigen Zeiten für den Neustart/Backup festgelegt, also nichts zu tun.
                return;
            }

            // Berechne die Differenz zwischen der nächsten geplanten Aktion (Backup oder Neustart) und der aktuellen Zeit
            TimeSpan timeDifference = nextActionTime - now;

            // Überprüfe, ob die Zeitdifferenz kleiner oder gleich 0 ist (d.h. die geplante Aktion ist jetzt oder bereits vergangen)
            if (timeDifference <= TimeSpan.Zero)
            {
                // Führe das Backup durch
                PerformBackup();

                // Aktualisiere die nächste geplante Aktion (Backup oder Neustart)
                nextActionTime = GetNextRestartTime();

                // Berechne die neue Zeitdifferenz für die nächste Aktion
                timeDifference = nextActionTime - now;
            }

            // Starte einen Timer, um die nächste Aktion (Backup oder Neustart) auszuführen, wenn die Zeitdifferenz erreicht ist
            if (restartTimer == null)
            {
                restartTimer = new System.Timers.Timer(timeDifference.TotalMilliseconds);
                restartTimer.Elapsed += (sender, e) => OnRestartTimerElapsed(sender, e, nextActionTime);
                restartTimer.AutoReset = false;
                restartTimer.Start();
            }
            else
            {
                restartTimer.Stop();
                restartTimer.Interval = timeDifference.TotalMilliseconds;
                restartTimer.Start();
            }
        }
        private void OnRestartTimerElapsed(object sender, ElapsedEventArgs e, DateTime nextActionTime)
        {
            // Der Timer ist abgelaufen, führe die nächste geplante Aktion (Backup oder Neustart) aus
            PerformBackup();

            // Aktualisiere die nächste geplante Aktion (Backup oder Neustart) für den Timer
            nextActionTime = GetNextRestartTime();

            // Berechne die neue Zeitdifferenz für die nächste Aktion
            TimeSpan timeDifference = nextActionTime - DateTime.Now;

            // Starte den Timer erneut für die nächste geplante Aktion
            restartTimer.Interval = timeDifference.TotalMilliseconds;
            restartTimer.Start();
        }
        private void DeleteOldBackups(string backupSubFolder, int maxBackupCount)
        {
            string backupFolder = Path.Combine(backupFolderPath, backupSubFolder);

            if (!Directory.Exists(backupFolder))
            {
                Debug.WriteLine($"Der {backupSubFolder}-Ordner existiert nicht.");
                return;
            }

            DirectoryInfo directoryInfo = new DirectoryInfo(backupFolder);
            DirectoryInfo[] backupDirectories = directoryInfo.GetDirectories();

            int backupCount = backupDirectories.Length;
            int backupsToDeleteCount = backupCount - maxBackupCount;

            Debug.WriteLine($"Anzahl der vorhandenen Backups im {backupSubFolder}-Ordner: {backupCount}");

            if (backupsToDeleteCount <= 0)
            {
                Debug.WriteLine($"Es gibt weniger als {maxBackupCount} Backups im {backupSubFolder}-Ordner.");
                return;
            }

            for (int i = 0; i < backupsToDeleteCount; i++)
            {
                Debug.WriteLine($"Lösche Backup-Ordner: {backupDirectories[i].Name}");
                backupDirectories[i].Delete(true);
            }

            Debug.WriteLine($"Die ältesten {backupsToDeleteCount} Backups im {backupSubFolder}-Ordner wurden gelöscht.");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!IsServerRunning())
            {
                StartServer();
                Debug.WriteLine("StartSERVER3");
            }
            else
            {
                MessageBox.Show("Der Server läuft bereits.");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (IsServerRunning())
            {
                StopServer();
            }
            else
            {
                MessageBox.Show("Der Server läuft nicht. Starte den Server zuerst, bevor du ihn stoppst.");
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            string command = textBox1.Text.Trim();
            ExecuteCommand(command);
            textBox1.Clear();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (IsServerRunning())
            {
                StopServer();
                System.Threading.Thread.Sleep(5000);
                StartServer();
                Debug.WriteLine("StartSERVER4");
            }
            else
            {
                MessageBox.Show("Der Server läuft nicht. Starte den Server zuerst, bevor du ihn neu startest.");
            }
        }
        private async void button4_Click(object sender, EventArgs e)
        {
            // Stoppe den Server
            StopServer();
            while (IsServerRunning())
            {
                await Task.Delay(100); // Kurze Pause, um der Benutzeroberfläche zu ermöglichen, zu reagieren
            }

            // Warte zusätzliche 5 Sekunden, um sicherzustellen, dass alle Prozesse beendet wurden
            await Task.Delay(5000);

            // Führe das Backup durch
            await Task.Run(() =>
            {
                BackupLogsFolder();
                BackupWorldFolder();
            });

            // Starte den Server wieder
            StartServer();
            UpdateNextRestartTime(); // Aktualisiere das Label für die nächste geplante Neustartzeit

            // Lösche alte Backups
            DeleteOldBackups("world_backup", maxBackupCount);
            DeleteOldBackups("logs_backup", maxBackupCount);
        }
        private void button10_Click(object sender, EventArgs e)
        {
            string logsFolderPath = Path.Combine(workingDirectory, "logs");

            if (Directory.Exists(logsFolderPath))
            {
                Process.Start("explorer.exe", logsFolderPath);
            }
            else
            {
                MessageBox.Show("Der 'logs'-Ordner existiert nicht im Arbeitsverzeichnis.");
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            string configFilePath = Path.Combine(backupFolderPath, "config.txt");

            if (File.Exists(configFilePath))
            {
                Process.Start("notepad.exe", configFilePath);
            }
            else
            {
                MessageBox.Show("Die Konfigurationsdatei existiert nicht.");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string propertiesFilePath = Path.Combine(workingDirectory, "server.properties");

            if (File.Exists(propertiesFilePath))
            {
                Process.Start("notepad.exe", propertiesFilePath);
            }
            else
            {
                MessageBox.Show("Die .properties-Datei existiert nicht im Working Directory.");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (IsServerRunning())
            {
                SendCommandToServer("whitelist reload");
            }
            else
            {
                MessageBox.Show("Der Server läuft nicht. Starte den Server zuerst, bevor du den Befehl ausführst.");
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            LoadConfigurationFromFile();
            // UpdateNextRestartTime(); // Aktualisiere das Label für die nächste geplante Neustartzeit
            MessageBox.Show("MCC Optionen wurden neu geladen.");
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(backupFolderPath))
            {
                Process.Start("explorer.exe", backupFolderPath);
            }
            else
            {
                MessageBox.Show("Der 'MCC_Backup'-Ordner existiert nicht.");
            }
        
    }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            SaveConfigurationInternal();
        }


        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            SaveConfigurationInternal();

            // Aktualisiere das Label für die nächste geplante Neustartzeit
            UpdateNextRestartTime();
            GetNextRestartTime();

        }

    }
}
