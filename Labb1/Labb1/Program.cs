while (true)
{
    ConsoleKeyInfo keyInfo = Console.ReadKey();

    if (keyInfo.Key == ConsoleKey.Enter)
    {
        Random random = new Random();
        for (int i = 0; i < 8; i++)
        {
            Console.Write(random.Next(1, 36) + " ");
        }
    }
}