using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EmployeeINC.Database.Tables;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace EmployeeINC.Telegram
{
    public static class TelegramBot
    {
        private static TelegramBotClient _botClient;

        private const string TOKEN = @"7078157731:AAE-28Tkzbb61yyBBveNk8xRIv7G1A1ShnU";

        public static async Task Initialize()
        {
            _botClient = new TelegramBotClient($"{TOKEN}");

            using CancellationTokenSource cts = new();

            // StartReceiving does not block the caфорller thread. Receiving is done on the ThreadPool.
            ReceiverOptions receiverOptions = new()
            {
                AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
            };

            _botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );

            var me = await _botClient.GetMeAsync(cancellationToken: cts.Token);

            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();

            // Send cancellation request to stop bot
            cts.Cancel();

            async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
                CancellationToken cancellationToken)
            {
                // Обрабатывать только обновления сообщений: https://core.telegram.org/bots/api#message
                if (update.Message is not { } message)
                    return;
                // Обрабатывать только текстовые сообщения
                if (message.Text is not { } messageText)
                    return;


                var chatId = message.Chat.Id;

                // Эхо получило текст сообщения
                Console.WriteLine(GetAnswer(messageText, message.Chat.Username));
                
                Message sentMessage = await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: GetAnswer(messageText, message.Chat.Username),
                    cancellationToken: cancellationToken);
            }

            Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception,
                CancellationToken cancellationToken)
            {
                var ErrorMessage = exception switch
                {
                    ApiRequestException apiRequestException
                        => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                    _ => exception.ToString()
                };

                Console.WriteLine(ErrorMessage);
                return Task.CompletedTask;
            }
        }

        private static string GetAnswer(string message, string login)
        {
            Сотрудники сотрудник = (Сотрудники)new Сотрудники()
                .ConvertToTables(DB.Database.ExecuteQuery($"SELECT * FROM Сотрудники WHERE tg_username = '{login}'"))
                .FirstOrDefault();
            if (сотрудник == null) return "Вас нет в базе данных!";
            DateTime начало;
            DateTime конец;
            TimeSpan timeBetween;
            int amount;
            string список;
            switch (message)
            {
                case "Команды":
                    return "Вот список команд:\n" +
                           " * Отпуски - выводит спосок отпусков\n" +
                           " * Отпускные дни - выводит сколько дней у вас накопилось\n" +
                           " * Больничный - выводит сумму ваших больничных и даты";
                case "Отпуски":
                    var отпуски = (Отпуски[])new Отпуски().ConvertToTables(DB.Database.ExecuteQuery(
                            $"SELECT * FROM Отпуски WHERE ID_Сотрудника = {сотрудник.ID_Сотрудника}"));
                    список = "";
                    for (int i = 0; i < отпуски.Length; i++)
                    {
                        список += $"{i + 1}) С {отпуски[i].Дата_начала} по {отпуски[i].Дата_завершения}\n";
                    }
                    return $"Список:\n{список}\n";
                case "Отпускные дни":
                    // Установка начальной даты для отсчета
                    DateTime startDate = DateTime.ParseExact(сотрудник.Дата_начала_работы, "dd.MM.yyyy H:mm:ss", null);
                    DateTime referenceDate = startDate < new DateTime(startDate.Year, 1, 1) ? new DateTime(startDate.Year, 1, 1) : startDate;

                    // Расчет общего количества рабочих дней между начальной датой и текущей датой, исключая выходные
                    float totalDays = 0;
                    for (DateTime date = referenceDate; date < DateTime.Now; date = date.AddDays(1))
                    {
                        if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                        {
                            totalDays += 1 / 29.3f;
                        }
                    }

                    // Определение количества накопленных отпускных дней, учитывая ограничение в 28 дней
                    float vacationDays = Math.Min(totalDays, 28);
                    return $"Отпускных дней накопилось: {vacationDays}";
                case "Больничный":
                    var больничные = (Больничные[])new Больничные().ConvertToTables(
                        DB.Database.ExecuteQuery(
                            $"SELECT * FROM Больничные WHERE ID_Сотрудника = {сотрудник.ID_Сотрудника}"));

                    if (больничные == null || больничные.Length == 0) return "Список пустой!";
                    начало = DateTime.ParseExact(больничные.Last().Дата_начала, "dd.MM.yyyy H:mm:ss", null);
                    конец = DateTime.ParseExact(больничные.Last().Дата_завершения, "dd.MM.yyyy H:mm:ss", null);
                    timeBetween = конец - начало;
                    amount = timeBetween.Days;
                    список = "";
                    for (int i = 0; i < больничные.Length; i++)
                    {
                        список +=
                            $"{i + 1}) С {больничные[i].Дата_начала} по {больничные[i].Дата_завершения}\n" +
                            $"Номер: {больничные[i].Номер_больничного}\n" +
                            $"Диагноз: {больничные[i].Диагноз}\n\n";
                    }

                    return "Вот ваши больничные:\n" +
                           $"Всего больничных дней [{amount}]\n\n" +
                           "Список:\n" +
                           $"{список}";
                default:
                    return "Я не знаю такой команды. Список команд - 'Команды'";
            }
        }
    }
}