namespace SeaSharpe_CVGS.Migrations
{
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        enum PlatformEnum { XBOX, PC, PS4, Wii, Mobile };

        enum CategoryEnum { Action, Adventure, RPG, FPS, RTS, Puzzle, Simulation };

        enum RatingEnum {   
            EC,  // Early Childhood"
            E,   // Everyone"
            E10, // Everyone 10+"
            T,   // Teen"
            M,   // Mature"
            AO,  // Adult Only"
        }

        ApplicationDbContext db;

        Dictionary<PlatformEnum, Platform> mockPlatforms = new Dictionary<PlatformEnum, Platform>();

        Dictionary<CategoryEnum, Category> mockCategories = new Dictionary<CategoryEnum, Category>();

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //The top n application users will be employees, where n is the value of the NUMBER_OF_EMPLOYEES variable
            const int NUMBER_OF_EMPLOYEES = 10;

            db = context;
            foreach (PlatformEnum platform in Enum.GetValues(typeof(PlatformEnum)))
            {
                string platformName = Enum.GetName(typeof(PlatformEnum), platform);
                mockPlatforms[platform] = context.Platforms.FirstOrDefault(p => p.Name == platformName)
                    ?? new Platform { Name = platformName }; // If the platform doesn't exist, make it
            }

            foreach (CategoryEnum category in Enum.GetValues(typeof(CategoryEnum)))
            {
                string categoryName = Enum.GetName(typeof(CategoryEnum), category);
                mockCategories[category] = context.Catagories.FirstOrDefault(c => c.Name == categoryName)
                    ?? new Category { Name = categoryName }; // If the category doesn't exist, make it
            }

