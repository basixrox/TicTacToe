using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TicTacToe
{
    /// <summary>
    /// Interaktionslogik für MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        int[,] game = new int[3,3];
        string currentplayer = "";
        string player1 = "Player 1";
        string player2 = "Player 2";
        bool playing = false;
        int score1 = 0;
        int score2 = 0;
        int moves = 0;

        public MainView()
        {
            InitializeComponent();
            startStopGame(false);
        }

        private int GetCurrentPlayer()
        {
            int player = (currentplayer == player1)?1:2;
            return player;
        }

        /// <summary>
        /// resets all variables to starting "positions"
        /// </summary>
        private void initializeGame()
        {
            displayScore();
            game = new int[3,3] { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };

            txt00.Text = "";
            txt01.Text = "";
            txt02.Text = "";
            txt10.Text = "";
            txt11.Text = "";
            txt12.Text = "";
            txt20.Text = "";
            txt21.Text = "";
            txt22.Text = "";

            moves = 0;

            txtMessages.Text = "";
        }
        /// <summary>
        /// enables or disables certain controls depending on the boolean input
        /// </summary>
        /// <param name="start">true if game can start, false if not</param>
        private void startStopGame(bool start)
        {
            txt00.IsEnabled = start;
            txt01.IsEnabled = start;
            txt02.IsEnabled = start;
            txt10.IsEnabled = start;
            txt11.IsEnabled = start;
            txt12.IsEnabled = start;
            txt20.IsEnabled = start;
            txt21.IsEnabled = start;
            txt22.IsEnabled = start;

            btnStart.Content = (start)?"Give up":"Start";
            txtPlayer1.IsEnabled = !start;
            txtPlayer2.IsEnabled = !start;
        }

        /// <summary>
        /// updates the score TextBlock
        /// </summary>
        private void displayScore()
        {
            if (txtScore != null)
            {
                txtScore.Text = String.Format("{0,-10} - {1}\n{2,10} - {3}", player1, player2, score1, score2);
            }
        }

        /// <summary>
        /// Changes the internal variables of the player names
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void playerNameChanged (object sender, EventArgs e)
        {
            TextBox nameBox = (TextBox)sender;
            String oldName = (nameBox.Name == "txtPlayer1") ? player1 : player2;
            String newName = nameBox.Text;
            if (oldName != newName)
            {
                if (score1 != 0 || score2 != 0)
                {
                    MessageBoxResult reset = MessageBox.Show("A player name was changed. Would you like to reset the game score?", "Reset score?", MessageBoxButton.YesNo);
                    if (reset == MessageBoxResult.Yes)
                    {
                        score1 = 0;
                        score2 = 0;
                    }
                }
                if (nameBox.Name == "txtPlayer1")
                {
                    player1 = newName;
                }
                else
                {
                    player2 = newName;
                }
                displayScore();
            }
        }

        /// <summary>
        /// Before closing the window, the user is asked to confirm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to close this application?", "Please confirm", MessageBoxButton.YesNoCancel, MessageBoxImage.None);
            if (!(result == MessageBoxResult.Yes))
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Closes the Window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Display the help window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("This feature is not implemented yet. It will be added soon.", "To be implemented", MessageBoxButton.OK);
        }
        
        /// <summary>
        /// handles the selection of a field
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectField(object sender, MouseButtonEventArgs e)
        {
            TextBlock selected = (TextBlock)sender;
            string fieldname = selected.Name;
            int row = Int16.Parse(fieldname.Substring(3,1));
            int col = Int16.Parse(fieldname.Substring(4,1));
            
            if (game[row, col] != 0)
            {
                txtMessages.Text = "This field was already selected. Please select another field!";
                return;
            }

            int playerCurrent = GetCurrentPlayer();
            game[row,col] = playerCurrent;
            selected.Text = (playerCurrent == 1) ? "X" : "O";

            if (won())
            {
                //inscrease score
                if (playerCurrent == 1) { score1++; } else { score2++; }
                displayScore();
                txtMessages.Text = currentplayer + " has won! Play again?";
                MessageBox.Show(currentplayer + " has won!", "Game has ended!", MessageBoxButton.OK);
                startStopGame(false);
                playing = false;
                return;
            }

            moves++;

            nextPlayer();

            if (moves == 9)
            {
                txtMessages.Text = "It's a tie! Play again?";
                MessageBox.Show("It's a tie!", "Game has ended!", MessageBoxButton.OK);
                startStopGame(false);
                playing = false;
                return;
            }
        }

        /// <summary>
        /// Switch to the next player
        /// </summary>
        private void nextPlayer()
        {
            if (txtMessages != null)
            {
                switch (GetCurrentPlayer())
                {
                    case 1:
                        currentplayer = player2;
                        txtMessages.Text = player2 + " it's your turn!";
                        break;
                    default:
                        currentplayer = player1;
                        txtMessages.Text = player1 + " it's your turn!";
                        break;
                }
            }
        }

        /// <summary>
        /// Handles clicks on the Start (or Stop) button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (playing)
            {
                startStopGame(false);
                playing = false;                
                if (currentplayer == player1)
                {
                    score2++;
                }
                else
                {
                    score1++;
                }
                displayScore();
                string giveUp = currentplayer + " gave up! Play again?";
                nextPlayer();
                txtMessages.Text = giveUp;
                return;
            }
            startStopGame(true);
            playing = true;
            initializeGame();
            if (currentplayer != player1 && currentplayer != player2)
            {
                currentplayer = player1;
            }
            txtMessages.Text = currentplayer + " it's your turn!";
        }

        /// <summary>
        /// Checks for a winner
        /// </summary>
        /// <returns>true if winenr was found, false if game continues</returns>
        private bool won()
        {
            int zz = game[0, 0];
            int zo = game[0, 1];
            int zt = game[0, 2];
            int oz = game[1, 0];
            int oo = game[1, 1];
            int ot = game[1, 2];
            int tz = game[2, 0];
            int to = game[2, 1];
            int tt = game[2, 2];

            //top row, first col or diagonal top left to bottom right
            if (zz != 0)
            {
                if (((zz == zo) && (zz == zt)) ||
                    ((zz == oz) && (zz == tz)) ||
                    ((zz == oo) && (zz == tt)))
                {
                    return true;
                }
            }
            //middle row, second col or diagonal bottom left to top right
            if (oo != 0)
            {
                if (((oo == oz) && (oo == ot)) ||
                    ((oo == zo) && (oo == to)) ||
                    ((oo == tz) && (oo == zt)))
                {
                    return true;
                }
            }
            //bottom row or third col
            if (tt != 0)
            {
                if (((tt == tz) && (tt == to)) ||
                    ((tt == ot) && (tt == zt)))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
