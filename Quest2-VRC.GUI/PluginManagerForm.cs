using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;

namespace Quest2_VRC
{
    public class PluginManagerForm : MaterialForm
    {
        private MaterialListView _listView;
        private MaterialButton _btnRefresh;
        private MaterialButton _btnStart;
        private MaterialButton _btnStop;
        private MaterialButton _btnStartAll;
        private MaterialButton _btnStopAll;

        // Runtime state only — plugins are still loaded and started by Program.GUI
        private readonly Dictionary<IPlugin, bool> _started = new();

        public PluginManagerForm()
        {
            Text = "Plugins";
            Size = new Size(820, 480);
            StartPosition = FormStartPosition.CenterParent;

            var ms = MaterialSkinManager.Instance;
            ms.AddFormToManage(this);
            ms.Theme = MaterialSkinManager.Themes.DARK;
            ms.ColorScheme = new ColorScheme(Primary.Amber800, Primary.Amber900, Primary.Cyan500, Accent.Cyan700, TextShade.WHITE);

            InitializeComponents();

            Load += PluginManagerForm_Load;
            FormClosing += PluginManagerForm_FormClosing;
        }

        private void InitializeComponents()
        {
            // Buttons panel
            var panel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 48,
                Padding = new Padding(6),
                FlowDirection = FlowDirection.LeftToRight
            };

            _btnRefresh = new MaterialButton { Text = "Refresh", Width = 110 };
            _btnStart = new MaterialButton { Text = "Start", Width = 110 };
            _btnStop = new MaterialButton { Text = "Stop", Width = 110 };
            _btnStartAll = new MaterialButton { Text = "Start All", Width = 110 };
            _btnStopAll = new MaterialButton { Text = "Stop All", Width = 110 };

            panel.Controls.Add(_btnRefresh);
            panel.Controls.Add(_btnStart);
            panel.Controls.Add(_btnStop);
            panel.Controls.Add(_btnStartAll);
            panel.Controls.Add(_btnStopAll);

            // MaterialListView for plugins
            _listView = new MaterialListView
            {
                View = View.Details,
                FullRowSelect = true,
                MultiSelect = false,
                Dock = DockStyle.Fill,
                AutoSize = true
            };
            _listView.Columns.Add("Name", 250);
            _listView.Columns.Add("Description", 450);
            _listView.Columns.Add("Status", 100);

            Controls.Add(_listView);
            Controls.Add(panel);

            // Event handlers
            _btnRefresh.Click += BtnRefresh_Click;
            _btnStart.Click += BtnStart_Click;
            _btnStop.Click += BtnStop_Click;
            _btnStartAll.Click += BtnStartAll_Click;
            _btnStopAll.Click += BtnStopAll_Click;
            _listView.DoubleClick += ListView_DoubleClick;
            _listView.SelectedIndexChanged += ListView_SelectedIndexChanged;
        }

        private void PluginManagerForm_Load(object? sender, EventArgs e)
        {
            if (PluginLoader.LoadedPlugins.Count == 0)
                PluginLoader.LoadPlugins();

            // Assume plugins were started by the application at startup
            foreach (var p in PluginLoader.LoadedPlugins)
            {
                if (!_started.ContainsKey(p))
                    _started[p] = true; // reflect default behavior where Program.StartAll() started plugins
            }

            UpdateList();
        }

        private void ListView_SelectedIndexChanged(object? sender, EventArgs e)
        {
            UpdateButtons();
        }

        private void BtnRefresh_Click(object? sender, EventArgs e)
        {
            UpdateList();
        }

        private void BtnStart_Click(object? sender, EventArgs e)
        {
            var plugin = GetSelectedPlugin();
            if (plugin == null) return;
            StartPlugin(plugin);
        }

        private void BtnStop_Click(object? sender, EventArgs e)
        {
            var plugin = GetSelectedPlugin();
            if (plugin == null) return;
            StopPlugin(plugin);
        }

        private void BtnStartAll_Click(object? sender, EventArgs e)
        {
            foreach (var plugin in PluginLoader.LoadedPlugins)
                StartPlugin(plugin);
        }

        private void BtnStopAll_Click(object? sender, EventArgs e)
        {
            foreach (var plugin in PluginLoader.LoadedPlugins)
                StopPlugin(plugin);
        }

        private void ListView_DoubleClick(object? sender, EventArgs e)
        {
            var plugin = GetSelectedPlugin();
            if (plugin == null) return;
            if (_started.TryGetValue(plugin, out var running) && running)
                StopPlugin(plugin);
            else
                StartPlugin(plugin);
        }

        private IPlugin? GetSelectedPlugin()
        {
            if (_listView.SelectedItems.Count == 0) return null;
            var idx = _listView.SelectedItems[0].Index;
            if (idx < 0 || idx >= PluginLoader.LoadedPlugins.Count) return null;
            return PluginLoader.LoadedPlugins[idx];
        }

        private void StartPlugin(IPlugin plugin)
        {
            try
            {
                plugin.Start();
                _started[plugin] = true;
                UpdateList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error starting {plugin.Name}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StopPlugin(IPlugin plugin)
        {
            try
            {
                plugin.Stop();
                _started[plugin] = false;
                UpdateList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error stopping {plugin.Name}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateList()
        {
            _listView.BeginUpdate();
            _listView.Items.Clear();
            foreach (var plugin in PluginLoader.LoadedPlugins)
            {
                var running = _started.TryGetValue(plugin, out var r) && r;
                var status = running ? "Running" : "Stopped";
                var item = new ListViewItem(new[] { plugin.Name, plugin.Description, status });
                _listView.Items.Add(item);
            }
            _listView.EndUpdate();
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            var plugin = GetSelectedPlugin();
            _btnStart.Enabled = plugin != null;
            _btnStop.Enabled = plugin != null;
        }

        private void PluginManagerForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            // no persistent state to save; plugins remain managed by main app
        }
    }
}