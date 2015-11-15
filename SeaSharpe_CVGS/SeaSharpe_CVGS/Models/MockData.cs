using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace SeaSharpe_CVGS.Models
{
    public class MockData
    {
        public IEnumerable<Population> People {get; private set;}

        public MockData(string path = null)
        {
            var csv = new CsvReader(GetStream(Population.Metadata));
            csv.Configuration.TrimFields = true;
            People = csv.GetRecords<Population>();
        }

        public class Population
        {
            public Guid Guid { get; set; }
            public int Sequence { get; set; }
            public string Surname { get; set; }
            public string GivenName { get; set; }
            public string PopulationClassificationCode { get; set; }
            public string Street { get; set; }
            public string City { get; set; }
            public string ProvinceCode { get; set; }
            public string LandLine { get; set; }
            public string Extension { get; set; }
            public string Mobile { get; set; }
            public string Fax { get; set; }
            public string Email { get; set; }
            public string Sin { get; set; }
            public string Hin { get; set; }
            public string FinancialInstitution { get; set; }
            public string BranchAddress { get; set; }
            public uint TransitNumber { get; set; }
            public uint BankCode { get; set; }
            public uint AccountNumber { get; set; }
            public string Visa { get; set; }
            public string MasterCard { get; set; }
            public string Amex { get; set; }
            public bool Allocated { get; set; }
            public static MockedData Metadata = new MockedData { FileName = "population.csv", FileURL = @"http://pastebin.com/raw.php?i=H47btLfP" };
        }

        public struct MockedData
        {
            public string FileName;
            public string FileURL;
        }

        private TextReader GetStream(MockedData data)
        {
            string localpath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + data.FileName;
            if (!File.Exists(localpath))
            {
                using (var client = new WebClient())
                {
                    client.DownloadFile(data.FileURL, localpath);
                }
            }
            return File.OpenText(localpath);
        }
    }
}