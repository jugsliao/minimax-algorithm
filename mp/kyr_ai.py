import time
import utils

class PlayerAI:
    
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
            return PlayerAI.State(positions = new_positions)


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
                        #forw   ard, if any piece blocking is invalid
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
            return sorted(valid_moves, 
            key = lambda move: self.get_new_state(move).evaluation_fn(evaluation_table), 
            reverse=maximizing_player)

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


#class PlayerNaive:
    ''' A naive agent that will always return the first available valid move '''
#    def make_move(self, board):
#        return utils.generate_rand_move(board)

class PlayerNaive:
    
    MAX, MIN, DEPTH = float('inf'), -float('inf'), 48
    
    maximizer_table = {}
    minimizer_table = {}
    heuristic_table = {}
    
    class Statee:
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
        def is_black_win(self):
            white_position = self.get_white_positions()
            black_position = self.get_black_positions()
            if not white_position:
                return True
            else:
                for i in range(6):
                    if (5, i) in black_position:
                        return True
                return False

        def is_white_win(self):
            white_position = self.get_white_positions()
            black_position = self.get_black_positions()
            if not black_position:
                return True
            else:
                for i in range(6):
                    if (0, i) in white_position:
                        return True
                return False
        
        def get_board(self):
            return self.board

        def get_new_state(self, move):
            start_pos = move[0]
            end_pos = move[1]

            new_positions = self.positions.copy()
            new_positions[end_pos] = new_positions[start_pos]
            del new_positions[start_pos]
            return PlayerNaive.Statee(positions = new_positions)


        def goal_test(self):
            if not self.get_white_positions():
                return True
            else:
                my_positions = self.get_black_positions()
                for i in range(6):
                    if (5, i) in my_positions:
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
        
        def heuristic(self):
            h = 0

            #valorizes winning position
            if self.is_black_win():
                h += 1000000
            if self.is_white_win():
                h -= 1000000
            return h
    
        def heuristicy(self):
            h = 0
            
            white_position = self.get_white_positions()
            black_position = self.get_black_positions()
            #penalise white almost win
            for i in range(6):
                if (2, i) in white_position:
                    h -= 50000

            for pos in black_position:
                row = pos[0]
                col = pos[1]

                #penalise danger of getting eaten
                if (row + 1, col + 1) in white_position:
                    h -= 1000
                if (row + 1, col - 1) in white_position:
                    h -= 1000

                #close to winning
                if row == 4:
                    h += 20 
                    #forced win (basically won)
                    if (row + 1, col + 1) not in white_position and (row +1 , col - 1) not in white_position:
                        h += 50000

                #protecting pawn
                if (row - 1, col - 1) in black_position or (row - 1, col + 1) in black_position:
                    h += 1000

                #defensive formation
                if (row - 1, col - 1) in black_position and (row - 1, col + 1) in black_position:
                    h += 5000

                #predicted win in two moves
                if row == 3:
                    if(row+1, col) not in white_position and (row+1, col) not in black_position:
                        if ((row+2, col+1) not in white_position and (row+2, col-1) not in white_position) or\
                        ((row+2, col) not in white_position and (row+2, col-2) not in white_position) or\
                        ((row+2, col) not in white_position and (row+2, col+2) not in white_position):
                            h += 3000
                    if(row+1, col+1) not in white_position and (row+1, col+1) not in black_position:
                        if ((row+2, col+2) not in white_position and (row+2, col) not in white_position) or\
                        ((row+2, col+1) not in white_position and (row+2, col-1) not in white_position) or\
                        ((row+2, col+1) not in white_position and (row+2, col+3) not in white_position):
                            h += 3000
                    if(row+1, col-1) not in white_position and (row+1, col-1) not in black_position:
                        if ((row+2, col) not in white_position and (row+2, col-2) not in white_position) or\
                        ((row+2, col-1) not in white_position and (row+2, col-3) not in white_position) or\
                        ((row+2, col-1) not in white_position and (row+2, col+1) not in white_position):
                            h += 3000

            h += 1000 * len(black_position)

            h += 1000 * (12 - len(white_position))

            max_row = 0
            #reward basic advancing moves
            for pos in black_position:
                temp = pos[0]
                max_row = max(temp, max_row)
            return h + max_row * 1000
        
        def order_moves(self, valid_moves, maximizing_player):
            return sorted(valid_moves, key = lambda move: self.get_new_state(move).heuristicy(), reverse=maximizing_player)

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
        
    
    #Returns optimal value for current player
    def minimax(self, depth, maximizing_player,
                curr_state, alpha, beta):
        
        # Terminating condition
        if curr_state.is_white_win() or curr_state.is_black_win():
            return curr_state.heuristic()
        
        if depth == self.DEPTH:
            return curr_state.heuristicy()

        if maximizing_player:
            string = str(curr_state)
            if string in self.maximizer_table:
                return self.maximizer_table[string]

            best = self.MIN

            valid_moves = curr_state.get_valid_moves(True)
            for move in curr_state.order_moves(valid_moves, maximizing_player):
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
            for move in curr_state.order_moves(valid_moves, maximizing_player):
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
        curr_state = PlayerNaive.Statee(board = board)
        next_move = None
        best_move_value = self.MIN
        possible_moves = curr_state.get_valid_moves(True)
        
        for move in curr_state.order_moves(possible_moves, True):
            new_state = curr_state.get_new_state(move)
            if new_state.goal_test():
                next_move = move
                break
            
            val = self.minimax(0, False, new_state, self.MAX, self.MIN)
            if val > best_move_value:
                best_move_value = val
                next_move = move
        
        return next_move

# You may replace PLAYERS with any two players of your choice
PLAYERS = [PlayerAI(), PlayerNaive()]
COLOURS = [BLACK, WHITE] = 'Black', 'White'
TIMEOUT = 3.0

##########################
# Game playing framework #
##########################
if __name__ == "__main__":

    print("Initial State")
    board = utils.generate_init_state()
    utils.print_state(board)
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