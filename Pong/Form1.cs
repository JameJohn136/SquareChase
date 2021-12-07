// 2-Player "Square Chase" by James Johnson
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace Pong
{
    public partial class Form1 : Form
    {
        // Declaring all the global variables
        Rectangle player1 = new Rectangle(100, 100, 25, 25);
        Rectangle player2 = new Rectangle(150, 150, 25, 25);
        Rectangle ball = new Rectangle(295, 195, 10, 10);
        Rectangle speedBoost = new Rectangle(295, 195, 10, 10);
        Rectangle area = new Rectangle(75, 75, 300, 300);

        SoundPlayer speedSound = new SoundPlayer(Properties.Resources.speedSound);
        SoundPlayer pointSound = new SoundPlayer(Properties.Resources.pointSound);

        int player1Score = 0;
        int player2Score = 0;

        int player1Speed = 3;
        int player2Speed = 3;

        bool wDown = false;
        bool sDown = false;
        bool dDown = false;
        bool aDown = false;
        bool upArrowDown = false;
        bool downArrowDown = false;
        bool leftArrowDown = false;
        bool rightArrowDown = false;

        SolidBrush blueBrush = new SolidBrush(Color.DodgerBlue);
        SolidBrush whiteBrush = new SolidBrush(Color.White);
        SolidBrush redBrush = new SolidBrush(Color.Red);
        SolidBrush greenBrush = new SolidBrush(Color.LawnGreen);
        Pen whitePen = new Pen(Color.White, 3);

        Random randGen = new Random(); // Generate Random Numbers

        public Form1()
        {
            InitializeComponent();

            // Make the Score Labels show 0
            p1ScoreLabel.Text = $"{player1Score}";
            p2ScoreLabel.Text = $"{player2Score}";

            // Find random positions for the objectives
            speedBoost.X = randGen.Next(75, 351);
            speedBoost.Y = randGen.Next(75, 351);
            ball.X = randGen.Next(75, 351);
            ball.Y = randGen.Next(75, 351);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode) // Get Input
            {
                case Keys.W:
                    wDown = true;
                    break;
                case Keys.S:
                    sDown = true;
                    break;
                case Keys.D:
                    dDown = true;
                    break;
                case Keys.A:
                    aDown = true;
                    break;
                case Keys.Up:
                    upArrowDown = true;
                    break;
                case Keys.Down:
                    downArrowDown = true;
                    break;
                case Keys.Left:
                    leftArrowDown = true;
                    break;
                case Keys.Right:
                    rightArrowDown = true;
                    break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)

        {
            switch (e.KeyCode) // Getting Input part 2
            {
                case Keys.W:
                    wDown = false;
                    break;
                case Keys.S:
                    sDown = false;
                    break;
                case Keys.D:
                    dDown = false;
                    break;
                case Keys.A:
                    aDown = false;
                    break;
                case Keys.Up:
                    upArrowDown = false;
                    break;
                case Keys.Down:
                    downArrowDown = false;
                    break;
                case Keys.Left:
                    leftArrowDown = false;
                    break;
                case Keys.Right:
                    rightArrowDown = false;
                    break;
            }

        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            // Always update the scores
            p1ScoreLabel.Text = player1Score.ToString();
            p2ScoreLabel.Text = player2Score.ToString();

            //move player 1 
            if (wDown == true && player1.Y > 75)
            {
                player1.Y -= player1Speed;
            }

            if (sDown == true && player1.Y < 350)
            {
                player1.Y += player1Speed;
            }

            if (aDown == true && player1.X > 75)
            {
                player1.X -= player1Speed;
            }

            if (dDown && player1.X < 350)
            {
                player1.X += player1Speed;
            }

            //move player 2 
            if (upArrowDown == true && player2.Y > 75)
            {
                player2.Y -= player2Speed;
            }

            if (downArrowDown == true && player2.Y < 350)
            {
                player2.Y += player2Speed;
            }

            if (leftArrowDown == true && player2.X > 75)
            {
                player2.X -= player2Speed;
            }

            if (rightArrowDown && player2.X < 350)
            {
                player2.X += player2Speed;
            }

            //check if ball hits either player. If it does change the direction 
            //and place the ball in front of the player hit 
            if (player1.IntersectsWith(ball))
            {
                // Call a seperate method that we input two variables to tell it what it needs to do
                SpawnNewBall("ball", 1);
            }
            else if (player1.IntersectsWith(speedBoost))
            {
                SpawnNewBall("speed", 1);
            }
            else if (player2.IntersectsWith(ball))
            {
                SpawnNewBall("ball", 2);
            }
            else if (player2.IntersectsWith(speedBoost))
            {
                SpawnNewBall("speed", 2);
            }

            // check score and stop game if either player is at 10 
            if (player1Score == 10)
            {
                gameTimer.Enabled = false;
                winLabel.Visible = true;
                p1ScoreLabel.Text = "10";
                winLabel.Text = "Player 1  Wins!!";
            }
            else if (player2Score == 10)
            {
                gameTimer.Enabled = false;
                p2ScoreLabel.Text = "10";
                winLabel.Visible = true;
                winLabel.Text = "Player 2  Wins!!";
            }
            Refresh();
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            // Paint the objects on the screen every frame
            e.Graphics.FillRectangle(redBrush, player1);
            e.Graphics.FillRectangle(blueBrush, player2);
            e.Graphics.FillRectangle(whiteBrush, ball);
            e.Graphics.DrawRectangle(whitePen, area);
            e.Graphics.FillRectangle(greenBrush, speedBoost);
            e.Graphics.DrawRectangle(whitePen, player1);
            e.Graphics.DrawRectangle(whitePen, player2);
        }

        public void SpawnNewBall(string type, int player) 
        {
            // I sent in two variables to easily manage all this in one method
            if (type == "ball")
            {
                switch (player)
                {
                    case 1:
                        player1Score++;
                        break;
                    case 2:
                        player2Score++;
                        break;
                }

                pointSound.Play();

                ball.X = randGen.Next(75, 351);
                ball.Y = randGen.Next(75, 351);
            }

            if (type == "speed")
            {
                switch (player)
                {
                    case 1:
                        if (player1Speed < 10) // Limits the player speed at 10 (3.3x normal speed) because it gets too fast
                        {
                        player1Speed++;
                        }
                        break;
                    case 2:
                        if (player2Speed < 10)
                        {
                        player2Speed++;
                        }
                        break;
                }

                speedSound.Play();

                speedBoost.X = randGen.Next(75, 351);
                speedBoost.Y = randGen.Next(75, 351);
            }

        }
    }
}
