using System;
using System.Linq;
using System.Collections.Generic;

namespace GildedRose
{
    class Program
    {
        private static List<ExpiringItem> ExpiringItems = new List<ExpiringItem>();
        private static IList<Item> Items = new List<Item>() {
            new Item {Name = "+5 Dexterity Vest", SellIn = 10, Quality = 20},
            new Item {Name = "Aged Brie", SellIn = 2, Quality = 0},
            new Item {Name = "Elixir of the Mongoose", SellIn = 5, Quality = 7},
            new Item {Name = "Sulfuras, Hand of Ragnaros", SellIn = 0, Quality = 80},
            new Item {Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 15, Quality = 20},
            new Item {Name = "Conjured Mana Cake", SellIn = 3, Quality = 6}
        };

        static void Main(string[] args) {
            Console.WriteLine(string.Join("\n", Items.Select(x => String.Format("Name: {0} SellIn: {1} Quality: {2}", x.Name, x.SellIn, x.Quality))));
            do
            {
                UpdateQuality();
                Console.WriteLine(string.Join("\n", ExpiringItems));
            } while (ExpiringItems.Any(x => x.Item.SellIn >= 0));
            Console.ReadKey();
        }

        public static void UpdateQuality() {
            if (ExpiringItems.Count == 0)
            {
                ExpiringItems = new List<ExpiringItem>();
                foreach (var item in Items)
                {
                    ExpiringItem expiringItem = null;
                    if (item.Name == "Aged Brie")
                    {
                        expiringItem = new ExpiringItem(item)
                        {
                            DegradationValue = 1,
                            ExpiredDegradationValue = 1
                        };
                    }
                    else if (item.Name == "Sulfuras, Hand of Ragnaros")
                    {
                        expiringItem = new ExpiringItem(item)
                        {
                            CustomDegradation = (x) => x.Item.Quality = 80
                        };
                    }
                    else if (item.Name == "Backstage passes to a TAFKAL80ETC concert")
                    {
                        expiringItem = new ExpiringItem(item)
                        {
                            DegradationValue = 1,
                            ExpiredDegradationValue = 0,
                            CustomDegradation = (x) =>
                            {
                                if (x.Item.SellIn < 0)
                                {
                                    x.DegradationValue = 0;
                                    x.Item.Quality = 0;
                                }
                                else if (x.Item.SellIn <= 5)
                                {
                                    x.DegradationValue = 3;
                                }
                                else if (x.Item.SellIn <= 10)
                                {
                                    x.DegradationValue = 2;
                                }
                            }
                        };
                    }
                    else if (item.Name == "Conjured Mana Cake")
                    {
                        expiringItem = new ExpiringItem(item)
                        {
                            DegradationValue = -2,
                            ExpiredDegradationValue = -4
                        };
                    }
                    else
                    {
                        expiringItem = new ExpiringItem(item);
                    }
                    ExpiringItems.Add(expiringItem);
                }
            }

            foreach (var item in ExpiringItems)
            {
                item.Item.SellIn--;
                item.UpdateQuality();
                item.CustomDegradation(item);
            }
        }

    }

    public class ExpiringItem
    {
        public int DegradationValue { get; set; }
        public int ExpiredDegradationValue { get; set; }
        public Action<ExpiringItem> CustomDegradation { get; set; }
        public Item Item { get; private set; }

        public ExpiringItem(Item item)
        {
            Item = item;

            DegradationValue = -1;
            ExpiredDegradationValue = -2;
            CustomDegradation = (expiringitem) => { };
        }

        public void UpdateQuality()
        {
            Item.Quality += Item.SellIn >= 0 ? DegradationValue : ExpiredDegradationValue;
            Item.Quality = Item.Quality < 0 ? 0 : Item.Quality;
            Item.Quality = Item.Quality > 50 ? 50 : Item.Quality;
        }

        public override string ToString()
        {
            return String.Format("Name: {0} SellIn: {1} Quality: {2}", Item.Name, Item.SellIn, Item.Quality);
        }
    }

    public class Item {
        public string Name { get; set; }
        public int SellIn { get; set; }
        public int Quality { get; set; }
    }

}
