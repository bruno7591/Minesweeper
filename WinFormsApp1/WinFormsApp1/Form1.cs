
using System.Drawing.Drawing2D;
using System.Reflection.Emit;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using System.Media;
using System.Drawing;
using MaterialSkin;
using MaterialSkin.Controls;

namespace WinFormsApp1
{
 

    public partial class Form1 : Form

    {

        private const int ROWS = 15;
        private const int COLS = 15;
        private const int MINES = 10;
        private int nbrevealed = 0;
        private int Xpad = 10;
        private int Ypad = 30;
        private int score = 0;
        private bool[,] mines;
        private bool[,] revealed;
        private Button[,] buttons;
        private System.Windows.Forms.Label[] labels;
        private int seconds = 101;
        private System.Windows.Forms.Timer timer;
        private bool win = false;
        private bool loose = false;
        private SoundPlayer player;


        public Form1()

        {

            InitializeComponent();
            this.player = new SoundPlayer();
            player.SoundLocation = @"C:\Users\serge\source\repos\WinFormsApp1\WinFormsApp1\bin\Debug\net6.0-windows\explosion.wav";
            this.Size = new Size(500, 600);
            mines = new bool[ROWS, COLS];

            revealed = new bool[ROWS, COLS];

            buttons = new Button[ROWS + 1, COLS + 1];

            labels = new System.Windows.Forms.Label[2];

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000; // 1 second
            timer.Tick += new EventHandler(OnTimerTick);
            timer.Start();

            //Compteur de temps
            labels[0] = new System.Windows.Forms.Label();
            labels[0].Location = new Point(35*11,0);
            labels[0].AutoSize = true;
            labels[0].Text = "";
            labels[0].Font = new Font("Arial", 18, FontStyle.Bold);
            labels[0].ForeColor = Color.Red;
            labels[0].BackColor = Color.Black;
            //Compteur de points
            labels[1] = new System.Windows.Forms.Label();
            labels[1].Location = new Point(0,0);
            labels[1].AutoSize = true;
            labels[1].Text = "0";
            labels[1].Font = new Font("Arial", 18, FontStyle.Bold);
            labels[1].ForeColor = Color.Red;
            labels[1].BackColor = Color.Black;

            

            this.Controls.Add(labels[0]);
            this.Controls.Add(labels[1]);




            this.Text = "Minesweeper by Bruno De Jesus";

            for (int row = 0; row < ROWS; row++)

            {

                for (int col = 0; col < COLS; col++)

                {

                    buttons[row, col] = new Button();

                    buttons[row, col].Size = new System.Drawing.Size(30, 30);

                    buttons[row, col].Location = new System.Drawing.Point(col * 30 + Xpad, row * 30+Ypad);

                    buttons[row, col].MouseUp += new MouseEventHandler(ButtonClick);



                    this.Controls.Add(buttons[row, col]);

                }

            }

            
            buttons[ROWS, COLS] = new Button();
            buttons[ROWS, COLS].Text = "Recommencer";
            buttons[ROWS, COLS].Size = new System.Drawing.Size(100, 30);
            buttons[ROWS, COLS].Location = new System.Drawing.Point(200,0);
            buttons[ROWS, COLS].MouseUp += new MouseEventHandler(OnRestartButtonClicked);
            buttons[ROWS, COLS].BackColor = default(Color);
            this.Controls.Add(buttons[ROWS, COLS]);
            

   
            // Place mines randomly

            //Array.Clear(mines, 0, mines.Length);
            //mines = new bool[ROWS, COLS];
            Random random = new Random();

            int minesPlaced = 0;

            while (minesPlaced < MINES)

            {

                int row = random.Next(ROWS);

                int col = random.Next(COLS);

                if (!mines[row, col])

                {

                    mines[row, col] = true;

                    minesPlaced++;

                }

            }
            /*
            PictureBox pictureBoxStart = new PictureBox();
            pictureBoxStart.Name = "pictureBoxStart";
            pictureBoxStart.Location = new System.Drawing.Point(0, 400);
            // Chargez l'animation d'explosion
            pictureBoxStart.Image = new Bitmap("fond.jpg");
            pictureBoxStart.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxStart.Dock = DockStyle.Fill;
            this.Controls.Add(pictureBoxStart);
            */
        }
        /*
        private void OnTimerTick(object sender, EventArgs e)
        {
            seconds = seconds - 1;
            if (seconds == 0 & loose == false )
            {
                loose = true;
                GameOver();
            }
            else if (seconds > 0 )
            {
                //labels[0].Text = "Temps Restant : " + seconds.ToString();
                labels[0].Text = string.Format("{0:00}:{1:00}", seconds/60, seconds%60);
            }
            else
            {
                labels[0].Text = "Perdu !";
            }
        }

        */
        private void RoundButtonCorners(Button btn)
        {
            GraphicsPath buttonPath = new GraphicsPath();
            int radius = 10;
            Rectangle rect = new Rectangle(0, 0, btn.Width, btn.Height);
            buttonPath.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            buttonPath.AddArc(rect.X + rect.Width - radius, rect.Y, radius, radius, 270, 90);
            buttonPath.AddArc(rect.X + rect.Width - radius, rect.Y + rect.Height - radius, radius, radius, 0, 90);
            buttonPath.AddArc(rect.X, rect.Y + rect.Height - radius, radius, radius, 90, 90);
            buttonPath.CloseFigure();
            btn.Region = new Region(buttonPath);
        }


