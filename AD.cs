using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace AD_Authentication
{
    class AD
    {
        /// <summary>
        /// Active Directory Server/Domain
        /// </summary>
        private static string domain = "AD.com";
  
        /// <summary>
        /// This method is to validate user in a traditional-way using System.DirectoryServices
         /// </summary>
        /// <param name="userName">Username</param>
        /// <param name="password">Password</param>
        /// <returns>true if the credentials are valid, false otherwise</returns>
        public static bool validateUser(string userName, string password)
        {
            DirectoryEntry de = new DirectoryEntry(null, domain +
              "\\" + userName, password);
            try
            {
                object obj = de.NativeObject;
                DirectorySearcher ds = new DirectorySearcher(de);
                ds.Filter = "samaccountname=" + userName;
                ds.PropertiesToLoad.Add("cn");
                SearchResult sr = ds.FindOne();
                if (sr == null) throw new Exception();
                return true;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains(':'))
                {
                    MessageBox.Show(ex.Message.Split(':')[1], ex.Message.Split(':')[0], MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return false;
            }

                              

        }

        /// <summary>
        /// Another way of validating a user is to perform a bind. In this case, the server
        /// queries its own database to validate the credentials. The server defines
        /// how a user is mapped to its directory.
        /// </summary>
        /// <param name="userName">Username</param>
        /// <param name="password">Password</param>
        /// <returns>true if the credentials are valid, false otherwise</returns>
        public static bool validateUserByBind(string userName, string password)
        {
            bool result = true;
            var credentials = new NetworkCredential(userName, password);
            var serverId = new LdapDirectoryIdentifier(domain); 

            var conn = new LdapConnection(serverId, credentials);
            try
            {
                conn.Bind();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                result = false;
            }

            conn.Dispose();

            return result;
        }

        /// <summary>
        /// This method is to check if the user is a member of a group using System.DirectoryServices
        /// </summary>
        /// <param name="userName">Username</param>
        /// <param name="role">Role</param>
        /// <returns>true if the user has the role, false otherwise</returns>
        public static bool IsInRole(string userName, string role)
        {
            try
            {
                role = role.ToLowerInvariant();
                DirectorySearcher ds = new DirectorySearcher(new DirectoryEntry(null));
                ds.Filter = "samaccountname=" + userName;
                SearchResult sr = ds.FindOne();
                DirectoryEntry de = sr.GetDirectoryEntry();
                PropertyValueCollection dir = de.Properties["memberOf"];
                for (int i = 0; i < dir.Count; ++i)
                {
                    string s = dir[i].ToString().Substring(3);
                    s = s.Substring(0, s.IndexOf(',')).ToLowerInvariant();
                    if (s == role) return true;
                }
                throw new Exception();
            }
            catch
            {
                return false;
            }
        }

    }
}
