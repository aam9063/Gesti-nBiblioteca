# Gesti-nBiblioteca


The project involves developing a digital library management system in C#. This system must allow the management of library items such as books and magazines, including their addition and removal. It should handle users, allowing new users to register and delete them, as well as manage loans of items to users and their return. The system must be capable of searching for items by title, displaying all available books and magazines, and showing all registered users. Additionally, it must include a notification mechanism for return reminders and handle a search history and a notification queue. Each library item and user must have specific detailed attributes, such as ID, title, publication year for items, and name, email for users. Loans must record the loan date and expected return date. The system interacts through a console menu that offers options for all mentioned functionalities.

The project features include the following:

1.Data Model for the Digital Library Management System:

LibraryItem:
Attributes include ID, Title, and PublicationYear, with derived classes for Book and Magazine.

Book:
Attributes cover Author and Genre, inheriting from LibraryItem.

Magazine:
Adds EditionNumber and Topics, also a LibraryItem derivative.

User:
Defined by UserID, Name, Email, MaxBorrowedItems, and a list of BorrowedItems, with events for borrowing and returning.

Loan:
Connects a LibraryItem with a User, tracking LoanDate and ReturnDate.

Catalog:
Provides methods for Add, Remove, Search, and to Show All Books or Magazines, managing a list of T, where T is a type of LibraryItem.

LoanManager and UserManager:
Offer functionality for making loans and managing users, respectively.

Notifications:
Handles the sending of notifications.

2. In the Main Class, we will have:

- Initialization of the model
- Menu

Menu:
***************************************************
Digital Library Management System
***************************************************
Welcome, select an option:
1. Search item
2. Add book
3. Add magazine
4. Remove book
5. Remove magazine
6. Register new user
7. Remove user
8. Make loan
9. Return item
10. Send return reminder
11. Show all books
12. Show all magazines
13. Show all users
14. Exit program