        private void OnTimerTick(object sender, EventArgs e)
        {
            seconds = seconds - 1;
            if (seconds == 0 & loose == false)
            {
                loose = true;
                GameOver();
            }
            else if (seconds > 0 & loose == false & win == false)
            {
                labels[0].Text = string.Format("{0:00}:{1:00}", seconds / 60, seconds % 60);
            }
            
        }

        private void OnRestartButtonClicked(object sender, MouseEventArgs e)

        {
            this.loose = false;
            this.win= false;
            // Reset the game state
            this.score = 0;
            labels[1].Text = score.ToString();
            seconds = 101;


            try
            {
                PictureBox pictureBoxExplosion = (PictureBox)this.Controls.Find("pictureBoxExplosion", true)[0];
                pictureBoxExplosion.Dispose();
                

            }
            catch (System.IndexOutOfRangeException ex)
            {
                Console.WriteLine("problem");
            }
            try
            {
                
                PictureBox pictureBoxVictory = (PictureBox)this.Controls.Find("pictureBoxVictory", true)[0];
                pictureBoxVictory.Dispose();

            }
            catch (System.IndexOutOfRangeException ex)
            {
                Console.WriteLine("problem");
            }
            PictureBox pictureBoxStart = new PictureBox();
            pictureBoxStart.Name = "pictureBoxStart";
            pictureBoxStart.Location = new System.Drawing.Point(0, 400);
            // Chargez l'animation d'explosion
            pictureBoxStart.Image = new Bitmap("fond.jpg");
            pictureBoxStart.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxStart.Dock = DockStyle.Fill;
            this.Controls.Add(pictureBoxStart);

            for (int row = 0; row < ROWS; row++)

            {

                for (int col = 0; col < COLS; col++)

                {

                    revealed[row, col] = false;
                    buttons[row, col].BackColor = default(Color);
                    buttons[row, col].Enabled = true;

                    buttons[row, col].Text = "";
                    buttons[row, col].Image = null;

                }

            }



            // Generate new mines
            mines = new bool[ROWS, COLS];

            Random random = new Random();

            int minesPlaced = 0;

            while (minesPlaced < MINES)

            {

                int row = random.Next(ROWS);

                int col = random.Next(COLS);

                if (!mines[row, col])

                {

                    mines[row, col] = true;

                    minesPlaced++;

                }

            }

        }
        private void CheckForVictory()
        {
            int revealedCount = 0;
            for (int row = 0; row < ROWS; row++)
            {
                for (int col = 0; col < COLS; col++)
                {
                    if (!mines[row, col] && revealed[row, col])
                    {
                        revealedCount++;
                    }
                }
            }
            if (revealedCount == (ROWS * COLS) - MINES)
            {
                labels[0].Text = "Victoire !";
                win = true;
                this.score = (ROWS * COLS) - MINES;
                labels[1].Text = score.ToString();
                for (int row = 0; row < ROWS; row++)

                {

                    for (int col = 0; col < COLS; col++)

                    {

                        if (mines[row, col])

                        {

                            Bitmap bmp = new Bitmap(Image.FromFile("minesweeper.jpg"), new Size(buttons[row, col].Width, buttons[row, col].Width * Image.FromFile("minesweeper.jpg").Height / Image.FromFile("minesweeper.jpg").Width));
                            Graphics g = Graphics.FromImage(bmp);
                            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            g.DrawImage(Image.FromFile("minesweeper.jpg"), 0, 0, bmp.Width, bmp.Height);
                            buttons[row, col].Image = bmp;

                        }

                        buttons[row, col].Enabled = false;

                    }

                }
                try
                {
                    PictureBox pictureBoxStart = (PictureBox)this.Controls.Find("pictureBoxStart", true)[0];
                    pictureBoxStart.Dispose();


                }
                catch (System.IndexOutOfRangeException ex)
                {
                    Console.WriteLine("problem");
                }
                PictureBox pictureBoxVictory = new PictureBox();
                pictureBoxVictory.Name = "pictureBoxVictory";

                pictureBoxVictory.Location = new System.Drawing.Point(0, 400);
                // Chargez l'animation d'explosion
                pictureBoxVictory.Image = new Bitmap("victoire.gif");
                pictureBoxVictory.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBoxVictory.Dock = DockStyle.Fill;
                this.Controls.Add(pictureBoxVictory);

                win = true;
            }
            
            
    
        }

