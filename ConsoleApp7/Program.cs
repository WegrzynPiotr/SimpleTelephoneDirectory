using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks.Dataflow;
using System.Xml.Linq;

namespace ConsoleApp2
{
    internal class Program
    {
        static string path = "contactList.txt";
        static SortedDictionary<string, string> dictionary = new SortedDictionary<string, string>();
        static void Main(string[] args)
        {
            string[] noFile = { "" };
            if (!File.Exists(path))
            {
                File.WriteAllLines(path, noFile);
            }

            string name = "";
            string number = "";
            foreach (var el in File.ReadAllLines(path))
            {
                if(el.Length != 0)
                {
                name = el.Substring(0, el.IndexOf(","));
                number = el.Substring(el.IndexOf(",") + 1);
                dictionary.Add(name, number);
                }
                else
                {
                    Console.WriteLine("Brak kontaktów! Dodaj nowy, klikając 'D'");
                }

            }
            
            do
            {
                getOption();
            } while (true);
        }

        static void renderMenu()
        {
            Console.WriteLine("Opcje:");
            Console.WriteLine("W - Wyświetl wszystko:");
            Console.WriteLine("S - Szukaj po imieniu i nazwisku:");
            Console.WriteLine("A - Szukaj po nr telefonu:");
            Console.WriteLine("D - Dodaj nowy kontakt:");
            Console.WriteLine("U - Usuń kontakt:");
            Console.WriteLine("E - Edytuj kontakt:");

        }
        static void getOption()
        {
            renderMenu();
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            Console.WriteLine();
            switch (keyInfo.Key)
            {
                case ConsoleKey.W:
                    Console.Clear();
                    renderAllContacts();
                    break;
                case ConsoleKey.S:
                    Console.Clear();
                    searchByFullName();
                    break;
                case ConsoleKey.A:
                    Console.Clear();
                    searchByNumber();
                    break;
                case ConsoleKey.D:
                    Console.Clear();
                    addNewElement();
                    break;
                case ConsoleKey.U:
                    removeContact();
                    break;
                case ConsoleKey.E:
                    Console.Clear();
                    editContact();
                    break;
            }
            safeToFile();
        }

        static void renderAllContacts()
        {
            foreach (var element in dictionary)
            {

                Console.WriteLine(element.Key + ' ' + element.Value);
                

            }
            Console.WriteLine();
        }

        static void editContact() {
            Console.Clear();
            renderAllContacts();
            Console.WriteLine("Wpisz kontakt który chcesz edytować");
            string fullName = Console.ReadLine();
            fullName = fullName.ToLower();

            string currName = "";
            string currNumber = "";
            foreach (var element in dictionary)
            {
                if (element.Key.ToLower().Contains(fullName) || element.Value.Contains(fullName))
                {
                    currName = element.Key;
                    currNumber = element.Value;
                }

            }
            if (!currName.ToLower().Contains(fullName) && !currNumber.Contains(fullName) || fullName.Length == 0)
            {
                Console.WriteLine("Błędne dane\n");
                return;

            }
            Console.Clear();
            Console.WriteLine("\nCzy " + currName + " z numerem tel. " + currNumber + " ma zostać edytowany? (kliknij Y aby potwierdzić)");
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            if (keyInfo.Key == ConsoleKey.Y)
            {
                Console.WriteLine("Jeśli chcesz zmienić tylko nazwę kliknij 'N', a jeśli numer tel 'T' ");
                string newName = "";
                string newNumber = "";
                ConsoleKeyInfo keyI = Console.ReadKey();
                switch (keyI.Key)
                {
                    case ConsoleKey.N:
                        Console.WriteLine("\nWpisz nową nazwę dla " + currName + "\n");
                        newName = Console.ReadLine();
                        dictionary.Remove(currName);
                        dictionary.Add(newName, currNumber);
                        Console.WriteLine("Zmiana z "+ currName + " " + currNumber + " na:");
                        Console.WriteLine(newName + " " + currNumber);
                        break;
                    case ConsoleKey.T:
                        Console.WriteLine("\nWpisz nowy numer dla" + currName + " z nr tel. " + currNumber + "\n");
                        newNumber = Console.ReadLine();
                        dictionary[currName] = newNumber;
                        Console.WriteLine("Zmiana z "+currName + " " + currNumber + " na:");
                        Console.WriteLine(currName + " " + newNumber);
                        break;
                }
                safeToFile();
                return;
            }
            else
            {
                Console.WriteLine("Żaden kontakt nie został Edytowany!");
                return;
            }
        }

