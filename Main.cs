program cs

using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        // Инициализация игровой коллекции (наборов фишек домино)
        List<List<int>> gameSets = new List<List<int>>();
        
        // Генерация 15 случайных наборов фишек
        for (int i = 0; i < 15; i++)
            gameSets.Add(GenerateRandomSet());
        
        // Количество раундов (окончание игры при остатке 2-х игроков)
        int roundsCount = gameSets.Count;
        
        // Начальные очки игрока и компьютера
        int playerScore = 0;
        int computerScore = 0;
        
        // Вывод приветствия
        Console.WriteLine("Добро пожаловать в игру Домино!");
        Console.WriteLine("Игра начинается с 15 наборами фишек.");
        
        // Цикл по раундам
        while (roundsCount > 2)
        {
            // Выводим текущий раунд
            Console.WriteLine($"Начало раунда {roundsCount}");
            
            // Выбор первого хода игроком
            int playerChoice = ChoosePlayerMove();
            
            // Компьютер делает ход после игрока
            int computerChoice = ComputerMove(playerChoice);
            
            // Оценка текущего хода
            CheckAndCalculatePoints(playerChoice, computerChoice, ref playerScore, ref computerScore);
            
            // Уменьшение количества раундов
            roundsCount--;
        }
        
        // Подсчет итоговых очков
        if (playerScore > computerScore)
            Console.WriteLine("Поздравляем! Вы выиграли!");
        else if (computerScore > playerScore)
            Console.WriteLine("Компьютер победил!");
        else
            Console.WriteLine("Ничья!");
        
        // Завершение программы
        Console.ReadLine();
    }
    
    private static List<int> GenerateRandomSet()
    {
        Random random = new Random();
        List<int> set = new List<int>();
        
        // Каждая фишка имеет два числа от 0 до 6
        for (int i = 0; i < 7; i++)
        {
            int number1 = random.Next(0, 7);
            int number2 = random.Next(0, 7);
            set.Add(number1 * 10 + number2);
        }
        
        return set;
    }
    
    private static int ChoosePlayerMove()
    {
        bool validInput = false;
        int choice = -1;
        
        do
        {
            Console.Write("Введите номер фишки для хода: ");
            string input = Console.ReadLine();
            
            try
            {
                choice = Convert.ToInt32(input);
                
                if (choice >= 0 && choice <= 49)
                    validInput = true;
                else
                    throw new ArgumentOutOfRangeException();
            }
            catch (FormatException)
            {
                Console.WriteLine("Неправильный формат ввода. Введите число от 0 до 49.");
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Число должно быть от 0 до 49.");
            }
        } while (!validInput);
        
        return choice;
    }
    
    private static int ComputerMove(int playerChoice)
    {
        // Получение всех возможных ходов компьютера
        List<int> possibleComputerMoves = GetPossibleComputerMoves(playerChoice);
        
        // Случайный выбор из возможных ходов
        Random random = new Random();
        int index = random.Next(possibleComputerMoves.Count);
        return possibleComputerMoves[index];
    }
    
    private static List<int> GetPossibleComputerMoves(int playerChoice)
    {
        List<int> possibleComputerMoves = new List<int>();
        
        foreach (var set in gameSets)
        {
            foreach (var item in set)
            {
                if ((item % 10 == playerChoice / 10 || item % 10 == playerChoice % 10) && !possibleComputerMoves.Contains(item))
                    possibleComputerMoves.Add(item);
            }
        }
        
        return possibleComputerMoves;
    }
    
    private static void CheckAndCalculatePoints(int playerChoice, int computerChoice, ref int playerScore, ref int computerScore)
    {
        // Если компьютер может поставить козырь, он выигрывает раунд
        if (IsWinningMove(computerChoice))
        {
            Console.WriteLine("Компьютер выиграл раунд!");
            computerScore += 20;
        }
        else if (IsWinningMove(playerChoice))
        {
            Console.WriteLine("Вы выиграли раунд!");
            playerScore += 20;
        }
        else
        {
            Console.WriteLine("Раунд продолжается...");
        }
    }
    
    private static bool IsWinningMove(int move)
    {
        return gameSets.All(set => set.Count(x => x == move) == 0);
    }
}
