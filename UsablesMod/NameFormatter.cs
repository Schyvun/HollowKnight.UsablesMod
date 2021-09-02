using System.Text.RegularExpressions;

namespace UsablesMod
{
    class NameFormatter
    {
        public static string AddIdToName(string name, int id)
        {
            return $"{name}_({id})";
        }

        public static int GetIdFromString(string descriptor)
        {
            Match match = Regex.Match(descriptor, @"_\((\d+)\)$");
            if (match.Success)
            {
                return int.Parse(match.Groups[1].Value);
            }

            return -1;
        }
    }
}
