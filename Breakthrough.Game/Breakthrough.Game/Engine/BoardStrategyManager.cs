using System;
using System.Collections.Generic;
using System.Text;

namespace Breakthrough.Game.Engine
{
	public static class BoardStrategyManager
	{

        /// <summary>
        /// Sets a strategy to play
        /// </summary>
        public static void SetStrategy(
             short pieceValue,
             short pieceDangerValue,
             short pieceHighDangerValue,
             short pieceAttackValue,
             short pieceProtectionValue,
             short pieceConnectionHValue,
             short pieceConnectionVValue
        )
        {
            // DESACTIVATED, USED ONLY FOR LEARNER
            //BoardEvaluation.PieceValue = pieceValue;
            //BoardEvaluation.PieceDangerValue = pieceDangerValue;
            //BoardEvaluation.PieceHighDangerValue = pieceHighDangerValue;
            //BoardEvaluation.PieceAttackValue = pieceAttackValue;
            //BoardEvaluation.PieceProtectionValue = pieceProtectionValue;
            //BoardEvaluation.PieceConnectionHValue = pieceConnectionHValue;
            //BoardEvaluation.PieceConnectionVValue = pieceConnectionVValue;
        }

        /// <summary>
        /// Sets a strategy to play
        /// </summary>
        public static void SetStrategy(short[] strategy)
        {
            if (strategy.Length != 7)
                throw new ArgumentException("Strategy array have a wrong size");

            SetStrategy(strategy[0], strategy[1], strategy[2], strategy[3], strategy[4], strategy[5], strategy[6]);
        }

        /// <summary>
        /// Gets current strategy values in form of array
        /// </summary>
        public static short[] GetStrategy()
        {
            return new List<short>(){
                BoardEvaluation.PieceValue,
                BoardEvaluation.PieceDangerValue ,
                BoardEvaluation.PieceHighDangerValue,
                BoardEvaluation.PieceAttackValue,
                BoardEvaluation.PieceProtectionValue,
                BoardEvaluation.PieceConnectionHValue,
                BoardEvaluation.PieceConnectionVValue
            }.ToArray();
        }
	}
}
