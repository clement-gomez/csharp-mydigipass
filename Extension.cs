using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Vasco.Web.Authentication.Extensions
{
    public static class Extensions
    {
        public static string GetToken(this MYDIGIPASSAuth mydigipass)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(mydigipass.ApiTokenUrl);
            request.InitializeRequest("POST", "application/x-www-form-urlencoded");

            if (!String.IsNullOrEmpty(mydigipass.Token))
            {
                request.Headers["Authorization"] = String.Format("Bearer {0}", mydigipass.Token);
            }

            request.WriteStream(mydigipass.TokenParameters);

            return request.GetWebResponse();
        }

        public static string Authenticate(this MYDIGIPASSAuth mydigipass)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(mydigipass.ApiAuthorizationUrl);

            request.InitializeRequest("GET", null);

            if (!String.IsNullOrEmpty(mydigipass.Token))
            {
                request.Headers["Authorization"] = String.Format("Bearer {0}", mydigipass.Token);
            }

            return request.GetWebResponse();
        }

        public static string Connect(this MYDIGIPASSAuth mydigipass)
        {
            mydigipass.Uuids = mydigipass.Uuid.ToList(); // make sure only 1 uuid is passed

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(mydigipass.ApiConnectUrl);
            request.InitializeRequest("POST", "application/json");
            request.BasicAuthentication(mydigipass);
            request.WriteStream(mydigipass);

            return request.GetWebResponse();          
        }

        public static string Connected(this MYDIGIPASSAuth mydigipass)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(mydigipass.ApiConnectUrl);
            request.InitializeRequest("GET", null);
            request.BasicAuthentication(mydigipass);

            return request.GetWebResponse();
        }

        public static string Disconnect(this MYDIGIPASSAuth mydigipass)
        {
            mydigipass.Uuids = mydigipass.Uuid.ToList(); // make sure only 1 uuid is passed

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(mydigipass.ApiDisconnectUrl);
            request.InitializeRequest("POST", "application/json");
            request.BasicAuthentication(mydigipass);
            request.WriteStream(mydigipass);

            return request.GetWebResponse();

        }

        private static HttpWebRequest InitializeRequest(this HttpWebRequest request, string method, string contentType)
        {
            request.Method = method;
            request.ServicePoint.Expect100Continue = false;
            request.Timeout = 20000;
            request.ContentType = contentType;

            return request;
        }

        private static HttpWebRequest BasicAuthentication(this HttpWebRequest request, MYDIGIPASSAuth mydigipass)
        {
            if (!String.IsNullOrEmpty(mydigipass.ApiKey) && !String.IsNullOrEmpty(mydigipass.ApiSecret))
            {
                string authInfo = mydigipass.ApiKey + ":" + mydigipass.ApiSecret;
                authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
                request.Headers["Authorization"] = "Basic " + authInfo;
            }

            return request;
        }

        private static string GetWebResponse(this HttpWebRequest request)
        {
            StreamReader responseReader = null;
            string responseData = string.Empty;
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    responseReader = new StreamReader(response.GetResponseStream());
                    responseData = responseReader.ReadToEnd();
                }
            }
            catch (WebException e)
            {
                using (WebResponse response = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    using (Stream data = response.GetResponseStream())
                    {
                        string responseError = new StreamReader(data).ReadToEnd();
                        throw new System.Exception(responseError);
                    }
                }
            }
            finally
            {
                responseReader.Close();
                responseReader = null;
            }
            return responseData;
        }

        private static void WriteStream(this HttpWebRequest request, MYDIGIPASSAuth mydigipass)
        {
            string jsonBody = JsonConvert.SerializeObject(mydigipass,
                                                          Newtonsoft.Json.Formatting.None,
                                                          new JsonSerializerSettings { });
            request.WriteStream(jsonBody);
            
        }
        private static void WriteStream(this HttpWebRequest request, string value)
        {
            byte[] encoded = System.Text.Encoding.UTF8.GetBytes(value);
            request.ContentLength = encoded.Length;
            StreamWriter writer = new StreamWriter(request.GetRequestStream());
            try
            {
                writer.Write(value);
            }
            catch (Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
            finally
            {
                writer.Close();
                writer = null;
            } 
        }

        public static List<string> ToList(this string value)
        {
            List<string> list = new List<string>();
            list.Add(value);

            return list;
        }

    }
}
