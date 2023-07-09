import utils
import time
from copy import deepcopy

class PlayerAI:
    
    #evaluation function for min_max algorithm
    def evaluate(self, board, player):
        if player:
            return self.black_score(board) - self.white_score(board)
        if player == False:
            return (self.black_score(board) - self.white_score(board)) * -1
    
    #finds the total score of all the black pawns on the board
    def black_score(self, board):
        score = 0
        win_value = 1000
        #This value weights the progress of a pawn
        pawn_values = [2, 1, 2, 3, 8, win_value]
        #This value weights a protection of a pawn
        protection_multiplier = 1.25
        #This value wights an unprotected pawn that is attack
        no_D_penalty = 0.1
        for i in range(utils.ROW):
            for j in range(utils.COL):
                #calculates the score of each individual pawn
                if board[i][j] == 'B':
                    pawn_value = pawn_values[i]

                    #adds points if the pawn is protected
                    if i - 1 >= 0 and j - 1 >= 0 and board[i-1][j-1] == 'B':
                            pawn_value *= protection_multiplier
                            #adds more points if the pawn is under attack
                            if i + 1 < utils.ROW and j - 1 >= 0 and board[i+1][j-1] == 'W' or i + 1 < utils.ROW and j + 1 < utils.COL and board[i+1][j+1] == 'W': 
                                pawn_value *= protection_multiplier
                    if i - 1 >= 0 and j + 1 < utils.COL  and board[i-1][j+1] == 'B':
                            pawn_value *= protection_multiplier
                            if i + 1 < utils.ROW and j - 1 >= 0 and board[i+1][j-1] == 'W' or i + 1 < utils.ROW and j + 1 < utils.COL and board[i+1][j+1] == 'W': 
                                pawn_value *= protection_multiplier
                    elif i + 1 < utils.ROW and j - 1 >= 0 and board[i+1][j-1] == 'W' or i + 1 < utils.ROW and j + 1 < utils.COL and board[i+1][j+1] == 'W':
                        pawn_value *= no_D_penalty
                    score += pawn_value                    
        return score

    def white_score(self, board):
        return self.black_score(utils.invert_board(board))

    
    #value for boardstate where black is won
    def win_game(self, board):
        win = 1000
        for i in range(utils.COL):
            if board[utils.ROW - 1][i] == 'B':
                return win
        return 0

    
    #searching algorithm
    def min_max(self, board, depth, player):
        if depth == 0 or utils.is_game_over == True:
            return self.evaluate(board, player), board
        
        if  player:
            maxEval = float('-inf')
            best_move = None

            for move in self.get_all_moves(board):
                evaluation = self.min_max(move[0], depth - 1, False)[0]
                maxEval = max(maxEval, evaluation)
                if maxEval == evaluation:
                    best_move = move[1]
                #returns best move before the timeout
                if time.time() - start >= 2.90:
                    return maxEval, best_move

            return maxEval, best_move
        else:
            # simulates opponents turn and tries to maximise their score to find best the best move
            minEval = float('inf')
            best_move = None

            for move in self.get_all_moves(utils.invert_board(board)):
                evaluation = self.min_max(move[0], depth  - 1, True)[0]
                minEval = min(minEval, evaluation)
                if minEval == evaluation:
                    best_move = move[1]
                #returns best move before the timeout
                if time.time() - start >= 2.90:
                    return minEval, best_move

            return minEval, best_move
    
    #gets all moves
    def get_all_moves(self, board):
        move_list = []
        for r in range(len(board)):
            for c in range(len(board[r])):
                # check if B can move diagonally left
                if c-1 >= 0 and r+1 < utils.ROW:
                    if board[r][c] == 'B' and board[r+1][c-1] == '_' or board[r][c] == 'B' and board[r+1][c-1] == 'W':
                        src = [r, c]
                        dst = [r+1, c-1]
                        temp_board = deepcopy(board)
                        new_board = utils.state_change(temp_board, src, dst)
                        move = [src, dst]
                        move_list.append([new_board, move]) 
                # check if B can move down straight    
                if r+1 < utils.ROW:
                    if board[r][c] == 'B' and board[r+1][c] == '_':
                        src = [r, c]
                        dst = [r+1, c]
                        temp_board = deepcopy(board)
                        new_board = utils.state_change(temp_board, src, dst)
                        move = [src, dst]
                        move_list.append([new_board, move]) 
                # check if B can move diagonally right 
                if c+1 < utils.COL and r+1 < utils.ROW:
                    if board[r][c] == 'B' and board[r+1][c+1] == '_' or board[r][c] == 'B' and board[r+1][c+1] == 'W':
                        src = [r, c]
                        dst = [r+1, c+1]
                        temp_board = deepcopy(board)
                        new_board = utils.state_change(temp_board, src, dst)
                        move = [src, dst]
                        move_list.append([new_board, move])
        return move_list
    

    
    # counts number of pawns
    def get_black_pawns(self, board):
        counter = 0
        for r in range(len(board)):
                for c in range(len(board[r])):
                    if board[r][c] == 'B':
                        counter += 1
        return counter
    def get_white_pawns(self, board):
        counter = 0
        for r in range(len(board)):
                for c in range(len(board[r])):
                    if board[r][c] == 'W':
                        counter += 1
        return counter
    

    def make_move(self, board):
        '''
        This is the function that will be called from main.py
        Your function should implement a minimax algorithm with 
        alpha beta pruning to select the appropriate move based 
        on the input board state. Play for black.

        Parameters
        ----------
        self: object instance itself, passed in automatically by Python
        board: 2D list-of-lists
        Contains characters 'B', 'W', and '_' representing
        Black pawns, White pawns and empty cells respectively
        
        Returns
        -------
        Two lists of coordinates [row_index, col_index]
        The first list contains the source position of the Black pawn 
        to be moved, the second list contains the destination position
        '''
        ################
        # Starter code #
        ################
        # TODO: Replace starter code with your AI
        return self.min_max(board, 3, True)[1]