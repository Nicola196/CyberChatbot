// ============================================================
// FILE: Program.cs
// PURPOSE: Entry point -- controls the full startup sequence.
//   1. Play voice greeting      3. Ask for the user's name
//   2. Display ASCII art        4. Run the main chat loop
// FRAMEWORK: .NET Framework (Console App) -- no NuGet needed
// ============================================================

using Cyber_securityChatbot;
using System;
using System.Linq;      // .Where() for punctuation removal
using System.Media;     // SoundPlayer for .wav voice greeting
using System.Threading; // Thread.Sleep for all animation delays

namespace CybersecurityChatbot
{
    internal class Program
    {
        // --------------------------------------------------------
        // ENTRY POINT: runs startup sequence then launches chatbot
        // --------------------------------------------------------
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8; // Fix encoding for .NET Framework
            PlayVoiceGreeting(); // Step 1: play .wav on launch

            // Step 2: ASCII art welcome banner
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(@"
 __        __   _                           _  
 \ \      / /__| | ___  ___  _ __ ___   ___| | 
  \ \ /\ / / _ \ |/ __/ _ \| '_ ` _ \ / _ \ | 
   \ V  V /  __/ | (_| (_) | | | | | |  __/_| 
    \_/\_/ \___|_|\___\___/|_| |_| |_|\___(_)  
  *** Cybersecurity Awareness Chatbot ***");

            Console.ResetColor();
            // Step 3: Decorative header banner
            Console.ForegroundColor = ConsoleColor.DarkCyan; Console.WriteLine("\n====================================================\n       [ CYBERSECURITY AWARENESS CHATBOT ]          \n====================================================");
            Console.ResetColor();
            // Step 4: Ask for name -- keep asking until a valid name is entered
            AnimateText("\nBot: Hello! Before we begin, what is your name?", ConsoleColor.Yellow);
            Console.ForegroundColor = ConsoleColor.Blue; Console.Write("You: "); Console.ResetColor();
            string userName = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(userName))
            {
                AnimateText("Bot: Please enter your name so I can personalise our chat.", ConsoleColor.Red);
                Console.Write("You: "); userName = Console.ReadLine();
            }

            StartChatbot(new User(userName)); // Step 5 & 6: create user and start chat
            Console.WriteLine("\nPress any key to exit..."); Console.ReadKey();
        }

        // --------------------------------------------------------
        // StartChatbot: main conversation loop -- runs until 'exit'.
        // --------------------------------------------------------
        static void StartChatbot(User user)
        {
            // Personalised welcome and topic list
            AnimateText("\nBot: How can i assist you? " + user.Name + "?", ConsoleColor.Yellow);
            AnimateText("Bot: Ask me about: CYBERSECURITY | PHISHING | SUSPICIOUS LINKS | PREVENT PHISHING | REPORT | PASSWORD | BROWSING", ConsoleColor.Cyan);
            AnimateText("Bot: Type 'help' to see topics again. Type 'exit' to quit.", ConsoleColor.Cyan);

            IChatbot chatbot = new Chatbot(); // Use interface for good coding practice

            // Main loop -- runs until the user types 'exit'
            while (!user.IsExiting)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("\n" + user.Name + ": "); Console.ResetColor();
                string input = Console.ReadLine();
                // Reject blank input
                if (string.IsNullOrWhiteSpace(input))
                { AnimateText("Bot: Please enter something so I can assist you.", ConsoleColor.Red); continue; }
                // Handle exit command
                if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
                { AnimateText("\nBot: Goodbye! Stay safe online!", ConsoleColor.DarkGreen); user.IsExiting = true; break; }
                // Strip punctuation so "phishing!" still matches "phishing"
                input = new string(input.Where(c => !char.IsPunctuation(c)).ToArray()).Trim();

                ShowProgressBar();      // Step A: animated loading bar
                ShowTypingIndicator();  // Step B: "Bot is typing..." dots
                chatbot.ProcessInput(input); // Step C: response types out in Chatbot.cs
            }
        }

        // --------------------------------------------------------
        // AnimateText: types text character by character with colour.
        // --------------------------------------------------------
        static void AnimateText(string text, ConsoleColor col = ConsoleColor.White, int delay = 30)
        {
            Console.ForegroundColor = col;
            foreach (char c in text) { Console.Write(c); Thread.Sleep(delay); }
            Console.WriteLine(); Console.ResetColor();
        }

        // --------------------------------------------------------
        // ShowTypingIndicator: shows "Bot is typing..." with animated
        // dots (. .. ...), then clears the line before the response.
        // --------------------------------------------------------
        static void ShowTypingIndicator()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            int left = Console.CursorLeft, top = Console.CursorTop;
            for (int i = 0; i < 3; i++) // Animate dots one at a time
            {
                Console.SetCursorPosition(left, top);
                Console.Write("Bot is typing" + new string('.', i + 1) + "   "); Thread.Sleep(400);
            }
            // Clear the typing line so the real response appears cleanly
            Console.SetCursorPosition(left, top); Console.Write(new string(' ', 30));
            Console.SetCursorPosition(left, top); Console.ResetColor(); Console.WriteLine();
        }

        // --------------------------------------------------------
        // ShowProgressBar: animated bar -- Processing... [====] 100%
        // Uses only Console + Thread.Sleep, no NuGet packages needed.
        // --------------------------------------------------------
        static void ShowProgressBar()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("\nProcessing... [");
            int left = Console.CursorLeft, top = Console.CursorTop;
            for (int i = 0; i <= 10; i++) // 10 steps = 100%
            {
                Console.SetCursorPosition(left, top);
                Console.Write(new string('=', i) + new string(' ', 10 - i) + "] " + (i * 10) + "%  ");
                Thread.Sleep(80); // 80ms between each step
            }
            Console.ResetColor(); Console.WriteLine();
        }

        // --------------------------------------------------------
        // PlayVoiceGreeting: plays VoiceGreeting.wav from the 'voice'
        // folder on launch. Shows a warning if the file is missing.
        // SETUP: Set .wav "Copy to Output Directory" to "Copy always"
        //        in the file's Visual Studio Properties panel.
        // --------------------------------------------------------
        static void PlayVoiceGreeting()
        {
            try { new SoundPlayer(@"Audio.wav").Play(); }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("[Note: Voice greeting could not be played - " + ex.Message + "]");
                Console.ResetColor();
            }
        }

        
    }
}