using System;
using System.Collections.Generic;

public abstract class LibraryItem
{
    public int ID { get; set; }
    public string Title { get; set; }
    public int PublicationYear { get; set; }

    public LibraryItem(int id, string title, int publicationYear)
    {
        ID = id;
        Title = title;
        PublicationYear = publicationYear;
    }

    public override string ToString()
    {
        return $"ID: {ID}, Title: {Title}, Publication Year: {PublicationYear}";
    }
}

public class Book : LibraryItem // Book class
{
    public string Author { get; set; }
    public string Genre { get; set; }

    public Book(int id, string title, int publicationYear, string author, string genre)
        : base(id, title, publicationYear)
    {
        Author = author;
        Genre = genre;
    }

    public override string ToString()
    {
        return base.ToString() + $", Author: {Author}, Genre: {Genre}";
    }
}

public class Magazine : LibraryItem
{
    public int EditionNumber { get; set; }
    public string Topics { get; set; }

    public Magazine(int id, string title, int publicationYear, int editionNumber, string topics)
        : base(id, title, publicationYear)
    {
        EditionNumber = editionNumber;
        Topics = topics;
    }

    public override string ToString()
    {
        return base.ToString() + $", Edition Number: {EditionNumber}, Topics: {Topics}";
    }
}

// User and Loan
public class User // User class
{
    public int UserID { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public List<LibraryItem> BorrowedItems { get; set; }
    public int MaxBorrowedItems { get; set; }

    public User(int userID, string name, string email, int maxBorrowedItems)
    {
        UserID = userID;
        Name = name;
        Email = email;
        MaxBorrowedItems = maxBorrowedItems;
        BorrowedItems = new List<LibraryItem>();
    }

    // Define delegates for events
    public delegate void BorrowedEventHandler(object source, EventArgs args);
    public delegate void ReturnedEventHandler(object source, EventArgs args);

    // Define events
    public event BorrowedEventHandler Borrowed;
    public event ReturnedEventHandler Returned;

    // Method to borrow an item
    public bool BorrowItem(LibraryItem item)
    {
        if (BorrowedItems.Count < MaxBorrowedItems)
        {
            BorrowedItems.Add(item);
            OnBorrowed();
            return true;
        }
        else
        {
            return false;
        }
    }

    // Method to return an item
    public bool ReturnItem(LibraryItem item)
    {
        if (BorrowedItems.Contains(item))
        {
            BorrowedItems.Remove(item);
            OnReturned();
            return true;
        }
        else
        {
            return false;
        }
    }

    // Methods to trigger events
    protected virtual void OnBorrowed()
    {
        if (Borrowed != null)
        {
            Borrowed(this, EventArgs.Empty);
        }
    }

    protected virtual void OnReturned()
    {
        if (Returned != null)
        {
            Returned(this, EventArgs.Empty);
        }
    }

    public override string ToString()
    {
        return $"ID: {UserID}, Name: {Name}, Email: {Email}, Max Borrowed Items: {MaxBorrowedItems}, Borrowed Items: {string.Join(", ", BorrowedItems)}";
    }

}

public class Loan // Loan class
{
    public LibraryItem Item { get; set; }
    public User User { get; set; }
    public DateTime LoanDate { get; set; }
    public DateTime ReturnDate { get; set; }

    public Loan(LibraryItem item, User user, DateTime loanDate, DateTime returnDate)
    {
        Item = item;
        User = user;
        LoanDate = loanDate;
        ReturnDate = returnDate;
    }

    public override string ToString()
    {
        return $"Item: {Item}, User: {User}, Loan Date: {LoanDate}, Return Date: {ReturnDate}";
    }
}

// Interfaces
public interface ISearchable<T>
{
    T Search(string title);
}

public interface IManageable<T>
{
    void Add(T item);
    void Remove(T item);
}

public interface INotifiable
{
    void SendNotification(User user, string message);
}

// Class Implementations with Interfaces
public class Catalog<T> : IManageable<T>, ISearchable<T> where T : LibraryItem
{
    public List<T> Items = new List<T>();

