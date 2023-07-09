import utils
import time
import signal
from copy import deepcopy

import utils
import time
from copy import deepcopy

class PlayerAI: 
    eval_table = {}

    class Board:
        def __init__(self, board):
            self.board = board

        def get_all_black_pieces(self):
            '''get 2d array of [row, col] for all black pieces'''
            black_pieces = []
            for row in range(utils.ROW):
                for col in range(utils.COL):
                    if self.board[row][col] == "B":
                        black_pieces.append([row, col])
            return black_pieces

        def get_all_white_pieces(self):
            '''get 2d array of [row, col] for all white pieces'''
            white_pieces = []
            for row in range(utils.ROW):
                for col in range(utils.COL):
                    if self.board[row][col] == "W":
                        white_pieces.append([row, col])
            return white_pieces

        def get_new_state(self, move):
            '''get a new state by moving a piece'''
            new_board = deepcopy(self)
            new_board.board = utils.state_change(new_board.board, move[0], move[1])
            return new_board

        def get_valid_moves(self, piece):
            '''
            get all valid moves for a black piece
            
            returns: 3d array of all possible moves [[src_row, src_col], [dst_row, dst_col]] 
            '''
            moves = []
            
            row = piece[0]
            col = piece[1]
            tos_ = [
            [row + 1, col], #move down
            [row + 1, col - 1], #move diagonal left
            [row + 1, col + 1] #move diagonal right
            ]

            for to_ in tos_:
                if utils.is_valid_move(self.board, piece, to_):
                    moves.append([piece, to_])

            return moves

        def get_sorted_moves(self, moves, max_player, eval_table):
            '''
            sort moves for optimizing alpha beta pruning

            sort based on evaluation score of the state after the move
            '''
            return sorted(moves, 
                key = lambda move: self.get_new_state(move).evaluate(eval_table), 
                reverse = not max_player #reverse if is min player
                )

        def get_black_pieces_value(self, black_pieces):
            '''get the value of each black piece'''
            pieces_value = {}

            for piece in black_pieces:
                piece_row = piece[0]
                piece_col = piece[1]

                if piece_row == utils.ROW - 1:
                    pieces_value[str(piece)] = 100000 #value of piece is very high since it wins
                elif piece_row == utils.ROW - 2:
                    pieces_value[str(piece)] = 100 #value of piece is quite high since it is 1 move away from winning
                else:
                    pieces_value[str(piece)] = piece_row + 1 #value of a piece increases as it moves forward

                    # if piece is being defended, it has a higher score.
                    diagonal_left = [piece_row - 1, piece_col - 1]
                    diagonal_right = [piece_row - 1, piece_col + 1]
                    if (diagonal_left or diagonal_right) in black_pieces:
                        pieces_value[str(piece)] += 20

            return pieces_value

        def get_white_pieces_value(self, white_pieces):
            '''get the value of each white piece'''
            pieces_value = {}

            for piece in white_pieces:
                piece_row = piece[0]
                piece_col = piece[1]

                if piece_row == 0:
                    pieces_value[str(piece)] = 100000 
                elif piece_row == 1:
                    pieces_value[str(piece)] = 100 
                else:
                    pieces_value[str(piece)] = utils.ROW - piece_row 

                    diagonal_left = [piece_row + 1, piece_col - 1]
                    diagonal_right = [piece_row + 1, piece_col + 1]
                    if (diagonal_left or diagonal_right) in white_pieces:
                        pieces_value[str(piece)] += 20

            return pieces_value

        def get_danger_squares(self):
            '''return squares that white could attack (black should avoid)'''
            white_pieces = self.get_all_white_pieces() 
            danger_squares = []

            for piece in white_pieces:
                row = piece[0]
                col = piece[1]

                diagonal_left = [row - 1, col - 1]
                danger_squares.append(diagonal_left)

                diagonal_right = [row - 1, col + 1]
                danger_squares.append(diagonal_right)

            return danger_squares
        
        def evaluate(self, eval_table):
            '''evaluate heuristic of the board'''

            state = str(self)
            if state in eval_table:
                return eval_table[state]

            black_pieces = self.get_all_black_pieces()
            white_pieces = self.get_all_white_pieces()
            black_pieces_values = self.get_black_pieces_value(black_pieces)
            white_pieces_values = self.get_white_pieces_value(white_pieces)
            danger_squares = self.get_danger_squares()

            total_points = 0 

            for square in danger_squares:
                if str(square) in black_pieces_values:
                    black_pieces_values[str(square)] = -100 #value of piece decreases since it can be captured
            
            for piece in black_pieces_values:
                total_points += black_pieces_values[piece]

            for piece in white_pieces_values:
                total_points -= white_pieces_values[piece]

            eval_table[state] = total_points
            return total_points

        def is_game_over(self):
            '''check if game is over'''
            return utils.is_game_over(self.board)

        def __str__(self):
            '''return string representation of the board'''
            return str(self.board)

    #############################################################
    ##############       end of Board class      ################
    #############################################################

    def get_all_moves_and_states(self, board, max_player):
        '''returns all valid moves and states'''
        moves_and_states = []

        for piece in board.get_all_black_pieces(): 
            valid_moves = board.get_valid_moves(piece)
            sorted_moves = board.get_sorted_moves(valid_moves, max_player, self.eval_table) 
            for move in sorted_moves: 
                new_state = board.get_new_state(move)
                moves_and_states.append([move, new_state])
        
        return moves_and_states

    def minimax(self, board, depth, maximizingPlayer, alpha, beta):
        '''minimax algorithm'''
        if depth == 0 or board.is_game_over():
            return board.evaluate(self.eval_table), board

        if maximizingPlayer:
            maxEval = float('-inf')
            best_move = None

            for move_and_state in self.get_all_moves_and_states(board, maximizingPlayer): # move_and_state is [[from_, to_], board]
                val = self.minimax(move_and_state[1], depth - 1, False, alpha, beta)[0] 
                maxEval = max(maxEval, val)
                alpha = max(alpha, maxEval)
                if val == maxEval:
                    best_move = move_and_state[0]
                if beta <= alpha:
                    break
            
            return maxEval, best_move
        
        else:
            minEval = float('inf')
            best_move = None

            for move_and_state in self.get_all_moves_and_states(board, maximizingPlayer):
                val = self.minimax(move_and_state[1], depth - 1, True, alpha, beta)[0]
                minEval = min(minEval, val)
                beta = min(beta, minEval)
                if val == minEval:
                    best_move = move_and_state[0]
                if beta <= alpha:
                    break
            
            return minEval, best_move  

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
        state = self.Board(board) # create a board object
        best_move = self.minimax(state, 3, True, float('-inf'), float('inf'))[1]
        return best_move

