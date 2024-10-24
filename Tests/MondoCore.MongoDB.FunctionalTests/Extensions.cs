using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MongoDB = MondoCore.MongoDB;

using MondoCore.TestHelpers;
using MondoCore.Data;

namespace MondoCore.MongoDB.FunctionalTests
{
    [TestClass]
    public class ExtensionsTests
    {
        private const string ConnectionString = "mongodb://localhost:27017/?readPreference=primary&appname=MongoDB%20Compass&ssl=false";

        private IDatabase _db;
        private IReadRepository<Guid, Person>  _reader;
        private IWriteRepository<Guid, Person> _writer;

        public ExtensionsTests()
        {
            _db = new MondoCore.MongoDB.MongoDB("functionaltests", ConnectionString);
            _writer = _db.GetRepositoryWriter<Guid, Person>("persons");
            _reader = _db.GetRepositoryReader<Guid, Person>("persons");
        }

        [TestInitialize]
        public void MongoCollection_Initialize()
        {
           // await _writer.Delete( _=> true );
        }
         
        [TestCleanup]
        public void MongoCollection_Cleanup()
        {
           // await _writer.Delete( _=> true );
        }

        [TestMethod]
        public async Task MongoCollection_1000()
        {
            await CreateLargeDatabase(1000);

            Assert.AreEqual(1000, _reader.Count());

            var result = await _reader.Get( i=> i.Surname == "Brown").ToList();

            Assert.AreNotEqual(0, result.Count());
        }

        [TestMethod]
        public async Task MongoCollection_10000()
        {
            await CreateLargeDatabase(10000);

            Assert.AreEqual(10000, _reader.Count());

            var result = await _reader.Get( i=> i.Surname == "Brown").ToList();

            Assert.AreNotEqual(0, result.Count());
        }

        [TestMethod]
        public async Task MongoCollection_100000()
        {
            await CreateLargeDatabase(100000);

            Assert.AreEqual(100000, _reader.Count());

            var result = await _reader.Get( i=> i.Surname == "Brown").ToList();

            Assert.AreNotEqual(0, result.Count());
        }

        [TestMethod]
        public async Task MongoCollection_1000000()
        {
            await CreateLargeDatabase(1000000);

            Assert.AreEqual(1000000, _reader.Count());

            var result = await _reader.Get( i=> i.Surname == "Brown").ToList();

            Assert.AreNotEqual(0, result.Count());
        }

        [TestMethod]
        public async Task MongoCollection_Update()
        {
            var count = _reader.Count(i=> i.Surname == "Anderson");

            await _writer.Update( new { Surname = "Anderson2" }, i=> i.Surname == "Anderson");

            Assert.AreEqual(0, _reader.Count( i=> i.Surname == "Anderson") );

            Assert.AreEqual(count, _reader.Count( i=> i.Surname == "Anderson2") );
        }

        #region Private

        private async Task CreateLargeDatabase(int numItems = 100000)
        {
            await _writer.Delete( _=> true );

            var list = new List<Person>();

            for(int i = 0; i < numItems; ++i)
            { 
               list.Add(CreateRandomPerson());
            }

            await _writer.Insert(list);

            return;
        }

        #region Test Data

        private Random _randomSurname      = new Random();
        private Random _randomMiddleName   = new Random();
        private Random _randomFirstName    = new Random();
        private Random _randomStreet       = new Random();
        private Random _randomCity         = new Random();
        private Random _randomState        = new Random();
        private Random _randomStreetNumber = new Random();
        private Random _randomBirthdate    = new Random();
        private Random _randomZip          = new Random();

        private Person CreateRandomPerson()
        {
            var zip = _randomZip.Next(100000);

            return new Person
            { 
                Id            = Guid.NewGuid(),
                Surname       = GetRandomValue(_randomSurname,      _surnames),
                FirstName     = GetRandomValue(_randomFirstName,    _givenName),
                MiddleName    = GetRandomValue(_randomMiddleName,   _givenName),
                Birthdate     = (new DateTime(2002, 1, 1, 0, 0, 0)).AddDays(-(_randomBirthdate.Next(26000))).ToString("dd-MMM-yyyy"),

                Addresses = new List<Address>
                { 
                    new Address
                    {
                        StreetNumber  = (100 + _randomStreetNumber.Next(100000)).ToString(),
                        StreetName    = GetRandomValue(_randomStreet,       _streets),
                        City          = GetRandomValue(_randomCity,         _cities),
                        State         = GetRandomValue(_randomState,        _states),
                        ZipCode       = zip.ToString().PadLeft(6, '0').Substring(1, 5)
                    },
                    new Address
                    {
                        StreetNumber  = (100 + _randomStreetNumber.Next(100000)).ToString(),
                        StreetName    = GetRandomValue(_randomStreet,       _streets),
                        City          = GetRandomValue(_randomCity,         _cities),
                        State         = GetRandomValue(_randomState,        _states),
                        ZipCode       = zip.ToString().PadLeft(6, '0').Substring(1, 5)
                    }
                }
            };
        }