    public void Add(T item)
    {
        Items.Add(item);
        Console.WriteLine($"Item added: {item.Title}");
    }

    public void Remove(T item)
    {
        Items.Remove(item);
        Console.WriteLine($"Item removed: {item.Title}");
    }

    public List<T> GetAll()
    {
        return Items;
    }

    public T Search(string title)
    {
        return Items.FirstOrDefault(item => item.Title == title);
    }

    public void ShowAllBooks()
    {
        foreach (var item in Items)
        {
            if (item is Book)
            {
                Console.WriteLine(item);
            }
        }
    }

    public void ShowAllMagazines()
    {
        foreach (var item in Items)
        {
            if (item is Magazine)
            {
                Console.WriteLine(item);
            }
        }
    }
}

public class LoanManager  
{
    public void MakeLoan(Loan loan)
    {
        Console.WriteLine($"Loan made for: {loan.Item.Title}");
    }
}

public class UserManager<T> : IManageable<T> where T : User
{
    public Dictionary<int, T> Users = new Dictionary<int, T>();

    public void Add(T user)
    {
        if (!Users.ContainsKey(user.UserID))
        {
            Users.Add(user.UserID, user);
            Console.WriteLine($"User added: {user.Name}");
        }
        else
        {
            Console.WriteLine($"The user with ID {user.UserID} already exists.");
        }
    }

    public List<T> GetAll()
    {
        return Users.Values.ToList();
    }

    public void Remove(T user)
    {
        Users.Remove(user.UserID);
        Console.WriteLine($"User removed: {user.Name}");
    }

    public void ShowAllUsers()
    {
        foreach (var user in Users)
        {
            Console.WriteLine($"ID: {user.Key}, Name: {user.Value.Name}");
        }
    }
}

public class Notifications : INotifiable // Notifications class
{
    public void SendNotification(User user, string message)
    {
        Console.WriteLine($"Notification sent to {user.Name}: {message}");
    }
}

// Digital Library Management System
class LibraryManagementSystem
{
    static List<Book> bookCatalog = new List<Book>();
    static List<Magazine> magazineCatalog = new List<Magazine>();
    static LoanManager loanManager = new LoanManager(); //Loan Manager
    static UserManager<User> userManager = new UserManager<User>(); //User Manager
    static Notifications notifications = new Notifications(); //Notifications
    static Stack<string> searchHistory = new Stack<string>(); //Search history
    static Queue<string> notificationQueue = new Queue<string>(); //Notification queue
    static Stack<User> registeredUsers = new Stack<User>(); //Registered users
    static Queue<User> usersToRemove = new Queue<User>(); //Users to remove


    static List<LibraryItem> libraryItems = new List<LibraryItem>()
    {
            new Book(1, "Libro 1", 2000, "Autor 1", "Género 1"),
            new Book(2, "Libro 2", 2005, "Autor 2", "Género 2"),
            new Magazine(3, "Revista 1", 2010, 1, "Tema 1"),
            new Magazine(4, "Revista 2", 2015, 2, "Tema 2"),
            new Book(5, "Libro 3", 2010, "Autor 3", "Género 3"),
            new Magazine(6, "Revista 3", 2015, 3, "Tema 3"),
            new Book(7, "Libro 4", 2015, "Autor 4", "Género 4"),
            new Magazine(8, "Revista 4", 2020, 4, "Tema 4"),
            new Book(9, "Libro 5", 2020, "Autor 5", "Género 5"),
            new Magazine(10, "Revista 5", 2020, 5, "Tema 5"),
            new Book(11, "Libro 6", 2020, "Autor 6", "Género 6"),
            new Magazine(12, "Revista 6", 2020, 6, "Tema 6")
    };