            var mockUsers = new ApplicationUser[] { // GUID             EMAIL                                       SALTED PASSWORD HASH                                        USERNAME            GENDER  FIRST NAME         LAST NAME       DATE OF BIRTH           DATE REGISTERED
                MakeUser(@"03ec4e30-6ba2-48b0-ae82-3d8e30d6307b", @"abaswell@arachnet.ca",     @"APXQhbnuJfhSBojKarJ0a9m6/imzNjs77rmVEoTZf7F8MqnMwiGU//EA9Y9GVOfw3A==",   @"AYSEBASWELL622782",      @"O",  @"AYSE",         @"BASWELL",    @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"0bc70e50-6ed5-49f1-a1f0-a8d262dea2fa", @"plalovic@cheapnfast.com",  @"AAxhW7ZHYubxVSiMREe+PDHyZMUU+2uhriBEVECzzeUBhWaHzR78dPL969/J9S5jCQ==",   @"PAMELALALOVIC475670",    @"O",  @"PAMELA",       @"LALOVIC",    @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"0fedaad9-325a-46c3-8295-fdf78fe2e09d", @"nlee@netnutz.biz",         @"AFFcvX/d/E7tFvSiCfaNMxXok89R2AyCgonpP6Np9qmSlYULEvrA9vcYO/83kRnBjw==",   @"NELLIELEE361672",        @"O",  @"NELLIE",       @"LEE",        @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"12482d3e-67e9-4d73-9e17-42a690c8581c", @"mbrookins@funnet.ca",      @"AM3YMJEEoRF5MUJYECV5vpMh/FTWt/Bmpmy6vxhXNyrnTqmr4WpqCuej9GbnzBH50g==",   @"MELISSABROOKINS141879",  @"O",  @"MELISSA",      @"BROOKINS",   @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"13c5b2fd-2681-419a-b990-4754fdda1a1f", @"gkhan@wired.biz",          @"ALvJCb4sFvBnEzwqZUSMxvczGmsDuHyaUFj3xSzhdfmTvd7IjIz8ix4NXYOoaqnTwA==",   @"GERTRUDEKHAN464278",     @"O",  @"GERTRUDE",     @"KHAN",       @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"185ecea7-4dc1-4ba5-b727-9d57268e64ae", @"lpelot@hotwired.com",      @"AFvJ+R6/AmUuCiuaT499/jgmCqbbIGfZBXJlbBZkhSJiCGXKwoPfCdL9MKOYZNKecA==",   @"LOUISPELOT715279",       @"O",  @"LOUIS",        @"PELOT",      @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"211072f3-9666-4c5c-9cfd-b0180ebc45fa", @"kfreet@webworks.com",      @"AE7PWrjO0UjZ+NgQgV6FIXCpFFsbS9Jlg7F1LseEkljCjBCKt0tKQ1hzZ+Q45T+U2g==",   @"KELLY L.FREET512339",    @"O",  @"KELLY L.",     @"FREET",      @"2015-11-19 22:46:39",  @"2015-11-19 22:46:39"),
                MakeUser(@"21f2a613-df08-44cb-b71c-db254085c074", @"drisatti@netnutz.biz",     @"APoNKqUpjcH+C4HIv1pThV9cWSfXmQBK6S8DhMKgYswcUm4WgkDpDBh95zpfc144pg==",   @"DOMONIQUERISATTI37874",  @"O",  @"DOMONIQUE",    @"RISATTI",    @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"25c8b5e0-ee1a-46b4-a9f1-8e543a6f555d", @"ecastenada@nolimits.com",  @"AD8X968nrxv+eoYvC6niKRg+FyS+hpsjHWHniYQL7jrF9/U3nYUknjHSwZch1BuQWA==",   @"EFFIECASTENADA664232",   @"O",  @"EFFIE",        @"CASTENADA",  @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"29a3e51c-d9d8-4359-83ba-57b74a1f9c5f", @"kperry@maplenet.ca",       @"AINwnT84pRS9uakp9pkPUerK8ZN0VXasxxmMZqvBIFwXM0UIfyg2Jv6gvGSVkqEp3w==",   @"KIMPERRY883536",         @"O",  @"KIM",          @"PERRY",      @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"2dec193e-7f24-4584-924d-36f32c23b548", @"lyokley@wired.biz",        @"AF2BuroNlwlHstJnQt8fGxVUSE8dgME/UZpONaq2GuWMOX7S/sqCZ5hgz5Z9YbKPYw==",   @"LEWISYOKLEY438432",      @"O",  @"LEWIS",        @"YOKLEY",     @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"2e6000ef-07a1-4b54-b012-c17c8738060f", @"rbrangan@webworks.com",    @"AHTwDr2x2aiDNX+mTMQtykFQgqvRWL0ev831AMXTkyriX2L2H0Lb9sFH+Vu3K4SWzg==",   @"ROSEBRANGAN283330",      @"O",  @"ROSE",         @"BRANGAN",    @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"32d53f96-6115-4f08-9942-4ceb9f1e2ec4", @"rsnow@wired.biz",          @"AH/IPfKGZzCOu/LxPOk2LpfuuohTE4VHM8KuW6ZkXkmctBfyPYkaIbOjN7ACfTcSyA==",   @"RSNOW726575",            @"O",  @"R",            @"SNOW",       @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"33ed6601-6173-4608-95ea-5f483cb11479", @"wmillian@wired.biz",       @"AFUmdg+m1k+fE7NFPDsYigJzH3hWhv4mfuNe62OeJ3dtTc8kd6NRJip7/f8aRle6XQ==",   @"WILLIAMAMILLIAN788469",  @"O",  @"WILLIAM A.",   @"MILLIAN",    @"2015-11-19 22:46:38",  @"2015-11-19 22:46:38"),
                MakeUser(@"36a311ea-36df-4050-b3cf-9e81399cff76", @"adermott@fishnet.nl.ca",   @"AJ7oEQfqFOFWWcsgy2kuHgPP2ZpJIuBLTCQDq+89BNXVKFKD+m3wc+Yvwdd0mcZIPA==",   @"ALANDERMOTT746130",      @"O",  @"ALAN",         @"DERMOTT",    @"2015-11-19 22:46:39",  @"2015-11-19 22:46:39"),
                MakeUser(@"3cf4a91f-5ed2-4c1d-8d57-64ff6b4a7485", @"dmcbride@cheapnfast.com",  @"AOIzfs8EZkWM4LijauQvwB+xosZ47/DnlUklUWC7BQyZQxfGCCg+oOFwNrCkuiH6Kw==",   @"DOROTHYMCBRIDE276135",   @"O",  @"DOROTHY",      @"MCBRIDE",    @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"3d20e201-2a24-4abd-8a34-2f0b66aba586", @"jpotmesil@arachnet.ca",    @"AKGU6GW0LdO/g6eddbAU5favjeI3QGwneIeMhxoaP+WWQacNj70Ej2uFmzPmxN3Lzw==",   @"JAQUELYNPOTMESL152377",  @"O",  @"JAQUELYN",     @"POTMESIL",   @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"4521cf1c-ac58-4941-aff0-9b927e900e44", @"mbrazan@hotwired.com",     @"ACCugK+XrgEfXZglpk/rIJjPfCB72xmi56mcxnftI+7HVD/fulps+pZjWCnNpNHc8g==",   @"MICHAELBRAZAN753739",    @"O",  @"MICHAEL",      @"BRAZAN",     @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"4809d300-5765-4946-8c65-003e1192e4a7", @"estubbert@wired.biz",      @"AAWopK7TDUd2cYd2knGRSXSZwoSIS/zAEMioTE7kzNyjZbYUknXGQGK4XbD76esjmQ==",   @"ELICIASTUBBERT437131",   @"O",  @"ELICIA",       @"STUBBERT",   @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"4cd9af4c-b394-45d8-bc6e-1b3c2c145e0f", @"gbarsalou@netnutz.biz",    @"AEg7/cIUFGagmDCgEyothcrggoUEBSFmeRlAGjzayhZ/G7XzIcEfKK7yScC38DPllA==",   @"GWENBARSALOU323131",     @"O",  @"GWEN",         @"BARSALOU",   @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"54620e36-75f7-419c-bc5d-07d0683b3047", @"smaline@scratchworx.com",  @"AMkZnvI5aGvl/24sf3QBn9uU1YMLUoDJtNxpbAsge9YJCkhB6HK+UEGN7S852mWUMg==",   @"SHONDRAMALINE433334",    @"O",  @"SHONDRA",      @"MALINE",     @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"61eb4d9b-be16-4f76-87d3-517db46d7c31", @"jsolow@funnet.ca",         @"AIG+Xj+G8q3hyLVXqdCc6FJfcr5sx5Har4UtP8vQ3c01cE3S4744zl9TSYVlnbmEtQ==",   @"JEFFERSONSOLOW461474",   @"O",  @"JEFFERSON",    @"SOLOW",      @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"6c605a25-6bd9-418f-b76d-9760d4425655", @"tvalasco@whatnet.on.ca",   @"AJYjvf56TmcGP3hSH6bHVtTvllrVF1sO+GWeDOCfejIv1w5W+tDzU1H5Nj2DsUhnAA==",   @"THOMASVALASCO422739",    @"O",  @"THOMAS",       @"VALASCO",    @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"72879d9d-fe3e-486b-ab91-0f7a171b286d", @"bmillard@arachnet.ca",     @"AM30FYuw+aOQuAbm/NWLHuPvrQnyvWPE7cKc4ba/Ig2aTMNSVY+msPfRaEZgFqmTng==",   @"BETHMILLARD721478",      @"O",  @"BETH",         @"MILLARD",    @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"73bed6c4-2306-45d9-9d65-c54bfcf92881", @"amannix@whatnet.on.ca",    @"AHQQXQza8ZNe56zxnbKeC/a9mp6hicTVcGQ3lNKVHDb6fF/OX8U4B7PbGaHFqe0vxQ==",   @"ANNEMANNIX757533",       @"O",  @"ANNE",         @"MANNIX",     @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"7c6c7be0-ee34-45dc-aeb0-4828a4823f55", @"twilliams@gamergold.com",  @"AP+FiY4baFW/mCfW3eQAx5MpBjGBht2SfqXFzmhavgiWP+bCiIrVhTAo2bqGzFFoRQ==",   @"THOMASWILLIAMS156589",   @"O",  @"THOMAS",       @"WILLIAMS",   @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"7f78b475-e428-45ce-a517-1e74fe386825", @"bxaintonge@whatnet.on.ca", @"AGBIcSzWQVjNiO6qsmZiwsvwqJ/fcPy+jaN8e62QvsAMoClg/qKUyEsh+zbUcuepAg==",   @"BERNARDXAINTONG4274",    @"O",  @"BERNARD",      @"XAINTONGE",  @"2015-11-19 22:46:39",  @"2015-11-19 22:46:39"),
                MakeUser(@"87dffe06-9212-4f6e-a509-f1d51bd4175b", @"rprice@celestial.bc.ca",   @"ANkTpS76Hiu6Yt92CaNTAy9wzjAZ95SE7IkK7aun3hmo9/7W8EW0490jQLe5bmKS2w==",   @"REUBEN B.PRICE187530",   @"O",  @"REUBEN B.",    @"PRICE",      @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"8b9f4b36-aad4-449c-8669-404dc2f1f3f5", @"swinsman@wired.biz",       @"AMC8ikO8Dc+nB+tFdl1MyRbcpOMhEGqCRHUGswFK0rcI9UAzavJWIpL6nfoIqBuK5Q==",   @"SARWATWINSMAN553131",    @"O",  @"SARWAT",       @"WINSMAN",    @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"8bee9544-6dbc-440c-b72a-3aa7222c8aaf", @"mdoorley@nolimits.com",    @"AMCu8g3OthN9ErDHyox7SWWmjgzheAHOyuR/RSEKV5pFHK489zzdCsU9uYwcQ1vaew==",   @"MICHAELDOORLEY145275",   @"O",  @"MICHAEL",      @"DOORLEY",    @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"8ee66ba1-d4ef-4298-8c70-57938348fbcd", @"kcherny@goalnet.qc.ca",    @"ACDGHWjeTkK+borepd1XCjz+w9Y3KrCkOR+gbe65SVJMSkfaIidaKDrtcfXHxylbuw==",   @"KAYCHERNY183636",        @"O",  @"KAY",          @"CHERNY",     @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"9afeafe2-1096-4aa7-aea5-ef7e0e1bda22", @"pstmartin@cannet.ca",      @"ANriDddVgRIPwkB2niDr+uWf5+XaO6ag+DMmfhx8x6XJllVv1GI6bw36FvGYqeiHKw==",   @"PAUL DSTMARTIN718878",   @"O",  @"PAUL D",       @"STMARTIN",   @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"9b51f209-3716-43c9-b52b-5039d825da7e", @"fsunyich@gamergold.com",   @"AEJTM6FF9McJNwPNIAvYXp36iRTMpdg8SBgY1CjBtnW6625EFyGGvaT/scNMWW7K9A==",   @"FAITHSUNYICH471779",     @"O",  @"FAITH",        @"SUNYICH",    @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"9c33b0a9-4a5a-4775-abec-7663894e58eb", @"ypetri@cheapnfast.com",    @"AN74OL514hgtEcv8O773yPAq7xgytDyf+uZ5ejSSYiSk0EVkMgfWDnzbNsVBrPn14g==",   @"YVONNE V.PETRI476174",   @"O",  @"YVONNE V.",    @"PETRI",      @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"a22ecfde-309b-4f66-815e-dd3557180339", @"lcarabine@arachnet.ca",    @"APUPQS3J+4GQLR8e4oX3ps3I7d1A1ICgfTazd4hywDJhtJALOB53Sny4CUMZxMxhQQ==",   @"LILLIANRCRABINE444648",  @"O",  @"LILLIAN R",    @"CARABINE",   @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"a7105c8d-9033-4e82-8dd1-63d8a8167814", @"jlofstrom@fishnet.nl.ca",  @"AAtSwvoURkCi6s9wNJZxVh62u9/yvSb0l1KN9ZQqA/GfTKio2xVw+7bypJ/cPbbj2A==",   @"JAMES VLOFSTROM784675",  @"O",  @"JAMES V",      @"LOFSTROM",   @"2015-11-19 22:46:39",  @"2015-11-19 22:46:39"),
                MakeUser(@"a8928339-a160-41ee-82ad-289990dfda51", @"cswauger@whatnet.on.ca",   @"AJIi+QnNhzZx/h0LKEnXWdPwUEf+ToEaVgC7r83rG8f2hi+hCj8AjBP/pjuB4PBisg==",   @"CLAUDE DSWAUGER841272",  @"O",  @"CLAUDE D",     @"SWAUGER",    @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"a8f06723-7530-4fc6-8819-b8a304054e04", @"jnelles@wired.biz",        @"AJwFvPMa88DJfASZxVIMtXflii2j+dRPfaSLiYigiQkl0O++gl4LtQS+WHI2y0J2jw==",   @"JOENELLES657733",        @"O",  @"JOE",          @"NELLES",     @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"ac91893c-fb4a-4310-a816-45a31af9a250", @"tthompson@wired.biz",      @"ACRixq9WI4sYQWWIgLZhXSJa4i2HKVAX1Nvke2hOvIsm0kFeVjUiB0AlRn6zGrFhWQ==",   @"TERESA MTHOMPSON33730",  @"O",  @"TERESA M",     @"THOMPSON",   @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"b6f1259e-6dd3-45f3-bc59-6e0183046594", @"rcarey@webworks.com",      @"AO90F53xby8VKuaF7sE3DDUi1iV9+Y20QkPCsBV22VXX2dfweiAZlvROheREAaPVZg==",   @"RAMONA KCAREY568871",    @"O",  @"RAMONA K",     @"CAREY",      @"2015-11-19 22:46:39",  @"2015-11-19 22:46:39"),
                MakeUser(@"bb97c6ab-34e5-4c12-b4e9-445e3ae4e87a", @"wtacopino@celestial.bc.ca",@"AB2IN9fx6XAD0c5c5QhPBRxtglLN1muxXtUp1v1O367OyILMMiumsRaMIc3vyI6Lyg==",   @"WILFORD845878",          @"O",  @"WILFORD J",    @"TACOPINO",   @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"c43a21e4-b25b-4815-b47f-e98b9ada021c", @"jdudycha@netnutz.biz",     @"AFfDeq5u2G+wSHoCf8pIfKCS1/AXHDdDfn5vZMOenORx5mlJ/PFaWibbR+wJKsEfVQ==",   @"JOHN J JR.DUDYCHA77",    @"O",  @"JOHN J JR.",   @"DUDYCHA",    @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"c5ffae0c-60cf-4d60-b977-ea9fa39cdb85", @"jabramowitz@hotwired.com", @"AETFvLez4Aldc/AUWVRxfQxejoSt0C2U7ahdExYs6DIuxJVFm40Tl02actPiQWxavQ==",   @"JUDYABRAMOWITZ23878",    @"O",  @"JUDY",         @"ABRAMOWITZ", @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"cce5e2f8-58d3-447e-89a1-b7f734598f12", @"amowder@goalnet.qc.ca",    @"ANRrwoHbG/GCkJyCyjzDG+ya34rtLj+anjN5HaEY4rR/1jWW3ap3KWZX8TchJDgNoA==",   @"ANNA MARIEMOWDER673",    @"O",  @"ANNA MARIE",   @"MOWDER",     @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"d054b7e3-3683-4863-bf91-eacfe903b128", @"pcostain@maplenet.ca",     @"AKme29gZD2AXNc6/yK8PT89/A1ipwuZ5T4/aEmr7PO5CxEngsOiWuoY7qK/wtEGOnQ==",   @"PAULACOSTAIN1188335",    @"O",  @"PAULA",        @"COSTAIN",    @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"d3a2ec01-316f-41f2-90c0-a3d4bed0e0ce", @"rturner@surfsup.com",      @"AH3qJwXVype8Pc1qNAleAtD4NZRZ7XLc3c/5n5ZH7Krm3biHgz1nPJsp46lu6OOpuw==",   @"RICHARD M.TURNER473",    @"O",  @"RICHARD M.",   @"TURNER",     @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"d56b8f94-bd40-4521-9666-3d0ea395426f", @"jfairweather@funnet.ca",   @"ANWHbW2XvY8rq6DS8Y0VQc4jYfUeug8pv/p2XO7g33614qsMyrYWrhgTzWF+oDQSaA==",   @"JAMES P.FAIRWTHR39",     @"O",  @"JAMES P.",     @"FAIRWEATHER",@"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"d7149b0e-e9d2-412f-aa7e-ccc77b609228", @"jestis@celestial.bc.ca",   @"AB7JFsL2mfsSIdp0fcxZ/vHOGzjlIF9SwGiZFR3Yh0+uK54X85m7tZ9AWFzPxso0nw==",   @"JOHNESTIS244358",        @"O",  @"JOHN",         @"ESTIS",      @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"f00c4480-ef01-4337-b9f8-f895ed821cf1", @"hunterkofler@wired.biz",   @"APN5cBBIH4HpPiaDMqZb4k+7smSqTaGm5i0thowuK3JeGz8nmrbxxMn68VdVQLOiwQ==",   @"HEATHERLUNTERKOFLER5",   @"O",  @"HEATHER L",    @"UNTERKOFLER",@"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"fc9f53e7-7e24-42a6-9d06-ba9fe617f615", @"ckubicki@spider.com",      @"AA9TUtBQVS2mBeB4OoP5diXp9fKxg5L4iTPkQd7OuT609x7gJKt1No+c4U0Y9JW++g==",   @"CRITKUBICKI272730",      @"O",  @"CRIT",         @"KUBICKI",    @"2015-11-19 22:46:39",  @"2015-11-19 22:46:39")
            };