        private string GetRandomValue(Random random, string[] names)
        {
            return names[random.Next(names.Length)];
        }

        public class Person : IIdentifiable<Guid>
        {
            public Guid   Id            { get;set; }
            public string Surname       { get;set; }
            public string FirstName     { get;set; }
            public string MiddleName    { get;set; }
            public string Birthdate     { get;set; }
            public List<Address> Addresses  { get;set; } = new List<Address>();
        }

        public class Address 
        {
            public string StreetNumber  { get;set; }
            public string StreetName    { get;set; }
            public string City          { get;set; }
            public string State         { get;set; }
            public string ZipCode       { get;set; }
        }

        #region Surnames

        private static string[] _surnames = new string[]
        {
            "Smith",        "Jones",        "Anderson",     "Johnson",
            "Taylor",       "Jordan",       "Franklin",     "Washington",
            "Madison",      "Monroe",       "Benson",       "Harris",
            "Boyle",        "Lincoln",      "Clinton",      "Carpenter",
            "Collins",      "Davis",        "Alexander",    "Allen",
            "Davidson",     "Porter",       "Griffin",      "Griffith",
            "Marshall",     "Moore",        "Weldy",        "Perkins",
            "Strauss",      "Palmer",       "Sullivan",     "Stone",
            "Warner",       "Nelson",       "Miller",       "Baldwin",
            "Simon",        "Wright",       "Watson",       "Holmes",
            "Chandler",     "Fuller",       "Sheldon",      "Field",
            "McCormick",    "Wolfe",        "Blair",        "Butler",
            "Cooper",       "Norris",       "Page",         "Cohen",
            "Wilson",       "Holden",       "Douglas",      "Bennet",
            "Adams",        "James",        "Green",        "White",
            "Black",        "Brown",        "King",         "Rey",
            "Reed",         "Scott",        "Shephard",     "Long",
            "Anthony",      "Williams",     "Lee",          "Clark",
            "Grant",        "Harrison",     "Henry",        "Wentworth",
            "Flintstone",   "Alvarez",      "Sanchez",      "Martinez",
            "Rodriguez",    "Gonzales",     "Hernandez",    "Romero",
            "Chavez",       "Vasquez",      "Morales",      "Velasquez",
            "Valdes",       "Cortez",       "Gomez",        "Ortega",
            "Garcia",       "Ruiz",         "Reyes",        "Lopez",
            "Trevino",      "Orloff",       "Stein",        "Nguyen",
            "Trinh",        "Wang",         "Wong",         "Wu",
            "Jin",          "Chen",         "Tang",         "Chao",
            "Ping",         "Chu",          "Chang",        "Yang",
            "Lombardo",     "Yamamoto",     "Matsumoto",    "Nakashima",
            "Nomura",       "Okamura",      "Zhukov",       "Kornilov",
            "Lazarev",      "Komarov",      "Popovich",     "Chorloff"
            };

        #endregion

        #region Given Names

