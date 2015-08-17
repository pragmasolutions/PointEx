using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PointEx.Entities
{
    [MetadataType(typeof(CardMetadata))]
    public partial class Card
    {
        public bool IsActive
        {
            get
            {
                return this.EndDate != null && (this.ExpirationDate != null && this.ExpirationDate > DateTime.Now);
            }
        }
    }

    public class CardMetadata
    {
        
    }
}
