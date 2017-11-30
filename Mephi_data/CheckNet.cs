using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.IO;

namespace Mephi_data
{
    public class CheckNet
    {
        #region StatusCode
        private static IEnumerable<HttpStatusCode> onlineStatusCodes = new[]
        {
        HttpStatusCode.Accepted,
        HttpStatusCode.Found,
        HttpStatusCode.OK,
    };
        #endregion
        public static bool isOnline(string URL)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Timeout = 3000;
            try
            {
                WebResponse resp = request.GetResponse();
            }
            catch (WebException e)
            {
                if (((HttpWebResponse)e.Response).StatusCode == HttpStatusCode.NotFound)
                    return false;
            }
            return true;
        }

        public static bool CheckURL(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = 15000;
            request.Method = "HEAD";
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    return response.StatusCode == HttpStatusCode.OK;
                }
            }
            catch (WebException)
            {
                return false;
            }
        }

    }
}
