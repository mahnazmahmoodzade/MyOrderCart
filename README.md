## 🛒 MyOrderCart

A Blazor Server application for browsing products, managing a shopping cart, and confirming orders via an external API.

---

### 📐 Architecture

This project follows the **Clean Architecture** pattern with clearly separated layers:

- **Domain**: Core business models (`Cart`, `CartItem`, `Product`)
- **Application**: Interfaces and business logic (`OrderService`, `IExternalOrderSender`)
- **Infrastructure**: EF Core persistence and HTTP communication
- **Blazor UI**: Presentation layer built with Blazor Server

---

### 🧰 Technologies Used

- **.NET 8** (Blazor Server)
- **Entity Framework Core** (in-memory database)
- **IOptions** pattern for configuration
- **xUnit** & **bUnit** for testing
- **Moq** for mocking dependencies in unit tests
- **Docker** for Linux-based containerization

---

### 🔌 External Integration

Orders are submitted to an external system via HTTP.  
The endpoint is configurable through `appsettings.json` using the `IOptions<OrderOptions>` pattern.

---

### ✅ Features Implemented

- Browse products from [`fakestoreapi.com`](https://fakestoreapi.com)
- Add, remove, increment, and decrement items in the cart
- Live cart updates using a shared `CartService`
- Confirm orders (cart becomes read-only)
- Send confirmed orders to an external API
- Store confirmed orders in an in-memory database (EF Core)
- Configurable product and order endpoints
- Unit tests for business logic
- UI component tests using BUnit
- Docker support

---

### 🚧 Future Improvements

- Enhanced styling with MudBlazor
- End-to-end UI tests (e.g., with Selenium)
- Improved error handling and logging

---

### ▶️ Running the App

1. Clone the repository
2. Run with Visual Studio **or** from CLI:

```bash
dotnet run --project src/MyOrderCart.BlazorUI
```

3. Or run with Docker:

```bash
docker build -t myordercart .
docker run -p 8080:80 myordercart
```

---

## 🧪 Running Tests

```bash
dotnet test
```

---

### 📂 Folder Structure

```
src/
  MyOrderCart.Domain/
  MyOrderCart.Application/
  MyOrderCart.Infrastructure/
  MyOrderCart.BlazorUI/

tests/
  MyOrderCart.UnitTests/
  MyOrderCart.BlazorUI.Tests/
```

---

### 👤 Author

Built by **Mahnaz Mahmoodzadeh** as part of a technical assessment.
