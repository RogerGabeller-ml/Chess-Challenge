using ChessChallenge.API;
using System;

public class MyBot : IChessBot
{
    int mDepth = 5;
    Move bestMove;
    int[] pieceValueList = { 71, 293, 300, 456, 905 };

    public Move Think(Board board, Timer timer)
    {
        NegaMax(board, mDepth, -10000, 10000, board.IsWhiteToMove ? 1 : -1);
        return bestMove;
    }
    
    int NegaMax(Board board, int depth, int alpha, int beta, int color) {
        if (board.IsDraw()) return 0;
        if (depth == 0) {
            if (board.IsInCheckmate()) return board.IsWhiteToMove ? -10000 : 10000;
            int sum = board.GetLegalMoves().Length;
            for (int i = 1; i < 6; i++) {
                sum += (board.GetPieceList((PieceType)i, true).Count - board.GetPieceList((PieceType)i, false).Count) * pieceValueList[i - 1];
            }
            return sum * color;
        }
        int score = -10000;
        foreach (Move move in board.GetLegalMoves()) {
            board.MakeMove(move);
            int newScore = -NegaMax(board, depth - 1, -beta, -alpha, -color);
            if (newScore > score) {
                score = newScore;
                if (depth == mDepth) bestMove = move;
            }

            board.UndoMove(move);
            alpha = Math.Max(alpha, score);
            if (alpha >= beta) break;
        }
        return score;
    }
}