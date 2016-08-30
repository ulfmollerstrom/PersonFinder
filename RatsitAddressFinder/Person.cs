namespace RatsitPersonFinder
{
    public class Person
    {
        public string Namn => (!string.IsNullOrEmpty(Tilltalsnamn.ToLower().Replace("ej registrerat", "")) ? Tilltalsnamn : Fornamn) + " " + Efternamn;
        public string Personnummer { get; set; }
        public string Fodelsedatum { get; set; }
        public string Fornamn { get; set; }
        public string Tilltalsnamn { get; set; }
        public string Efternamn { get; set; }
        public string COadress { get; set; }
        public string Gatuadress { get; set; }
        public string Postnummer { get; set; }
        public string Postort { get; set; }
        public string Kommun { get; set; }
        public string Lan { get; set; }
        internal string Url { get; set; }
    }
}