            //MockEmployee Data
            var mockEmployees = new Employee[NUMBER_OF_EMPLOYEES];
            for (int i = 0; i < NUMBER_OF_EMPLOYEES; i++)
            {
                // If the user is already in the db, use that user. 
                ApplicationUser existingUser = context.Users.Find(mockUsers[i].Id);
                mockEmployees[i] = new Employee { User = existingUser ?? mockUsers[i], Id = i };
            }

            // Mock Member Data
            var mockMembers = new Member[mockUsers.Count() - NUMBER_OF_EMPLOYEES];
            for (int i = 0; i < mockMembers.Length; i++)
            {
                // If the user is already in the db, use that user. 
                ApplicationUser existingUser = context.Users.Find(mockUsers[i + NUMBER_OF_EMPLOYEES].Id);
                mockMembers[i] = new Member { User = existingUser ?? mockUsers[i + NUMBER_OF_EMPLOYEES], Id = i + NUMBER_OF_EMPLOYEES };
            }
            context.Members.AddOrUpdate(mockMembers);
            context.Employees.AddOrUpdate(mockEmployees);
            context.SaveChanges();

            
            //Mock Address Data
            var mockAddress = new Address[]
            {               //memberId                  Address                 City            Region      Country     PostalCode
                MakeAddress(@"CRITKUBICKI272730",       "123 Victory Road",     "Kitchener",    "Ontario",  "Canada",   "N2M5B5"),
                MakeAddress(@"HEATHERLUNTERKOFLER5",    "789 Blue Ave",         "Kitchener",    "Ontario",  "Canada",   "N6M7B8"),
                MakeAddress(@"HEATHERLUNTERKOFLER5",    "456 Fake Road",        "Kitchener",    "Ontario",  "Canada",   "N3M4B5"),
                MakeAddress(@"JOHNESTIS244358",         "15 Weber St.",         "Waterloo",     "Ontario",  "Canada",   "N9M8B1"),
                MakeAddress(@"JAMES P.FAIRWTHR39",      "815 Brybeck St.",      "Kitchener",    "Ontario",  "Canada",   "N3M4B7"),
            };

