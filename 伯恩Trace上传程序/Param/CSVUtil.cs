using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;

namespace CSVParam
{
    public static class CSVUtil
    {
        public static void WriteCSV(string filePathName, String[] ls)
        {
            WriteCSV(filePathName, true, ls);
        }

        //文件存在则以追加形式写入，若是不存在则创建
        public static void WriteCSV(string filePathName, string[] ls, string[] head)
        {
            if (File.Exists(filePathName))
            {
                WriteCSV(filePathName, true, ls);
            }
            else
            {
                WriteCSV_head(filePathName, true, head);
                WriteCSV(filePathName, true, ls);
            }
        }
        //写表头
        public static void WriteCSV_head(string filePathName, bool append, string[] head)
        {
            try
            {
                StreamWriter fileWriter = new StreamWriter(filePathName, append, Encoding.Default);
                fileWriter.WriteLine(String.Join(",", head));
                fileWriter.Flush();
                fileWriter.Close();
            }
            catch
            {
                //MessageBox.Show(ex.Message);
            }
        }
        public static void WriteCSV(string filePathName, bool append, String[] ls)
        {
            try
            {
                StreamWriter fileWriter = new StreamWriter(filePathName, append, Encoding.Default);

                fileWriter.WriteLine(String.Join(",", ls));

                fileWriter.Flush();
                fileWriter.Close();
            }
            catch
            {
                //MessageBox.Show(ex.Message);
            }
        }

        public static void WriteCSV(string filePathName, bool append, String strMessage)
        {
            StreamWriter fileWriter = new StreamWriter(filePathName, append, Encoding.Default);
            fileWriter.WriteLine(strMessage);
            fileWriter.Flush();
            fileWriter.Close();
        }
    }
}
