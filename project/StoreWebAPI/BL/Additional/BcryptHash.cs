namespace ClothingStore.Service.Additional {
    public class BcryptHash {
        private const string HARD_CODED_SALT = "&N*3k#";

        public static string GenerateBcryptHash(string input) {
            var passwordWithHardcodedSalt = input + HARD_CODED_SALT;
            return BCrypt.Net.BCrypt.HashPassword(passwordWithHardcodedSalt, BCrypt.Net.BCrypt.GenerateSalt());
        }

        public static bool CheckBcryptPassword(string password, string hash) {
            return BCrypt.Net.BCrypt.Verify(password + HARD_CODED_SALT, hash);
        }
    }
}
