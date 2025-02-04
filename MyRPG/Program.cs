using MyRPG;
using System.ComponentModel;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace MyRPG
{
    public interface CharacterInteface
    {
        int Level { get; }
        string Name { get; }
        int Attack { get; }
        int Armor { get; }
        int Health { get; }
        bool IsDead { get; }
        void TakeDamage(int damage);
    }
    public class Warrior : CharacterInteface, Item
    {
        public int Level { get; set; }
        public string Name { get; }
        public int Attack { get; set; }
        public int Armor { get; }
        public int Health { get; set; }
        public int Gold { get; set; }
        public bool IsDead => Health <= 0;

        public Warrior(string name)
        {
            Name = name;
            Health = 100;
            Attack = 10;
            Gold = 0;
        }
        public void TakeDamage(int damage)
        {
            Health -= damage;
            Console.WriteLine("캐릭터가" + damage + " 만큼 피해를 입었습니다!" + "\n현재체력 : " + Health);
            if (IsDead)
            {
                Console.WriteLine("캐릭터가 사망했습니다. 여행을 마칩니다.");
            }
        }
    }
    public class Monster : CharacterInteface
    {
        public int Level { get; set; }
        public string Name { get; }
        public int Attack => new Random().Next(10, 20);
        public int Armor { get; set; }
        public int Health { get; set; }
        public bool IsDead => Health <= 0;
        public Monster(string _name, int _health)
        {
            Name = _name;
            Health = _health;
        }
        public void TakeDamage(int damage)
        {
            Health -= damage;
            Console.WriteLine(Name + "이" + damage + "만큼 공격 받았습니다.");
            Console.WriteLine("현재 체력 : " + Health);

            if (IsDead)
            {

                Console.WriteLine(Name + "가 사망했습니다.");
            }
        }
    }

    public class Goblin : Monster
    {
        public Goblin(string name) : base(name, 50) { } // 체력 50
    }
    public class Dragon : Monster
    {
        public Dragon(string name) : base(name, 50) { }
    }
}
public interface Item
{
    void Use(Warrior warrior)
    {

    }
}
public class HealthPotion : Item
{
    void Use(Warrior warrior)
    {
        warrior.Health += 10;
    }
}
public class StrengthPotion : Item
{
    void Use(Warrior warrior)
    {
        warrior.Attack += 10;
    }
}
public class Stage
{
    public CharacterInteface _warrior;
    public CharacterInteface _monster;
    public List<Item> _item;

    public Item _heathpotion;
    public Item _strengthPotion;
    public Stage(CharacterInteface _REF_warrior, CharacterInteface _REF_monster, List<Item> _REF_item)
    {
        this._warrior = _REF_warrior;
        this._monster = _REF_monster;
        this._item = _REF_item;
    }
    public void Start()
    {
        Console.WriteLine($"스테이지 시작! 플레이어 정보: 체력({_warrior.Health}), 공격력({_warrior.Attack})");
        Console.WriteLine($"몬스터 정보: 이름({_monster.Name}), 체력({_monster.Health}), 공격력({_monster.Attack})");
        Console.WriteLine("----------------------------------------------------");
        Console.WriteLine("스테이지를 시작합니다.");
        while (!_warrior.IsDead && !_monster.IsDead)
        {
            Console.WriteLine(_warrior.Name + " 의 턴!");
            PlayerTurn();
            Console.WriteLine();

            Thread.Sleep(1000);

            if (_monster.IsDead)
            {
                StageClear();
                Console.WriteLine($"{_monster.Name}을 처치하였습니다!");
                break;  // 몬스터가 죽었다면 턴 종료
            }

            // 몬스터의 턴
            Console.WriteLine($"{_monster.Name}의 턴!");
            _warrior.TakeDamage(_monster.Attack);
            Console.WriteLine();
            Thread.Sleep(1000);  // 턴 사이에 1초 대기
        }
    }

    void PlayerTurn()
    {
        int _warriorBHV = int.Parse(Console.ReadLine());
        switch (_warriorBHV)
        {
            case 1:
                Console.WriteLine("공격을 선택하셨습니다.");
                _monster.TakeDamage(_warrior.Attack);
                break;

            case 2:
                Console.WriteLine("체력 포션을 마십니다.");

                break;

            case 3:
                Console.WriteLine("강화 포션을 마십니다.");

                break;

            default:
                Console.WriteLine("나쁜 아이는 상범 아저씨가 마을로 데려가용");
                break;
        }
    }

