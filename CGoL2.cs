//11/13/16
//Conway's Game of Life C++

#include <iostream>
#include <fstream>

using namespace std;

void enterBoard(char firstBoard[][45], int& rows, int& columns);
void fileBoard(char firstBoard[][45], int &rows, int &columns);
void keyboardBoard(char firstBoard[][45], int &rows, int &columns);
void pattern(char board[][45], int& rows, int& columns);
void playGame(char currentBoard[][45], char nextBoard[][45], int rows, int columns);
void restartGame(char firstBoard[][45],char currentBoard[][45], int rows, int columns);
void displayBoard(char board[][45], int rows, int columns);
int cellNeighbors(char board[][45], int row, int column);
void boardCopy(char initBoard[][45], char newBoard[][45]);
float numLiveCells(char board[][45]);
void stop(void);

int main()
{
	char firstBoard[25][45], currentBoard[25][45], nextBoard[25][45];
	int choice, rows, columns;


	cout << "Conway's Game of Life" << endl;
	enterBoard(firstBoard, rows, columns);
	boardCopy(firstBoard, currentBoard);
	displayBoard(currentBoard, rows, columns);

	do
	{
		do
		{
			cout << "Menu" << endl
				 << "(1) Play Conway's Game of Life" << endl
				 << "(2) Restart the Game" << endl
				 << "(3) Display the Current Board" << endl
				 << "(4) Enter a New Board" << endl
				 << "(5) Stop the Program" << endl;
			cout << "Please enter your choice between 1 and 5: ";
			cin  >> choice;
		}while(choice > 5 || choice < 1);
	
		if(choice == 1)
			playGame(currentBoard, nextBoard, rows, columns);
		else if(choice == 2)
			restartGame(firstBoard, currentBoard, rows, columns);
		else if(choice == 3)
			displayBoard(currentBoard, rows, columns);
		else if(choice == 4)
			enterBoard(firstBoard, rows, columns);
		else if(choice == 5)
			stop();
	}while(choice != 5);

	
	
return 0;
}

void enterBoard(char firstBoard[][45], int& rows, int& columns)
{
	int fileKeyboard;

	cout << "Would you like to load the data from a file(1), keyboard?(2)" << endl
		 << "or use a predifined pattern(3): ";
	cin  >> fileKeyboard;

	do
	{
	if(fileKeyboard == 1)
		fileBoard(firstBoard, rows, columns);
	else if(fileKeyboard == 2)
		keyboardBoard(firstBoard, rows, columns);
	else if(fileKeyboard == 3)
		pattern(firstBoard, rows, columns);
	else
	{
		cout << "Please enter 1 for file, 2 for keyboard or 3 for pattern: ";
		cin  >> fileKeyboard;
	}
	}while(fileKeyboard < 1 || fileKeyboard > 3);
}

void fileBoard(char firstBoard[][45], int &rows, int &columns)
{
	char fileName[101];
	fstream infile;

	cout << "Please enter the location of the file: ";
	cin  >> fileName;
	infile.open(fileName, ios::in);
	infile >> rows;
	infile >> columns;

	for(int i =  1; i <= rows; i++)
	{
		for(int ix = 1; ix <= columns; ix++)
		{
			infile >> firstBoard[i][ix];
		}
	}
	infile.close();
}

void keyboardBoard(char firstBoard[][45], int &rows, int &columns)
{
	char again;
	int liveRow, liveCol;


	do
	{
	cout << "Please enter the number of rows between 5 and 20: ";
	cin  >> rows;
	}while(rows < 5 || rows > 20);

	do
	{
	cout << "Please enter the number of columns between 5 and 40: ";
	cin  >> columns;
	}while(columns < 5 || columns > 40);
	
	for(int i =  1; i <= rows; i++)
	{
		for(int ix = 1; ix <= columns; ix++)
		{
			firstBoard[i][ix] = '-';
		}
	}
	cout << "Please enter the live cells" << endl;
	do
	{
		do
		{
			cout << "Please enter the row (between 1 and " << rows << "): ";
			cin  >> liveRow;
		}while(liveRow < 1 || liveRow > rows);
		do
		{
			cout << "Please enter the column (between 1 and " << columns << "): ";
			cin  >> liveCol;
		}while(liveCol < 1 || liveCol > columns);

		firstBoard[liveRow][liveCol] = '*';

		cout << "Would you like to add another live cell? (y/n): ";
		cin  >> again;
	}while(again == 'y' || again == 'Y');

}

