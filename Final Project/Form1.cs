using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Media;

namespace Final_Project
{
    public class LineSeg
    {
        public PointF PS, PE;
        public void DrawYourSelf(Graphics g)
        {
            Pen Pn = new Pen(Color.DarkCyan, 4);
            g.DrawLine(Pn, PS.X, PS.Y, PE.X, PE.Y);
            g.FillEllipse(Brushes.Red, PS.X - 2, PS.Y - 3, 5, 5);
            g.FillEllipse(Brushes.Red, PE.X - 2, PE.Y - 3, 5, 5);
        }
        public void DrawYourSelf2(Graphics g)
        {
            Pen Pn = new Pen(Color.DarkCyan, 4);
            g.DrawLine(Pn, PS.X, PS.Y, PE.X, PE.Y);
        }
        public void Translate(float tx, float ty)
        {
            PS.X += tx;
            PS.Y += ty;

            PE.X += tx;
            PE.Y += ty;
        }

        public void RotateArounAny(float theta, PointF pRef)
        {
            Translate(-pRef.X, -pRef.Y);
            Rotate(theta);
            Translate(pRef.X, pRef.Y);
        }

        public void Rotate(float theta)
        {
            float thR = (float)(theta * Math.PI / 180);

            double xn = PS.X * Math.Cos(thR) - PS.Y * Math.Sin(thR);
            double yn = PS.X * Math.Sin(thR) + PS.Y * Math.Cos(thR);
            PS.X = (float)xn;
            PS.Y = (float)yn;

            xn = PE.X * Math.Cos(thR) - PE.Y * Math.Sin(thR);
            yn = PE.X * Math.Sin(thR) + PE.Y * Math.Cos(thR);
            PE.X = (float)xn;
            PE.Y = (float)yn;

        }
    }
    class pointsz
    {
        public float x;
        public float y;
        public float thrad;
    }
    class elevating
    {
        public float x;
        public float y;
        public string scoreee;
    }
    public partial class Form1 : Form
    {
        //images
        Bitmap off, ground, Menus;
        //====================================================
        //Flags
        bool inmenu = true, options = false, fx = true, sound = true, tempfx, tempsound, mangle = false, mpower = false, turns = true, fired = false, scatter = false, pine = false, spider = false, secondpine = false, thirdpine = false, dodmage = false, enteredname = false, oneplayer = true;
        bool[] fired1 = { false, false, false, false, false, false };
        bool[] fired2 = { false, false, false, false, false, false };
        bool[] hit = { false, false, false, false, false };
        int names = 0;
        //====================================================
        //misc
        Timer tt = new Timer();
        Pen p = new Pen(Color.FromArgb(10, 64, 35));
        Circle tank1angle = new Circle();
        Circle tank2angle = new Circle();
        Circle pineaplemainshot = new Circle();
        Font f = new Font("Arial", 25);
        Font f2 = new Font("Arial", 10);
        Font f3 = new Font("Arial", 50);
        SoundPlayer music = new SoundPlayer();
        Random rnd = new Random();
        //====================================================
        //Arrays&Lists
        List<pointsz> poi = new List<pointsz>();
        List<LineSeg> tank1 = new List<LineSeg>();
        List<LineSeg> tank2 = new List<LineSeg>();
        List<pointsz> spiders = new List<pointsz>();
        List<pointsz> pinaples = new List<pointsz>();
        List<elevating> elevatingscore = new List<elevating>();
        List<Circle> Damage = new List<Circle>();
        //====================================================
        //Positions
        int t1xs = 120, t1xe = 160, t2xs = 1400, t2xe = 1440, movingp1 = 160, movingp2 = 1440;
        float t1ax, t1ay, t2ax, t2ay, angle1 = 320, angle2 = 230, mx, my;
        float mouseposx, mouseposy;
        float wx, wy, facx, facy, wx2, wy2, facx2, facy2, wx3, wy3, facx3, facy3, wx4, wy4, facx4, facy4, wx5, wy5, facx5, facy5;
        //====================================================
        //Strings
        string angles, player1 = "Player1", player2 = "Player2";
        string[] weapons = { "Single shot", "3-Shots", "5-Shots", "Scatter Shot", "Spider", "Pineapple" };
        //====================================================
        //Variables
        int power1 = 60, power2 = 60, nomoves1 = 99, nomoves2 = 4, rounds = 1, Score1 = 0, Score2 = 0, selectedweapon1 = 0, selectedweapon2 = 0;
        float dis = 0;
        //====================================================
        public Form1()
        {
            this.Load+=Form1_Load;
            this.Paint += Form1_Paint;
            this.MouseDown += Form1_MouseDown;
            this.MouseUp += Form1_MouseUp;
            this.MouseMove += Form1_MouseMove;
            this.KeyDown += Form1_KeyDown;
            this.WindowState = FormWindowState.Maximized;
            tt.Tick += new EventHandler(tt_Tick);
            tt.Start();
            tt.Interval = 10;
        }

