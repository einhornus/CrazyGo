
using System.Collections.Generic; using System;

using System.Text;

namespace BrainDuelsLib.model.entities
{
    public class Country
    {
        int id;

        private static String[] titles = {
            "Unknown", "Afghanistan", "Albania", "Algeria", "American-Samoa", "Andorra", "Angola", "Anguilla", "Antigua-and-Barbuda", "Argentina", "Armenia", "Aruba", "Australia", "Austria", "Azerbaijan", "Bahamas", "Bahrain", "Bangladesh", "Barbados", "Belarus", "Belgium", "Belize", "Benin", "Bermuda", "Bhutan", "Bolivia", "Bosnia-and-Herzegovina", "Botswana", "Brazil", "British-Virgin-Islands", "Brunei", "Bulgaria", "Burkina-Faso", "Burundi", "Cambodia", "Cameroon", "Canada", "Canary-Islands", "Cape-Verde", "Cayman-Islands", "Central-African-Republic", "Chad", "Chile", "China", "Colombia", "Comoros", "Costa-Rica", "Cote-dIvoire", "Croatia", "Cuba", "Curacao", "Cyprus", "Czech-Republic", "Democratic-Republic-of-the-Congo", "Denmark", "Djibouti", "Dominica", "Dominican-Republic", "East-Timor", "Ecuador", "Egypt", "El-Salvador", "England", "Equatorial-Guinea", "Eritrea", "Estonia", "Ethiopia", "Fiji", "Finland", "France", "Gabon", "Gambia", "Georgia", "Germany", "Ghana", "Greece", "Grenada", "Guam", "Guatemala", "Guernsey", "Guinea-Bissau", "Guinea", "Guyana", "Honduras", "Hong-Kong", "Hungary", "Iceland", "India", "Indonesia", "Iran", "Iraq", "Ireland", "Israel", "Italy", "Jamaica", "Japan", "Jordan", "Kazakhstan", "Kenya", "Kiribati", "Kosovo", "Kuwait", "Kyrgyzstan", "Laos", "Latvia", "Lebanon", "Lesotho", "Liberia", "Libya", "Liechtenstein", "Lithuania", "Luxembourg", "Macedonia", "Madagascar", "Malawi", "Malaysia", "Maldives", "Mali", "Malta", "Marshall-Islands", "Martinique", "Mauritania", "Mauritius", "Mexico", "Micronesia", "Moldova", "Monaco", "Mongolia", "Montenegro", "Morocco", "Mozambique", "Myanmar", "Namibia", "Nauru", "Nepal", "Netherlands-Antilles", "Netherlands", "New-Caledonia", "New-Zealand", "Nicaragua", "Niger", "Nigeria", "Norfolk-Island", "North-Korea", "Northern-Cyprus", "Northern-Mariana-Islands", "Norway", "Oman", "Pakistan", "Palestine", "Panama", "Papua-New-Guinea", "Paraguay", "Peru", "Philippines", "Poland", "Portugal", "Puerto-Rico", "Qatar", "Republic-of-the-Congo", "Romania", "Russia", "Rwanda", "Saint-Barthelemy", "Saint-Helena", "Saint-Kitts-and-Nevis", "Saint-Lucia", "Saint-Martin", "Saint-Vincent-and-the-Grenadines", "Samoa", "San-Marino", "Sao-Tome-and-Principe", "Saudi-Arabia", "Scotland", "Senegal", "Serbia", "Seychelles", "Sierra-Leone", "Singapore", "Slovakia", "Slovenia", "Solomon-Islands", "Somalia", "Somaliland", "South-Africa", "South-Korea", "South-Sudan", "Spain", "Sri-Lanka", "Sudan", "Suriname", "Swaziland", "Sweden", "Switzerland", "Syria", "Taiwan", "Tajikistan", "Tanzania", "Thailand", "Togo", "Tokelau", "Tonga", "Trinidad-and-Tobago", "Tunisia", "Turkey", "Turkmenistan", "Tuvalu", "Uganda", "Ukraine", "United-Arab-Emirates", "United-Kingdom", "United-States", "Uruguay", "US-Virgin-Islands", "Uzbekistan", "Vanuatu", "Vatican-City", "Venezuela", "Vietnam", "Wales", "Wallis-And-Futuna", "Yemen", "Zambia", "Zimbabwe"
        };

        static Dictionary<String, int> reversedHash = new Dictionary<String, int>();

        static bool initExecuted = false;
        internal static void Init()
        {
            for (int i = 0; i < titles.Length; i++)
            {
                reversedHash.Add(titles[i], i);
            }
            initExecuted = true;
        }

        public Country(int id)
        {
            if(!initExecuted){
                Init();
            }
            this.id = id;
        }

        public Country(String title)
        {
            if (!initExecuted)
            {
                Init();
            }
            this.id = reversedHash[title];
        }

        public static String[] GetAllTitles()
        {
            return titles;
        }

        public static Country GetUnknownCountry()
        {
            return new Country("Unknown");
        }

        public String GetTitle()
        {
            return titles[id];
        }

        public int GetId()
        {
            return id;
        }
    }
}
