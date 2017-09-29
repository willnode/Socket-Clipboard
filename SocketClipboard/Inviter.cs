using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SocketClipboard
{
    public partial class Inviter : Form
    {

        public List<string> localhosts = new List<string>();

        public Inviter(List<NetClient> clients)
        {
            InitializeComponent();
            var local = SystemInformation.ComputerName;
            var font = new Font(Font, FontStyle.Italic);
            InviterUtility.GetLocalComputers(localhosts);

            for (int i = localhosts.Count - 1; i >= 0; i--)
            {
                var host = localhosts[i];
                if (host == local) { localhosts.RemoveAt(i); continue; }

                var exist = clients.Any((x) => x.Name == host);
                _list.Items.Add(new ListViewItem(host)
                {
                    Checked = !exist,
                    Font = exist ? font : Font,
                    ForeColor = exist ? Color.Gray : Color.Black
                });
            }
        }

        private void _cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void _add_Click(object sender, EventArgs e)
        {
            for (int i = localhosts.Count - 1; i >= 0; i--)
            {
                if (!_list.Items[i].Checked)
                    localhosts.RemoveAt(i);
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void _manual_Click(object sender, EventArgs e)
        {
            string input = string.Empty;
            if (Utility.ShowInputDialog(ref input, "Enter a Hostname Manually") == DialogResult.OK)
            {
                localhosts.Clear();
                localhosts.Add(input);
                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
