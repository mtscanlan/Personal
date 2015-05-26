using System;
using System.Linq;
using System.Collections.Generic;

namespace GildedRose
{
    class Program
    {
        /*
            Hi and welcome to team Gilded Rose. As you know, we are a small inn with a prime location in a prominent 
            city ran by a friendly innkeeper named Allison. We also buy and sell only the finest goods. Unfortunately, 
            our goods are constantly degrading in quality as they approach their sell by date. We have a system in 
            place that updates our inventory for us. It was developed by a no-nonsense type named Leeroy, who has moved 
            on to new adventures. Your task is to add the new feature to our system so that we can begin selling a new 
            category of items. First an introduction to our system:

	            - All items have a SellIn value which denotes the number of days we have to sell the item
	            - All items have a Quality value which denotes how valuable the item is
	            - At the end of each day our system lowers both values for every item

            Pretty simple, right? Well this is where it gets interesting:

	            - Once the sell by date has passed, Quality degrades twice as fast
	            - The Quality of an item is never negative
	            - "Aged Brie" actually increases in Quality the older it gets
	            - The Quality of an item is never more than 50
	            - "Sulfuras", being a legendary item, never has to be sold or decreases in Quality
	            - "Backstage passes", like aged brie, increases in Quality as it's SellIn value approaches; Quality 
                  increases by 2 when there are 10 days or less and by 3 when there are 5 days or less but Quality drops to 0 
                  after the concert

            We have recently signed a supplier of conjured items. This requires an update to our system:

	            - "Conjured" items degrade in Quality twice as fast as normal items

            Feel free to make any changes to the UpdateQuality method and add any new code as long as everything still 
            works correctly. However, do not alter the Item class or Items property as those belong to the goblin in the 
            corner who will insta-rage and one-shot you as he doesn't believe in shared code ownership (you can make the 
            UpdateQuality method and Items property static if you like, we'll cover for you). Your work needs to be 
            completed by Friday, February 18, 2011 08:00:00 AM PST.

            Just for clarification, an item can never have its Quality increase above 50, however "Sulfuras" is a legendary 
            item and as such its Quality is 80 and it never alters.

         */

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
            } while (ExpiringItems.Any(x => x.Item.SellIn > 0));
            Console.ReadKey();
        }

        // ************* MATTHEWS SOLUTION *************

        private static List<ExpiringItem> ExpiringItems = new List<ExpiringItem>();
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
        // ************* END OF MATTHEWS SOLUTION *************
    }
    
    public class Item {
        public string Name { get; set; }
        public int SellIn { get; set; }
        public int Quality { get; set; }
    }

}
