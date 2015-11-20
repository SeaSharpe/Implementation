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
        public static int MEMBER_ID_START = 30000000;

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            var mockUsers = new ApplicationUser[] {
                MakeUser(@"03ec4e30-6ba2-48b0-ae82-3d8e30d6307b", @"abaswell@arachnet.ca",  @"APXQhbnuJfhSBojKarJ0a9m6/imzNjs77rmVEoTZf7F8MqnMwiGU//EA9Y9GVOfw3A==",  @"f58e0ee9-75c5-4da8-aa6a-34be62860492",  @"AYSEBASWELL622782",  @"O",  @"AYSE",  @"BASWELL",  @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"0bc70e50-6ed5-49f1-a1f0-a8d262dea2fa", @"plalovic@cheapnfast.com",  @"AAxhW7ZHYubxVSiMREe+PDHyZMUU+2uhriBEVECzzeUBhWaHzR78dPL969/J9S5jCQ==",  @"8bc735d6-6b27-49b9-ac94-5bc216efdf14",  @"PAMELALALOVIC475670",  @"O",  @"PAMELA",  @"LALOVIC",  @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"0fedaad9-325a-46c3-8295-fdf78fe2e09d", @"nlee@netnutz.biz",  @"AFFcvX/d/E7tFvSiCfaNMxXok89R2AyCgonpP6Np9qmSlYULEvrA9vcYO/83kRnBjw==",  @"30859149-5622-47ca-8fc3-b68357c06c10",  @"NELLIELEE361672",  @"O",  @"NELLIE",  @"LEE",  @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"12482d3e-67e9-4d73-9e17-42a690c8581c", @"mbrookins@funnet.ca",  @"AM3YMJEEoRF5MUJYECV5vpMh/FTWt/Bmpmy6vxhXNyrnTqmr4WpqCuej9GbnzBH50g==",  @"06851347-3e16-47c7-87d5-8d8cab840875",  @"MELISSABROOKINS141879",  @"O",  @"MELISSA",  @"BROOKINS",  @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"13c5b2fd-2681-419a-b990-4754fdda1a1f", @"gkhan@wired.biz",  @"ALvJCb4sFvBnEzwqZUSMxvczGmsDuHyaUFj3xSzhdfmTvd7IjIz8ix4NXYOoaqnTwA==",  @"0e6ed642-d4f5-4880-9d35-e2f7a280a850",  @"GERTRUDEKHAN464278",  @"O",  @"GERTRUDE",  @"KHAN",  @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"185ecea7-4dc1-4ba5-b727-9d57268e64ae", @"lpelot@hotwired.com",  @"AFvJ+R6/AmUuCiuaT499/jgmCqbbIGfZBXJlbBZkhSJiCGXKwoPfCdL9MKOYZNKecA==",  @"5ea94c8b-7b34-430b-a109-b6be94ba4cf2",  @"LOUISPELOT715279",  @"O",  @"LOUIS",  @"PELOT",  @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"211072f3-9666-4c5c-9cfd-b0180ebc45fa", @"kfreet@webworks.com",  @"AE7PWrjO0UjZ+NgQgV6FIXCpFFsbS9Jlg7F1LseEkljCjBCKt0tKQ1hzZ+Q45T+U2g==",  @"61f19b5d-e851-4abd-ab78-8bf7a906f396",  @"KELLY L.FREET512339",  @"O",  @"KELLY L.",  @"FREET",  @"2015-11-19 22:46:39",  @"2015-11-19 22:46:39"),
                MakeUser(@"21f2a613-df08-44cb-b71c-db254085c074", @"drisatti@netnutz.biz",  @"APoNKqUpjcH+C4HIv1pThV9cWSfXmQBK6S8DhMKgYswcUm4WgkDpDBh95zpfc144pg==",  @"42fcdecb-c404-4dba-94e7-08f3efb9907d",  @"DOMONIQUERISATTI375874",  @"O",  @"DOMONIQUE",  @"RISATTI",  @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"25c8b5e0-ee1a-46b4-a9f1-8e543a6f555d", @"ecastenada@nolimits.com",  @"AD8X968nrxv+eoYvC6niKRg+FyS+hpsjHWHniYQL7jrF9/U3nYUknjHSwZch1BuQWA==",  @"a16bb509-8f7d-4acc-985f-08ab70bb4c0d",  @"EFFIECASTENADA664232",  @"O",  @"EFFIE",  @"CASTENADA",  @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"29a3e51c-d9d8-4359-83ba-57b74a1f9c5f", @"kperry@maplenet.ca",  @"AINwnT84pRS9uakp9pkPUerK8ZN0VXasxxmMZqvBIFwXM0UIfyg2Jv6gvGSVkqEp3w==",  @"da5f0f29-e214-48b4-884e-ed2c682d16bb",  @"KIMPERRY883536",  @"O",  @"KIM",  @"PERRY",  @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"2dec193e-7f24-4584-924d-36f32c23b548", @"lyokley@wired.biz",  @"AF2BuroNlwlHstJnQt8fGxVUSE8dgME/UZpONaq2GuWMOX7S/sqCZ5hgz5Z9YbKPYw==",  @"75551f32-6371-4d2e-a6fe-2ca0ee2dfc28",  @"LEWISYOKLEY438432",  @"O",  @"LEWIS",  @"YOKLEY",  @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"2e6000ef-07a1-4b54-b012-c17c8738060f", @"rbrangan@webworks.com",  @"AHTwDr2x2aiDNX+mTMQtykFQgqvRWL0ev831AMXTkyriX2L2H0Lb9sFH+Vu3K4SWzg==",  @"9005b3ed-ccc2-4717-b568-f0559ab54112",  @"ROSEBRANGAN283330",  @"O",  @"ROSE",  @"BRANGAN",  @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"32d53f96-6115-4f08-9942-4ceb9f1e2ec4", @"rsnow@wired.biz",  @"AH/IPfKGZzCOu/LxPOk2LpfuuohTE4VHM8KuW6ZkXkmctBfyPYkaIbOjN7ACfTcSyA==",  @"87f45c90-52e1-4496-be8b-5bc97f62029b",  @"RSNOW726575",  @"O",  @"R",  @"SNOW",  @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"33ed6601-6173-4608-95ea-5f483cb11479", @"wmillian@wired.biz",  @"AFUmdg+m1k+fE7NFPDsYigJzH3hWhv4mfuNe62OeJ3dtTc8kd6NRJip7/f8aRle6XQ==",  @"1d693344-be62-44a5-9cca-0788c01c9957",  @"WILLIAM A.MILLIAN788469",  @"O",  @"WILLIAM A.",  @"MILLIAN",  @"2015-11-19 22:46:38",  @"2015-11-19 22:46:38"),
                MakeUser(@"36a311ea-36df-4050-b3cf-9e81399cff76", @"adermott@fishnet.nl.ca",  @"AJ7oEQfqFOFWWcsgy2kuHgPP2ZpJIuBLTCQDq+89BNXVKFKD+m3wc+Yvwdd0mcZIPA==",  @"f1bc698b-ae55-4bb9-b7d5-e421e6fa55e5",  @"ALANDERMOTT746130",  @"O",  @"ALAN",  @"DERMOTT",  @"2015-11-19 22:46:39",  @"2015-11-19 22:46:39"),
                MakeUser(@"3cf4a91f-5ed2-4c1d-8d57-64ff6b4a7485", @"dmcbride@cheapnfast.com",  @"AOIzfs8EZkWM4LijauQvwB+xosZ47/DnlUklUWC7BQyZQxfGCCg+oOFwNrCkuiH6Kw==",  @"a0d77466-f5ed-47d9-9fd5-535346df6cb4",  @"DOROTHYMCBRIDE276135",  @"O",  @"DOROTHY",  @"MCBRIDE",  @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"3d20e201-2a24-4abd-8a34-2f0b66aba586", @"jpotmesil@arachnet.ca",  @"AKGU6GW0LdO/g6eddbAU5favjeI3QGwneIeMhxoaP+WWQacNj70Ej2uFmzPmxN3Lzw==",  @"6c707f9a-cd44-418a-a398-af3896aa9d29",  @"JAQUELYNPOTMESIL152377",  @"O",  @"JAQUELYN",  @"POTMESIL",  @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"4521cf1c-ac58-4941-aff0-9b927e900e44", @"mbrazan@hotwired.com",  @"ACCugK+XrgEfXZglpk/rIJjPfCB72xmi56mcxnftI+7HVD/fulps+pZjWCnNpNHc8g==",  @"d3059a91-b96e-4cb8-966a-e9741a97c239",  @"MICHAEL (MIKE)BRAZAN753739",  @"O",  @"MICHAEL (MIKE)",  @"BRAZAN",  @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"4809d300-5765-4946-8c65-003e1192e4a7", @"estubbert@wired.biz",  @"AAWopK7TDUd2cYd2knGRSXSZwoSIS/zAEMioTE7kzNyjZbYUknXGQGK4XbD76esjmQ==",  @"c4a8419f-3ba9-4918-8812-374df01f8e79",  @"ELICIASTUBBERT437131",  @"O",  @"ELICIA",  @"STUBBERT",  @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"4cd9af4c-b394-45d8-bc6e-1b3c2c145e0f", @"gbarsalou@netnutz.biz",  @"AEg7/cIUFGagmDCgEyothcrggoUEBSFmeRlAGjzayhZ/G7XzIcEfKK7yScC38DPllA==",  @"588c8cf6-2945-43c0-99bd-269d6d9d645b",  @"GWENBARSALOU323131",  @"O",  @"GWEN",  @"BARSALOU",  @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"54620e36-75f7-419c-bc5d-07d0683b3047", @"smaline@scratchworx.com",  @"AMkZnvI5aGvl/24sf3QBn9uU1YMLUoDJtNxpbAsge9YJCkhB6HK+UEGN7S852mWUMg==",  @"8ff7d551-66d8-4202-99ae-57b5fb8c8d92",  @"SHONDRAMALINE433334",  @"O",  @"SHONDRA",  @"MALINE",  @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"61eb4d9b-be16-4f76-87d3-517db46d7c31", @"jsolow@funnet.ca",  @"AIG+Xj+G8q3hyLVXqdCc6FJfcr5sx5Har4UtP8vQ3c01cE3S4744zl9TSYVlnbmEtQ==",  @"f5aa4b50-d711-4c81-99e3-938c413f4cb5",  @"JEFFERSONSOLOW461474",  @"O",  @"JEFFERSON",  @"SOLOW",  @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"6c605a25-6bd9-418f-b76d-9760d4425655", @"tvalasco@whatnet.on.ca",  @"AJYjvf56TmcGP3hSH6bHVtTvllrVF1sO+GWeDOCfejIv1w5W+tDzU1H5Nj2DsUhnAA==",  @"bc2de8d1-1a64-49ec-9664-85409bf06d6c",  @"THOMASVALASCO422739",  @"O",  @"THOMAS",  @"VALASCO",  @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"72879d9d-fe3e-486b-ab91-0f7a171b286d", @"bmillard@arachnet.ca",  @"AM30FYuw+aOQuAbm/NWLHuPvrQnyvWPE7cKc4ba/Ig2aTMNSVY+msPfRaEZgFqmTng==",  @"aa0584fb-118a-4517-a1f5-e4c00ffa1e42",  @"BETHMILLARD721478",  @"O",  @"BETH",  @"MILLARD",  @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"73bed6c4-2306-45d9-9d65-c54bfcf92881", @"amannix@whatnet.on.ca",  @"AHQQXQza8ZNe56zxnbKeC/a9mp6hicTVcGQ3lNKVHDb6fF/OX8U4B7PbGaHFqe0vxQ==",  @"aba69af0-50ac-436f-8534-7504880e64c0",  @"ANNEMANNIX757533",  @"O",  @"ANNE",  @"MANNIX",  @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"7c6c7be0-ee34-45dc-aeb0-4828a4823f55", @"twilliams@gamergold.com",  @"AP+FiY4baFW/mCfW3eQAx5MpBjGBht2SfqXFzmhavgiWP+bCiIrVhTAo2bqGzFFoRQ==",  @"145bf657-0ac8-4d60-a86e-aea95d7717cf",  @"THOMASWILLIAMS156589",  @"O",  @"THOMAS",  @"WILLIAMS",  @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"7f78b475-e428-45ce-a517-1e74fe386825", @"bxaintonge@whatnet.on.ca",  @"AGBIcSzWQVjNiO6qsmZiwsvwqJ/fcPy+jaN8e62QvsAMoClg/qKUyEsh+zbUcuepAg==",  @"92e58e3a-1aed-49c8-bc1f-d1fcf9ea1c98",  @"BERNARDXAINTONGE234274",  @"O",  @"BERNARD",  @"XAINTONGE",  @"2015-11-19 22:46:39",  @"2015-11-19 22:46:39"),
                MakeUser(@"87dffe06-9212-4f6e-a509-f1d51bd4175b", @"rprice@celestial.bc.ca",  @"ANkTpS76Hiu6Yt92CaNTAy9wzjAZ95SE7IkK7aun3hmo9/7W8EW0490jQLe5bmKS2w==",  @"34119e0a-09c8-4281-aea7-5ccada067383",  @"REUBEN B.PRICE187530",  @"O",  @"REUBEN B.",  @"PRICE",  @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"8b9f4b36-aad4-449c-8669-404dc2f1f3f5", @"swinsman@wired.biz",  @"AMC8ikO8Dc+nB+tFdl1MyRbcpOMhEGqCRHUGswFK0rcI9UAzavJWIpL6nfoIqBuK5Q==",  @"affe7fa6-6e0f-4f24-9738-5696814a8664",  @"SARWATWINSMAN553131",  @"O",  @"SARWAT",  @"WINSMAN",  @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"8bee9544-6dbc-440c-b72a-3aa7222c8aaf", @"mdoorley@nolimits.com",  @"AMCu8g3OthN9ErDHyox7SWWmjgzheAHOyuR/RSEKV5pFHK489zzdCsU9uYwcQ1vaew==",  @"0f667c5e-203a-4b34-9c0f-70f640f85a8d",  @"MICHAELDOORLEY145275",  @"O",  @"MICHAEL",  @"DOORLEY",  @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"8ee66ba1-d4ef-4298-8c70-57938348fbcd", @"kcherny@goalnet.qc.ca",  @"ACDGHWjeTkK+borepd1XCjz+w9Y3KrCkOR+gbe65SVJMSkfaIidaKDrtcfXHxylbuw==",  @"34e98e60-e71b-4091-aa73-0a297c68dafe",  @"KAYCHERNY183636",  @"O",  @"KAY",  @"CHERNY",  @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"9afeafe2-1096-4aa7-aea5-ef7e0e1bda22", @"pstmartin@cannet.ca",  @"ANriDddVgRIPwkB2niDr+uWf5+XaO6ag+DMmfhx8x6XJllVv1GI6bw36FvGYqeiHKw==",  @"147a0e5b-68ed-4d06-a743-8fa02656cea4",  @"PAUL DSTMARTIN718878",  @"O",  @"PAUL D",  @"STMARTIN",  @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"9b51f209-3716-43c9-b52b-5039d825da7e", @"fsunyich@gamergold.com",  @"AEJTM6FF9McJNwPNIAvYXp36iRTMpdg8SBgY1CjBtnW6625EFyGGvaT/scNMWW7K9A==",  @"0cdb4b9f-577f-4dea-892e-4d4f4baeadbd",  @"FAITHSUNYICH471779",  @"O",  @"FAITH",  @"SUNYICH",  @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"9c33b0a9-4a5a-4775-abec-7663894e58eb", @"ypetri@cheapnfast.com",  @"AN74OL514hgtEcv8O773yPAq7xgytDyf+uZ5ejSSYiSk0EVkMgfWDnzbNsVBrPn14g==",  @"57bda455-6762-4714-a370-3ef1f41699a8",  @"YVONNE V.PETRI476174",  @"O",  @"YVONNE V.",  @"PETRI",  @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"a22ecfde-309b-4f66-815e-dd3557180339", @"lcarabine@arachnet.ca",  @"APUPQS3J+4GQLR8e4oX3ps3I7d1A1ICgfTazd4hywDJhtJALOB53Sny4CUMZxMxhQQ==",  @"01a81bc4-e12b-46fa-ab3e-8bb884f7daeb",  @"LILLIAN RCARABINE444648",  @"O",  @"LILLIAN R",  @"CARABINE",  @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"a7105c8d-9033-4e82-8dd1-63d8a8167814", @"jlofstrom@fishnet.nl.ca",  @"AAtSwvoURkCi6s9wNJZxVh62u9/yvSb0l1KN9ZQqA/GfTKio2xVw+7bypJ/cPbbj2A==",  @"735bd8b2-2aef-4059-82da-056df1ac907f",  @"JAMES VLOFSTROM784675",  @"O",  @"JAMES V",  @"LOFSTROM",  @"2015-11-19 22:46:39",  @"2015-11-19 22:46:39"),
                MakeUser(@"a8928339-a160-41ee-82ad-289990dfda51", @"cswauger@whatnet.on.ca",  @"AJIi+QnNhzZx/h0LKEnXWdPwUEf+ToEaVgC7r83rG8f2hi+hCj8AjBP/pjuB4PBisg==",  @"27a68419-4744-4318-ba6f-63d03b51c3a3",  @"CLAUDE DSWAUGER841272",  @"O",  @"CLAUDE D",  @"SWAUGER",  @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"a8f06723-7530-4fc6-8819-b8a304054e04", @"jnelles@wired.biz",  @"AJwFvPMa88DJfASZxVIMtXflii2j+dRPfaSLiYigiQkl0O++gl4LtQS+WHI2y0J2jw==",  @"56dc7869-127c-458b-a2e4-431009c85336",  @"JOENELLES657733",  @"O",  @"JOE",  @"NELLES",  @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"ac91893c-fb4a-4310-a816-45a31af9a250", @"tthompson@wired.biz",  @"ACRixq9WI4sYQWWIgLZhXSJa4i2HKVAX1Nvke2hOvIsm0kFeVjUiB0AlRn6zGrFhWQ==",  @"41ed9fd5-3945-4796-8e2e-6b8269384ecf",  @"TERESA MTHOMPSON337330",  @"O",  @"TERESA M",  @"THOMPSON",  @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"b6f1259e-6dd3-45f3-bc59-6e0183046594", @"rcarey@webworks.com",  @"AO90F53xby8VKuaF7sE3DDUi1iV9+Y20QkPCsBV22VXX2dfweiAZlvROheREAaPVZg==",  @"8faa3642-a871-46e7-b5ce-b4b5f7542202",  @"RAMONA KCAREY568871",  @"O",  @"RAMONA K",  @"CAREY",  @"2015-11-19 22:46:39",  @"2015-11-19 22:46:39"),
                MakeUser(@"bb97c6ab-34e5-4c12-b4e9-445e3ae4e87a", @"wtacopino@celestial.bc.ca",  @"AB2IN9fx6XAD0c5c5QhPBRxtglLN1muxXtUp1v1O367OyILMMiumsRaMIc3vyI6Lyg==",  @"a0db28a5-d617-4654-b0d7-dab3ba9d2295",  @"WILFORD JTACOPINO845878",  @"O",  @"WILFORD J",  @"TACOPINO",  @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"c43a21e4-b25b-4815-b47f-e98b9ada021c", @"jdudycha@netnutz.biz",  @"AFfDeq5u2G+wSHoCf8pIfKCS1/AXHDdDfn5vZMOenORx5mlJ/PFaWibbR+wJKsEfVQ==",  @"1487c27b-b13a-4702-bbd9-3605866da61b",  @"JOHN J JR.DUDYCHA752337",  @"O",  @"JOHN J JR.",  @"DUDYCHA",  @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"c5ffae0c-60cf-4d60-b977-ea9fa39cdb85", @"jabramowitz@hotwired.com",  @"AETFvLez4Aldc/AUWVRxfQxejoSt0C2U7ahdExYs6DIuxJVFm40Tl02actPiQWxavQ==",  @"dfab6bcb-f29c-4e13-929c-9445a5724c82",  @"JUDYABRAMOWITZ238478",  @"O",  @"JUDY",  @"ABRAMOWITZ",  @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"cce5e2f8-58d3-447e-89a1-b7f734598f12", @"amowder@goalnet.qc.ca",  @"ANRrwoHbG/GCkJyCyjzDG+ya34rtLj+anjN5HaEY4rR/1jWW3ap3KWZX8TchJDgNoA==",  @"1cc155ff-a5be-40cd-86ea-4aa77f11d46a",  @"ANNA MARIEMOWDER244673",  @"O",  @"ANNA MARIE",  @"MOWDER",  @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"d054b7e3-3683-4863-bf91-eacfe903b128", @"pcostain@maplenet.ca",  @"AKme29gZD2AXNc6/yK8PT89/A1ipwuZ5T4/aEmr7PO5CxEngsOiWuoY7qK/wtEGOnQ==",  @"29c4a000-3511-463c-84e3-8662011ac7e8",  @"PAULACOSTAIN118835",  @"O",  @"PAULA",  @"COSTAIN",  @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"d3a2ec01-316f-41f2-90c0-a3d4bed0e0ce", @"rturner@surfsup.com",  @"AH3qJwXVype8Pc1qNAleAtD4NZRZ7XLc3c/5n5ZH7Krm3biHgz1nPJsp46lu6OOpuw==",  @"7e353206-421e-4610-bc74-b59c3c273da6",  @"RICHARD M.TURNER515473",  @"O",  @"RICHARD M.",  @"TURNER",  @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"d56b8f94-bd40-4521-9666-3d0ea395426f", @"jfairweather@funnet.ca",  @"ANWHbW2XvY8rq6DS8Y0VQc4jYfUeug8pv/p2XO7g33614qsMyrYWrhgTzWF+oDQSaA==",  @"326fed4d-bd9a-4c8e-b68b-9e0756f31156",  @"JAMES P.FAIRWEATHER767839",  @"O",  @"JAMES P.",  @"FAIRWEATHER",  @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"d7149b0e-e9d2-412f-aa7e-ccc77b609228", @"jestis@celestial.bc.ca",  @"AB7JFsL2mfsSIdp0fcxZ/vHOGzjlIF9SwGiZFR3Yh0+uK54X85m7tZ9AWFzPxso0nw==",  @"68b500da-ac0b-41ed-9e72-ebb9e22277d9",  @"JOHNESTIS244358",  @"O",  @"JOHN",  @"ESTIS",  @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"f00c4480-ef01-4337-b9f8-f895ed821cf1", @"hunterkofler@wired.biz",  @"APN5cBBIH4HpPiaDMqZb4k+7smSqTaGm5i0thowuK3JeGz8nmrbxxMn68VdVQLOiwQ==",  @"669c59bf-a608-428a-ba22-b61fdd1d73cd",  @"HEATHER LUNTERKOFLER426531",  @"O",  @"HEATHER L",  @"UNTERKOFLER",  @"2015-11-19 22:46:40",  @"2015-11-19 22:46:40"),
                MakeUser(@"fc9f53e7-7e24-42a6-9d06-ba9fe617f615", @"ckubicki@spider.com",  @"AA9TUtBQVS2mBeB4OoP5diXp9fKxg5L4iTPkQd7OuT609x7gJKt1No+c4U0Y9JW++g==",  @"903542ae-5dbb-4731-abfd-dc39fa0c5a8e",  @"CRITKUBICKI272730",  @"O",  @"CRIT",  @"KUBICKI",  @"2015-11-19 22:46:39",  @"2015-11-19 22:46:39")
            };

            var mockMembers = new Member[mockUsers.Count()];
            for (int i = 0; i < mockMembers.Length; i++)
            {
                mockMembers[i] = new Member { User = mockUsers[i], Id = i + 1 + MEMBER_ID_START };
            }

            context.Users.AddOrUpdate(mockUsers);

            context.Members.AddOrUpdate(mockMembers);

            context.Platforms.AddOrUpdate(new Platform[] {});

            context.Games.AddOrUpdate(new Game[] {});
        }

        private ApplicationUser MakeUser(string id, string email, string passwordHash, string securityStamp, string userName, string gender, string firstName, string lastName, string birthdate, string dateRegistered)
        {
            return new ApplicationUser { Id = id, Email = email, PasswordHash = passwordHash, SecurityStamp = securityStamp, UserName = userName, Gender = gender, FirstName = firstName, LastName = lastName, DateOfBirth = DateTime.Parse(birthdate), DateOfRegistration = DateTime.Parse(dateRegistered), LockoutEnabled = true };
        }
    }
}

