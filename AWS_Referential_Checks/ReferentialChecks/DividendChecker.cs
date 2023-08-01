using System;
using System.Collections.Generic;
using System.Text;
using AWS_Referential.Implementation;
using AWS_Referential.Query;
using System.Linq;
using Bloomberg;
using AWS_Referential.Enumeration;

namespace ReferentialChecks
{
    public class DividendChecker
    {
        public List<Dividend> CheckPossibleDuplicateDividends()
        {
            int Tol = 2;
            Query AskDb = new Query();
            List<Dividend> DuplicateDividends = new List<Dividend>();
            var AllDividends = AskDb.GetAlldDividends();
            foreach(var Dividend in AllDividends)
            {
                var PossibleDuplicate = AllDividends.Where(p => p.InstrumentId == Dividend.InstrumentId && p.ExDate >= Dividend.ExDate.AddDays(-Tol) && p.ExDate <= Dividend.ExDate.AddDays(Tol) && p.TaxCode == Dividend.TaxCode && p.ExDate > DateTime.Today &&
                p.DividendType==Dividend.DividendType).ToList();

                if (PossibleDuplicate.Count > 1)
                {
                    DuplicateDividends.AddRange(PossibleDuplicate);
                }
            }
            return DuplicateDividends;
        }
    }
}
