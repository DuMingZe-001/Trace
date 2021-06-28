using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ProFile
{
    public class CProfIniFile
    {
        [DllImport("kernel32", CharSet = CharSet.Ansi)]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32", CharSet = CharSet.Ansi)]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        private string m_strFileName = null;
        public static object locker = new object();

        public CProfIniFile(string strFileName)
        {
            m_strFileName = strFileName;
        }

        public void Write(string section, string key, string value)
        {
            lock (locker)
            {
                try
                {
                    WritePrivateProfileString(section, key, value, m_strFileName);
                }
                catch (Exception e)
                {
                    ShowErrorText(e.ToString());
                }
            }
        }

        public string Read(string section, string key, string strDefault)
        {
            lock (locker)
            {
                string str_temp = "";
                try
                {
                    StringBuilder strTemp = new StringBuilder(256);
                    GetPrivateProfileString(section, key, strDefault, strTemp, 256, m_strFileName);
                    str_temp = strTemp.ToString();
                }
                catch (Exception e)
                {
                    ShowErrorText(e.ToString());
                }

                return str_temp;
            }
        }

        public int ReadInt(string section, string key, int nDefault)
        {
            lock (locker)
            {
                try
                {
                    int nValue = 0;
                    string strValue = Read(section, key, "");
                    if (int.TryParse(strValue, out nValue))
                    {
                        return nValue;
                    }
                    else
                    {
                        return nDefault;
                    }
                }
                catch (Exception e)
                {
                    ShowErrorText(e.ToString());
                }
                return nDefault;
            }
        }

        public void WriteInt(string section, string key, int nValue)
        {
            lock (locker)
            {
                try
                {
                    string strValue = string.Format("{0:d}", nValue);
                    Write(section, key, strValue);
                }
                catch (Exception e)
                {
                    ShowErrorText(e.ToString());
                }
            }
        }

        public double ReadDouble(string section, string key, double dbDefault)
        {
            lock (locker)
            {
                try
                {
                    double dbValue = 0;
                    string strValue = Read(section, key, "");
                    if (double.TryParse(strValue, out dbValue))
                    {
                        return dbValue;
                    }
                    else
                    {
                        return dbDefault;
                    }
                }
                catch (Exception e)
                {
                    ShowErrorText(e.ToString());
                }
                return dbDefault;
            }
        }

        public void WriteDouble(string section, string key, double dbValue)
        {
            lock (locker)
            {
                try
                {
                    string strValue = dbValue.ToString();
                    Write(section, key, strValue);
                }
                catch (Exception e)
                {
                    ShowErrorText(e.ToString());
                }
            }
        }

        private void ShowErrorText(string str)
        {
            MessageBox.Show(str);
        }
    }
}