        static void removeContact() {

            Console.Clear();
            renderAllContacts();
            Console.WriteLine("Wpisz osobę lub numer kontaktu, który chcesz skasować");
            string fullName = Console.ReadLine();         
            fullName = fullName.ToLower();

            string currName = "";
            string currNumber = "";
            foreach (var element in dictionary)
            {
                if (element.Key.ToLower().Contains(fullName) || element.Value.Contains(fullName))
                {
                    currName = element.Key;
                    currNumber = element.Value;
                }

            }
            if (!currName.ToLower().Contains(fullName.ToLower()) && !currNumber.Contains(fullName) || fullName.Length==0)
            {
                Console.WriteLine("Błędne dane\n");
                return;
                
            }

            Console.WriteLine("\nCzy " + currName + " z numerem tel. " +currNumber + " ma zostać skasowany? (kliknij Y aby potwierdzić)");
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            Console.Clear();
            if (keyInfo.Key == ConsoleKey.Y)
            {
            Console.WriteLine("\n"+currName + " Został skasowany\n");
            dictionary.Remove(currName);
            safeToFile();
                return;
            }
            else
            {
                Console.WriteLine("Żaden kontakt nie został usunięty!");
                return;
            }


        }

        static void searchByFullName()
        {
            Console.WriteLine("Wpisz osobę której szukasz");
            string fullName = Console.ReadLine();
            fullName = fullName.ToLower();
                foreach (var element in dictionary)
                {
                if (element.Key.ToLower().Contains(fullName))
                {
                    Console.WriteLine(element.Key + " " + element.Value);
                }

            }
            
        }
        static void searchByNumber()
        {
            Console.WriteLine("Wpisz nr telefonu po którym chcesz znaleźć");
            string number = Console.ReadLine();
            foreach (var element in dictionary)
            {
                if (element.Value.Contains(number))
                {
                    Console.WriteLine(element.Key + " " + element.Value);
                }
            }
        }

        static string getName(string input) {
            string firstName = "";
            if (input.Contains(" "))
            {
                firstName = input.Substring(0, input.IndexOf(" "));
                firstName = firstName[0].ToString().ToUpper() + firstName.Substring(1).ToLower();
            }
            else
            {
                firstName = input[0].ToString().ToUpper() + input.Substring(1).ToLower();
            }

            string fullName = "";
            if (input.Contains(" "))
            {
                string surname = input.Substring(input.IndexOf(" ") + 1);
                surname = surname[0].ToString().ToUpper() + surname.Substring(1).ToLower();
                fullName = firstName + " " + surname;
            }
            else
            {
                fullName = firstName;
            };

            return fullName;

        }
        static void addNewElement()
        {
            Console.WriteLine("Podaj imię i nazwisko");
            string input = Console.ReadLine();
            Console.WriteLine("Wpisz nr telefonu");
            string number = Console.ReadLine();
            string infoAboutContact = "";
            bool addedContactToList = false;
                if (dictionary.ContainsKey(getName(input)) && dictionary.ContainsValue(number))
                {
                Console.Clear();
                infoAboutContact = "taki kontakt już istnieje, wpisz jeszcze raz!"; 
                }
            else
            {
                infoAboutContact = "Kontakt dodano do listy!";
                addedContactToList = true;
                dictionary.Add(getName(input), number);
                safeToFile();
            }
            Console.WriteLine(infoAboutContact+"\n");
            if (!addedContactToList) getOption();
        }

        static void safeToFile()
        {
            File.WriteAllLines(path,
                dictionary.Select(x =>   x.Key + "," + x.Value));
        }

    }
}