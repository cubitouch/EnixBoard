# EnixBoard
Modular library to help you build a board playing game

# Why
This library allow you to create board game by simply having:
- A board
- 2 players
- Somes cards
- A victory rule
- An AI

# Practice
To add a game, simply create a class that inherit Game.
Interrested ? Have a look at the [RowColumnGame](https://github.com/cubitouch/EnixBoard/blob/master/EnixBoard.Business/Games/RowColumnGame.cs) class.

You just need to overrride:

1. The GameTitle (the title of you game) and WinningRule (the help text) properties so that it can be shared accross platforms
2. The GenerateDeck() method so that you follow your random path
3. The InitBoard() method to size it the way you want
4. The End() method to determine which player win once the board is full of cards
5. The PlayAIDelayed() method to allow your players to practice on their own

That's it! If you followed these steps you just created a new game that you can use both for Web and Mobile app :)

To see how you can render this as a Web app', have a look at the [EnixBoard.Web](https://github.com/cubitouch/EnixBoard/tree/master/EnixBoard.Web) sample project.
To try it, let's play on [Azure](http://enixboard.azurewebsites.net/) :)
