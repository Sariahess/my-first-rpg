using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sariah_assign2_RPG_Game
{
    class Game
    {
        public Hero Hero { get; set; }
        public List<Monster> Monsters { get; set; }
        public List<Monster> Bosses { get; set; }
        public Fight Fight { get; set; }

        public Game()
        {
            Monsters = new List<Monster>();
            Bosses = new List<Monster>();
        }

        public void InitializeGame()
        {
            AddToMonsters();
            Console.WriteLine("Please name your hero. (in 1 word)");
            var playerName = Console.ReadLine();

            while (playerName == "" || playerName.Contains(" "))
            {
                if (playerName.Contains(" ")) Console.WriteLine("NO SPACES please.");
                else Console.WriteLine("No name?! That's sad...");
                playerName = Console.ReadLine();
            }

            int fixedHp = int.Parse(ConfigurationManager.AppSettings.Get("FixedOriginalHealth"));
            CreateHero(playerName, 0, 0, fixedHp, this);
            Console.WriteLine("---------------------");
            Console.WriteLine("Monsters are randomly chosen for you per match.");
            Console.WriteLine("Docile monsters don't attack you first; Aggressive monsters do!!");
            Console.WriteLine("You may choose to fight a boss monster when you have more than 500 coins in your possession. All bosses are aggressive.");
            Console.WriteLine("You may quit the game by closing this console window, however, all your accomplishments will be lost once you quit.");
            Console.WriteLine("In order to equip, you require coins of the item's price.");
            Console.WriteLine("You must choose a weapon and an armor right before each battle.");
            Console.WriteLine("As far as your coin amount allows, it's better if you equip the strongest weapon and armor.");
            Console.WriteLine("Follow the instructions or press any key to continue.");
            Console.WriteLine("GOOD LUCK!!");
            Console.WriteLine("---------------------");
            Console.WriteLine("");
            Console.ReadKey();
            Console.WriteLine($"Welcome { Hero.Name }!!");
            Console.WriteLine("---------------------");
            Hero.ShowStats();
            Hero.ShowInventory();
            Console.WriteLine("");
            Console.ReadKey();
        }

        public void CreateMonster(string name, int str, int def, int hp, int prize, MonsterType type)
        {
            Monster newMon = new Monster(name, str, def, hp, prize, type);

            if (type == MonsterType.boss) this.Bosses.Add(newMon);
            else this.Monsters.Add(newMon);
        }

        public void AddToMonsters()
        {
            CreateMonster("rabbit", 0, 0, 30, 40, MonsterType.docile);
            CreateMonster("skeleton", 9, 0, 50, 50, MonsterType.docile);
            CreateMonster("zombie", 15, 0, 80, 100, MonsterType.docile);
            CreateMonster("orcs", 15, 5, 100, 150, MonsterType.aggressive);
            CreateMonster("warlock", 23, 4, 150, 200, MonsterType.aggressive);
            CreateMonster("demon", 28, 12, 100, 350, MonsterType.aggressive);
            CreateMonster("Minotaur", 51, 39, 100, 1000, MonsterType.boss);
            CreateMonster("Doppleganger", 61, 43, 100, 2000, MonsterType.boss);
        }

        public Monster ChooseMonster(List<Monster> genericOrBoss)
        {
            Random rand = new Random();
            int index = rand.Next(0, genericOrBoss.Count);
            return genericOrBoss[index];
        }

        public Hero CreateHero(string name, int str, int def, int hp, Game game)
        {
            this.Hero = new Hero(name, str, def, hp, game);
            return Hero;
        }

        public string ChooseNext(HashSet<string> choices)
        {
            Console.Write("Choose by number: ");
            string answer = Console.ReadLine();

            if (choices.Contains(answer)) return answer;

            return null;
        }

        public void MainMenu()
        {
            HashSet<string> flowNum = new HashSet<string> { "1", "2", "3", "4", "5" };
            Console.WriteLine(" ========== ");
            Console.WriteLine("What would you like to do?");
            Console.WriteLine("1 = show current status  /  2 = show inventory again  /  3 = equip weapon  /  4 = equip armor  /  5 = everything's done");
            string choice = ChooseNext(flowNum);

            while (!flowNum.Contains(choice))
            {
                Console.WriteLine("Choose the right number and press ENTER.");
                Console.WriteLine("");
                choice = ChooseNext(flowNum);
            }

            while (choice != "5" && flowNum.Contains(choice))
            {
                switch (choice)
                {
                    case "1":
                        Hero.ShowStats();
                        break;
                    case "2":
                        Hero.ShowInventory();
                        break;
                    case "3":
                        Hero.EquipWeapon();
                        break;
                    case "4":
                        Hero.EquipArmor();
                        break;
                }

                Console.WriteLine("");
                Console.WriteLine(" ========== ");
                Console.WriteLine("What would you like to do?");
                Console.WriteLine("1 = show current status  /  2 = show inventory again  /  3 = equip weapon  /  4 = equip armor  /  5 = everything's done");
                choice = ChooseNext(flowNum);
            }

            if (choice == "5")
            {
                if (Hero.EquippedWeapon == null || Hero.EquippedArmor == null)
                {
                    if (Hero.EquippedWeapon == null && Hero.EquippedArmor == null)
                    {
                        Console.WriteLine("You're bare naked with no weapons!!");
                        Hero.EquipWeapon();
                        Hero.EquipArmor();
                    }
                    
                    if (Hero.EquippedWeapon == null) Hero.EquipWeapon();
                    
                    if (Hero.EquippedArmor == null) Hero.EquipArmor();
                }
            }

            Console.WriteLine("");
            Console.WriteLine($"{ Hero.Name } - let's fight!!");
            Console.WriteLine("---------------------");
            Console.WriteLine("");
        }

        public void Start()
        {
            Console.WriteLine("Welcome to The Infinite Challenge!");
            InitializeGame();
            bool gameOn = true;

            while (gameOn)
            {
                MainMenu();
                bool fightAgain = true;

                while (fightAgain)
                {
                    if (Hero.Coins >= 300)
                    {
                        Console.WriteLine("Would you like to fight the boss monster?");
                        Console.WriteLine("Boss (y / n)");
                        string chooseBoss = Console.ReadLine().ToLower();

                        while (chooseBoss != "y" && chooseBoss != "n")
                        {
                            Console.WriteLine("Wrong choice. Type 'y' or 'n' and press enter.");
                            Console.WriteLine("Would you like to fight the boss monster?");
                            Console.WriteLine("Boss (y / n)");
                            chooseBoss = Console.ReadLine().ToLower();
                        }

                        if (chooseBoss == "y") this.Fight = new Fight(this, Hero, ChooseMonster(Bosses));
                        else this.Fight = new Fight(this, Hero, ChooseMonster(Monsters));
                    }
                    else this.Fight = new Fight(this, Hero, ChooseMonster(Monsters));

                    Fight.InFight();
                    
                    if (Hero.Coins == 0) Console.WriteLine("No more coins. Without equipments, you can kill the rabbit and skeleton.");

                    Console.WriteLine("Continue the hunt?");
                    Console.Write("Yes or No? (y / n): ");
                    string answer = Console.ReadLine().ToLower();

                    while (answer != "y" && answer != "n")
                    {
                        Console.WriteLine("Press either the 'y' or 'n' key to choose your answer.");
                        Console.WriteLine("Continue the hunt? If you stop hunting, you're quitting the game.");
                        Console.Write("Yes or No? (y / n): ");
                        answer = Console.ReadLine().ToLower();
                    }

                    if (answer == "y")
                    {
                        Fight.HealthRefill();
                        MainMenu();
                    }
                    else fightAgain = false;
                }

                gameOn = false;
                Console.WriteLine("");
                Console.WriteLine($"{ Hero.Name }, your battle is over.");
                Console.WriteLine($"Wins: { Hero.Wins }  |   Losts: { Hero.Losts }");
                Console.WriteLine($"You've completed this hunting streak with { ((Hero.Wins / (Hero.Wins + Hero.Losts)) * 100).ToString("0.00") }% Winning rate.");
                Console.WriteLine($"Good Game. Bye { Hero.Name }!!");
            }
        }
    }
}
