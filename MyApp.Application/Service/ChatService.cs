using System.Text.RegularExpressions;
using MyApp.Application.Dto.Request;
using MyApp.Application.Dto.Response;
using MyApp.Application.Interface.Repository;
using MyApp.Application.Interface.Service;
using MyApp.Domain.Config;
using OpenAI;
using OpenAI.Chat;

namespace MyApp.Application.Service;

public class ChatService(
    IBookRepository bookRepository,
    OpenAIClient openAiClient,
    OpenAiConfig openAiConfig
) : IChatService
{
    private static readonly HashSet<string> StopWords = new(StringComparer.OrdinalIgnoreCase)
    {
        "có", "không", "shop", "ơi", "ạ", "nha", "nhé", "giúp", "cho", "mình",
        "em", "anh", "chị", "bạn", "với", "nào", "ko", "k", "còn", "hả", "vậy", "thế"
    };

    private static readonly HashSet<string> GenericWords = new(StringComparer.OrdinalIgnoreCase)
    {
        "sách", "truyện", "cuốn", "bộ", "tập"
    };

    private static readonly Dictionary<string, List<string>> KeywordAliases = new(StringComparer.OrdinalIgnoreCase)
    {
        ["songoku"] = new() { "songoku", "dragon ball", "7 viên ngọc rồng" },
        ["goku"] = new() { "goku", "dragon ball", "7 viên ngọc rồng" },
        ["doremon"] = new() { "doremon", "doraemon" },
        ["doraemon"] = new() { "doraemon", "doremon" },
        ["conan"] = new() { "conan", "thám tử lừng danh conan" },
        ["one piece"] = new() { "one piece", "đảo hải tặc" }
    };

    public async Task<ChatResponse> ChatAsync(string userQuestion, List<MessageDto> history)
    {
        var searchFilter = await ExtractSearchFilterAsync(userQuestion);

        Console.WriteLine($"=== Extracted TypeBook: {searchFilter.TypeBook}");
        Console.WriteLine($"=== Extracted MinPrice: {searchFilter.MinPrice}");
        Console.WriteLine($"=== Extracted MaxPrice: {searchFilter.MaxPrice}");
        Console.WriteLine($"=== Extracted Keywords: {string.Join(", ", searchFilter.Keywords ?? new List<string>())}");

        var products = await bookRepository.SearchAsync(searchFilter);
        Console.WriteLine($"=== Tìm được: {products.Count} sản phẩm");

        var productContext = products.Any()
            ? string.Join("\n---\n", products.Select(p =>
                $"Tên: {p.Name} | Danh mục: {p.TypeBook?.Name ?? "Không rõ"} | Giá: {p.Price:N0} VNĐ | Tồn kho: {p.Stock} | Mô tả: {p.Description ?? "Không có mô tả"}"))
            : "Không tìm thấy sản phẩm phù hợp.";

        var messages = new List<ChatMessage>
        {
            new SystemChatMessage($"""
                Bạn là trợ lý tư vấn sản phẩm chuyên nghiệp.
                Chỉ trả lời dựa trên dữ liệu sản phẩm bên dưới.
                Nếu không có sản phẩm phù hợp, hãy thông báo thật với khách hàng.
                Trả lời bằng tiếng Việt, thân thiện và chuyên nghiệp.

                DỮ LIỆU SẢN PHẨM:
                {productContext}
                """)
        };

        foreach (var msg in history ?? new List<MessageDto>())
        {
            if (msg.Role == "user") messages.Add(new UserChatMessage(msg.Content));
            else if (msg.Role == "assistant") messages.Add(new AssistantChatMessage(msg.Content));
        }

        messages.Add(new UserChatMessage(userQuestion));

        var chatClient = openAiClient.GetChatClient(openAiConfig.Model);
        var response = await chatClient.CompleteChatAsync(messages);

        return new ChatResponse
        {
            message = response.Value.Content[0].Text
        };
    }

    private Task<SearchFilterDto> ExtractSearchFilterAsync(string userQuestion)
    {
        Console.WriteLine($"=== User Question (Extract): {userQuestion}");

        var typeBook = DetectTypeBook(userQuestion);
        var minPrice = ExtractMinPrice(userQuestion);
        var maxPrice = ExtractMaxPrice(userQuestion);
        var keywords = ExtractKeywordsSmart(userQuestion, typeBook);

        var filter = new SearchFilterDto
        {
            TypeBook = typeBook,
            MinPrice = minPrice,
            MaxPrice = maxPrice,
            Keywords = keywords
        };

        return Task.FromResult(filter);
    }

    private string? DetectTypeBook(string text)
    {
        var lower = text.ToLower();

        if (lower.Contains("truyện tranh")) return "truyện tranh";
        if (lower.Contains("manga")) return "truyện tranh";
        if (lower.Contains("comic")) return "truyện tranh";

        if (lower.Contains("sách giáo khoa")) return "sách giáo khoa";
        if (lower.Contains("sgk")) return "sách giáo khoa";

        if (lower.Contains("tiểu thuyết")) return "tiểu thuyết";

        // Nếu user chỉ nói "truyện" thì có thể map sang truyện tranh (tuỳ shop bạn)
        if (lower.Contains("truyện")) return "truyện tranh";

        return null;
    }

    private decimal? ExtractMaxPrice(string text)
    {
        var lower = text.ToLower();

        // dưới 20k / tối đa 50k / không quá 100000
        var match = Regex.Match(lower, @"(dưới|<=|tối đa|không quá)\s*(\d+)\s*(k|nghìn|ngan)?");
        if (match.Success)
        {
            var value = decimal.Parse(match.Groups[2].Value);
            var unit = match.Groups[3].Value;

            if (unit == "k" || unit == "nghìn" || unit == "ngan")
                value *= 1000;

            return value;
        }

        return null;
    }

    private decimal? ExtractMinPrice(string text)
    {
        var lower = text.ToLower();

        // trên 20k / từ 50k
        var match = Regex.Match(lower, @"(trên|>=|từ|tối thiểu)\s*(\d+)\s*(k|nghìn|ngan)?");
        if (match.Success)
        {
            var value = decimal.Parse(match.Groups[2].Value);
            var unit = match.Groups[3].Value;

            if (unit == "k" || unit == "nghìn" || unit == "ngan")
                value *= 1000;

            return value;
        }

        return null;
    }

    private List<string> ExtractKeywordsSmart(string text, string? typeBook = null)
    {
        var cleaned = text.ToLower();

        // bỏ dấu câu
        cleaned = Regex.Replace(cleaned, @"[^\p{L}\p{N}\s]", " ");

        // bỏ typebook khỏi câu
        if (!string.IsNullOrWhiteSpace(typeBook))
        {
            cleaned = cleaned.Replace(typeBook.ToLower(), " ");
        }

        // bỏ các cụm giá
        cleaned = Regex.Replace(cleaned, @"(dưới|trên|từ|đến|không quá|tối đa|tối thiểu)\s*\d+\s*(k|nghìn|ngan)?", " ");

        var words = cleaned
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Where(w => w.Length > 1)
            .Where(w => !StopWords.Contains(w))
            .Where(w => !GenericWords.Contains(w))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        // ghép 2 từ liên tiếp để bắt "one piece", "nhật bản"
        var twoGrams = new List<string>();
        for (int i = 0; i < words.Count - 1; i++)
        {
            twoGrams.Add($"{words[i]} {words[i + 1]}");
        }

        var allCandidates = words
            .Concat(twoGrams)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        var expanded = new List<string>();

        foreach (var keyword in allCandidates)
        {
            expanded.Add(keyword);

            if (KeywordAliases.TryGetValue(keyword, out var aliases))
            {
                expanded.AddRange(aliases);
            }
        }

        return expanded
            .Where(k => !string.IsNullOrWhiteSpace(k))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
    }
}