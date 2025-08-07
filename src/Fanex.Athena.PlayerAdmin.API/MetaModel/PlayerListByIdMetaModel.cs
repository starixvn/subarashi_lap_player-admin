namespace Fanex.Athena.PlayerAdmin.API.MetaModel
{
    public class PlayerListByIdMetaModel : PlayerListBaseMetaModel
    {
        public string IdList { get; set; }

        public byte BaFilter { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }
    }
}