using System.Collections;
using System.Collections.Generic;
using Model.Rules;
using System.Linq;

namespace Model {
    public sealed class EraseField {
        private int[][] fields = null;

        public EraseField(int[][] fields) {
            fields.CopyTo(this.fields, 0);
        }

    }
}
