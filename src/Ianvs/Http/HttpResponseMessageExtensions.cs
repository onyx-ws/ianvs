using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Onyx.Ianvs.Http
{
    public static class HttpResponseMessageExtensions
    {
        public async static Task<string> ReadBodyAsStringAsync(this HttpResponseMessage message)
        {
            return await message.Content.ReadAsStringAsync();
        }
    }
}