This is a private project created by me.
It is literally Chess. Nothing less nothing more.

The solution is separated into few smaller projects - Api, Engine & Core.

- Api is backend, designed in .NET, responsible for REST.
- Engine is also backend, though it containts business logic.
- Core is responsible for frontend - web app.

In order to play your own game of Chess (by yourself or with a friend), you need to:

1. Run ChessApi: https -> by default it starts on 7081 port.
2. If you want to check REST, go to `https://localhost:7081/swagger/index.html` after starting api.
3. Go to your terminal and type in `npm start`.
4. Enjoy the beautiful game of Chess on your own!
