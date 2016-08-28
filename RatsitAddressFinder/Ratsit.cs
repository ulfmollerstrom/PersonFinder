using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;

namespace RatsitPersonFinder
{
    public static class Ratsit
    {
        internal static Dictionary<string, string> PropertyMap()
        {
            return new Dictionary<string, string>
            {
                {"Personnummer", "Personnummer"},
                {"Fornamn", "Förnamn"},
                {"Tilltalsnamn", "Tilltalsnamn"},
                {"Efternamn", "Efternamn"},
                {"Fodelsedatum", "Födelsedatum"},
                {"COadress", "C/O-adress"},
                {"Gatuadress", "Gatuadress"},
                {"Postnummer", "Postnummer"},
                {"Postort", "Postort"},
                {"Kommun", "Kommun"},
                {"Lan", "Län"}
            };
        }

        public static List<Person> FindPersons(string firstName, string lastName, string yyyymmdd = "")
        {
            const string wwwRatsitSe = "https://www.ratsit.se";
            const string searchUrl = wwwRatsitSe + "/sok/avancerat/person?";
            var queryString = QueryString(firstName, lastName, yyyymmdd);

            var persons = GetPersons(searchUrl + queryString);

            foreach (var person in persons)
            {
                person.Url = wwwRatsitSe + person.Url;
                SetDetails(person);
            }
            return persons;
        }

        private static void SetDetails(Person person)
        {
            var web = new HtmlWeb();
            var document = web.Load(person.Url);

            var personDetails = GetPersonDetails(document);
            var addressDetails = GetAddressDetails(document);
            SetPersonDetails(person, personDetails);
            SetAddressDetails(person,addressDetails);
        }

        private static void SetAddressDetails(Person person, Dictionary<string, string> addressDetails)
        {
            var propertyMap = PropertyMap();
            person.COadress = addressDetails[propertyMap["COadress"]];
            person.Gatuadress = addressDetails[propertyMap["Gatuadress"]];
            person.Kommun = addressDetails[propertyMap["Kommun"]];
            person.Lan = addressDetails[propertyMap["Lan"]];
            person.Postnummer = addressDetails[propertyMap["Postnummer"]];
            person.Postort = addressDetails[propertyMap["Postort"]];
        }

        private static void SetPersonDetails(Person person, Dictionary<string, string> personDetails)
        {
            var propertyMap = PropertyMap();
            person.Efternamn = personDetails[propertyMap["Efternamn"]];
            person.Fodelsedatum = personDetails[propertyMap["Fodelsedatum"]].Replace(" ", "").Split('(')[0];
            person.Fornamn = personDetails[propertyMap["Fornamn"]];
            person.Personnummer = personDetails[propertyMap["Personnummer"]];
            person.Tilltalsnamn = personDetails[propertyMap["Tilltalsnamn"]];
        }

        private static List<Person> GetPersons(string searchUrl)
        {
            var web = new HtmlWeb();
            var document = web.Load(searchUrl);

            var urls = SelectNodes(document, "//a[contains(@class,\'search-list-content\')]", n => n.Attributes["href"].Value);
            var namesAndAge = SelectNodes(document, "//span[contains(@class,\'name\')]",
                n => HttpUtility.HtmlDecode(n.InnerText));
            var addresses = SelectNodes(document, "//span[contains(@class,\'address\')]",
                n => HttpUtility.HtmlDecode(n.InnerText));

            var persons = Enumerable.Range(0, namesAndAge.Count)
                .Select(i => new Person
                {
                    Url = urls[i]
                }).ToList();
            return persons;
        }

        private static IList<string> SelectNodes(HtmlDocument document, string xPath, Func<HtmlNode, string> selector)
        {
            return document.DocumentNode.SelectNodes(xPath).Select(selector).ToList();
        }

        private static string QueryString(string firstName, string lastName, string yyyymmdd)
        {
            return $"fnamn={firstName}&enamn={lastName}&pnr={yyyymmdd}";
        }

        private static dynamic NameAndAge(string nameage)
        {
            var name = nameage.Split(',')[0];
            var age = nameage.Split(',')[1].Trim();
            return new { Name = name, Age = age };
        }

        private static Dictionary<string, string> GetAddressDetails(HtmlDocument document)
        {
            var tmp = document.DocumentNode.SelectNodes("//*[@id=\"show7\"]/table[1]//tr//td")
                .Where(node => node.Attributes["colspan"] == null)
                .Select(node => HttpUtility.HtmlDecode(node.InnerText).Replace(":", ""))
                .ToList();

            var address = new Dictionary<string, string>();
            for (var i = 0; i < tmp.Count; i += 2)
            {
                address.Add(tmp[i], tmp[i + 1]);
            }
            return address;
        }

        private static Dictionary<string, string> GetPersonDetails(HtmlDocument document)
        {
            var props = SelectNodes(document, "//dt[contains(@class,\'\')]", n => n.InnerText.Replace(":", ""));
            var values = SelectNodes(document, "//dd[contains(@class,\'overview-text\')]", n => HttpUtility.HtmlDecode(n.InnerText));

            var details = props.Zip(values, (p, v) => new { p, v }).ToDictionary(item => item.p, item => item.v);
            return details;
        }

    }
}
