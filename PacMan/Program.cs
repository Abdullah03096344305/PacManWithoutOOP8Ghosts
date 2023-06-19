using System;
using System.Threading;
using EZInput;
using System.IO;

namespace PacMan
{
    class Program
    {
        static int score = 0;
        static void Main(string[] args)
        {
            // PacMan Coordinates
            int pacmanX = 9;
            int pacmanY = 31;

            // Ghost 1 (Horizontal) Information
            char previous1 = ' ';
            int ghost1X = 15;
            int ghost1Y = 39;
            string ghost1direction = "left";
            int count1 = 0;

            // Ghost 1 Copy (Horizontal) Information
            char previous1copy = ' ';
            int ghost1Xcopy = 16;
            int ghost1Ycopy = 38;
            string ghost1directioncopy = "left";
            int count1copy = 0;

            // Ghost 2 (Vertical) Information
            char previous2 = ' ';
            int ghost2X = 20;
            int ghost2Y = 57;
            string ghost2direction = "up";
            int count2 = 0;

            // Ghost 2 Copy (Vertical) Information
            char previous2copy = ' ';
            int ghost2Xcopy = 19;
            int ghost2Ycopy = 56;
            string ghost2directioncopy = "up";
            int count2copy = 0;

            // Ghost 3 (Random) Information
            char previous3 = ' ';
            int ghost3X = 19;
            int ghost3Y = 25;

            // Ghost 3 Copy (Random) Information
            char previous3copy = ' ';
            int ghost3Xcopy = 18;
            int ghost3Ycopy = 26;

            // Ghost 4 (Smart) Information
            char previous4 = ' ';
            int ghost4X = 21;
            int ghost4Y = 30;

            // Ghost 4 Copy (Smart) Information
            char previous4copy = ' ';
            int ghost4Xcopy = 20;
            int ghost4Ycopy = 29;

            char[,] maze = new char[24, 71];

            readData(maze);
            printMaze(maze);

            Console.SetCursorPosition(pacmanY, pacmanX);
            Console.Write("P");

            bool gameRunning = true;
            bool gameRunningCopy = true;
            while (true)
            {
                Thread.Sleep(90);
                printScore();
                if (Keyboard.IsKeyPressed(Key.UpArrow))
                {
                    movePacManUp(maze, ref pacmanX, ref pacmanY);
                }
                if (Keyboard.IsKeyPressed(Key.DownArrow))
                {
                    movePacManDown(maze, ref pacmanX, ref pacmanY);
                }
                if (Keyboard.IsKeyPressed(Key.LeftArrow))
                {
                    movePacManLeft(maze, ref pacmanX, ref pacmanY);
                }
                if (Keyboard.IsKeyPressed(Key.RightArrow))
                {
                    movePacManRight(maze, ref pacmanX, ref pacmanY);
                }
                count1++;
                count2++;
                count1copy++;
                count2copy++;
                if (count1 == 5 || count1copy == 5)            // Slowest Movement
                {
                    gameRunning = moveGhostInLine(ref ghost1direction, maze, ref ghost1X, ref ghost1Y, ref previous1);
                    gameRunningCopy = MoveGhostInLineCopy(ref ghost1directioncopy, maze, ref ghost1Xcopy, ref ghost1Ycopy, ref previous1copy);
                    if (gameRunning == false || gameRunningCopy == false)
                    { 
                        break;
                    }
                    count1 = 0;
                    count1copy = 0;                    
                }
                if (count2 == 2 || count2copy == 2)            // Slow Movement
                {
                    gameRunning = moveGhostInLine(ref ghost2direction, maze, ref ghost2X, ref ghost2Y, ref previous2);
                    gameRunningCopy = MoveGhostInLineCopy(ref ghost2directioncopy, maze, ref ghost2Xcopy, ref ghost2Ycopy, ref previous2copy);
                    if (gameRunning == false || gameRunningCopy == false)
                    {
                        break;
                    }
                    count2 = 0;
                    count2copy = 0;
                }
                gameRunning = moveGhostRandom(maze, ref ghost3X, ref ghost3Y, ref previous3);
                gameRunningCopy = MoveGhostRandomCopy(maze, ref ghost3Xcopy, ref ghost3Ycopy, ref previous3copy);
                if (gameRunning == false || gameRunningCopy == false)
                {
                    break;
                }
                gameRunning = moveGhostSmart(maze, ref ghost4X, ref ghost4Y, ref previous4, pacmanX, pacmanY);
                gameRunningCopy = MoveGhostSmartCopy(maze, ref ghost4Xcopy, ref ghost4Ycopy, ref previous4copy, pacmanX, pacmanY);
                if (gameRunning == false || gameRunningCopy == false)
                {
                    break;
                }
            }
        }
        static void printScore()
        {
            Console.SetCursorPosition(79, 12);
            Console.WriteLine("Score: " + score);
        }
        static bool moveGhostSmart(char[,] maze, ref int X, ref int Y, ref char previous, int pX, int pY)
        {
            double[] distance = new double[4] {1000000,1000000,1000000,1000000};
            if (maze[X, Y - 1] != '|' && maze[X, Y - 1] != '%' )
            {
                distance[0] = (calculateDistance(X, Y-1, pX, pY));
            }
            if (maze[X, Y + 1] != '|' && maze[X, Y + 1] != '%')
            {
                distance[1] = (calculateDistance(X, Y + 1, pX, pY));
            }
            if (maze[X + 1, Y] != '|' && maze[X + 1, Y] != '%' && maze[X + 1, Y] != '#')
            {
                distance[2] = (calculateDistance(X + 1, Y, pX, pY));
            }
            if (maze[X - 1, Y] != '|' && maze[X - 1, Y] != '%' && maze[X - 1, Y] != '#')
            {
                distance[3] = (calculateDistance(X - 1, Y, pX, pY));
            }
            if (distance[0] <= distance[1] && distance[0] <= distance[2] && distance[0] <= distance[3])
            {
                string direction = "left";
                return moveGhostInLine(ref direction, maze, ref X, ref Y, ref previous);
            }
            if (distance[1] <= distance[0] && distance[1] <= distance[2] && distance[1] <= distance[3])
            {
                string direction = "right";
                return moveGhostInLine(ref direction, maze, ref X, ref Y, ref previous);
            }
            if (distance[2] <= distance[0] && distance[2] <= distance[1] && distance[2] <= distance[3])
            {
                string direction = "down";
                return moveGhostInLine(ref direction, maze, ref X, ref Y, ref previous);
            }
            else
            {
                string direction = "up";
                return moveGhostInLine(ref direction, maze, ref X, ref Y, ref previous);
            }
        }

