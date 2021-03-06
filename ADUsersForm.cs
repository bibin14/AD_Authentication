﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AD_Authentication
{
    public partial class ADUsersForm : Form
    {
        public ADUsersForm()
        {
            InitializeComponent();
        }

        private void ADUsersForm_Load(object sender, EventArgs e)
        {
            List<Users> lstADUsers = new List<Users>();
            AD _ad = new AD();
            lstADUsers = _ad.GetADUsers();
            foreach (Users _user in lstADUsers)
            {
                Button btnUser = new Button();
                btnUser.Text = _user.DisplayName;
                btnUser.Tag = _user.Email;
                btnUser.Name = _user.UserName;  
                btnUser.Size = new System.Drawing.Size(100, 100);
                btnUser.Image = global::AD_Authentication.Properties.Resources.user_img;
                btnUser.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
                btnUser.Click += new System.EventHandler(OnbtnUserClick);
                this.flowLayoutPanel1.Controls.Add(btnUser);
                
                /// To load only 250 users
                if (this.flowLayoutPanel1.Controls.Count > 250)
                {
                    break;
                }
            }

        }

        void OnbtnUserClick(object sender, EventArgs e)
        {
            UserDetailsForm usrDt = new UserDetailsForm(((Button)sender).Name, ((Button)sender).Text, ((Button)sender).Tag.ToString());
            usrDt.ShowDialog(this);
        }

    }
}
