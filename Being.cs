using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sariah_assign2_RPG_Game
{
    class Being
    {
        public string Name { get; set; }
        public int Strength { get; set; }
        public int Defense { get; set; }
        public int OriginalHealth { get; set; }
        public int CurrentHealth { get; set; }

        public Being() { }

        public Being(string name, int str, int def, int originHp)
        {
            this.Name = name;
            this.Strength = str;
            this.Defense = def;
            this.OriginalHealth = originHp;
            this.CurrentHealth = originHp;
        }
    }

    class Hero : Being
    {
        public Game Game { get; set; }
        public int Coins = 40;
        public double Wins = 0;
        public double Losts = 0;
        public Weapon EquippedWeapon { get; set; }
        public Armor EquippedArmor { get; set; }
        public List<Weapon> WeaponsBag { get; set; }
        public List<Armor> ArmorsBag { get; set; }

        public Hero(string name, int str, int def, int hp, Game game) : base(name, str, def, hp)
        {
            this.Game = game;
            this.Name = name;
            this.OriginalHealth = hp;
            this.CurrentHealth = 100;
            this.Strength = 0;
            this.Defense = 0;
            this.WeaponsBag = new List<Weapon>();
            this.ArmorsBag = new List<Armor>();
            AddToInventory();
        }

        public void AddWeapons(string name, int power, int price)
        {
            Weapon weapon = new Weapon();
            weapon.ItemName = name;
            weapon.Power = power;
            weapon.Price = price;
            WeaponsBag.Add(weapon);
        }

        public void AddArmors(string name, int power, int price)
        {
            Armor armor = new Armor();
            armor.ItemName = name;
            armor.Power = power;
            armor.Price = price;
            ArmorsBag.Add(armor);
        }

        public void AddToInventory()
        {
            // weapons
            AddWeapons("none", 5, 0);
            AddWeapons("dagger", 10, 10);
            AddWeapons("wand", 20, 20);
            AddWeapons("sphere", 30, 30);
            // armors
            AddArmors("none", 0, 0);
            AddArmors("wooden armor", 10, 10);
            AddArmors("iron mail", 20, 20);
            AddArmors("titanium plate", 30, 30);
        }

        public void ShowStats()
        {
            Console.WriteLine($"Health :  [{ (CurrentHealth > 0 ? CurrentHealth : 0) }]");
            Console.WriteLine($"Strength :  [{ Strength }]");
            Console.WriteLine($"Defense :  [{ Defense }]");
            Console.WriteLine($"Wealth :  { Coins } coins");
            Console.WriteLine($"Total Wins :  { Wins }");
            Console.WriteLine($"Total Losts :  { Losts }");
        }

        public void ShowInventory()
        {
            Console.WriteLine("******weapons:");

            for (int i = 0; i < WeaponsBag.Count; i++)
            {
                Console.WriteLine($"  #{ i } = { WeaponsBag[i].ItemName }  [ damage: { WeaponsBag[i].Power } ]  [ price: { WeaponsBag[i].Price } coins ]");
            }

            Console.WriteLine("******armors:");

            for (int i = 0; i < ArmorsBag.Count; i++)
            {
                Console.WriteLine($"  #{ i } = { ArmorsBag[i].ItemName }  [ shield: { ArmorsBag[i].Power } ]  [ price: { ArmorsBag[i].Price } coins ]");
            }
        }

        public void EquipWeapon()
        {
            if (this.EquippedWeapon == null)
            {
                Console.WriteLine("------ equip weapon : Increase STRENGTH!! ------");
                Console.WriteLine("Enter the weapon's number.");
                HashSet<string> itemNums = new HashSet<string>();

                for (int i = 0; i < WeaponsBag.Count; i++) itemNums.Add(i.ToString());

                string weapon = Game.ChooseNext(itemNums);

                while (!itemNums.Contains(weapon))
                {
                    Console.WriteLine("Wrong number. Choose again.");
                    weapon = Game.ChooseNext(itemNums);
                }

                if (Coins >= WeaponsBag[int.Parse(weapon)].Price)
                {
                    this.EquippedWeapon = WeaponsBag[int.Parse(weapon)];
                    this.Strength = EquippedWeapon.Power;
                    this.Coins -= EquippedWeapon.Price;
                }
                else
                {
                    ShowInventory();
                    Console.WriteLine("");
                    Console.WriteLine($"Not enough coin. You have { Coins } coins to get your weapon.");
                    EquipWeapon();
                }
            }
            else Console.WriteLine("You already have a weapon.");
        }

        public void EquipArmor()
        {
            if (this.EquippedArmor == null)
            {
                Console.WriteLine("------ equip armor : Increase DEFENSE!! ------");
                Console.WriteLine("Enter the armor's number.");
                HashSet<string> itemNums = new HashSet<string>();

                for (int i = 0; i < ArmorsBag.Count; i++) itemNums.Add(i.ToString());

                string armor = Game.ChooseNext(itemNums);

                while (!itemNums.Contains(armor))
                {
                    Console.WriteLine("Wrong number. Choose again.");
                    armor = Game.ChooseNext(itemNums);
                }

                if (Coins >= ArmorsBag[int.Parse(armor)].Price)
                {
                    this.EquippedArmor = ArmorsBag[int.Parse(armor)];
                    this.Defense = EquippedArmor.Power;
                    this.Coins -= EquippedArmor.Price;
                }
                else
                {
                    ShowInventory();
                    Console.WriteLine("");
                    Console.WriteLine($"Not enough coin. You have { Coins } coins to get your armor.");
                    EquipArmor();
                }
            }
            else Console.WriteLine("You already have an armor.");
        }
    }

    enum MonsterType
    {
        aggressive,
        docile,
        boss
    }

    class Monster : Being
    {
        public int Prize;
        public MonsterType Type { get; set; }

        public Monster(string name, int str, int def, int hp, int prize, MonsterType type) : base(name, str, def, hp)
        {
            this.Name = name;
            this.Strength = str;
            this.Defense = def;
            this.OriginalHealth = hp;
            this.CurrentHealth = hp;
            this.Prize = prize;
            this.Type = type;
        }
    }
}
