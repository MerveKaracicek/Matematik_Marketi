using Matematik_Marketi.Data;
using Matematik_Marketi.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

public class QuestionSeeder
{
    public static void SeedQuestions(AppDbContext context)
    {
        if (context.Questions.Any()) return; // Seed sadece bir kez

        var random = new Random();
        var questions = new List<Question>();
        int targetCount = 150;

        while (questions.Count < targetCount)
        {
            try
            {
                var question = GenerateQuestion(random);
                questions.Add(question);
            }
            catch
            {
                // Hatalı soru → atla, DB'ye eklenmez
            }
        }

        context.Questions.AddRange(questions);
        context.SaveChanges();
    }

    private static Question GenerateQuestion(Random random)
    {
        int a = random.Next(1, 16); // 1-15 arası sayı
        int b = random.Next(1, 16);
        int c = random.Next(1, 16);

        string[] operations = { "+", "-", "*", "/" };
        string op1 = operations[random.Next(operations.Length)];
        string op2 = operations[random.Next(operations.Length)];

        // %50 olasılıkla parantez ekle
        string expression;
        if (random.Next(0, 2) == 1)
        {
            expression = $"{a} {op1} ({b} {op2} {c})";
        }
        else
        {
            expression = $"{a} {op1} {b} {op2} {c}";
        }

        // Cevabı hesapla, negatif veya bölme hatası varsa exception fırlat
        int result = EvaluateExpression(a, b, c, op1, op2, expression.Contains("("));

        return new Question
        {
            QuestionText = expression,
            CorrectAnswer = result.ToString()
        };
    }

    private static int EvaluateExpression(int a, int b, int c, string op1, string op2, bool hasParentheses)
    {
        int inner, result;

        if (hasParentheses)
        {
            inner = op2 switch
            {
                "+" => b + c,
                "-" when b - c >= 0 => b - c,
                "*" => b * c,
                "/" when c != 0 && b % c == 0 => b / c,
                _ => throw new InvalidOperationException()
            };

            result = op1 switch
            {
                "+" => a + inner,
                "-" when a - inner >= 0 => a - inner,
                "*" => a * inner,
                "/" when inner != 0 && a % inner == 0 => a / inner,
                _ => throw new InvalidOperationException()
            };
        }
        else
        {
            // Sıralı işlem (öncelik * ve /)
            int first, second;

            (first, second) = (op1 == "*" || op1 == "/") ? (a, b) : (b, c);

            int temp1 = op1 switch
            {
                "+" => a + b,
                "-" when a - b >= 0 => a - b,
                "*" => a * b,
                "/" when b != 0 && a % b == 0 => a / b,
                _ => throw new InvalidOperationException()
            };

            result = op2 switch
            {
                "+" => temp1 + c,
                "-" when temp1 - c >= 0 => temp1 - c,
                "*" => temp1 * c,
                "/" when c != 0 && temp1 % c == 0 => temp1 / c,
                _ => throw new InvalidOperationException()
            };
        }

        if (result < 0) throw new InvalidOperationException(); // Negatif sonuç yok
        return result;
    }
}