    static LibraryManagementSystem() //Constructor
    {
        foreach (var item in libraryItems)
        {
            if (item is Book book)
            {
                bookCatalog.Add(book);
            }
            else if (item is Magazine magazine)
            {
                magazineCatalog.Add(magazine);
            }
        }
    }


    static List<User> users = new List<User>()
        {
            new User(1, "Usuario 1", "usuario1@email.com", 2),
            new User(2, "Usuario 2", "usuario2@email.com", 2),
            new User(3, "Usuario 3", "usuario3@email.com", 2),
            new User(4, "Usuario 4", "usuario4@email.com", 2),
            new User(5, "Usuario 5", "usuario5@email.com", 2),
            new User(6, "Usuario 6", "usuario6@email.com", 2),
            new User(7, "Usuario 7", "usuario7@email.com", 2),
            new User(8, "Usuario 8", "usuario7@email.com", 2),
            new User(9, "Usuario 9", "usuario7@email.com", 2),
            new User(10, "Usuario 10", "usuario7@email.com", 2),
            new User(11, "Usuario 11", "usuario7@email.com", 2),
            new User(12, "Usuario 12", "usuario7@email.com", 2)
        };




    static void Main(string[] args)
    {

        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("***************************************************");
            Console.WriteLine("Digital Library Management System");
            Console.WriteLine("***************************************************");
            Console.WriteLine("                                                    ");
            Console.WriteLine("Welcome, select an option: ");
            Console.WriteLine("                                                    ");
            Console.WriteLine("1. Search item");
            Console.WriteLine("2. Add book");
            Console.WriteLine("3. Add magazine");
            Console.WriteLine("4. Remove book");
            Console.WriteLine("5. Remove magazine");
            Console.WriteLine("6. Register new user");
            Console.WriteLine("7. Remove user");
            Console.WriteLine("8. Make loan");
            Console.WriteLine("9. Return item");
            Console.WriteLine("10. Send return reminder");
            Console.WriteLine("11. Show all books");
            Console.WriteLine("12. Show all magazines");
            Console.WriteLine("13. Show all users");
            Console.WriteLine("14. Exit program");

            int option = Convert.ToInt32(Console.ReadLine());

            switch (option)
            {
                case 1:
                    // Search item
                    SearchItem();
                    break;
                case 2:
                    // Add book
                    AddBook();
                    break;
                case 3:
                    // Add magazine
                    AddMagazine();
                    break;
                case 4:
                    // Remove book
                    RemoveBook();
                    break;
                case 5:
                    // Remove magazine
                    RemoveMagazine();
                    break;
                case 6:
                    // Register new user
                    RegisterNewUser();
                    break;
                case 7:
                    // Remove user
                    RemoveUser();
                    break;
                case 8:
                    // Make loan
                    MakeLoan();
                    break;
                case 9:
                    // Return item
                    ReturnItem();
                    break;
                case 10:
                    // Send return reminder
                    SendReturnReminder();
                    break;
                case 11:
                    // Show all books
                    ShowAllBooks();
                    break;
                case 12:
                    // Show all magazines
                    ShowAllMagazines();
                    break;
                case 13:
                    // Show all users
                    ShowAllUsers();
                    break;
                case 14:
                    exit = true;
                    Console.WriteLine("Exiting the program...");
                    break;
                default:
                    Console.WriteLine("Invalid option, try again.");
                    break;
            }

            // Process the notification queue
            ProcessNotificationQueue();

           
        }
    }

    // Method to search for an item
    static void SearchItem()
    {
        Console.WriteLine("Enter the title of the item:");
        string criteria = Console.ReadLine().Trim();
        if (string.IsNullOrEmpty(criteria))
        {
            Console.WriteLine("No se introdujo ningún criterio de búsqueda.");
            return;
        }
        var book = bookCatalog.Find(b => b.Title.Trim().Equals(criteria, StringComparison.OrdinalIgnoreCase));
        var magazine = magazineCatalog.Find(m => m.Title.Trim().Equals(criteria, StringComparison.OrdinalIgnoreCase));
        if (book != null)
        {
            Console.WriteLine($"Book found: {book}");
        }
        else if (magazine != null)
        {
            Console.WriteLine($"Magazine found: {magazine}");
        }
        else
        {
            Console.WriteLine("Item not found.");
        }
        searchHistory.Push(criteria);
    }

