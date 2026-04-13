using System;
using System.Collections.Generic;
using System.Threading;

namespace CybersecurityChatbot
{
    public interface IChatbot
    {
        void ProcessInput(string input);
    }

    public class Chatbot : IChatbot
    {

        // EMOTIONS
      
        static readonly (string[] keys, ConsoleColor col, string[] lines)[] Emotions =
        {
            (new string[] { "happy","great","good mood","joyful","cheerful","wonderful","fantastic","feeling good" },
             ConsoleColor.Yellow,
             new string[] {
                 "That is wonderful to hear! Your positive energy is contagious!",
                 "A happy mindset combined with good cyber habits is unstoppable."
             }),

            (new string[] { "sad","unhappy","feeling down","depressed","miserable","heartbroken","upset","crying","not okay" },
             ConsoleColor.Cyan,
             new string[] {
                 "I am really sorry to hear that you are feeling this way.",
                 "It is completely okay to have difficult days. Please be kind to yourself."
             }),

            (new string[] { "angry","furious","frustrated","mad","irritated","annoyed","rage","livid","fed up" },
             ConsoleColor.Red,
             new string[] {
                 "I can hear that you are feeling really frustrated right now.",
                 "That is completely valid. Take a deep breath -- you are in a safe space here."
             }),

            (new string[] { "anxious","anxiety","worried","nervous","scared","afraid","fear","panicking","overwhelmed","stressed" },
             ConsoleColor.Magenta,
             new string[] {
                 "It sounds like you are feeling a bit overwhelmed right now.",
                 "Breathe easy. I will guide you through everything step by step. No rush."
             }),

            (new string[] { "excited","thrilled","pumped","hyped","cant wait","can't wait","stoked","energised","energized","enthusiastic" },
             ConsoleColor.Green,
             new string[] {
                 "Woohoo! I love that energy! Let's make the most of it!",
                 "Cybersecurity is one of the most exciting and relevant skills today. Let's dive in!"
             })
        };

        // =============================
        // TOPICS
        // =============================
        static readonly Dictionary<string, (string intro, string[] opts, string[][] det)> Topics =
            new Dictionary<string, (string, string[], string[][])>()
        {
            {
                "cybersecurity",
                ("Cybersecurity protects computers, networks, and data from digital attacks.",
                new string[] {
                    "1. Why is it important?",
                    "2. What are cyber threats?",
                    "3. How does it protect me?"
                },
                new string[][]
                {
                    new string[] { "SA has one of the highest cybercrime rates in Africa. Attacks cause financial loss and data breaches." },
                    new string[] { "Phishing: fake messages. Malware: harmful software. Ransomware: locks files for payment." },
                    new string[] { "Antivirus removes threats. Firewalls block access. Encryption scrambles your data." }
                })
            },

            {
                "phishing",
                ("Phishing emails are fake messages designed to trick you into giving away personal information.",
                new string[] {
                    "1. How do I spot one?",
                    "2. What do I do if I receive one?",
                    "3. What makes them convincing?"
                },
                new string[][]
                {
                    new string[] { "Sender email looks wrong (e.g. paypa1.com). Creates urgency. Contains spelling mistakes." },
                    new string[] { "Do NOT click links or attachments. Report as phishing. If you clicked, change passwords immediately." },
                    new string[] { "They copy real logos and layouts. Use your real name. Create fear so you act without thinking." }
                })
            }
        };

        // =============================
        // MAIN PROCESS
        // =============================
        public void ProcessInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return;

            string lower = input.ToLowerInvariant();

            // STEP 1: Emotion detection
            foreach (var emotion in Emotions)
            {
                foreach (string key in emotion.keys)
                {
                    if (lower.Contains(key))
                    {
                        Console.ForegroundColor = emotion.col;
                        Console.WriteLine();

                        foreach (string line in emotion.lines)
                            AnimateResponse(line);

                        Console.ResetColor();
                        Console.WriteLine();

                        AnimateResponse("Whenever you are ready, I am here to help you stay safe online.");
                        AnimateResponse("Type 'help' to see what topics I can assist with.");
                        return;
                    }
                }
            }

            // STEP 2: Help / greeting
            if (lower == "help" || lower.Contains("what can you do"))
            {
                AnimateResponse("Topics: CYBERSECURITY | PHISHING");
                return;
            }

            if (lower.Contains("how are you"))
            {
                AnimateResponse("I am running at full security capacity! How can I help?");
                return;
            }

            // STEP 3: Topics
            foreach (var topic in Topics)
            {
                if (!lower.Contains(topic.Key))
                    continue;

                var data = topic.Value;

                AnimateResponse(data.intro + "\n");

                foreach (string opt in data.opts)
                    AnimateResponse(opt);

                AnimateResponse("\nType 1, 2, or 3:");
                string choice = Console.ReadLine();

                Console.WriteLine();

                string[] selected = null;

                int num;
                if (int.TryParse(choice, out num) && num >= 1 && num <= data.det.Length)
                {
                    selected = data.det[num - 1];
                }

                if (selected != null)
                {
                    foreach (string line in selected)
                        AnimateResponse(line);
                }
                else
                {
                    AnimateResponse("Invalid choice. Please type 1, 2, or 3.");
                }

                AnimateResponse("Anything else? Type 'help' to continue.");
                return;
            }

            // DEFAULT
            AnimateResponse("I did not understand that.");
            AnimateResponse("Type 'help' to see options.");
        }

        // =============================
        // TYPING EFFECT
        // =============================
        private void AnimateResponse(string message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Bot: ");

            foreach (char c in message)
            {
                Console.Write(c);
                Thread.Sleep(10);
            }

            Console.WriteLine();
            Console.ResetColor();
        }
    }
}