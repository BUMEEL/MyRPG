﻿using MyRPG;
using System.ComponentModel;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace MyRPG
{
    public interface CharacterInteface //캐릭터 인터페이스
    {
        int Level { get; set; }
        string Name { get; }
        int Attack { get; set; }
        int Armor { get; }
        int Health { get; set; }
        bool IsDead { get; }
        int Gold { get; set; }

        void TakeDamage(int damage);
    }
    public class Warrior : CharacterInteface //워리어 클래스, 캐릭터 인터페이스 상속
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
            Level = 1;
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
    public class Monster : CharacterInteface //몬스터 클래스, 캐릭터 인터페이스 상속
    {
        public int Level { get; set; }
        public string Name { get; }
        public int Attack
        {
            get => new Random().Next(10, 20);
            set  => new Random().Next(10, 20);
        }

        public int Armor { get; set; }
        public int Health { get; set; }
        public int Gold { get; set; }
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
        public void use()
        {

        }
    }

    public class Goblin : Monster //고블린 클래스, 몬스터 클래스 상속
    {
        public Goblin(string name) : base(name, 50) { } 
    }
    public class Dragon : Monster //드래곤 클래스, 몬스터 클래스 상속
    {
        public Dragon(string name) : base(name, 50) { }
    }
}
public interface Item //아이템 인터페이스
{
    void Use(CharacterInteface warrior);
}
public class HealthPotion : Item //헬스 포션 클래스, 아이템 인터페이스 상속
{
    public void Use(CharacterInteface warrior)
    {
        warrior.Health += 10;
    }
}
public class StrengthPotion : Item// 스트렝스 포션 클래스, 아이템 인터페이스 상속
{
    public  void Use(CharacterInteface warrior)
    {
        warrior.Attack += 10;
    }
}
public class Stage // 스테이지 클래스
{
    public CharacterInteface _warrior;
    public CharacterInteface _monster;
    public List<Item> _item;

    public Item _heathpotion;
    public Item _strengthPotion;
    public Stage(CharacterInteface _REF_warrior, CharacterInteface _REF_monster, List<Item> _REF_item) //스테이지 인수 설정
    {
        this._warrior = _REF_warrior;
        this._monster = _REF_monster;
        this._item = _REF_item;
    }
    public void Start() //스테이지 시작
    {
        Console.WriteLine($"스테이지 시작! 플레이어 정보: 체력({_warrior.Health}), 공격력({_warrior.Attack})");
        Console.WriteLine($"몬스터 정보: 이름({_monster.Name}), 체력({_monster.Health}), 공격력({_monster.Attack})");
        Console.WriteLine("----------------------------------------------------");
        Console.WriteLine("스테이지를 시작합니다.");

        while (!_warrior.IsDead && !_monster.IsDead) //둘 중 하나 죽을때 까지
        {
            Console.WriteLine(_warrior.Name + " 의 턴!"); //플레이어 선턴
            PlayerTurn();
            Console.WriteLine();

            Thread.Sleep(1000);

            if (_monster.IsDead) //몬스터 죽었으면
            {
                StageClear(); //스테이지 클리어 메서드
                Console.WriteLine($"{_monster.Name}을 처치하였습니다!");
                break; //메서드 탈출, 사후경직 방지, else로 처리할까 했는데 간결하게 쓰고싶어서 그냥 뒀습니다유
            }
            Console.WriteLine($"{_monster.Name}의 턴!");
            _warrior.TakeDamage(_monster.Attack);
            Console.WriteLine();
            Thread.Sleep(1000);  // 턴 사이에 1초 대기 (추가 요구사항)
        }
    }

    void PlayerTurn() //플레이어 턴일 시 행동 선택 메서드
    {
        Console.WriteLine("\n행동을 선택해주세요.\n1번 : 공격\n2번 : 체력 포션\n3번 : 강화 포션");
        int _warriorBHV = int.Parse(Console.ReadLine()); //입력 받아옴
        switch (_warriorBHV) //받은 입력으로 행동 인덱스 스위치
        {
            case 1:
                Console.WriteLine("공격을 선택하셨습니다.");
                _monster.TakeDamage(_warrior.Attack);
                break;

            case 2:// <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<이거 이상합니다
                Console.WriteLine("체력 포션을 마십니다.");
                Console.Write($"체력이 {_warrior.Health}에서");
                _heathpotion.Use(_warrior); 
                Console.Write($"{_warrior.Health}로 증가합니다.");

                break;

            case 3:// <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<이거 이상합니다
                Console.WriteLine("강화 포션을 마십니다.");

                break;

            default:
                Console.WriteLine("나쁜 아이는 상범 아저씨가 떼찌해용");
                break;
        }
    }

    void MonsterBHV() //몬스터 행동 (랜덤성, 구현예정)
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
    void StageClear() // 스테이지 클리어 시 레벨, 보상획득
    {
        Console.Write($"플레이어의 레벨이\n{_warrior.Level}에서");
        _warrior.Level++;
        Console.WriteLine($"{_warrior.Level}로 증가합니다.");
        Console.WriteLine("스테이지를 클리어 하였습니다 \n마을로 돌아갑니다.");
        Console.WriteLine("1번 : 1000골드\n2번 : 포션 10개 ");
        int Choice = int.Parse(Console.ReadLine());

        switch (Choice) //보상 선택
        {
            case 1:
                _warrior.Gold += 100;
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
        List<Item> stage3Rewards = new List<Item> { new StrengthPotion(), new HealthPotion() };
        List<Item> stage4Rewards = new List<Item> { new StrengthPotion(), new HealthPotion() };

        Stage _stage = new Stage(_warriror, _goblin, stage1Rewards);

        EnterTown();

        void EnterTown()
        {
            Console.WriteLine("===================================\n스파르타 마을에 오신 여러분 환영합니다.\r\n이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\r\n\r\n1. 상태 보기\r\n2. 인벤토리\r\n3. 상점\n이상한 입력을 하시면 던전으로 추방합니다.\r\n\r\n원하시는 행동을 입력해주세요.\r\n>>");
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
                    EnterDungeon(_warriror.Level);
                    break;
            }
            void EnterDungeon(int PLV)
            {
                switch (PLV)
                {
                    case 1:
                        _stage = new Stage(_warriror, _goblin, stage1Rewards);
                        break;
                    case 2:
                        _stage = new Stage(_warriror, _dragon, stage2Rewards);
                        break;
                    case 3:
                        _stage = new Stage(_warriror, _goblin, stage3Rewards);
                        break;
                    case 4:
                        _stage = new Stage(_warriror, _goblin, stage4Rewards);
                        break;

                    default:
                        break;
                }
                _stage.Start();
                EnterTown();
            }
        }
    }
}