    static void AddLibraryItem<T>(Func<string[], T> createItem, List<T> catalog) where T : LibraryItem // Method to add a library item
    {
        Console.WriteLine("Enter the details of the item:");
        string[] inputs = Console.ReadLine().Split(',');
        T item = createItem(inputs);
        catalog.Add(item);
        libraryItems.Add(item);
    }


    static void AddBook() // Method to add a book
    {
        AddLibraryItem(inputs =>
        {
            int id = Convert.ToInt32(inputs[0].Trim());
            string title = inputs[1].Trim();
            int publicationYear = Convert.ToInt32(inputs[2].Trim());
            string author = inputs[3].Trim();
            string genre = inputs[4].Trim();
            return new Book(id, title, publicationYear, author, genre);
        }, bookCatalog);
    }

    static void AddMagazine() // Method to add a magazine
    {
        AddLibraryItem(inputs =>
        {
            int id = Convert.ToInt32(inputs[0].Trim());
            string title = inputs[1].Trim();
            int publicationYear = Convert.ToInt32(inputs[2].Trim());
            int editionNumber = Convert.ToInt32(inputs[3].Trim());
            string topics = inputs[4].Trim();
            return new Magazine(id, title, publicationYear, editionNumber, topics);
        }, magazineCatalog);
    }

    static void RemoveBook() // Method to remove a book
    {
        Console.WriteLine("Enter the title of the book to remove:");
        string title = Console.ReadLine().Trim();
        var bookToRemove = bookCatalog.Find(b => b.Title.Trim().Equals(title, StringComparison.OrdinalIgnoreCase));
        if (bookToRemove != null)
        {
            bookCatalog.Remove(bookToRemove);
            libraryItems.RemoveAll(item => item.Title.Trim().Equals(title, StringComparison.OrdinalIgnoreCase));
            Console.WriteLine("Book removed.");
        }
        else
        {
            Console.WriteLine("Book not found.");
        }
    }

    // Método para eliminar una revista
    static void RemoveMagazine()
    {
        Console.WriteLine("Enter the title of the magazine to remove:");
        string title = Console.ReadLine().Trim();
        var magazineToRemove = magazineCatalog.Find(m => m.Title.Trim().Equals(title, StringComparison.OrdinalIgnoreCase));
        if (magazineToRemove != null)
        {
            magazineCatalog.Remove(magazineToRemove);
            libraryItems.RemoveAll(item => item.Title.Trim().Equals(title, StringComparison.OrdinalIgnoreCase));
            Console.WriteLine("Magazine removed.");
        }
        else
        {
            Console.WriteLine("Magazine not found.");
        }
    }

    // Method to register a new user
    static void RegisterNewUser()
    {
        Console.WriteLine("Enter the user details (ID, Name, Email, Maximum number of items borrowed):");
        string[] userData = Console.ReadLine().Split(','); // Split the input by comma
        int id = Convert.ToInt32(userData[0].Trim());
        string name = userData[1].Trim();
        string email = userData[2].Trim();
        int maxBorrowedItems = Convert.ToInt32(userData[3].Trim());

        User newUser = new User(id, name, email, maxBorrowedItems); // Create a new user
        users.Add(newUser); // Add the user to the list of users
        registeredUsers.Push(newUser); // Add the user to the stack of registered users

        foreach (User user in users) // Display the list of users
        {
            Console.WriteLine(user);
        }
    }

