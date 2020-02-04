using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace JMProject.Common
{
    public class HttpPostStream
    {
        public static T ConvertT<T>(Stream stream)
        {
            try
            {
                int dataLen = Convert.ToInt32(stream.Length);
                byte[] bytes = new byte[dataLen];
                stream.Read(bytes, 0, dataLen);
                string requestStringData = Encoding.UTF8.GetString(bytes);
                T result = JsonConvert.DeserializeObject<T>(requestStringData);
                return result;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
            
        }
    }
}