        void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!enteredname)
            {
                if (e.KeyCode==Keys.Enter)
                {
                    names++;
                    if (names==2)
                    {
                        enteredname = true;
                    }
                }
                else
                {
                    if (names==0)
                    {
                        if (player1 == "Player1")
                        {
                            player1 = "";
                        }
                        player1 += e.KeyCode;
                    }
                    else
                    {
                        if (player2 == "Player2")
                        {
                            player2 = "";
                        }
                        player2 += e.KeyCode;
                    }
                }
            }
        }

        void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!inmenu)
            {
                if (mangle)
                {
                    if (turns)
                    {
                        angle1 = (float)(((float)Math.Atan2((e.Y - tank1angle.YC), (e.X - tank1angle.XC))) * 180 / Math.PI);
                    }
                    else
                    {
                        angle2 = (float)(((float)Math.Atan2((e.Y - tank2angle.YC), (e.X - tank2angle.XC))) * 180 / Math.PI);
                    }
                }
                if (mpower)
                {
                    if (turns)
                    {
                        if (e.X > 1050 && e.X < 1336)
                        {
                            power1 = (int)((e.X - 1050) / 2.815F);
                        }
                    }
                    else
                    {
                        if (e.X > 1050 && e.X < 1336)
                        {
                            power2 = (int)((e.X - 1050) / 2.815F);
                        }
                    }
                    mouseposx = e.X;
                    mouseposy = e.Y;
                }
            }
            else
            {
                if (!options)
                {
                    if (e.X >= 675 && e.X <= 1235 && e.Y >= 165 && e.Y <= 220)
                    {
                        Menus.Dispose();
                        Menus = new Bitmap("Menu/1Playerg.png");
                    }
                    else if (e.X >= 625 && e.X <= 1295 && e.Y >= 245 && e.Y <= 300)
                    {
                        Menus.Dispose();
                        Menus = new Bitmap("Menu/2Players.png");
                    }
                    else if (e.X >= 655 && e.X <= 1270 && e.Y >= 335 && e.Y <= 390)
                    {
                        Menus.Dispose();
                        Menus = new Bitmap("Menu/Lam.png");
                    }
                    else if (e.X >= 415 && e.X <= 1505 && e.Y >= 420 && e.Y <= 470)
                    {
                        Menus.Dispose();
                        Menus = new Bitmap("Menu/Target.png");
                    }
                    else if (e.X >= 415 && e.X <= 1505 && e.Y >= 505 && e.Y <= 555)
                    {
                        Menus.Dispose();
                        Menus = new Bitmap("Menu/return.png");
                    }
                    else if (e.X >= 700 && e.X <= 1215 && e.Y >= 675 && e.Y <= 720)
                    {
                        Menus.Dispose();
                        Menus = new Bitmap("Menu/Options.png");
                    }
                    else
                    {
                        Menus.Dispose();
                        Menus = new Bitmap("Menu/Menu.png");
                    }
                }
            }
        }

        void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (!inmenu)
            {
                mx = e.X;
                my = e.Y;
                //Move angle with mouse
                if (e.X >= 1155 && e.X <= 1349 && e.Y >= 783 && e.Y <= 836)
                {
                    mangle = true;
                }
                //move angle with mouse clicks ( + )
                else if (e.X >= 1306 && e.X <= 1349 && e.Y >= 838 && e.Y <= 875)
                {
                    if (turns)
                    {
                        angle1++;
                    }
                    else
                    {
                        angle2++;
                    }
                }
                //move angle with mouse clicks ( - )
                else if (e.X >= 1158 && e.X <= 1201 && e.Y >= 838 && e.Y <= 875)
                {
                    if (turns)
                    {
                        angle1--;
                    }
                    else
                    {
                        angle2--;
                    }
                }
                //Move Tank Right
                else if (e.X >= 734 && e.X <= 785 && e.Y >= 797 && e.Y <= 867)
                {
                    moveright();
                }
                //Move Tank Left
                else if (e.X >= 567 && e.X <= 628 && e.Y >= 800 && e.Y <= 869)
                {
                    moveleft();
                }
                //Change Power With mouse
                else if (e.X >= 1050 && e.X <= 1336 && e.Y >= 919 && e.Y <= 935)
                {
                    mpower=true;
                    mouseposx = e.X;
                    mouseposy = e.Y;
                }
                //Change Power With Clicks ( + )
                else if (e.X >= 1306 && e.X <= 1349 && e.Y >= 950 && e.Y <= 987)
                {
                    //Tank1
                    if (turns)
                    {
                        if (power1 < 100)
                        {
                            power1++;
                        }
                    }
                    //Tank2
                    else
                    {
                        if (power2 < 100)
                        {
                            power2++;
                        }
                    }
                }
                //Change Power With Clicks ( - )
                else if (e.X >= 1157 && e.X <= 1201 && e.Y >= 950 && e.Y <= 987)
                {
                    //Tank1
                    if (turns)
                    {
                        if (power1 > 0)
                        {
                            power1--;
                        }
                    }
                    //Tank2
                    else
                    {
                        if (power2 > 0)
                        {
                            power2--;
                        }
                    }
                }
                //Change Weapon With Clicks ( UP )
                else if (e.X >= 925 && e.X <= 978 && e.Y >= 923 && e.Y <= 953)
                {
                    if (turns)
                    {
                        if (selectedweapon1 > 0)
                        {
                            for (int i = selectedweapon1; i > 0; i--)
                            {
                                if (!fired1[(i-1)])
                                {
                                    selectedweapon1 = i - 1;
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (selectedweapon2 > 0)
                        {
                            for (int i = selectedweapon2; i > 0; i--)
                            {
                                if (!fired2[i])
                                {
                                    selectedweapon2 = i - 1;
                                    break;
                                }
                            }
                        }
                    }
                }
                //Change Weapon With Clicks ( Down )
                else if (e.X >= 925 && e.X <= 978 && e.Y >= 958 && e.Y <= 988)
                {
                    if (turns)
                    {
                        if (selectedweapon1 < weapons.Length - 1)
                        {
                            for (int i = selectedweapon1; i < weapons.Length; i++)
                            {
                                if (!fired1[i+1])
                                {
                                    selectedweapon1 = i + 1;
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (selectedweapon2 < weapons.Length - 1)
                        {
                            for (int i = selectedweapon2; i < weapons.Length; i++)
                            {
                                if (!fired2[i+1])
                                {
                                    selectedweapon2 = i + 1;
                                    break;
                                }
                            }
                        }
                    }
                }
                //Fire Button
                else if (e.X >= 866 && e.X <= 1077 && e.Y >= 802 && e.Y <= 862)
                {
                    fired = true;
                    if (turns)
                    {
                        fired1[selectedweapon1] = true;
                        //oneshot
                        if (selectedweapon1 == 0)
                        {
                            oneshot(power1, angle1, tank1[5].PE.X, tank1[5].PE.Y);
                        }
                        //threeshots
                        else if (selectedweapon1 == 1)
                        {
                            threeshots(power1, angle1, tank1[5].PE.X, tank1[5].PE.Y);
                        }
                        //fiveshots
                        else if (selectedweapon1 == 2)
                        {
                            fiveshots(power1, angle1, tank1[5].PE.X, tank1[5].PE.Y);
                        }
                        //scatteredshot
                        else if (selectedweapon1 == 3)
                        {
                            scattershot(power1, angle1, tank1[5].PE.X, tank1[5].PE.Y);
                        }
                        //spider
                        else if (selectedweapon1 == 4)
                        {
                            spidershot(power1, angle1, tank1[5].PE.X, tank1[5].PE.Y);
                        }
                        //pineaple
                        else if (selectedweapon1 == 5)
                        {
                            pineshot(power1, angle1, tank1[5].PE.X, tank1[5].PE.Y);
                        }
                    }
                    else
                    {
                        fired2[selectedweapon2] = true;
                        //oneshot
                        if (selectedweapon2 == 0)
                        {
                            oneshot(power2, angle2, tank2[5].PE.X, tank2[5].PE.Y);
                        }
                        //threeshots
                        else if (selectedweapon2 == 1)
                        {
                            threeshots(power2, angle2, tank2[5].PE.X, tank2[5].PE.Y);
                        }
                        //fivesshots
                        else if (selectedweapon2 == 2)
                        {
                            fiveshots(power2, angle2, tank2[5].PE.X, tank2[5].PE.Y);
                        }
                        //scatteredshot
                        else if (selectedweapon2 == 3)
                        {
                            scattershot(power2, angle2, tank2[5].PE.X, tank2[5].PE.Y);
                        }
                        //spider
                        else if (selectedweapon2 == 4)
                        {
                            spidershot(power2, angle2, tank2[5].PE.X, tank2[5].PE.Y);
                        }
                        //pineaple
                        else if (selectedweapon2 == 5)
                        {
                            pineshot(power2, angle2, tank2[5].PE.X, tank2[5].PE.Y);
                        }
                    }
                    //==============================================================
                    if (turns)
                    {
                        if (selectedweapon2 < weapons.Length - 1)
                        {
                            selectedweapon2++;
                        }
                    }
                    else
                    {
                        if (selectedweapon1 < weapons.Length - 1)
                        {
                            selectedweapon1++;
                        }
                    }
                    rounds++;
                }
            }
            else
            {
                //MainMenu
                if (!options)
                {
                    //this.Text = "" + e.X + "+" + e.Y;
                    //1Player
                    if (e.X >= 675 && e.X <= 1235 && e.Y >= 165 && e.Y <= 220)
                    {
                        oneplayer = true;
                        inmenu = false;
                        Menus.Dispose();
                        Menus = new Bitmap("InGame/WeaponDashboard.png");
                    }
                    //2Players
                    else if (e.X >= 625 && e.X <= 1295 && e.Y >= 245 && e.Y <= 300)
                    {
                        inmenu = false;
                        Menus.Dispose();
                        Menus = new Bitmap("InGame/WeaponDashboard.png");
                    }
                    //Lan
                    else if (e.X >= 655 && e.X <= 1270 && e.Y >= 335 && e.Y <= 390)
                    {
                        Menus.Dispose();
                    }
                    //Target
                    else if (e.X >= 415 && e.X <= 1505 && e.Y >= 420 && e.Y <= 470)
                    {
                        Menus.Dispose();
                    }
                    //Return
                    else if (e.X >= 415 && e.X <= 1505 && e.Y >= 505 && e.Y <= 555)
                    {
                        Menus.Dispose();
                    }
                    //Options
                    else if (e.X >= 700 && e.X <= 1215 && e.Y >= 675 && e.Y <= 720)
                    {
                        options = true;
                        Menus.Dispose();
                        Menus = new Bitmap("Options/Options0.png");
                        tempfx = fx;
                        tempsound = sound;
                    }
                }
                //=================================================================================
                //Options Menu
                else
                {
                    //FX ON
                    if (e.X >= 22 && e.X <= 186 && e.Y >= 450 && e.Y <= 550)
                    {
                        Menus.Dispose();
                        if (sound)
                        {
                            Menus = new Bitmap("Options/Options0.png");
                        }
                        else
                        {
                            Menus = new Bitmap("Options/Options2.png");
                        }
                        fx = true;
                    }
                    //FX OFF
                    else if (e.X >= 282 && e.X <= 475 && e.Y >= 445 && e.Y <= 550)
                    {
                        Menus.Dispose();
                        if (sound)
                        {
                            Menus = new Bitmap("Options/Options1.png");
                        }
                        else
                        {
                            Menus = new Bitmap("Options/Options3.png");
                        }
                        fx = false;
                    }
                    //Sound ON
                    else if (e.X >= 22 && e.X <= 189 && e.Y >= 750 && e.Y <= 850)
                    {
                        Menus.Dispose();
                        if (fx)
                        {
                            Menus = new Bitmap("Options/Options0.png");
                        }
                        else
                        {
                            Menus = new Bitmap("Options/Options1.png");
                        }
                        sound = true;
                        music.Play();
                    }
                    //Sound OFF
                    else if (e.X >= 283 && e.X <= 475 && e.Y >= 750 && e.Y <= 850)
                    {
                        Menus.Dispose();
                        if (fx)
                        {
                            Menus = new Bitmap("Options/Options2.png");
                        }
                        else
                        {
                            Menus = new Bitmap("Options/Options3.png");
                        }
                        sound = false;
                        music.Stop();
                    }
                    //save and exit
                    else if (e.X >= 1353 && e.X <= 1772 && e.Y >= 182 && e.Y <= 245)
                    {
                        Menus.Dispose();
                        Menus = new Bitmap("Menu/Menu.png");
                        options = false;
                    }
                    //dafaults
                    else if (e.X >= 1387 && e.X <= 1731 && e.Y >= 486 && e.Y <= 531)
                    {
                        Menus.Dispose();
                        Menus = new Bitmap("Menu/Menu.png");
                        options = false;
                        sound = true;
                        fx = true;
                    }
                    //cancel
                    else if (e.X >= 1432 && e.X <= 1703 && e.Y >= 772 && e.Y <= 823)
                    {
                        Menus.Dispose();
                        Menus = new Bitmap("Menu/Menu.png");
                        options = false;
                        fx = tempfx;
                        sound = tempsound;
                    }
                }
            }
        }

        void tt_Tick(object sender, EventArgs e)
        {
            if (oneplayer)
            {
                turns = true;
            }
            //Angles
            if (turns)
            {
                t1ax = tankanglex(angle1, tank1angle.Rad, tank1[5].PS.X);
                t1ay = tankangley(angle1, tank1angle.Rad, tank1[5].PS.Y);
                tank1[tank1.Count - 1].PE.X = t1ax;
                tank1[tank1.Count - 1].PE.Y = t1ay;
            }
            else
            {
                t2ax = tankanglex(angle2, tank2angle.Rad, tank2[5].PS.X);
                t2ay = tankangley(angle2, tank2angle.Rad, tank2[5].PS.Y);
                tank2[tank2.Count - 1].PE.X = t2ax;
                tank2[tank2.Count - 1].PE.Y = t2ay;
            }
            DrawDubb(this.CreateGraphics());
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            off = new Bitmap(ClientSize.Width, ClientSize.Height);
            music.SoundLocation = "Sounds/pocket.wav";
            music.Play();
            Menus = new Bitmap("Menu/Menu.png");
            ground = new Bitmap(ClientSize.Width, 765);
            Graphics g2 = Graphics.FromImage(ground);
            int i, k = 550;
            int whichscheme= rnd.Next(1, 5);
            if (whichscheme == 1||oneplayer)
            {
                for (i = 0; i < this.ClientSize.Width; i++)
                {
                    pointsz pnn = new pointsz();
                    if (i<100)
                    {
                        k--;
                    }
                    else if (i>300&&i<400)
                    {
                        k++;
                    }
                    else if (i > 450 && i < 460)
                    {
                        k--;
                    }
                    else if (i > 460 && i < 470)
                    {
                        k++;
                    }
                    else if (i > 480 && i < 495)
                    {
                        k--;
                    }
                    else if (i > 495 && i < 505)
                    {
                        k++;
                    }
                    if (i > 550 && i < 650)
                    {
                        k--;
                    }
                    else if (i > 700 && i < 900)
                    {
                        k--;
                    }
                    else if (i > 900 && i < 1100)
                    {
                        k++;
                    }
                    g2.DrawLine(p, i, 765, i, k);
                    pnn.x = i;
                    pnn.y = k;
                    poi.Add(pnn);
                }
            }
            else if (whichscheme == 2)
            {
                for (i = 0; i < this.ClientSize.Width; i++)
                {
                    pointsz pnn = new pointsz();
                    if (i < 100)
                    {
                        k--;
                    }
                    else if (i > 300 && i < 400)
                    {
                        k--;
                    }
                    else if (i > 450 && i < 460)
                    {
                        k++;
                    }
                    else if (i > 460 && i < 470)
                    {
                        k++;
                    }
                    else if (i > 480 && i < 495)
                    {
                        k--;
                    }
                    else if (i > 495 && i < 505)
                    {
                        k++;
                    }
                    if (i > 550 && i < 650)
                    {
                        k--;
                    }
                    else if (i > 700 && i < 900)
                    {
                        k--;
                    }
                    else if (i > 900 && i < 1100)
                    {
                        k++;
                    }
                    g2.DrawLine(p, i, 765, i, k);
                    pnn.x = i;
                    pnn.y = k;
                    poi.Add(pnn);
                }
            }
            else if (whichscheme == 3)
            {
                for (i = 0; i < this.ClientSize.Width; i++)
                {
                    pointsz pnn = new pointsz();
                    if (i < 100)
                    {
                        k--;
                    }
                    else if (i > 300 && i < 400)
                    {
                        k--;
                    }
                    else if (i > 400 && i < 500)
                    {
                        k++;
                    }
                    if (i > 550 && i < 650)
                    {
                        k--;
                    }
                    else if (i > 700 && i < 900)
                    {
                        k--;
                    }
                    else if (i > 900 && i < 1100)
                    {
                        k++;
                    }
                    g2.DrawLine(p, i, 765, i, k);
                    pnn.x = i;
                    pnn.y = k;
                    poi.Add(pnn);
                }
            }
            else if (whichscheme == 4)
            {
                for (i = 0; i < this.ClientSize.Width; i++)
                {
                    pointsz pnn = new pointsz();
                    if (i > 200 && i < 250)
                    {
                        k--;
                    }
                    else if (i > 250 && i < 310)
                    {
                        k++;
                    }
                    else if (i > 510 && i < 590)
                    {
                        k--;
                    }
                    else if (i > 600 && i < 680)
                    {
                        k++;
                    }
                    else if (i > 700 && i < 1000)
                    {
                        k--;
                    }
                    else if (i > 1000 && i < 1200)
                    {
                        k++;
                    }
                    g2.DrawLine(p, i, 765, i, k);
                    pnn.x = i;
                    pnn.y = k;
                    poi.Add(pnn);
                }
            }
            tank1angle.Rad = 20;
            tank1angle.XC = t1xs + 20;
            tank1angle.YC = poi[t1xs].y-4;
            tank2angle.Rad = 20;
            tank2angle.XC = t2xs + 20;
            tank2angle.YC = poi[t2xs].y-4;
            t1ax = tankanglex(angle1, tank1angle.Rad, tank1angle.XC);
            t1ay = tankangley(angle1, tank1angle.Rad, tank1angle.YC);
            t2ax = tankanglex(angle2, tank2angle.Rad, tank2angle.XC);
            t2ay = tankangley(angle2, tank2angle.Rad, tank2angle.YC);
            for (int z = 0; z < 6; z++)
            {
                LineSeg pnn = new LineSeg();
                if (z==5)
                {
                    pnn.PS = new PointF(tank1angle.XC, tank1angle.YC);
                    pnn.PE = new PointF(t1ax, t1ay);
                }
                else
                {
                    pnn.PS = new PointF(t1xs, poi[120].y - 1 - z);
                    pnn.PE = new PointF(t1xe, poi[120].y - 1 - z);
                }
                tank1.Add(pnn);
            }
            for (int z = 0; z < 6; z++)
            {
                LineSeg pnn = new LineSeg();
                if (z == 5)
                {
                    pnn.PS = new PointF(tank2angle.XC, tank2angle.YC);
                    pnn.PE = new PointF(t2ax, t2ay);
                }
                else
                {
                    pnn.PS = new PointF(t2xs, poi[1400].y - 1 - z);
                    pnn.PE = new PointF(t2xe, poi[1400].y - 1 - z);
                }
                tank2.Add(pnn);
            }
        }
        void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawDubb(e.Graphics);
        }
        void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            mangle = false;
            mpower = false;
        }
        void DrawDubb(Graphics g)
        {
            Graphics g2 = Graphics.FromImage(off);
            DrawScene(g2);
            g.DrawImage(off, 0, 0);
        }
        void DrawScene(Graphics g)
        {
            if (!inmenu)
            {
                if (enteredname)
                {
                    g.Clear(Color.FromArgb(13, 0, 26));
                    g.DrawImage(Menus, 0, 764, this.ClientSize.Width, (this.ClientSize.Height - 764));
                    //======================================================================================
                    //Ground
                    g.DrawImage(ground, 0, 0);
                    //======================================================================================
                    //Tank1
                    for (int i = 0; i < tank1.Count; i++)
                    {
                        if (i > 0)
                        {
                            tank1[i].DrawYourSelf2(g);
                        }
                        else
                        {
                            tank1[i].DrawYourSelf(g);
                        }
                    }
                    //===============================================
                    //Tank2
                    for (int i = 0; i < tank2.Count; i++)
                    {
                        if (i > 0)
                        {
                            tank2[i].DrawYourSelf2(g);
                        }
                        else
                        {
                            tank2[i].DrawYourSelf(g);
                        }
                    }
                    //======================================================================================
                    //Stats
                    if (turns)
                    {
                        if (angle1 < 0)
                        {
                            angles = ((int)angle1 + 360).ToString();
                        }
                        else
                        {
                            angles = ((int)angle1).ToString();
                        }
                        g.DrawString(nomoves1.ToString(), f, Brushes.Yellow, 665, 833);
                        g.DrawString(angles, f, Brushes.Yellow, 1220, 837);
                        g.FillRectangle(Brushes.IndianRed, 1050, 917, 2.815F * power1, 17);
                        g.DrawString(power1.ToString(), f, Brushes.Yellow, 1233, 954);
                        g.DrawString(weapons[selectedweapon1], f, Brushes.Yellow, 586, 940);
                    }
                    else
                    {
                        if (angle2 < 0)
                        {
                            angles = ((int)angle2 + 360).ToString();
                        }
                        else
                        {
                            angles = ((int)angle2).ToString();
                        }
                        g.DrawString(nomoves2.ToString(), f, Brushes.Yellow, 665, 833);
                        g.DrawString(angles, f, Brushes.Yellow, 1220, 837);
                        g.FillRectangle(Brushes.IndianRed, 1050, 917, 3 * power2, 17);
                        g.DrawString(power2.ToString(), f, Brushes.Yellow, 1233, 954);
                        g.DrawString(weapons[selectedweapon2], f, Brushes.Yellow, 586, 940);
                    }
                    //================================
                    //Info/Score
                    g.DrawString(player1, f2, Brushes.Yellow, 0, 0);
                    g.DrawString(player2, f2, Brushes.Yellow, this.ClientSize.Width - 50, 0);
                    g.DrawString("Round", f2, Brushes.WhiteSmoke, this.ClientSize.Width / 2 - 50, 0);
                    g.DrawString(Score1.ToString(), f2, Brushes.Yellow, 18, 15);
                    g.DrawString(Score2.ToString(), f2, Brushes.Yellow, this.ClientSize.Width - 28, 15);
                    g.DrawString(rounds.ToString(), f2, Brushes.WhiteSmoke, this.ClientSize.Width / 2 - 34, 15);
                    //======================================================================================
                    //Weapons
                    if (fired)
                    {
                        if (turns)
                        {
                            if (selectedweapon1 == 0)
                            {
                                g.FillEllipse(Brushes.Red, wx, wy, 5, 5);
                            }
                            else if (selectedweapon1 == 1)
                            {
                                if (!hit[0])
                                {
                                    g.FillEllipse(Brushes.Red, wx, wy, 5, 5);
                                }
                                if (!hit[1])
                                {
                                    g.FillEllipse(Brushes.Red, wx2, wy2, 5, 5);
                                }
                                if (!hit[2])
                                {
                                    g.FillEllipse(Brushes.Red, wx3, wy3, 5, 5);
                                }
                            }
                            else if (selectedweapon1 == 2)
                            {
                                if (!hit[0])
                                {
                                    g.FillEllipse(Brushes.Red, wx, wy, 5, 5);
                                }
                                if (!hit[1])
                                {
                                    g.FillEllipse(Brushes.Red, wx2, wy2, 5, 5);
                                }
                                if (!hit[2])
                                {
                                    g.FillEllipse(Brushes.Red, wx3, wy3, 5, 5);
                                }
                                if (!hit[3])
                                {
                                    g.FillEllipse(Brushes.Red, wx4, wy4, 5, 5);
                                }
                                if (!hit[4])
                                {
                                    g.FillEllipse(Brushes.Red, wx5, wy5, 5, 5);
                                }
                            }
                            else if (selectedweapon1 == 3)
                            {
                                if (!hit[0])
                                {
                                    g.FillEllipse(Brushes.Red, wx, wy, 5, 5);
                                }
                                if (scatter)
                                {
                                    if (!hit[1])
                                    {
                                        g.FillEllipse(Brushes.Yellow, wx2, wy2, 5, 5);
                                    }
                                    if (!hit[2])
                                    {
                                        g.FillEllipse(Brushes.Green, wx3, wy3, 5, 5);
                                    }
                                    if (!hit[3])
                                    {
                                        g.FillEllipse(Brushes.Blue, wx4, wy4, 5, 5);
                                    }
                                    if (!hit[4])
                                    {
                                        g.FillEllipse(Brushes.Violet, wx5, wy5, 5, 5);
                                    }
                                }
                            }
                            else if (selectedweapon1 == 4)
                            {
                                if (!spider)
                                {
                                    g.FillEllipse(Brushes.Red, wx, wy, 5, 5);
                                }
                                else
                                {
                                    for (int i = 0; i < spiders.Count; i += 2)
                                    {
                                        g.DrawLine(Pens.White, spiders[i].x, spiders[i].y, spiders[i + 1].x, spiders[i + 1].y);
                                    }
                                }
                            }
                            else if (selectedweapon1 == 5)
                            {
                                if (!pine)
                                {
                                    g.FillEllipse(Brushes.Red, wx, wy, 5, 5);
                                }
                                else if (secondpine)
                                {
                                    float theta = 0;
                                    float currX, currY;
                                    float th_in_radian;

                                    while (theta <= 360)
                                    {
                                        th_in_radian = (float)(theta * Math.PI / 180);

                                        currX = (float)(pineaplemainshot.Rad * Math.Cos(th_in_radian)) + pineaplemainshot.XC;
                                        currY = (float)(pineaplemainshot.Rad * Math.Sin(th_in_radian)) + pineaplemainshot.YC;
                                        if (!(currY > poi[(int)currX].y) && !ishit(currX, currY))
                                        {
                                            g.FillEllipse(Brushes.GreenYellow, currX, currY, 5, 5);
                                        }
                                        else
                                        {
                                            if (ishit(currX, currY))
                                            {
                                                calculatescore();
                                            }
                                            else
                                            {
                                                Circle pnn = new Circle();
                                                pnn.XC = currX;
                                                pnn.YC = currY;
                                                pnn.Rad = 20;
                                                Damage.Add(pnn);
                                                damageground();
                                            }
                                        }
                                        theta += 10;
                                    }
                                }
                                else if (thirdpine)
                                {
                                    for (int i = 0; i < pinaples.Count; i++)
                                    {
                                        g.FillEllipse(Brushes.GreenYellow, pinaples[i].x, pinaples[i].y, 5, 5);
                                    }
                                }
                            }
                        }
                        //====================================================================
                        else
                        {
                            if (selectedweapon2 == 0)
                            {
                                g.FillEllipse(Brushes.Red, wx, wy, 5, 5);
                            }
                            else if (selectedweapon2 == 1)
                            {
                                if (!hit[0])
                                {
                                    g.FillEllipse(Brushes.Red, wx, wy, 5, 5);
                                }
                                if (!hit[1])
                                {
                                    g.FillEllipse(Brushes.Red, wx2, wy2, 5, 5);
                                }
                                if (!hit[2])
                                {
                                    g.FillEllipse(Brushes.Red, wx3, wy3, 5, 5);
                                }
                            }
                            else if (selectedweapon2 == 2)
                            {
                                if (!hit[0])
                                {
                                    g.FillEllipse(Brushes.Red, wx, wy, 5, 5);
                                }
                                if (!hit[1])
                                {
                                    g.FillEllipse(Brushes.Red, wx2, wy2, 5, 5);
                                }
                                if (!hit[2])
                                {
                                    g.FillEllipse(Brushes.Red, wx3, wy3, 5, 5);
                                }
                                if (!hit[3])
                                {
                                    g.FillEllipse(Brushes.Red, wx4, wy4, 5, 5);
                                }
                                if (!hit[4])
                                {
                                    g.FillEllipse(Brushes.Red, wx5, wy5, 5, 5);
                                }
                            }
                            else if (selectedweapon2 == 3)
                            {
                                if (!hit[0])
                                {
                                    g.FillEllipse(Brushes.Red, wx, wy, 5, 5);
                                }
                                if (scatter)
                                {
                                    if (!hit[1])
                                    {
                                        g.FillEllipse(Brushes.Yellow, wx2, wy2, 5, 5);
                                    }
                                    if (!hit[2])
                                    {
                                        g.FillEllipse(Brushes.Green, wx3, wy3, 5, 5);
                                    }
                                    if (!hit[3])
                                    {
                                        g.FillEllipse(Brushes.Blue, wx4, wy4, 5, 5);
                                    }
                                    if (!hit[4])
                                    {
                                        g.FillEllipse(Brushes.Violet, wx5, wy5, 5, 5);
                                    }
                                }
                            }
                            else if (selectedweapon2 == 4)
                            {
                                if (!spider)
                                {
                                    g.FillEllipse(Brushes.Red, wx, wy, 5, 5);
                                }
                                else
                                {
                                    for (int i = 0; i < spiders.Count; i += 2)
                                    {
                                        g.DrawLine(Pens.White, spiders[i].x, spiders[i].y, spiders[i + 1].x, spiders[i + 1].y);
                                    }
                                }
                            }
                            else if (selectedweapon2 == 5)
                            {
                                if (!pine)
                                {
                                    g.FillEllipse(Brushes.Red, wx, wy, 5, 5);
                                }
                                else if (secondpine)
                                {
                                    float theta = 0;
                                    float currX, currY;
                                    float th_in_radian;

                                    while (theta <= 360)
                                    {
                                        th_in_radian = (float)(theta * Math.PI / 180);

                                        currX = (float)(pineaplemainshot.Rad * Math.Cos(th_in_radian)) + pineaplemainshot.XC;
                                        currY = (float)(pineaplemainshot.Rad * Math.Sin(th_in_radian)) + pineaplemainshot.YC;
                                        if (!(currY > poi[(int)currX].y))
                                        {
                                            g.FillEllipse(Brushes.GreenYellow, currX, currY, 5, 5);
                                        }
                                        else
                                        {
                                            if (ishit(currX, currY))
                                            {
                                                calculatescore();
                                            }
                                            else
                                            {
                                                Circle pnn = new Circle();
                                                pnn.XC = currX;
                                                pnn.YC = currY;
                                                pnn.Rad = 20;
                                                Damage.Add(pnn);
                                                damageground();
                                            }
                                        }
                                        theta += 10;
                                    }
                                }
                                else if (thirdpine)
                                {
                                    for (int i = 0; i < pinaples.Count; i++)
                                    {
                                        g.FillEllipse(Brushes.GreenYellow, pinaples[i].x, pinaples[i].y, 5, 5);
                                    }
                                }
                            }
                        }
                        for (int i = 0; i < elevatingscore.Count; i++)
                        {
                            g.DrawString(elevatingscore[i].scoreee, f2, Brushes.WhiteSmoke, elevatingscore[i].x, elevatingscore[i].y);
                        }
                        if (dodmage)
                        {
                            for (int i = 0; i < Damage.Count; i++)
                            {
                                Damage[i].DrawYourSelf(g);
                            }
                        }
                    }
                }
                else
                {
                    g.Clear(Color.Black);
                    g.DrawString("Please Enter Player1 name", f3, Brushes.White, this.ClientSize.Width / 2 - 400, (this.ClientSize.Height / 2)-200);
                    g.DrawString(player1, f3, Brushes.IndianRed, this.ClientSize.Width / 2-100 , (this.ClientSize.Height / 2)-100);
                    g.DrawString("Please Enter Player2 name", f3, Brushes.White, this.ClientSize.Width / 2 -400, (this.ClientSize.Height / 2));
                    g.DrawString(player2, f3, Brushes.DarkViolet, this.ClientSize.Width / 2-100 , (this.ClientSize.Height / 2)+100);
                }
            }
            //======================================================================================
            else
            {
                g.Clear(Color.FromArgb(13, 0, 26));
                g.DrawImage(Menus, 0, 0, this.ClientSize.Width, this.ClientSize.Height);
            }
        }
        //============================================================================================================================================
        //My Functions
        //tank angels
        float tankanglex(float angle, float rad, float xc)
        {
            float th_in_radian = (float)(angle * Math.PI / 180);
            float currx = (float)(rad * Math.Cos(th_in_radian)) + xc;
            return currx;
        }
        float tankangley(float angle, float rad, float yc)
        {
            float th_in_radian = (float)(angle * Math.PI / 180);
            float curry = (float)(rad * Math.Sin(th_in_radian)) + yc;
            return curry;
        }
        void moveright()
        {
            //tank1
            if (turns)
            {
                if (nomoves1 > 0)
                {
                    nomoves1--;
                    Stopwatch s = new Stopwatch();
                    s.Start();
                    while (s.Elapsed < TimeSpan.FromSeconds(3))
                    {
                        movingp1 += 2;
                        for (int i = 0; i < tank1.Count; i++)
                        {
                            if (i != 5)
                            {
                                tank1[i].PS.X = poi[movingp1 - 40].x;
                                tank1[i].PE.X = poi[movingp1].x;
                                tank1[i].PS.Y = poi[movingp1 - 40].y - 1 - i;
                                tank1[i].PE.Y = poi[movingp1].y - 1 - i;
                            }
                            else
                            {
                                tank1[i].PS.X = poi[movingp1 - 20].x;
                                tank1[i].PS.Y = poi[movingp1 - 20].y - 1 - i;
                            }
                        }
                        t1ax = tankanglex(angle1, tank1angle.Rad, tank1[5].PS.X);
                        t1ay = tankangley(angle1, tank1angle.Rad, tank1[5].PS.Y);
                        tank1[tank1.Count - 1].PE.X = t1ax;
                        tank1[tank1.Count - 1].PE.Y = t1ay;
                        DrawDubb(this.CreateGraphics());
                    }
                    s.Stop();
                }
            }
            //tank2
            else
            {
                if (nomoves2 > 0)
                {
                    nomoves2--;
                    Stopwatch s = new Stopwatch();
                    s.Start();
                    while (s.Elapsed < TimeSpan.FromSeconds(3))
                    {
                        movingp2 += 2;
                        for (int i = 0; i < tank2.Count; i++)
                        {
                            if (i != 5)
                            {
                                tank2[i].PS.X = poi[movingp2 - 40].x;
                                tank2[i].PE.X = poi[movingp2].x;
                                tank2[i].PS.Y = poi[movingp2 - 40].y - 1 - i;
                                tank2[i].PE.Y = poi[movingp2].y - 1 - i;
                            }
                            else
                            {
                                tank2[i].PS.X = poi[movingp2 - 20].x;
                                tank2[i].PS.Y = poi[movingp2 - 20].y - 1 - i;
                            }
                        }
                        t2ax = tankanglex(angle2, tank2angle.Rad, tank2[5].PS.X);
                        t2ay = tankangley(angle2, tank2angle.Rad, tank2[5].PS.Y);
                        tank2[tank2.Count - 1].PE.X = t2ax;
                        tank2[tank2.Count - 1].PE.Y = t2ay;
                        DrawDubb(this.CreateGraphics());
                    }
                    s.Stop();
                }
            }
        }
        void moveleft()
        {
            if (turns)
            {
                if (nomoves1 > 0)
                {
                    nomoves1--;
                    Stopwatch s = new Stopwatch();
                    s.Start();
                    while (s.Elapsed < TimeSpan.FromSeconds(3))
                    {
                        movingp1 -= 2;
                        for (int i = 0; i < tank1.Count; i++)
                        {
                            if (i != 5)
                            {
                                tank1[i].PS.X = poi[movingp1 - 40].x;
                                tank1[i].PE.X = poi[movingp1].x;
                                tank1[i].PS.Y = poi[movingp1 - 40].y - 1 - i;
                                tank1[i].PE.Y = poi[movingp1].y - 1 - i;
                            }
                            else
                            {
                                tank1[i].PS.X = poi[movingp1 - 20].x;
                                tank1[i].PS.Y = poi[movingp1 - 20].y - 1 - i;
                            }
                        }
                        t1ax = tankanglex(angle1, tank1angle.Rad, tank1[5].PS.X);
                        t1ay = tankangley(angle1, tank1angle.Rad, tank1[5].PS.Y);
                        tank1[tank1.Count - 1].PE.X = t1ax;
                        tank1[tank1.Count - 1].PE.Y = t1ay;
                        DrawDubb(this.CreateGraphics());
                    }
                    s.Stop();
                }
            }
            //tank2
            else
            {
                if (nomoves2 > 0)
                {
                    nomoves2--;
                    Stopwatch s = new Stopwatch();
                    s.Start();
                    while (s.Elapsed < TimeSpan.FromSeconds(3))
                    {
                        movingp2 -= 2;
                        for (int i = 0; i < tank2.Count; i++)
                        {
                            if (i != 5)
                            {
                                tank2[i].PS.X = poi[movingp2 - 40].x;
                                tank2[i].PE.X = poi[movingp2].x;
                                tank2[i].PS.Y = poi[movingp2 - 40].y - 1 - i;
                                tank2[i].PE.Y = poi[movingp2].y - 1 - i;
                            }
                            else
                            {
                                tank2[i].PS.X = poi[movingp2 - 20].x;
                                tank2[i].PS.Y = poi[movingp2 - 20].y - 1 - i;
                            }
                        }
                        t2ax = tankanglex(angle2, tank2angle.Rad, tank2[5].PS.X);
                        t2ay = tankangley(angle2, tank2angle.Rad, tank2[5].PS.Y);
                        tank2[tank2.Count - 1].PE.X = t2ax;
                        tank2[tank2.Count - 1].PE.Y = t2ay;
                        DrawDubb(this.CreateGraphics());
                    }
                    s.Stop();
                }
            }
        }
        void oneshot(int power, float angle, float x, float y)
        {
            wx = x;
            wy = y;
            float thetainrad = (float)(angle * Math.PI / 180);
            facx = (float)((power * Math.Cos(thetainrad)) / 5.3f);
            facy = (float)(power * Math.Sin(thetainrad) / 5.3f);
            dis = tank1[5].PS.Y - 0 / power;
            while (fired)
            {
                wx += 2 * facx;
                wy += facy;
                if (wy < Math.Abs(dis / 2) || wy < 0)
                {
                    facy += 1;
                }
                if (ishit(wx,wy))
                {
                    calculatescore();
                    while (elevatingscore.Count>0)
                    {
                        movescore();
                        DrawDubb(this.CreateGraphics());
                    }
                    fired = false;
                    turns = !turns;
                    break;
                }
                if (wx < 1 || wx > this.ClientSize.Width - 2 || wy > poi[(int)wx].y)
                {
                    Circle pnn = new Circle();
                    pnn.XC = wx;
                    pnn.YC = wy;
                    pnn.Rad = 20;
                    Damage.Add(pnn);
                    damageground();
                    fired = false;
                    turns = !turns;
                }
                DrawDubb(this.CreateGraphics());
            }
        }
        void threeshots(int power,float angle,float x, float y)
        {
            wx = x;
            wy = y;
            float thetainrad = (float)(angle * Math.PI / 180);
            facx = (float)((power * Math.Cos(thetainrad)) / 5.3f);
            facy = (float)(power * Math.Sin(thetainrad) / 5.3f);
            wx2 = x;
            wy2 = y;
            facx2 = (float)((power * Math.Cos(thetainrad)) / 4.3f);
            facy2 = (float)(power * Math.Sin(thetainrad) / 4.3f);
            wx3 = x;
            wy3 = y;
            facx3 = (float)((power * Math.Cos(thetainrad)) / 3.3f);
            facy3 = (float)(power * Math.Sin(thetainrad) / 3.3f);
            dis = tank1[5].PS.Y - 0 / power;
            while (fired)
            {
                wx += 2 * facx;
                wy += facy;
                wx2 += 2 * facx2;
                wy2 += facy2;
                wx3 += 2 * facx3;
                wy3 += facy3;
                if (wy < Math.Abs(dis / 2) || wy < 0)
                {
                    facy += 1;
                }
                if (wy2 < Math.Abs(dis / 2) || wy2 < 0)
                {
                    facy2 += 1;
                }
                if (wy3 < Math.Abs(dis / 2) || wy3 < 0)
                {
                    facy3 += 1;
                }
                if (!hit[0] && (wx < 1 || wx > this.ClientSize.Width - 2 || wy > poi[(int)wx].y || ishit(wx, wy)))
                {
                    hit[0] = true;
                    if (ishit(wx, wy))
                    {
                        calculatescore();
                    }
                    else
                    {
                        Circle pnn = new Circle();
                        pnn.XC = wx;
                        pnn.YC = wy;
                        pnn.Rad = 20;
                        Damage.Add(pnn);
                        damageground();
                    }
                }
                if (!hit[1] && (wx2 < 1 || wx2 > this.ClientSize.Width - 2 || wy2 > poi[(int)wx2].y || ishit(wx2, wy2)))
                {
                    hit[1] = true;
                    if (ishit(wx2, wy2))
                    {
                        calculatescore();
                    }
                    else
                    {
                        Circle pnn = new Circle();
                        pnn.XC = wx2;
                        pnn.YC = wy2;
                        pnn.Rad = 20;
                        Damage.Add(pnn);
                        damageground();
                    }
                }
                if (!hit[2] && (wx3 < 1 || wx3 > this.ClientSize.Width - 2 || wy3 > poi[(int)wx3].y || ishit(wx3, wy3)))
                {
                    hit[2] = true;
                    if (ishit(wx3, wy3))
                    {
                        calculatescore();
                    }
                    else
                    {
                        Circle pnn = new Circle();
                        pnn.XC = wx3;
                        pnn.YC = wy3;
                        pnn.Rad = 20;
                        Damage.Add(pnn);
                        damageground();
                    }
                }
                if (hit[0] && hit[1] && hit[2])
                {
                    for (int i = 0; i < 5;i++)
                    {
                        hit[i] = false;
                    }
                    while(elevatingscore.Count>0)
                    {
                        movescore();
                        DrawDubb(this.CreateGraphics());
                    }
                    fired = false;
                    turns = !turns;
                }
                movescore();
                DrawDubb(this.CreateGraphics());
            }
        }
        void fiveshots(int power, float angle, float x, float y)
        {
            wx = x;
            wy = y;
            float thetainrad = (float)(angle * Math.PI / 180);
            facx = (float)((power * Math.Cos(thetainrad)) / 7.3f);
            facy = (float)(power * Math.Sin(thetainrad) / 7.3f);
            wx2 = x;
            wy2 = y;
            facx2 = (float)((power * Math.Cos(thetainrad)) / 6.3f);
            facy2 = (float)(power * Math.Sin(thetainrad) / 6.3f);
            wx3 = x;
            wy3 = y;
            facx3 = (float)((power * Math.Cos(thetainrad)) / 5.3f);
            facy3 = (float)(power * Math.Sin(thetainrad) / 5.3f);
            wx4 = x;
            wy4 = y;
            facx4 = (float)((power * Math.Cos(thetainrad)) / 4.3f);
            facy4 = (float)(power * Math.Sin(thetainrad) / 4.3f);
            wx5 = x;
            wy5 = y;
            facx5 = (float)((power * Math.Cos(thetainrad)) / 3.3f);
            facy5 = (float)(power * Math.Sin(thetainrad) / 3.3f);
            dis = tank1[5].PS.Y - 0 / power;
            while (fired)
            {
                wx += 2 * facx;
                wy += facy;
                wx2 += 2 * facx2;
                wy2 += facy2;
                wx3 += 2 * facx3;
                wy3 += facy3;
                wx4 += 2 * facx4;
                wy4 += facy4;
                wx5 += 2 * facx5;
                wy5 += facy5;
                if (wy < Math.Abs(dis / 2) || wy < 0)
                {
                    facy += 1;
                }
                if (wy2 < Math.Abs(dis / 2) || wy2 < 0)
                {
                    facy2 += 1;
                }
                if (wy3 < Math.Abs(dis / 2) || wy3 < 0)
                {
                    facy3 += 1;
                }
                if (wy4 < Math.Abs(dis / 2) || wy4 < 0)
                {
                    facy4 += 1;
                }
                if (wy5 < Math.Abs(dis / 2) || wy5 < 0)
                {
                    facy5 += 1;
                }
                if (!hit[0] && (wx < 1 || wx > this.ClientSize.Width - 2 || wy > poi[(int)wx].y || ishit(wx, wy)))
                {
                    hit[0] = true;
                    if (ishit(wx, wy))
                    {
                        calculatescore();
                    }
                    else
                    {
                        Circle pnn = new Circle();
                        pnn.XC = wx;
                        pnn.YC = wy;
                        pnn.Rad = 20;
                        Damage.Add(pnn);
                        damageground();
                    }
                }
                if (!hit[1] && (wx2 < 1 || wx2 > this.ClientSize.Width - 2 || wy2 > poi[(int)wx2].y || ishit(wx2, wy2)))
                {
                    hit[1] = true;
                    if (ishit(wx2, wy2))
                    {
                        calculatescore();
                    }
                    else
                    {
                        Circle pnn = new Circle();
                        pnn.XC = wx2;
                        pnn.YC = wy2;
                        pnn.Rad = 20;
                        Damage.Add(pnn);
                        damageground();
                    }
                }
                if (!hit[2] && (wx3 < 1 || wx3 > this.ClientSize.Width - 2 || wy3 > poi[(int)wx3].y || ishit(wx3, wy3)))
                {
                    hit[2] = true;
                    if (ishit(wx3, wy3))
                    {
                        calculatescore();
                    }
                    else
                    {
                        Circle pnn = new Circle();
                        pnn.XC = wx3;
                        pnn.YC = wy3;
                        pnn.Rad = 20;
                        Damage.Add(pnn);
                        damageground();
                    }
                }
                if (!hit[3] && (wx4 < 1 || wx4 > this.ClientSize.Width - 2 || wy4 > poi[(int)wx4].y || ishit(wx4, wy4)))
                {
                    hit[3] = true;
                    if (ishit(wx4, wy4))
                    {
                        calculatescore();
                    }
                    else
                    {
                        Circle pnn = new Circle();
                        pnn.XC = wx4;
                        pnn.YC = wy4;
                        pnn.Rad = 20;
                        Damage.Add(pnn);
                        damageground();
                    }
                }
                if (!hit[4] && (wx5 < 1 || wx4 > this.ClientSize.Width - 2 || wy5 > poi[(int)wx5].y || ishit(wx5, wy5)))
                {
                    hit[4] = true;
                    if (ishit(wx5, wy5))
                    {
                        calculatescore();
                    }
                    else
                    {
                        Circle pnn = new Circle();
                        pnn.XC = wx5;
                        pnn.YC = wy5;
                        pnn.Rad = 20;
                        Damage.Add(pnn);
                        damageground();
                    }
                }
                if (hit[0] && hit[1] && hit[2] && hit[3] && hit[4])
                {
                    for (int i = 0; i < 5; i++)
                    {
                        hit[i] = false;
                    }
                    while (elevatingscore.Count > 0)
                    {
                        movescore();
                        DrawDubb(this.CreateGraphics());
                    }
                    fired = false;
                    turns = !turns;
                }
                movescore();
                DrawDubb(this.CreateGraphics());
            }
        }
        void scattershot(int power, float angle, float x, float y)
        {
            wx = x;
            wy = y;
            float thetainrad = (float)(angle * Math.PI / 180);
            facx = (float)((power * Math.Cos(thetainrad)) / 5.3f);
            facy = (float)(power * Math.Sin(thetainrad) / 5.3f);
            dis = tank1[5].PS.Y - 0 / power;
            while (fired)
            {
                wx += 2 * facx;
                wy += facy;
                wx2 += 2 * facx2;
                wy2 += facy2;
                wx3 += 2 * facx3;
                wy3 += facy3;
                if (wy < Math.Abs(dis / 2) || wy < 0)
                {
                    facy += 1;
                }
                if (wx < 1 || wx > this.ClientSize.Width - 2)
                {
                    fired = false;
                    turns = !turns;
                }
                else if (wy > poi[(int)wx].y-2)
                {
                    scatter = true;
                    completescatter(power, angle, wx, wy);
                }
                DrawDubb(this.CreateGraphics());
            }
        }
        void completescatter(int power, float angle, float x, float y)
        {
            wx = x;
            wy = y;
            wx2 = x;
            wy2 = y;
            wx3 = x;
            wy3 = y;
            wx4 = x;
            wy4 = y;
            wx5 = x;
            wy5 = y;
            float thetainrad = (float)(30 * Math.PI / 180);
            facx2 = (float)((10 * Math.Cos(thetainrad)) / 5.3f);
            facy2 = (float)-(10 * Math.Sin(thetainrad) / 5.3f);
            thetainrad = (float)(20 * Math.PI / 180);
            facx3 = (float)((10 * Math.Cos(thetainrad)) / 5.3f);
            facy3 = (float)-(10 * Math.Sin(thetainrad) / 5.3f);
            thetainrad = (float)(-30 * Math.PI / 180);
            facx4 = (float)-((10 * Math.Cos(thetainrad)) / 5.3f);
            facy4 = (float)(10 * Math.Sin(thetainrad) / 5.3f);
            thetainrad = (float)(-20 * Math.PI / 180);
            facx5 = (float)-((10 * Math.Cos(thetainrad)) / 5.3f);
            facy5 = (float)(10 * Math.Sin(thetainrad) / 5.3f);
            int k = 3;
            while (fired)
            {
                wy -= k;
                wx2 += facx2;
                wy2 += 3*facy2;
                wx3 += 2*facx3;
                wy3 += 3 * facy3;
                wx4 += facx4;
                wy4 += 3 * facy4;
                wx5 += 2 * facx5;
                wy5 += 3 * facy5;
                if (wy < poi[(int)wx].y - 50)
                {
                    k = -3;
                }
                if (wy2 < poi[(int)wx2].y - 50 || wy2 < 0)
                {
                    facy2 += 1;
                }
                if (wy3 < poi[(int)wx3].y - 50 || wy3 < 0)
                {
                    facy3 += 1;
                }
                if (wy4 < poi[(int)wx4].y - 50 || wy4 < 0)
                {
                    facy4 += 1;
                }
                if (wy5 < poi[(int)wx5].y - 50 || wy5 < 0)
                {
                    facy5 += 1;
                }
                if (!hit[0] && (wx < 1 || wx > this.ClientSize.Width - 2 || wy > poi[(int)wx].y || ishit(wx, wy)))
                {
                    hit[0] = true;
                    if (ishit(wx, wy))
                    {
                        calculatescore();
                    }
                    else
                    {
                        Circle pnn = new Circle();
                        pnn.XC = wx;
                        pnn.YC = wy;
                        pnn.Rad = 20;
                        Damage.Add(pnn);
                        damageground();
                    }
                }
                if (!hit[1] && (wx2 < 1 || wx2 > this.ClientSize.Width - 2 || wy2 > poi[(int)wx2].y || ishit(wx2, wy2)))
                {
                    hit[1] = true;
                    if (ishit(wx2, wy2))
                    {
                        calculatescore();
                    }
                    else
                    {
                        Circle pnn = new Circle();
                        pnn.XC = wx2;
                        pnn.YC = wy2;
                        pnn.Rad = 20;
                        Damage.Add(pnn);
                        damageground();
                    }
                }
                if (!hit[2] && (wx3 < 1 || wx3 > this.ClientSize.Width - 2 || wy3 > poi[(int)wx3].y + 2 || ishit(wx3, wy3)))
                {
                    hit[2] = true;
                    if (ishit(wx3, wy3))
                    {
                        calculatescore();
                    }
                    else
                    {
                        Circle pnn = new Circle();
                        pnn.XC = wx3;
                        pnn.YC = wy3;
                        pnn.Rad = 20;
                        Damage.Add(pnn);
                        damageground();
                    }
                }
                if (!hit[3] && (wx4 < 1 || wx4 > this.ClientSize.Width - 2 || wy4 > poi[(int)wx4].y || ishit(wx4, wy4)))
                {
                    hit[3] = true;
                    if (ishit(wx4, wy4))
                    {
                        calculatescore();
                    }
                    else
                    {
                        Circle pnn = new Circle();
                        pnn.XC = wx4;
                        pnn.YC = wy4;
                        pnn.Rad = 20;
                        Damage.Add(pnn);
                        damageground();
                    }
                }
                if (!hit[4] && (wx5 < 1 || wx4 > this.ClientSize.Width - 2 || wy5 > poi[(int)wx5].y + 2 || ishit(wx5, wy5)))
                {
                    hit[4] = true;
                    if (ishit(wx5, wy5))
                    {
                        calculatescore();
                    }
                    else
                    {
                        Circle pnn = new Circle();
                        pnn.XC = wx5;
                        pnn.YC = wy5;
                        pnn.Rad = 20;
                        Damage.Add(pnn);
                        damageground();
                    }
                }
                if (hit[0] && hit[1] && hit[2] && hit[3] && hit[4])
                {
                    for (int i = 0; i < 5; i++)
                    {
                        hit[i] = false;
                    }
                    while (elevatingscore.Count > 0)
                    {
                        movescore();
                        DrawDubb(this.CreateGraphics());
                    }
                    fired = false;
                    turns = !turns;
                    scatter = false;
                }
                movescore();
                DrawDubb(this.CreateGraphics());
            }
        }
        void spidershot(int power, float angle, float x, float y)
        {
            wx = x;
            wy = y;
            float thetainrad = (float)(angle * Math.PI / 180);
            facx = (float)((power * Math.Cos(thetainrad)) / 5.3f);
            facy = (float)(power * Math.Sin(thetainrad) / 5.3f);
            dis = tank1[5].PS.Y - 0 / power;
            while (fired)
            {
                wx += 2 * facx;
                wy += facy;
                if (wy < Math.Abs(dis / 2) || wy < 0)
                {
                    facy += 1;
                }
                if (wx < 1 || wx > this.ClientSize.Width - 2)
                {
                    fired = false;
                    turns = !turns;
                }
                if (turns)
                {
                    if ((wx < (tank2[0].PS.X)) && (wx > (tank2[0].PS.X - 100)))
                    {
                        spider = true;
                        completespider(power, angle, wx, wy);
                    }
                }
                else
                {
                    if ((wx > (tank1[0].PE.X)) && (wx > (tank1[0].PE.X - 100)))
                    {
                        spider = true;
                        completespider(power, angle, wx, wy);
                    }
                }
                DrawDubb(this.CreateGraphics());
            }
        }
        void completespider(int power, float angle, float x, float y)
        {
            for (int i = 0; i < 20;i++)
            {
                pointsz pnn = new pointsz();
                pnn.x = (int)wx;
                pnn.y = (int)wy;
                spiders.Add(pnn);
                pointsz pnn2 = new pointsz();
                pnn2.x = (int)(wx + (5 * facx) + (5*i));
                pnn2.y = (int)(wy + (5 * facy));
                spiders.Add(pnn2);
            }
            while (fired)
            {
                for (int i = 0; i < spiders.Count; i+=2)
                {
                    spiders[i].x += 1+i;
                    spiders[i].y += (int)(0.5f * facy);
                    spiders[i + 1].x += 1+i;
                    spiders[i + 1].y += (int)(0.5f * facy);
                    if ((spiders[i + 1].x < 2) || (spiders[i + 1].x > this.ClientSize.Width - 2) || (spiders[i + 1].y > poi[(int)spiders[i + 1].x].y) || ishit(spiders[i + 1].x, spiders[i + 1].y))
                    {
                        if (ishit(spiders[i + 1].x, spiders[i + 1].y))
                        {
                            calculatescore();
                        }
                        else
                        {
                            Circle pnn = new Circle();
                            pnn.XC = spiders[i + 1].x;
                            pnn.YC = spiders[i + 1].y;
                            pnn.Rad = 10;
                            Damage.Add(pnn);
                            damageground();
                        }
                        spiders.RemoveAt(i);
                        spiders.RemoveAt(i);
                    }
                }
                if (spiders.Count==0)
                {
                    while (elevatingscore.Count > 0)
                    {
                        movescore();
                    }
                    fired = false;
                    turns = !turns;
                    spider = false;
                }
                movescore();
                DrawDubb(this.CreateGraphics());
            }
        }
        void pineshot(int power, float angle, float x, float y)
        {
            wx = x;
            wy = y;
            float thetainrad = (float)(angle * Math.PI / 180);
            facx = (float)((power * Math.Cos(thetainrad)) / 5.3f);
            facy = (float)(power * Math.Sin(thetainrad) / 5.3f);
            dis = tank1[5].PS.Y - 0 / power;
            while (fired)
            {
                wx += 2 * facx;
                wy += facy;
                if (wy < Math.Abs(dis / 2) || wy < 0)
                {
                    facy += 1;
                }
                if (wx < 1 || wx > this.ClientSize.Width - 2)
                {
                    fired = false;
                    turns = !turns;
                }
                if (turns)
                {
                    if ((wx < (tank2[0].PS.X)) && (wx > (tank2[0].PS.X - 50)))
                    {
                        pine = true;
                        secondpine = true;
                        completepine(power, angle, wx, wy);
                    }
                }
                else
                {
                    if ((wx > (tank1[0].PE.X)) && (wx > (tank1[0].PE.X - 50)))
                    {
                        pine = true;
                        secondpine = true;
                        completepine(power, angle, wx, wy);
                    }
                }
                DrawDubb(this.CreateGraphics());
            }
        }
        void completepine(int power, float angle, float x, float y)
        {
            pineaplemainshot.XC = x;
            pineaplemainshot.YC = y;
            pineaplemainshot.Rad = 10;
            while (pineaplemainshot.Rad < 100)
            {
                pineaplemainshot.Rad += 5;
                if (pineaplemainshot.Rad > 95)
                {
                    float theta = 0;
                    float currX, currY;
                    float th_in_radian;

                    while (theta <= 360)
                    {
                        th_in_radian = (float)(theta * Math.PI / 180);

                        currX = (float)(pineaplemainshot.Rad * Math.Cos(th_in_radian)) + pineaplemainshot.XC;
                        currY = (float)(pineaplemainshot.Rad * Math.Sin(th_in_radian)) + pineaplemainshot.YC;
                        if (!(currY > poi[(int)currX].y))
                        {
                            pointsz pnn = new pointsz();
                            pnn.x = (int)currX;
                            pnn.y = (int)currY;
                            pnn.thrad = th_in_radian;
                            pinaples.Add(pnn);
                        }
                        theta += 10;
                    }
                    secondpine = false;
                    thirdpine = true;
                }
                DrawDubb(this.CreateGraphics());
            }
            if (thirdpine)
            {
                while (fired)
                {
                    for (int i = 0; i < pinaples.Count; i++)
                    {
                        /*facx2 = (float)((10 * Math.Cos(pinaples[i].thrad)) / 5.3f);
                        facy2 = (float)-(10 * Math.Sin(pinaples[i].thrad) / 5.3f);
                        pinaples[i].x += facx2;
                        if (facy2 > 0)
                        {
                            pinaples[i].y += facy2;
                        }
                        else
                        {
                            pinaples[i].y -= facy2;
                        }*/
                        facx2 = (float)((3 * Math.Cos(pinaples[i].thrad)) / 5.3f);
                        pinaples[i].x += facx2;
                        pinaples[i].y+=2;
                        if (pinaples[i].y > poi[(int)pinaples[i].x].y || ishit(pinaples[i].x, pinaples[i].y))
                        {
                            if (ishit(pinaples[i].x, pinaples[i].y))
                            {
                                calculatescore();
                            }
                            else
                            {
                                Circle pnn = new Circle();
                                pnn.XC = pinaples[i].x;
                                pnn.YC = pinaples[i].y;
                                pnn.Rad = 20;
                                Damage.Add(pnn);
                                damageground();
                            }
                            pinaples.RemoveAt(i);
                        }
                    }
                    if (pinaples.Count == 0)
                    {
                        while(elevatingscore.Count>0)
                        {
                            movescore();
                        }
                        fired = false;
                        turns = !turns;
                        pine = false;
                        thirdpine = false;
                    }
                    movescore();
                    DrawDubb(this.CreateGraphics());
                }
            }
        }
        bool ishit(float wx,float wy)
        {
            if (turns)
            {
                if ((wx > tank2[0].PS.X) && (wx < tank2[0].PE.X) &&
                    (wy > tank2[5].PE.Y) && (wy < tank2[0].PS.Y))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if ((wx > tank1[0].PS.X) && (wx < tank1[0].PE.X) &&
                    (wy > tank1[5].PE.Y) && (wy < tank1[0].PS.Y))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        void calculatescore()
        {
            if (turns)
            {
                elevating pnn = new elevating();
                pnn.scoreee = "10";
                pnn.x = tank2[0].PS.X + 10;
                pnn.y = tank2[4].PS.Y - 10;
                elevatingscore.Add(pnn);
                Score1 += 10;
            }
            else
            {
                elevating pnn = new elevating();
                pnn.scoreee = "10";
                pnn.x = tank1[0].PS.X + 10;
                pnn.y = tank1[4].PS.Y - 10;
                elevatingscore.Add(pnn);
                Score2 += 10;
            }
        }
        void movescore()
        {
            for (int i=0;i<elevatingscore.Count;i++)
            {
                elevatingscore[i].y -= 5+(1*i);
                if (elevatingscore[i].y<5)
                {
                    elevatingscore.RemoveAt(i);
                }
            }
        }
        void damageground()
        {
            for (int i=0;i<Damage.Count;i++)
            {
                float theta = 0;
                float currX, currY;
                float th_in_radian;
                while (theta <= 180)
                {
                    th_in_radian = (float)(theta * Math.PI / 180);
                    currX = (float)(Damage[i].Rad * Math.Cos(th_in_radian)) + Damage[i].XC;
                    currY = (float)(Damage[i].Rad * Math.Sin(th_in_radian)) + Damage[i].YC;
                    if (currX < poi.Count && currX > 1)
                    {
                        if (poi[(int)currX].y < currY)
                        {
                            poi[(int)currX].y = (int)currY;
                        }
                    }
                    theta += 0.5f;
                }
                tank2angle.XC = tank2[0].PS.X + 20;
                tank2angle.YC = poi[(int)tank2[0].PS.X].y - 4;
                t2ax = tankanglex(angle2, tank2angle.Rad, tank2angle.XC);
                t2ay = tankangley(angle2, tank2angle.Rad, tank2angle.YC);
                for (int z = 0; z < tank2.Count; z++)
                {
                    if (z == 5)
                    {
                        tank2[z].PS.X = tank2angle.XC;
                        tank2[z].PS.Y = tank2angle.YC;
                        tank2[z].PE.X = t2ax;
                        tank2[z].PE.Y = t2ay;
                    }
                    else
                    {
                        tank2[z].PS.X = tank2[z].PS.X;
                        tank2[z].PS.Y = poi[(int)tank2[z].PS.X].y - 3 - z;
                        tank2[z].PE.X = tank2[z].PE.X;
                        tank2[z].PE.Y = poi[(int)tank2[z].PE.X].y - 3 - z;
                    }
                }
                tank1angle.XC = tank1[0].PS.X + 20;
                tank1angle.YC = poi[(int)tank1[0].PS.X].y - 4;
                t1ax = tankanglex(angle1, tank1angle.Rad, tank1angle.XC);
                t1ay = tankangley(angle1, tank1angle.Rad, tank1angle.YC);
                for (int z = 0; z < tank1.Count; z++)
                {
                    if (z == 5)
                    {
                        tank1[z].PS.X = tank1angle.XC;
                        tank1[z].PS.Y = tank1angle.YC;
                        tank1[z].PE.X = t1ax;
                        tank1[z].PE.Y = t1ay;
                    }
                    else
                    {
                        tank1[z].PS.X = tank1[z].PS.X;
                        tank1[z].PS.Y = poi[(int)tank1[z].PS.X].y - 3 - z;
                        tank1[z].PE.X = tank1[z].PE.X;
                        tank1[z].PE.Y = poi[(int)tank1[z].PE.X].y - 3 - z;
                    }
                }
                ground.Dispose();
                ground = new Bitmap(ClientSize.Width, 765);
                Graphics g2 = Graphics.FromImage(ground);
                for (int z = 0; z < this.ClientSize.Width; z++)
                {
                    g2.DrawLine(p, z, 765, z, poi[z].y);
                }
            }
            for (int i = 0; i < Damage.Count; i++)
            {
                Damage.RemoveAt(i);
            }
        }
    }
}
