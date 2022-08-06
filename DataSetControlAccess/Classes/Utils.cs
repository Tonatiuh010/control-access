using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DataSet.Classes
{
    public class Utils
    {
        public static byte[] GetImage(string url)
        {
            byte[] imageBytes;

            try
            {
                using (var client = new HttpClient())
                {
                    using (var response = client.GetAsync(url))
                    {
                        response.Wait();
                        var stream = response.Result.Content.ReadAsStream();
                        using(var memo = new MemoryStream())
                        {
                            stream.CopyTo(memo);
                            imageBytes = memo.ToArray();
                            memo.Dispose();
                            stream.Dispose();
                        }
                    }
                }
            } catch
            {
                imageBytes = Array.Empty<byte>();
            }

            return imageBytes;
        }
    }
}
