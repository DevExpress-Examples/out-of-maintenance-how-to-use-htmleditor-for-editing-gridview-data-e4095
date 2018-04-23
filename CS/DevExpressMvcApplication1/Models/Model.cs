using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DevExpressMvcApplication1.Models {
    public class Repository {
        public static IList<Person> Persons {
            get {
                if (HttpContext.Current.Session["Persons"] == null)
                    HttpContext.Current.Session["Persons"] = GeneratePersons();
                return (IList<Person>)HttpContext.Current.Session["Persons"];
            }
        }
        public static Person GetPersonByID(int id) {
            return (from person in Persons where person.ID == id select person).SingleOrDefault<Person>();
        }
        public static int GeneratePersonID() {
            return Persons.Count > 0 ? Persons.Last().ID + 1 : 0;
        }
        public static void InsertPerson(Person person) {
            if (person != null) {
                person.ID = GeneratePersonID();
                Persons.Add(person);
            }
        }
        public static void UpdatePerson(Person person) {
            Person editablePerson = GetPersonByID(person.ID);
            if (editablePerson != null) {
                editablePerson.ID = person.ID;
                editablePerson.FirstName = person.FirstName;
                editablePerson.SecondName = person.SecondName;
                editablePerson.Description = person.Description;
            }
        }
        public static void RemovePersonByID(int id) {
            Person editablePerson = GetPersonByID(id);
            if (editablePerson != null)
                Persons.Remove(editablePerson);
        }
        static IList<Person> GeneratePersons() {
            return new List<Person>{
                        new Person(){
                            ID = 0,
                            FirstName = "Nick",
                            SecondName = "F",
                            Description = "<b>Admin</b>"
                        },
                        new Person(){
                            ID = 1,
                            FirstName = "Jain",
                            SecondName = "K",
                            Description = "<i>User</i>"
                        },
                        new Person(){
                            ID = 2,
                            FirstName = "Loren",
                            SecondName = "F",
                            Description = "<u>Guest</u>"
                        },
                        new Person(){
                            ID = 3,
                            FirstName = "Mike",
                            SecondName = "N",
                            Description = "Undefined"
                        }
                    };
        }
    }

    public class Person {
        public int ID { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Description { get; set; }
    }
}