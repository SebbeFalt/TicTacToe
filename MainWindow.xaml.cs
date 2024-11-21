using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TicTacToe
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    private bool isHumanTurn = true;
    private string[,] board = new string[3, 3]
    {
        { "", "", "" },
        { "", "", "" },
        { "", "", "" }
    };
    public MainWindow()
    {
      InitializeComponent();
    }
    private async void Square_Click(object sender, RoutedEventArgs e)
    {
      if (!isHumanTurn)
        return;

      Button clickedButton = (Button)sender;
      clickedButton.Content = "X";
      clickedButton.Foreground = new SolidColorBrush(Color.FromRgb(139, 0, 0));
      clickedButton.IsEnabled = false;

      UpdateBoard(clickedButton, "X");

      string winner = CheckWinner();

      if (winner != null)
      {
        MessageBox.Show(this, winner, "Game Status", MessageBoxButton.OK);
        ResetGame();
        return;
      }

      if (IsDraw())
      {
        MessageBox.Show(this, "It's a draw!", "Game Status", MessageBoxButton.OK);
        ResetGame();
        return;
      }

      isHumanTurn = false;
      await ComputerMove();
      isHumanTurn = true;
    }

    private async Task ComputerMove()
    {
      await Task.Delay(500);
      var availableButtons = new List<Button>
        {
            Square00, Square01, Square02,
            Square10, Square11, Square12,
            Square20, Square21, Square22
        }.Where(b => b.IsEnabled).ToList();

      if (availableButtons.Count > 0)
      {
        Random random = new Random();
        Button chosenButton = availableButtons[random.Next(availableButtons.Count)];
        chosenButton.Content = "O";
        chosenButton.Foreground = new SolidColorBrush(Color.FromRgb(255, 140, 0));
        chosenButton.IsEnabled = false;
        UpdateBoard(chosenButton, "O");
        string winner = CheckWinner();
        if (winner != null)
        {
          MessageBox.Show(this, winner, "Game Status", MessageBoxButton.OK);
          ResetGame();
          return;
        }

        if (IsDraw())
        {
          MessageBox.Show(this, "It's a draw!", "Game Status", MessageBoxButton.OK);
          ResetGame();
          return;
        }
      }
      else
      {
        MessageBox.Show(this, "It's a draw!", "Game Status", MessageBoxButton.OK);
        ResetGame();
      }
    }
    private string CheckWinner()
    {
      for (int row = 0; row < 3; row++)
      {
        if (board[row, 0] == board[row, 1] && board[row, 1] == board[row, 2] && board[row, 0] != "")
         return HumanOrComputer(board[row, 0]);
      }

      for (int col = 0; col < 3; col++)
      {
        if (board[0, col] == board[1, col] && board[1, col] == board[2, col] && board[0, col] != "")
          return HumanOrComputer(board[0,col]);
      }

      if (board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2] && board[0, 0] != "")
        return HumanOrComputer(board[0, 0]);

      if (board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0] && board[0, 2] != "")
        return HumanOrComputer(board[0, 2]);

      return null; // No winner
    }

    private string HumanOrComputer(string OorX)
    {
      return OorX == "X" ? "Player wins!" : "Computer wins!";
    }

    private bool IsDraw()
    {
      for (int row = 0; row < 3; row++)
      {
        for (int col = 0; col < 3; col++)
        {
          if (board[row, col] == "")
            return false; 
        }
      }
      return true; 
    }
    private void UpdateBoard(Button button, string player)
    {
      int row = int.Parse(button.Name.Substring(6, 1));
      int col = int.Parse(button.Name.Substring(7, 1));
      board[row, col] = player;
    }
    private void ResetGame()
    {
      for (int row = 0; row < 3; row++)
      {
        for (int col = 0; col < 3; col++)
        {
          board[row, col] = "";
        }
      }

      foreach (Button btn in new List<Button> { Square00, Square01, Square02, Square10, Square11, Square12, Square20, Square21, Square22 })
      {
        btn.Content = "";
        btn.IsEnabled = true;
      }

      isHumanTurn = true;
    }


  }
}
