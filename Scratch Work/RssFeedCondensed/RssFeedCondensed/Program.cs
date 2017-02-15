using System;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;
using NDesk.Options;

namespace RssFeedCondensed
{
    class Program
    {
        static void Main(string[] args)
        {
            bool search = false;
            string searchField = string.Empty;
            bool showHelp = false;
            var options = new OptionSet()
                {
                    { "s|search=", "Specify whether to search. [true, false]", v => search = Convert.ToBoolean(v) },
                    { "v|value=", "The string value to search", v => searchField = v },
                    { "h|help",  "Show help.", v => showHelp = v != null },
                };
            options.Parse(args);

            if (showHelp)
                options.WriteOptionDescriptions(Console.Out);
            else
                SyndicationFeed.Load(XmlReader.Create("http://rss.cnn.com/rss/cnn_topstories.rss"))
                    .Items
                    .Where(w => search ? w.Summary.Text.Contains(searchField) : true)
                    .OrderByDescending(s => s.PublishDate)
                    .Take(5)
                    .ToList()
                    .ForEach(fe =>
                        Console.WriteLine($"{fe.Title.Text}\n{fe.Summary.Text}\n{fe.PublishDate}\n{fe.Id}"));

            Console.ReadLine();
        }
    }
}