class Player2AI:
    
    MAX, MIN, DEPTH = float('inf'), -float('inf'), 30
    
    maximizer_table = {}
    minimizer_table = {}
    evaluation_table = {}
    
    class State:
        def __init__(self, board = None, positions = None):
            self.board = board
            if positions is None:
                self.positions = {}
            else:
                self.positions = positions

            if self.board is not None:
                for row in range(6):
                    for col in range(6):
                        if board[row][col] == 'B':
                            #use 1 to represent black
                            self.positions[(row,col)] = 1;
                        if board[row][col] == 'W':
                            #use 0 to represent white
                            self.positions[(row,col)] = 0;

        def get_board(self):
            return self.board

        def get_new_state(self, move):
            start_pos = move[0]
            end_pos = move[1]

            new_positions = self.positions.copy()
            new_positions[end_pos] = new_positions[start_pos]
            del new_positions[start_pos]
            return Player2AI.State(positions = new_positions)


        def goal_test(self):
            if not self.get_white_positions():
                return True
            else:
                my_positions = self.get_black_positions()
                for i in range(6):
                    if (5, i) in my_positions:
                        return True
                return False
            
        def lose_test(self):
            if not self.get_black_positions():
                return True
            else:
                opp_positions = self.get_white_positions()
                for i in range(6):
                    if (0, i) in opp_positions:
                        return True
                return False

        def get_black_positions(self):
            return [k for k,v in self.positions.items() if float(v) == 1]

        def get_white_positions(self):
            return [k for k,v in self.positions.items() if float(v) == 0]

        def get_valid_moves(self, is_black_turn):
            valid_moves = []
            if is_black_turn:
                black_positions = self.get_black_positions()
                for pos in black_positions:
                        row = pos[0]
                        col = pos[1]
                        #diagonal left, if there is white piece can eat
                        if row != 5 and col != 0 and (row + 1, col - 1) not in black_positions:
                            valid_moves.append((pos,(row + 1, col - 1)))
                        #forward, if any piece blocking is invalid
                        if row != 5 and (row + 1, col) not in self.positions:
                            valid_moves.append((pos, (row + 1, col)))
                        #diagonal right, if there is white piece can eat
                        if row != 5 and col != 5 and (row + 1, col + 1) not in black_positions:
                            valid_moves.append((pos, (row + 1, col + 1)))
            else:
                white_positions = self.get_white_positions()
                for pos in white_positions:
                        row = pos[0]
                        col = pos[1]
                        #diagonal left, if there is white piece can eat
                        if row != 0 and col != 0 and (row - 1, col - 1) not in white_positions:
                            valid_moves.append((pos,(row - 1, col - 1)))
                        #forward, if any piece blocking is invalid
                        if row != 0 and (row - 1, col) not in self.positions:
                            valid_moves.append((pos, (row + 1, col)))
                        #diagonal right, if there is white piece can eat
                        if row != 0 and col != 5 and (row - 1, col + 1) not in white_positions:
                            valid_moves.append((pos, (row - 1, col + 1)))

            return valid_moves

        def order_moves(self, valid_moves, maximizing_player, evaluation_table):
            return sorted(valid_moves, key = lambda move: self.get_new_state(move).evaluation_fn(evaluation_table), reverse=maximizing_player)

        def __str__(self):
            string = ""
            black_positions = self.get_black_positions()
            white_positions = self.get_white_positions()
            for i in range(6):
                for j in range(6):
                    pos = (i,j)
                    if pos in black_positions:
                        string += "1"
                    elif pos in white_positions:
                        string += "0"
                    else:
                        string += "_"
            return string
        
        def heuristic(self):
            if self.goal_test():
                return 1000000
            if self.lose_test():
                return -1000000
                
        def evaluation_fn(self, evaluation_table):
            
            string = str(self)
            if string in evaluation_table:
                return evaluation_table[string]
            
            h = 0
            black_positions = self.get_black_positions()
            white_positions = self.get_white_positions()

            #reward for win
            

            for pos in white_positions:
                row = pos[0]
                col = pos[1]

                #penalise close to losing moves
                if row == 1:
                    h -= 200

                    #penalise heavily if next move loss
                    if (row - 1, col) not in black_positions:
                        h -= 10000

            for pos in black_positions:
                row = pos[0]
                col = pos[1]

                #reward close to winning moves
                if row == 4:
                    h += 200

                    #reward heavily if next move win
                    if (row+1, col) not in white_positions:
                        h += 10000

                #reward protective strategy
                if (row - 1, col - 1) in black_positions or (row - 1, col + 1) in black_positions:
                    h += 50
                    
                #predicted win in two moves
                if row == 3:
                    if(row+1, col) not in white_positions and (row+1, col) not in black_positions:
                        if ((row+2, col+1) not in white_positions and (row+2, col-1) not in white_positions) or\
                        ((row+2, col) not in white_positions and (row+2, col-2) not in white_positions) or\
                        ((row+2, col) not in white_positions and (row+2, col+2) not in white_positions):
                            h += 3000
                    if(row+1, col+1) not in white_positions and (row+1, col+1) not in black_positions:
                        if ((row+2, col+2) not in white_positions and (row+2, col) not in white_positions) or\
                        ((row+2, col+1) not in white_positions and (row+2, col-1) not in white_positions) or\
                        ((row+2, col+1) not in white_positions and (row+2, col+3) not in white_positions):
                            h += 3000
                    if(row+1, col-1) not in white_positions and (row+1, col-1) not in black_positions:
                        if ((row+2, col) not in white_positions and (row+2, col-2) not in white_positions) or\
                        ((row+2, col-1) not in white_positions and (row+2, col-3) not in white_positions) or\
                        ((row+2, col-1) not in white_positions and (row+2, col+1) not in white_positions):
                            h += 3000

            #reward more number of pawns left
            h += 10 * len(black_positions)

            #rewards state with pawn nearest to row 5
            max_row = 0
            for pos in black_positions:
                temp = pos[0]
                max_row = max(temp, max_row)

            evaluation_table[string] = h + max_row * 10
            return h + max_row * 10
    
    #Returns optimal value for current player
    def minimax(self, depth, maximizing_player,
                curr_state, alpha, beta):
        
        if curr_state.goal_test() or curr_state.lose_test():
            return curr_state.heuristic()
        # Terminating condition
        if depth == self.DEPTH:
            return curr_state.evaluation_fn(self.evaluation_table)

        if maximizing_player:
            string = str(curr_state)
            if string in self.maximizer_table:
                return self.maximizer_table[string]

            best = self.MIN

            valid_moves = curr_state.get_valid_moves(True)
            for move in curr_state.order_moves(valid_moves, maximizing_player, self.evaluation_table):
                new_state = curr_state.get_new_state(move)
                val = self.minimax(depth + 1, False, new_state, alpha, beta)
                best = max(best, val)
                alpha = max(alpha, best)

                # Alpha Beta Pruning
                if beta <= alpha:
                    break
                    
            self.maximizer_table[string] = best

            return best

        else:
            string = str(curr_state)
            if string in self.minimizer_table:
                return self.minimizer_table[string]
            
            best = self.MAX

            valid_moves = curr_state.get_valid_moves(False)
            for move in curr_state.order_moves(valid_moves, maximizing_player, self.evaluation_table):
                new_state = curr_state.get_new_state(move)            
                val = self.minimax(depth + 1, True, new_state, alpha, beta)
                best = min(best, val)
                beta = min(beta, best)

                # Alpha Beta Pruning
                if beta <= alpha:
                    break
            
            self.minimizer_table[string] = best

            return best
        
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
        curr_state = self.State(board = board)
        next_move = None
        best_move_value = self.MIN
        possible_moves = curr_state.get_valid_moves(True)
        
        for move in curr_state.order_moves(possible_moves, True, self.evaluation_table):
            new_state = curr_state.get_new_state(move)
            if new_state.goal_test():
                next_move = move
                break
            
            val = self.minimax(0, False, new_state, self.MAX, self.MIN)
            if val > best_move_value:
                best_move_value = val
                next_move = move
        
        return next_move

