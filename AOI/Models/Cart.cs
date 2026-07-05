using System.Collections.Generic;
using System.Linq;
using System.Web.SessionState;

namespace AOI.Models
{
    public static class Cart
    {
        private const string SessionKey = "CartLines";

        public static List<CartLine> GetLines(HttpSessionState session)
        {
            var lines = session[SessionKey] as List<CartLine>;
            if (lines == null)
            {
                lines = new List<CartLine>();
                session[SessionKey] = lines;
            }
            return lines;
        }

        public static void AddItem(HttpSessionState session, int productId, int quantity = 1)
        {
            var lines = GetLines(session);
            var existing = lines.FirstOrDefault(l => l.ProductId == productId);
            if (existing != null)
                existing.Quantity += quantity;
            else
                lines.Add(new CartLine { ProductId = productId, Quantity = quantity });
        }

        public static void UpdateQuantity(HttpSessionState session, int productId, int quantity)
        {
            var lines = GetLines(session);
            var existing = lines.FirstOrDefault(l => l.ProductId == productId);
            if (existing == null) return;

            if (quantity <= 0)
                lines.Remove(existing);
            else
                existing.Quantity = quantity;
        }

        public static void RemoveItem(HttpSessionState session, int productId)
        {
            var lines = GetLines(session);
            lines.RemoveAll(l => l.ProductId == productId);
        }

        public static void Clear(HttpSessionState session)
        {
            session[SessionKey] = new List<CartLine>();
        }

        public static int GetItemCount(HttpSessionState session)
        {
            return GetLines(session).Sum(l => l.Quantity);
        }
    }
}