            context.Addresses.AddOrUpdate(mockAddress);
            context.SaveChanges();

            
            
            // Mock Game Data
            var mockGames = new Game[]
            {            // Game Name                          Release Date       Price                Platform                                        Image                                       Publisher       Categories ( Can list none or many )
                MakeGame("Fallout 4",                     "2015-11-11 00:00:00",    79.99m,   PlatformEnum.PS4,    "http://files.sabotagetimes.com/image/upload/MTI5NDc3Mjk5NTgxMDY1Njk0.jpg",           "test",   RatingEnum.E,  CategoryEnum.Action, CategoryEnum.RPG ),
                MakeGame("Footbal Manager 2016",          "2015-09-11 00:00:00",     2.00m,   PlatformEnum.PC,     "http://files.sabotagetimes.com/image/upload/MTI5NDc3Mjk5NTgxMDY1Njk0.jpg",           "test",   RatingEnum.E,  CategoryEnum.Simulation ),
                MakeGame("Skyrim",                        "2015-11-10 00:00:00",    20.00m,   PlatformEnum.PS4,    "http://files.sabotagetimes.com/image/upload/MTI5NDc3Mjk5NTgxMDY1Njk0.jpg",           "test",   RatingEnum.E,  CategoryEnum.RPG ),
                MakeGame("Counter-Strike",                "2015-11-04 00:00:00",     1.00m,   PlatformEnum.PC,     "http://i.imgur.com/09Ytyye.jpg",                                                     "rwar",   RatingEnum.E,  CategoryEnum.FPS, CategoryEnum.Action ),
                MakeGame("rwar",                          "2015-11-10 00:00:00",     1.00m,   PlatformEnum.PC,     "http://files.sabotagetimes.com/image/upload/MTI5NDc3Mjk5NTgxMDY1Njk0.jpg",           "rwar",   RatingEnum.E,  CategoryEnum.Puzzle ),
                MakeGame("test",                          "2015-11-03 00:00:00",     2.00m,   PlatformEnum.PC,     "http://files.sabotagetimes.com/image/upload/MTI5NDc3Mjk5NTgxMDY1Njk0.jpg",           "test",   RatingEnum.E,  CategoryEnum.FPS),
                MakeGame("test",                          "2015-11-03 00:00:00",     2.00m,   PlatformEnum.PC,     "http://files.sabotagetimes.com/image/upload/MTI5NDc3Mjk5NTgxMDY1Njk0.jpg",           "test",   RatingEnum.AO, CategoryEnum.FPS),
                MakeGame("test",                          "2015-11-26 00:00:00",    20.00m,   PlatformEnum.PS4,    "http://files.sabotagetimes.com/image/upload/MTI5NDc3Mjk5NTgxMDY1Njk0.jpg",             null,   RatingEnum.EC, CategoryEnum.FPS),
                MakeGame("test",                          "2015-12-16 00:00:00",    20.00m,   PlatformEnum.PS4,    "http://files.sabotagetimes.com/image/upload/MTI5NDc3Mjk5NTgxMDY1Njk0.jpg",             null,   RatingEnum.EC, CategoryEnum.FPS),
                MakeGame("A Game with no categories",     "2015-11-02 00:00:00",    20.00m,   PlatformEnum.Mobile, "http://files.sabotagetimes.com/image/upload/MTI5NDc3Mjk5NTgxMDY1Njk0.jpg",             null,   RatingEnum.EC)
            };
            context.Games.AddOrUpdate(mockGames);
            context.SaveChanges();
            
