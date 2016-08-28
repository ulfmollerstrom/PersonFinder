using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HtmlAgilityPack;
using NUnit.Framework;
using RatsitPersonFinder;

namespace RatsitPersonFinder.Test
{
    [TestFixture]
    public class FindPersonTests
    {
        [Test]
        public void FindPersonsWithRatsitWithNameAndBirthdayTest()
        {
            //-- Arrange
            //-- Act
            var persons = Ratsit.FindPersons("Nils", "Andersson", "19700501");

            //-- Assert
            Assert.IsNotEmpty(persons);
            foreach (var person in persons)
            {
                Console.WriteLine(person.Namn);
            }
        }
    }
}
