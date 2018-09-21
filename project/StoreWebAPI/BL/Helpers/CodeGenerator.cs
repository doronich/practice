using System;
using System.Collections.Generic;
using System.Linq;
using ClothingStore.Service.Settings;

namespace ClothingStore.Service.Helpers {
    public class CodeGenerator<T> where T : BaseGeneratorSettings {
        private readonly string[] m_randomChars = {
            "ABCDEFGHJKLMNOPQRSTUVWXYZ", // uppercase 
            "abcdefghijkmnopqrstuvwxyz", // lowercase
            "0123456789", // digits
            "!@$?_-" // non-alphanumeric
        };

        public T Settings { get; set; }

        public CodeGenerator(T settings) {
            this.Settings = settings;
        }

        public string Generate() {
            var rand = new Random(Environment.TickCount);
            var chars = new List<char>();
            var mas =new List<int>() { 0, 1, 2, 3 };
            if(this.Settings.RequireUppercase)
                chars.Insert(rand.Next(0, chars.Count),
                    this.m_randomChars[0][rand.Next(0, this.m_randomChars[0].Length)]);
            else mas.Remove(0);

            if(this.Settings.RequireLowercase)
                chars.Insert(rand.Next(0, chars.Count),
                    this.m_randomChars[1][rand.Next(0, this.m_randomChars[1].Length)]);
            else mas.Remove(1);

            if (this.Settings.RequireDigit)
                chars.Insert(rand.Next(0, chars.Count),
                    this.m_randomChars[2][rand.Next(0, this.m_randomChars[2].Length)]);
            else mas.Remove(2);

            if (this.Settings.RequireNonAlphanumeric)
                chars.Insert(rand.Next(0, chars.Count),
                    this.m_randomChars[3][rand.Next(0, this.m_randomChars[3].Length)]);
            else mas.Remove(3);

            for (var i = chars.Count;
                i < this.Settings.RequiredLength
                || chars.Distinct().Count() < this.Settings.RequiredUniqueChars;
                i++) {

                var rcs = this.m_randomChars[mas[rand.Next(0, mas.Count)]];
                chars.Insert(rand.Next(0, chars.Count),
                    rcs[rand.Next(0, rcs.Length)]);
            }
            var result = new string(chars.ToArray());
            return result;
        }
    }
}
