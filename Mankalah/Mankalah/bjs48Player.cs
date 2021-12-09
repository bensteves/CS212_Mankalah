/*********************************************************************
* Mankalah - Program 5
* CS 212
* Ben Steves, 12-4-20
*
*********************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
namespace Mankalah
{

    public class bjs48Player : Player
    {

        public bjs48Player(Position pos, int timeLimit) : base(pos, "bjs48", timeLimit) { }

        public override string gloat()
        {
            return "Guess I won. Time to celebrate with an ice cold Pepsi.";
        }

        public override String getImage() { return "ben.png"; }


        /// <summary>
        /// chooseMove() starts a stopwatch and picks move from minimax search
        /// </summary>
        /// <param name="b"> A board object B </param> 
        /// <returns> A move from the minimax tree </returns> 
        public override int chooseMove(Board b)
        {
            // create stopwatch and start it
            Stopwatch watch = new Stopwatch();
            watch.Start();

            // add depth var and create new blank result
            int depth = 1;
            Result move = new Result(0, 0);

            // before time runs out, do minimax search
            while (watch.ElapsedMilliseconds < getTimePerMove())
            {
                move = minimax(b, depth++, watch);
            }
            return move.getMove();
        }

        /// <summary>
        /// minimax() creates a minimax and finds next move
        /// </summary>
        /// <param name="b"> a board object B </param> 
        /// <param name="d"> a depth, type int </param>
        /// <param name="w"> a stopwatch object w </param>
        /// <returns> A result type object with a best move and a best value </returns> 
        private Result minimax(Board b, int d, Stopwatch w)
        {
            // make vars to keep track of best move and score
            int bestMove = 0;
            int bestVal;

            // game over or depth == 0
            if (b.gameOver() || d == 0)
            {
                return new Result(0, evaluate(b));
            }

            //  check to see if move is legal and recursively go through minimax tree for best move
            if (b.whoseMove() == Position.Top)
            {
                bestVal = int.MinValue;
                for (int move = 7; move <= 12; move++)
                {
                    if (b.legalMove(move)) //&& w.ElapsedMilliseconds < getTimePerMove())
                    {
                        Board b1 = new Board(b);
                        b1.makeMove(move, false);
                        Result val = minimax(b1, d - 1, w);
                        if (val.getScore() > bestVal)
                        {
                            bestVal = val.getScore();
                            bestMove = move;
                        }
                    }
                }
            }
            else
            {
                bestVal = int.MaxValue;
                for (int move = 0; move <= 5; move++)
                {
                    if (b.legalMove(move))// && w.ElapsedMilliseconds < getTimePerMove())
                    {
                        Board b1 = new Board(b);
                        b1.makeMove(move, false);
                        Result val = minimax(b1, d - 1, w);
                        if (val.getScore() < bestVal)
                        {
                            bestVal = val.getScore();
                            bestMove = move;
                        }
                    }
                }
            }
            return new Result(bestMove, bestVal);
        }

        /// <summary>
        /// evaluate() generates a score for a certain move
        /// </summary>
        /// <param name="b"> a board object B </param> 
        /// <returns> an int with the score for a move or board state </returns> 
        public override int evaluate(Board b)
        {
            // create factors for evaltuation
            int score = b.stonesAt(13) - b.stonesAt(6);
            int numStones = 0;
            int captures = 0;
            int goAgains = 0;

            // if bjs48 is top, add values to score
            if (b.whoseMove() == Position.Top)
            {
                for (int i = 7; i <= 12; i++)
                {
                    numStones = numStones + b.stonesAt(i);
                    int marker = b.stonesAt(i) + i;
                    if (marker == 13)
                    {
                        goAgains = goAgains + 1;
                    }
                    if (marker < 13 && b.stonesAt(marker) == 0 && b.stonesAt(13 - marker - 1) != 0)
                    {
                        captures = captures + b.stonesAt(13 - marker - 1);
                    }

                }
                return (score + numStones + (goAgains + 6) + (captures + 12));

            }

            // if bjs48 is bottom, subtract values from score
            else
            {
                for (int i = 0; i <= 5; i++)
                {
                    numStones = numStones - b.stonesAt(i);
                    int marker = b.stonesAt(i) + i;
                    if (marker == 6)
                    {
                        goAgains = goAgains - 1;
                    }
                    if (marker < 6 && b.stonesAt(marker) == 0 && b.stonesAt(13 - marker - 1) != 0)
                    {
                        captures = captures - b.stonesAt(13 - marker - 1);
                    }
                }
                return ((score + numStones + (goAgains + 6) + (captures + 12))*-1);
            }



        }

        // Result class to access move and score
        class Result
        {
            private int move;
            private int score;

            public Result(int m, int s)
            {
                move = m;
                score = s;
            }

            public int getMove()
            {
                return move;
            }

            public int getScore()
            {
                return score;
            }
        }
    }
}

