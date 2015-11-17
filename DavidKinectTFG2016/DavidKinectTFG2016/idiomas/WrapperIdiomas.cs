using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Resources;
using System.Reflection;

namespace DavidKinectTFG2016.idiomas
{
    class WrapperIdiomas
    {
        private static ObjectDataProvider m_provider;

        public WrapperIdiomas()
        {
        }

        public idiomas GetResourceInstance()
        {
            return new idiomas();
        }

        public static ObjectDataProvider ResourceProvider
        {
            get
            {
                if (m_provider == null)
                    m_provider = (ObjectDataProvider)App.Current.FindResource("IdiomasRes");
                return m_provider;
            }
        }

        public static void ChangeCulture(CultureInfo culture)
        {
            Properties.Resources.Culture = culture;
            ResourceProvider.Refresh();
        }
    }
}