        private static string[] _givenName = new string[] 
        {
            "Oliver",       "Archer",       "Adrian",       "Daniel",
            "Liam",         "Lucas",        "Elias",        "Nathaniel",
            "Ethan",        "Noah",         "Samuel",       "Ezra",
            "Aiden",        "Harrison",     "Arthur",       "Beau",
            "Gabriel",      "Hudson",       "Gideon",       "Zachary",
            "Caleb",        "Felix",        "Kaden",        "Tobias",
            "Theodore",     "Elliott",      "Arlo",         "Carter",
            "Declan",       "Jacob",        "James",        "Matthew",
            "Owen",         "Atticus",      "Adam",         "Ian",
            "Elijah",       "Lincoln",      "Colton",       "Ezekiel",
            "Henry",        "Gavin",        "Ronan",        "Aaron",
            "Jackson",      "Dominic",      "Roman",        "Thomas",
            "Grayson",      "Jack",         "Asher",        "Xander",
            "Levi",         "Atlas",        "Nolan",        "Soren",
            "Benjamin",     "Isaac",        "Jonah",        "Oscar",
            "Finn",         "Logan",        "Rhys",         "Callum",
            "Miles",        "Wyatt",        "Nathan",       "Nicholas",
            "Alexander",    "Silas",        "Axel",         "Ace",
            "Sebastian",    "Cole",         "August",       "Josiah",
            "Leo",          "Theo",         "Connor",       "Michael",
            "Landon",       "Holden",       "Xavier",       "Vincent",
            "Emmett",       "Luke",         "Charles",      "Edward",
            "Everett",      "William",      "Lachlan",      "Chase",
            "Milo",         "Isaiah",       "Apollo",        "David",
            "Jasper",       "Eli",          "Jace",          "Malachi",

            "Charlotte",     "Ivy",         "Naomi",         "Arabella",
            "Ava",           "Ella",        "Zoey",          "Athena",
            "Amelia",        "Eleanor",     "Aaliyah",       "Maya",
            "Olivia",        "Audrey",      "Elizabeth",     "Saoirse",
            "Aurora",        "Genevieve",   "Evangeline",    "Zoe",
            "Violet",        "Iris",        "Autumn",        "Sienna",
            "Luna",          "Isabella",    "Esme",          "Evelyn",
            "Hazel",         "Lucy",        "Mia",           "Rose",
            "Chloe",         "Ophelia",     "Daisy",         "Harper",
            "Aria/Arya",     "Eloise",      "Ruby",          "Josephine",
            "Scarlett",      "Vivian",      "Margot",        "Felicity",
            "Isla",          "Lorelei",     "Layla",         "Delilah",
            "Abigail",       "Wren",        "Anastasia",     "Amaya",
            "Freya",         "Hannah",      "Sadie",         "Caroline",
            "Adeline",       "Clara",       "Stella",        "Olive",
            "Sophia",        "Lydia",       "Lila",          "Adalyn",
            "Nora",          "Madeleine",   "Rosalie",       "Brielle",
            "Adelaide",      "Claire",      "Daphne",        "Matilda",
            "Emma",          "Astrid",      "Fiona",         "Aurelia",
            "Mila",          "Thea",        "Phoebe",        "Willow",
            "Lily",          "Kaia",        "Savannah",      "Natalie",
            "Grace",         "Cora",        "Alice",         "Leilani",
            "Maeve",         "Penelope",    "Eliana",        "Ada",
            "Eliza",         "Elena",       "Gemma",         "Mabel",
            "Emily",         "Isabelle",    "Nadia",         "Amara"
        };

        #endregion

        #region Cities

