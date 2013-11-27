using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using Newtonsoft.Json;
using Vasco.Web.Authentication.Extensions;

namespace Vasco.Web.Authentication
{
    [DataContract]
    public class MYDIGIPASSAuth
    {
        #region Properties

        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
        public string ApiCode { get; set; }
        public string ApiRedirectUri { get; set; }

        public string ApiTokenUrl { get; set; }
        public string ApiAuthorizationUrl { get; set; }
        public string ApiConnectUrl { get; set; }
        public string ApiDisconnectUrl { get; set; }

        [DataMember(Name = "access_token")]
        public string Token { get; set; }
        [DataMember(Name = "email")]
        public string Email { get; set; }
        [DataMember(Name = "uuid")]
        public string Uuid { get; set; }
        [DataMember(Name = "uuids")]
        public List<string> Uuids { get; set; }

        public Dictionary<string, string> Data { get; set; }

        public string TokenParameters
        {
            set { }
            get
            {
                return String.Format("code={0}&client_id={1}&client_secret={2}&redirect_uri={3}&grant_type=authorization_code",
                                      this.ApiCode, 
                                      this.ApiKey, 
                                      this.ApiSecret, 
                                      this.ApiRedirectUri);
            }
        }

        public string ApiTokenUrlWithParameters
        {
            set { }
            get
            {
                if (!String.IsNullOrEmpty(this.ApiTokenUrl))
                {
                    return String.Format("{0}?{1}", this.ApiTokenUrl,
                                                    this.TokenParameters);
                }
                return String.Empty;
            }
        }

        public bool HasToken
        {
            set { }
            get
            {
                if (this.Token.Length > 0)
                {
                    return true;
                }
                return false;
            }
        }

        #endregion

        #region Public Methods

        public void AccessToken()
        {
            try
            {
                string response = this.GetToken();
                if (response.Length > 0)
                {
                    MYDIGIPASSAuth mydpResponse = (MYDIGIPASSAuth)JsonConvert.DeserializeObject(response, typeof(MYDIGIPASSAuth));
                    this.Token = mydpResponse.Token;
                }
            }
            catch (Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
        }

        public void Authorization()
        {
            try
            {
                string response = this.Authenticate();
                if (response.Length > 0)
                {
                    MYDIGIPASSAuth mydpResponse = (MYDIGIPASSAuth)JsonConvert.DeserializeObject(response, typeof(MYDIGIPASSAuth));
                    this.Email = mydpResponse.Email;
                    this.Uuid = mydpResponse.Uuid;
                    this.Data = (Dictionary<string, string>)JsonConvert.DeserializeObject(response, typeof(Dictionary<string, string>));
                }
            }
            catch (Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
        }

        public void ConnectUser()
        {
            try
            {
                string response = this.Connect();
                if (response.Length > 0)
                {
                    // Store UUID in your web application database if missing
                }
            }
            catch (Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
        }

        public void ConnectedUsers()
        {
            try
            {
                string response = this.Connected();
                if (response.Length > 0)
                {
                    MYDIGIPASSAuth mydpResponse = (MYDIGIPASSAuth)JsonConvert.DeserializeObject(response, typeof(MYDIGIPASSAuth));
                    this.Uuids = mydpResponse.Uuids;
                }
            }
            catch (Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
        }

        public void DisconnectUser()
        {
            try
            {
                string response = this.Disconnect();
                if (response.Length > 0)
                {
                    // Delete UUID in your web application database
                }
            }
            catch (Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
        }

        #endregion
    }
}
