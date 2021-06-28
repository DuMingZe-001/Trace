using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace 伯恩Trace上传程序
{
    public class GlobalVar
    {
        public static bool IsAutoRunning = false;//自动运行标志
        public static bool m_Tcp;
        //public static bool contain = false; //登录结果
        public static bool contain = true; //登录结果
        public static int cbresult;
        public static int AllNum=0;//上传总数
        public static int OKNum=0;//上传成功计数
        public static int NGNum=0;//上传失败计数
        public static int insertNum = 0;//插架成功计数
        public static int insertNumA = 0;//插架成功计数A左框
        public static int insertNumB = 0;//插加成功计数B右框
        public static string uutStarTime;//初始扫码时间
        public static string  m_getresultdata;//Biel服务器Get请求返回数据
        public static bool getTraceJson;//Trace服务器Get请求返回Json数据
        public static string getJson;//Post请求返回Json数据
        public static string getNgResult;//Get请求错误响应状态返回结果
        public static string postNgResult;//Post请求错误响应状态返回结果

        public static string unitSerialNumber = "";//保存第一片白片码数据
        public static string unitSerialNumberSec = "";//保存第二片白片码数据
        public static int resultUnitSerialNumber = 0;
        public static int resultUnitSerialNumberrSec = 0;
        public static int startNumber =1;//序号
        public static int startNumber2 =1;//清零--序号

        //定义自动运行线程
        public static System.Threading.Thread threadAutoRun;
    }

 
}