        static bool MoveGhostSmartCopy(char[,] maze, ref int X, ref int Y, ref char previous, int pX, int pY)
        {
            double[] distance = new double[4] { 1000000, 1000000, 1000000, 1000000 };
            if (maze[X, Y - 1] != '|' && maze[X, Y - 1] != '%')
            {
                distance[0] = (calculateDistance(X, Y - 1, pX, pY));
            }
            if (maze[X, Y + 1] != '|' && maze[X, Y + 1] != '%')
            {
                distance[1] = (calculateDistance(X, Y + 1, pX, pY));
            }
            if (maze[X + 1, Y] != '|' && maze[X + 1, Y] != '%' && maze[X + 1, Y] != '#')
            {
                distance[2] = (calculateDistance(X + 1, Y, pX, pY));
            }
            if (maze[X - 1, Y] != '|' && maze[X - 1, Y] != '%' && maze[X - 1, Y] != '#')
            {
                distance[3] = (calculateDistance(X - 1, Y, pX, pY));
            }
            if (distance[0] <= distance[1] && distance[0] <= distance[2] && distance[0] <= distance[3])
            {
                string direction = "left";
                return MoveGhostInLineCopy(ref direction, maze, ref X, ref Y, ref previous);
            }
            if (distance[1] <= distance[0] && distance[1] <= distance[2] && distance[1] <= distance[3])
            {
                string direction = "right";
                return MoveGhostInLineCopy(ref direction, maze, ref X, ref Y, ref previous);
            }
            if (distance[2] <= distance[0] && distance[2] <= distance[1] && distance[2] <= distance[3])
            {
                string direction = "down";
                return MoveGhostInLineCopy(ref direction, maze, ref X, ref Y, ref previous);
            }
            else
            {
                string direction = "up";
                return MoveGhostInLineCopy(ref direction, maze, ref X, ref Y, ref previous);
            }
        }