        private void ButtonClick(object sender, MouseEventArgs e)

        {

            Button button = (Button)sender;

            int row = (button.Location.Y - Ypad) / 30;

            int col = (button.Location.X - Xpad)/ 30;



            if (e.Button == MouseButtons.Left)

            {

                if (mines[row, col])

                {


                    Bitmap bmp = new Bitmap(Image.FromFile("minesweeper.jpg"), new Size(button.Width, button.Width * Image.FromFile("minesweeper.jpg").Height / Image.FromFile("minesweeper.jpg").Width));
                    Graphics g = Graphics.FromImage(bmp);
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.DrawImage(Image.FromFile("minesweeper.jpg"), 0, 0, bmp.Width, bmp.Height);
                    button.Image = bmp;

                    GameOver();

                }

                else

                {

                    Reveal(row, col);

                }

            }

            else if (e.Button == MouseButtons.Right)

            {

                if (button.Image != null)

                {

                    
                    button.Image = null;

                }

                else

                {

                    
                    Bitmap bmp = new Bitmap(Image.FromFile("drapeau.png"), new Size(button.Width, button.Width * Image.FromFile("drapeau.png").Height / Image.FromFile("drapeau.png").Width));
                    Graphics g = Graphics.FromImage(bmp);
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.DrawImage(Image.FromFile("drapeau.png"), 0, 0, bmp.Width, bmp.Height);
                    button.Image = bmp;

                }

            }
            CheckForVictory();

        }


        private void Reveal(int row, int col)

        {

            if (row < 0 || row >= ROWS || col < 0 || col >= COLS)

            {

                return;

            }

            if (revealed[row, col])

            {

                return;

            }

            revealed[row, col] = true;
            nbrevealed += 1;
            this.score += 1;
            buttons[row, col].Image = null;
            buttons[row, col].BackColor = Color.DarkGray;
            //buttons[row, col].Enabled = false;
            labels[1].Text = score.ToString();


            if (mines[row, col])

            {

                return;

            }


            int adjacentMines = CountAdjacentMines(row, col);

            if (adjacentMines > 0)

            {
                buttons[row, col].Image = null;
                buttons[row, col].Text = adjacentMines.ToString();
                if (adjacentMines == 1)
                {
                    buttons[row, col].ForeColor = Color.Blue;
                }
                else if (adjacentMines == 2)
                {
                    buttons[row, col].ForeColor = Color.Green;
                }
                else if (adjacentMines == 3)
                {
                    buttons[row, col].ForeColor = Color.Red;
                }
                
                
                return;

            }

            else

            {

                Reveal(row - 1, col);

                Reveal(row + 1, col);

                Reveal(row, col - 1);

                Reveal(row, col + 1);

                



            }
           

        }

        private int CountAdjacentMines(int row, int col)

        {

            int count = 0;

            for (int i = row - 1; i <= row + 1; i++)

            {

                for (int j = col - 1; j <= col + 1; j++)

                {

                    if (i >= 0 && i < ROWS && j >= 0 && j < COLS && mines[i, j])

                    {

                        count++;

                    }

                }

            }

            return count;

        }


        private void GameOver()

        {

            try
            {
                PictureBox pictureBoxStart = (PictureBox)this.Controls.Find("pictureBoxStart", true)[0];
                pictureBoxStart.Dispose();


            }
            catch (System.IndexOutOfRangeException ex)
            {
                Console.WriteLine("problem");
            }

            for (int row = 0; row < ROWS; row++)

            {

                for (int col = 0; col < COLS; col++)

                {

                    if (mines[row, col])

                    {

                        Bitmap bmp = new Bitmap(Image.FromFile("minesweeper.jpg"), new Size(buttons[row, col].Width, buttons[row, col].Width * Image.FromFile("minesweeper.jpg").Height / Image.FromFile("minesweeper.jpg").Width));
                        Graphics g = Graphics.FromImage(bmp);
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.DrawImage(Image.FromFile("minesweeper.jpg"), 0, 0, bmp.Width, bmp.Height);
                        buttons[row, col].Image = bmp;

                    }

                    buttons[row, col].Enabled = false;

                }

            }
            player.Play();
            PictureBox pictureBoxExplosion = new PictureBox();
            pictureBoxExplosion.Name = "pictureBoxExplosion";

            pictureBoxExplosion.Location = new System.Drawing.Point(0,400);
            // Chargez l'animation d'explosion
            pictureBoxExplosion.Image = new Bitmap("explosion.gif");
            pictureBoxExplosion.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxExplosion.Dock= DockStyle.Fill;
            this.Controls.Add(pictureBoxExplosion);

            loose = true;
            // Attendez 2 secondes avant de masquer l'animation
            // pictureBoxExplosion.Visible = false;
            labels[0].Text = "Perdu !";
            //MessageBox.Show("Game Over!");

        }

    }

}