            //Mock Order Data
            var mockOrders = new Order[]
            {
                         //UserName                 ApproverId                  orderPlacementDate      shipDate                billAddr    shippAddr   IsProcessed     Games
                //order shipped
                MakeOrder(@"CRITKUBICKI272730",     @"PAMELALALOVIC475670",     "2015-11-11 00:00:00",  "2015-12-11 00:00:00",  1,          1,          true,           "Fallout 4" ),
                //cart made with games, not paid for, not processed, not shipped
                MakeOrder(@"JOHNESTIS244358",       null,                       null,                   null,                   4,          4,          false,          "Footbal Manager 2016", "Counter-Strike"),
                //order shipped
                MakeOrder(@"JOHNESTIS244358",       @"PAMELALALOVIC475670",     "2015-11-11 00:00:00",  "2015-12-11 00:00:00",  4,          4,          true,           "Skyrim"),
                //order paid for, not processed, not shipped
                MakeOrder(@"JAMES P.FAIRWTHR39",    null,                       "2015-11-11 04:00:00",  null,                   5,          5,          false,          "Skyrim", "rwar"),
                //order paid for, order has been processed, not shipped
                MakeOrder(@"HEATHERLUNTERKOFLER5",  @"abaswell@arachnet.ca",    "2015-11-11 04:00:00",  null,                   2,          3,          true,           "Skyrim", "rwar")
            };
            
