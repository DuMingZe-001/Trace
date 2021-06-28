using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 伯恩Trace上传程序
{
    /// <summary>
    /// Used for usermanager
    /// </summary>
    public class User
    {
        public User(string name, string code, int authority)
        {
            _Name = name;
            _Code = code;
            _Authority = authority;
        }

        private string _Name = null;
        private string _Code = null;
        private int _Authority = 0;

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public string Code
        {
            get { return _Code; }
            set { _Code = value; }
        }

        public int Authority
        {
            get { return _Authority; }
            set { _Authority = value; }
        }
        //用户名和密码是否匹配
        public bool Equal(string name, string code,int authority)
        {
            if (_Name == name && _Code == code&& _Authority== authority)
                return true;
            else
                return false;
        }
    }
}
