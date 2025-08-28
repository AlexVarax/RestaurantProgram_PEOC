using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PEOC_Server
{
    public class ComClients
    {
        Dictionary<string, Device> devices = new Dictionary<string, Device>();
        Dictionary<string, IPAddress> ip_devices = new Dictionary<string, IPAddress>();
        Dictionary<string, Device> ExpectDevic;

        IPAddress brodcastAddress;

        public ComClients()
        {
            this.brodcastAddress = CreatingWideAddress();

            this.ExpectDevic = RememberDevices();
            Dictionary<string, string> DevicesActuallyFound = new Dictionary<string, string>();

            DeviceDetection(ExpectDevic.Count, ref DevicesActuallyFound);

            foreach (var mac_dev in ExpectDevic)
            {
                ExpectDevic[mac_dev.Key].IPAdd(DevicesActuallyFound[mac_dev.Key]);
                ip_devices.Add(ExpectDevic[mac_dev.Key].Name, IPAddress.Parse(ExpectDevic[mac_dev.Key].IP));
                this.SendingMessageIP(ip_devices[ExpectDevic[mac_dev.Key].Name], "OK PEOC");
            }
        }

        public void ReconnectingСlients()
        {
            /// <summary>
            /// Метод повторного подключения с Клиентами локальной сети. Работает 1 минуту
            /// </summary>

            Dictionary<string, string> DevicesActuallyFound = new Dictionary<string, string>();

            DeviceDetection(ExpectDevic.Count, ref DevicesActuallyFound);

            foreach (var mac_dev in ExpectDevic)
            {
                ExpectDevic[mac_dev.Key].IPAdd(DevicesActuallyFound[mac_dev.Key]);
                ip_devices.Add(ExpectDevic[mac_dev.Key].Name, IPAddress.Parse(ExpectDevic[mac_dev.Key].IP));
                this.SendingMessageIP(ip_devices[ExpectDevic[mac_dev.Key].Name], "OK PEOC");
            }
        }

        IPAddress CreatingWideAddress()
        {
            string comand_ip = "Get-NetIPAddress -AddressFamily IPv4 | Where-Object { $_.PrefixOrigin -eq 'Dhcp' -and $_.SuffixOrigin -eq 'Dhcp' } | Select-Object PrefixLength, IPAddress";

            ProcessStartInfo psi = new ProcessStartInfo("powershell", comand_ip);
            psi.RedirectStandardOutput = true;
            psi.UseShellExecute = false;

            byte[] chunks_ip = new byte[5];
            int i = 0;
            foreach (string line in Process.Start(psi).StandardOutput.ReadToEnd().Split(new char[] { '.', '\n', ' ' }))
            {
                if (Byte.TryParse(line, out byte res))
                {
                    chunks_ip[i] = res;
                    i++;
                }
            }

            chunks_ip[0] >>= 3;

            // создания ip адреса для широкоугольной рассылки
            byte[] new_ip = new byte[4];
            Array.Copy(chunks_ip, 1, new_ip, 0, chunks_ip[0]);

            for (int _ = chunks_ip[0]; _ < 4; _++)
            {
                new_ip[_] = 255;
            }

            string broadcast_address_ip = string.Join('.', new_ip);
            new_ip[3] = chunks_ip[4];
            string own_ip = string.Join(".", new_ip);

            return IPAddress.Parse(broadcast_address_ip);
        }

        Dictionary<string, Device> RememberDevices()
        {
            Dictionary<string, Device> mac_dev = new Dictionary<string, Device>();

            try
            {
                using (FileStream fs = new FileStream("user.json", FileMode.OpenOrCreate))
                {
                    Device dev = JsonSerializer.Deserialize<Device>(fs);
                    mac_dev.Add(dev.MAC, dev);
                }
            }
            catch { }
            

            return mac_dev;
        }

        Dictionary<string, string> DeviceDetection(int count_dev, ref Dictionary<string, string> mac_dev)
        {
            HashSet<string> ip_dev = new HashSet<string>();
            mac_dev = new Dictionary<string, string>();

            using var receiver = new UdpClient(552);
            receiver.JoinMulticastGroup(brodcastAddress);
            receiver.MulticastLoopback = false;
            IPEndPoint plug = null;

            Stopwatch sw = new Stopwatch();
            sw.Start();

            while (count_dev > 0)
            {
                if (sw.ElapsedMilliseconds > 60000)
                    break;

                var result = receiver.Receive(ref plug);
                try
                {
                    string[] message = Encoding.UTF8.GetString(result).Split('|');

                    if (ip_dev.Contains(message[0])) continue;

                    ip_dev.Add(message[0]);
                    count_dev--;
                    mac_dev.Add(message[1], message[0]);
                }
                catch {
                    continue;
                }
                
            }

            return mac_dev;
        }

        public async Task SendingMessageIP(IPAddress ip_addres, string message)
        {
            using var client = new UdpClient();

            byte[] mes_byte = Encoding.UTF8.GetBytes(message);

            await client.SendAsync(mes_byte, new IPEndPoint(ip_addres, 552));
        }

        async Task AssigningTemporaryNumb()
        {
            HashSet<string> ip_dev = new HashSet<string>();
            List<Device> devices = new List<Device>();
            int num = 0;

            using var receiver = new UdpClient(552);
            receiver.JoinMulticastGroup(brodcastAddress);
            receiver.MulticastLoopback = false;

            while (true)
            {
                var result = await receiver.ReceiveAsync();
                string[] message = Encoding.UTF8.GetString(result.Buffer).Split('|');

                if (ip_dev.Contains(message[0])) continue;

                ip_dev.Add(message[0]);
                num++;
                
            }
        }
    }
}
