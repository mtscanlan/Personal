using System;
using System.Linq;
using System.Collections.Generic;

namespace GildedRose
{
    class Program
    {
        private static IList<ExpiringItem> Items = new List<ExpiringItem>() {
            new ExpiringItem {Name = "+5 Dexterity Vest", SellIn = 10, Quality = 20},
            new ExpiringItem {Name = "Aged Brie", SellIn = 2, Quality = 0, DegradationValue = 1, ExpiredDegradationValue = 1},
            new ExpiringItem {Name = "Elixir of the Mongoose", SellIn = 5, Quality = 7},
            new ExpiringItem {Name = "Sulfuras, Hand of Ragnaros", SellIn = 0, Quality = 80, CustomDegradation = (expiringItem) => expiringItem.Quality = 80},
            new ExpiringItem {Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 15, Quality = 20, DegradationValue = 1, ExpiredDegradationValue = 0,
                CustomDegradation = (expiringItem) => {
                    if (expiringItem.SellIn < 0) {
                        expiringItem.DegradationValue = 0;
                        expiringItem.Quality = 0;
                    } else if (expiringItem.SellIn <= 5) {
                        expiringItem.DegradationValue = 3;
                    } else if (expiringItem.SellIn <= 10) {
                        expiringItem.DegradationValue = 2;
                    }
                }},
            new ExpiringItem {Name = "Conjured Mana Cake", SellIn = 3, Quality = 6, DegradationValue = -2, ExpiredDegradationValue = -4}
        };



        static void Main(string[] args) {
            Console.WriteLine(string.Join("\n", Items));
            while(Items.Any(x => x.SellIn >= 0)) {
                UpdateQuality();
                Console.WriteLine(string.Join("\n", Items));
            }
            Console.ReadKey();
        }

        private class ExpiringItem : Item
        {
            public int DegradationValue { get; set; }
            public int ExpiredDegradationValue { get; set; }
            public Action<ExpiringItem> CustomDegradation { get; set; }

            public ExpiringItem()
            {
                DegradationValue = -1;
                ExpiredDegradationValue = -2;
                CustomDegradation = (expiringitem) => { };
            }

            public void UpdateQuality() {
                Quality += SellIn >= 0 ? DegradationValue : ExpiredDegradationValue;
                Quality = Quality < 0 ? 0 : Quality;
                Quality = Quality > 50 ? 50 : Quality;
            }

            public override string ToString()
            {
                return String.Format("Name: {0} SellIn: {1} Quality: {2}", Name, SellIn, Quality);
            }
        }
        public static void UpdateQuality() {
            foreach (var item in Items) {
                item.SellIn--;
                item.UpdateQuality();
                item.CustomDegradation(item);
            }
        }

    }

    public class Item {
        public string Name { get; set; }
        public int SellIn { get; set; }
        public int Quality { get; set; }
    }

}
