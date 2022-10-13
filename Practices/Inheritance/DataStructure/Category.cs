using System;
using System.Linq;

namespace Inheritance.DataStructure
{
    public class Category : IComparable
    {
        public readonly string Product;
        public readonly MessageType MessageType;
        public readonly MessageTopic MessageTopic;
        
        public Category(string product, MessageType messageType, MessageTopic messageTopic)
        {
            Product = product;
            MessageType = messageType;
            MessageTopic = messageTopic;
        }
        
        public int CompareTo(object obj)
        {
            if (!(obj is Category other))
            {
                return 0;
            }
            
            return (Product, MessageType, MessageTopic)
                .CompareTo((other.Product, other.MessageType, other.MessageTopic));
        }
        
        public override string ToString()
        {
            return Product + '.' + MessageType + '.' + MessageTopic;
        }
        
        public override bool Equals(object obj)
        {
            return obj != null && GetHashCode() == obj.GetHashCode();
        }
        
        public override int GetHashCode()
        {
            return (Product, MessageType, MessageTopic).GetHashCode();
        }
        
        public static bool operator >(Category a, Category b)
        {
            return a.CompareTo(b) > 0;
        }
        
        public static bool operator <(Category a, Category b)
        {
            return a.CompareTo(b) < 0;
        }
        
        public static bool operator >=(Category a, Category b)
        {
            return a.CompareTo(b) >= 0;
        }
        
        public static bool operator <=(Category a, Category b)
        {
            return a.CompareTo(b) <= 0;
        }
    }
}
