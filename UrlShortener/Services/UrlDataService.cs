using UrlShortener.Data;
using UrlShortener.Models;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;

namespace UrlShortener.Services
{
    public class UrlDataService
    {
        private readonly DataContext _data;
        public UrlDataService(DataContext data)
        {
            _data = data;
        }
        public UrlPair? GetUrlLongToPair(string longUrl)
        {
            var urlPair = _data.urlPairs.Include(x => x.Entries)
                             .FirstOrDefault(x => x.LongUrl == longUrl);

            if (longUrl is null || urlPair == null)
                return null;
            return urlPair;
        }
        public UrlPair? GetUrlShortToPair(string shortUrl)
        {
            var urlPair = _data.urlPairs.FirstOrDefault(x => x.ShortUrl == shortUrl);
            if (shortUrl is null || urlPair == null)
                return null;
            return urlPair;
        }
        public UrlPair Create(string longUrl, string? userName)
        {
            var randomS = randomString();
            var newUrlPair = new UrlPair 
            { 
                LongUrl = longUrl, 
                ShortUrl = randomS, 
                User = userName 
            };
            _data.urlPairs.Add(newUrlPair);
            _data.SaveChanges();
            return newUrlPair;
        }

        public IEnumerable<UrlPair> GetAllUrlPairForUser(string userName)
        {
            var urlList = _data.urlPairs
                .Where(u => u.User == userName)
                .Include(u => u.Entries)
                .ToList();
            return urlList;
        }

        public void AddEntry(UrlPair urlPair, string ip)
        {
            Entry entry = new Entry { Ip = ip };
            _data.urlPairs.First(u => u == urlPair).Entries.Add(entry);
            _data.SaveChanges();
        }

        public async Task AddEntryAsync(UrlPair urlPair, string ip)
        {
            var matchingUrlPair = await _data.urlPairs.FindAsync(urlPair.LongUrl);
            if (matchingUrlPair != null)
            {
                var newEntry = new Entry { Ip = ip };
                matchingUrlPair.Entries.Add(newEntry);
                await _data.SaveChangesAsync();
            }
        }

        public void AddMessage(ContactModel cm)
        {
            _data.messages.Add(cm);
            _data.SaveChanges();
        }

        public UrlPair cleanLongUrl(UrlPair urlPair)
        {
            if (urlPair.LongUrl.Contains("https://www."))
                urlPair.LongUrl = urlPair.LongUrl.Replace("https://www.", "");
            else if (urlPair.LongUrl.Contains("www."))
                urlPair.LongUrl = urlPair.LongUrl.Replace("www.", "");
            return urlPair;
        }

        public string randomString()
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 5)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
