using System;
using System.Windows.Forms;

namespace CodingTimeTracker
{
    public partial class MainForm : Form
    {
        private readonly AssemblaService _assemblaService;

        private DateTime _startOfWorkDateTime;
        private DateTime _endOfWorkDateTime;

        private int _secs;
        private int _mins;
        private int _hours;

        private int _count;

        public MainForm()
        {
            InitializeComponent();

            btnSave.Enabled = false;

            _assemblaService =
                new AssemblaService("", "");

            SetLblName();
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            if (cbSpace.SelectedIndex != -1 && cbTicket.SelectedIndex != -1)
            {
                tWork.Enabled = !tWork.Enabled;

                if (cbSpace.Enabled)
                {
                    _startOfWorkDateTime = DateTime.Now;
                    cbSpace.Enabled = cbTicket.Enabled = !cbSpace.Enabled;
                }

                if (tWork.Enabled)
                    tWork.Start();
                else
                    tWork.Stop();
            }
            else
                MessageBox.Show(@"Please Select Space And Ticket Before Start", @"Suggestion",
                    MessageBoxButtons.OK);
        }

        private void tWork_Tick(object sender, EventArgs e)
        {
            _secs++;

            if (_secs == 60)
            {
                _secs = 0;
                _mins++;

                btnSave.Enabled = true;

                if (_mins == 60)
                {
                    _mins = 0;
                    _hours++;
                }
            }  
            ShowTime();
        }

        private async void cbSpace_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbTicket.SelectedIndex = -1;

            var tickets = await _assemblaService.GetAssemblaTickets((cbSpace.SelectedItem as Space).wiki_name);

            if (tickets == null)
                return;

            cbTicket.Items.Clear();

            foreach (var ticket in tickets)
                cbTicket.Items.Add(ticket);

            cbTicket.DisplayMember = "summary";
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtDescription.Text))
            {
                MessageBox.Show(@"Description Is Required", @"Suggestion",
                     MessageBoxButtons.OK);
                return;
            }

            if (tWork.Enabled)
            {
                if (MessageBox.Show(@"Time Will Be Stopped", @"Caution",
                    MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                    return;

                tWork.Enabled = false;
            }

            if (MessageBox.Show(@"Are You Sure The Task Is Over?", @"Confirm Save!",
                MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            btnSave.Enabled = false;

            _endOfWorkDateTime = DateTime.Now;

            var assemblaMins = (_mins * 2 / 1.2).ToString();

            if (assemblaMins.Length > 2)
            {
                if (assemblaMins.IndexOf(".", StringComparison.Ordinal) == 1)
                    assemblaMins = '0' + assemblaMins.Substring(0, 4);
                else
                    assemblaMins = assemblaMins.Substring(0, 5);
            }

            var ticket = (cbTicket.SelectedItem as Ticket);

            var taskSaved = await _assemblaService.AddAssemblaTasks(
                    txtDescription.Text,
                    Convert.ToInt32(ticket.Id),
                    ticket.space_id,
                    _startOfWorkDateTime,
                    _endOfWorkDateTime,
                    float.Parse(_hours.ToString() + '.' + assemblaMins.Replace(".", string.Empty))
                    );

            if (!string.IsNullOrEmpty(txtComment.Text))
                await
                    _assemblaService.AddAssemblaTicketComments(ticket.number,
                        (cbSpace.SelectedItem as Space)?.wiki_name, txtComment.Text);

            if (!taskSaved)
            {
                btnSave.Enabled = true;
                return;
            }

            txtDescription.Text = txtComment.Text = "";
            cbSpace.Enabled = cbTicket.Enabled = true;
            cbTicket.SelectedIndex = -1;
            _hours = _mins = _secs = 0;
            btnSave.Enabled = false;
            ShowTime();
        }

        private async void btnGetSpaces_Click(object sender, EventArgs e)
        {
            var spaces = await _assemblaService.GetAssemblaSpaces();

            if (spaces == null)
                return;

            cbSpace.Items.Clear();

            foreach (var space in spaces)
                cbSpace.Items.Add(space);

            cbSpace.DisplayMember = "name";
            cbSpace.ValueMember = "id";
        }

        private void ShowTime()
        {
            lblHr.Text = string.Format(_hours + "  :");
            lblMin.Text = string.Format(_mins + "  :");
            lblSec.Text = _secs.ToString();
        }

        private async void SetLblName()
        {
            if (_count >= 1)
                return;

            _count++;

            var user = await _assemblaService.GetAssemblaName();

            if (user != null)
                lblUser.Text = lblUser.Text + @" " + user.name;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            tWork.Stop();

            var mf = new MainForm();
            mf.Show();
        }
    }
}
