using System.Collections.Generic;

namespace Library.Services.Helper
{
    public static class ResumeFilterHelper
    {
        private static readonly HashSet<string> PhrasesNotNames = new HashSet<string>
        {
            "curric", "vitae", "address", "history", "application", "street", "flat",
            "resume", "person", "information", "phone", "mobile", "off", "detail",
            "state", "email", "work", "employ", "experience", "skill", "market",
            "nysc", "referee", "nation", "education", "position", "post",
            "letter", "cv", "apply", "nation", "nigeria", "birth", "profile", 
            "origin", "country", "univers", "school", "class", "society",
            "personal", "activit", "place", "block", "county", "year", "month",
            "expert", "object", "room"
        };

        public static HashSet<string> GetPhrasesNotNames()
        {
            return PhrasesNotNames;
        }


        private static readonly HashSet<string> StopWords = new HashSet<string>{
                "SKILL", "Skill", 
                "EXPERIENCE", "Experience", 
                "EMPLOYMENT", "Employment", 
                "EDUCATION", "Education", 
                "WORK", "Work", 
                "NYSC", "Nysc", "nysc", 
                "REFEREE", "Referee"
            };

        public static HashSet<string> GetStopWords()
        {
            return StopWords;
        }

        private static readonly string[] SkipWords = {
            "Dear", 
            "Sir", "Madam",
            "APPLICATION FOR THE POST", "APPLICATION FOR THE POSITION", "APPLICATION LETTER", 
            "I hereby",
            "apply for the post", "apply for the position",
            "Yours", 
            "Thank You", "Thank you",
            "Faithfully", "faithfully", 
            "Sincerely", "sincerely"
        };

        public static string[] GetSkipWords()
        {
            return SkipWords;
        }

        private static readonly string[] States = {
                "Abia", "Adamawa", "Akwa Ibom", "Anambra", 
                "Bauchi", "Bayelsa", "Benue", "Borno", 
                "Cross River", 
                "Delta", 
                "Ebonyi", "Edo", "Ekiti", "Enugu", 
                "FCT", "Gombe", "Imo", "Jigawa", 
                "Kaduna", "Kano", "Katsina", "Kebbi", "Kogi", "Kwara", 
                "Lagos", "Nasarawa", "Niger", 
                "Ogun", "Ondo", "Osun", "Oyo", 
                "Plateau", 
                "Rivers", 
                "Sokoto", 
                "Taraba ", 
                "Yobe", 
                "Zamfara"
            };
        public static string[] GetStates()
        {
            return States;
        }

        private static readonly string Template = @"
                    <p>Dear Admin,</p>

                    <p>The following application is missing some details:<p>                    

                    <table style='border: 1px solid #000; border-collapse: collapse; border-spacing: 10px;' border=1 cellpadding=10>
                        <thead>
                            <tr style='background-color: #EEE;'>
                                <th>First Name</th>
                                <th>Last Name</th>
                                <th>Age</th>
                                <th>Email</th>
                                <th>Phone</th>
                                <th>State of Origin</th>
                            </tr>
                        </thead>

                        <tbody
                            <tr>
                                <td>{0}</td>
                                <td>{1}</td>
                                <td>{2}</td>
                                <td>{3}</td>
                                <td>{4}</td>
                                <td>{5}</td>
                            </tr>                            
                        </tbody>
                    </table>

                    <p> The following fields need to be updated: {6} </p>
            ";

        public static string GetNotificationTemplate()
        {
            return Template;
        }
    }
}
