using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AD_Authentication
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            LoginForm login = new LoginForm();
            login.ShowDialog(this);
        }

        private void btnGetUsers_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not Implemented !!!", "Work in progress");
        }

        private void btnGetUserDetails_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not Implemented !!!", "Work in progress");
        }
    }
}
