using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game
{
    public partial class Form1 : Form
    {
        private TicTacToeGame game;
        private List<Button> gameButtons;

        public Form1()
        {
            InitializeComponent();
            this.Height = 700;
            this.Width = 900;

            InitializeGame();
        }

        private void InitializeGame()
        {
            gameButtons = new List<Button> { button1, button2, button3, button4, button5, button6, button7, button8, button9 };
            game = new TicTacToeGame();

            UpdateUI();
        }

        private void PlayerMove(object sender, EventArgs e)
        {
            var button = (Button)sender;
            int index = gameButtons.IndexOf(button);

            if (index >= 0 && game.MakeMove(index))
            {
                UpdateBoard();
                CheckGameStatus();
            }
        }

        private void UpdateBoard()
        {
            for (int i = 0; i < gameButtons.Count; i++)
            {
                gameButtons[i].Text = game.Board[i].ToString();
                gameButtons[i].Enabled = !game.Board[i].HasValue;
            }
        }

        private void CheckGameStatus()
        {
            var status = game.CheckGameStatus();

            if (status.IsGameOver)
            {
                ShowGameResult(status);
                game.Reset();
                UpdateBoard();
            }

            UpdateUI();
        }

        private void ShowGameResult(GameStatus status)
        {
            string message = status.Winner.HasValue
                ? $"Игрок {status.Winner} победил! :)"
                : "Ничья! Игра начнется заново :)";

            MessageBox.Show(message);

            if (status.Winner.HasValue)
            {
                if (status.Winner == Player.X)
                {
                    label5.Text = (int.Parse(label5.Text) + 1).ToString();
                }
                else
                {
                    label6.Text = (int.Parse(label6.Text) + 1).ToString();
                }
            }
        }

        private void UpdateUI()
        {
            label1.Text = $"Сейчас ходит игрок {game.CurrentPlayer}";
        }

        private void RestartGame(object sender, EventArgs e) // сброс
        {
            game.Reset();
            label5.Text = "0";
            label6.Text = "0";
            UpdateBoard();
            UpdateUI();
        }

        private void ExitGame(object sender, EventArgs e) // выход
        {
            this.Close();
        }
    }

    public enum Player
    {
        X,
        O
    }

    public class GameStatus
    {
        public bool IsGameOver { get; set; }
        public Player? Winner { get; set; }
    }

    public class TicTacToeGame
    {
        public Player CurrentPlayer { get; private set; }
        public Player?[] Board { get; private set; }

        private static readonly int[][] WinningCombinations = new[]
        {
            new[] {0, 1, 2}, //горизонтали
            new[] {3, 4, 5},
            new[] {6, 7, 8},
            new[] {0, 3, 6}, //вертикали
            new[] {1, 4, 7},
            new[] {2, 5, 8},
            new[] {0, 4, 8}, //диагонали
            new[] {2, 4, 6}
        };

        public TicTacToeGame()
        {
            Board = new Player?[9];
            CurrentPlayer = Player.X;
        }

        public bool MakeMove(int position)
        {
            if (position < 0 || position >= Board.Length || Board[position].HasValue)
                return false;

            Board[position] = CurrentPlayer;
            CurrentPlayer = CurrentPlayer == Player.X ? Player.O : Player.X;
            return true;
        }

        public GameStatus CheckGameStatus()
        {
            var status = new GameStatus();

            // Проверка победных комбинаций
            foreach (var combination in WinningCombinations)
            {
                if (combination.All(i => Board[i] == Player.X))
                {
                    status.IsGameOver = true;
                    status.Winner = Player.X;
                    return status;
                }

                if (combination.All(i => Board[i] == Player.O))
                {
                    status.IsGameOver = true;
                    status.Winner = Player.O;
                    return status;
                }
            }

            // Проверка на ничью
            if (Board.All(cell => cell.HasValue))
            {
                status.IsGameOver = true;
                status.Winner = null;
                return status;
            }

            status.IsGameOver = false;
            return status;
        }

        public void Reset()
        {
            Board = new Player?[9];
            CurrentPlayer = Player.X;
        }
    }
}