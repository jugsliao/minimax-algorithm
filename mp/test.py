a = [1,2,3,4,5]
b = [1]

# print(b in a)
# dict = {}
# dict[str([1,2])] = 5
# print(dict[str([1,2])])

if (3 or 8) in a:
    print('hi')
else:
    print('bye')


############
## eval 1 ##
############

# total_points = 4*5 + 5*5 #total possible start distance 
# endpoints = [[5, 0], [5, 1], [5, 2], [5, 3], [5, 4], [5, 5]] 
# black_total_score = 0

# # for each black piece, calculculate shortest dist to endpoint
# black_pieces = self.get_all_black_pieces()
# white_pieces = self.get_all_white_pieces()
# for piece in black_pieces:
#     piece_row = piece[0]
#     piece_col = piece[1]
#     min_dist = 0
#     for end in endpoints:
#         end_row = end[0]
#         end_col = end[1]
#         dist = (end_row - piece_row) + (end_col - piece_col)
#         min_dist = min(min_dist, dist)
#     total_points -= min_dist

##############
### eval 2 ###
##############
# black_dist = self.get_total_dist_from_goal(black_pieces, "B")
# white_dist = self.get_total_dist_from_goal(white_pieces, "W")

# print([1,2] == [1,2])

# class State:
#     def __init__(self, board):
#         self.board = board
#         # self.squares = {}

#         # for row in range(utils.ROW):
#         #     for col in range(utils.COL):
#         #         if board[row][col] == "B":
#         #             self.squares[str([row, col])] = "B"
#         #         elif board[row][col] == "W":
#         #             self.squares[str([row, col])] = "W"

#     def get_all_black_pieces(self):
#         '''get 2d array of [row, col] for all black pieces'''
#         black_pieces = []
#         for row in range(utils.ROW):
#             for col in range(utils.COL):
#                 if self.board[row][col] == "B":
#                     black_pieces.append([row, col])
#         return black_pieces

#     # def get_all_black_pieces(self):
#     #     '''get 2d array of [row, col] for all black pieces'''
#     #     black_pieces = []
#     #     for k, v in self.squares.items():
#     #         if v == "B":
#     #             black_pieces.append(k)
#     #     return black_pieces

#     def get_all_white_pieces(self):
#         '''get 2d array of [row, col] for all white pieces'''
#         white_pieces = []
#         for row in range(utils.ROW):
#             for col in range(utils.COL):
#                 if self.board[row][col] == "W":
#                     white_pieces.append([row, col])
#         return white_pieces

#     # def get_all_white_pieces(self):
#     #     '''get 2d array of [row, col] for all white pieces'''
#     #     white_pieces = []
#     #     for k, v in self.squares.items():
#     #         if v == "W":
#     #             white_pieces.append(k)
#     #     return white_pieces
    
#     def get_valid_moves(self, piece):
#         '''
#         get all valid moves for a black piece
        
#         returns: 3d array of all possible moves [[src_row, src_col], [dst_row, dst_col]] 
#         '''
#         moves = []
        
#         row = piece[0]
#         col = piece[1]
#         tos_ = [
#         [row + 1, col], #move down
#         [row + 1, col - 1], #move diagonal left
#         [row + 1, col + 1] #move diagonal right
#         ]

#         for to_ in tos_:
#             if utils.is_valid_move(self.board, piece, to_):
#                 moves.append([piece, to_])

#         return moves

#     # def get_valid_moves(self, piece):
#     #     '''
#     #     get all valid moves for a black piece
        
#     #     returns: 3d array of all possible moves [[src_row, src_col], [dst_row, dst_col]] 
#     #     '''
#     #     moves = []
#     #     black_pieces = self.get_all_black_pieces()
        
#     #     row = piece[0]
#     #     col = piece[1]

#     #     if row != 5:
#     #     #can move forward only if there are no piece blocking
#     #         forward = str([row +  1, col])
#     #         if forward not in self.squares:
#     #             moves.append(forward)
#     #     #can move diagonal only if black piece is not blocking and they are not at the edge
#     #         diagonal_left = str([row + 1, col - 1])
#     #         diagonal_right = str([row + 1, col + 1])
#     #         if col != 0 and diagonal_left not in black_pieces:
#     #             moves.append(diagonal_left)
#     #         if col !=5 and diagonal_right not in black_pieces:
#     #             moves.append(diagonal_right) 

#     #     return moves

#     # def get_total_dist_from_goal(self, pieces, isblack):
#     #     total_dist = 0 
#     #     for piece in pieces:
#     #         row = piece[0]
#     #         if isblack:
#     #             dist = utils.ROW - row
#     #             total_dist += dist
#     #         else:
#     #             dist = row 
#     #             total_dist += row

#     #     return total_dist

#     def get_pieces_value(self, pieces):
#         '''
#         get the value of each piece
        
#         value increases as it moves forward. 
#         prioritizes rushing a single piece to end point.
#         the furthest piece has the highest value.
#         '''
#         pieces_value = {}

#         for piece in pieces:
#             piece_row = piece[0]
#             pieces_value[str(piece)] = piece_row #value of a piece is how near it is to the goal (row 5)

#         return pieces_value

#     def get_danger_squares(self):
#         '''return squares that white could attack'''
#         white_pieces = self.get_all_white_pieces() 
#         danger_squares = []

#         for piece in white_pieces:
#             row = piece[0]
#             col = piece[1]

#             diagonal_left = [row - 1, col - 1]
#             danger_squares.append(diagonal_left)

#             diagonal_right = [row - 1, col + 1]
#             danger_squares.append(diagonal_right)

#         return danger_squares
    
#     def evaluate(self):
#         '''evaluate heuristic of the board'''
#         black_pieces = self.get_all_black_pieces()
#         white_pieces = self.get_all_white_pieces()
#         pieces_values = self.get_pieces_value(black_pieces)
#         danger_squares = self.get_danger_squares()

#         total_points = 0 

#         for square in danger_squares:
#             if str(square) in pieces_values:
#                 pieces_values[str(square)] = 0 #value of piece becomes 0 since it can be captured
        
#         for piece in pieces_values:
#             total_points += pieces_values[piece]

#         return total_points

#     def move(self, from_, to_):
#         '''actually move a piece'''
#         self.board = utils.state_change(self.board, from_, to_)

#     def is_game_over(self):
#         '''check if game is over'''
#         return utils.is_game_over(self.board)

#     def __str__(self):
#         return str(self.board)