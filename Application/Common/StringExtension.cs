using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
   public static class StringExtension
    {
        public static string GetFilename(this string _filename)
        {
            try
            {
                return _filename.Substring(_filename.LastIndexOf("/") + 1, _filename.Length - _filename.LastIndexOf("/") - 1);
            }
            catch (Exception)
            {
                return _filename;
            }
        }

    }
}