void pattern(char board[][45], int& rows, int& columns)
{
	int i, ix;
	rows = 20;
	columns = 40;
	for(i = 1; i <= rows; ++i)
	{
		for(ix = 1; ix <= columns; ++ix)
		{	
				board[i][ix] = '-';
		}
	}
	
	for(i = 1; i <= rows; i+=2)
	{
		for(ix = 1; ix <= columns; ix++)
		{	
				board[i][ix] = '*';
		}
	}

}

void playGame(char currentBoard[][45], char nextBoard[][45], int rows, int columns)
{
	int numTurns;
	int neighbors;

	do
	{
		cout << "How many turns would you like to play? (Greater than 0): ";
		cin  >> numTurns;
	}while(numTurns < 1);
		
	displayBoard(currentBoard, rows, columns);

	for(int turns = 0; turns < numTurns; ++turns)
	{
		for(int i = 1; i <= rows; i++)
		{
			for(int ix = 1; ix <= columns; ix++)
			{
				neighbors = cellNeighbors(currentBoard, i, ix);
				if (neighbors < 2)
					nextBoard[i][ix] = '-';
				else if (neighbors > 3)
					nextBoard[i][ix] = '-';
				else if (neighbors == 3)
					nextBoard[i][ix] = '*';
				else if (currentBoard[i][ix] == '*' && neighbors == 2)
					nextBoard[i][ix] = '*';
				else
					nextBoard[i][ix] = '-';
			}

		}
		displayBoard(nextBoard, rows, columns);
		boardCopy(nextBoard, currentBoard);
	}
}

void restartGame(char firstBoard[][45],char currentBoard[][45], int rows, int columns)
{
	playGame(firstBoard, currentBoard, rows, columns);
}

void displayBoard(char board[][45], int rows, int columns)
{
	char keepGoing;
	int i, ix;
	float percent, live;

	live = 0;

	for(i = 1; i <= rows + 1; i++)
	{
		cout << "|";
		for(ix = 1; ix <= columns; ix++)
		{
			if(board[i][ix] != '*')
				board[i][ix] = ' ';

			cout << board[i][ix];
			
		}

		cout << "|" << endl;
	}
	live = numLiveCells(board);
	percent = (live / (rows * columns)) * 100;

	cout << "Rows = " << rows << ", Columns = " << columns << ", Live = " << live << ", Pct = " << percent << endl;
	cout << "Press any letter or number to continue: ";
	cin  >> keepGoing;
}

int cellNeighbors(char board[][45], int row, int column)
{
	int neighbor;

	neighbor = 0;

	if(board[row-1][column-1] == '*')
		++neighbor;
	if(board[row][column-1] == '*')
		++neighbor;
	if(board[row-1][column] == '*')
		++neighbor;
	if(board[row-1][column+1] == '*')
		++neighbor;
	if(board[row+1][column-1] == '*')
		++neighbor;
	if(board[row+1][column] == '*')
		++neighbor;
	if(board[row][column+1] == '*')
		++neighbor;
	if(board[row+1][column+1] == '*')
		++neighbor;

return neighbor;
}

void boardCopy(char initBoard[][45], char newBoard[][45])
{
	for(int i = 0; i < 25; i++)
	{
		for(int ix = 0; ix < 45; ix++)
		{
			newBoard[i][ix] = initBoard[i][ix];
		}
	}
	
}

float numLiveCells(char board[][45])
{
	float live;
	live = 0.0f;
	for(int i = 1; i <= 20; i++)
	{
		for(int ix = 1; ix <= 40; ix++)
		{
			if(board[i][ix] == '*')
				++live;
		}
	}
return live;
}

void stop(void)
{
	cout << "Goodbye" << endl;
}
