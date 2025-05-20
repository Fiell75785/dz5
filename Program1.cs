using System;
using System.Collections.Generic;
using System.Linq;

namespace CardGameInteractive
{ 
    public class Karta
    {
        public string Mast { get; private set; }
        public string Value { get; private set; }

        public Karta(string mast, string value)
        {
            Mast = mast;
            Value = value;
        }

        public override string ToString()
        {
            return $"{Value} {Mast}";
        }

        public int GetRank()
        {
            switch (Value)
            {
                case "6": return 6;
                case "7": return 7;
                case "8": return 8;
                case "9": return 9;
                case "10": return 10;
                case "Валет": return 11;
                case "Дама": return 12;
                case "Король": return 13;
                case "Туз": return 14;
                default: return 0;
            }
        }
    }
    public class Player
    {
        public string Name { get; private set; }
        private Queue<Karta> Cards;

        public Player(string name)
        {
            Name = name;
            Cards = new Queue<Karta>();
        }

        public void AddCards(IEnumerable<Karta> newCards)
        {
            foreach (var card in newCards)
            {
                Cards.Enqueue(card);
            }
        }

        public Karta PlayCard()
        {
            if (Cards.Count > 0)
                return Cards.Dequeue();
            return null;
        }

        public int CardCount()
        {
            return Cards.Count;
        }

        public void ShowCards()
        {
            Console.WriteLine($"{Name} имеет {Cards.Count} карт:");
            int index = 1;
            foreach (var card in Cards)
            {
                Console.WriteLine($"{index}. {card}");
                index++;
            }
        }

        public void SetCards(IEnumerable<Karta> newCards)
        {
            Cards = new Queue<Karta>(newCards);
        }
        public Karta ChooseCard()
        {
            if (Cards.Count == 0) return null;
            ShowCards();
            int choice = 0;
            while (true)
            {
                Console.Write($"Выберите номер карты (1-{Cards.Count}): ");
                string input = Console.ReadLine();
                if (int.TryParse(input, out choice) && choice >= 1 && choice <= Cards.Count)
                {
                    break;
                }
                Console.WriteLine("Некорректный ввод. Попробуйте снова.");
            }
            var tempList = Cards.ToList();
            var selectedCard = tempList[choice - 1];
            tempList.RemoveAt(choice - 1);
            SetCards(tempList);
            return selectedCard;
        }
    } 
    public class Game
    {
        private List<Player> players;
        private List<Karta> deck;
        private Random rand;

        public Game(List<string> playerNames)
        {
            rand = new Random();
            players = new List<Player>();
            foreach (var name in playerNames)
            {
                players.Add(new Player(name));
            }
            InitializeDeck();
            ShuffleDeck();
            DealCards();
        }

        private void InitializeDeck()
        {
            string[] masts = { "Пики", "Червы", "Бубны", "Трефы" };
            string[] values = { "6", "7", "8", "9", "10", "Валет", "Дама", "Король", "Туз" };

            deck = new List<Karta>();
            foreach (var mast in masts)
            {
                foreach (var value in values)
                {
                    deck.Add(new Karta(mast, value));
                }
            }
        }

        private void ShuffleDeck()
        {
            deck = deck.OrderBy(x => rand.Next()).ToList();
        }

        private void DealCards()
        {
            int playerCount = players.Count;
            for (int i = 0; i < deck.Count; i++)
            {
                players[i % playerCount].AddCards(new List<Karta> { deck[i] });
            }
        }

        public void Start()
        {
            Console.WriteLine("Начинаем игру! У вас два игрока.");

            while (players.All(p => p.CardCount() > 0))
            {
                Console.WriteLine("\n------------------------------");
                Console.WriteLine($"Ход раунда. У каждого по {players[0].CardCount()} и {players[1].CardCount()} карт.");
                foreach (var player in players)
                {
                    player.ShowCards();
                }
                var playedCards = new List<(Player player, Karta card)>();
                for (int i = 0; i < players.Count; i++)
                {
                    var currentPlayer = players[i];
                    Console.WriteLine($"\n{currentPlayer.Name}, выберите карту для игры:");
                    var selectedCard = currentPlayer.ChooseCard();
                    Console.WriteLine($"{currentPlayer.Name} выбрал {selectedCard}");
                    playedCards.Add((currentPlayer, selectedCard));
                }
                var maxRank = playedCards.Max(pc => pc.card.GetRank());
                var winners = playedCards.Where(pc => pc.card.GetRank() == maxRank).ToList();

                if (winners.Count == 1)
                {
                    var winner = winners[0].player;
                    Console.WriteLine($"\n{winner.Name} выигрывает раунд и забирает карты");
                    winner.AddCards(playedCards.Select(pc => pc.card));
                }
                else
                {
                    var winner = winners[0].player;
                    Console.WriteLine($"\nНичья! {winner.Name} забирает карты");
                    winner.AddCards(playedCards.Select(pc => pc.card));
                }
            }
            var overallWinner = players.OrderByDescending(p => p.CardCount()).First();
            Console.WriteLine($"\nИгра окончена! Победитель: {overallWinner.Name}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Добро пожаловать в карточную игру!");

            var playerNames = new List<string> { "Игрок 1", "Игрок 2" };
            var game = new Game(playerNames);
            game.Start();

            Console.WriteLine("Спасибо за игру!");
        }
    }
}
