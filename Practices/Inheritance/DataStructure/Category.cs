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
            
            int compareResult = string.Compare(Product, other.Product, StringComparison.InvariantCultureIgnoreCase);
            if (compareResult != 0)
            {
                return compareResult;
            }
            
            compareResult = MessageType.CompareTo(other.MessageType);
            if (compareResult != 0)
            {
                return compareResult;
            }
            
            return MessageTopic.CompareTo(other.MessageTopic);
        }
        
        public override string ToString()
        {
            return Product + '.' + MessageType + '.' + MessageTopic;
        }
        
        public override bool Equals(object obj)
        {
            if (!(obj is Category other))
            {
                return false;
            }
            
            return Product == other.Product
                && MessageType == other.MessageType
                && MessageTopic == other.MessageTopic;
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