    // Method to remove a user
    static void RemoveUser()
    {
        Console.WriteLine("Enter the ID of the user to remove:");
        int id = Convert.ToInt32(Console.ReadLine());

        // Find the user in the list of users
        User user = users.Find(u => u.UserID == id);
        if (user != null)
        {
            // Add the user to the queue of users to remove
            usersToRemove.Enqueue(user);

            // Remove the user from the list of users
            users.Remove(user);
            Console.WriteLine("Success.");
        }
        else
        {
            Console.WriteLine("User not found.");
        }

        // Process the queue of users to remove
        while (usersToRemove.Count > 0)
        {
            User userToRemove = usersToRemove.Dequeue();
            Console.WriteLine($"User removed: {userToRemove.Name}");
        }
    }

    // Method to make a loan
    static void MakeLoan()
    {
        Console.WriteLine("Enter the ID of the user and the title of the item to loan:");
        string[] input = Console.ReadLine().Split(',');
        int userId = Convert.ToInt32(input[0].Trim());
        string title = input[1].Trim();

        // Find the user
        User user = users.Find(u => u.UserID == userId);
        if (user == null)
        {
            Console.WriteLine("Usuario no encontrado");
            return;
        }

        // Find the item
        LibraryItem item = libraryItems.Find(i => i.Title == title);
        if (item == null)
        {
            Console.WriteLine("Artículo no encontrado");
            return;
        }

        // Loan the item to the user
        user.BorrowItem(item);
        loanManager.MakeLoan(new Loan(item, user, DateTime.Now, DateTime.Now.AddDays(7)));
        Console.WriteLine("Loan made.");
    }



    // Method to return an item
    static void ReturnItem()
    {
        Console.WriteLine("Enter the ID of the user and the title of the item to return, separated by a comma:");
        string[] input = Console.ReadLine().Split(','); // Split the input by comma
        if (int.TryParse(input[0].Trim(), out int userId))
        {
            // Find the user
            User user = users.Find(u => u.UserID == userId);
            if (user != null)
            {
                string title = input.Length > 1 ? input[1].Trim() : string.Empty; // Get the title of the item
                var item = user.BorrowedItems.FirstOrDefault(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase)); // Find the item in the user's borrowed items
                if (item != null)
                {
                    if (user.ReturnItem(item))
                    {
                        Console.WriteLine("Item successfully returned.");
                    }
                    else
                    {
                        Console.WriteLine("The item could not be returned.");
                    }
                }
                else
                {
                    Console.WriteLine("Item not found in the user's loans.");
                }
            }
            else
            {
                Console.WriteLine("User not found.");
            }
        }
        else
        {
            Console.WriteLine("Invalid user ID format. Please enter a number.");
        }
    }


    // Method to send a return reminder
    static void SendReturnReminder()
    {
        Console.WriteLine("Enter the ID of the user to whom to send the reminder:");
        int userId = Convert.ToInt32(Console.ReadLine());
        var user = users.FirstOrDefault(u => u.UserID == userId); // Find the user
        if (user != null)
        {
            notifications.SendNotification(user, "Please return the borrowed items.");
            notificationQueue.Enqueue($"Reminder sent to {user.Name}");
            ProcessNotificationQueue(); // Process the notification queue
        }
        else
        {
            Console.WriteLine("User not found.");
        }
    }


    // Method to process the notification queue
    static void ProcessNotificationQueue()
    {
        while (notificationQueue.Count > 0)
        {
            string notification = notificationQueue.Dequeue();
            Console.WriteLine(notification);
        }
    }

    // Method to show all books
    static void ShowAllBooks()
    {
        foreach (var item in libraryItems)
        {
            if (item is Book)
            {
                Console.WriteLine(item);
            }
        }
    }

    // Method to show all magazines
    static void ShowAllMagazines()
    {
        foreach (var item in libraryItems)
        {
            if (item is Magazine)
            {
                Console.WriteLine(item);
            }
        }
    }

    // Method to show all users
    static void ShowAllUsers()
    {
        foreach (var user in users)
        {
            Console.WriteLine(user);
        }
    }

}
