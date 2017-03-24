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

        /// <summary>
        /// This method is to retrive all users from AD
        /// </summary>
        public List<Users> GetADUsers()
        {
            List<Users> lstADUsers = new List<Users>();
            try
            {
                DirectorySearcher search = new DirectorySearcher();
                search.Filter = "(&(objectClass=user)(objectCategory=person))";
                search.PropertiesToLoad.Add("samaccountname");
                search.PropertiesToLoad.Add("mail");
                search.PropertiesToLoad.Add("usergroup");
                search.PropertiesToLoad.Add("displayname");
                SearchResult result;
                SearchResultCollection resultCol = search.FindAll();
                if (resultCol != null)
                {
                    for (int counter = 0; counter < resultCol.Count; counter++)
                    {
                        string UserNameEmailString = string.Empty;
                        result = resultCol[counter];
                        if (result.Properties.Contains("samaccountname") &&
                                 result.Properties.Contains("mail") &&
                            result.Properties.Contains("displayname"))
                        {
                            Users objSurveyUsers = new Users();
                            objSurveyUsers.Email = (String)result.Properties["mail"][0] +
                              "^" + (String)result.Properties["displayname"][0];
                            objSurveyUsers.UserName = (String)result.Properties["samaccountname"][0];
                            objSurveyUsers.DisplayName = (String)result.Properties["displayname"][0];
                            objSurveyUsers.isMapped = true;
                            lstADUsers.Add(objSurveyUsers);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return lstADUsers;

        }

        public Users GetADUser(string DomainID)
        {
           Users adUsers = new Users();
            try
            {
                DirectorySearcher search = new DirectorySearcher();
                search.Filter = "(&(objectClass=user)(samaccountname=" + DomainID + "))";
                search.PropertiesToLoad.Add("samaccountname");
                search.PropertiesToLoad.Add("mail");
                search.PropertiesToLoad.Add("usergroup");
                search.PropertiesToLoad.Add("displayname");
                SearchResult result = search.FindOne();
                if (result != null)
                {
                    if (result.Properties.Contains("samaccountname") &&
                                 result.Properties.Contains("mail") &&
                            result.Properties.Contains("displayname"))
                    {
                        adUsers.Email = (String)result.Properties["mail"][0] +
                          "^" + (String)result.Properties["displayname"][0];
                        adUsers.UserName = (String)result.Properties["samaccountname"][0];
                        adUsers.DisplayName = (String)result.Properties["displayname"][0];
                        adUsers.isMapped = true;
                   }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return adUsers;

        }
    }
}