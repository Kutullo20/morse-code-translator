using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace morse_code_translator.Pages;

public class IndexModel : PageModel
{
    [BindProperty]
    public MorseTranslatorModel Translator { get; set; } = new();

    public void OnGet()
    {
    }

    public IActionResult OnPostTranslateToMorse()
    {
        if (ModelState.IsValid)
        {
            Translator.MorseResult = LettersToMorseCode(Translator.TextInput);
        }
        return Page();
    }

    public IActionResult OnPostTranslateToText()
    {
        if (ModelState.IsValid)
        {
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

    public static string LettersToMorseCode(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;

        var result = new List<string>(text.Length);

        foreach (char character in text.ToUpperInvariant())
        {
            if (MorseCodeDictionary.TryGetValue(character, out string? morseChar) && morseChar != null)
            {
                result.Add(morseChar);
            }
            else
            {
                result.Add("?"); // handle invalid characters
            }
        }

        return string.Join(' ', result);
    }

    public static string MorseCodeToLetters(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            return string.Empty;

        var result = new List<char>();
        var morseWords = code.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        foreach (string morseWord in morseWords)
        {
            if (ReverseMorseDictionary.TryGetValue(morseWord, out char letter))
            {
                result.Add(letter);
            }
            else
            {
                result.Add('?'); // handle invalid characters
            }
        }

        return new string(result.ToArray());
    }
        
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

