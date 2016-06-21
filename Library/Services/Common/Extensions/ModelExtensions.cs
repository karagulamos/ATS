using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Library.Models;

namespace Library.Services.Common.Extensions
{
    public static class ModelExtensions
    {
        public static bool IsValidCandidate(this Candidate candidate)
        {
            return !string.IsNullOrEmpty(candidate.Email) && !string.IsNullOrEmpty(candidate.PhoneNumber) || !HasAllEmptyProperties(candidate);
        }

        public static bool IsNullOrEmpty(this object obj)
        {
            return obj == null || String.IsNullOrWhiteSpace(obj.ToString()) || obj.ToString() == "0";
        }

        public static bool HasAllEmptyProperties(Candidate candidate)
        {
            var type = typeof(Candidate);
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(prop => !IgnoredProperties.Contains(prop.Name));
            var hasProperty = properties.Any(x => x.GetValue(candidate, null).IsNullOrEmpty());
            return !hasProperty;
        }

        public static List<string> GetIncompleteCandidateDetails(this Candidate candidate)
        {
            var type = typeof(Candidate);
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(prop => !IgnoredProperties.Contains(prop.Name));
            var nullProperties = properties.Where(prop => prop.GetValue(candidate, null).IsNullOrEmpty())
                                           .Select(property => property.Name); 

            return nullProperties.ToList();
        }

        private static readonly HashSet<string> IgnoredProperties = new HashSet<string>
        {
            "CandidateId", "InboundEmail", "InboundEmailId", "InboundAttachment", "InboundAttachmentId", "Age"
        };
    }
}
