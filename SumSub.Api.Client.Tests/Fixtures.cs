using SumSub.Api.Tests.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SumSub.Api.Tests
{
    public class Fixtures : FixtureBase
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

        public Fixtures()
        {
            var rand = new Random();
            var idDocTypes = Enum.GetValues(typeof(IdDocType)).Cast<IdDocType>().Where(_ => _ != IdDocType.UNDEFINED).ToList();
            var idDocSetTypes = Enum.GetValues(typeof(IdDocSetType)).Cast<IdDocSetType>().Where(_ => _ != IdDocSetType.UNDEFINED).ToList();
            var idDocSubTypes = Enum.GetValues(typeof(IdDocSubType)).Cast<IdDocSubType>().Where(_ => _ != IdDocSubType.UNDEFINED).ToList();

            Fixture.Customize<Info>(c => c
                .With(i => i.Country, _countrycode.RandomElement())
                .With(_ => _.Dob, DateTime.Now.Date.AddDays(-rand.Next(20, 40)))
            );

            Fixture.Customize<Address>(c => c
                .With(i => i.Country, _countrycode.RandomElement())
                .With(i => i.StartDate, DateTime.Now.Date.AddDays(-rand.Next(20, 40)))
                .With(i => i.EndDate, DateTime.Now.Date.AddDays(-rand.Next(20, 40)))
            );

            Fixture.Customize<ApplicantIdDocs>(c => c
                .With(_ => _.Country, _countrycode.RandomElement())
                .With(_ => _.IncludedCountries, new List<string> { _countrycode.RandomElement() })
                .With(_ => _.ExcludedCountries, new List<string> { _countrycode.RandomElement() })
            );

            Fixture.Customize<IdDoc>(c => c
                .With(_ => _.Country, _countrycode.RandomElement())
                .With(_ => _.Dob, DateTime.Now.Date.AddDays(-rand.Next(20, 40)))
                .With(_ => _.IssuedDate, DateTime.Now.Date.AddDays(-rand.Next(20, 40)))
                .With(_ => _.IdDocType, idDocTypes.RandomElement())
            );

            Fixture.Customize<DocSet>(c => c
                .With(_ => _.IdDocSetType, idDocSetTypes.RandomElement())
            );

            Fixture.Customize<ApplicantRequest>(c => c
                .With(_ => _.RequiredIdDocs, (DocSets)null)
            );
        }
    }
}
