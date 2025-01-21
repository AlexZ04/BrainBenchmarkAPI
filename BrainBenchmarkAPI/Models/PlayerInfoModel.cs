namespace BrainBenchmarkAPI.Models
{
    public class PlayerInfoModel
    {
        public UserModel MainInfo { get; set; }
        
        public PlayerInfoModel(UserModel info) 
        { 
            MainInfo = info;
        }
    }
}
