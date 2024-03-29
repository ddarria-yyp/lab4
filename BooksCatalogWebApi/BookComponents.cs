﻿using BooksCatalog;

namespace BooksCatalogWebApi
{
    public class BooksComponent
    {
        private readonly IConfiguration? _configuration;
        public BookCatalog BookCatalog { get; private set; }
        public BooksComponent(IConfiguration configuration) 
        {
            _configuration = configuration;
            BookCatalog = ReadBooks() ?? new BookCatalog();
        }

        public BooksComponent(IEnumerable<Book> books)
        {
            BookCatalog = new BookCatalog();
            BookCatalog.Books = books.ToList();
        }

        private IReaderWriter GetReaderWriter()
        {
            try
            {
                if (_configuration == null || string.IsNullOrWhiteSpace(_configuration["SavingType"]))
                {
                    return new JsonReaderWriter();
                }

                bool success = Enum.TryParse<SavingType>(_configuration["SavingType"], out SavingType type);
                return success ? GetReaderWriterBySavingType(type) : new JsonReaderWriter();
            }
            catch
            {
                return new JsonReaderWriter();
            }
        }

        public IReaderWriter GetReaderWriterBySavingType(SavingType savingType)
        {
            switch (savingType)
            {
                case SavingType.JSon:
                    return new JsonReaderWriter();
                case SavingType.Xml:
                    return new XmlReaderWriter();
                case SavingType.SqLite:
                    return new SqLiteReaderWriter();
                default:
                    return new JsonReaderWriter();
            }
        }

        private void Save()
        {
            IReaderWriter readerWriter = GetReaderWriter();
            readerWriter.Save(BookCatalog);
        }

        private BookCatalog ReadBooks()
        {
            return GetReaderWriter().ReadBooks();
        }

        private IEnumerable<Book> FindByAuthor(string author)
        {
            return BookCatalog.Books.Where(x => x.Author == author);
        }

        private IEnumerable<Book> FindByISBN(string isbn)
        {
            return BookCatalog.Books.Where(x => x.ISBN == isbn);
        }

        private IEnumerable<Book> FindByTitle(string title)
        {
            return BookCatalog.Books.Where(x => x.Title.Contains(title));
        }

        private IEnumerable<Book> FindByKeyWords(string[] keyWords)
        {
            Dictionary<Book, int> books = new Dictionary<Book, int>();
            foreach (string word in keyWords)
            {
                string keyWord = word.Trim();
                foreach (var book in BookCatalog.Books)
                {
                    if (book.Brief.Contains(keyWord))
                    {
                        if (books.ContainsKey(book))
                        {
                            books[book] = books[book] + 1;
                        }
                        else
                        {
                            books.Add(book, 1);
                        }
                    }
                }
            }

            return books.OrderByDescending(x => x.Value).Select(x => x.Key).ToList();
        }

        public IEnumerable<Book> FindBooksByCategory(Category category, string value)
        {
            switch (category)
            {
                case Category.Author: return FindByAuthor(value);
                case Category.ISBN: return FindByISBN(value);
                case Category.Title: return FindByTitle(value);
                case Category.Keyword: return FindByKeyWords(value.Split(','));
                default: return new List<Book>();
            }
        }

        public void AddBook(Book book)
        {
            BookCatalog.Books.Add(book);
            Save();
        }

    }
}
