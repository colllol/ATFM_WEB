using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Saplin.Controls
{
    [ParseChildren(true)]
    [PersistChildren(false)]
    [ToolboxData("<{0}:DropDownCheckBoxes runat=\"server\"></{0}:DropDownCheckBoxes>")]
    public class DropDownCheckBoxes : CheckBoxList
    {
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public DropDownCheckBoxesStyle Style { get; } = new DropDownCheckBoxesStyle();

        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public DropDownCheckBoxesTexts Texts { get; } = new DropDownCheckBoxesTexts();

        [DefaultValue(false)]
        public bool AddJQueryReference { get; set; }

        [DefaultValue(false)]
        public bool UseButtons { get; set; }

        [DefaultValue(false)]
        public bool UseSelectAllNode { get; set; }
    }

    public class DropDownCheckBoxesStyle
    {
        public Unit SelectBoxWidth { get; set; }

        public Unit DropDownBoxBoxWidth { get; set; }

        public Unit DropDownBoxBoxHeight { get; set; }
    }

    public class DropDownCheckBoxesTexts
    {
        public string SelectBoxCaption { get; set; }
    }

    public class ExtendedRequiredFieldValidator : RequiredFieldValidator
    {
        protected override bool ControlPropertiesValid()
        {
            return true;
        }

        protected override bool EvaluateIsValid()
        {
            Control control = NamingContainer.FindControl(ControlToValidate);
            var list = control as ListControl;
            if (list == null)
            {
                return base.EvaluateIsValid();
            }

            foreach (ListItem item in list.Items)
            {
                if (item.Selected)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