            var mockFriendships = new Friendship[]
            {
                MakeFriendShip("JOHNESTIS244358", "HEATHERLUNTERKOFLER5", false),
                MakeFriendShip("JOHNESTIS244358", "CRITKUBICKI272730", false),
                MakeFriendShip("JOHNESTIS244358", "PAULACOSTAIN1188335", true),
                MakeFriendShip("JOHNESTIS244358", "RICHARD M.TURNER473", true)
            };

            // Fill the tables
            context.Users.AddOrUpdate(mockUsers);
            context.Friendships.AddOrUpdate(mockFriendships);
            context.Events.AddOrUpdate(new Event[] { });           // TODO: Update this placeholder
            context.Orders.AddOrUpdate(mockOrders);           
            context.OrderItems.AddOrUpdate(new OrderItem[] { });   // TODO: Update this placeholder
            context.Platforms.AddOrUpdate(mockPlatforms.Values.ToArray());
            context.Catagories.AddOrUpdate(mockCategories.Values.ToArray());
            context.Reviews.AddOrUpdate(new Review[] { });         // TODO: Update this placeholder
            context.SaveChanges();
        }



        /// <summary>
        /// Use this method to create a mock addresses
        /// </summary>
        /// <param name="member">Username of the ApplicationUser</param>
        /// <param name="addressName">address</param>
        /// <param name="city">city name</param>
        /// <param name="region">region(/province)</param>
        /// <param name="country">country name</param>
        /// <param name="postalCode">postal code</param>
        /// <returns>Address Object</returns>
        private Address MakeAddress(string member, string addressName, string city, string region, string country, string postalCode)
        {
            Address address = new Address
            {
                Member = db.Members.First(m => m.User.UserName == member),
                StreetAddress = addressName,
                City = city,
                Region = region,
                Country = country,
                PostalCode = postalCode
            };

            return address;
        }

        /// <summary>
        /// Use this method to create a mock order
        /// </summary>
        /// <param name="member">memberId</param>
        /// <param name="aprover">approverId (null if unprocessed)</param>
        /// <param name="orderPlacementDate">order placement date (null for shopping cart)</param>
        /// <param name="shipDate">date that it was shipped out (null if not yet shipped)</param>
        /// <param name="billingAddressIndex">index of billing address</param>
        /// <param name="shippingAddressIndex">index of shipping address</param>
        /// <param name="isProcessed">true if it has been processed</param>
        /// <param name="games">name of all games in the order</param>
        /// <returns>Order Object</returns>
        private Order MakeOrder(string member, string aprover, string orderPlacementDate, string shipDate, int billingAddressIndex, int shippingAddressIndex, bool isProcessed, params string[] games)
        {
            Order order = new Order
            {
                Member = db.Members.First(m => m.User.UserName == member),
                Aprover = db.Employees.FirstOrDefault(e => e.User.UserName == aprover),
                BillingAddress = null, //db.Addresses.First(b => b.Id == billingAddressIndex),
                ShippingAddress = null, //db.Addresses.First(b => b.Id == shippingAddressIndex),
                IsProcessed = isProcessed
            };

            if (shipDate == null)
            {
                order.ShipDate = null;
            }
            else
            {
                order.ShipDate = DateTime.Parse(shipDate);
            }

            if (orderPlacementDate == null)
            {
                order.OrderPlacementDate = null;
            }
            else
            {
                order.OrderPlacementDate = DateTime.Parse(orderPlacementDate);
            }

            /*
            if (aprover == null)
            {
                order.Aprover = null;
            }
            else
            {
                order.Aprover = 
            }*/

            if (order.OrderItems == null)
            {
                order.OrderItems = new List<OrderItem>();
            }

            foreach (string game in games)
            {
                var currGame = db.Games.First(g => g.Name == game);
                order.OrderItems.Add(new OrderItem { Order = order, Game = currGame, SalePrice = currGame.SuggestedRetailPrice });
            }

            return order;
        }

        /// <summary>
        /// Use this method to create a mock game
        /// </summary>
        /// <param name="name">The name of the game</param>
        /// <param name="releaseDate">The date the game was released</param>
        /// <param name="price">The suggested retail price of the game</param>
        /// <param name="platform">The platform this version was released on, the mockPlatforms object must be populated</param>
        /// <param name="image">The URL to an image of the game</param>
        /// <param name="publisher">The game's publisher</param>
        /// <param name="esrb">The rating for this game</param>
        /// <param name="categories">The catagory(ies) that this game falls under. This method requires that the mockCategories object exist and is populated.</param>
        /// <returns>A game object with category relationships in place</returns>
        private Game MakeGame(string name, string releaseDate, decimal price, PlatformEnum platform, string image, string publisher, RatingEnum esrb, params CategoryEnum[] categories)
        {
            Platform platformObj = mockPlatforms[platform];

            // Create game object
            Game game = db.Games.FirstOrDefault(g => g.Name == name && g.Platform.Id == platformObj.Id) ??
                new Game
            {
                Name                 = name,
                ReleaseDate          = DateTime.Parse(releaseDate),
                SuggestedRetailPrice = price,
                Platform             = platformObj,
                ImagePath            = image,
                Publisher            = publisher,
                ESRB                 = Enum.GetName(typeof(RatingEnum), esrb)
            };

            // Add optional categories
            foreach (CategoryEnum category in categories)
            {
                if (game.Categories == null) game.Categories = new List<Category>();
                game.Categories.Add(mockCategories[category]);
            }

            return game;
        }


        private Friendship MakeFriendShip(string userNameFriender, string userNameFriendee, bool isFamily)
        {
            var checkIfExist = db.Friendships.FirstOrDefault(a => a.Friender.User.UserName == userNameFriender && a.Friendee.User.UserName == userNameFriendee);

            //if the Frienship exist will return the existing friendShip
            //this is to avoid duplicate mockData
            if (checkIfExist != null)
            {
                return checkIfExist;
            }

            Member memberFriender = db.Members.FirstOrDefault(f => f.User.UserName == userNameFriender);
            Member memberFriendee = db.Members.FirstOrDefault(f => f.User.UserName == userNameFriendee);

            Friendship res = new Friendship();
            res.Friender = memberFriender;
            res.Friendee = memberFriendee;
            res.IsFamilyMember = isFamily;

            return res;
        }

        /// <summary>
        /// A method to create a mock user
        /// </summary>
        /// <param name="id">The GUID string</param>
        /// <param name="email">Their email address</param>
        /// <param name="passwordHash">A hash of their password</param>
        /// <param name="userName"></param>
        /// <param name="gender"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="birthdate"></param>
        /// <param name="dateRegistered"></param>
        /// <returns></returns>
        private ApplicationUser MakeUser(string id, string email, string passwordHash, string userName, string gender, string firstName, string lastName, string birthdate, string dateRegistered)
        {
            return new ApplicationUser {
                Id = id,
                Email = email,
                PasswordHash = passwordHash,
                SecurityStamp = "",
                UserName = userName,
                Gender = gender,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = DateTime.Parse(birthdate),
                DateOfRegistration = DateTime.Parse(dateRegistered),
                LockoutEnabled = true };
        }

        public void SeedDebug(ApplicationDbContext context)
        {
            Seed(context);
        }
    }
}

