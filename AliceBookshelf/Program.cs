using System.Text;

namespace AliceBookshelf
{
    internal class Program
    {
        static void Main(string[] args)
        {
            AliceBookshelf(@"..\..\..\input.txt");
        }
        static void AliceBookshelf(string path)
        {
            StreamReader sr = new StreamReader(path);
            string buffer = sr.ReadLine();
            if (buffer == null)
            {
                throw new Exception("invalid file input");
            }
            sr.Close();

            ChainedShelf alice = new ChainedShelf(buffer.ToCharArray());
            string solution = alice.Solved();
            Console.WriteLine("Solution: " + solution);

            StreamWriter sw = new StreamWriter("output.txt");
            sw.WriteLine(solution);
            sw.Close();
            Console.WriteLine($"Also saved to {Directory.GetCurrentDirectory() + @"\output.txt"}");
        }
    }
    public interface IBookshelfItem
    {

    }
    public class Book : IBookshelfItem
    {
        char[] _book;
        public Book(char b)
        {
            _book = new char[] { b };
        }
        public override string ToString()
        {
            return _book[0].ToString();
        }
    }
    public class ChainedShelf : IBookshelfItem
    {
        protected List<IBookshelfItem> bitems = new List<IBookshelfItem>();
        public ChainedShelf(char[] books)
        {
            for (int i = 0; i < books.Length; i++)
            {
                if (books[i] == '/')
                {
                    int level = 0;
                    for (int j = i + 1; j < books.Length; j++)
                    {                       
                        if (books[j] == '/')
                        {
                            level++;
                        }
                        if (books[j] == '\\')
                        {
                            if(level == 0)
                            {
                                char[] newshelf = new char[j - (i + 1)];
                                for(int k = i + 1; k < j; k++)
                                {
                                    newshelf[k - (i + 1)] = books[k];
                                }
                                bitems.Add(new ChainedShelf(newshelf));
                                i = j;
                                break;
                            }
                            else
                            {
                                level--;
                            }
                        }
                        if(j == books.Length - 1)
                        {
                            throw new InvalidDataException("Bad input!");
                        }
                    }
                }
                else
                {
                    bitems.Add(new Book(books[i]));
                }
            }            
        }
        void Solve()
        {
            for(int i = 0; i < bitems.Count; i++)
            {
                if (bitems[i].GetType() == typeof(ChainedShelf))
                {
                    ChainedShelf c = (ChainedShelf)bitems[i];
                    c.Solve();
                    int solvedindex = bitems.IndexOf(c);
                    bitems.Remove(c);
                    for(int j = 0; j < c.bitems.Count; j++)
                    {
                        bitems.Insert(solvedindex, c.bitems[j]);
                    }
                }            
            }
        }
        public string Solved()
        {
            Solve();
            StringBuilder sb = new StringBuilder();
            foreach(IBookshelfItem c in bitems)
            {
                sb.Append(c.ToString());
            }
            return sb.ToString();
        }
    }
}