        static double calculateDistance(int X, int Y, int pX, int pY)
        {
            return Math.Sqrt(Math.Pow((pX - X), 2) + Math.Pow((pY - Y), 2));
        }

        static bool moveGhostInLine(ref string direction, char[,] maze, ref int X, ref int Y, ref char previous)
        {
            if (maze[X, Y - 1] == 'P' || maze[X, Y + 1] == 'P' || maze[X + 1, Y] == 'P' || maze[X - 1, Y] == 'P')
            {
                return false;
            }
            if (direction == "left" && (maze[X, Y-1] == ' ' || maze[X, Y-1] == '.'))
            {
                maze[X, Y] = previous;
                Console.SetCursorPosition(Y, X);
                Console.Write(previous);
                
                Y = Y - 1;
               
                previous = maze[X, Y];
                Console.SetCursorPosition(Y, X);
                Console.Write("G");
                if(maze[X,Y-1] == '|')
                {
                    direction = "right";
                }
            }
            else if (direction == "right" && (maze[X, Y + 1] == ' ' || maze[X, Y + 1] == '.'))
            {
                maze[X, Y] = previous;
                Console.SetCursorPosition(Y, X);
                Console.Write(previous);
                
                Y = Y + 1;
                
                previous = maze[X, Y];
                Console.SetCursorPosition(Y, X);
                Console.Write("G");
                if (maze[X, Y+1] == '|')
                {
                    direction = "left";
                }
            }
            else if (direction == "up" && (maze[X-1, Y] == ' ' || maze[X-1, Y] == '.'))
            {
                maze[X, Y] = previous;
                Console.SetCursorPosition(Y, X);
                Console.Write(previous);

                X = X - 1;

                previous = maze[X, Y];
                Console.SetCursorPosition(Y, X);
                Console.Write("G");
                if (maze[X - 1, Y] == '%' || maze[X - 1, Y] == '#')
                {
                    direction = "down";
                }
            }
            else if (direction == "down" && (maze[X + 1, Y] == ' ' || maze[X + 1, Y] == '.'))
            {
                maze[X, Y] = previous;
                Console.SetCursorPosition(Y, X);
                Console.Write(previous);

                X = X + 1;

                previous = maze[X, Y];
                Console.SetCursorPosition(Y, X);
                Console.Write("G");
                if (maze[X + 1, Y] == '%' || maze[X + 1, Y] == '#')
                {
                    direction = "up";
                }
            }
            return true;
        }

