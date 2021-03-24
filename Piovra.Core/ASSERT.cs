using System;

namespace Piovra {
    public class ASSERT {
        public static void True(bool condition, string message = null) {
            if (!condition) {
                throw new Exception(message);
            }
        }
        public static void False(bool condition, string message = null) => True(!condition, message);
    }
}