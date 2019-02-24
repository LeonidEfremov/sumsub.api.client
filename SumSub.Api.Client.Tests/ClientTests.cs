using AutoFixture;
using Microsoft.Extensions.Configuration;
using SumSub.Api.Tests.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SumSub.Api.Tests
{
    public partial class ClientTests
    {
        private readonly List<string> _countrycode = new List<string>
            {
                "ABW", "AFG", "AGO", "AIA", "ALA", "ALB", "AND", "ARE", "ARG", "ARM", "ASM", "ATA", "ATF", "ATG", "AUS",
                "AUT", "AZE", "BDI", "BEL", "BEN", "BES", "BFA", "BGD", "BGR", "BHR", "BHS", "BIH", "BLM", "BLR", "BLZ",
                "BMU", "BOL", "BRA", "BRB", "BRN", "BTN", "BVT", "BWA", "CAF", "CAN", "CCK", "CHE", "CHL", "CHN", "CIV",
                "CMR", "COD", "COG", "COK", "COL", "COM", "CPV", "CRI", "CUB", "CUW", "CXR", "CYM", "CYP", "CZE", "DEU",
                "DJI", "DMA", "DNK", "DOM", "DZA", "ECU", "EGY", "ERI", "ESH", "ESP", "EST", "ETH", "FIN", "FJI", "FLK",
                "FRA", "FRO", "FSM", "GAB", "GBR", "GEO", "GGY", "GHA", "GIB", "GIN", "GLP", "GMB", "GNB", "GNQ", "GRC",
                "GRD", "GRL", "GTM", "GUF", "GUM", "GUY", "HKG", "HMD", "HND", "HRV", "HTI", "HUN", "IDN", "IMN", "IND",
                "IOT", "IRL", "IRN", "IRQ", "ISL", "ISR", "ITA", "JAM", "JEY", "JOR", "JPN", "KAZ", "KEN", "KGZ", "KHM",
                "KIR", "KNA", "KOR", "KWT", "LAO", "LBN", "LBR", "LBY", "LCA", "LIE", "LKA", "LSO", "LTU", "LUX", "LVA",
                "MAC", "MAF", "MAR", "MCO", "MDA", "MDG", "MDV", "MEX", "MHL", "MKD", "MLI", "MLT", "MMR", "MNE", "MNG",
                "MNP", "MOZ", "MRT", "MSR", "MTQ", "MUS", "MWI", "MYS", "MYT", "NAM", "NCL", "NER", "NFK", "NGA", "NIC",
                "NIU", "NLD", "NOR", "NPL", "NRU", "NZL", "OMN", "PAK", "PAN", "PCN", "PER", "PHL", "PLW", "PNG", "POL",
                "PRI", "PRK", "PRT", "PRY", "PSE", "PYF", "QAT", "REU", "ROU", "RUS", "RWA", "SAU", "SDN", "SEN", "SGP",
                "SGS", "SHN", "SJM", "SLB", "SLE", "SLV", "SMR", "SOM", "SPM", "SRB", "SSD", "STP", "SUR", "SVK", "SVN",
                "SWE", "SWZ", "SXM", "SYC", "SYR", "TCA", "TCD", "TGO", "THA", "TJK", "TKL", "TKM", "TLS", "TON", "TTO",
                "TUN", "TUR", "TUV", "TWN", "TZA", "UGA", "UKR", "UMI", "URY", "USA", "UZB", "VAT", "VCT", "VEN", "VGB",
                "VIR", "VNM", "VUT", "WLF", "WSM", "YEM", "ZAF", "ZMB", "ZWE"
            };

        private IClient _client;
        private Fixture _fixture;

        public ClientTests()
        {
            var settings = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            var configuration = new Configuration
            {
                Endpoint = new Uri(settings.GetSection("sumsub:endpoint").Value),
                Key = settings.GetSection("sumsub:key").Value
            };
            var httpClient = new HttpClient();

            _client = new Client(configuration, httpClient);

            _fixture = CreateFixture();
        }

        private Fixture CreateFixture()
        {
            var fixture = new Fixture();
            var rand = new Random();
            var idDocTypes = Enum.GetValues(typeof(IdDocType)).Cast<IdDocType>().Where(_ => _ != IdDocType.UNDEFINED).ToList();
            var idDocSetTypes = Enum.GetValues(typeof(IdDocSetType)).Cast<IdDocSetType>().Where(_ => _ != IdDocSetType.UNDEFINED).ToList();
            var idDocSubTypes = Enum.GetValues(typeof(IdDocSubType)).Cast<IdDocSubType>().Where(_ => _ != IdDocSubType.UNDEFINED).ToList();

            fixture.Customize<Info>(c => c
                .With(i => i.Country, _countrycode.RandomElement())
                .With(_ => _.Dob, DateTime.Now.Date.AddDays(-rand.Next(20, 40)))
            );

            fixture.Customize<Address>(c => c
                .With(i => i.Country, _countrycode.RandomElement())
                .With(i => i.StartDate, DateTime.Now.Date.AddDays(-rand.Next(20, 40)))
                .With(i => i.EndDate, DateTime.Now.Date.AddDays(-rand.Next(20, 40)))
            );

            fixture.Customize<ApplicantIdDocs>(c => c
                .With(_ => _.Country, _countrycode.RandomElement())
                .With(_ => _.IncludedCountries, new List<string> { _countrycode.RandomElement() })
                .With(_ => _.ExcludedCountries, new List<string> { _countrycode.RandomElement() })
            );

            fixture.Customize<IdDoc>(c => c
                .With(_ => _.Country, _countrycode.RandomElement())
                .With(_ => _.Dob, DateTime.Now.Date.AddDays(-rand.Next(20, 40)))
                .With(_ => _.IssuedDate, DateTime.Now.Date.AddDays(-rand.Next(20, 40)))
                .With(_ => _.IdDocType, idDocTypes.RandomElement())
            );

            fixture.Customize<DocSet>(c => c
                .With(_ => _.IdDocSetType, idDocSetTypes.RandomElement())
            );

            fixture.Customize<ApplicantRequest>(c => c
                .With(_ => _.RequiredIdDocs, (DocSets)null)
            );

            return fixture;
        }


        private async Task<ApplicantResponse> CreateApplicantAsync()
        {
            var applicantRequest = _fixture.Create<ApplicantRequest>();

            var applicant = await _client.CreateApplicantAsync(applicantRequest);

            return applicant;
        }

        private async Task<ApplicantIdDocs> CreateApplicantIdDocs(ApplicantResponse applicant)
        {
            var idDocs = _fixture.Create<ApplicantIdDocs>();
            var applicantIdDocs = await _client.SetApplicantIdDocsAsync(applicant.Id, idDocs);

            return applicantIdDocs;
        }

        private async Task<IdDoc> AddApplicantIdDoc(string applicantId, IdDoc idDoc, string documentFilePath, string documentContentType)
        {
            var fileName = Path.GetFileName(documentFilePath);
            var bytes = File.ReadAllBytes(documentFilePath);
            var content = Convert.ToBase64String(bytes);
            var idDocCreate = new IdDocCreate
            {
                Metadata = idDoc,
                ImageFile = new ImageFile
                {
                    Content = content,
                    ContentType = documentContentType,
                    FileName = fileName
                }
            };
            var resultIdDoc = await _client.AddApplicantIdDocAsync(applicantId, idDocCreate);

            return resultIdDoc;
        }
    }
}