        private static string[] _cities = new string[]
        {
            "New York",         "Memphis",              "Riverside",        "Madison",
            "Los Angeles",      "Louisville",           "Corpus Christi",   "Lubbock",
            "Chicago",          "Baltimore",            "Lexington",        "Scottsdale",
            "Houston",          "Milwaukee",            "Henderson",        "Reno",
            "Phoenix",          "Albuquerque",          "Stockton",         "Buffalo",
            "Philadelphia",     "Tucson",               "Saint Paul",       "Gilbert",
            "San Antonio",      "Fresno",               "Cincinnati",       "Glendale",
            "San Diego",        "Mesa",                 "St. Louis",        "N Las Vegas",
            "Dallas",           "Sacramento",           "Pittsburgh",       "Winston�Salem",
            "San Jose",         "Atlanta",              "Greensboro",       "Chesapeake",
            "Austin",           "Kansas City",          "Lincoln",          "Norfolk",
            "Jacksonville",     "Colorado Springs",     "Anchorage",        "Fremont",
            "Fort Worth",       "Omaha",                "Plano",            "Garland",
            "Columbus",         "Raleigh",              "Orlando",          "Irving",
            "Charlotte",        "Miami",                "Irvine",           "Hialeah",
            "San Francisco",    "Long Beach",           "Newark",           "Richmond",
            "Indianapolis",     "Virginia Beach",       "Durham",           "Boise",
            "Seattle",          "Oakland",              "Chula Vista",      "Spokane",
            "Denver",           "Minneapolis",          "Toledo",           "Baton Rouge",
            "Washington",       "Tulsa",                "Fort Wayne",       "Tacoma",
            "Boston",           "Tampa",                "St Petersburg",    "San Bernardino",
            "El Paso",          "Arlington",            "Modesto",          "Overland Park",
            "Nashville",        "New Orleans",          "Fontana",          "Grand Prairie",
            "Detroit",          "Wichita",              "Des Moines",       "Tallahassee",
            "Oklahoma City",    "Bakersfield",          "Moreno Valley",    "Cape Coral",
            "Portland",         "Cleveland",            "Santa Clarita",    "Mobile",
            "Las Vegas",        "Aurora",               "Fayetteville",     "Knoxville",
            "Anaheim",          "Laredo",               "Fort Collins",     "Rockford",
            "Honolulu",         "Jersey City",          "Corona",           "Paterson",
            "Santa Ana",        "Chandler",             "Springfield",      "Savannah",
            "Birmingham",       "Shreveport",           "Jackson",          "Bridgeport",
            "Oxnard",           "Worcester",            "Alexandria",       "Torrance",
            "Rochester",        "Ontario",              "Hayward",          "McAllen",
            "Port St. Lucie",   "Vancouver",            "Clarksville",      "Syracuse",
            "Grand Rapids",     "Sioux Falls",          "Lakewood",         "Surprise",
            "Huntsville",       "Chattanooga",          "Lancaster",        "Denton",
            "Salt Lake City",   "Brownsville",          "Salinas",          "Roseville",
            "Frisco",           "Fort Lauderdale",      "Palmdale",         "Thornton",
            "Yonkers",          "Providence",           "Hollywood",        "Miramar",
            "Amarillo",         "Newport News",         "Springfield",      "Pasadena",
            "Glendale",         "Rancho Cucamonga",     "Macon",            "Mesquite",
            "Huntington Beach", "Santa Rosa",           "Kansas City",      "Olathe",
            "McKinney",         "Peoria",               "Sunnyvale",        "Dayton",
            "Montgomery",       "Oceanside",            "Pomona",           "Carrollton",
            "Augusta",          "Elk Grove",            "Killeen",          "Waco",
            "Aurora",           "Salem",                "Escondido",        "Orange",
            "Akron",            "Pembroke Pines",       "Pasadena",         "Fullerton",
            "Little Rock",      "Eugene",               "Naperville",       "Charleston",
            "Tempe",            "Garden Grove",         "Bellevue",         "W Valley",
            "Columbus",         "Cary",                 "Joliet",           "Visalia",
            "Murfreesboro",     "Hampton",              "Victorville",      "Temecula",
            "Midland",          "Gainesville",          "Clovis",           "Gresham",
            "Warren",           "Hartford",             "Springfield",      "Lewisville",
            "Coral Springs",    "Vallejo",              "Meridian",         "Hillsboro",
            "Cedar Rapids",     "Allentown",            "Westminster",      "Ventura",
            "Round Rock",       "Berkeley",             "Costa Mesa",       "Greeley",
            "Sterling Heights", "Richardson",           "High Point",       "Inglewood",
            "Kent",             "Arvada",               "Manchester",       "Waterbury",
            "Columbia",         "Ann Arbor",            "Pueblo",           "League City",
            "Santa Clara",      "Rochester",            "Lakeland",         "Santa Maria",
            "New Haven",        "Cambridge",            "Pompano Beach",    "Tyler	",
            "Stamford",         "Sugar Land",           "W Palm Beach",     "Davie",
            "Concord",          "Lansing",              "Antioch",          "Lakewood",
            "Elizabeth",        "Evansville",           "Everett",          "Daly City",
            "Athens",           "College Station",      "Downey",           "Boulder",
            "Thousand Oaks",    "Fairfield",            "Lowell",           "Allen",
            "Lafayette",        "Clearwater",           "Centennial",       "W Covina",
            "Simi Valley",      "Beaumont",             "Elgin",            "Sparks",
            "Topeka",           "Independence",         "Richmond",         "Wichita",
            "Norman",           "Provo",                "Peoria",           "Green Bay",
            "Fargo",            "W Jordan",             "Broken Arrow",     "San Mateo",
            "Wilmington",       "Murrieta",             "Miami Gardens",    "Norwalk",
            "Abilene",          "Palm Bay",             "Billings",         "Rialto",
            "Odessa",           "El Monte",             "Jurupa Valley",    "Las Cruces",
            "Columbia",         "Carlsbad",             "Sandy Springs",    "Chico",
            "Pearland",         "Charleston",           "El Cajon",          "Tuscaloosa",
            "Burbank",           "Carmel",              "S Bend",            "Spokane Valley",
            "Renton",            "San Angelo",          "Vista",             "Vacaville",
            "Davenport",         "Clinton",             "Edinburg",          "Bend",
            "Woodbridge",        "Norton",              "Fredericksburg",    "Georgetown"
            };

        #endregion

        #region States

