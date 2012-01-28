using System;

namespace ThreeCardMonte
{
	public class ColorDef : ColorScheme
   {
        public virtual CompanyDarkBlue
        { 
           get { return Color(0,56,147); }
        }

        public virtual CompanyBlue
        {
           get { return Color(0,56,147); }
        }
		
		public virtual CompanyLightBlue
        {
           get { return Color(0,145,201); }
        }
        // etc.
	}
}

