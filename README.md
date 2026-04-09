# Chess ♟️

A clean, modular implementation of the classic game of Chess. This isn't just a game; it's a project built with a clean separation of concerns, featuring a dedicated move engine and a modern web interface.

---

## 🏗 Architecture

The solution is divided into three specialized projects to ensure scalability and maintainable code:

* **Chess.Engine**: The brain of the operation. Contains all core business logic, move validation, and game state rules.
* **Chess.Api**: The communication layer. Built with **.NET**, this project handles RESTful requests and coordinates between the engine and the user.
* **Chess.Core**: The visual layer. A responsive web application that serves as the primary interface for players.

---

## 🚀 Getting Started

Follow these steps to get your local environment up and running.

### 1. Start the Backend (API)
Navigate to the `Chess.Api` directory and run the project. By default, the server will start on port `7081`.
* **Command**: `dotnet run` (or run via Visual Studio/Rider).
* **Base URL**: `https://localhost:7081`

### 2. Explore the API (Optional)
If you want to test the REST endpoints manually, you can access the Interactive Swagger documentation:
* **URL**: [https://localhost:7081/swagger/index.html](https://localhost:7081/swagger/index.html)

### 3. Start the Frontend
Open your terminal in the `Chess.Core` directory and launch the web application:
```bash
npm install
npm start
```

---

## 🎮 How to Play
1. Once both the API and the Frontend are running:
2. Open your browser to the local address provided by the terminal (usually http://localhost:4200).
3. Challenge a friend locally or play against yourself to test out strategies.
4. Enjoy the beautiful game of Chess!

---

## 🛠 Tech Stack
* Backend: .NET / C#
* Frontend: React / JavaScript (Node.js)
* Documentation: Swagger / OpenAPI

---

## 👥 Author
* Mikołaj Śnieżko
* mikolajsniezko@gmail.com
