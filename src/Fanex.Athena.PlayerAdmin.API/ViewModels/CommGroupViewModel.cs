namespace Fanex.Athena.PlayerAdmin.API.ViewModels
{
    using Core.Resources;

    public class CommGroupViewModel
    {
        public CommGroupViewModel()
        {
            Id = "0";
            Name = ViewResources.None;
        }

        public string Id { get; set; }
        public string Name { get; set; }
    }
}
