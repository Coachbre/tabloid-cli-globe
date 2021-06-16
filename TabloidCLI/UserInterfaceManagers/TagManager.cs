﻿// Manager files handle the connections between the data returned from SQL requests and user interface
using System;
using System.Collections.Generic;
using TabloidCLI.Models;

namespace TabloidCLI.UserInterfaceManagers
{
    public class TagManager : IUserInterfaceManager
    {
        private readonly IUserInterfaceManager _parentUI;
        private TagRepository _tagRepository;
        private string _connectionString;
        // _ denotes private properties that are being created and inherited

        public TagManager(IUserInterfaceManager parentUI, string connectionString)
            // constructor used to create new objects
        {
            _parentUI = parentUI;
            //_parentUI is used to go back to the main menu
            _tagRepository = new TagRepository(connectionString);
            _connectionString = connectionString;
        }

        public IUserInterfaceManager Execute()
        {
            Console.WriteLine("Tag Menu");
            Console.WriteLine(" 1) List Tags");
            Console.WriteLine(" 2) Add Tag");
            Console.WriteLine(" 3) Edit Tag");
            Console.WriteLine(" 4) Remove Tag");
            Console.WriteLine(" 0) Go Back");

            Console.Write("> ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    List();
                    return this;
                case "2":
                    Add();
                    return this;
                case "3":
                    Edit();
                    return this;
                case "4":
                    Remove();
                    return this;
                case "0":
                    return _parentUI;
                default:
                    Console.WriteLine("Invalid Selection");
                    return this;
            }
        }

        //All methods are private in this project
        private void List()
        {
            List<Tag> tags = _tagRepository.GetAll();
            foreach (Tag tag in tags)
            {
                Console.WriteLine(tag);
            }
            //get full list of tags via GetAll()
            // for each tag object in the list, show full data
        }

        private Tag Choose( string prompt = null)
        {
            if (prompt == null)
            {
                prompt = "Please choose a tag:";
            }

            Console.WriteLine(prompt);

            List<Tag> tags = _tagRepository.GetAll();

            for (int i = 0; i < tags.Count; i++)
                {
                Tag tag = tags[i];
                Console.WriteLine($" {i + 1}) { tag.Name}");
            }
            Console.Write(">");

            string input = Console.ReadLine();
            try
            {
                int choice = int.Parse(input);
                return tags[choice - 1];
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid selection");
                return null;
            }
        }

        private void Add()
        {
            Console.WriteLine("Time for a new tag!");
            Tag tag = new Tag();

            Console.WriteLine("Enter the new tag name:");
            tag.Name = Console.ReadLine();

            _tagRepository.Insert(tag);
        }

        private void Edit()
        {
            Tag tagToEdit = Choose("Which tag would you like to change?");
            if (tagToEdit == null)
            {
                return;
            }

            Console.WriteLine();
            Console.Write("New tag name (blank to leave unchaged: ");
            string Name = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(Name))
            {
                tagToEdit.Name = Name;
            }

            _tagRepository.Update(tagToEdit);
            //Update tag name with user input
        }

        private void Remove()
        {
            Tag tagToDelete = Choose("Which tag would you like to delete?");
            if (tagToDelete !=null)
            {
                _tagRepository.Delete(tagToDelete.Id);
            }
        }
    }
}
