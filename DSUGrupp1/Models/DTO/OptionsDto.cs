namespace DSUGrupp1.Models.DTO
{
    public class OptionsDto
    {
        public OptionsDto(string type)
        {
            Type = type;
            Plugins = new PluginsDto();

            if (Type != "pie")
            {

                Scales = new ScalesDto
                {
                    X = new AxisDto
                    {
                        Ticks = new TicksDto
                        {
                            Font = new FontDto
                            {
                                Weight = "900",
                                Color = "black",
                                
                            }
                        }
                    },
                    Y = new AxisDto
                    {
                        Ticks = new TicksDto
                        {
                            Font = new FontDto
                            {
                                Weight = "900",
                                Color = "black",
                                
                            }
                        }
                    }
                };

            }
        }

        public string Type { get; set; }
        public PluginsDto Plugins { get; set; }
        public bool Responsive { get; set; } 
        public bool MaintainAspectRatio { get; set; }
        public ScalesDto Scales { get;  set; }
    }   
}
