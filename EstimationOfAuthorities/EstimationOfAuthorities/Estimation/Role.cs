using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimationOfAuthorities.Estimation
{
    /// <summary>
    /// Rola pracownika
    /// </summary>

    public class Role
    {
        #region Properties
        /// <summary>
        /// Nazwa roli
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Priorytet roli
        /// </summary>
        public int Priority { get; set; }
        #endregion

        #region Constructors
        public Role(string name, int prior) {
            Name = name;
            Priority = prior;
        }
        public Role() { }
        #endregion
    }
}
