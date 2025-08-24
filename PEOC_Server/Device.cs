using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PEOC_Server
{
    public struct Device
    {
        [JsonIgnore]
        public string IP;

        [JsonPropertyName("MAC")]
        public string MAC;

        [JsonPropertyName("Name")]
        public string Name;

        public Device(string mac, string name)
        {
            MAC = mac;
            Name = name;
        }

        public Device(string ip, string mac, string name)
        {
            IP= ip; 
            MAC = mac; 
            Name= name;
        }

        public void IPAdd(string ip)
        {
            this.IP = ip;
        }
    }
}
