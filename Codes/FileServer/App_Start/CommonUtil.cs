using FileServer.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace FileServer
{
    public class CommonUtil
    {
        private static readonly string PUBLICFILEPATH = ConfigurationManager.AppSettings["publicKey"];
        private static readonly string PRIVATEFILEPATH = ConfigurationManager.AppSettings["privateKey"];
        private static readonly string ISCHECKUSER = ConfigurationManager.AppSettings["IsCheckUser"];
        private const string COOKIENAME = "FILESERVERUSER";
        private const string USERTOKEN = "usertoken";
        public void CheckUser(HttpContext context)
        {
            try
            {
                bool isCheckUser = true;
                bool.TryParse(ISCHECKUSER, out isCheckUser);
                if (isCheckUser)
                {
                    string usertokens = string.Empty;
                    if (context.Request[USERTOKEN] != null)
                    {
                        usertokens = context.Request[USERTOKEN];
                        //usertokens = HttpUtility.UrlDecode(usertokens);
                    }
                    if (context.Request != null && context.Request.Cookies[COOKIENAME] != null)
                    {
                        usertokens = context.Request.Cookies[COOKIENAME].ToString();
                    }
                    if (!string.IsNullOrEmpty(usertokens))
                    {
                        RSAHelp rsa = new RSAHelp();
                        FileHelp fileHelp = new FileHelp();
                        string privateKey = fileHelp.ReadTxtFile(PRIVATEFILEPATH);
                        string user = rsa.RsaDecrypt(usertokens, privateKey);
                        if (!CheckSysUser(user))
                        {
                            context.Response.Clear();
                            context.Response.StatusCode = 404;
                            context.ApplicationInstance.CompleteRequest();
                        }
                    }
                    else
                    {
                        context.Response.Clear();
                        context.Response.StatusCode = 404;
                        context.ApplicationInstance.CompleteRequest();
                    }
                }
            }
            catch(Exception e)
            {
                NLog.LogManager.GetCurrentClassLogger().Error(e.Message);
                context.Response.Clear();
                context.Response.StatusCode = 500;
                context.ApplicationInstance.CompleteRequest();
            }
        }

        private bool CheckSysUser(string user)
        {
            List<string> strLst = GetUserSection();
            if (strLst != null)
            {
                return strLst.Contains(user);
            }
            else
            {
                return false;
            }
        }

        private List<string> GetUserSection()
        {
            List<string> strLst = new List<string>();
            UserManageSection userManageSection = (UserManageSection)ConfigurationManager.GetSection("UserManageSection");
            foreach (UserManageSectionElement userManageSectionElement in userManageSection.KeyVaules)
            {
                strLst.Add(userManageSectionElement.User);
            }
            return strLst;
        }
    }
}