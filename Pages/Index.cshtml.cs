using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace morse_code_translator.Pages
{
    // Razor Page Model for the Index page, handles Morse code translation logic
    public class IndexModel : PageModel
    {
        // BindProperty allows this property to bind form inputs automatically
        [BindProperty]
        public MorseTranslatorModel Translator { get; set; } = new();

        // Handles HTTP GET requests - no special logic needed on initial load
        public void OnGet()
        {
        }

        // Handler for translating text input to Morse code on form submission
        public IActionResult OnPostTranslateToMorse()
        {
            if (ModelState.IsValid)
            {
                // Convert text input to Morse code and store in MorseResult
                Translator.MorseResult = LettersToMorseCode(Translator.TextInput);
            }

            // Return the current page with updated data
            return Page();
        }

        // Handler for translating Morse code input to plain text on form submission
        public IActionResult OnPostTranslateToText()
        {
            if (ModelState.IsValid)
            {
                // Convert Morse input to text and store in TextResult
                Translator.TextResult = MorseCodeToLetters(Translator.MorseInput);
            }
            return Page();
        }

        private static readonly Dictionary<char, string> MorseCodeDictionary = new()
        {
            // Letters A-Z
            {'A', ".-"}, {'B', "-..."}, {'C', "-.-."}, {'D', "-.."}, {'E', "."},
            {'F', "..-."}, {'G', "--."}, {'H', "...."}, {'I', ".."}, {'J', ".---"},
            {'K', "-.-"}, {'L', ".-.."}, {'M', "--"}, {'N', "-."}, {'O', "---"},
            {'P', ".--."}, {'Q', "--.-"}, {'R', ".-."}, {'S', "..."}, {'T', "-"},
            {'U', "..-"}, {'V', "...-"}, {'W', ".--"}, {'X', "-..-"}, {'Y', "-.--"},
            {'Z', "--.."},
            
            // Numbers 0-9
            {'0', "-----"}, {'1', ".----"}, {'2', "..---"}, {'3', "...--"},
            {'4', "....-"}, {'5', "....."}, {'6', "-...."}, {'7', "--..."},
            {'8', "---.."}, {'9', "----."},
            
            // Punctuation and special characters
            {'.', ".-.-.-"}, {',', "--..--"}, {'?', "..--.."}, {'\'', ".----."},
            {'!', "-.-.--"}, {'/', "-..-."}, {'(', "-.--."}, {')', "-.--.-"},
            {'&', ".-..."}, {':', "---..."}, {';', "-.-.-."}, {'=', "-...-"},
            {'+', ".-.-."}, {'-', "-....-"}, {'_', "..--.-"}, {'"', ".-..-."},
            {'$', "...-..-"}, {'@', ".--.-."},
            
            // Space character
            {' ', "/"}
        };

        private static readonly Dictionary<string, char> ReverseMorseDictionary = new()
        {
            // Letters A-Z
            {".-", 'A'}, {"-...", 'B'}, {"-.-.", 'C'}, {"-..", 'D'}, {".", 'E'},
            {"..-.", 'F'}, {"--.", 'G'}, {"....", 'H'}, {"..", 'I'}, {".---", 'J'},
            {"-.-", 'K'}, {".-..", 'L'}, {"--", 'M'}, {"-.", 'N'}, {"---", 'O'},
            {".--.", 'P'}, {"--.-", 'Q'}, {".-.", 'R'}, {"...", 'S'}, {"-", 'T'},
            {"..-", 'U'}, {"...-", 'V'}, {".--", 'W'}, {"-..-", 'X'}, {"-.--", 'Y'},
            {"--..", 'Z'},
            
            // Numbers 0-9
            {"-----", '0'}, {".----", '1'}, {"..---", '2'}, {"...--", '3'},
            {"....-", '4'}, {".....", '5'}, {"-....", '6'}, {"--...", '7'},
            {"---..", '8'}, {"----.", '9'},
            
            // Punctuation and special characters
            {".-.-.-", '.'}, {"--..--", ','}, {"..--..", '?'}, {".----.", '\''},
            {"-.-.--", '!'}, {"-..-.", '/'}, {"-.--.", '('}, {"-.--.-", ')'},
            {".-...", '&'}, {"---...", ':'}, {"-.-.-.", ';'}, {"-...-", '='},
            {".-.-.", '+'}, {"-....-", '-'}, {"..--.-", '_'}, {".-..-.", '"'},
            {"...-..-", '$'}, {".--.-.", '@'},
            
            // Space character
            {"/", ' '}
        };


        // Converts a plain text string to Morse code
        public static string LettersToMorseCode(string text)
        {
            // Return empty if input is null or whitespace
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            var result = new List<string>(text.Length);

            // Convert each character to uppercase and map to Morse code
            foreach (char character in text.ToUpperInvariant())
            {
                // Try to get Morse code, else add "?" for unknown chars
                if (MorseCodeDictionary.TryGetValue(character, out string? morseChar) && morseChar != null)
                {
                    result.Add(morseChar);
                }
                else
                {
                    result.Add("?"); // handle invalid characters
                }
            }
            // Join Morse code sequences with spaces between letters
            return string.Join(' ', result);
        }

         // Converts Morse code input back into readable text
        private string MorseCodeToLetters(string morseInput)
        {
            // Return empty if input is null or whitespace
            if (string.IsNullOrWhiteSpace(morseInput))
                return string.Empty;

            // Split input into words by '/' delimiter
            var words = morseInput.Trim().Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var decodedWords = new List<string>();

             // Decode each word
            foreach (var word in words)
            {
                // Split word into individual Morse code letters
                var letters = word.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var decodedLetters = new List<char>();

                // Decode each Morse code letter to a char
                foreach (var letter in letters)
                {
                    if (ReverseMorseDictionary.TryGetValue(letter, out char decodedChar))
                    {
                        decodedLetters.Add(decodedChar);
                    }
                    else
                    {
                        decodedLetters.Add('?');
                    }
                }

                // Combine decoded letters to form the decoded word
                decodedWords.Add(new string(decodedLetters.ToArray()));
            }
            // Join decoded words with spaces to form full decoded text
            return string.Join(" ", decodedWords);
        }
    }


    // Data model to hold user input and results, with UI display names
    public class MorseTranslatorModel
    {
        [Display(Name = "Text to translate")]
        public string TextInput { get; set; } = string.Empty;

        [Display(Name = "Morse Code Result")]
        public string MorseResult { get; set; } = string.Empty;

        [Display(Name = "Morse Code to translate")]
        public string MorseInput { get; set; } = string.Empty;

        [Display(Name = "Text Result")]
        public string TextResult { get; set; } = string.Empty;
    }
}