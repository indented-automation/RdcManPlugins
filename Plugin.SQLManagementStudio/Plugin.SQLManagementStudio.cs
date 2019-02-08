using RdcMan;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace Plugin.SQLManagementStudio
{
    [Export(typeof(IPlugin))]
    public class PluginSQLManagementStudio : IPlugin
    {
        private ServerBase server;

        public void OnContextMenu(System.Windows.Forms.ContextMenuStrip contextMenuStrip, RdcTreeNode node)
        {
            if ((node as GroupBase) == null)
            {
                if ((node as ServerBase) != null)
                {
                    this.server = node as ServerBase;

                    ToolStripMenuItem NewMenuItem = new DelegateMenuItem("Open SQL Management Studio", MenuNames.None, () => this.OpenManagementStudio());
                    NewMenuItem.Image = Properties.Resources.ssms;

                    contextMenuStrip.Items.Insert(contextMenuStrip.Items.Count - 1, NewMenuItem);
                    contextMenuStrip.Items.Insert(contextMenuStrip.Items.Count - 1, new ToolStripSeparator());
                }
            }
        }

        public void OnDockServer(ServerBase server)
        {
        }

        public void OnUndockServer(IUndockedServerForm form)
        {
        }

        public void PostLoad(IPluginContext context)
        {
        }

        public void PreLoad(IPluginContext context, XmlNode xmlNode)
        {
        }

        public XmlNode SaveSettings()
        {
            return null;
        }

        public void Shutdown()
        {
        }

        private string ShowPasswordInputBox()
        {
            PasswordRequestForm passwordRequestForm = new PasswordRequestForm();
            DialogResult result = passwordRequestForm.ShowDialog();

            if (result == DialogResult.OK)
            {
                return passwordRequestForm.Password;
            }
            else
            {
                return "";
            }
        }

        private string GetSsmsPath()
        {
            var pathSearchInfo = new[] {
                new {
                    basePath  = Path.Combine(Environment.GetEnvironmentVariable("PROGRAMFILES(x86)"), "Microsoft SQL Server"),
                    pattern   = "*",
                    childPath = @"Tools\Binn\ManagementStudio\Ssms.exe"
                },
                new {
                    basePath  = Environment.GetEnvironmentVariable("PROGRAMFILES(x86)"),
                    pattern   = "Microsoft SQL Server Management Studio *",
                    childPath = @"Common7\IDE\Ssms.exe"
                }
            };

            string path = pathSearchInfo
                .AsEnumerable()
                .SelectMany( searchInfo =>
                    Directory.GetDirectories(
                        searchInfo.basePath,
                        searchInfo.pattern,
                        SearchOption.TopDirectoryOnly
                    )
                    .AsEnumerable()
                    .Select( ssmsPath =>
                        new FileInfo(Path.Combine(ssmsPath, searchInfo.childPath))
                    )
                )
                .Where( fileInfo => fileInfo.Exists )
                .OrderBy( fileInfo => new Version(FileVersionInfo.GetVersionInfo(fileInfo.FullName).ProductVersion) )
                .Select( fileInfo => fileInfo.FullName )
                .LastOrDefault();

            if (path != null)
            {
                return path;
            }
            else
            {
                throw new InvalidOperationException("Failed to discover an SSMS version to use");
            }
        }

        public void OpenManagementStudio()
        {
            try
            {
                string command = String.Format(@"""{0}"" -S {1} -E",
                    this.GetSsmsPath(),
                    this.server.ServerName
                );

                if (String.IsNullOrWhiteSpace(this.server.UserName.ToString()))
                {
                    Process.Start(command);
                }
                else
                {
                    string password = String.IsNullOrWhiteSpace(this.server.Password.ToString()) ? this.ShowPasswordInputBox() : this.server.Password.ToString();

                    if (!String.IsNullOrWhiteSpace(password))
                    {
                        RunAsHelper.RunAs(
                            command,
                            this.server.Domain.ToString(),
                            this.server.UserName.ToString(),
                            password
                        );
                    }
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
