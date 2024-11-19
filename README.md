# Food Ordering Website 🍴

A web-based application built with ASP.NET Core MVC that allows users to browse a food catalog, place orders, and provide feedback. The application includes administrative features to manage products, orders, and users efficiently.

---

## Features ✨

### User Features:
- **User Registration/Login**: Secure user authentication.
- **Browse Products**: View the food catalog with categories and detailed descriptions.
- **Search Functionality**: Quickly find items using the search bar.
- **Shopping Cart**: Add items to the cart, update quantities, and review before checkout.
- **Order Placement**: Seamless order processing with a payment gateway integration.
- **Order History**: View past orders and their details.

### Admin Features:
- **Product Management**: Add, update, or delete food items.
- **Order Management**: View and manage customer orders.
- **User Management**: Manage registered users and their roles.

---

## Technologies Used 🛠️

- **Frontend**: Razor Pages (HTML, CSS, JavaScript)
- **Backend**: ASP.NET Core MVC (C#)
- **Database**: SQL Server Express
- **Tools**: Visual Studio, Entity Framework Core

---

## Prerequisites 📦

1. **Install SQL Server Express:**
   - [Download SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
   - Install SQL Server Express and SQL Server Management Studio (SSMS) for database management.

2. **Install Visual Studio 2022 (or newer):**
   - During installation, ensure you include:
     - **ASP.NET and web development**
     - **.NET desktop development**
     - **Data storage and processing**
   - [Download Visual Studio](https://visualstudio.microsoft.com/)

3. **Install Entity Framework Core Tools:**
   - Open **Package Manager Console** in Visual Studio and run:
     ```powershell
     Install-Package Microsoft.EntityFrameworkCore.Tools
     ```

4. **Clone the Repository:**
   ```bash
   git clone https://github.com/your-username/food-ordering-website.git
   cd food-ordering-website
   
5. **Open the Folder in Visual Studio**
   - Add the code in Program.cs:    ``` "ConnectionStrings": { "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=FoodOrderingDB;Trusted_Connection=True;MultipleActiveResultSets=true" } ```
    
6. **Apply Database Migrations**
   - Open **Package Manager Console** in Visual Studio and run:
    ``` powershell
      Add-Migration InitialCreate
      Update-Database
    ```

7. **Run the Project**
   - Press **Ctrl+F5** in Visual Studio to launch the application.
  
