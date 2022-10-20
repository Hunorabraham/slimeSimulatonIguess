using System;
using System.Threading;

namespace stuff
{
    class particle
    {
        public double[] position = new double[2];
        public double speed = 0;
        public double angle;
        public particle(double posx, double posy, double speedx, double angley)
        {
            position[0] = posx;
            position[1] = posy;
            speed = speedx;
            angle = angley;
        }
        public particle(double[] pos, double sped, double ang)
        {
            position = pos;
            speed = sped;
            angle = ang;
        }
    }
    class Program
    {
        static int s = 20, amount = 10;
        static ConsoleColor[,] buffer = new ConsoleColor[s, s];
        static double distance = 2, rotspeed = Math.PI/18;
        static void Main(string[] args)
        {
            Console.Title = "Suffering of the children of Anyád";
            start();

            Random rnd = new Random();
            particle[] parts = new particle[amount];

            for (int i = 0; i < amount; i++)
            {
                parts[i] = new particle(s * (rnd.NextDouble() - 0.5), s * (rnd.NextDouble() - 0.5),1,(rnd.NextDouble()-0.5)*-2*Math.PI);
            }

            int a = 0;
            while (true)
            {
                parts = rotate(parts);
                for (int i = 0; i < amount; i++)
                {
                    parts[i] = updateParticle(parts[i]);
                }
                buffer = updateBuffer(buffer,parts);
                Console.SetCursorPosition(0, 0);
                for (int i = 0; i < s; i++)
                {
                    for (int j = 0; j < s; j++)
                    {
                        Console.ForegroundColor = buffer[i,j];
                        Console.Write("*");
                    }
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("aaa\n");
                }
                Console.Write($"Wtf{a}");
                a++;
            }
        }

        static ConsoleColor[,] updateBuffer(ConsoleColor[,] prev,particle[] parts)
        {
            ConsoleColor[,] fresh = prev;
            double avg = 0;
            //blur
            /*
            for(int i = 0; i < s; i++)
            {
                for(int j = 0; j < s; j++)
                {
                    double amt = 9;
                    //inside
                    for(int k = -1; k < 2; k++)
                    {
                        for (int l = -1; l < 2; l++)
                        {
                            try
                            {
                                switch (prev[i + k, j + l])
                                {
                                    case ConsoleColor.White:
                                        avg += 3;
                                        break;
                                    case ConsoleColor.Gray:
                                        avg += 2;
                                        break;
                                    case ConsoleColor.DarkGray:
                                        avg += 1;
                                        break;
                                    case ConsoleColor.Black:
                                        break;
                                }
                            }
                            catch
                            {
                                amt -= 1;
                            }
                        }
                    }
                    avg /= amt;
                    switch (Math.Round(avg+0.5))
                    {
                        case 3:
                            fresh[i, j] = ConsoleColor.White;
                            break;
                        case 2:
                            fresh[i, j] = ConsoleColor.Gray;
                            break;
                        case 1:
                            fresh[i, j] = ConsoleColor.DarkGray;
                            break;
                        case 0:
                            fresh[i, j] = ConsoleColor.Black;
                            break;
                    }
                }
            }*/
            //decipate
            
            for (int i = 0; i < s; i++)
            {
                for (int j = 0; j < s; j++)
                {
                    switch (fresh[i, j])
                    {
                        case ConsoleColor.White:
                            fresh[i, j] = ConsoleColor.Gray;
                            break;
                        case ConsoleColor.Gray:
                            fresh[i, j] = ConsoleColor.DarkGray;
                            break;
                        default:
                            fresh[i, j] = ConsoleColor.Black;
                            break;
                    }
                }
            }
            //add new
            for (int i = 0; i < amount; i++)
            {
                fresh[Convert.ToInt32(Math.Round(parts[i].position[0])), Convert.ToInt32(Math.Round(parts[i].position[1]))] = ConsoleColor.White;
            }

            //return
            return fresh;
        }
        static particle updateParticle(particle prev)
        {
            prev.position[0] += prev.speed * Math.Sin(prev.angle);
            prev.position[1] += prev.speed * Math.Cos(prev.angle);

            if (prev.position[0] > s - 1)
            {
                prev.angle *= -1;
                prev.position[0] = (s - 1) * 2 - prev.position[0];
            }
            if (prev.position[0] < 0)
            {
                prev.angle *= -1;
                prev.position[0] *= -1;
            }
            if (prev.position[1] > s - 1)
            {
                prev.angle += ((1 / prev.angle) * Math.Abs(prev.angle)) * Math.Abs((Math.PI/2 - Math.Abs(prev.angle)) * 2);
                prev.position[1] = (s - 1) * 2 - prev.position[1];
            }
            if (prev.position[1] < 0)
            {
                prev.angle -= ((1 / prev.angle) * Math.Abs(prev.angle)) * Math.Abs((Math.PI/2 - Math.Abs(prev.angle)) * 2);

                prev.position[1] *= -1;
            }
            return prev;
        }
        static particle rot(particle raw)
        {
            raw.angle += rotspeed;
            return raw;
        }
        static particle[] rotate(particle[] group)
        {
            particle[] newgroup = group;

            for(int i = 0; i < group.Length; i++)
            {
                double vmean = 0, hmean = 0;
                for(int j = 0; j < group.Length; j++)
                {
                    vmean += ness((group[j].position[0] - newgroup[i].position[0]) * Math.Round(distance / (group[j].position[0] - newgroup[i].position[0])));
                    hmean += ness((group[j].position[1] - newgroup[i].position[1]) * Math.Round(distance / (group[j].position[1] - newgroup[i].position[1])));


                }
                newgroup[i].angle -= rotspeed * ness(vmean);
                newgroup[i].angle += rotspeed * ness(vmean)*ness(newgroup[i].angle);
            }
            return newgroup;
        }
        static void start()
        {
            Console.CursorVisible = false;
        }
        static double ness(double raw)
        {
            if(raw < 0){
                raw = -1;
            }
            else if(raw > 0){
                raw = 1;
            }
            else{
                raw = 0;
            }
            return raw;
        }
    }
}
