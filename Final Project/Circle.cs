using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Final_Project
{
    class Circle
    {
            public int Rad;
            public float XC, YC;
            public float thStart, thEnd;
            public int triggered;
            public float slope;
            public float dx, dy;


            public void DrawYourArc(Graphics g)
            {
                float theta = thStart;
                float currX, currY;
                float th_in_radian;

                while (theta <= thEnd)
                {
                    th_in_radian = (float)(theta * Math.PI / 180);

                    currX = (float)(Rad * Math.Cos(th_in_radian)) + XC;
                    currY = (float)(Rad * Math.Sin(th_in_radian)) + YC;

                    if (theta == thStart || theta == thEnd)
                        g.DrawLine(Pens.Blue, XC, YC, currX, currY);

                    //g.DrawImage(img, currX, currY);
                    g.FillEllipse(Brushes.Yellow, currX, currY, 2, 2);
                    theta += 1;
                }
            }
            public void DrawYourSelf(Graphics g)
            {
                float theta = 0;
                float currX, currY;
                float th_in_radian;

                while (theta <= 360)
                {
                    th_in_radian = (float)(theta * Math.PI / 180);

                    currX = (float)(Rad * Math.Cos(th_in_radian)) + XC;
                    currY = (float)(Rad * Math.Sin(th_in_radian)) + YC;

                    //g.DrawImage(img, currX, currY);
                    g.FillEllipse(Brushes.Yellow, currX, currY, 5, 5);
                    theta += 1;
                }
            }
            public void DrawYourSelfpineaple(Graphics g)
            {
                float theta = 0;
                float currX, currY;
                float th_in_radian;

                while (theta <= 360)
                {
                    th_in_radian = (float)(theta * Math.PI / 180);

                    currX = (float)(Rad * Math.Cos(th_in_radian)) + XC;
                    currY = (float)(Rad * Math.Sin(th_in_radian)) + YC;
                    
                    g.FillEllipse(Brushes.GreenYellow, currX, currY, 5, 5);
                    theta += 10;
                }
            }
    }
}
