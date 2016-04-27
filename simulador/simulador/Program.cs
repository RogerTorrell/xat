using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace simulador
{
    class Program
    {
        static void Main(string[] args)
        {
            Process C1 = new Process();
            Process C2 = new Process();
            Process C3 = new Process();

            C1.StartInfo.FileName = @"C:\Users\Roger\Dropbox\La Salle - 15_16\DAM_M9\UF3\02_Practiques\xat_wpf_sockets\client\client\bin\Debug\client.exe";
            C1.Start();

            C2.StartInfo.FileName = @"C:\Users\Roger\Dropbox\La Salle - 15_16\DAM_M9\UF3\02_Practiques\xat_wpf_sockets\client\client\bin\Debug\client.exe";
            C2.Start();

            C3.StartInfo.FileName = @"C:\Users\Roger\Dropbox\La Salle - 15_16\DAM_M9\UF3\02_Practiques\xat_wpf_sockets\client\client\bin\Debug\client.exe";          
            C3.Start();

        }
    }
}
