using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 伯恩Trace上传程序
{

    //Json发送----单片条码上传

    //public class Propertiess
    //{
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public string Factory { get; set; }
    //}

    //public class Data
    //{
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public Insight insight { get; set; }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public Propertiess properties { get; set; }
    //}

    //public class Root
    //{
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public Serials serials { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public Data data { get; set; }
    //}


    //public class Serials
    //{
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public string bg_ss { get; set; }
    //}

    //public class Test_attributes
    //{
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public string test_result { get; set; }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public string unit_serial_number { get; set; }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public string uut_start { get; set; }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public string uut_stop { get; set; }
    //}

    //public class Test_station_attributes
    //{
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public string fixture_id { get; set; }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public string head_id { get; set; }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public string line_id { get; set; }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public string software_name { get; set; }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public string software_version { get; set; }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public string station_id { get; set; }
    //}

    //public class Uut_attributes
    //{
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public string STATION_STRING { get; set; }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public string rack_id { get; set; }
    //}

    //public class Insight
    //{
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public Test_attributes test_attributes { get; set; }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public Test_station_attributes test_station_attributes { get; set; }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public Uut_attributes uut_attributes { get; set; }
    //}


    //Json发送----多片条码上传
    public class TestAttributes
    {
        public string test_result { get; set; }
        public string unit_serial_number { get; set; }
        public string uut_start { get; set; }
        public string uut_stop { get; set; }
    }

    public class TestStationAttributes
    {
        public string fixture_id { get; set; }
        public string head_id { get; set; }
        public string line_id { get; set; }
        public string software_name { get; set; }
        public string software_version { get; set; }
        public string station_id { get; set; }
    }

    public class UutAttributes
    {
        public string STATION_STRING { get; set; }
        public string rack_id { get; set; }
    }

    public class Insight
    {
        public TestAttributes test_attributes { get; set; }
        public TestStationAttributes test_station_attributes { get; set; }
        public UutAttributes uut_attributes { get; set; }
    }

    public class Properties2
    {
        public string Factory { get; set; }
    }

    public class Data
    {
        public Insight insight { get; set; }
        public Properties2 properties { get; set; }
    }

    public class Root
    {
        public string serial_type { get; set; }
        // public IList<string> serials { get; set; } 
        public List<string> serials { get; set; }

        public Data data { get; set; }
    }


    //Json解析---Trace服务器Get请求
    public class Root2
    {
        public bool pass { get; set; }
        //public IList<object> processes { get; set; }

        public List<object> processes { get; set; }
        public object choice_ids { get; set; }
        public string control_id { get; set; }
    }

    //Json解析---Trace服务器Post请求
    public class Root3
    {
        public string id { get; set; }
        public string serial_type { get; set; }
        public string serial { get; set; }
    }


}

