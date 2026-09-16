using SportPark.DataAccess.Context;
using SportPark.Domains.Entities;
using SportPark.Domains.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace SportPark.DataAccess.Context
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            // ===== УСЛУГИ =====
            await AddServiceIfNotExists(context, "Бассейн", "Бассейн длиной 25 метров для плавания и занятий разными видами водного спорта", "Бассейн");
            await AddServiceIfNotExists(context, "Фитнес", "Просторный зал с современным оборудованием для силовых и кардиотренировок", "Фитнес");
            await AddServiceIfNotExists(context, "Кроссфит", "Зона функциональных тренировок: сила, выносливость и координация", "Кроссфит");
            await AddServiceIfNotExists(context, "Настольный теннис", "Развивает реакцию, координацию и концентрацию", "Настольный теннис");
            await AddServiceIfNotExists(context, "Сквош", "Динамичная игра, развивающая выносливость и скорость реакции", "Сквош");
            await AddServiceIfNotExists(context, "Пилатес", "Контроль движений, дыхание, осанка, гибкость и баланс", "Пилатес");
            await AddServiceIfNotExists(context, "Массаж", "Восстановительные и релаксационные программы массажа", "Массаж");
            await AddServiceIfNotExists(context, "Спа-зона", "Хамам, финская баня, джакузи", "Спа-зона");
            await AddServiceIfNotExists(context, "Групповые программы", "Кардио, тонирование, реабилитация, танцы и функциональные тренировки", "Групповые программы");

            await context.SaveChangesAsync();

            // ===== ТРЕНЕРЫ =====
            // Бассейн
            await AddTrainerIfNotExists(context, "Анна Захарова", "trainer1@sportpark.md", "Бассейн", "Старший тренер по плаванию");
            await AddTrainerIfNotExists(context, "Кристиан Ракул", "trainer2@sportpark.md", "Бассейн", "Персональный тренер по плаванию");
            await AddTrainerIfNotExists(context, "Александр Гуцу", "trainer3@sportpark.md", "Бассейн", "Персональный тренер по плаванию");
            await AddTrainerIfNotExists(context, "Иван Захаров", "trainer4@sportpark.md", "Бассейн", "Персональный тренер по плаванию");
            await AddTrainerIfNotExists(context, "Евгений Себов", "trainer5@sportpark.md", "Бассейн", "Персональный тренер по плаванию");
            await AddTrainerIfNotExists(context, "Игорь Душкевич", "trainer6@sportpark.md", "Бассейн", "Персональный тренер по плаванию");
            await AddTrainerIfNotExists(context, "Илья Замоз", "trainer7@sportpark.md", "Бассейн", "Персональный тренер по плаванию");
            await AddTrainerIfNotExists(context, "Егор Дарабанский", "trainer8@sportpark.md", "Бассейн", "Персональный тренер по плаванию");
            await AddTrainerIfNotExists(context, "Юрий Влас", "trainer9@sportpark.md", "Бассейн", "Персональный тренер по плаванию");
            await AddTrainerIfNotExists(context, "Андрей Лупашку", "trainer10@sportpark.md", "Бассейн", "Персональный тренер по плаванию");

            // Фитнес
            await AddTrainerIfNotExists(context, "Ирина Канарская", "trainer11@sportpark.md", "Фитнес", "Старший фитнес-тренер");
            await AddTrainerIfNotExists(context, "Марчел Стропша", "trainer12@sportpark.md", "Фитнес", "Фитнес-инструктор");
            await AddTrainerIfNotExists(context, "Диана Подоляну", "trainer13@sportpark.md", "Фитнес", "Фитнес-инструктор");
            await AddTrainerIfNotExists(context, "Виктор Рошка", "trainer14@sportpark.md", "Фитнес", "Фитнес-инструктор");
            await AddTrainerIfNotExists(context, "Олег Пасченко", "trainer15@sportpark.md", "Фитнес", "Фитнес-инструктор");
            await AddTrainerIfNotExists(context, "Михай Кроитор", "trainer16@sportpark.md", "Фитнес", "Фитнес-инструктор");
            await AddTrainerIfNotExists(context, "Попа Дану", "trainer17@sportpark.md", "Фитнес", "Фитнес-инструктор");
            await AddTrainerIfNotExists(context, "Мариус Мунтян", "trainer18@sportpark.md", "Фитнес", "Фитнес-инструктор");
            await AddTrainerIfNotExists(context, "Петра Ионуску", "trainer19@sportpark.md", "Фитнес", "Фитнес-тренер");
            await AddTrainerIfNotExists(context, "Гайчан Вадим", "trainer20@sportpark.md", "Фитнес", "Фитнес-тренер");
            await AddTrainerIfNotExists(context, "Игорь Алла", "trainer21@sportpark.md", "Фитнес", "Фитнес-тренер");
            await AddTrainerIfNotExists(context, "Тимур Кырлан", "trainer22@sportpark.md", "Фитнес", "Фитнес-тренер");
            await AddTrainerIfNotExists(context, "Адриана Мунтян", "trainer23@sportpark.md", "Фитнес", "Фитнес-тренер");
            await AddTrainerIfNotExists(context, "Арион Дориан", "trainer24@sportpark.md", "Фитнес", "Фитнес-тренер");

            // Групповые программы
            await AddTrainerIfNotExists(context, "Наталья Войнаровская", "trainer25@sportpark.md", "Групповые программы", "Тренер по групповым программам");
            await AddTrainerIfNotExists(context, "Анна Киструга", "trainer26@sportpark.md", "Групповые программы", "Тренер по групповым программам");
            await AddTrainerIfNotExists(context, "Станислав Суравски", "trainer27@sportpark.md", "Групповые программы", "Тренер по групповым программам");
            await AddTrainerIfNotExists(context, "Ольга Тимошкова", "trainer28@sportpark.md", "Групповые программы", "Тренер по групповым программам");
            await AddTrainerIfNotExists(context, "Сергей Присакару", "trainer29@sportpark.md", "Групповые программы", "Тренер по групповым программам");
            await AddTrainerIfNotExists(context, "Раду Алина", "trainer30@sportpark.md", "Групповые программы", "Тренер по групповым программам");
            await AddTrainerIfNotExists(context, "Инна Остапчук", "trainer31@sportpark.md", "Групповые программы", "Тренер по групповым программам");

            // Кинетотерапия, массаж
            await AddTrainerIfNotExists(context, "Александр Гладко", "trainer32@sportpark.md", "Массаж", "Кинетотерапевт/массажист");
            await AddTrainerIfNotExists(context, "Юлия Ротарь", "trainer33@sportpark.md", "Массаж", "Кинетотерапевт/массажист");
            await AddTrainerIfNotExists(context, "Яцко Михаела", "trainer34@sportpark.md", "Массаж", "Массажист");
            await AddTrainerIfNotExists(context, "Илья Замоз", "trainer35@sportpark.md", "Массаж", "Массажист"); // второй профиль того же человека — другая специализация
            await AddTrainerIfNotExists(context, "Тимур Кырлан", "trainer36@sportpark.md", "Массаж", "Массажист"); // второй профиль того же человека — другая специализация

            // Боевые искусства
            await AddTrainerIfNotExists(context, "Юлия Назарова", "trainer37@sportpark.md", "Боевые искусства", "Тренер по тхэквондо");
            await AddTrainerIfNotExists(context, "Мариус Мунтян", "trainer38@sportpark.md", "Боевые искусства", "Тренер по кикбоксингу"); // второй профиль того же человека

            // Теннис
            await AddTrainerIfNotExists(context, "Роман Тудоряну", "trainer39@sportpark.md", "Настольный теннис", "Инструктор по теннису");
            await AddTrainerIfNotExists(context, "Кэтэлин Тропин", "trainer40@sportpark.md", "Настольный теннис", "Инструктор по теннису");
            await AddTrainerIfNotExists(context, "Тудор Грозав", "trainer41@sportpark.md", "Настольный теннис", "Инструктор по теннису");

            // Сквош
            await AddTrainerIfNotExists(context, "Александр Граб", "trainer42@sportpark.md", "Сквош", "Тренер по сквошу");

            await context.SaveChangesAsync();

            // ===== ПРИМЕРЫ ЗАНЯТИЙ В РАСПИСАНИИ =====
            // Спа-зона намеренно не включена в расписание — туда можно приходить
            // без записи, либо это относится к отдельным сеансам с массажистами
            // (услуга "Массаж"), а не к фиксированным групповым занятиям.
            if (!context.ClassSessions.Any())
            {
                await AddExampleSession(context, "Бассейн", "trainer1@sportpark.md", DayOfWeek.Monday, "09:00", "Бассейн");
                await AddExampleSession(context, "Фитнес", "trainer11@sportpark.md", DayOfWeek.Monday, "10:00", "Tonus");
                await AddExampleSession(context, "Кроссфит", "trainer12@sportpark.md", DayOfWeek.Monday, "18:00", "Crossfit"); // тренер фитнеса
                await AddExampleSession(context, "Настольный теннис", "trainer39@sportpark.md", DayOfWeek.Tuesday, "18:00", "Squash");
                await AddExampleSession(context, "Сквош", "trainer42@sportpark.md", DayOfWeek.Wednesday, "19:00", "Squash");
                await AddExampleSession(context, "Массаж", "trainer32@sportpark.md", DayOfWeek.Thursday, "11:00", "Массажный кабинет");
                await AddExampleSession(context, "Групповые программы", "trainer25@sportpark.md", DayOfWeek.Friday, "17:00", "Intense");
                await AddExampleSession(context, "Пилатес", "trainer26@sportpark.md", DayOfWeek.Friday, "09:00", "Tonus"); // тренер групповых программ

                await context.SaveChangesAsync();
            }
        }

        private static async Task AddServiceIfNotExists(AppDbContext context, string name, string description, string category)
        {
            if (context.Services.Any(s => s.Name == name)) return;

            context.Services.Add(new Service
            {
                Name = name,
                Description = description,
                Category = category
            });
        }

        private static async Task AddTrainerIfNotExists(AppDbContext context, string name, string email, string specialization, string bio)
        {
            if (context.Users.Any(u => u.Email == email)) return;

            var user = new User
            {
                Name = name,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Trainer123!"),
                Role = UserRole.Trainer,
                RegisteredAt = DateTime.UtcNow
            };
            context.Users.Add(user);
            await context.SaveChangesAsync(); // нужно сохранить, чтобы получить user.Id для тренера

            context.Trainers.Add(new Trainer
            {
                UserId = user.Id,
                Specialization = specialization,
                Bio = bio
            });
        }

        private static async Task AddExampleSession(AppDbContext context, string serviceCategory, string trainerEmail, DayOfWeek day, string startTime, string hall)
        {
            var service = context.Services.FirstOrDefault(s => s.Category == serviceCategory);
            var trainer = context.Trainers.FirstOrDefault(t => t.User!.Email == trainerEmail);

            if (service == null || trainer == null) return;

            context.ClassSessions.Add(new ClassSession
            {
                ServiceId = service.Id,
                TrainerId = trainer.Id,
                DayOfWeek = day,
                StartTime = TimeSpan.Parse(startTime),
                Hall = hall
            });

            await Task.CompletedTask;
        }
    }
}
