using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text.RegularExpressions;
using Library.Core.Bootstrapper;
using Library.Core.Models;
using Library.Services.Helper;

namespace Library.Services.Validation
{
    class PhoneNumberValidator : IResumeValidator<Candidate>
    {
        public PhoneNumberValidator()
        {
            MefDependencyBase.Container.SatisfyImportsOnce(this);
        }
        public void Validate(Candidate candidate, List<string> dataRows, IRegexCompiler regexCompiler)
        {
            //var pattern = @"\+?234[ ]*[7-9]\d{9}|0[7-9]\d{9}";
            var phoneNumberRegex = regexCompiler.Compile(RegexOptionHelper.PhoneNumberRegex);

            foreach (var dataRow in dataRows)
            {
                if (phoneNumberRegex.IsMatch(dataRow))
                {
                    var phoneNumber = Regex.Replace(dataRow, @"[\S ]*(\d{10})", "0$1");
                    if (!string.IsNullOrEmpty(phoneNumber) && phoneNumber.Length == 11)
                    {
                        candidate.PhoneNumber = phoneNumber;
                        break;
                    }

                    var match = phoneNumberRegex.Match(dataRow);
                    var strippedPhoneNumber = Regex.Replace(match.Value, @"[^0-9]", string.Empty);

                    if (!string.IsNullOrEmpty(strippedPhoneNumber))
                    {
                        strippedPhoneNumber = Regex.Replace(strippedPhoneNumber, @"^(2340)", "0");
                        strippedPhoneNumber = Regex.Replace(strippedPhoneNumber, @"^(234)", string.Empty);

                        if (strippedPhoneNumber.Length == 10)
                            strippedPhoneNumber = "0" + strippedPhoneNumber;

                        candidate.PhoneNumber = strippedPhoneNumber;
                        break;
                    }
                }
            }
        }
    }
}
