using System.Threading.Tasks;
using GuessingGame.Models.GameFlow;

namespace GuessingGameProgram
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var gameFlow = new GameFlow();

            await gameFlow.Run();
        }
    }
}
