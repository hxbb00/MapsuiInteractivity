﻿using Mapsui.Interactivity.UI.Input;

namespace Mapsui.Interactivity.UI
{
    public class EditController : ControllerBase, IMapController
    {
        public EditController()
        {
            this.BindMouseDown(MouseButton.Left, MapCommands.Editing);
            this.BindMouseEnter(MapCommands.HoverEditing);
        }
    }
}