        static bool MoveGhostInLineCopy(ref string direction, char[,] maze, ref int X, ref int Y, ref char previous)
        {
            if (maze[X, Y - 1] == 'P' || maze[X, Y + 1] == 'P' || maze[X + 1, Y] == 'P' || maze[X - 1, Y] == 'P')
            {
                return false;
            }
            if (direction == "left" && (maze[X, Y - 1] == ' ' || maze[X, Y - 1] == '.'))
            {
                maze[X, Y] = previous;
                Console.SetCursorPosition(Y, X);
                Console.Write(previous);

                Y = Y - 1;

                previous = maze[X, Y];
                Console.SetCursorPosition(Y, X);
                Console.Write("C");
                if (maze[X, Y - 1] == '|')
                {
                    direction = "right";
                }
            }
            else if (direction == "right" && (maze[X, Y + 1] == ' ' || maze[X, Y + 1] == '.'))
            {
                maze[X, Y] = previous;
                Console.SetCursorPosition(Y, X);
                Console.Write(previous);

                Y = Y + 1;

                previous = maze[X, Y];
                Console.SetCursorPosition(Y, X);
                Console.Write("C");
                if (maze[X, Y + 1] == '|')
                {
                    direction = "left";
                }
            }
            else if (direction == "up" && (maze[X - 1, Y] == ' ' || maze[X - 1, Y] == '.'))
            {
                maze[X, Y] = previous;
                Console.SetCursorPosition(Y, X);
                Console.Write(previous);

                X = X - 1;

                previous = maze[X, Y];
                Console.SetCursorPosition(Y, X);
                Console.Write("C");
                if (maze[X - 1, Y] == '%' || maze[X - 1, Y] == '#')
                {
                    direction = "down";
                }
            }
            else if (direction == "down" && (maze[X + 1, Y] == ' ' || maze[X + 1, Y] == '.'))
            {
                maze[X, Y] = previous;
                Console.SetCursorPosition(Y, X);
                Console.Write(previous);

                X = X + 1;

                previous = maze[X, Y];
                Console.SetCursorPosition(Y, X);
                Console.Write("C");
                if (maze[X + 1, Y] == '%' || maze[X + 1, Y] == '#')
                {
                    direction = "up";
                }
            }
            return true;
        }
        static int ghostDirection()
        {
            Random r = new Random();
            int value = r.Next(4);
            return value;
        }
        static int GhostDirectionCopy()
        {
            Random r1 = new Random();
            int value = r1.Next(4);
            return value;
        }
        static bool moveGhostRandom(char [,] maze, ref int X, ref int Y, ref char previous)
        {
            if (maze[X, Y - 1] == 'P' || maze[X, Y + 1] == 'P' || maze[X + 1, Y] == 'P' || maze[X - 1, Y] == 'P')
            {
                return false;
            }
            int value = ghostDirection();
            if (value == 0)
            {
                if (maze[X, Y - 1] == ' ' || maze[X, Y - 1] == '.' || maze[X, Y - 1] == 'P')
                {
                    maze[X, Y] = previous;
                    Console.SetCursorPosition(Y, X);
                    Console.Write(previous);

                    Y = Y - 1;
                    previous = maze[X, Y];
                    Console.SetCursorPosition(Y, X);
                    Console.Write('G');
                }
            }
            else if (value == 1)
            {
                if (maze[X, Y + 1] == ' ' || maze[X, Y + 1] == '.' || maze[X, Y + 1] == 'P')
                {
                    maze[X,Y] = previous;
                    Console.SetCursorPosition(Y, X);
                    Console.Write(previous);
                    Y = Y + 1;
                    previous = maze[X,Y];
                    Console.SetCursorPosition(Y, X);
                    Console.Write('G');
                }
            }
            else if (value == 2)
            {
                if (maze[X - 1,Y] == ' ' || maze[X - 1,Y] == '.' || maze[X - 1,Y] == 'P')
                {
                    maze[X,Y] = previous;
                    Console.SetCursorPosition(Y, X);
                    Console.Write(previous);
                    X = X - 1;
                    previous = maze[X,Y];
                    Console.SetCursorPosition(Y, X);
                    Console.Write('G');
                }
            }
            else if (value == 3)
            {
                if (maze[X + 1,Y] == ' ' || maze[X + 1,Y] == '.' || maze[X + 1,Y] == '.')
                {
                    maze[X,Y] = previous;
                    Console.SetCursorPosition(Y, X);
                    Console.Write(previous);
                    X = X + 1;
                    previous = maze[X,Y];
                    Console.SetCursorPosition(Y, X);
                    Console.Write('G');
                }
            }
            return true;
        }
        
        static bool MoveGhostRandomCopy(char[,] maze, ref int X, ref int Y, ref char previous)
        {
            if (maze[X, Y - 1] == 'P' || maze[X, Y + 1] == 'P' || maze[X + 1, Y] == 'P' || maze[X - 1, Y] == 'P')
            {
                return false;
            }
            int value = GhostDirectionCopy();
            if (value == 0)
            {
                if (maze[X, Y - 1] == ' ' || maze[X, Y - 1] == '.' || maze[X, Y - 1] == 'P')
                {
                    maze[X, Y] = previous;
                    Console.SetCursorPosition(Y, X);
                    Console.Write(previous);

                    Y = Y - 1;
                    previous = maze[X, Y];
                    Console.SetCursorPosition(Y, X);
                    Console.Write('C');
                }
            }
            else if (value == 1)
            {
                if (maze[X, Y + 1] == ' ' || maze[X, Y + 1] == '.' || maze[X, Y + 1] == 'P')
                {
                    maze[X, Y] = previous;
                    Console.SetCursorPosition(Y, X);
                    Console.Write(previous);
                    Y = Y + 1;
                    previous = maze[X, Y];
                    Console.SetCursorPosition(Y, X);
                    Console.Write('C');
                }
            }
            else if (value == 2)
            {
                if (maze[X - 1, Y] == ' ' || maze[X - 1, Y] == '.' || maze[X - 1, Y] == 'P')
                {
                    maze[X, Y] = previous;
                    Console.SetCursorPosition(Y, X);
                    Console.Write(previous);
                    X = X - 1;
                    previous = maze[X, Y];
                    Console.SetCursorPosition(Y, X);
                    Console.Write('C');
                }
            }
            else if (value == 3)
            {
                if (maze[X + 1, Y] == ' ' || maze[X + 1, Y] == '.' || maze[X + 1, Y] == '.')
                {
                    maze[X, Y] = previous;
                    Console.SetCursorPosition(Y, X);
                    Console.Write(previous);
                    X = X + 1;
                    previous = maze[X, Y];
                    Console.SetCursorPosition(Y, X);
                    Console.Write('C');
                }
            }
            return true;
        }


















