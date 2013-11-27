using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Vasco.Web.Authentication;

namespace MYDIGIPASS.COM
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["code"] != null && !IsPostBack)
            {
                MYDIGIPASSAuth mydigipass = new MYDIGIPASSAuth();
                mydigipass.ApiTokenUrl = "https://sandbox.mydigipass.com/oauth/token";
                mydigipass.ApiAuthorizationUrl = "https://sandbox.mydigipass.com/oauth/user_data";
                mydigipass.ApiRedirectUri = "{redirect-uri}";
                mydigipass.ApiConnectUrl = "{api-connected-url}";
                mydigipass.ApiDisconnectUrl = "{api-dissconect-url}";
                mydigipass.ApiKey = "{client-id}";
                mydigipass.ApiSecret = "{client-secret}";
                mydigipass.ApiCode = Request["code"];

                mydigipass.AccessToken();

                if (mydigipass.HasToken)
                {
                    mydigipass.Authorization();

                    if (!String.IsNullOrEmpty(mydigipass.Email))
                    {
                        // Object stored in Session only for test purpose
                        HttpContext.Current.Session["MYDPSession"] = mydigipass;
                        HttpContext.Current.Session.Timeout = 20;

                        // Store UUID in your local database and then confirm user is connected to mydigipass.com
                        mydigipass.ConnectUser();

                        // Show the "ConnectedUsers" and "DisconnectUser" buttons for test purpose
                        _btnConnectedUsers.Enabled = true;
                        _btnDisconnectUser.Enabled = true;   
                    }                    
                }
            }

            // Only for test purpose
            if ((MYDIGIPASSAuth)HttpContext.Current.Session["MYDPSession"] != null)
            {
                MYDIGIPASSAuth mydigipass = (MYDIGIPASSAuth)HttpContext.Current.Session["MYDPSession"];
                Response.Write(String.Format("<div>Successfully logged in with MYDIGIPASS.COM as: </div>", mydigipass.Email));
            }

            if (Request["error"] != null)
            {
                Response.Write(String.Format("<div>Error: {0}, {1}</div>", Request["error"], Request["error_description"]));
            }
        }

        protected void _btnConnectedUsers_Click(object sender, EventArgs e)
        {
            MYDIGIPASSAuth mydigipass = (MYDIGIPASSAuth)HttpContext.Current.Session["MYDPSession"];

            if (mydigipass != null && mydigipass.Data != null)
            {                
                mydigipass.ConnectedUsers();

                Response.Write("<div>List of UUIDs:</div><ul>");
                foreach (string uuid in mydigipass.Uuids)
                {
                    Response.Write("<li>" + uuid + "</li>");
                }
                Response.Write("</ul>");
            }
        }

        protected void _btnDisconnectUser_Click(object sender, EventArgs e)
        {
            MYDIGIPASSAuth mydigipass = (MYDIGIPASSAuth)HttpContext.Current.Session["MYDPSession"];

            if (mydigipass != null && mydigipass.Data != null)
            {
                mydigipass.DisconnectUser();

                Response.Write("<div>Disconnected!</div>");
            }
        }
    }
}
