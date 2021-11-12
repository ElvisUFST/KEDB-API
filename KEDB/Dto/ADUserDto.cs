using System;

namespace KEDB.Dto
{
    public class ADUserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public static ADUserDto Parse(string brugerData)
        {
            if (brugerData == null) { return null; }

            var elements = brugerData.Split("|", 2, StringSplitOptions.RemoveEmptyEntries);

            if (elements.Length == 2 && Guid.TryParse(elements[0], out Guid id))
            {
                return new ADUserDto
                {
                    Id = id,
                    Name = elements[1]
                };
            }

            throw new FormatException($"Kan ikke parse '{brugerData}' som en {nameof(ADUserDto)}.");
        }

        public override string ToString()
        {
            return $"{this.Id.ToString("N")}|{this.Name}";
        }
    }
}