        private static string[] _states = new string[]
        {
        "AL",
        "AK",
        "AZ",
        "AR",
        "CA",
        "CO",
        "CT",
        "DE",
        "FL",
        "GA",
        "HI",
        "ID",
        "IL",
        "IN",
        "IA",
        "KS",
        "KY",
        "LA",
        "ME",
        "MD",
        "MA",
        "MI",
        "MN",
        "MS",
        "MO",
        "MT",
        "NE",
        "NV",
        "NH",
        "NJ",
        "NM",
        "NY",
        "NC",
        "ND",
        "OH",
        "OK",
        "OR",
        "PA",
        "RI",
        "SC",
        "SD",
        "TN",
        "TX",
        "UT",
        "VT",
        "VA",
        "WA",
        "WV",
        "WI",
        "WY"        };

        #endregion

        #region Streets

        private static string[] _streets = new string[]
        {
            "Main St",          "Church St",        "Jackson St",       "Park Place",
            "Main St N",        "Main St S",        "Bridge St",        "Locust St",
            "Elm St",           "High St",          "Madison Ave",      "Meadow Ln",
            "Main St W",        "Washington St",    "Spruce St",        "Grove St",
            "Main St E",        "Park Ave",         "Ridge Rd",         "5th St",
            "2nd St",           "Walnut St",        "Pearl St",         "Lincoln St",
            "Chestnut St",      "Maple Ave",        "Madison St",       "Dogwood Dr",
            "Maple St",         "Broad St",         "Lincoln Ave",      "Pennsylvania Ave",
            "Oak St",           "Center St",        "Pleasant St",      "4th St W",
            "Pine St",          "Pike St",          "Adams St",         "Hickory Ln",
            "Wilshire Blvd",    "Tampa Ave",        "Route 1",          "Jefferson Ave",
            "Renton Ave",       "River Rd",         "Summit Ave",       "3rd St W",     
            "Market St",        "Water St",         "Virginia Ave",     "7th St",
            "Union St",         "South St",         "12th St",          "Academy St",
            "Park St",          "3rd St",           "5th Ave",          "11th St",
            "Washington Ave",   "Cherry St",        "6th St",           "2nd Ave",
            "N St",             "4th St",           "9th St",           "East St",
            "Court St",         "Highland Ave",     "Charles St",       "Green St",
            "Mill St",          "Franklin St",      "Cherry Ln",        "Elizabeth St",
            "Prospect St",      "School St",        "Hill St",          "Woodland Dr",
            "Spring St",        "Central Ave",      "River St",         "6th St W",
            "1st St",           "State St",         "10th St",          "Brookside Dr",
            "Front St",         "West St",          "Colonial Dr",      "Hillside Ave",
            "Jefferson St",     "Cedar St",         "Monroe St",        "Lake St",
            "Valley Rd",        "13th St",          "5th St W",         "Highland Dr",
            "Winding Way",      "4th Ave",          "6th Ave",          "Holly Dr",
            "1st Ave",          "5th St N",         "Berkshire Dr",     "King St",
            "Fairway Dr",       "College St",       "Buckingham Dr",    "Lafayette Ave",
            "Liberty St",       "Dogwood Ln",       "Circle Dr",        "Linden St",
            "2nd St W",         "Mill Rd",          "Clinton St",       "Mulberry St",
            "3rd Ave",          "7th Ave",          "George St",        "Poplar St",
            "Broadway",         "8th St",           "Hillcrest Dr",     "Ridge Ave",
            "Church Rd",        "Beech St",         "Hillside Dr",      "7th St E",
            "Delaware Ave",     "Division St",      "Laurel St",        "James St",
            "Prospect Ave",     "Harrison St",      "Park Dr",          "Magnolia Dr",
            "Sunset Dr",        "Heather Ln",       "Penn St",          "Myrtle Ave",
            "Vine St",          "Lakeview Dr",      "Railroad Ave",     "Shady Ln",
            "Laurel Ln",        "Creek Rd",         "Riverside Dr",     "Surrey Ln",
            "New St",           "Durham Rd",        "Sherwood Dr",      "Walnut Ave",
            "Oak Ln",           "Elm Ave",          "Summit St",        "Warren St",
            "Primrose Ln",      "Fairview Ave",     "2nd St E",         "Williams St",
            "Railroad St",      "Front St N",       "6th St N",         "Wood St",
            "Willow St",        "Grant St",         "Cedar Ln",         "Woodland Ave",
            "4th St N",         "Hamilton St",      "Belmont Ave",      "Denny Way",
            "Cambridge Ct",     "Alki Ave",         "Cambridge Dr",     "Edgewater Ln",
            "Clark St",         "Edgemont Pl",      "College Ave",      "Euclid Ave",
            "Essex Ct",         "Evanston St",      "Franklin Ave",     "Ranier Blvd",
            "Hilltop Rd",       "Evergreen Way",    "Fremont Ave",      "Fairview Dr"
            };

        #endregion

        #endregion

        #endregion
    }
}