    void MonsterBHV()
    {
        int _warriorBHV = int.Parse(Console.ReadLine());
        switch (_warriorBHV)
        {
            case 1:
                Console.WriteLine("공격을 선택하셨습니다.");
                _monster.TakeDamage(_warrior.Attack);
                break;

            case 2:
                Console.WriteLine("체력 포션을 마십니다.");

                break;
        }
    }
    void StageClear()
    {
        _warrior.Level++;
        Console.WriteLine("스테이지를 클리어 하였습니다 \n마을로 돌아갑니다.");
        Console.WriteLine("1번 : 1000골드\n2번 : 포션 10개 ");
        int Choice = int.Parse(Console.ReadLine());
        switch (Choice)
        {
            case 1:
                //_warrior.Gold; 
                break;

            case 2:

                break;

            default:
                break;
        }
    }
}
internal class Program
{
    static void Main(string[] args)
    {
        Warrior _warriror = new Warrior("플레이어");
        Goblin _goblin = new Goblin("고블린");
        Dragon _dragon = new Dragon("드래곤");

        List<Item> stage1Rewards = new List<Item> { new HealthPotion(), new StrengthPotion() };
        List<Item> stage2Rewards = new List<Item> { new StrengthPotion(), new HealthPotion() };

        Stage _stage = new Stage(_warriror, _goblin, stage1Rewards);

        EnterTown();

        void EnterTown()
        {
            Console.WriteLine("===================================\n스파르타 마을에 오신 여러분 환영합니다.\r\n이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\r\n\r\n1. 상태 보기\r\n2. 인벤토리\r\n3. 상점\r\n\r\n원하시는 행동을 입력해주세요.\r\n>>");
            GoTo(int.Parse(Console.ReadLine()));
        }

        void GoTo(int _GoToIndex)
        {
            switch (_GoToIndex)
            {
                case 1:
                    Console.WriteLine($"\n===============================\n상태 보기\n캐릭터의 정보가 표시됩니다.\nLV. {_warriror.Level}\n{_warriror.Name}( 전사 )\n공격력 : {_warriror.Attack}\n방어력 : {_warriror.Armor}\n체력 : {_warriror.Health}\n골드 : {_warriror.Gold}\n===============================\n0 : 나가기\n\n원하시는 행동을 입력해주세요\n>>");
                    Console.ReadLine();
                    EnterTown();
                    break;
                case 2:
                    Console.WriteLine("인벤토리\r\n보유 중인 아이템을 관리할 수 있습니다.\r\n\r\n[아이템 목록]\r\n\r\n1. 장착 관리\r\n0. 나가기\r\n\r\n원하시는 행동을 입력해주세요.\r\n>>");
                    Console.ReadLine();
                    EnterTown();
                    break;
                case 3:
                    Console.WriteLine("상점\r\n필요한 아이템을 얻을 수 있는 상점입니다.\r\n\r\n[보유 골드]\r\n800 G\r\n\r\n[아이템 목록]\r\n- 수련자 갑옷    | 방어력 +5  | 수련에 도움을 주는 갑옷입니다.             |  1000 G\r\n- 무쇠갑옷      | 방어력 +9  | 무쇠로 만들어져 튼튼한 갑옷입니다.           |  구매완료\r\n- 스파르타의 갑옷 | 방어력 +15 | 스파르타의 전사들이 사용했다는 전설의 갑옷입니다.|  3500 G\r\n- 낡은 검      | 공격력 +2  | 쉽게 볼 수 있는 낡은 검 입니다.            |  600 G\r\n- 청동 도끼     | 공격력 +5  |  어디선가 사용됐던거 같은 도끼입니다.        |  1500 G\r\n- 스파르타의 창  | 공격력 +7  | 스파르타의 전사들이 사용했다는 전설의 창입니다. |  구매완료\r\n\r\n1. 아이템 구매\r\n0. 나가기\r\n\r\n원하시는 행동을 입력");
                    Console.ReadLine();
                    EnterTown();
                    break;

                default:
                    Console.WriteLine("틀린 선택입니다.");
                    Console.WriteLine("\n나쁜아이는 상범 아저씨가 던전으로 데려갑니다.");
                    EnterDungeon();
                    break;
            }
            void EnterDungeon()
            {
                _stage.Start();
                EnterTown();
            }
        }
    }
}