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
    public partial class UserSearchForm : Form
    {
        public UserSearchForm()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                AD _ad = new AD();
                Users user = new Users();
                user = _ad.GetADUser(textBoxDomainID.Text);
                this.textBoxDName.Text = user.DisplayName;
                this.textBoxUID.Text = user.UserName;
                this.textBoxEmail.Text = user.Email;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            
        }
    }
}
