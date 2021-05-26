using System;

namespace M5MouseController.Controller
{
    class MouseController
    {
        public String preval = "";
        public int preival = 0;
        public String premethod = "";
        public int scroll_adjust = 0;

        public string Control(String val)
        {
            string resmsg = "";
            
            if (val.Length < 2) return resmsg;
            if (this.preval == val) return resmsg;

            String method = val.Substring(0, 1);
            String v_val = val.Substring(1);

            int i_val;
            if (!int.TryParse(v_val, out i_val)) return resmsg;

            if (this.premethod != method && method != "L" && method != "R" && method != "M" && this.premethod != "L" && this.premethod != "R" && this.premethod != "M") this.preival = 0;

            if (method == "X")
            {
                NativeController.SendMouseMove((i_val - this.preival), 0);
            }
            else if (method == "Y")
            {
                NativeController.SendMouseMove(0, (i_val - this.preival));
            }
            else if (method == "S")
            {
                // for Scroll behavior
                NativeController.SendMouseWheel((i_val  - this.preival) * this.scroll_adjust);
            }
            else if (method == "U")
            {
                NativeController.SendMouseHWheel((i_val - this.preival));
            }
            else if (method == "V")
            {
                NativeController.SendMouseWheel((i_val - this.preival));
            }
            else if (method == "L")
            {
                if (i_val == 1)
                {
                    NativeController.SendMouseDown();
                }
                else
                {
                    NativeController.SendMouseUp();
                }

            }
            else if (method == "R")
            {
                if (i_val == 1)
                {
                    NativeController.SendMouseDownR();
                }
                else
                {
                    NativeController.SendMouseUpR();
                }

            }
            else if (method == "M")
            {
                if (i_val == 1)
                {
                    NativeController.SendMouseDownM();
                }
                else
                {
                    NativeController.SendMouseUpM();
                }

            }


            this.premethod = method;
            if (method != "L" && method != "R")
            {
                this.preival = i_val;
            }
            this.preval = val;

            return "";
        }
    }

}