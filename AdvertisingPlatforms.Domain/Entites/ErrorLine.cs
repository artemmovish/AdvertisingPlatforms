using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvertisingPlatforms.Domain.Entites
{
    public class ErrorLine
    {
        public int Id {  get; set; }
        public string Data { get; set; }
        public ErrorLine() { }
        public ErrorLine(int id, string data) { Id = id; Data = data; }
    }
}
