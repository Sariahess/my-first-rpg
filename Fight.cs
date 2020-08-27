using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sariah_assign2_RPG_Game
{
    class Fight
    {
        public Game Game { get; set; }
        public Hero Hero { get; set; }
        public Monster Monster { get; set; }

        public Fight(Game game, Hero hero, Monster monster)
        {
            this.Game = game;
            this.Hero = hero;
            this.Monster = monster;
        }

        public void EachHit(Being being, Monster monster)
        {
            if (being != Hero)
            {
                int heroDamage = monster.Strength - Hero.Defense;

                if (heroDamage > 0)
                {
                    Hero.CurrentHealth -= heroDamage;
                    Console.WriteLine($"{ monster.Name } hit { Hero.Name } with damage { heroDamage }!");
                }
                else Console.WriteLine($"{ Hero.Name } got no damage from { monster.Name }.");
            }
            else
            {
                int monsterDamage = Hero.Strength - monster.Defense;

                if (monsterDamage > 0)
                {
                    monster.CurrentHealth -= monsterDamage;
                    Console.WriteLine($"{ Hero.Name } hit { monster.Name } with damage { monsterDamage }!");
                }
                else Console.WriteLine($"{ monster.Name } got no damage from { Hero.Name }.");
            }

            Console.WriteLine(" ---------------------------- ");
        }

        public void HealAgainstBoss()
        {
            Console.WriteLine("Your current health is below 30 HP!");
            Console.WriteLine("Would you like to heal yourself to full HP? Healing requires 100 coins.");
            Console.Write("Yes or No? (y / n): ");
            string answer = Console.ReadLine().ToLower();
            Console.WriteLine("");

            while (answer != "y" && answer != "n")
            {
                Console.WriteLine("Press either the 'y' or 'n' key to choose your answer.");
                Console.Write("Healing yourself: Yes or No? (y / n) ");
                answer = Console.ReadLine().ToLower();
            }

            if (answer == "y")
            {
                Hero.CurrentHealth = 100;
                Hero.Coins -= 100;
            }
        }

        public void InFight()
        {
            int turn = 1;
            Console.WriteLine($"You've been matched with { Monster.Name }! HP: { Monster.OriginalHealth }");
            Console.WriteLine($"{ Monster.Name } has { Monster.Defense } defense capability, and");
            Console.WriteLine($"{ Hero.Name } has { Hero.Strength } attack power.");
            Console.WriteLine("If your attack power is equal or less than your target's defense,");
            Console.WriteLine("Then your damage will be 0.");
            Console.WriteLine($"Defeat { Monster.Name } and get { Monster.Prize } coins!!");
            Console.WriteLine("");
            Console.WriteLine("****** Begin Fight ******");

            while (Hero.CurrentHealth > 0 && Monster.CurrentHealth > 0)
            {
                if (Monster.Type == MonsterType.docile)
                {
                    if (turn % 2 != 0) EachHit(Hero, Monster);
                    else EachHit(Monster, Monster);
                }
                else
                {
                    if (turn % 2 != 0) EachHit(Monster, Monster);
                    else EachHit(Hero, Monster);
                }

                Console.WriteLine($"{ Hero.Name } HP: { (Hero.CurrentHealth > 0 ? Hero.CurrentHealth : 0) }   |   { Monster.Name } HP: { (Monster.CurrentHealth > 0 ? Monster.CurrentHealth : 0) }");
                Console.WriteLine("---------------------");
                Console.WriteLine("<press any key to continue>");
                Console.WriteLine("");

                if (Monster.Type == MonsterType.boss
                    && Hero.CurrentHealth < 30
                    && Hero.Coins >= 100
                    && Monster.CurrentHealth > 0) HealAgainstBoss();

                turn++;
                Console.ReadKey();
            }

            if (Hero.CurrentHealth <= 0)
            {
                Hero.Losts++;
                Console.WriteLine($"{ Hero.Name } lost battle to { Monster.Name }. You've earned 0 coins.");
            }
            else if (Monster.CurrentHealth <= 0)
            {
                Hero.Wins++;
                Hero.Coins += Monster.Prize;
                Console.WriteLine($"{ Hero.Name } won against { Monster.Name }! You've earned { Monster.Prize } coins.");
            }

            Console.WriteLine("");
            Console.WriteLine($"Current Status of { Hero.Name } is as follows: ");
            Console.WriteLine(" ------------ ");
            Hero.ShowStats();
            Console.WriteLine(" ------------ ");
            Console.WriteLine("");
            HealthRefill();
        }

        public void HealthRefill()
        {
            Hero.CurrentHealth = 100;
            Hero.EquippedArmor = null;
            Hero.EquippedWeapon = null;
            Hero.Strength = 0;
            Hero.Defense = 0;
            Monster.CurrentHealth = Monster.OriginalHealth;
        }
    }
}