        static void calculateScore()
        {
            score = score + 1;
        }

        static void movePacManUp(char[,] maze, ref int pacmanX, ref int pacmanY)
        {
            if (maze[pacmanX - 1, pacmanY] == ' ' || maze[pacmanX - 1, pacmanY] == '.')
            {
                maze[pacmanX, pacmanY] = ' ';
                Console.SetCursorPosition(pacmanY, pacmanX);
                Console.Write(" ");
                pacmanX = pacmanX - 1;
                if (maze[pacmanX, pacmanY] == '.')
                {
                    calculateScore();
                }
                Console.SetCursorPosition(pacmanY, pacmanX);
                maze[pacmanX, pacmanY] = 'P';
                Console.Write("P");

            }
        }
        static void movePacManDown(char[,] maze, ref int pacmanX, ref int pacmanY)
        {
            if (maze[pacmanX + 1,pacmanY] == ' ' || maze[pacmanX + 1,pacmanY] == '.')
            {
                maze[pacmanX,pacmanY] = ' ';
                Console.SetCursorPosition(pacmanY, pacmanX);
                Console.Write(" ");
                pacmanX = pacmanX + 1;
                Console.SetCursorPosition(pacmanY, pacmanX);
                if (maze[pacmanX, pacmanY] == '.')
                {
                    calculateScore();
                }
                maze[pacmanX, pacmanY] = 'P';
                Console.Write("P");
                
            }
        }

        static void movePacManLeft(char[,] maze, ref int pacmanX, ref int pacmanY)
        {
            if (maze[pacmanX,pacmanY - 1] == ' ' || maze[pacmanX,pacmanY - 1] == '.')
            {
                maze[pacmanX,pacmanY] = ' ';
                Console.SetCursorPosition(pacmanY, pacmanX);
                Console.Write(" ");
                pacmanY = pacmanY - 1;
                if (maze[pacmanX, pacmanY] == '.')
                {
                    calculateScore();
                }
                Console.SetCursorPosition(pacmanY, pacmanX);
                maze[pacmanX, pacmanY] = 'P';
                Console.Write("P");
                
            }
        }

        static void movePacManRight(char[,] maze, ref int pacmanX, ref int pacmanY)
        {
            if (maze[pacmanX,pacmanY + 1] == ' ' || maze[pacmanX,pacmanY + 1] == '.')
            {
                maze[pacmanX,pacmanY] = ' ';
                Console.SetCursorPosition(pacmanY, pacmanX);
                Console.Write(" ");
                pacmanY = pacmanY + 1;
                if (maze[pacmanX, pacmanY] == '.')
                {
                    calculateScore();
                }
                Console.SetCursorPosition(pacmanY, pacmanX);
                maze[pacmanX, pacmanY] = 'P';
                Console.Write("P");
                
            }
        }

        static void printMaze(char[,] maze)
        {
            for (int x = 0; x < maze.GetLength(0); x++)
            {
                for (int y = 0; y < maze.GetLength(1); y++)
                {
                    Console.Write(maze[x,y]);
                }
                Console.WriteLine();
            }
        }

        static void readData(char [,] maze)
        {
            StreamReader fp = new StreamReader("maze.txt");
            string record;
            int row = 0;
            while ((record = fp.ReadLine()) != null)
            {
                for (int x = 0; x < 71; x++)
                {
                    maze[row, x] = record[x];
                }
                row++;
            }

            fp.Close();
        }
    }
}