class Player3AI:
    
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

class PlayerNaive:
    ''' A naive agent that will always return the first available valid move '''
    def make_move(self, board):
        return utils.generate_rand_move(board)

# You may replace PLAYERS with any two players of your choice
PLAYERS = [Player2AI(), PlayerAI()]
COLOURS = [BLACK, WHITE] = 'Black', 'White'
TIMEOUT = 3.0

##########################
# Game playing framework #
##########################
if __name__ == "__main__":

    print("Initial State")
    board = utils.generate_init_state()
    utils.print_state(board)

    # state = Board(board)
    # print(get_all_moves_and_states(state))
    move = 0

    # game starts
    while not utils.is_game_over(board):
        player = PLAYERS[move % 2]
        colour = COLOURS[move % 2]
        if colour == WHITE: # invert if white
            utils.invert_board(board)
        start = time.time()
        src, dst = player.make_move(board) # returns [i1, j1], [i2, j2] -> pawn moves from position [i1, j1] to [i2, j2]
        end = time.time()
        within_time = end - start <= TIMEOUT
        valid = utils.is_valid_move(board, src, dst) # checks if move is valid
        if not valid or not within_time: # if move is invalid or time is exceeded, then we give a random move
            print('executing random move')
            src, dst = utils.generate_rand_move(board)
        utils.state_change(board, src, dst) # makes the move effective on the board
        if colour == WHITE: # invert back if white
            utils.invert_board(board)

        print(f'Move No: {move} by {colour}')
        utils.print_state(board) # printing the current configuration of the board after making move
        move += 1
    print(f'{colour} Won')