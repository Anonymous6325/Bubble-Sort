using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace RockPaperScissorsGame
{
    // Abstract class for a Player
    public abstract class Player
    {
        public string Name { get; set; }
        public int Lives { get; set; }

        public abstract string ChooseMove();
    }

    // Human player inheriting from Player
    public class HumanPlayer : Player
    {
        public HumanPlayer(string name)
        {
            Name = name;
            Lives = 3;
        }

        public override string ChooseMove()
        {
            string[] moves = { "rock", "paper", "scissors" };
            int selectedIndex = 0;
            ConsoleKey key;

            do
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"Score: {Game.GetScore()}");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Lives: {Game.GetPlayerLives()}");
                Console.ResetColor();
                Console.WriteLine("\nUse the arrow keys to choose a move, and press Enter to select.");

                // Displaying the choices with 3D effect
                for (int i = 0; i < moves.Length; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"> {moves[i]} <");
                        Console.ResetColor();
                    }
                    else
                    {
                        // 3D effect simulation for choices
                        Console.WriteLine($"  {moves[i]}  ");
                    }
                }

                key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.UpArrow)
                {
                    selectedIndex = (selectedIndex - 1 + moves.Length) % moves.Length;
                }
                else if (key == ConsoleKey.DownArrow)
                {
                    selectedIndex = (selectedIndex + 1) % moves.Length;
                }

            } while (key != ConsoleKey.Enter);

            return moves[selectedIndex];
        }
    }

    // Computer player inheriting from Player
    public class ComputerPlayer : Player
    {
        private static readonly Random Random = new Random();
        private static readonly string[] Moves = { "rock", "paper", "scissors" };
        private int _difficultyLevel;

        public ComputerPlayer(int difficultyLevel)
        {
            Name = "Computer";
            Lives = int.MaxValue; // Computer doesn't lose lives
            _difficultyLevel = difficultyLevel;
        }

        public override string ChooseMove()
        {
            if (_difficultyLevel == 0) // Easy: Always pick Rock
            {
                return "rock";
            }
            else if (_difficultyLevel == 1) // Moderate: Random, but biased to lose
            {
                return Moves[Random.Next(Moves.Length)];
            }
            else // Hard: Random with slight bias to win
            {
                return Moves[(Random.Next(Moves.Length) + 1) % 3];
            }
        }
    }

    // Main Game Logic
    public class Game
    {
        private readonly HumanPlayer _humanPlayer;
        private readonly ComputerPlayer _computerPlayer;
        private static int _score;

        public static int GetScore() => _score;
        public static int GetPlayerLives() => _instance._humanPlayer.Lives;

        private readonly Dictionary<string, string> _winningMoves = new Dictionary<string, string>
        {
            { "rock", "scissors" },
            { "paper", "rock" },
            { "scissors", "paper" }
        };

        private static Game _instance;

        public Game(string playerName, int difficultyLevel)
        {
            _humanPlayer = new HumanPlayer(playerName);
            _computerPlayer = new ComputerPlayer(difficultyLevel);
            _score = 0;
            _instance = this;
        }

        private void DisplayAnimation(string humanMove, string computerMove)
        {
            // Simulate 3D animation here (simplified for this example)
            Console.Clear();
            Console.WriteLine("\nAnimating moves...");
            Thread.Sleep(500);

            string[] computerHand = { "rock", "paper", "scissors" };
            string[] humanHand = { "rock", "paper", "scissors" };
            // Display moves here (3D effect for hands, etc.)
        }

        // Difficulty settings
        public void DisplayDifficultySelection()
        {
            Console.Clear();
            Console.WriteLine("Select difficulty:");
            Console.WriteLine("1. Easy");
            Console.WriteLine("2. Moderate");
            Console.WriteLine("3. Hard");

            ConsoleKey key;
            do
            {
                key = Console.ReadKey(true).Key;
            } while (key != ConsoleKey.D1 && key != ConsoleKey.D2 && key != ConsoleKey.D3);

            int difficultyLevel = key == ConsoleKey.D1 ? 0 : key == ConsoleKey.D2 ? 1 : 2;
            StartGame(difficultyLevel);
        }

        private void StartGame(int difficultyLevel)
        {
            Console.Clear();
            Console.WriteLine($"Welcome to Rock, Paper, Scissors!");
            Console.WriteLine("Press any key to enter...");
            Console.ReadKey();
            // Game loop
            Play(difficultyLevel);
        }

        public void Play(int difficultyLevel)
        {
            Console.WriteLine($"Welcome, {_humanPlayer.Name}! You have 3 lives. Let's play Rock-Paper-Scissors!");

            while (_humanPlayer.Lives > 0)
            {
                string humanMove = _humanPlayer.ChooseMove();
                string computerMove = _computerPlayer.ChooseMove();

                DisplayAnimation(humanMove, computerMove);

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"Score: {GetScore()}");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Lives: {_humanPlayer.Lives}");
                Console.ResetColor();
                Console.WriteLine($"\nYou chose {humanMove}, Computer chose {computerMove}.");

                if (humanMove == computerMove)
                {
                    Console.WriteLine("It's a draw!");
                }
                else if (_winningMoves.ContainsKey(humanMove) && _winningMoves[humanMove] == computerMove)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("You win this round!");
                    Console.ResetColor();
                    _score++;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("You lose this round.");
                    Console.ResetColor();
                    _humanPlayer.Lives--;
                }

                Thread.Sleep(2000); // Brief pause before the next round
            }

            Console.Clear();
            Console.WriteLine("Game Over!");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Your final score: {_score}");
            Console.ResetColor();
        }
    }

    // Leaderboard to store player scores
    public class Leaderboard
    {
        private readonly List<(string Name, int Score)> _scores = new List<(string Name, int Score)>();

        public void AddScore(string playerName, int score)
        {
            _scores.Add((playerName, score));
        }

        public void DisplayTopScores()
        {
            Console.WriteLine("--- Leaderboard ---");
            foreach (var entry in _scores)
            {
                Console.WriteLine($"{entry.Name}: {entry.Score}");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter your name: ");
            string playerName = Console.ReadLine();

            var leaderboard = new Leaderboard();
            var game = new Game(playerName, 1); // Default moderate difficulty

            game.DisplayDifficultySelection();

            leaderboard.AddScore(playerName, Game.GetScore());
            leaderboard.DisplayTopScores();
        }
    }
}