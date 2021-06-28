/********************************************
 *  Ver 2021/02/10
 *  By DuMingZe
 ********************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace 伯恩Trace上传程序
{
    public class HttpUitls
    {
        frm_main frm = new frm_main();
        public static string m_getstatusdescription;
        public static string m_poststatusDescription;

        /// <summary>
        /// 创建Get方式的HTTP请求
        /// </summary>
        /// <param name="Url"></param>
        /// <returns></returns>
        public static string Get(string Url)
        {
            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.Proxy = null;
                request.KeepAlive = false;
                request.Method = "GET";
                request.ContentType = "application/json; charset=UTF-8";
                request.AutomaticDecompression = DecompressionMethods.GZip;

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();//服务器返回数据
                                                                                  //m_getstatusCode = (HttpStatusCode)Convert.ToInt32(response.StatusCode);//获取服务器状态码
                m_getstatusdescription = response.StatusDescription;//获取服务器响应状态
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
                string retString = myStreamReader.ReadToEnd();

                myStreamReader.Close();
                myResponseStream.Close();

                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
                return retString;
            }
            catch (Exception ex)
            {    
                if (ex.Message.Length > 12)
                {
                    GlobalVar.getNgResult = ex.Message.Substring(12, 3);
                    m_getstatusdescription = "NG";
                    return GlobalVar.getNgResult;
                }
                else
                {
                    GlobalVar.getNgResult = "ER";
                    m_getstatusdescription = "NG";
                    return GlobalVar.getNgResult;
                }
              
            }
        }


        /// <summary>
        /// 创建Post方式的HTTP请求
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="Data"></param>
        /// <param name="Referer"></param>
        /// <returns></returns>
        public static string Post(string Url, string Data, string Referer)
        {
            try
            {
                string retString;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.Method = "POST";
                request.Referer = Referer;
                byte[] bytes = Encoding.UTF8.GetBytes(Data);
                //request.ContentType = "application/x-www-form-urlencoded";
                request.ContentType = "application/json; charset=UTF-8";
                request.ContentLength = bytes.Length;
                Stream myResponseStream = request.GetRequestStream();
                myResponseStream.Write(bytes, 0, bytes.Length);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                m_poststatusDescription = response.StatusDescription;//获取服务器响应状态
                                                                     // m_poststatusCode = (HttpStatusCode)Convert.ToInt32(response.StatusCode);//获取服务器状态码
                StreamReader myStreamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                retString = myStreamReader.ReadToEnd();

                myStreamReader.Close();
                myResponseStream.Close();

                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
                return retString;

            }
            catch (Exception ex)
            {

                GlobalVar.postNgResult = ex.Message.Substring(12, 3);
                m_poststatusDescription = "NG";
                return GlobalVar.postNgResult;
            }
        }

    }


}
