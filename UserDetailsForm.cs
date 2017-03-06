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
    public partial class UserDetailsForm : Form
    {
        public UserDetailsForm()
        {
            InitializeComponent();
        }

        public UserDetailsForm(string UserName, string DisplayName, string Email)
        {
            InitializeComponent();
            this.textBoxDName.Text  = DisplayName;
            this.textBoxUID.Text = UserName;
            this.textBoxEmail.Text = Email;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
