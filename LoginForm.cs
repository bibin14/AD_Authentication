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
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            MainForm _mainFrm = new AD_Authentication.MainForm();
            _mainFrm.loggedInUser = "";
            if (comboBoxADMethod.Text=="Using LDAP Bind")
	        {
               if (AD.validateUserByBind(this.textBoxUName.Text, this.textBoxPwd.Text) == true)
               {
                    MessageBox.Show("Logged in successfully", "Login");
                    // bind the loggedin user id
                    _mainFrm.loggedInUser = "Logged in as " + this.textBoxUName.Text.ToUpper();
                    this.Close();
                }

            }
            else
            {
                if (AD.validateUser(this.textBoxUName.Text, this.textBoxPwd.Text) == true)
                {
                    MessageBox.Show("Logged in successfully", "Login");
                    // bind the loggedin user id
                    _mainFrm.loggedInUser = "Logged in as " + this.textBoxUName.Text.ToUpper();
                    this.Close();
                }

            }
 
        }
    }
}
